using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace AngelSix.SolidDna
{
    public partial class SolidWorksApplication : SharedSolidDnaObject<SldWorks>
    {
        public class SolidWorksPreferences
        {
            /// <summary>
            /// The scaling factor used when exporting as DXF
            /// </summary>
            public double DxfOutputScaleFactor
            {
                get => SolidWorksEnvironment.Application.GetUserPreferencesDouble(swUserPreferenceDoubleValue_e.swDxfOutputScaleFactor);
                set => SolidWorksEnvironment.Application.SetUserPreferencesDouble(swUserPreferenceDoubleValue_e.swDxfOutputScaleFactor, value);
            }

            /// <summary>
            /// The scaling factor used when exporting as DXF
            /// </summary>
            public int DxfMultiSheetOption
            {
                get => SolidWorksEnvironment.Application.GetUserPreferencesInteger(swUserPreferenceIntegerValue_e.swDxfMultiSheetOption);
                set => SolidWorksEnvironment.Application.SetUserPreferencesInteger(swUserPreferenceIntegerValue_e.swDxfMultiSheetOption, value);
            }

            /// <summary>
            /// The scaling of DXF output. If true no scaling will be done
            /// </summary>
            public bool DxfOutputNoScale
            {
                get => SolidWorksEnvironment.Application.GetUserPreferencesInteger(swUserPreferenceIntegerValue_e.swDxfOutputNoScale) == 1;
                set => SolidWorksEnvironment.Application.SetUserPreferencesInteger(swUserPreferenceIntegerValue_e.swDxfOutputNoScale, value ? 1 : 0);
            }
        }
    }
}
