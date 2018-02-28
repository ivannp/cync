using Google.Protobuf;
using System;

namespace CloudSync.Core
{
    public class Context
    {
        public byte[] Key { set; get; }

        public bool UseChecksums { set; get; } = false;

        public RepoConfig RepoCfg { set; get; }
        public IStorage Storage { set; get; }

        public EncodingConfig EncodingCfg { set; get; }

        public Action<string> AlwaysWriteLine { get; set; } = (s) => Console.WriteLine(s);
        public Action<string> ErrorWriteLine { get; set; } = (s) => Console.WriteLine(s);
        public Action<string> InfoWriteLine { get; set; } = (s) => Console.WriteLine(s);

        public void InitEncodingConfig(bool force = false)
        {
            if(force || EncodingCfg == null)
            {
                EncodingCfg = new EncodingConfig { };
                EncodingCfg.CompressionLevel = RepoCfg.CompressionLevel;
                EncodingCfg.Key = ByteString.CopyFrom(Key);
                EncodingCfg.Ciphers.Add(RepoCfg.Ciphers);
            }
        }

        public void InitRepoFromStorage()
        {
            using (var localPath = Storage.Download(RepoConfig.ConfigFile))
                RepoCfg = RepoConfig.Load(localPath.Path);
        }

        public static Context CreateLocal(string path, int compressionLevel = 5, int maxVersions = 4, string ciphers = "aes")
        {
            Context context = new Context { RepoCfg = new RepoConfig(compressionLevel, maxVersions, ciphers) };
            context.Storage = new LocalStorage(path);
            return context;
        }

        public static Context OpenLocal(string path)
        {
            Context context = new Context { };
            context.Storage = new LocalStorage(path);
            context.InitRepoFromStorage();
            return context;
        }
    }
}
