using System;
using System.Collections.Generic;

namespace CloudSync.Core
{
    public class OutputColumn
    {
        public List<string> Items { get; }  = new List<string>();
        public int Count => Items.Count;
        public int MaxWidth { get; private set; } = 0;

        public void Add(string s)
        {
            Items.Add(s);
            MaxWidth = Math.Max(MaxWidth, s.Length);
        }
    }
}
