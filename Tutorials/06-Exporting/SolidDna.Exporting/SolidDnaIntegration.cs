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
            // Create our command group
            var group1 = Dna.Application.CommandManager.CreateCommands(
                title: "Export Part",
                items: new List<CommandManagerItem>(new[] { new CommandManagerItem {
                    Name = "DXF",
                    Tooltip = "DXF",
                    Hint = "Export part as DXF",
                    VisibleForDrawings = false,
                    VisibleForAssemblies = false,
                    OnClick = () =>
                    {
                        FileExporting.ExportPartAsDxf();
                    }
                } }),
                iconListsPath: "",
                hint: "Export parts in other formats",
                tooltip: "Such as DXF, STEP and IGES");

            var group2 = Dna.Application.CommandManager.CreateCommands(
                title: "Export Assembly",
                items: new List<CommandManagerItem>(new[] { new CommandManagerItem { Name = "my name3", Tooltip = "tool3", Hint = "hint2", VisibleForDrawings = false, VisibleForParts = false } }),
                iconListsPath: "",
                hint: "Export assemblies in other formats",
                tooltip: "Such as DXF, Step and IGES");
        }
        
        public override void DisconnetedFromSolidWorks()
        {

        }

        #endregion
    }
}
