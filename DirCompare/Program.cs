using System;
using CommandLine;
using DirCompare.Lib;

namespace DirCompare
{
    class Program
    {
        class Options
        {
            [Option('o', "output", Default = "", HelpText = "File to write the result to. If this is not set, the result will be written to stdout.")]
            public string OutputFile { get; set; }

            [Value(0, Required = true, MetaName = "basedir", HelpText = "The directory that is scanned recursively.")]
            public string basedir { get; set; }

            [Value(1, Default = "", MetaName = "basedir 2", HelpText = "The other directory that is scanned. The results will be compared.")]
            public string secondBasedir { get; set; }
        }

        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    IOutput output;
                    if (o.OutputFile.Length > 0)
                    {
                        output = new FileOutput(o.OutputFile);
                    }
                    else
                    {
                        output = new ConsoleOutput();
                    }
                    var md5sums = new RecursiveMD5ListOfDirectory(o.basedir);
                    if (o.secondBasedir.Length > 0)
                    {
                        var secondSums = new RecursiveMD5ListOfDirectory(o.secondBasedir);
                        var diffSums = md5sums.Diff(secondSums);
                        output.Write(diffSums.GetPathsWithMD5Sum());
                    }
                    else
                    {
                        output.Write(md5sums.GetPathsWithMD5Sum());
                    }
                });
        }
    }
}
