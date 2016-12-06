using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AngelSix.SolidWorks.BlankAddin
{
    [ProgId(TaskpaneIntegration.SWTASKPANE_PROGID)]
    public partial class TaskpaneHostUI : UserControl
    {
        public TaskpaneHostUI()
        {
            InitializeComponent();
        }
    }
}
