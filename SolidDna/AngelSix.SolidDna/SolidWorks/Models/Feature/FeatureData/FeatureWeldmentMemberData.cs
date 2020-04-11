using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Weldment Member feature data
    /// </summary>
    public class FeatureWeldmentMemberData : SolidDnaObject<IStructuralMemberFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureWeldmentMemberData(IStructuralMemberFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
