using SolidWorks.Interop.sldworks;
using System.Threading.Tasks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Taskpane
    /// </summary>
    public class Taskpane : SolidDnaObject<ITaskpaneView>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Taskpane(ITaskpaneView taskpane) : base(taskpane)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a control (Windows <see cref="System.Windows.Forms.UserControl"/>) to the taskpane
        /// that has been exposed to COM and has a given ProgId
        /// </summary>
        /// <typeparam name="T">The type of UserControl being created</typeparam>
        /// <param name="progId">The [ProgId()] attribute value adorned on the UserControl class</param>
        /// <param name="licenseKey">The license key (for specific SolidWorks add-in types)</param>
        /// <returns></returns>
        public async Task<T> AddControlAsync<T>(string progId, string licenseKey)
        {
            // Wrap any error creating the taskpane in a SolidDna exception
            return SolidDnaErrors.Wrap<T>(() =>
            {
                // Attempt to create the taskpane
                return (T)BaseObject.AddControl(progId, licenseKey);
            },
                SolidDnaErrorTypeCode.SolidWorksTaskpane,
                SolidDnaErrorCode.SolidWorksTaskpaneAddControlError,
                await Localization.GetStringAsync("ErrorSolidWorksTaskpaneAddControlError"));
        }


        #endregion
    }
}
