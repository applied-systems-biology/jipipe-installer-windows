using System.ComponentModel;
using System.Diagnostics;

namespace JIPipeInstaller
{
    public partial class MainWindow : Form
    {
        private BackgroundWorker backgroundWorker;
        public MainWindow()
        {
            InitializeComponent();
            InitializeBackgroundWorker();

            backgroundWorker.RunWorkerAsync();
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

           
        }

        private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Cancelled)
            {
                MessageBox.Show("Operation was cancelled.", "JIPipe Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(e.Error != null)
            {
                MessageBox.Show("An error occured: " + e.Error.Message, "JIPipe Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                StartUpdater();
            }
            Application.Exit();
        }

        private void StartUpdater()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = GetJavaBinPath(),
                Arguments = "-jar " + GetUpdaterJarPath(),
                CreateNoWindow = false,
                UseShellExecute = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false
            };

            Process process = new Process
            {
                StartInfo = processStartInfo
            };

            process.Start();
        }

        private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            statusLabel.Text = e.UserState?.ToString();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            cancelButton.Enabled = false;
            backgroundWorker.CancelAsync();
        }
    }
}
