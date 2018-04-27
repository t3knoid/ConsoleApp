using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConsoleWindow
{
    /// <summary>
    /// This class provides a way to execute a console process and capture its output
    /// to a WPF window.
    /// </summary>
    public partial class ConsoleApp : Window
    {
        public string Command { get; set; }
        private Process process = null;
        private int processID = 0;
        /// <summary>
        /// Make sure to set the command if using this default contructor.
        /// </summary>
        public ConsoleApp()
        {
            InitializeComponent();
        }
        /// <summary>
        /// This constructor allows 
        /// </summary>
        /// <param name="command">The command to execute</param>
        public ConsoleApp(string command)
        {
            this.Command = command;
            InitializeComponent();
        }

        /// <summary>
        /// Starts the specified command
        /// </summary>
        /// <returns>true if the command completed successfully</returns>
        public bool Start()
        {
            if (!File.Exists(this.Command))
                throw new FileNotFoundException(String.Format("{0} not found.", this.Command));

            this.process = new Process();
            try
            {
                process.StartInfo.FileName = this.Command;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardInput = true; // Is a MUST!
                process.EnableRaisingEvents = true;
                process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(process_OutputDataReceivedHandler);
                process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(process_ErrorDataReceivedHandler);
                process.Start();
                processID = process.Id;

                // Asynchronously read the standard output of the spawned process. 
                // This raises OutputDataReceived events for each line of output.
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            finally
            {
                if (process != null)
                {
                    process.Close();
                }
            }
            return true;
        }
        /// <summary>
        /// Error data receiver handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void process_ErrorDataReceivedHandler(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                tbConsoleOutput.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,new Action(() => { tbConsoleOutput.AppendText(e.Data + Environment.NewLine); tbConsoleOutput.ScrollToEnd(); }));
            }
        }
        /// <summary>
        /// Output data receiver handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void process_OutputDataReceivedHandler(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                tbConsoleOutput.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => { tbConsoleOutput.AppendText(e.Data + Environment.NewLine); tbConsoleOutput.ScrollToEnd(); }));
            }
            
        }
        /// <summary>
        /// Close button handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Checks if the associated process exists
        /// </summary>
        /// <returns></returns>
        private bool ProcessExists()
        {
            return Process.GetProcesses().Any(x => x.Id == processID);
        }
        /// <summary>
        /// Prompts the user if they really want to close the console window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Close", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                if (ProcessExists())
                {
                    Process p = Process.GetProcessById(processID);
                    p.Kill();

                }
            }
            else
                e.Cancel = true;

        }
    }
}
