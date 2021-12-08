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

        public SolidAddIn ParentAddIn { get; set; }

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
        private SolidAddIn mParentAddIn;

        /// <summary>
        /// The add-in that contains this plugin.
        /// We override the default add-in property so we can call <see cref="AppDomainBoundary"/> and <see cref="PlugInIntegration"/> methods and include the generic T.
        /// </summary>
        public new SolidAddIn ParentAddIn
        {
            get => mParentAddIn;
            set
            {
                // If we already have an add-in, do not change it.
                if (mParentAddIn != null) return;
                mParentAddIn = value;
                
                // Once we have our parent add-in, we can call these methods.

                // Add any references from the parent plug-in project
                ParentAddIn.AppDomainBoundary.AddReferenceAssemblies<T>();

                // Disable discovering plug-in and make it quicker by auto-adding it
                ParentAddIn.PlugInIntegration.AutoDiscoverPlugins = false;

                // Add this plug-in
                ParentAddIn.PlugInIntegration.AddPlugIn<T>();
            }
        }

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SolidPlugIn() : base()
        {
            
        }

        #endregion
    }
}
