using AngelSix.SolidDna;
using System;
using static AngelSix.SolidDna.SolidWorksEnvironment;

namespace SolidDna.StandAlone
{
    class Program
    {
        static void Main(string[] args)
        {
            // 
            //  *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
            //
            //     Welcome to SolidDNA by AngelSix
            //
            //        SolidDNA is a modern framework designed to make developing SolidWorks Add-ins easy.
            //
            //        With this template you have a ready-to-go stand-alone console application that
            //        has access to all the SolidDNA API.
            //
            //        Simply open up SolidWorks and when ready, run this project to connect to the instance
            //        and run your code without having to restart SolidWorks.
            //
            //        A bunch of useful example projects available here 
            //        https://github.com/angelsix/solidworks-api/tree/develop/Tutorials
            //
            // 
            //  *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-

            Console.WriteLine("Connecting to active SolidWorks instance...");

            // If we fail to connect to the active SolidWorks intance...
            if (!AddInIntegration.ConnectToActiveSolidWorks())
            {
                // Tell the user it failed and exit
                Console.WriteLine("Unable to connect to active SolidWorks instance");
                Console.WriteLine("Press any key to exit");
                Console.ReadLine();
                return;
            }

            // Let user know we are about to say hello
            Console.WriteLine("Saying hello in SolidWorks...");

            // Do something in SolidWorks :)
            Application.ShowMessageBox("Hello from Stand Alone application");

            Console.WriteLine("Done. Press any key to exit");
            Console.ReadLine();

            // Clean up once done
            AddInIntegration.TearDown();
        }
    }
}
