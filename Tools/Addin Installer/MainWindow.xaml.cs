using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Linq;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Controls;
using AngelSix.SolidWorksApi.AddinInstaller.Properties;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AngelSix.SolidWorksApi.AddinInstaller
{
    /// <summary>
    /// A tool to aid in running the correct RegAsm command on a SolidWorks Add-in Dll
    /// to visually install and uninstall add-ins
    /// 
    /// NOTE: This application is designed to run on x64 machines and x64 installs of SolidWorks by default
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        #region Private Members

        /// <summary>
        /// The name of the RegAsm tool
        /// </summary>
        private const string MRegAsmFilename = "RegAsm.exe";

        /// <summary>
        /// The folder location where 64bit .Net Frameworks are installed, relative to the Windows folder
        /// </summary>
        private const string MRegAsmWindowsPath = "Microsoft.NET\\Framework64";

        /// <summary>
        /// List of installed add-ins. We use a backing field so we can call <see cref="PropertyChanged"/> when the list is set.
        /// </summary>
        private ObservableCollection<string> _installedAddInTitles = new ObservableCollection<string>();

        #endregion

        #region Public Members

        /// <summary>
        /// The event that fires when the list of active add-ins changes value so the user interface knows when to refresh.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Try and local RegAsm
            LocateRegAsm();
            
            // Set up the data context so we can use binding
            DataContext = this;

            // Find previous dll paths
            ReadPreviousPaths();

            // Get all add-ins that are currently installed
            GetInstalledAddIns();
        }

        #endregion

        #region Public properties

        /// <summary>
        /// A list of all add-ins that were previously registered.
        /// </summary>
        public ObservableCollection<string> PreviousAddInPaths { get; private set; } = new ObservableCollection<string>();

        /// <summary>
        /// A list of all add-ins that are currently installed.
        /// </summary>
        public ObservableCollection<string> InstalledAddInTitles
        {
            get => _installedAddInTitles;
            private set
            {
                _installedAddInTitles = value;
                OnPropertyChanged();
            }
        }
        
        #endregion

        #region Private Helpers

        /// <summary>
        /// Get the previously used DLL paths from the user settings.
        /// </summary>
        private void ReadPreviousPaths()
        {
            // Get the list of previously used paths from the user settings as a string collection
            var settings = Settings.Default.PreviousPaths;
            if (settings != null)
                PreviousAddInPaths = new ObservableCollection<string>(settings.Cast<string>());

            // Get notified whenever we add or remove a path so we can save them
            PreviousAddInPaths.CollectionChanged += SavePaths;
        }

        /// <summary>
        /// Get all add-ins that are currently installed.
        /// </summary>
        private void GetInstalledAddIns()
        {
            const RegistryHive hive = RegistryHive.LocalMachine;
            const RegistryView view = RegistryView.Registry64;
            const string keyPath = "SOFTWARE\\SolidWorks\\AddIns";

            var addInTitles = new List<string>();
            using (var registryKey = RegistryKey.OpenBaseKey(hive, view).OpenSubKey(keyPath))
            {
                if (registryKey == null)
                    return;

                foreach (var subKeyName in registryKey.GetSubKeyNames())
                {
                    using (var key = registryKey.OpenSubKey(subKeyName))
                    {
                        var value = (string) key?.GetValue("Title");

                        // The Presentation Manager can be listed multiple times and it's not listed in the add-in window, so we skip it.
                        if (value == null || value.Equals("Presentation Manager"))
                            continue;

                        addInTitles.Add(value);
                    }
                }
            }

            InstalledAddInTitles = new ObservableCollection<string>(addInTitles);
        }

        /// <summary>
        /// Call this method when a property changes and the user interface needs a refresh.
        /// </summary>
        /// <param name="name"></param>
        private void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        
        /// <summary>
        /// Save the list of previously used paths as user settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SavePaths(object sender, NotifyCollectionChangedEventArgs e)
        {
            var stringCollection = new StringCollection();
            stringCollection.AddRange(PreviousAddInPaths.ToArray());
            Settings.Default.PreviousPaths = stringCollection;
            Settings.Default.Save();
        }

        /// <summary>
        /// Checks we have a SolidWorks application, regasm and an add-in
        /// </summary>
        /// <returns></returns>
        private static bool SanityCheck(string regasmPath, string dllPath)
        {
            // Check RegAsm
            if (string.IsNullOrEmpty(regasmPath))
            {
                MessageBox.Show("Please specify a path to a valid RegAsm application", "No RegAsm found");
                return false;
            } 
            if (!File.Exists(regasmPath))
            {
                MessageBox.Show("The RegAsm file does not exist", "No RegAsm found");
                return false;
            }

            // Check Dll
            if (string.IsNullOrEmpty(dllPath))
            {
                MessageBox.Show("Please specify a path to a valid SolidWorks Add-in dll", "No Add-in found");
                return false;
            }
            if (!File.Exists(dllPath))
            {
                MessageBox.Show("The dll file does not exist", "File not found");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Attempts to local the RegAsm exe and set it to the path
        /// </summary>
        private void LocateRegAsm()
        {
            // Locate SolidWorks exe in Program Files
            var results = new List<string>();
            FindByFilename(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), MRegAsmWindowsPath), null, MRegAsmFilename, results);

            // If we have at least one, use the last one (so newest version)
            if (results?.Count > 0)
                RegAsmPath.Text = results.Last();
        }

        /// <summary>
        /// Gets all files that match the search pattern
        /// Searches recursively
        /// </summary>
        /// <param name="path">The path to search within</param>
        /// <param name="pathContains">The string the path must contain in order to process further down the tree, or null to check all</param>
        /// <param name="filename">The filename to find (case insensitive)</param>
        /// <param name="results">The results to store the results in</param>
        /// <returns></returns>
        private static void FindByFilename(string path, string pathContains, string filename, List<string> results = null)
        {
            // Create new list if none passed in
            if (results == null)
                results = new List<string>();

            // Get all files in the current folder
            try
            {
                var files = Directory.EnumerateFiles(path).Where(f => string.Equals(Path.GetFileName(f), filename, StringComparison.InvariantCultureIgnoreCase)).ToList();
                if (files.Count > 0)
                    results.AddRange(files);
            }
            catch
            { }

            // Search all sub-folders
            try
            {
                // Allow case-insensitive checking
                pathContains = pathContains?.ToLower();

                // Search into directories that match
                Directory.EnumerateDirectories(path).Where(f => string.IsNullOrEmpty(pathContains) || f.ToLower().Contains(pathContains)).ToList().ForEach(dir => FindByFilename(dir, null, filename, results));
            }
            catch { }
        }

        /// <summary>
        /// Try to register an addin by its path.
        /// </summary>
        /// <param name="addinPath"></param>
        private void InstallAddin(string addinPath)
        {
            // Sanity check
            if (!SanityCheck(RegAsmPath.Text, addinPath))
                return;

            // Run the RegAsm with the Dll path as an argument
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = RegAsmPath.Text,
                    Arguments = $"/codebase \"{addinPath}\"",
                    // Run as admin
                    Verb = "runas",
                    // Redirect input and output
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            // Read the output
            var result = process.StandardError.ReadToEnd();
            process.WaitForExit();

            // If it exit successfully
            if (process.ExitCode == 0)
            {
                AddPathToPreviousPaths(addinPath);
                GetInstalledAddIns();
                MessageBox.Show("Add-in was successfully registered", "Success");
            }
            // Otherwise just show the results
            else
                MessageBox.Show(result, "Unexpected Response", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Try to unregister an addin by its path.
        /// </summary>
        /// <param name="addinPath"></param>
        private void UninstallAddin(string addinPath)
        {
            // Sanity check
            if (!SanityCheck(RegAsmPath.Text, addinPath))
                return;

            // Run the RegAsm with the Dll path as an argument
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = RegAsmPath.Text,
                    Arguments = $"/u \"{addinPath}\"",
                    // Run as admin
                    Verb = "runas",
                    // Redirect input and output
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            // Read the output
            var result = process.StandardError.ReadToEnd();
            process.WaitForExit();

            // If it exit successfully
            if (process.ExitCode == 0)
            {
                AddPathToPreviousPaths(addinPath);
                GetInstalledAddIns();
                MessageBox.Show("Add-in was successfully unregistered", "Success");
            }
            // Otherwise just show the results
            else
                MessageBox.Show(result, "Unexpected Response", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Add the path to the list of previous paths, if it is not on the list already
        /// </summary>
        /// <param name="addinPath"></param>
        private void AddPathToPreviousPaths(string addinPath)
        {
            if (!PreviousAddInPaths.Any(x => x.Equals(addinPath, StringComparison.InvariantCultureIgnoreCase)))
                PreviousAddInPaths.Add(addinPath);
        }

        #endregion

        #region UI Events

        /// <summary>
        /// Browse for RegAsm manually
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseRegAsmButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = $"RegAsm | {MRegAsmFilename}",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Windows)
            };

            // Open dialog
            var result = ofd.ShowDialog();

            // If they canceled, return
            if (!result.HasValue || !result.Value)
                return;

            // If they selected a file, use that
            RegAsmPath.Text = ofd.FileName;
        }

        /// <summary>
        /// Browse for an add-in dll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseDllButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = $"Add-in Dll | *.dll",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            // Open dialog
            var result = ofd.ShowDialog();

            // If they canceled, return
            if (!result.HasValue || !result.Value)
                return;

            // If they selected a file, use that
            DllPath.Text = ofd.FileName;
        }

        /// <summary>
        /// Installs the selected add-in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstallButton_Click(object sender, RoutedEventArgs e) => InstallAddin(DllPath.Text);

        /// <summary>
        /// Uninstalls the selected add-in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UninstallButton_Click(object sender, RoutedEventArgs e) => UninstallAddin(DllPath.Text);

        /// <summary>
        /// Installs an addin from the list of previous paths
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstallPreviousAddIn_OnClick(object sender, RoutedEventArgs e)
        {
            // Make sure this gets called from a button
            if (!(sender is Button button)) return;

            // Get the data context of the button, which should just a string path
            var addInPath = (string) button.DataContext;

            // Try to install the addin
            InstallAddin(addInPath);
        }

        /// <summary>
        /// Uninstalls an addin from the list of previous paths
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UninstallPreviousAddIn_OnClick(object sender, RoutedEventArgs e)
        {
            // Make sure this gets called from a button
            if (!(sender is Button button)) return;

            // Get the data context of the button, which should just a string path
            var addInPath = (string) button.DataContext;

            // Try to uninstall the addin
            UninstallAddin(addInPath);
        }

        /// <summary>
        /// Removes an addin path from the list of previous paths
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemovePathFromPrevious_OnClick(object sender, RoutedEventArgs e)
        {
            // Make sure this gets called from a button
            if (!(sender is Button button)) return;

            // Get the data context of the button, which should just a string path
            var addInPath = (string) button.DataContext;

            // Remove the item from the list
            PreviousAddInPaths.Remove(addInPath);
        }

        #endregion
    }
}
