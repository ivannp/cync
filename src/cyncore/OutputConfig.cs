using System;

namespace CloudSync.Core
{
    public class OutputConfig
    {
        public enum OutputType
        {
            Default,
            Long,
            None
        }

        public OutputType Type = OutputType.Default;
        public Action EndOfLineAction = () => Console.WriteLine();
        public Action<string, ItemInfo> ItemAction = (s, i) => Console.Write(s);

        public OutputConfig()
        { }
    }
}
