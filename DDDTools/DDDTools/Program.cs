using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace DDDTools
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 


        [STAThread]
        static void Main()
        {
            // Add exception handling for better error reporting and Windows 10 compatibility
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                // Log startup diagnostic information
                LogStartupInfo();
                
                // Check for required DLLs before starting
                string appDir = Application.StartupPath;
                string epplusPath = Path.Combine(appDir, "EPPlus.dll");
                string itextPath = Path.Combine(appDir, "itextsharp.dll");
                
                List<string> missingDlls = new List<string>();
                if (!File.Exists(epplusPath))
                    missingDlls.Add("EPPlus.dll");
                if (!File.Exists(itextPath))
                    missingDlls.Add("itextsharp.dll");
                
                if (missingDlls.Count > 0)
                {
                    string message = $"Required DLLs are missing:\n\n{string.Join("\n", missingDlls)}\n\n" +
                                   $"Please ensure these files are in the same folder as DDDTools.exe.\n" +
                                   $"Application directory: {appDir}";
                    MessageBox.Show(message, "Missing Dependencies", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                LogMessage("All required DLLs found, starting application...");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form2());
            }
            catch (Exception ex)
            {
                LogError("Main", ex);
                MessageBox.Show(
                    $"An error occurred while starting the application:\n\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}\n\nCheck DDDTools.log for details.",
                    "Application Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogError("Application_ThreadException", e.Exception);
            try
            {
                MessageBox.Show(
                    $"An unhandled exception occurred:\n\n{e.Exception.Message}\n\nStack Trace:\n{e.Exception.StackTrace}\n\nCheck DDDTools.log for details.",
                    "Application Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch
            {
                // If MessageBox fails, at least we logged it
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                LogError("CurrentDomain_UnhandledException", ex);
                try
                {
                    MessageBox.Show(
                        $"A fatal error occurred:\n\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}\n\nCheck DDDTools.log for details.",
                        "Fatal Application Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch
                {
                    // If MessageBox fails, at least we logged it
                }
            }
            else
            {
                LogError("CurrentDomain_UnhandledException", new Exception($"Non-Exception object: {e.ExceptionObject?.GetType().Name}"));
            }
        }

        static void LogError(string source, Exception ex)
        {
            try
            {
                string logPath = Path.Combine(Application.StartupPath, "DDDTools.log");
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{source}] {ex.GetType().Name}: {ex.Message}\nStack Trace:\n{ex.StackTrace}\n\n";
                File.AppendAllText(logPath, logEntry);
            }
            catch
            {
                // If logging fails, we can't do much
            }
        }

        static void LogMessage(string message)
        {
            try
            {
                string logPath = Path.Combine(Application.StartupPath, "DDDTools.log");
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [INFO] {message}\n";
                File.AppendAllText(logPath, logEntry);
            }
            catch { }
        }

        static void LogStartupInfo()
        {
            try
            {
                LogMessage("=== DDDTools Startup Diagnostics ===");
                LogMessage($"Application Version: {Assembly.GetExecutingAssembly().GetName().Version}");
                LogMessage($"Application Directory: {Application.StartupPath}");
                LogMessage($"Working Directory: {Environment.CurrentDirectory}");
                LogMessage($"OS Version: {Environment.OSVersion}");
                LogMessage($"OS Platform: {Environment.OSVersion.Platform}");
                LogMessage($"OS Service Pack: {Environment.OSVersion.ServicePack}");
                LogMessage($"Machine Name: {Environment.MachineName}");
                LogMessage($"User Name: {Environment.UserName}");
                LogMessage($"CLR Version: {Environment.Version}");
                LogMessage($"64-bit Process: {Environment.Is64BitProcess}");
                LogMessage($"64-bit OS: {Environment.Is64BitOperatingSystem}");
                
                // Get .NET Framework version
                string dotNetVersion = GetDotNetFrameworkVersion();
                LogMessage($".NET Framework Version: {dotNetVersion}");
                
                // Check DLLs
                string appDir = Application.StartupPath;
                LogMessage($"EPPlus.dll exists: {File.Exists(Path.Combine(appDir, "EPPlus.dll"))}");
                LogMessage($"itextsharp.dll exists: {File.Exists(Path.Combine(appDir, "itextsharp.dll"))}");
                
                // Try to get DLL versions if they exist
                try
                {
                    string epplusPath = Path.Combine(appDir, "EPPlus.dll");
                    if (File.Exists(epplusPath))
                    {
                        var epplusVersion = Assembly.LoadFrom(epplusPath).GetName().Version;
                        LogMessage($"EPPlus.dll Version: {epplusVersion}");
                    }
                }
                catch (Exception ex) { LogMessage($"Could not read EPPlus.dll version: {ex.Message}"); }
                
                try
                {
                    string itextPath = Path.Combine(appDir, "itextsharp.dll");
                    if (File.Exists(itextPath))
                    {
                        var itextVersion = Assembly.LoadFrom(itextPath).GetName().Version;
                        LogMessage($"itextsharp.dll Version: {itextVersion}");
                    }
                }
                catch (Exception ex) { LogMessage($"Could not read itextsharp.dll version: {ex.Message}"); }
                
                LogMessage("=== End Startup Diagnostics ===");
            }
            catch (Exception ex)
            {
                LogError("LogStartupInfo", ex);
            }
        }

        static string GetDotNetFrameworkVersion()
        {
            try
            {
                // Check registry for .NET Framework versions
                string frameworkVersion = Environment.Version.ToString();
                
                // Try to get more specific version from registry
                try
                {
                    using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
                    {
                        if (ndpKey != null)
                        {
                            int release = (int)ndpKey.GetValue("Release", 0);
                            string version = GetFrameworkVersionFromRelease(release);
                            if (!string.IsNullOrEmpty(version))
                            {
                                return $"{version} (Release: {release})";
                            }
                        }
                    }
                }
                catch { }
                
                return $"CLR {frameworkVersion}";
            }
            catch
            {
                return "Unknown";
            }
        }

        static string GetFrameworkVersionFromRelease(int release)
        {
            // .NET Framework release numbers
            if (release >= 533320) return "4.8";
            if (release >= 528040) return "4.8 (Preview)";
            if (release >= 461808) return "4.7.2";
            if (release >= 461308) return "4.7.1";
            if (release >= 460798) return "4.7";
            if (release >= 394802) return "4.6.2";
            if (release >= 394254) return "4.6.1";
            if (release >= 393295) return "4.6";
            if (release >= 379893) return "4.5.2";
            if (release >= 378675) return "4.5.1";
            if (release >= 378389) return "4.5";
            return null;
        }
    }
}
