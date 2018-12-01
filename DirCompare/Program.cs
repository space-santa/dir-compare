using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace DirCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Now processing {args[0]}");
            ProcessDirectory(args[0]);
        }

        public static void ProcessDirectory(string path)
        {
            List<string> sums = new List<string>();
            foreach (string file in DirSearch(path))
            {
                sums.Add($"{GetConsistentPathWithoutBase(file, path)} {CalculateMD5(file)}");
            }
            sums.Sort();
            foreach (string line in sums)
            {
                Console.WriteLine(line);
            }
        }

        private static IEnumerable<string> DirSearch(string basePath)
        {
            var files = Directory.EnumerateFiles(basePath, "*.*", SearchOption.AllDirectories);
            return files;
        }

        private static string GetConsistentPathWithoutBase(string fullPath, string basePath)
        {
            // To compare the content of the directory we must remove the basePath.
            // It will be different which makes the diff useless.
            // We also need to have consistent path-separators for the same reason.
            var pathWithoutBase = fullPath.Replace(basePath, "");
            return EnsureNoLeadingSeparator(EnforceWindowsPathSeparator(pathWithoutBase));
        }

        private static string EnforceWindowsPathSeparator(string path)
        {
            return path.Replace("/", "\\");
        }

        private static string EnsureNoLeadingSeparator(string path)
        {
            if (path.StartsWith("\\"))
            {
                return path.Remove(0, 1);
            }
            return path;
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
