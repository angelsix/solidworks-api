using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Coordinate System feature data
    /// </summary>
    public class FeatureCoordinateSystemData : SolidDnaObject<ICoordinateSystemFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCoordinateSystemData(ICoordinateSystemFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
