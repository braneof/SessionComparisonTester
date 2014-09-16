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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;

namespace SessionComparisonTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonDataDir_Click(object sender, RoutedEventArgs e)
        {
            // Run the tests
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Select the data directory";
            dlg.IsFolderPicker = true;
            //dlg.InitialDirectory = TextBoxDataDir.Text;  // uses default or recent directory if text is empty or invalid

            //dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = Environment.CurrentDirectory;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var folder = dlg.FileName;
                LabelDataDir.Content = folder;
            }
        }

        private void ButtonSessionFile_Click(object sender, RoutedEventArgs e)
        {
            // Run the tests
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Choose an OrthoVis session file";
            //dlg.IsFolderPicker = true;
            //dlg.InitialDirectory = TextBoxSessionFile.Text;  // uses default or recent directory if text is empty or invalid

            //dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = Environment.CurrentDirectory;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var sessionFile = dlg.FileName;
                LabelSessionFile.Content = sessionFile;
            }
        }

        private void ButtonDesktopToWeb_Click(object sender, RoutedEventArgs e)
        {
            //string cmd = ssprintf("\"%s/R:\OrthoVis Releases\OtherTools\WebSessionConverter_latest\SessionConverter.exe\" \"%s/data\" \"%s\" \"%s.websession.zip\"", appPath.c_str(), appPath.c_str(), sessionFile.c_str(), sessionNoExtension.c_str());
            string fullPath = @"R:\OrthoVis Releases\OtherTools\WebSessionConverter_latest\SessionConverter.exe";
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = System.IO.Path.GetFileName(fullPath);
            psi.WorkingDirectory = System.IO.Path.GetDirectoryName(fullPath);
            string outputZip = System.IO.Path.GetFileNameWithoutExtension(LabelSessionFile.Content.ToString()) + ".zip";
            string outputDir = System.IO.Path.GetDirectoryName(LabelSessionFile.Content.ToString()) + "\\";
            psi.Arguments = "\"" + LabelDataDir.Content.ToString() + "\" \"" + LabelSessionFile.Content.ToString() + "\" \"" + outputDir + outputZip + "\"";
            //Debugging
            SetDebugTextAndClipboard(psi.Arguments);
            // end debug
            Process.Start(psi);
        }

        private void ButtonButtonWebToDesktop_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SetDebugTextAndClipboard(string text)
        {
            LabelDebug.Content = text;
            System.Windows.Clipboard.SetText(text);
        }
    }
}
