using SolidWorks.Interop.sldworks;
using System;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Exposes all Assembly Document calls from a <see cref="Model"/>
    /// </summary>
    public class AssemblyDocument
    {
        #region Protected Members

        /// <summary>
        /// The base model document. Note we do not dispose of this (the parent Model will)
        /// </summary>
        protected AssemblyDoc mBaseObject;

        #endregion

        #region Public Properties

        /// <summary>
        /// The raw underlying COM object
        /// WARNING: Use with caution. You must handle all disposal from this point on
        /// </summary>
        public AssemblyDoc UnsafeObject => mBaseObject;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public AssemblyDocument(AssemblyDoc model)
        {
            mBaseObject = model;
        }

        #endregion

        #region Feature Methods

        /// <summary>
        /// Gets the <see cref="ModelFeature"/> of the item in the feature tree based on its name
        /// </summary>
        /// <param name="featureName">Name of the feature</param>
        /// <returns>The <see cref="ModelFeature"/> for the named feature</returns>
        public void GetFeatureByName(string featureName, Action<ModelFeature> action)
        {
            // Wrap any error
            SolidDnaErrors.Wrap(() =>
            {
                // Create feature
                using (var model = new ModelFeature((Feature)mBaseObject.FeatureByName(featureName)))
                {
                    // Run action
                    action(model);
                }
            },
                SolidDnaErrorTypeCode.SolidWorksModel,
                SolidDnaErrorCode.SolidWorksModelAssemblyGetFeatureByNameError,
                Localization.GetString(nameof(SolidDnaErrorCode.SolidWorksModelAssemblyGetFeatureByNameError)));
        }

        #endregion
    }
}
