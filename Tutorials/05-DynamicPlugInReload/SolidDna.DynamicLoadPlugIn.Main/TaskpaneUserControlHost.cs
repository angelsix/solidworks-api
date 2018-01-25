using System.Windows.Forms;
using System.Runtime.InteropServices;
using AngelSix.SolidDna;

namespace SolidDna.DynamicLoadPlugIn
{
    [ProgId(MyProgId)]
    public partial class TaskpaneUserControlHost : UserControl, ITaskpaneControl
    {
        #region Private Members

        /// <summary>
        /// Our unique ProgId for SolidWorks to find and load us
        /// </summary>
        private const string MyProgId = "AngelSix.SolidDna.DynamicLoad";

        #endregion

        #region Public Properties

        public string ProgId => MyProgId;

        #endregion
    }
}
