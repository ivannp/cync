using System;
using System.Text;

namespace CloudSync.Core
{
    public class ProgressBar
    {
        private readonly string _start = "[";
        private readonly string _end = "]";
        private readonly string _fill = "#";
        private readonly string _empty = ".";
        private readonly int _width = 13;

        private readonly double _min;
        private readonly double _max;
        private readonly double _step;

        private double _nextUpdate;

        private bool _hasOutput = false;

        public string Text { get; set; }
        public bool Flush { get; set; } = false;

        private int _row;
        private int _col;

        private int _toDelete;

        public ProgressBar(double max, double step = 1, bool flush = false)
        {
            _max = max;
            _min = 0;
            _step = step;
            _nextUpdate = _min;

            Console.CursorVisible = false;

            _row = Console.CursorTop;
            _col = Console.CursorLeft;

            _toDelete = 0;

            Flush = flush;
        }

        public void Update(double progress)
        {
            if (progress < _min)
                progress = _min;

            if (progress > _max)
                progress = _max;

            if (_hasOutput && progress < _max && progress < _nextUpdate)
                return;

            //if (_hasOutput)
            //    Console.Write("\r");

            var fraction = (progress - _min) / (_max - _min);

            var sb = new StringBuilder();

            // Write the percentage and the start
            sb.Append(string.Format("{0,3:n0}%", Math.Floor(fraction * 100)));
            sb.Append("  " + _start);
            // Write the filling
            var graphWidth = _width * _fill.Length;
            var done = (int)(_width * fraction);
            var i = 0;
            for (; i < done; ++i)
                sb.Append(_fill);
            for (; i < _width; ++i)
                sb.Append(_empty);
            sb.Append(_end);

            if (!string.IsNullOrWhiteSpace(Text))
                sb.Append("  " + Text);

            if (sb.Length < _toDelete)
                sb.Append(' ', _toDelete - sb.Length);

            Console.CursorLeft = _col;
            Console.CursorTop = _row;
            Console.Write(sb.ToString());

            if (Flush)
                Console.Out.Flush();
            
            _hasOutput = true;

            _toDelete = sb.Length;

            if (progress > _nextUpdate)
                _nextUpdate += _step;
        }
    }
}
    