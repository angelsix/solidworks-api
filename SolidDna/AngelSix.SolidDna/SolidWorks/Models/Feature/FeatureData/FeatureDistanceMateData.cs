using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Distance Mate feature data
    /// </summary>
    public class FeatureDistanceMateData : SolidDnaObject<IDistanceMateFeatureData>
    {
    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    public FeatureDistanceMateData(IDistanceMateFeatureData model) : base(model)
    {

    }

    #endregion
    }
}