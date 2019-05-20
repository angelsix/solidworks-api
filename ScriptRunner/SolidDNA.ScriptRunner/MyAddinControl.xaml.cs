using AngelSix.SolidDna;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using static AngelSix.SolidDna.SolidWorksEnvironment;

namespace SolidDNA.ScriptRunner
{
    /// <summary>
    /// Interaction logic for MyAddinControl.xaml
    /// </summary>
    public partial class MyAddinControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MyAddinControl()
        {
            InitializeComponent();

            // Set example script direct code
            DirectEntry.Text = File.ReadAllText(Path.Combine(Path.GetDirectoryName(typeof(MyAddinControl).Assembly.Location), "ScriptDirectExample.cs"));
        }

        #endregion

        #region UI Events

        /// <summary>
        /// When the Run button is clicked on the Direct tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirectEntry_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Run the code
            RunScript(DirectEntry.Text);
        }

        /// <summary>
        /// When the Run button is clicked on the File tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RunFile_ClickAsync(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var scriptText = string.Empty;

                // For now just read file direct from users desktop called script
                var scriptLocation = FileLocation.Text;

                // If script file not found
                if (!File.Exists(scriptLocation))
                {
                    // Let user know
                    Application.ShowMessageBox($"No script file found '{scriptLocation}'");
                    return;
                }

                // Read script file contents
                using (var fileReader = new StreamReader(File.Open(scriptLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                    // Store results
                    scriptText = await fileReader.ReadToEndAsync();

                // Run the script
                RunScript(scriptText);
            }
            catch (Exception ex)
            {
                // Show error to user
                Application.ShowMessageBox($"Unexpected error running SolidDna script. {ex.ToString()}");
            }
        }

        private void BrowseFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog
            {
                Filter = "C# Script (*.cs)|*.cs",
                Title = "Select C# script",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            })
            {
                // If they didn't select a file
                if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    // Return
                    return;

                // Set filename
                FileLocation.Text = openFileDialog.FileName;
            }
        }

        #endregion

        /// <summary>
        /// Runs the given script
        /// </summary>
        /// <param name="scriptText">The script to run</param>
        private void RunScript(string scriptText)
        {
            try
            {
                // Prepend basic wrapper if this is not a full C# class file
                scriptText = PrependScriptWrapper(scriptText);
                var references = GetAssemblyReferences();

                // Create a compilation task ready to compile the code
                var compilation = CSharpCompilation.Create(
                    // Use a random assembly name
                    Path.GetRandomFileName(),
                    // Add the code to parse
                    syntaxTrees: new[] { CSharpSyntaxTree.ParseText(scriptText) },
                    // Add the references
                    references: references,
                    // And make it a dynamically linked library
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                // Create a memory stream to load the dynamic assembly in to
                using (var ms = new MemoryStream())
                {
                    // Compile the code
                    var result = compilation.Emit(ms);

                    // If it failed to compile...
                    if (!result.Success)
                    {
                        // For each error...
                        foreach (var error in result.Diagnostics.Where(f => f.IsWarningAsError || f.Severity == DiagnosticSeverity.Error))
                            // Show the error to the user
                            Application.ShowMessageBox($"{error.Id}: {error.GetMessage()}. \n\n{error.Location?.SourceTree}");
                    }
                    // Otherwise...
                    else
                    {
                        // Compile worked, so go to beginning of memory stream
                        ms.Seek(0, SeekOrigin.Begin);

                        // Create an assembly from the bytes
                        var assembly = Assembly.Load(ms.ToArray());

                        // Try and get the class SolidDnaScript from the assembly
                        var type = assembly.GetType("SolidDnaScript");

                        // Create an instance of that type
                        var obj = Activator.CreateInstance(type);

                        // Then invoke the Run method inside that class
                        type.InvokeMember("Run", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, null);
                    }
                }
            }
            catch (Exception ex)
            {
                // Show error to user
                Application.ShowMessageBox($"Unexpected error running SolidDna script. {ex.ToString()}");
            }
        }

        #region Private Helpers

        /// <summary>
        /// Gets all referenced assemblies for this add-in
        /// </summary>
        /// <returns></returns>
        private List<MetadataReference> GetAssemblyReferences()
        {
            // Get references to all known references of this project
            var references = IoC.AddIn.ReferencedAssemblies.Select(reference =>
            {
                // Load the reference
                var loadedAssembly = Assembly.Load(reference);

                // And add a reference to this assembly into the dynamic code assembly
                return (MetadataReference)MetadataReference.CreateFromFile(loadedAssembly.Location);

            }).ToList();

            // Return references
            return references;
        }

        /// <summary>
        /// Wraps basic script text in a basic class for all standard/default using statements so you can run really basic
        /// scripts without having to define a class and a Run method or any using statements
        /// </summary>
        /// <param name="scriptText">The script text to wrap</param>
        /// <remarks>If the text already contains a SolidDnaScript class nothing will get wrapped</remarks>
        /// <returns></returns>
        private string PrependScriptWrapper(string scriptText)
        {
            // If it already has a SolidDnaScript class...
            if (scriptText.Contains("class SolidDnaScript"))
                // Do nothing
                return scriptText;

            // Wrap the text in the standard script file
            return File.ReadAllText(Path.Combine(Path.GetDirectoryName(typeof(MyAddinControl).Assembly.Location), "ScriptWrapperFormat.cs")).Replace("//CODE", scriptText);
        }

        #endregion
    }
}
