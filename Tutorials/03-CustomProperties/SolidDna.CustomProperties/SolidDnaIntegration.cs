using AngelSix.SolidDna;
using System.IO;
using System.Threading.Tasks;

namespace SolidDna.CustomProperties
{
    /// <summary>
    /// Register as a SolidWorks Add-in
    /// </summary>
    public class SolidDnaAddinIntegration : AddInIntegration
    {
        /// <summary>
        /// Specific application start-up code
        /// </summary>
        /// <param name="solidWorks"></param>
        public override void ApplicationStartup()
        {

        }
    }

    /// <summary>
    /// Register as SolidDna Plguin
    /// </summary>
    public class CustomPropertiesSolidDnaPlugin : ISolidPlugIn
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
        public string AddInDescription {  get { return "An example of manipulating Custom Properties inside a SolidWorks model"; } }

        /// <summary>
        /// My Add-in title
        /// </summary>
        public string AddInTitle { get { return "SolidDNA Custom Properties"; } }

        #endregion

        #region Connect To SolidWorks

        public void ConnectedToSolidWorks()
        {
            // Create our taskpane
            mTaskpane = new TaskpaneIntegration<TaskpaneUserControlHost>()
            {
                Icon = Path.Combine(PlugInIntegration.PlugInFolder, "logo-small.png"),
                WpfControl = new CustomPropertiesUI()
            };

            // Add to taskpane
            Task.Run(() => mTaskpane.AddToTaskpane());
        }

        public void DisconnetedFromSolidWorks()
        {

        }

        #endregion
    }
}
