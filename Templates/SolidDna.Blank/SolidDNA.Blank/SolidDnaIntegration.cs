using AngelSix.SolidDna;
using Dna;
using System.IO;
using static AngelSix.SolidDna.SolidWorksEnvironment;

namespace SolidDNA.Blank
{
    // 
    //  *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //
    //     Welcome to SolidDNA by AngelSix
    //
    //        SolidDNA is a modern framework designed to make developing SolidWorks Add-ins easy.
    //
    //        With this template you have a ready-to-go add-in that will load inside of SolidWorks
    //        and a bunch of useful example projects available here 
    //        https://github.com/angelsix/solidworks-api/tree/develop/Tutorials
    //
    //
    //     Registering Add-in Dll
    //
    //        To get your dll to run inside SolidWorks as an add-in you need to register it.
    //        Inside this project template in the Resources folder is the SolidWorksAddinInstaller.exe.
    //        Compile your project, open up the SolidWorksAddinInstaller.exe, then browse for your
    //        output dll file (for example /bin/Debug/SolidDNA.Blank.dll) and click Install.
    //
    //        Now when you start SolidWorks your Add-in should load and should appear in the 
    //        Tools > Add-ins menu. 
    //
    //        NOTE: You only need to register your add-in once, or when you move the location or 
    //              change the filename.
    //        
    //
    //     Debugging Code
    //
    //        In order to press F5 to start up SolidWorks and instantly begin debugging your code,
    //        open up Project Properties, go to Debug, select Start External Program, and point
    //        to `C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\SLDWORKS.exe` by default.
    //        If your install is in a different location just change this path.
    //
    //        Also the Project `Properties > Application > Assembly Information` has the
    //        `Make Assembly COM Visible` checked.
    //
    //
    //     Startup Flow
    //
    //        When your SolidDna add-in first loads, SolidWorks will call the ConnectToSW method
    //        inside your AddInIntegration class. 
    //
    //        This method will fire the following methods in this order:
    // 
    //         - ConfigureServices
    //         - PreConnectToSolidWorks
    //         - PreLoadPlugIns
    //         - ApplicationStartup
    //         - ConnectedToSolidWorks
    //        
    //        Once your add-in is unloaded by SolidWorks the DisconnectFromSW method will be called
    //        which will in turn fire the following methods:
    //
    //         - DisconnectedFromSolidWorks
    //
    //  *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    //

    /// <summary>
    /// Register as a SolidWorks Add-In
    /// </summary>
    public class MyAddinIntegration : AddInIntegration
    {
        /// <summary>
        /// Specific application start-up code
        /// </summary>
        public override void ApplicationStartup()
        {

        }

        /// <summary>
        /// Called when Dependency Injection is being setup
        /// </summary>
        /// <param name="construction">The framework construction</param>
        public override void ConfigureServices(FrameworkConstruction construction)
        {
            //
            //  Example
            // ---------
            //
            //   Add a service like this (include using Microsoft.Extensions.DependencyInjection):
            //
            //      construction.Services.AddSingleton(new SomeClass());
            //
            //   Retrieve the service anywhere in your application like this
            //
            //      Dna.Framework.Service<SomeClass>();
            //

            // Add file logger (will be in /bin/Debug/SolidDNA.Blank.log.txt)
            construction.AddFileLogger(Path.ChangeExtension(this.AssemblyFilePath(), "log.txt"));
        }

        /// <summary>
        /// Use this to do early initialization and any configuration of the 
        /// PlugInIntegration class properties such as <see cref="PlugInIntegration.UseDetachedAppDomain"/>
        /// </summary>
        public override void PreConnectToSolidWorks()
        {

        }

        /// <summary>
        /// Steps to take before any plug-in loads
        /// </summary>
        public override void PreLoadPlugIns()
        {

        }
    }

    /// <summary>
    /// Registers as a SolidDna PlugIn to be loaded by our AddIn Integration class 
    /// when the SolidWorks add-in gets loaded.
    /// 
    /// NOTE: We can have multiple plug-ins in a single add-in
    /// </summary>
    public class MySolidDnaPlugIn : SolidPlugIn<MySolidDnaPlugIn>
    {
        #region Region Public Properties

        /// <summary>
        /// My Add-in title
        /// </summary>
        public override string AddInTitle => "My AddIn Title";

        /// <summary>
        /// My Add-in description
        /// </summary>
        public override string AddInDescription => "My AddIn Description";

        #endregion

        #region Connect To SolidWorks

        public override void ConnectedToSolidWorks()
        {
            Application.ShowMessageBox("Our first SolidDNA add-in... how easy was that? :)", SolidWorksMessageBoxIcon.Information);

            // In here you could now create and add a Taskpane using TaskpaneIntegration
            // https://github.com/angelsix/solidworks-api/tree/develop/Tutorials/02-WpfAddIn
        }

        public override void DisconnectedFromSolidWorks()
        {

        }

        #endregion
    }
}
