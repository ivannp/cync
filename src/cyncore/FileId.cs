namespace CloudSync.Core
{
    public class FileId
    {
        string _id;

        public FileId(string ss)
        {
            _id = ss;
        }

        public string Get()
        {
            return _id;
        }

        public string Directory { get { return _id.Substring(0, 2); } }
        public string FileName { get { return _id.Substring(2); } }

        public string FullPath
        {
            get
            {
                return ObjectTree.ObjectsDir + "/" + Directory + "/" + FileName;
            }
        }

        public string DirectoryPath
        {
            get
            {
                return ObjectTree.ObjectsDir + "/" + Directory;
            }
        }

        public override string ToString()
        {
            return _id;
        }
    }
}
