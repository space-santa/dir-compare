
using System.Collections.Generic;

namespace DirCompare.Wpf
{
    public class Comparator
    {
        public static List<string> Compare(string folder1, string folder2)
        {
            var md5sums = new RecursiveMD5ListOfDirectory(folder1);

            if (folder2.Length > 0)
            {
                var secondSums = new RecursiveMD5ListOfDirectory(folder2);
                var diffSums = md5sums.Diff(secondSums);
                return diffSums.GetPathsWithMD5Sum();
            }

            return md5sums.GetPathsWithMD5Sum();
        }
    }
}
