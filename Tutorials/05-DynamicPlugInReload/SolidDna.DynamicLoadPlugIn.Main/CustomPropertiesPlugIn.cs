using AngelSix.SolidDna;
using System.IO;

namespace SolidDna.DynamicLoadPlugIn
{
    /// <summary>
    /// Register as SolidDna Plugin
    /// </summary>
    public class CustomPropertiesSolidDnaPlugin : SolidPlugIn
    {
        #region Private Members

        /// <summary>
        /// The Taskpane UI for our plug-in
        /// </summary>
        private TaskpaneIntegration<TaskpaneUserControlHost> mTaskpane;

        #endregion

        #region Public Properties

        /// <summary>
        /// My Add-in description
        /// </summary> 
        public override string AddInDescription => "An example of editing a plug-in with SolidWorks still running";

        /// <summary>
        /// My Add-in title
        /// </summary>
        public override string AddInTitle => "SolidDNA Dynamic Load PlugIn";

        #endregion

        #region Connect To SolidWorks

        public override void ConnectedToSolidWorks()
        {
            // Create our taskpane
            mTaskpane = new TaskpaneIntegration<TaskpaneUserControlHost>()
            {
                Icon = Path.Combine(this.AssemblyPath(), "logo-small.bmp"),
                // IMPORTANT: You must also run regasm (or the SolidWorks Add-in Installer tool)
                //            on the plug-in dll that is created for this project to expose
                //            this WPF UI over COM, otherwise your taskpane will not load the UI
                //
                //            In this example that is running regasm on SolidDna.DynamicLoadPlugIn.Main.dll
                //            as well as on SolidDna.DynamicLoadPlugIn.dll
                WpfControl = new CustomPropertiesUI()
            };

            // Add to taskpane
            mTaskpane.AddToTaskpaneAsync();
        }

        public override void DisconnectedFromSolidWorks()
        {

        }

        #endregion
    }
}
