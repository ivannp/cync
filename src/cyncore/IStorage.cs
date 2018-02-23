using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace CloudSync.Core
{
    public class LocalPath
    {
        public string Path { get; set; }
        public bool DeleteOnExit { get; set; }

        public LocalPath(string path, bool deleteOnExit = true)
        {
            Path = path;
            DeleteOnExit = deleteOnExit;
        }

        ~LocalPath()
        {
            if(DeleteOnExit && File.Exists(Path))
            {
                try
                {
                    File.Delete(Path);
                }
                catch(Exception)
                {
                }
            }
        }
    }

    public interface IStorage
    {
        LocalPath Download(string src);
        void Download(string src, string dest);

        void CleanLocalFile(string path);

        void CreateDirectory(string path);

        void Upload(string src, string dest, bool finalizeLocal = true);

        void RemoveFile(string path);

        void Move(string src, string dest);

        JObject ToJson();

        IEnumerable<ItemInfo> ListDirectory(string path);
        ItemInfo GetItemInfo(string path);
    }
}
