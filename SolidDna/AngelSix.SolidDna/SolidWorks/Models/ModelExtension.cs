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
        #region Public Properties

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
            Parent = parent;
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

            return new CustomPropertyEditor(BaseObject.CustomPropertyManager[configuration]);
        }

        #endregion

        #region Mass

        /// <summary>
        /// Gets the mass properties of a part/assembly
        /// </summary>
        /// <param name="doNotThrowOnError">If true, don't throw on errors, just return empty mass</param>
        /// <returns></returns>
        public MassProperties GetMassProperties(bool doNotThrowOnError = true)
        {
            // Wrap any error
            return SolidDnaErrors.Wrap(() =>
            {
                // Make sure we are a part
                if (!Parent.IsPart && !Parent.IsAssembly)
                {
                    if (doNotThrowOnError)
                        return new MassProperties();
                    else
                        throw new InvalidOperationException(Localization.GetString("SolidWorksModelGetMassModelNotPartError"));
                }

                double[] massProps = null;
                var status = -1;

                //
                // SolidWorks 2016 is the start of support for MassProperties2
                //
                // Tested on 2015 and crashes, so drop-back to lower version for support
                //
                if (SolidWorksEnvironment.Application.SolidWorksVersion.Version < 2016)
                    // NOTE: 2 is best accuracy
                    massProps = (double[])BaseObject.GetMassProperties(2, ref status);
                else
                    // NOTE: 2 is best accuracy
                    massProps = (double[])BaseObject.GetMassProperties2(2, out status, false);

                // Make sure it succeeded
                if (status == (int)swMassPropertiesStatus_e.swMassPropertiesStatus_UnknownError)
                {
                    if (doNotThrowOnError)
                        return new MassProperties();
                    else
                        throw new InvalidOperationException(Localization.GetString("SolidWorksModelGetMassModelStatusFailed"));
                }
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
            Parent = null;

            base.Dispose();
        }

        #endregion
    }
}
