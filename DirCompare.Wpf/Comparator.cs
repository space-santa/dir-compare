
using System.Collections.Generic;

namespace DirCompare.Wpf
{
    class ListOutput : DirCompare.IOutput
    {
        public List<string> result { get; set; }

        public void Write(List<string> md5sums)
        {
            result = md5sums;
        }
    }

    public class Comparator
    {
        public static List<string> Compare(string folder1, string folder2)
        {
            var output = new ListOutput();

            var md5sums = new RecursiveMD5ListOfDirectory(folder1, output);
            if (folder2.Length > 0)
            {
                var secondSums = new RecursiveMD5ListOfDirectory(folder2, output);
                var diffSums = md5sums.Diff(secondSums);
                diffSums.Write();
            }
            else
            {
                md5sums.Write();
            }

            return output.result;
        }
    }
}
