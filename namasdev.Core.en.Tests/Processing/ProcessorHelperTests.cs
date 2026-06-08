using System.Collections.Generic;
using namasdev.Core.Processing;
using Xunit;

namespace namasdev.Core.en.Tests.Processing
{
    public class ProcessorHelperTests
    {
        [Fact]
        public void ProcessInBatch_ProcessesAllItems()
        {
            var items = new[] { 1, 2, 3, 4, 5 };
            var processed = new List<int>();

            ProcessorHelper.ProcessInBatch(items, batchSize: 2, args =>
            {
                foreach (var item in args.Items)
                    processed.Add(item);
            });

            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, processed);
        }

        [Fact]
        public void ProcessInBatch_BatchNumbersAreSequential()
        {
            var items = new[] { 1, 2, 3, 4, 5 };
            var batchNumbers = new List<int>();

            ProcessorHelper.ProcessInBatch(items, batchSize: 2, args =>
            {
                batchNumbers.Add(args.BatchNumber);
            });

            Assert.Equal(new[] { 1, 2, 3 }, batchNumbers);
        }

        [Fact]
        public void ProcessInBatch_CancelProcessing_StopsEarly()
        {
            var items = new[] { 1, 2, 3, 4, 5 };
            var processed = new List<int>();

            ProcessorHelper.ProcessInBatch(items, batchSize: 2, args =>
            {
                foreach (var item in args.Items)
                    processed.Add(item);

                if (args.BatchNumber == 1)
                    args.CancelProcessing = true;
            });

            Assert.Equal(new[] { 1, 2 }, processed);
        }

        [Fact]
        public void ProcessInBatch_EmptyList_NeverCallsAction()
        {
            var called = false;

            ProcessorHelper.ProcessInBatch(new int[0], batchSize: 10, _ => called = true);

            Assert.False(called);
        }

        [Fact]
        public void ProcessInBatch_BatchSizeLargerThanList_SingleBatch()
        {
            var items = new[] { 1, 2 };
            int batchCount = 0;

            ProcessorHelper.ProcessInBatch(items, batchSize: 10, args => batchCount++);

            Assert.Equal(1, batchCount);
        }
    }
}
