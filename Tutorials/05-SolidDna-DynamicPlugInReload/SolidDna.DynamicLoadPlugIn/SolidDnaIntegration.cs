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
        /// Add our plug-ins
        /// </summary>
        /// <returns></returns>
        public override void PreLoadPlugIns()
        {
            PlugInIntegration.AddPlugIn(@"D:\git\angelsix\solidworks-api\Tutorials\05-SolidDna-DynamicPlugInReload\SolidDna.DynamicLoadPlugIn\bin\Debug\SolidDna.DynamicLoadPlugIn.Main.dll");
        }
    }
}
