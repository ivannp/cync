using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace CloudSync.Core
{
    class OneDriveStorage : IStorage
    {
        public void CleanLocalFile(string path)
        {
            throw new NotImplementedException();
        }

        public void CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public FileInfo DefaultFileInfo(bool dir = true)
        {
            throw new NotImplementedException();
        }

        public LocalPath Download(string src)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> GetConfig()
        {
            throw new NotImplementedException();
        }

        public void RemoveFile(string path)
        {
            throw new NotImplementedException();
        }

        public void Upload(string src, string dest, bool finalizeLocal = true)
        {
            throw new NotImplementedException();
        }

        public JObject ToJson()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItemInfo> ListDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public ItemInfo GetItemInfo(string path)
        {
            throw new NotImplementedException();
        }

        public void Download(string src, string dest)
        {
            throw new NotImplementedException();
        }
    }
}
