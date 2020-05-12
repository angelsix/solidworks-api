using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a selected object of a SolidWorks object.
    /// The type can be one of many different things
    /// 
    /// NOTE: All mappings from selected entities to specific objects are here
    /// http://help.solidworks.com/2020/English/api/swconst/SolidWorks.Interop.swconst~SolidWorks.Interop.swconst.swSelectType_e.html
    /// </summary>
    public class SelectedObject : SolidDnaObject<object>
    {
        #region Public Properties

        /// <summary>
        /// The type of the selected object
        /// </summary>
        public swSelectType_e ObjectType { get; set; }

        #region Type Checks

        /// <summary>
        /// True if this object is a feature.
        /// From the feature you can check the specific feature type and
        /// get the specific feature from that
        /// </summary>
        public bool IsFeature => ObjectType == swSelectType_e.swSelDATUMPLANES ||
                    ObjectType == swSelectType_e.swSelDATUMAXES ||
                    ObjectType == swSelectType_e.swSelDATUMPOINTS ||
                    ObjectType == swSelectType_e.swSelATTRIBUTES ||
                    ObjectType == swSelectType_e.swSelSKETCHES ||
                    ObjectType == swSelectType_e.swSelSECTIONLINES ||
                    ObjectType == swSelectType_e.swSelDETAILCIRCLES ||
                    ObjectType == swSelectType_e.swSelMATES ||
                    ObjectType == swSelectType_e.swSelBODYFEATURES ||
                    ObjectType == swSelectType_e.swSelREFCURVES ||
                    ObjectType == swSelectType_e.swSelREFERENCECURVES ||
                    ObjectType == swSelectType_e.swSelCTHREADS ||
                    ObjectType == swSelectType_e.swSelCONFIGURATIONS ||
                    ObjectType == swSelectType_e.swSelREFSILHOUETTE ||
                    ObjectType == swSelectType_e.swSelCAMERAS ||
                    ObjectType == swSelectType_e.swSelSWIFTANNOTATIONS ||
                    ObjectType == swSelectType_e.swSelSWIFTFEATURES;

        /// <summary>
        /// True if this object is a dimension
        /// </summary>
        public bool IsDimension => ObjectType == swSelectType_e.swSelDIMENSIONS;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SelectedObject(object model) : base(model)
        {
            
        }

        #endregion

        #region Type Cast

        /// <summary>
        /// Casts the object to a <see cref="ModelFeature"/>.
        /// Check with <see cref="IsFeature"/> first to assure that it is this type
        /// </summary>
        /// <param name="action">The feature is passed into this action to be used within it</param>
        public void AsFeature(Action<ModelFeature> action)
        {
            // Wrap any error
            SolidDnaErrors.Wrap(() =>
            {
                // Create feature
                using (var model = new ModelFeature((Feature)BaseObject))
                {
                    // Run action
                    action(model);
                }
            },
                SolidDnaErrorTypeCode.SolidWorksModel,
                SolidDnaErrorCode.SolidWorksModelSelectedObjectCastError,
                Localization.GetString("SolidWorksModelSelectedObjectCastError"));
        }

        /// <summary>
        /// Casts the object to a <see cref="ModelDisplayDimension"/>.
        /// Check with <see cref="IsDimension"/> first to assure that it is this type
        /// </summary>
        /// <param name="action">The Dimension is passed into this action to be used within it</param>
        public void AsDimension(Action<ModelDisplayDimension> action)
        {
            // Wrap any error
            SolidDnaErrors.Wrap(() =>
            {
                // Create feature
                using (var model = new ModelDisplayDimension((IDisplayDimension)BaseObject))
                {
                    // Run action
                    action(model);
                }
            },
                SolidDnaErrorTypeCode.SolidWorksModel,
                SolidDnaErrorCode.SolidWorksModelSelectedObjectCastError,
                Localization.GetString("SolidWorksModelSelectedObjectCastError"));
        }

        #endregion
    }
}
