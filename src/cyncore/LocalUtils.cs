using System;
using System.IO;
using System.Linq;

namespace CloudSync.Core
{
    public static class LocalUtils
    {
        public static void TryDeleteFile(string path)
        {
            try
            {
                if(File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception)
            {
            }
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public static bool CompareFiles(string p1, string p2)
        {
            var f1 = new FileInfo(p1);
            var f2 = new FileInfo(p2);
            if (f1.Length != f2.Length) return false;

            const int bufferSize = 64 * 1024;
            var buffer1 = new byte[bufferSize];
            var buffer2 = new byte[bufferSize];
            using (var fs1 = File.OpenRead(p1))
            {
                using (var fs2 = File.OpenRead(p2))
                {
                    var left = f1.Length;
                    while(left > 0)
                    {
                        var toRead = Math.Min(left, buffer1.Length);

                        var read1 = fs1.Read(buffer1, 0, (int)toRead);
                        var read2 = fs2.Read(buffer2, 0, (int)toRead);

                        if (read1 != read2)
                            return false;

                        var equal = buffer1.Take(read1).SequenceEqual(buffer2.Take(read2));
                        if (!equal)
                            return false;

                        left -= toRead;
                    }
                }
            }
            return true;
        }

        public static string GetTempFileName(string prefix = "cync", string suffix = "")
        {
            const int RETRIES = 4;
            var tempDir = Path.GetTempPath();
            for(var i = 0; i < RETRIES; ++i)
            {
                var path = Path.Combine(tempDir, $"{prefix}{Guid.NewGuid().ToString("N")}{suffix}");
                try
                {
                    using (var stream = File.OpenWrite(path))
                        return path;
                }
                catch(Exception)
                {
                }
            }
            throw new PathException($"Failed to create a temp file path in {RETRIES} retries.");
        }

        public static void CreateSecureFolder(string path)
        {
            if (Directory.Exists(path))
                return;

            Directory.CreateDirectory(path);
        }
    }
}