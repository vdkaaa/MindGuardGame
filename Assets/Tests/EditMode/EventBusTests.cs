namespace MindGuard.Tests
{
    using NUnit.Framework;
    using MindGuard.Core.EventBus;

    public class EventBusTests
    {
        private EventBus _eventBus;

        /// <summary>
        /// Struct de prueba para los tests del EventBus.
        /// </summary>
        private struct TestEvent
        {
            public int Value;
            public string Message;
        }

        [SetUp]
        public void Setup()
        {
            _eventBus = new EventBus();
        }

        [Test]
        public void Subscribe_And_Publish_ReceivesEvent()
        {
            // Arrange
            var receivedEvent = default(TestEvent);
            var eventReceived = false;

            void Handler(TestEvent evt)
            {
                receivedEvent = evt;
                eventReceived = true;
            }

            _eventBus.Subscribe<TestEvent>(Handler);
            var testEvent = new TestEvent { Value = 42, Message = "Test" };

            // Act
            _eventBus.Publish(testEvent);

            // Assert
            Assert.IsTrue(eventReceived, "El handler debería haber sido invocado");
            Assert.AreEqual(42, receivedEvent.Value);
            Assert.AreEqual("Test", receivedEvent.Message);
        }

        [Test]
        public void Unsubscribe_DoesNotReceiveEvents()
        {
            // Arrange
            var eventReceived = false;

            void Handler(TestEvent evt)
            {
                eventReceived = true;
            }

            _eventBus.Subscribe<TestEvent>(Handler);
            _eventBus.Unsubscribe<TestEvent>(Handler);

            var testEvent = new TestEvent { Value = 1 };

            // Act
            _eventBus.Publish(testEvent);

            // Assert
            Assert.IsFalse(eventReceived, "El handler no debería ser invocado después de desuscribirse");
        }

        [Test]
        public void Publish_WithoutSubscribers_DoesNotThrow()
        {
            // Arrange
            var testEvent = new TestEvent { Value = 123 };

            // Act & Assert
            Assert.DoesNotThrow(() => _eventBus.Publish(testEvent));
        }
    }
}
