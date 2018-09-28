using System;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// An base class to implement to become a SolidDna plug-in
    /// The compiled dll of SolidDna must be in the same location as 
    /// the plug-in dll to be discovered
    /// </summary>
    public abstract class SolidPlugIn : MarshalByRefObject
    {
        /// <summary>
        /// Get's the desired title to show in the SolidWorks add-in
        /// </summary>
        /// <returns></returns>
        public abstract string AddInTitle { get; }

        /// <summary>
        /// Get's the desired description to show in the SolidWorks add-in
        /// </summary>
        /// <returns></returns>
        public abstract string AddInDescription { get; }

        /// <summary>
        /// Called when the add-in is loaded into SolidWorks and connected
        /// </summary>
        /// <returns></returns>
        public abstract void ConnectedToSolidWorks();

        /// <summary>
        /// Called when the add-in is unloaded from SolidWorks and disconnected
        /// </summary>
        /// <returns></returns>
        public abstract void DisconnectedFromSolidWorks();
    }
}
