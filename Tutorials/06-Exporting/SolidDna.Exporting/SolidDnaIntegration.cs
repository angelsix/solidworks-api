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
            // Part commands
            var partGroup = Dna.Application.CommandManager.CreateCommands(
                title: "Export Part",
                items: new List<CommandManagerItem>(new[] {

                    new CommandManagerItem {
                        Name = "DXF",
                        Tooltip = "DXF",
                        ImageIndex = 0,
                        Hint = "Export part as DXF",
                        VisibleForDrawings = false,
                        VisibleForAssemblies = false,
                        OnClick = () =>
                        {
                            FileExporting.ExportPartAsDxf();
                        }
                    },

                    new CommandManagerItem {
                        Name = "STEP",
                        Tooltip = "STEP",
                        ImageIndex = 2,
                        Hint = "Export part as STEP",
                        VisibleForDrawings = false,
                        VisibleForAssemblies = false,
                        OnClick = () =>
                        {
                            FileExporting.ExportModelAsStep();
                        }
                    },

                }),
                iconListsPath: "icons{0}.png",
                hint: "Export parts in other formats",
                tooltip: "Such as DXF, STEP and IGES");

            // Assembly commands
            var assemblyGroup = Dna.Application.CommandManager.CreateCommands(
                title: "Export Assembly",
                items: new List<CommandManagerItem>(new[] {

                    new CommandManagerItem {
                        Name = "STEP",
                        Tooltip = "STEP",
                        ImageIndex = 2,
                        Hint = "Export assembly as STEP",
                        VisibleForDrawings = false,
                        VisibleForParts = false,
                        OnClick = () =>
                        {
                            FileExporting.ExportModelAsStep();
                        }
                    },

                }),
                iconListsPath: "icons{0}.png",
                hint: "Export assemblies in other formats",
                tooltip: "Such as Step");

            // Drawing commands
            var drawingGroup = Dna.Application.CommandManager.CreateCommands(
                title: "Export Drawing",
                items: new List<CommandManagerItem>(new[] {

                    new CommandManagerItem {
                        Name = "PDF",
                        Tooltip = "PDF",
                        Hint = "Export drawing as PDF",
                        ImageIndex = 1,
                        VisibleForParts = false,
                        VisibleForAssemblies = false,
                        OnClick = () =>
                        {
                            FileExporting.ExportDrawingAsPdf();
                        }
                    },

                }),
                iconListsPath: "icons{0}.png",
                hint: "Export drawing to other formats",
                tooltip: "Such as PDF");
        }

        public override void DisconnetedFromSolidWorks()
        {

        }

        #endregion
    }
}
