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
                sums.Add($"{PathProcessor.GetConsistentPathWithoutBase(file, path)} {CalculateMD5(file)}");
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
