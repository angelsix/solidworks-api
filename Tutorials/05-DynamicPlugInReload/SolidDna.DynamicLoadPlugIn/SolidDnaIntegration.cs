using AngelSix.SolidDna;
using Dna;

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
            PlugInIntegration.AddPlugIn(@"D:\git\solidworks-api\Tutorials\05-DynamicPlugInReload\SolidDna.DynamicLoadPlugIn\bin\Debug\SolidDna.DynamicLoadPlugIn.Main.dll");
        }

        public override void PreConnectToSolidWorks()
        {
            // NOTE: To run in our own AppDomain do the following
            //       Be aware doing so sometimes causes API's to fail
            //       when they try to load dll's
            //
            AppDomainBoundary.UseDetachedAppDomain = true;
        }

        public override void ConfigureServices(FrameworkConstruction construction)
        {
            base.ConfigureServices(construction);
        }
    }
}
