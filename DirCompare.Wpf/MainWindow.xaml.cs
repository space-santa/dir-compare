using System;
using System.IO;
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

namespace DirCompare.Wpf
{
    public partial class MainWindow : Window
    {
        string _folder1;
        string _folder2;

        public MainWindow()
        {
            InitializeComponent();
            ResetResultListBox();
        }

        private void ResetResultListBox()
        {
            var tmp = new List<string>();
            tmp.Add("Waiting for comparison...");
            ResultListBox.ItemsSource = tmp;
        }

        private string GetFolderForComparison()
        {
            var dialog = new FolderDialog();
            return dialog.SelectFolder();
        }

        private void FolderOneButton_Click(object sender, RoutedEventArgs e)
        {
            _folder1 = GetFolderForComparison();
            UpdateFolderTextBoxes();
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

            var result = await Task.Run(() => Comparator.Compare(_folder1, _folder2));
            ResultListBox.ItemsSource = result;

            ComparisonProgressBar.Visibility = Visibility.Collapsed;
        }

        private void FolderTwoButton_Click(object sender, RoutedEventArgs e)
        {
            _folder2 = GetFolderForComparison();
            UpdateFolderTextBoxes();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ResetResultListBox();
            _folder1 = "";
            _folder2 = "";
            UpdateFolderTextBoxes();
        }

        private void UpdateFolderTextBoxes()
        {
            if (_folder1 != "")
            {
                FolderOneTextBox.Text = _folder1;
            }
            else
            {
                FolderOneTextBox.Text = "";
            }

            if (_folder2 != "")
            {
                FolderTwoTextBox.Text = _folder2;
            }
            else
            {
                FolderTwoTextBox.Text = "";
            }
        }

        private void SaveButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            var items = ResultListBox.Items;
            if (items.Count < 2)
            {
                return;
            }

            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "DirCompareResult";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "DirCompareResult | *.txt";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                var path = dlg.FileName;
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    foreach (var line in items)
                    {
                        sw.WriteLine(line);
                    }
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
