using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A box for a command manager tab in SolidWorks
    /// </summary>
    public class CommandManagerTabBox : SolidDnaObject<ICommandTabBox>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CommandManagerTabBox(ICommandTabBox box) : base(box)
        {

        }

        #endregion
    }
}
