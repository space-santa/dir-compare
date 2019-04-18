using System.Collections.Generic;
using System.IO;

namespace DirCompare.Lib
{
    public class FileOutput : IOutput
    {
        private readonly string _path;

        public FileOutput(string path)
        {
            _path = path;
        }

        public void Write(List<string> md5sums)
        {
            using (StreamWriter sw = new StreamWriter(_path, false))
            {
                foreach (var line in md5sums)
                {
                    sw.WriteLine(line);
                }
            }
        }
    }
}
