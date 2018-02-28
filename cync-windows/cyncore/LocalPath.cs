using System;
using System.IO;

namespace CloudSync.Core
{
    public class LocalPath : IDisposable
    {
        private bool _disposed = false;

        public string Path { get; set; }
        public bool DeleteOnExit { get; set; }

        public LocalPath(string path, bool deleteOnExit = true)
        {
            Path = path;
            DeleteOnExit = deleteOnExit;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (!DeleteOnExit)
                return;

            if (disposing)
            {
                try
                {
                    if (File.Exists(Path))
                        File.Delete(Path);
                }
                catch(Exception)
                {
                }
            }

            // Dispose unmanaged resources

            _disposed = true;
        }
    }
}
