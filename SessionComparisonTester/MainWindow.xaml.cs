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
using System.IO.Compression;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

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

        private void ButtonWebFile_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Choose an OrthoVis web file";
            //dlg.IsFolderPicker = true;
            //dlg.InitialDirectory = TextBoxWebFile.Text;  // uses default or recent directory if text is empty or invalid

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
                var webFile = dlg.FileName;
                LabelWebFile.Content = webFile;
                /* debug */
                //string outputZip = System.IO.Path.GetFileNameWithoutExtension(LabelWebFile.Content.ToString()) + ".zip";
                //string outputDir = System.IO.Path.GetDirectoryName(LabelWebFile.Content.ToString()) + "\\";
                //SetDebugTextAndClipboard(outputDir + outputZip);
                /* end debug */
            }
        }

        private void ButtonSessionFile_Click(object sender, RoutedEventArgs e)
        {
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
                /* debug */
                string outputZip = System.IO.Path.GetFileNameWithoutExtension(LabelSessionFile.Content.ToString()) + ".zip";
                string outputDir = System.IO.Path.GetDirectoryName(LabelSessionFile.Content.ToString()) + "\\";
                SetDebugTextAndClipboard(outputDir + outputZip);
                /* end debug */
            }
        }

        //private void ButtonDesktopToWeb_Click(object sender, RoutedEventArgs e)
        //{
        //    //string cmd = ssprintf("\"%s/R:\OrthoVis Releases\OtherTools\WebSessionConverter_latest\SessionConverter.exe\" \"%s/data\" \"%s\" \"%s.websession.zip\"", appPath.c_str(), appPath.c_str(), sessionFile.c_str(), sessionNoExtension.c_str());
        //    string fullPath = @"R:\OrthoVis Releases\OtherTools\WebSessionConverter_latest\SessionConverter.exe";
        //    ProcessStartInfo psi = new ProcessStartInfo();
        //    psi.FileName = System.IO.Path.GetFileName(fullPath);
        //    psi.WorkingDirectory = System.IO.Path.GetDirectoryName(fullPath);
        //    string outputZip = System.IO.Path.GetFileNameWithoutExtension(LabelSessionFile.Content.ToString()) + ".zip";
        //    string outputDir = System.IO.Path.GetDirectoryName(LabelSessionFile.Content.ToString()) + "\\";
        //    psi.Arguments = "\"" + LabelWebFile.Content.ToString() + "\" \"" + LabelSessionFile.Content.ToString() + "\" \"" + outputDir + outputZip + "\"";
        //    //Debugging
        //    SetDebugTextAndClipboard(outputDir + outputZip);
        //    // end debug
        //    Process.Start(psi);
        //}

        private void ButtonWebToDesktop_Click(object sender, RoutedEventArgs e)
        {
            //string startPath = @"c:\example\start";
            //string zipPath = @"c:\example\result.zip";
            //string extractPath = @"c:\example\extract";

            //System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath);

            string extractPath = System.IO.Path.GetDirectoryName(LabelSessionFile.Content.ToString()) + "\\" + System.IO.Path.GetFileNameWithoutExtension(LabelSessionFile.Content.ToString()) + "\\ExtractedZipFolder";

            //////////ZipFile.ExtractToDirectory(LabelDebug.Content.ToString(), extractPath);

            //SetDebugTextAndClipboard(System.IO.Path.GetDirectoryName(LabelSessionFile.Content.ToString()) + "\\" + System.IO.Path.GetFileNameWithoutExtension(LabelSessionFile.Content.ToString()) + "\\ExtractedZipFolder");
            
            using (StreamReader reader = File.OpenText(extractPath + @"\session.json"))
            {
                JObject data = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                
                int implantVersion = (int)data["implantVersion"];

                int implantInclination = (int)data["implantInclination"];

                int implantRoll = (int)data["implantRoll"];

                data["implantVersion"] = implantVersion + 5;
                data["implantInclination"] = implantInclination + 5;
                data["implantRoll"] = implantRoll + 5;

                JObject implantPosition = (JObject)data["implantPosition"];
                // ["Json.NET", "CodePlex"]

                float posX = (float)implantPosition["x"];
                float posY = (float)implantPosition["y"];
                float posZ = (float)implantPosition["z"];

                implantPosition["x"] = posX + 10.0f;
                implantPosition["y"] = posY + 10.0f;
                implantPosition["z"] = posZ + 10.0f;
                posX = (float)implantPosition["x"];

                SetDebugTextAndClipboard(String.Format("version: {0}, inclination: {1}, roll: {2}, posX: {3}, posY: {4}, posZ: {5}", implantVersion, implantInclination, implantRoll, posX, posY, posZ));

                string modifiedJson = @"c:\testSession.json";
                File.WriteAllText(modifiedJson, data.ToString());


                //string cmd = ssprintf("\"%s/R:\OrthoVis Releases\OtherTools\WebSessionConverter_latest\SessionConverter.exe\" \"%s/data\" \"%s\" \"%s.websession.zip\"", appPath.c_str(), appPath.c_str(), sessionFile.c_str(), sessionNoExtension.c_str());
                string fullPath = @"R:\OrthoVis Releases\OtherTools\WebSessionConverter_latest\SessionConverter.exe";
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = System.IO.Path.GetFileName(fullPath);
                psi.WorkingDirectory = System.IO.Path.GetDirectoryName(fullPath);
                //string outputZip = System.IO.Path.GetFileNameWithoutExtension(LabelSessionFile.Content.ToString()) + ".zip";
                //string outputDir = System.IO.Path.GetDirectoryName(LabelSessionFile.Content.ToString()) + "\\";

                psi.Arguments = "\"" + LabelWebFile.Content.ToString() + "\" \"" + modifiedJson + "\" \"" + LabelSessionFile.Content.ToString() + "\"";
                //Debugging
                // end debug
                Process.Start(psi);
            }


        }

        private void SetDebugTextAndClipboard(string text)
        {
            LabelDebug.Content = text;
            System.Windows.Clipboard.SetText(text);
        }
    }
}
