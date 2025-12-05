using System.Collections.Generic;

namespace namasdev.Core.Processing
{
    public class BatchArgs<T>
    {
        public int BatchNumber { get; set; }
        public IEnumerable<T> Items { get; set; }
        public bool CancelProcessing { get; set; }
    }
}
