using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System;

namespace DirCompare
{
    public class RecursiveMD5ListOfDirectory
    {
        private List<string> _pathsWithMD5Sum = new List<string>();
        private readonly string _basePath;

        public RecursiveMD5ListOfDirectory(string basePath)
        {
            _basePath = basePath;
            ProcessDirectory();
        }

        public RecursiveMD5ListOfDirectory(List<string> pathsWithMD5Sum)
        {
            _pathsWithMD5Sum = pathsWithMD5Sum;
        }

        public List<string> GetPathsWithMD5Sum()
        {
            return _pathsWithMD5Sum;
        }

        public RecursiveMD5ListOfDirectory Diff(RecursiveMD5ListOfDirectory rhs)
        {
            var aNotInB = _pathsWithMD5Sum.Except(rhs.GetPathsWithMD5Sum()).ToList();
            var bNotInA = rhs.GetPathsWithMD5Sum().Except(_pathsWithMD5Sum).ToList();
            List<string> diffList = new List<string>();
            diffList.Add("aNotInB");
            diffList = diffList.Concat(aNotInB).ToList();
            diffList.Add("bNotInA");
            diffList = diffList.Concat(bNotInA).ToList();
            return new RecursiveMD5ListOfDirectory(diffList);
        }

        private void ProcessDirectory()
        {
            _pathsWithMD5Sum.Clear();
            foreach (string file in DirSearch(_basePath))
            {
                _pathsWithMD5Sum.Add($"{PathProcessor.GetConsistentPathWithoutBase(file, _basePath)} {CalculateMD5(file)}");
            }
            _pathsWithMD5Sum.Sort();
        }

        private static IEnumerable<string> DirSearch(string basePath)
        {
            var files = Directory.EnumerateFiles(basePath, "*.*", SearchOption.AllDirectories);
            return files;
        }

        private static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
