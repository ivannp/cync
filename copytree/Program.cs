using System;
using System.Collections.Generic;
using System.IO;

namespace copytree
{
    class Program
    {
        static void Main(string[] args)
        {
            // Copy a directory, but the file content is replaced
            // with random, short data.

            var src = args[0];
            var dest = args[1];

            const int maxLen = 1024;
            var random = new Random();

            if (Directory.Exists(dest))
                throw new Exception($"The path '{dest}' exists.");

            Directory.CreateDirectory(dest);

            Queue<Tuple<string, string>> queue = new Queue<Tuple<string, string>>();
            queue.Enqueue(Tuple.Create(src, dest));
            while(queue.Count > 0)
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
                        var len = random.Next(1, maxLen);
                        var buffer = new byte[len];
                        File.WriteAllBytes(fullDestPath, buffer);
                    }
                }
            }
        }
    }
}
