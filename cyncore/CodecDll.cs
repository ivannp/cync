using System.Runtime.InteropServices;

namespace CloudSync.Core
{
    public class CodecDll
    {
        [DllImport(@"encoding.dll")]
        public static extern void GenerateKey(byte[] buf, uint bufLen);

        [DllImport(@"encoding.dll")]
        public static extern void EncodeFile(byte[] buf, uint bufLen, byte[] hash, uint hashLen);

        [DllImport(@"encoding.dll")]
        public static extern void DecodeFile(byte[] buf, uint bufLen, byte[] hash, uint hashLen);

        [DllImport(@"encoding.dll")]
        public static extern void ComputeHash(byte[] buf, uint bufLen, byte[] hash, uint hashLen);
    }
}
