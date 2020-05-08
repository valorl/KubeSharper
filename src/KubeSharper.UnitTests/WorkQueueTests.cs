using KubeSharper.WorkQueue;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KubeSharper.UnitTests
{
    [Collection("WorkQueue tests")]
    public class WorkQueueTests
    {
        private readonly IWorkQueue<string> _sut = new WorkQueue<string>();
        [Fact(DisplayName = "TryAdd should add item if item does not already exist.")]
        public async Task WorkQueue_TryAdd_When_ItemNew_Should_AddItem()
        {
            var item = "item";
            var result = await _sut.TryAdd(item);

            Assert.True(result);
            Assert.Equal(1, _sut.Count);
        }
        [Fact(DisplayName = "TryAdd should not add item if item already exist.")]
        public async Task WorkQueue_TryAdd_When_ItemDuplicate_Should_NotAddItem()
        {
            var item = "item";
            await _sut.TryAdd(item);

            var result = await _sut.TryAdd(item);

            Assert.False(result);
        }

        [Fact(DisplayName = "TryAdd should succeed but not enqueue when an item currently being processed is re-added.")]
        public async Task WorkQueue_TryAdd_When_ItemReaddedAndProcessing_Should_SucceedButNotEnqueue()
        {
            var item = "item";
            await _sut.TryAdd(item);
            _ = await _sut.TryTake();
            var result = await _sut.TryAdd(item);

            Assert.True(result);
            Assert.Equal(0, _sut.Count);
        }

        [Fact(DisplayName = "TryTake should return item when queue not empty.")]
        public async Task WorkQueue_TryTake_When_HasItems_Should_ReturnItem()
        {
            var item = "item";
            await _sut.TryAdd(item);

            var (result, taken) = await _sut.TryTake();

            Assert.True(result);
            Assert.NotNull(taken);
            Assert.Equal(item, taken);
        }
        
        [Fact(DisplayName = "TryTake should block when queue empty.")]
        public async Task WorkQueue_TryTake_When_Empty_Should_Block()
        {
            var (isBlocking, _, _)  = await TestTryTake();
            Assert.True(isBlocking);
        }

        [Fact(DisplayName = "TryTake should return the least recent item.")]
        public async Task WorkQueue_TryTake_When_MultipleItems_Should_ReturnLeastRecent()
        {
            await _sut.TryAdd("1");
            await _sut.TryAdd("2");
            await _sut.TryAdd("3");

            var (result1, taken1) = await _sut.TryTake();
            var (result2, taken2) = await _sut.TryTake();
            var (result3, taken3) = await _sut.TryTake();

            Assert.True(result1);
            Assert.Equal("1", taken1);
            Assert.True(result2);
            Assert.Equal("2", taken2);
            Assert.True(result3);
            Assert.Equal("3", taken3);
        }

        [Fact(DisplayName = "TryTake should block if an item has been re-added but not marked as processed.")]
        public async Task WorkQueue_TryTake_When_ReaddedButStillProcessing_Should_Block()
        {
            var item = "item";
            await _sut.TryAdd(item);
            var (result1, taken1) = await _sut.TryTake();
            await _sut.TryAdd(item);

            (bool result2, string taken2) = (false, null);
            var (isBlocking, _, _)  = await TestTryTake();

            Assert.Equal(0, _sut.Count);
            Assert.True(isBlocking);
        }

        [Fact(DisplayName = "TryTake should return if an item has been re-added and marked as processed.")]
        public async Task WorkQueue_TryTake_When_ReaddedAndProcessed_Should_ReturnItem()
        {
            var item = "item";
            await _sut.TryAdd(item);
            var (result1, taken1) = await _sut.TryTake();
            await _sut.TryAdd(item);
            await _sut.MarkProcessed(item);

            var countAfterProcessed = _sut.Count;
            
            var (isBlocking, result2, _)   = await TestTryTake();

            Assert.Equal(1, countAfterProcessed);
            Assert.False(isBlocking);
            Assert.True(result2);
        }

        [Fact(DisplayName = "MarkProcessed should enqueue duplicate that were added during processing.")]
        public async Task WorkQueue_MarkProcessed_When_DuplicateAddedWhileProcessing_Should_RequeueItem()
        {
            var item = "item";
            await _sut.TryAdd(item);
            var (result1, taken1) = await _sut.TryTake();
            await _sut.TryAdd(item);

            var countBefore = _sut.Count;
            await _sut.MarkProcessed(item);
            var countAfter = _sut.Count;

            var (_, _, taken)   = await TestTryTake();

            Assert.Equal(1, countAfter);
            Assert.Equal(0, countBefore);
            Assert.Equal(item, taken);
        }

        [Fact(DisplayName = "TryAdd should allow add item when consumer consuming")]
        public async Task WorkQueue_TryAdd_When_Consumer_Should_AddItem()
        {
            var consumer = _sut.TryTake();
            var item = "item";
            var producer = _sut.TryAdd(item);
            await Task.Delay(1000);

            Assert.True(producer.IsCompletedSuccessfully);
            Assert.True(producer.Result);
            Assert.True(consumer.IsCompletedSuccessfully);
            var (result, taken) = consumer.Result;
            Assert.Equal(item, taken);
        }


        private async Task<(bool blocking, bool? result, string item)> TestTryTake()
        {
            try
            {
                var (result, item) = await _sut.TryTake(new CancellationTokenSource(2000).Token);
                return (false, result, item);
            }
            catch (TaskCanceledException)
            {
                return (true, null, null);
            }
        } 
    }
}
