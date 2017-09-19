using AngelSix.SolidDna;
using SolidWorks.Interop.sldworks;
using System.IO;

namespace SolidDna.WpfAddIn
{
    /// <summary>
    /// Register as a SolidWorks Add-in
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
        /// Steps to take before any add-ins load
        /// </summary>
        /// <returns></returns>
        public override void PreLoadPlugIns()
        {

        }
    }

    /// <summary>
    /// My first SolidDna Plguin
    /// </summary>
    public class MySolidDnaPlguin : SolidPlugIn
    {
        #region Private Members

        /// <summary>
        /// The Taskpane UI for our plug-in
        /// </summary>
        private TaskpaneIntegration<MyTaskpaneUI> mTaskpane;

        #endregion

        #region Public Properties

        /// <summary>
        /// My Add-in description
        /// </summary>
        public override string AddInDescription {  get { return "My Addin Description"; } }

        /// <summary>
        /// My Add-in title
        /// </summary>
        public override string AddInTitle { get { return "My Addin Title"; } }

        #endregion

        #region Connect To SolidWorks

        public override void ConnectedToSolidWorks()
        {
            // Create our taskpane
            mTaskpane = new TaskpaneIntegration<MyTaskpaneUI>()
            {
                Icon = Path.Combine(PlugInIntegration.PlugInFolder, "logo-small.png"),
                WpfControl = new MyAddinControl()
            };

            mTaskpane.AddToTaskpane();
        }

        public override void DisconnectedFromSolidWorks()
        {

        }

        #endregion
    }
}
