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
        private readonly IOutput _output;

        public RecursiveMD5ListOfDirectory(string basePath, IOutput output)
        {
            _basePath = basePath;
            _output = output;
            ProcessDirectory();
        }

        public void Write()
        {
            _output.Write(_pathsWithMD5Sum);
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
