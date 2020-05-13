using System;
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
    public class TaskpaneIntegration<THost> where THost : ITaskpaneControl, new()
    {
        #region Protected Members

        /// <summary>
        /// The SolidWorks Taskpane object for this taskpane
        /// </summary>
        protected Taskpane mTaskpaneView;

        /// <summary>
        /// The host <see cref="ITaskpaneControl"/> control this taskpane will create
        /// </summary>
        protected ITaskpaneControl mHostControl;

        /// <summary>
        /// The host control that hosts the WPF control
        /// </summary>
        protected ElementHost mElementHost;

        /// <summary>
        /// The ProgId of the host control to be created
        /// </summary>
        protected string mHostProgId;

        #endregion

        #region Public Properties

        /// <summary>
        ///     An absolute path to an icon to use for the taskpane.
        ///     The bitmap should be 16 colors and 16 x 18 (width x height) pixels. 
        ///     Any portions of the bitmap that are white (RGB 255,255,255) will be transparent.
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
        public TaskpaneIntegration()
        {
            // Find out the ProgId of the desired control type
            mHostProgId = new THost().ProgId;
        }

        #endregion

        #region Add To Taskpane

        /// <summary>
        /// Adds the specified host control to the SolidWorks Taskpane
        /// 
        /// NOTE: This call MUST be run on the UI thread
        /// </summary>
        public async void AddToTaskpaneAsync()
        {
            // Create our Taskpane
            mTaskpaneView = await AddInIntegration.SolidWorks.CreateTaskpaneAsync(Icon, AddInIntegration.SolidWorksAddInTitle);

            // Load our UI into the taskpane
            mHostControl = await mTaskpaneView.AddControlAsync<ITaskpaneControl>(mHostProgId, string.Empty);

            // Set UI thread
            ThreadHelpers.Enable((Control)mHostControl);

            // Hook into disconnect event of SolidWorks to unload ourselves automatically
            AddInIntegration.DisconnectedFromSolidWorks += () => RemoveFromTaskpane();

            // Add WPF control if we have one
            if (WpfControl != null)
            {
                // NOTE: ElementHost must be created on UI thread
                // Create a new ElementHost to host the WPF control
                mElementHost = new ElementHost
                {
                    // Add given WPF control
                    Child = WpfControl,
                    // Dock fill it
                    Dock = DockStyle.Fill
                };

                // IMPORTANT: 
                //
                //   This litle f*cking beauty right here took me 18 hours to figure out
                //   Whenever you add a WPF control to SolidWorks, Win 10 is nice enough
                //   if your machine has a pen or stylus to start up a
                //   System.Windows.Input.PenThreadWorker.ThreadProc
                //   thread that NEVER ends, even after closing SolidWorks or unloading 
                //   the domain. This causes the AppDomain.Unload to fail, or SolidWorks
                //   to never close.
                //   
                //   I found a way to disable the thread using this AppContext switch
                //   Now life is good again.
                //
                AppContext.SetSwitch("Switch.System.Windows.Input.Stylus.DisableStylusAndTouchSupport", true);

                // Add and dock it to the parent control
                if (mHostControl is Control)
                {
                    // Make sure parent is docked
                    (mHostControl as Control).Dock = DockStyle.Fill;

                    // Add WPF host
                    (mHostControl as Control).Controls.Add(mElementHost);
                }

            }
        }

        /// <summary>
        /// Cleanup the taskpane when we disconnect/unload
        /// </summary>
        public void RemoveFromTaskpane()
        {
            if (mTaskpaneView == null)
                return;

            (mHostControl as Control)?.Controls.Clear();

            mElementHost?.Dispose();

            // Remove taskpane view
            mTaskpaneView.Dispose();
        }

        #endregion
    }
}
