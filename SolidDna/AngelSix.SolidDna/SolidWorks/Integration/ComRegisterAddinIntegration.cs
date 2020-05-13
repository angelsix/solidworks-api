using Dna;
using System;
using System.Diagnostics;
using System.IO;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A basic implementation of the AddIn Integration class used when registering the dll for COM.
    /// Mainly used for setting up DI so when loading the PlugIn's they have the expected services
    /// </summary>
    public class ComRegisterAddInIntegration : AddInIntegration
    {
        public ComRegisterAddInIntegration()
        {
            try
            {
                // As for COM Registration this won't get ConnectedToSW called
                // and thereby no call to setup IoC, we do this manually here
                // Setup application (allowing for AppDomain boundary setup)
                AppDomainBoundary.Setup(this.AssemblyPath(), this.AssemblyFilePath(), typeof(ComRegisterAddInIntegration).Assembly.AssemblyFilePath(), "");
            }
            catch (Exception ex)
            {
                Debugger.Break();

                // Fall-back just write a static log directly
                File.AppendAllText(Path.ChangeExtension(this.AssemblyFilePath(), "fatal.log.txt"), $"\r\nUnexpected error: {ex}");
            }
        }

        public override void ApplicationStartup()
        {

        }

        public override void PreConnectToSolidWorks()
        {

        }

        public override void PreLoadPlugIns()
        {

        }

        [ConfigureService]
        public override void ConfigureServices(FrameworkConstruction construction)
        {
            base.ConfigureServices(construction);

            // Add file logger to output log txt when registering for COM, with same name as dll plus "com-register-log.txt"
            var logPath = Path.ChangeExtension(this.AssemblyFilePath(), "com-register-log.txt");
            construction.AddFileLogger(logPath);
        }
    }
}
