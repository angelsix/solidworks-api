using AngelSix.SolidDna;

namespace SolidDna.DynamicLoadPlugIn
{
    /// <summary>
    /// Register as a SolidWorks Add-in
    /// </summary>
    public class SolidDnaAddinIntegration : AddInIntegration
    {
        /// <summary>
        /// Specific application start-up code
        /// </summary>
        public override void ApplicationStartup()
        {

        }

        /// <summary>
        /// Steps to take before any add-ins load
        /// </summary>
        /// <returns></returns>
        public override void PreLoadPlugIns()
        {

        }


        public override void PreConnectToSolidWorks()
        {
            // NOTE: To run in our own AppDomain do the following
            //       Be aware doing so sometimes causes API's to fail
            //       when they try to load dll's
            //
            //      To use, keep your SolidPlugIn implementation in separate
            //      projects, and just this add-in on its own
            //
            //      Right-click solution and select Configuration Manager
            //      Uncheck this project and AngelSix.SolidDNA "Build" box
            //      so building the project does not build them
            //      or alternatively when building right click the specific
            //      project containing the SolidPlugIn and select Build
            //
            //      Run the code as normal having this project as startup
            //      and running SLDWORKS.exe. When you want to make a change
            //      go in SolidWorks to Add-ins... and unload this one
            //
            //      Then in Visual Studio select Debug Detach All
            //      Now SolidWorks is still open, you can edit the SolidPlugIn
            //      project code, and re-build the projects, then select
            //      Debug > Attach to Process... and find SLDWORKS.exe
            //      and now Add-ins.. and load the add-in. All changes will
            //      be loaded in.
            //  
            AppDomainBoundary.UseDetachedAppDomain = true;
        }
    }
}
