using Newtonsoft.Json.Linq;
using System;
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
            path = Path.Combine(RootPath, path);
            var items = new List<ItemInfo>();
            foreach (var entry in Directory.EnumerateFileSystemEntries(path))
            {
                var info = new FileInfo(entry);
                var isDir = (info.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
                items.Add(new ItemInfo { Name = info.Name, Size = isDir ? (long?)null : info.Length, IsDir = isDir, LastWriteTime = info.LastWriteTime });
            }
            return items;
        }

        public ItemInfo GetItemInfo(string path)
        {
            throw new System.NotImplementedException();
        }

        public void Move(string src, string dest)
        {
            if (Directory.Exists(src))
                Directory.Move(src, dest);
            else if (File.Exists(src))
                File.Move(src, dest);
            else
                throw new NotSupportedException($"Can't move '{src}'. Only files and directories are supported.");
        }
    }
}
