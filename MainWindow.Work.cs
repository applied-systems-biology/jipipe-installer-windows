using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using File = System.IO.File;

namespace JIPipeInstaller
{
    public partial class MainWindow
    {
        public const string JRE_URL = "https://github.com/applied-systems-biology/jipipe-launcher/releases/download/current/openjdk-8-windows.zip";
        public const string UPDATER_URL = "https://github.com/applied-systems-biology/jipipe-launcher/releases/download/current/jipipe-launcher-updater.jar";

        private string GetInstancePath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appDataPath, "JIPipe", "launcher");
        }

        private string GetJavaBinPath()
        {
            return Path.Combine(GetInstancePath(), "jre", "bin", "javaw.exe");
        }

        private string GetUpdaterJarPath()
        {
            return Path.Combine(GetInstancePath(), "jipipe-launcher-updater.jar");
        }

        private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            string instancePath = GetInstancePath();

            if (Directory.Exists(instancePath))
            {
                backgroundWorker.ReportProgress(0, "Removing old installations ... (instances are preserved)");
                Directory.Delete(instancePath, true);
            }

            backgroundWorker.ReportProgress(10, "Creating in " + instancePath + " ...");
            Directory.CreateDirectory(instancePath);

            backgroundWorker.ReportProgress(20, "Creating in " + instancePath + " ... downloading JRE (Adoptium 8u402b06)");
            string jreZipFile = DownloadFile(JRE_URL, Path.GetTempFileName() + ".zip", "Creating in " + instancePath + " ... downloading JRE (Adoptium 8u402b06)", e);

            backgroundWorker.ReportProgress(60, "Creating in " + instancePath + " ... extracting JRE (Adoptium 8u402b06)");
            ZipFile.ExtractToDirectory(jreZipFile, instancePath);
            File.Delete(jreZipFile);
            if (backgroundWorker.CancellationPending)
            {
                return;
            }

            backgroundWorker.ReportProgress(80, "Creating in " + instancePath + " ... downloading updater");
            DownloadFile(UPDATER_URL, Path.Combine(instancePath, "jipipe-launcher-updater.jar"), "Creating in " + instancePath + " ... downloading updater", e);

            backgroundWorker.ReportProgress(90, "Creating desktop icon ...");
            if (backgroundWorker.CancellationPending)
            {
                return;
            }

            //Icon? appIcon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            string appIconPath = Path.Combine(instancePath, "jipipe.ico");
            SaveEmbeddedFile("JIPipeInstaller.icon.ico", appIconPath);

            // Save the icon to an ICO file
            //using (FileStream fs = new FileStream(Path.Combine(instancePath, "jipipe.ico"), FileMode.Create))
            //{
            //    appIcon?.Save(fs);
            //}

            string desktopIconPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "JIPipe.lnk");
            if (File.Exists(desktopIconPath))
            {
                File.Delete(desktopIconPath);
            }

            CreateShortcut(desktopIconPath,
                           GetJavaBinPath(),
                           "-jar " + GetUpdaterJarPath(),
                           appIconPath);

            backgroundWorker.ReportProgress(100, "Done. Launching now.");
            Thread.Sleep(2000);
        }

        static void SaveEmbeddedFile(string resourceName, string outputPath)
        {
            // Get the assembly where the file is embedded
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Load the file from the embedded resource
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                // Save the stream to a file
                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }


        static void CreateShortcut(string shortcutPath, string targetPath, string arguments, string iconPath)
        {
            // Create a new WshShellClass instance
            var shell = new WshShell();

            // Create a shortcut
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

            // Set shortcut properties
            shortcut.TargetPath = targetPath;
            shortcut.Arguments = arguments;
            shortcut.IconLocation = iconPath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);

            // Save the shortcut
            shortcut.Save();
        }

        private string DownloadFile(string url, string outputPath, string label, DoWorkEventArgs e)
        {
            using (WebClient webClient = new())
            {
                // Download the file
                webClient.DownloadProgressChanged += (s, args) =>
                {
                    // Report progress to the UI thread
                    backgroundWorker.ReportProgress(args.ProgressPercentage, $"{label}: {args.ProgressPercentage}%");
                };

                webClient.DownloadFileAsync(new Uri(url), outputPath);

                while (webClient.IsBusy)
                {
                    if (backgroundWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        return "";
                    }
                }

                Console.WriteLine($"File downloaded and stored in temporary location: {outputPath}");
                e.Result = outputPath;
            }
            return outputPath;
        }
    }
}
