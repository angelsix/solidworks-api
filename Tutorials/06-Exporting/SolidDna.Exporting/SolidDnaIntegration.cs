using AngelSix.SolidDna;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SolidDna.Exporting
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

        public override void PreLoadPlugIns()
        {

        }
    }

    /// <summary>
    /// Register as SolidDna Plugin
    /// </summary>
    public class MySolidDnaPlugin : SolidPlugIn
    {
        #region Private Members

        #endregion

        #region Public Properties

        /// <summary>
        /// My Add-in description
        /// </summary>
        public override string AddInDescription {  get { return "An example of Command Items and exporting"; } }

        /// <summary>
        /// My Add-in title
        /// </summary>
        public override string AddInTitle { get { return "SolidDNA Exporting"; } }

        #endregion

        #region Connect To SolidWorks

        public override void ConnectedToSolidWorks()
        {
            Dna.Application.FileOpened += Application_FileOpened;
        }

        private void Application_FileOpened(string arg1, Model arg2)
        {
            // Create our command group
            var group1 = Dna.Application.CommandManager.CreateCommands(
                title: "test",
                items: new List<CommandManagerItem>(new[] { new CommandManagerItem { Name = "my name", Tooltip = "tool", Hint = "hint" } }),
                iconListsPath: "",
                hint: "my hint",
                tooltip: "my tooltip");

            //var group2 = Dna.Application.CommandManager.CreateCommands(
            //    title: "test 2",
            //    items: new List<CommandManagerItem>(new[] { new CommandManagerItem { } }),
            //    iconListsPath: "",
            //    hint: "my hint 2",
            //    tooltip: "my tooltip 2");
        }

        public override void DisconnetedFromSolidWorks()
        {

        }

        #endregion
    }
}
