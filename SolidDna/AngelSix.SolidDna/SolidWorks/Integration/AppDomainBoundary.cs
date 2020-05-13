using Dna;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static Dna.FrameworkDI;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Helper class used to bridge the gap between app domains
    /// </summary>
    public class AppDomainBoundary
    {
        #region Protected Members

        /// <summary>
        /// A list of assemblies to use when resolving any missing references
        /// </summary>
        protected static List<AssemblyName> mReferencedAssemblies = new List<AssemblyName>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the list of all known reference assemblies in this solution
        /// </summary>
        public AssemblyName[] ReferencedAssemblies => mReferencedAssemblies.ToArray();

        /// <summary>
        /// The AppDomain used to load and unload plug-ins
        /// </summary>
        public static AppDomain AppDomain { get; private set; }

        /// <summary>
        /// If true, will load your Add-in dll in its own application domain so you can 
        /// unload and rebuild your add-in without having to close SolidWorks
        /// NOTE: This does seem to expose some bugs and issues in SolidWorks API
        ///       in terms of resolving references to specific dll's, so if you experience
        ///       issues try turning this off
        /// NOTE: Also no IoC is available in this detached domain at the moment
        ///       due to AddinIntegration non-static instance intializing the IoC.
        ///       That means no Logger for example, so safe log with Logger?.
        /// </summary>
        public static bool UseDetachedAppDomain { get; set; }

        /// <summary>
        /// The cross-domain marshal to use for the cross-Application domain calls
        /// </summary>
        public static AppDomainBoundaryMarshal Marshal { get; private set; }

        #endregion

        #region App Domain Setup

        /// <summary>
        /// Sets up the application, allowing for cross-app domain setup if required
        /// </summary>
        /// <param name="assemblyPath">Working directory path for the application for the app domain (usually the working directory of add-in dll)</param>
        /// <param name="assemblyFilePath">The path to the assembly</param>
        /// <param name="configureDllPath">Path to the dll that contains the custom configure services method</param>
        /// <param name="configureName">The name of the <see cref="ConfigureServiceAttribute"/> method to use</param>
        public static void Setup(string assemblyPath, string assemblyFilePath, string configureDllPath, string configureName)
        {
            // Help resolve any assembly references
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            // Add references from this assembly (AngelSix.SolidDna) including itself
            // to be resolved by the assembly resolver
            AddReferenceAssemblies<AddInIntegration>(includeSelf: true);

            // If we want a separate app domain...
            if (UseDetachedAppDomain)
            {
                // Create random number at end to allow for multiple add-ins
                var random = new Random();
                var randomName = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                          .Select(s => s[random.Next(s.Length)]).ToArray());

                AppDomain = AppDomain.CreateDomain($"SolidDnaAppDomain-{randomName}", null, new AppDomainSetup
                {
                    // Use given folder for resolving references
                    ApplicationBase = assemblyPath,
                });

                // Run code on new app-domain to configure
                Marshal = (AppDomainBoundaryMarshal)AppDomain.CreateInstanceAndUnwrap(typeof(AppDomainBoundaryMarshal).Assembly.FullName, typeof(AppDomainBoundaryMarshal).FullName);

                // Setup IoC
                Marshal.SetupIoC(assemblyFilePath, configureDllPath, configureName);
            }

            // Always setup IoC on the normal app domain
            // This is so both sides of the application code can
            // so access the dependencies (like loggers)
            // even though they may be different instances
            SetupIoC(assemblyFilePath, configureDllPath, configureName);
        }

        #region IoC

        /// <summary>
        /// Configures the application IoC
        /// </summary>
        /// <param name="assemblyFilePath">The path to the assembly</param>
        /// <param name="pathToConfigureDll">Absolute path to dll where the IConfigureServices implementation lies</param>
        /// <param name="configureName">The name of the <see cref="ConfigureServiceAttribute"/> method to use</param>
        public static void SetupIoC(string assemblyFilePath, string pathToConfigureDll = null, string configureName = null)
        {
            // Create default construction
            Framework.Construct(new DefaultFrameworkConstruction(configure =>
            {
                // If the add-in path is not null
                if (!string.IsNullOrEmpty(assemblyFilePath))
                    // Add configuration file for the name of this file
                    // For example if it is MyAddin.dll then the configuration file
                    // will be in the same folder called MyAddin.appsettings.json"
                    configure.AddJsonFile(Path.ChangeExtension(assemblyFilePath, "appsettings.json"), optional: true);
            }));

            // If we have a dll to try and find the configure method from...
            if (File.Exists(pathToConfigureDll))
            {
                // AddIn class type
                var addinType = typeof(AddInIntegration);

                // Load all methods...
                var match = Assembly.LoadFile(pathToConfigureDll).GetTypes()
                    // Find AddInIntegration parent class
                    .Where(f => f.IsSubclassOf(addinType))
                      .Select(t => (
                        // Store class
                        methodClass: t,
                        // Select the ConfigureServices method
                        method: t.GetMethod(nameof(AddInIntegration.ConfigureServices)))
                      )
                      // Only use first method for now
                      .FirstOrDefault();

                if (match.method != null)
                {
                    var classInstance = Activator.CreateInstance(match.methodClass, null);
                    match.method.Invoke(classInstance, new[] { Framework.Construction });
                }
            }

            // Build DI
            Framework.Construction.Build();

            // Log details
            Logger?.LogDebugSource($"DI Setup complete");
            Logger?.LogDebugSource($"Assembly File Path {assemblyFilePath}");
        }

        #endregion


        /// <summary>
        /// Unloads the created app domain
        /// </summary>
        public static void Unload()
        {
            // Now tear down app domain
            Logger?.LogDebugSource($"Unloading cross-domain...");

            if (AppDomain != null)
                // NOTE: If you get 'Error while unloading appdomain. (Exception from HRESULT: 0x80131015)'
                //       here, make sure you have no threads hanging that wait forever, and make sure
                //       you called AppContext.SetSwitch("Switch.System.Windows.Input.Stylus.DisableStylusAndTouchSupport", true);
                //       if loading a WPF control (done for you in TaskpaneIntegration.AddToTaskpaneAsync
                AppDomain.Unload(AppDomain);
        }

        #endregion

        #region Plugin Integration

        /// <summary>
        /// Must be called to setup the PlugInIntegration
        /// </summary>
        /// <param name="addinPath">The path to the add-in that is calling this setup (typically acquired using GetType().Assembly.Location)</param>
        /// <param name="cookie">The cookie Id of the SolidWorks instance</param>
        /// <param name="version">The version of the currently connected SolidWorks instance</param>
        public static void PluginIntegrationSetup(string addinPath, string version, int cookie)
        {
            // Call method from other app domain
            Marshal.PluginIntegrationSetup(addinPath, version, cookie);
        }

        /// <summary>
        /// Tears down the app-domain that the plug-ins run inside of
        /// </summary>
        public static void PluginIntegrationTeardown()
        {
            // Call method from other app domain
            Marshal.PluginIntegrationTeardown();
        }

        /// <summary>
        /// Adds a plug-in based on its <see cref="SolidPlugIn"/> implementation
        /// </summary>
        /// <typeparam name="T">The class that implements the <see cref="SolidPlugIn"/></typeparam>
        public static void AddPlugIn<T>()
        {
            Marshal.AddPlugIn<T>();
        }

        /// <summary>
        /// Adds a plug-in based on its <see cref="SolidPlugIn"/> implementation
        /// </summary>
        /// <param name="fullPath">The absolute path to the plug-in dll</param>
        public static void AddPlugIn(string fullPath)
        {
            Marshal.AddPlugIn(fullPath);
        }

        /// <summary>
        /// Called by the SolidWorks domain (AddInIntegration) when a callback is fired
        /// </summary>
        /// <param name="name">The parameter passed into the generic callback</param>
        public static void OnCallback(string name)
        {
            Marshal.OnCallback(name);
        }

        /// <summary>
        /// Runs any initialization code required on plug-ins
        /// </summary>
        /// <param name="addinPath">The path to the add-in that is calling this setup (typically acquired using GetType().Assembly.Location)</param>
        public static void ConfigurePlugIns(string addinPath)
        {
            Marshal.ConfigurePlugIns(addinPath);
        }

        #endregion

        #region Connect / Disconnect To SolidWorks

        /// <summary>
        /// Called when the add-in has connected to SolidWorks
        /// </summary>
        public static void ConnectedToSolidWorks()
        {
            Marshal.ConnectedToSolidWorks();
        }

        /// <summary>
        /// Called when the add-in has disconnected from SolidWorks
        /// </summary>
        public static void DisconnectedFromSolidWorks()
        {
            Marshal.DisconnectedFromSolidWorks();
        }

        #endregion

        #region Assembly Resolve Methods

        /// <summary>
        /// Adds any reference assemblies to the assemblies that get resolved when loading assemblies
        /// based on the reference type. To add all references from a project, pass in any type that is
        /// contained in the project as the reference type
        /// </summary>
        /// <typeparam name="ReferenceType">The type contained in the assembly where the references are</typeparam>
        /// <param name="includeSelf">True to include the calling assembly</param>
        public static void AddReferenceAssemblies<ReferenceType>(bool includeSelf = false)
        {
            // Find all reference assemblies from the type
            var referencedAssemblies = typeof(ReferenceType).Assembly.GetReferencedAssemblies();

            // If there are any references
            if (referencedAssemblies?.Length > 0)
                // Add them
                mReferencedAssemblies.AddRange(referencedAssemblies);

            // If we should including calling assembly...
            if (includeSelf)
                // Add self
                mReferencedAssemblies.Add(typeof(ReferenceType).Assembly.GetName());
        }

        /// <summary>
        /// Attempts to resolve missing assemblies based on a list of known references
        /// primarily from SolidDna and the Add-in project itself
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Try and find a reference assembly that matches...
            var resolvedAssembly = mReferencedAssemblies.FirstOrDefault(f => string.Equals(f.FullName, args.Name, StringComparison.InvariantCultureIgnoreCase));

            // If we didn't find any assembly
            if (resolvedAssembly == null)
                // Return null
                return null;

            // If we found a match...
            try
            {
                // Try and load the assembly
                var assembly = Assembly.Load(resolvedAssembly.Name);

                // If it loaded...
                if (assembly != null)
                    // Return it
                    return assembly;

                // Otherwise, throw file not found
                throw new FileNotFoundException();
            }
            catch
            {
                //
                // Try to load by filename - split out the filename of the full assembly name
                // and append the base path of the original assembly (i.e. look in the same directory)
                //
                // NOTE: this doesn't account for special search paths but then that never
                //       worked before either
                //
                var parts = resolvedAssembly.Name.Split(',');
                var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + parts[0].Trim() + ".dll";

                // Try and load assembly and let it throw FileNotFound if not there 
                // as it's an expected failure if not found
                return Assembly.LoadFrom(filePath);
            }
        }

        #endregion
    }
}
