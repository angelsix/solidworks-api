using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A command manager tab for the top command group in SolidWorks
    /// </summary>
    public class CommandManagerTab : SolidDnaObject<ICommandTab>
    {
        #region Public Properties

        /// <summary>
        /// The command tab box for this tab
        /// </summary>
        public CommandManagerTabBox Box { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CommandManagerTab(ICommandTab tab) : base(tab)
        {
            // Adds the command tab box on creation
            Box = new CommandManagerTabBox(BaseObject.AddCommandTabBox());
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposing
        /// </summary>
        public override void Dispose()
        {
            // Dispose of box
            Box?.Dispose();

            base.Dispose();
        }

        #endregion
    }
}
