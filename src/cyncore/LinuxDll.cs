using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CloudSync.Core
{
    public class LinuxDll : IDll, IDisposable
    {
        private IntPtr _handle;
        private bool _disposed = false;

        private const int RTLD_NOW = 2;

        [DllImport("libdl.so.2")]
        private static extern IntPtr dlopen(byte [] path, int flags);

        [DllImport("libdl.so.2")]
        private static extern IntPtr dlsym(IntPtr handle, byte [] name);

        [DllImport("libdl.so.2")]
        private static extern int dlclose(IntPtr handle);

        [DllImport("libdl.so.2")]
        private static extern IntPtr dlerror();

        public LinuxDll(string path)
        {
            var encoding = new UTF8Encoding();
            dlerror();
            _handle = dlopen(encoding.GetBytes(path), RTLD_NOW);
            if (_handle == IntPtr.Zero)
                throw new FileNotFoundException($"Failed to load {path}");
        }

        ~LinuxDll()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        public Delegate GetDelegate<T>(string name)
        {
            var encoding = new UTF8Encoding();
            dlerror();
            var result = dlsym(_handle, encoding.GetBytes(name));
            if (result == IntPtr.Zero)
                throw new Exception($"Error loading dll '{name}' functionality.");
            return Marshal.GetDelegateForFunctionPointer(result, typeof(T));
        }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.

            if (_handle != IntPtr.Zero)
            {
                dlclose(_handle);
                _handle = IntPtr.Zero;
            }

            _disposed = true;
        }
    }
}
