using System;
using System.Collections.Generic;
using System.Linq;

namespace namasdev.Core.Processing
{
    public class ProcessorHelper
    {
        public static void ProcessInBatch<T>(IEnumerable<T> list, int batchSize, Action<BatchArgs<T>> processAction)
        {
            BatchArgs<T> batchArgs;
            IEnumerable<T> items;
            int itemCount,
                number = 1;
            bool endOfList = false;
            while (!endOfList)
            {
                items = list.Skip((number - 1) * batchSize).Take(batchSize);
                itemCount = items.Count();

                if (itemCount > 0)
                {
                    batchArgs = new BatchArgs<T> { BatchNumber = number, Items = items };
                    processAction(batchArgs);

                    if (batchArgs.CancelProcessing)
                    {
                        break;
                    }
                }

                endOfList = itemCount == 0 || itemCount < batchSize;

                number++;
            }
        }
    }
}
