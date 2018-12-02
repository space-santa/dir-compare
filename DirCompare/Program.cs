namespace DirCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            var md5sums = new RecursiveMD5ListOfDirectory(args[0], new ConsoleOutput());
            md5sums.Write();
        }
    }
}
