using System;
using System.IO;

namespace CloudSync.Core
{
    public static class LocalUtils
    {
        public static void TryDeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
            }
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
    }
}