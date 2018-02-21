using ColoredConsole;
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;

namespace copytree
{
    class Options
    {
        [Option('m', "max-size", Default = 1024, HelpText = "The maximum file size.")]
        public int MaxSize { get; set; }

        [Value(0)]
        public string Src { get; set; }

        [Value(1)]
        public string Dest { get; set; }
    }

    class Program
    {
        static void CopyTree(Options options)
        {
            var src = options.Src;
            var dest = options.Dest;

            var random = new Random();

            if (Directory.Exists(dest))
                throw new Exception($"The path '{dest}' exists.");

            Directory.CreateDirectory(dest);

            Queue<Tuple<string, string>> queue = new Queue<Tuple<string, string>>();
            queue.Enqueue(Tuple.Create(src, dest));
            while (queue.Count > 0)
            {
                var tuple = queue.Dequeue();

                // Process all directory entries. Files are added to the repo directly,
                // sub directories are queued for further processing.
                var entries = Directory.EnumerateFileSystemEntries(tuple.Item1);

                // Create the path if it doesn't exist
                if (tuple.Item2.Length > 0)
                {
                    Directory.CreateDirectory(tuple.Item2);
                }

                foreach (var entry in entries)
                {
                    FileInfo fi = new FileInfo(entry);
                    if ((fi.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        // Enque the directory for later processing
                        var destFullPath = Path.Combine(tuple.Item2, Path.GetFileName(entry));
                        queue.Enqueue(Tuple.Create(entry, destFullPath));
                    }
                    else
                    {
                        // A file - write some data
                        var fullDestPath = Path.Combine(tuple.Item2, Path.GetFileName(entry));
                        var len = random.Next(1, options.MaxSize);
                        var buffer = new byte[len];
                        File.WriteAllBytes(fullDestPath, buffer);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            // Copy a directory, but the file content is replaced
            // with random, short data.

            try
            {
                Parser.Default.ParseArguments<Options>(args)
                    .WithParsed<Options>(opts => CopyTree(opts))
                    .WithNotParsed(errs => { foreach (var e in errs) ColorConsole.WriteLine($"copytree encountered a fatal error: {e.ToString()}".OnDarkRed()); });
            }
            catch (Exception ee)
            {
                ColorConsole.WriteLine($"cync encountered a fatal error: {ee.ToString()}".OnDarkRed());
                if (ee.InnerException != null)
                    ColorConsole.WriteLine($"cync encountered a fatal error: {ee.ToString()}".OnDarkRed());
            }
        }
    }
}
