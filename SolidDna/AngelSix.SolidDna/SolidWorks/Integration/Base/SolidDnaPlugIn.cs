using System;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// An base class to implement to become a SolidDna plug-in. 
    /// The compiled dll of SolidDna must be in the same location as 
    /// the plug-in dll to be discovered
    /// </summary>
    public abstract class SolidPlugIn : MarshalByRefObject
    {
        #region Public Properties

        /// <summary>
        /// Gets the desired title to show in the SolidWorks add-in
        /// </summary>
        /// <returns></returns>
        public abstract string AddInTitle { get; }

        /// <summary>
        /// Gets the desired description to show in the SolidWorks add-in
        /// </summary>
        /// <returns></returns>
        public abstract string AddInDescription { get; }

        #endregion

        #region Public Methods

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

        #endregion
    }

    /// <summary>
    /// An base class to implement to become a SolidDna plug-in. 
    /// The compiled dll of SolidDna must be in the same location as 
    /// the plug-in dll to be discovered
    /// </summary>
    public abstract class SolidPlugIn<T> : SolidPlugIn
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SolidPlugIn() : base()
        {
            // Add any references from the parent plug-in project
            AppDomainBoundary.AddReferenceAssemblies<T>();

            // Disable discovering plug-in and make it quicker by auto-adding it
            PlugInIntegration.AutoDiscoverPlugins = false;

            // Add this plug-in
            PlugInIntegration.AddPlugIn<T>();
        }

        #endregion
    }
}
