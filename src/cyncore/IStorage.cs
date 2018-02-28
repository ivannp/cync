using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace CloudSync.Core
{
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
