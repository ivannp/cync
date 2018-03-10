using System;
using System.Runtime.InteropServices;

namespace CloudSync.Core
{
    public class CodecDll : IDisposable
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GenerateKeyDelegate(byte[] buf, uint bufLen);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void EncodeFileDelegate(byte[] buf, uint bufLen, byte[] hash, uint hashLen);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DecodeFileDelegate(byte[] buf, uint bufLen, byte[] hash, uint hashLen);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ComputeHashDelegate(byte[] buf, uint bufLen, byte[] hash, uint hashLen);

        private bool _disposed = false;

        private readonly IDll _dll;

        private GenerateKeyDelegate _generateKey;
        private EncodeFileDelegate _encodeFile;
        private DecodeFileDelegate _decodeFile;
        private ComputeHashDelegate _computeHash;

        private static CodecDll _instance { get; } = new CodecDll();

        private CodecDll()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                _dll = new WindowsDll("encoding.dll");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                _dll = new LinuxDll("encoding.so");
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                throw new NotSupportedException($"MaxOS is not supported yet");

            _generateKey = (GenerateKeyDelegate)_dll.GetDelegate<GenerateKeyDelegate>("GenerateKey");
            _encodeFile = (EncodeFileDelegate)_dll.GetDelegate<EncodeFileDelegate>("EncodeFile");
            _decodeFile = (DecodeFileDelegate)_dll.GetDelegate<DecodeFileDelegate>("DecodeFile");
            _computeHash = (ComputeHashDelegate)_dll.GetDelegate<ComputeHashDelegate>("ComputeHash");
        }

        public static void GenerateKey(byte[] buf, uint bufLen)
        {
            _instance._generateKey(buf, bufLen);
        }

        public static void EncodeFile(byte[] buf, uint bufLen, byte[] hash, uint hashLen)
        {
            _instance._encodeFile(buf, bufLen, hash, hashLen);
        }

        public static void DecodeFile(byte[] buf, uint bufLen, byte[] hash, uint hashLen)
        {
            _instance._decodeFile(buf, bufLen, hash, hashLen);
        }

        public static void ComputeHash(byte[] buf, uint bufLen, byte[] hash, uint hashLen)
        {
            _instance._computeHash(buf, bufLen, hash, hashLen);
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
                (_dll as IDisposable).Dispose();

            // Free any unmanaged objects here.

            _disposed = true;
        }
    }
}
