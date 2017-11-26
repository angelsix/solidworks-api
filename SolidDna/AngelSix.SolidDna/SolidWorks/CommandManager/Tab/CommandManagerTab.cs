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
        public CommandManagerTabBox Box { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CommandManagerTab(ICommandTab tab) : base(tab)
        {
            // Add's the command tab box on creation
            Box = new CommandManagerTabBox(mBaseObject.AddCommandTabBox());
        }

        #endregion

        #region Public Methods



        #endregion

        #region Dipose

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
