using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace CloudSync.Core
{
    public class LocalStorage : IStorage
    {
        string ObjectsPath;

        string rootPath;
        string RootPath {
            get { return rootPath; }
            set
            {
                rootPath = value;
                ObjectsPath = LexicalPath.Combine(rootPath, "objects");
            }
        }

        public LocalStorage(string rootPath)
        {
            RootPath = rootPath;
        }

        public void CleanLocalFile(string path)
        {
        }

        public LocalPath Download(string src)
        {
            return new LocalPath(LexicalPath.Combine(RootPath, src), false);
        }

        public void Download(string src, string dest)
        {
            File.Copy(src, dest);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(LexicalPath.Combine(RootPath, path));
        }

        public void Upload(string src, string dest, bool finalizeLocal = true)
        {
            var pp = LexicalPath.Combine(RootPath, dest);

            if (File.Exists(pp)) File.Delete(pp);

            if (finalizeLocal) File.Move(src, pp);
            else File.Copy(src, pp);
        }

        public FileInfo DefaultFileInfo(bool dir = true)
        {
            if (dir) return new FileInfo(RootPath);
            else return new FileInfo(LexicalPath.Combine(RootPath, RepoConfig.ConfigFile));
        }

        public void RemoveFile(string path)
        {
            var pp = LexicalPath.Combine(RootPath, path);
            if (File.Exists(pp)) File.Delete(pp);
        }

        public JObject ToJson()
        {
            var res = new JObject();
            res["Type"] = "Local";
            res["Path"] = RootPath;
            return res;
        }

        public IEnumerable<ItemInfo> ListDirectory(string path)
        {
            throw new System.NotImplementedException();
        }

        public ItemInfo GetItemInfo(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
