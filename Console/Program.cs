/********************************************
 *   Advanced C# Sample App                 *
 *                                          *
 *  Project created by: Vanja Petrović      *
 *  Date: 12.12.2015                        *
 *                                          *
 ********************************************/
using System;
using ConsoleApp.code;
using ConsoleApp.code.model;
using ConsoleApp.code.utils;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            #region Resolving and initalization
            // Hello Mistral
            Console.Write(Hello.SayHello());

            // Create Unity container
            IUnityContainer objContainer = new UnityContainer();
            // Load configuration file (App.config)
            objContainer.LoadConfiguration();
            // Register all types
            objContainer.RegisterType<Logger>();
            objContainer.RegisterType<LoggerDefault>();
            objContainer.RegisterType<MyDownload>();
            objContainer.RegisterType<Execute>();
            objContainer.RegisterType<LocalStorage>();

            // Resolve logger (from config)
            var logger = objContainer.Resolve<ILogger>("LoggerWithTime");
            // Register logger instance 
            objContainer = objContainer.RegisterInstance(logger);

            var localStorage = objContainer.Resolve<ILocalStorage>("LocalStorage");
            objContainer = objContainer.RegisterInstance(localStorage);

            // Resolve downloader
            var downloader = objContainer.Resolve<MyDownload>();

            // Resolve execute component
            var exec = objContainer.Resolve<Execute>();
            #endregion

            // Check for updates 
            downloader.CheckForUpdate<RootResponse>(Constants.UpdateURL);
            // Download updates
            downloader.DownloadUpdates();
            // Execute plugins
            exec.ExecuteAllDlls();

        }

        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("The following error occurred:");
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
