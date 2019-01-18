using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
            FolderOneTextBox.Text = _folder1.Path;
        }

        private async void FolderTwoButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            _folder2 = await GetFolderForComparisonAsync();
            FolderTwoTextBox.Text = _folder2.Path;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _folder1 = null;
            _folder2 = null;
            FolderOneTextBox.Text = "";
            FolderTwoTextBox.Text = "";
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private static async System.Threading.Tasks.Task<List<string>> GetMD5ListAsync(StorageFolder folder)
        {
            var files = await folder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName);
            var filesWithMD5 = new List<string>();

            foreach (var file in files)
            {
                var stream = await file.OpenStreamForReadAsync();
                filesWithMD5.Add($"{file.Path} - {CalculateMD5(stream)}");
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
