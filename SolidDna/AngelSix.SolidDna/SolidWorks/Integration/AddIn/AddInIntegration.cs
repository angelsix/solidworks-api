using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Dna;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using static Dna.FrameworkDI;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Integrates into SolidWorks as an add-in and registers for callbacks provided by SolidWorks
    /// 
    /// IMPORTANT: The class that overrides <see cref="ISwAddin"/> MUST be the same class that 
    /// contains the ComRegister and ComUnregister functions due to how SolidWorks loads add-ins
    /// </summary>
    public static class AddInIntegration
    {
        #region Public Properties

        /// <summary>
        /// Represents the current SolidWorks application
        /// </summary>
        public static SolidWorksApplication SolidWorks { get; set; }

        #endregion

        #region Private properties

        /// <summary>
        /// A list of all add-ins that are currently active.
        /// Private so we can call <see cref="TearDown"/> when the list becomes empty.
        /// </summary>
        private static List<SolidAddIn> ActiveAddIns { get; } = new List<SolidAddIn>();

        #endregion

        #region Connect to SOLIDWORKS

        /// <summary>
        /// Attempts to set the SolidWorks property to the active SolidWorks instance
        /// </summary>
        /// <returns></returns>
        public static bool ConnectToActiveSolidWorksForStandAlone()
        {
            try
            {
                // Try and get the active SolidWorks instance
                SolidWorks = new SolidWorksApplication((SldWorks)Marshal.GetActiveObject("SldWorks.Application"), 0);

                // Log it
                Logger?.LogDebugSource($"Acquired active instance SolidWorks in Stand-Alone mode");

                // Return if successful
                return SolidWorks != null;
            }
            // If we failed to get active instance...
            catch (COMException)
            {
                // Log it
                Logger?.LogDebugSource($"Failed to get active instance of SolidWorks in Stand-Alone mode");

                // Return failure
                return false;
            }
        }

        /// <summary>
        /// Attempts to set the SolidWorks property to the active SolidWorks instance.
        /// Remember to call <see cref="TearDown"/> once done.
        /// </summary>
        /// <returns></returns>
        public static bool ConnectToActiveSolidWorks()
        {
            // Create new blank add-in
            var addin = new BlankAddInIntegration();

            // Return if we successfully got an instance
            return ConnectToActiveSolidWorksForStandAlone();
        }

        #endregion

        #region Adding and removing active add-ins

        /// <summary>
        /// Add a newly loaded add-in to the list of active ones.
        /// </summary>
        /// <param name="addIn"></param>
        public static void AddAddIn(SolidAddIn addIn)
        {
            ActiveAddIns.Add(addIn);
        }

        /// <summary>
        /// Remove an unloaded add-in from the list of active ones.
        /// </summary>
        /// <param name="addIn"></param>
        public static void RemoveAddInAndTearDownSolidWorksWhenLast(SolidAddIn addIn)
        {
            ActiveAddIns.Remove(addIn);

            // If the list is now empty, we tear down SOLIDWORKS
            if (!ActiveAddIns.Any())
                TearDown();
        }

        #endregion

        #region Tear Down

        /// <summary>
        /// Cleans up the SolidWorks instance
        /// </summary>
        private static void TearDown()
        {
            // If we have an reference...
            if (SolidWorks != null)
            {
                // Log it
                Logger?.LogDebugSource($"Disposing SolidWorks COM reference...");

                // Dispose SolidWorks COM
                SolidWorks?.Dispose();
            }

            // Set to null
            SolidWorks = null;
        }

        #endregion
    }
}
