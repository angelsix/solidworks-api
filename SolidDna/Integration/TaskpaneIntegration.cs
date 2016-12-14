using SolidWorks.Interop.sldworks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A <see cref="UserControl"/> with additional helper methods for loading and working as
    /// a SolidWorks Taskpane control
    /// 
    /// IMPORTANT: It is required that the class overriding this only uses a parameterless constructor
    /// as it is created via Com so cannot have a parameter-based construction otherwise it won't load
    /// </summary>
    public class TaskpaneIntegration<THost>
        where THost : ITaskpaneControl, new()
    {
        #region Protected Members

        /// <summary>
        /// The current instance of the SolidWorks application
        /// </summary>
        protected SldWorks mSolidWorksApplication;

        /// <summary>
        /// The view of this control in the Taskpane as a SolidWorks Com native object
        /// </summary>
        protected TaskpaneView mTaskpaneView;

        /// <summary>
        /// The host <see cref="ITaskpaneControl"/> control this taskpane will create
        /// </summary>
        protected ITaskpaneControl mHostControl;

        /// <summary>
        /// The ProgId of the host control to be created
        /// </summary>
        protected string mHostProgId;

        #endregion

        #region Public Properties

        /// <summary>
        /// An absolute path to an image icon (ideally 37x37px) to use for the taskpane icon
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// The WPF user control to inject as the main control inside the <see cref="ITaskpaneControl"/> control
        /// Leave as null to ignore
        /// 
        /// NOTE: If using this control, the <see cref="THost"/> must be of type <see cref="IContainerControl"/>
        /// such as a <see cref="System.Windows.Forms.UserControl"/>
        /// </summary>
        public System.Windows.Controls.UserControl WpfControl { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="solidWorks">The currently connected SolidWorks instance</param>
        public TaskpaneIntegration(SldWorks solidWorks)
        {
            // Store reference to SolidWorks
            mSolidWorksApplication = solidWorks;

            // Fimd out the ProgId of the desired control type
            mHostProgId = new THost().ProgId;
        }

        #endregion

        #region Add To Taskpane

        /// <summary>
        /// Adds the specified host control to the SolidWorks Taskpane
        /// </summary>
        public void AddToTaskpane()
        {
            // Create our Taskpane
            mTaskpaneView = mSolidWorksApplication.CreateTaskpaneView2(this.Icon, AddInIntegration.SolidWorksAddInTitle);

            // Load our UI into the taskpane
            mHostControl = (ITaskpaneControl)mTaskpaneView.AddControl(mHostProgId, string.Empty);

            // Hook into disconnect event of SolidWorks to unload ourselves automatically
            AddInIntegration.DisconnectedFromSolidWorks += () => RemoveFromTaskpane();

            // Add WPF control if we have one
            if (this.WpfControl != null)
            {
                // Create a new ElementHost to host the Wpf control
                var elementHost = new ElementHost {
                    // Add given WPF control
                    Child = WpfControl,
                    // Dock fill it
                    Dock = DockStyle.Fill };
                
                // Add and dock it to the parent control
                if (mHostControl is Control)
                {
                    // Make sure parent is docked
                    (mHostControl as Control).Dock = DockStyle.Fill;

                    // Add WPF host
                    (mHostControl as Control).Controls.Add(elementHost);
                }
            }
        }

        /// <summary>
        /// Cleanup the taskpane when we disconnect/unload
        /// </summary>
        public void RemoveFromTaskpane()
        {
            if (mTaskpaneView != null)
            {
                // Remove taskpane view
                mTaskpaneView.DeleteView();

                // Release COM reference and cleanup memory
                Marshal.ReleaseComObject(mTaskpaneView);
            }

            mTaskpaneView = null;
        }

        #endregion
    }
}
