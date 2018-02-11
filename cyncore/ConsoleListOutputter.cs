using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudSync.Core
{
    public class ConsoleListOutputter
    {
        private const long Kb = 1024L;
        private const long Mb = 1024L * 1024;
        private const long Gb = 1024L * 1024 * 1024;
        private const long Tb = 1024L * 1024 * 1024 * 1024;
        private const long Pb = 1024L * 1024 * 1024 * 1024 * 1024;

        private string _columnSep = "  ";

        private readonly Action<string> _lineAction = s => Console.WriteLine(s);

        public ConsoleListOutputter(Action<string> lineAction = null)
        {
            if (lineAction != null)
                _lineAction = lineAction;
        }

        public void Output(IEnumerable<ItemInfo> items)
        {
            var attributeColumn = new OutputColumn();
            var nameColumn = new OutputColumn();
            var sizeColumn = new OutputColumn();
            var timeColumn = new OutputColumn();
            var itemList = new List<ItemInfo>();

            foreach (var item in items)
            {
                nameColumn.Add(item.Name);
                attributeColumn.Add(item.IsDir ? "d---------" : "----------");
                sizeColumn.Add((item.IsDir || !item.Size.HasValue) ? "---" : FormatSize(item.Size.Value));
                timeColumn.Add(item.LastWriteTime.Value.ToString("yyyy-MM-dd HH:mm"));
                itemList.Add(item);
            }

            var indexes = Enumerable.Range(0, nameColumn.Count).ToList();
            var comparer = new IndexComparer(itemList);
            indexes.Sort(comparer);

            foreach(var id in indexes)
            {
                var sb = new StringBuilder();
                sb.Append(attributeColumn.Items[id]);
                sb.Append(_columnSep);

                if (sizeColumn.Items[id].Length < sizeColumn.MaxWidth)
                    sb.Append(' ', sizeColumn.MaxWidth - sizeColumn.Items[id].Length);
                sb.Append(sizeColumn.Items[id]);
                sb.Append(_columnSep);

                if (timeColumn.Items[id].Length < timeColumn.MaxWidth)
                    sb.Append(' ', timeColumn.MaxWidth - timeColumn.Items[id].Length);
                sb.Append(timeColumn.Items[id]);
                sb.Append(_columnSep);

                sb.Append(nameColumn.Items[id]);

                _lineAction(sb.ToString());
            }
        }

        private class IndexComparer : IComparer<int>
        {
            private readonly List<ItemInfo> _items;

            public IndexComparer(List<ItemInfo> items)
            {
                _items = items;
            }

            public int Compare(int x, int y)
            {
                if(_items[x].IsDir)
                {
                    if (_items[y].IsDir)
                        return String.Compare(_items[x].Name, _items[y].Name);
                    return -1;
                }

                if(_items[y].IsDir)
                    return 1;

                return String.Compare(_items[x].Name, _items[y].Name);
            }
        }

        private string FormatSize(long size)
        {
            if(size < Kb)
            {
                return size.ToString();
            }
            else if(size < Mb)
            {
                return string.Format("{0:N0} KB", size / (double)Kb);
            }
            else if (size < Gb)
            {
                return string.Format("{0:N0} MB", size / (double)Mb);
            }
            else if (size < Tb)
            {
                return string.Format("{0:N0} GB", size / (double)Gb);
            }
            else if (size < Pb)
            {
                return string.Format("{0:N0} TB", size / (double)Tb);
            }
            else
            {
                return string.Format("{0:N0} PB", size / (double)Pb);
            }
        }
    }
}
