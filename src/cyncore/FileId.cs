namespace CloudSync.Core
{
    public class FileId
    {
        string id;

        public FileId(string ss)
        {
            id = ss;
        }

        public string Get()
        {
            return id;
        }

        public string Directory()
        {
            return id.Substring(0, 2);
        }

        public string FileName()
        {
            return id.Substring(2);
        }

        public string FullPath
        {
            get
            {
                return ObjectTree.ObjectsDir + "/" + Directory() + "/" + FileName();
            }
        }

        public string DirectoryPath
        {
            get
            {
                return ObjectTree.ObjectsDir + "/" + Directory();
            }
        }
    }
}
