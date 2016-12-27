using SolidWorks.Interop.sldworks;


namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks model selection manager
    /// </summary>
    public class ModelSelectionManager : SolidDnaObject<SelectionMgr>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ModelSelectionManager(SelectionMgr model) : base(model)
        {

        }

        #endregion
    }
}