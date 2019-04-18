using DirCompare.Lib;
using System.Collections.Generic;

namespace DirCompare.Wpf
{
    public class Comparator
    {
        private static List<string> ListHeader(string folder1, string folder2)
        {
            List<string> listHeader = new List<string>();
            listHeader.Add($"a = {folder1}");
            listHeader.Add($"b = {folder2}");
            listHeader.Add("-------------------");
            return listHeader;
        }
        public static List<string> Compare(string folder1, string folder2)
        {
            var md5sums = new RecursiveMD5ListOfDirectory(folder1);
            List<string> diffList;

            if (folder2.Length > 0)
            {
                var secondSums = new RecursiveMD5ListOfDirectory(folder2);
                var diffSums = md5sums.Diff(secondSums);
                diffList = diffSums.GetPathsWithMD5Sum();
            }
            else
            {
                diffList = md5sums.GetPathsWithMD5Sum();
            }

            var comparedList = ListHeader(folder1, folder2);
            comparedList.AddRange(diffList);
            return comparedList;
        }
    }
}
