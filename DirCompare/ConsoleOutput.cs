using System.Collections.Generic;

namespace DirCompare
{
    public class ConsoleOutput : IOutput
    {
        public void Write(List<string> md5sums)
        {
            foreach (var line in md5sums)
            {
                System.Console.WriteLine(line);
            }
        }
    }
}
