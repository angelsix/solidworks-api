using Dna;
using System.IO;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A basic implementation of the AddIn Integration class used when registering the dll for COM.
    /// Mainly used for setting up DI so when loading the PlugIn's they have the expected services
    /// </summary>
    public class ComRegisterAddInIntegration : AddInIntegration
    {
        public override void ApplicationStartup()
        {

        }

        public override void ConfigureServices(FrameworkConstruction construction)
        {
            // Add file logger (will be in /bin/Debug/SolidDNA.ScriptRunner.com-register-log.txt)
            construction.AddFileLogger(Path.ChangeExtension(this.AssemblyFilePath(), "com-register-log.txt"));
        }

        public override void PreConnectToSolidWorks()
        {

        }

        public override void PreLoadPlugIns()
        {

        }
    }
}
