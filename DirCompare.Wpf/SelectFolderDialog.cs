
namespace DirCompare.Wpf
{
    public class FolderDialog
    {
        System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;

        public FolderDialog()
        {
            folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the directory that you want to use as the default.";
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.Personal;
        }

        public string SelectFolder()
        {
            System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;
            }

            return "";
        }
    }
}
