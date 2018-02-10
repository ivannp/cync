using Google.Protobuf;
using System.IO;

namespace CloudSync.Core
{
    public class CodecHelper
    {
        public static void EncodeFile(ref Context context, string src, string dest, ref byte[] hash)
        {
            context.InitEncodingConfig();
            context.EncodingCfg.Src = src;
            context.EncodingCfg.Dest = dest;

            using (MemoryStream ms = new MemoryStream())
            {
                context.EncodingCfg.WriteTo(ms);
                CodecDll.EncodeFile(ms.GetBuffer(), (uint)ms.Length, hash, (uint)hash.Length);
            }
        }

        public static void DecodeFile(ref Context context, string src, string dest, ref byte[] hash)
        {
            context.InitEncodingConfig();
            context.EncodingCfg.Src = src;
            context.EncodingCfg.Dest = dest;

            using (MemoryStream ms = new MemoryStream())
            {
                context.EncodingCfg.WriteTo(ms);
                CodecDll.DecodeFile(ms.GetBuffer(), (uint)ms.Length, hash, (uint)hash.Length);
            }
        }

        public static void ComputeHash(string path, ref byte[] hash)
        {
            var b = new ComputeHashParams { };
            b.Hash = "sha256";
            b.Path = path;

            using (MemoryStream ms = new MemoryStream())
            {
                b.WriteTo(ms);
                CodecDll.ComputeHash(ms.GetBuffer(), (uint)ms.Length, hash, (uint)hash.Length);
            }
        }

        public static void GenerateKey(ref byte[] key)
        {
            CodecDll.GenerateKey(key, (uint)key.Length);
        }
    }
}
