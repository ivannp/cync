using System;

namespace CloudSync.Core
{
    public interface IObjectTree
    {
        void CreateRoot(ref Context context);
        void Pull(ref Context context, string src, string dest);
        void Push(ref Context context, string src, string dest);
        void List(ref Context context, string path, OutputConfig config);
        void Remove(ref Context context, string path);
    }
}
