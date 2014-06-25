using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Globalization;
using System.Reflection;
namespace OE110Prozessdatenbank
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {

        

        protected override void OnStartup(StartupEventArgs e)
        {

            //AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            
            base.OnStartup(e);

            new OE110Prozessdatenbank.MainWindow().ShowDialog();

        }
     

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                LogError(ex);

                MessageBox.Show("Entschuldigung, das hätte nicht passieren dürfen. Ein unerwarteter Fehler führte zum Absturz der Anwendung. Es wurde ein Fehlerbericht erstellt!\n\n" +
                        "Fehler:\n" +
                        ex.Message,
                    "Fataler Fehler", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        private string LogError(Exception exception)
        {
            Assembly caller = Assembly.GetEntryAssembly();
            Process thisProcess = Process.GetCurrentProcess();

            using (StreamWriter sw = new StreamWriter(@"ErrorLog.txt"))
            {
                sw.WriteLine("==============================================================================");
                sw.WriteLine(caller.FullName);
                sw.WriteLine("------------------------------------------------------------------------------");
                sw.WriteLine("Application Information");
                sw.WriteLine("------------------------------------------------------------------------------");
                sw.WriteLine("Program      : " + caller.Location);
                sw.WriteLine("Time         : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                sw.WriteLine("User         : " + Environment.UserName);
                sw.WriteLine("Computer     : " + Environment.MachineName);
                sw.WriteLine("OS           : " + Environment.OSVersion.ToString());
                sw.WriteLine("Culture      : " + CultureInfo.CurrentCulture.Name);
                sw.WriteLine("Processors   : " + Environment.ProcessorCount);
                sw.WriteLine("Working Set  : " + Environment.WorkingSet);
                sw.WriteLine("Framework    : " + Environment.Version);
                sw.WriteLine("Run Time     : " + (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString());
                sw.WriteLine("------------------------------------------------------------------------------");
                sw.WriteLine("Exception Information");
                sw.WriteLine("------------------------------------------------------------------------------");
                sw.WriteLine("Source       : " + exception.Source.ToString().Trim());
                sw.WriteLine("Method       : " + exception.TargetSite.Name.ToString());
                sw.WriteLine("Type         : " + exception.GetType().ToString());
                sw.WriteLine("Error        : " + GetExceptionStack(exception));
                sw.WriteLine("Stack Trace  : " + exception.StackTrace.ToString().Trim());
                sw.WriteLine("------------------------------------------------------------------------------");
                sw.WriteLine("Loaded Modules");
                sw.WriteLine("------------------------------------------------------------------------------");
                foreach (ProcessModule module in thisProcess.Modules)
                {
                    try
                    {
                        sw.WriteLine(module.FileName + " | " + module.FileVersionInfo.FileVersion + " | " + module.ModuleMemorySize);
                    }
                    catch (FileNotFoundException)
                    {
                        sw.WriteLine("File Not Found: " + module.ToString());
                    }
                    catch (Exception)
                    {

                    }
                }
                sw.WriteLine("------------------------------------------------------------------------------");
                sw.WriteLine("ErrorLog.txt");
                sw.WriteLine("==============================================================================");
            }

            return @"ErrorLog.txt";
        }

         private string GetExceptionStack(Exception e)
        {
            StringBuilder message = new StringBuilder();
            message.Append(e.Message);
            while (e.InnerException != null)
            {
                e = e.InnerException;
                message.Append(Environment.NewLine);
                message.Append(e.Message);
            }

            return message.ToString();
        }

    }
}
