using NUnit.Framework;
using TheCat.Core;

namespace TheCat.Tests
{
    public sealed class EventBusTests
    {
        private readonly struct TestEvent
        {
            public TestEvent(int value)
            {
                Value = value;
            }

            public int Value { get; }
        }

        [Test]
        public void Publish_DeliversEventToSubscribers()
        {
            EventBus bus = new EventBus();
            int received = 0;

            bus.Subscribe<TestEvent>(evt => received = evt.Value);
            bus.Publish(new TestEvent(7));

            Assert.AreEqual(7, received);
        }

        [Test]
        public void DisposeSubscription_RemovesSubscriber()
        {
            EventBus bus = new EventBus();
            int received = 0;

            System.IDisposable subscription = bus.Subscribe<TestEvent>(evt => received = evt.Value);
            subscription.Dispose();
            bus.Publish(new TestEvent(7));

            Assert.AreEqual(0, received);
            Assert.AreEqual(0, bus.CountSubscribers<TestEvent>());
        }
    }
}
