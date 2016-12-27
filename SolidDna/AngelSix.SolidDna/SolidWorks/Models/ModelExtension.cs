using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks model extension of any type (Drawing, Part or Assembly)
    /// </summary>
    public class ModelExtension : SolidDnaObject<ModelDocExtension>
    {
        #region Protected Members

        /// <summary>
        /// The parent Model for this extension
        /// </summary>
        public Model Parent { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ModelExtension(ModelDocExtension model, Model parent) : base(model)
        {
            this.Parent = parent;
        }

        #endregion

        #region Custom Properties

        /// <summary>
        /// Gets a configuration-specific custom property editor for the specified configuration
        /// If no configuration is specified the default custom property manager is returned
        /// 
        /// NOTE: Custom Property Editor must be disposed of once finished
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public CustomPropertyEditor CustomPropertyEditor(string configuration = null)
        {
            // TODO: Add error checking and exception catching

            return new CustomPropertyEditor(mBaseObject.CustomPropertyManager[configuration]);
        }

        #endregion

        #region Mass

        /// <summary>
        /// Gets the mass properties of a part/assembly
        /// </summary>
        /// <returns></returns>
        public MassProperties GetMassProperties()
        {
            // Wrap any error
            return SolidDnaErrors.Wrap(() =>
            {
                // Make sure we are a part
                if (!this.Parent.IsPart && !this.Parent.IsAssembly)
                    throw new InvalidOperationException(Localization.GetString("SolidWorksModelGetMassModelNotPartError"));

                int status;
                // NOTE: 2 is best accuracy, 
                var massProps = (double[])mBaseObject.GetMassProperties2(2, out status, false);

                // Make sure it succeeded
                if (status == (int)swMassPropertiesStatus_e.swMassPropertiesStatus_UnknownError)
                    throw new InvalidOperationException(Localization.GetString("SolidWorksModelGetMassModelStatusFailed"));
                // If we have no mass, return empty
                else if (status == (int)swMassPropertiesStatus_e.swMassPropertiesStatus_NoBody)
                    return new MassProperties();

                // Otherwise we have the properties so return them
                return new MassProperties(massProps);
            },
                SolidDnaErrorTypeCode.SolidWorksModel,
                SolidDnaErrorCode.SolidWorksModelGetMassPropertiesError,
                Localization.GetString("SolidWorksModelGetMassPropertiesError"));
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            // Clear reference to be safe
            this.Parent = null;

            base.Dispose();
        }

        #endregion
    }
}
