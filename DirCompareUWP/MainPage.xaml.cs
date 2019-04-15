using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DirCompareUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        StorageFolder _folder1;
        StorageFolder _folder2;

        public MainPage()
        {
            this.InitializeComponent();
            ResetResultListBox();
        }

        private static async System.Threading.Tasks.Task<StorageFolder> GetFolderForComparisonAsync()
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            return folder;
        }

        private async void FolderOneButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            _folder1 = await GetFolderForComparisonAsync();
            UpdateFolderTextBoxes();
        }

        private async void FolderTwoButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            _folder2 = await GetFolderForComparisonAsync();
            UpdateFolderTextBoxes();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ResetResultListBox();
            _folder1 = null;
            _folder2 = null;
            UpdateFolderTextBoxes();
        }

        private void UpdateFolderTextBoxes()
        {
            FolderOneTextBox.Text = _folder1 != null ? _folder1.Path : "";
            FolderTwoTextBox.Text = _folder2 != null ? _folder2.Path : "";
        }

        private async void CompareButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            ResetResultListBox();

            if (_folder1 == null || _folder2 == null)
            {
                return;
            }

            ComparisonProgressBar.IsIndeterminate = true;
            ComparisonProgressBar.Visibility = Visibility.Visible;

            var files1 = await _folder1.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName);
            var files2 = await _folder2.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName);

            ComparisonProgressBar.IsIndeterminate = false;
            ComparisonProgressBar.Maximum = files1.Count + files2.Count;
            ComparisonProgressBar.Value = 0;

            var lhs = await GetMD5ListAsync(files1, _folder1);
            var rhs = await GetMD5ListAsync(files2, _folder2);

            ComparisonProgressBar.Visibility = Visibility.Collapsed;

            var aNotInB = lhs.Except(rhs).ToList();
            var bNotInA = rhs.Except(lhs).ToList();
            List<string> diffList = new List<string>();
            diffList.Add($"a = {_folder1.Path}");
            diffList.Add($"b = {_folder2.Path}");
            diffList.Add("-------------------");
            diffList.Add("aNotInB");
            diffList = diffList.Concat(aNotInB).ToList();
            diffList.Add("bNotInA");
            diffList = diffList.Concat(bNotInA).ToList();
            ResultListBox.ItemsSource = diffList;
        }

        private void ResetResultListBox()
        {
            var tmp = new List<string>();
            tmp.Add("Waiting for comparison...");
            ResultListBox.ItemsSource = tmp;
        }

        private async void SaveButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "New Document";
            StorageFile file = await savePicker.PickSaveFileAsync();

            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);
                // write to file
                await FileIO.WriteTextAsync(file, string.Join("\n", ResultListBox.Items.ToArray()));
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);

                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    SimpleMessagePopup($"File {file.Name} saved.");
                }
                else
                {
                    SimpleMessagePopup($"File {file.Name} couldn't be saved.");
                }
            }
        }

        private async void SimpleMessagePopup(string message)
        {
            var messageDialog = new MessageDialog(message);
            messageDialog.Commands.Add(new UICommand("Close"));
            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;
            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 0;
            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private async System.Threading.Tasks.Task<List<string>> GetMD5ListAsync(IReadOnlyList<StorageFile> files, StorageFolder folder)
        {
            var filesWithMD5 = new List<string>();

            foreach (var file in files)
            {
                var stream = await file.OpenStreamForReadAsync();
                filesWithMD5.Add($"{file.Path.Replace(folder.Path, "")} - {CalculateMD5(stream)}");
                ComparisonProgressBar.Value += 1;
            }

            return filesWithMD5;
        }

        private static string CalculateMD5(Stream file)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(file);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
