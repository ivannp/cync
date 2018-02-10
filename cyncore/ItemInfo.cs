using System;

namespace CloudSync.Core
{
    public sealed class ItemInfo
    {
        public int? Attributes { get; set; }
        public DateTime? CreationTime { get; set; }
        public DateTime? LastAccessTime { get; set; }
        public DateTime? LastWriteTime { get; set; }
        public long? Size { get; set; }
        public bool IsDir { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
