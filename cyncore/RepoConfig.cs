using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace CloudSync.Core
{
    public class RepoConfig
    {
        public const string ConfigFile = "properties.json";

        public int Version { get; set; }
        public int CompressionLevel { get ; set ; }
        public int MaxVersions { get ; set; }
        public string[] Ciphers { get; set; }

        public RepoConfig(int compressionLevel, int maxVersions, string ciphers)
            : this()
        {
            CompressionLevel = compressionLevel;
            MaxVersions = maxVersions;
            Ciphers = Regex.Split(ciphers, "\\s*,\\s*");
        }

        public RepoConfig()
        {
            Version = 1 << 16;
        }

        public static RepoConfig Load(string path)
        {
            return JsonConvert.DeserializeObject<RepoConfig>(File.ReadAllText(path));
        }

        public void Save(string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
