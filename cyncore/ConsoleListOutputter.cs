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

        private readonly OutputConfig _config = new OutputConfig();

        public ConsoleListOutputter(OutputConfig config = null)
        {
            if (config != null)
                _config = config;
        }

        public void Output(IEnumerable<ItemInfo> items)
        {
            if (_config.Type == OutputConfig.OutputType.Long)
                LongOutput(items);
            else DefaultOutput(items);
        }

        private void LongOutput(IEnumerable<ItemInfo> items)
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

                _config.ItemAction(sb.ToString(), itemList[id]);
                _config.EndOfLineAction();
            }
        }

        private void DefaultOutput(IEnumerable<ItemInfo> items)
        {
            var nameColumn = new OutputColumn();
            var itemList = new List<ItemInfo>();

            foreach (var item in items)
            {
                nameColumn.Add(item.Name);
                itemList.Add(item);
            }

            if (nameColumn.Count == 0)
                return;

            var indexes = Enumerable.Range(0, nameColumn.Count).ToList();
            var comparer = new IndexComparer(itemList, foldersFirst: false);
            indexes.Sort(comparer);

            // Figure out how many columns can fit the output
            var maxWidth = Console.WindowWidth;
            var numColumns = Math.Min(15, nameColumn.Count);

            var columnWidths = new List<int>();

            var done = false;

            while (numColumns > 1)
            {
                columnWidths.Clear();

                // Compute the window with necessary to display the output in numColumns columns
                var numRows = (nameColumn.Count + numColumns - 1) / numColumns;
                var numFullColumns = nameColumn.Count - (numRows - 1) * numColumns; // The number of items in the last row, in other words
                var itemId = 0;
                for (var i = 0; i < numColumns; ++i)
                {
                    var thisColumnNum = i < numFullColumns ? numRows : (numRows - 1);
                    var thisColumnWidth = 0;
                    for(var j = 0; j < thisColumnNum; ++j)
                    {
                        thisColumnWidth = Math.Max(thisColumnWidth, nameColumn.Items[indexes[itemId]].Length);
                        ++itemId;
                    }
                    columnWidths.Add(thisColumnWidth);
                }

                var width = columnWidths.Sum() + (columnWidths.Count - 1)*_columnSep.Length;
                if (width < maxWidth)
                {
                    var rowsData = new List<int>[numRows];
                    for (var i = 0; i < rowsData.Length; ++i)
                        rowsData[i] = new List<int>();
                    itemId = 0;
                    for(var i = 0; i < columnWidths.Count; ++i)
                    {
                        var thisColumnNum = i < numFullColumns ? numRows : (numRows - 1);
                        for(var j = 0; j < thisColumnNum; ++j)
                        {
                            rowsData[j].Add(indexes[itemId]);
                            ++itemId;
                        }
                    }

                    for(var i = 0; i < numRows; ++i)
                    {
                        for(var j = 0; j < rowsData[i].Count; ++j)
                        {
                            if (j > 0)
                                _config.ItemAction(_columnSep, null);
                            var str = nameColumn.Items[rowsData[i][j]];
                            _config.ItemAction(str, itemList[rowsData[i][j]]);
                            if (str.Length < columnWidths[j])
                                for(var k = 0; k < (columnWidths[j] - str.Length); ++k)
                                    _config.ItemAction(" ", null);
                        }
                        _config.EndOfLineAction();
                    }

                    done = true;
                    break;
                }

                --numColumns;
            }

            if(!done)
            {
                // Single column output
                for (var i = 0; i < nameColumn.Count; ++i)
                    Console.WriteLine(nameColumn.Items[indexes[i]]);
            }
        }

        private class IndexComparer : IComparer<int>
        {
            private readonly bool _foldersFirst;
            private readonly List<ItemInfo> _items;

            public IndexComparer(List<ItemInfo> items, bool foldersFirst = true)
            {
                _items = items;
                _foldersFirst = foldersFirst;
            }

            public int Compare(int x, int y)
            {
                if(_foldersFirst)
                {
                    if (_items[x].IsDir)
                    {
                        if (_items[y].IsDir)
                            return String.Compare(_items[x].Name, _items[y].Name);
                        return -1;
                    }

                    if (_items[y].IsDir)
                        return 1;

                    return String.Compare(_items[x].Name, _items[y].Name);
                }

                return String.Compare(_items[x].Name, _items[y].Name);
            }
        }

        private string FormatSize(long size)
        {
            if(size < Kb)
                return size.ToString();
            else if(size < Mb)
                return string.Format("{0:N0} KB", size / (double)Kb);
            else if (size < Gb)
                return string.Format("{0:N0} MB", size / (double)Mb);
            else if (size < Tb)
                return string.Format("{0:N0} GB", size / (double)Gb);
            else if (size < Pb)
                return string.Format("{0:N0} TB", size / (double)Tb);
            else
                return string.Format("{0:N0} PB", size / (double)Pb);
        }
    }
}
