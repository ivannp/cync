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
    }
}