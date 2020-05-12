namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents the suppression state action of a <see cref="ModelFeature"/>
    /// 
    /// NOTE: Known types are here http://help.solidworks.com/2020/english/api/swconst/SOLIDWORKS.Interop.swconst~SOLIDWORKS.Interop.swconst.swFeatureSuppressionAction_e.html
    /// 
    /// </summary>
    public enum ModelFeatureSuppressionState
    {
        /// <summary>
        /// Suppress the feature
        /// </summary>
        SuppressFeature = 0,

        /// <summary>
        /// Unsuppress the feature
        /// </summary>
        UnSuppressFeature = 1,

        /// <summary>
        /// Unsuppress the children of the feature
        /// </summary>
        UnSuppressDependent = 2,

    }
}
