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
            public double DxfOutputScaleFactor => Dna.Application.GetUserPreferencesDouble(swUserPreferenceDoubleValue_e.swDxfOutputScaleFactor);
        }
    }
}
