using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Thread feature data
    /// </summary>
    public class FeatureThreadData : SolidDnaObject<IThreadFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureThreadData(IThreadFeatureData model) : base(model)
        {

        }

        #endregion
    }
}