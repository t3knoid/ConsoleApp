using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConsoleWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btBrowse_Click(object sender, RoutedEventArgs e)
        {
            dialog.ShowDialog();
            tbCommand.Text = dialog.FileName;
        }

        private void tbExecute_Click(object sender, RoutedEventArgs e)
        {
            ConsoleApp consoleapp = new ConsoleApp();
            consoleapp.Command = tbCommand.Text;
            consoleapp.Owner = this;
            consoleapp.Show();
            consoleapp.Start();
        }
    }
}
