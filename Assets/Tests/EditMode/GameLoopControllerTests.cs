namespace MindGuard.Tests
{
    using NUnit.Framework;
    using MindGuard.Core.EventBus;
    using MindGuard.Domain.Gameplay;
    using MindGuard.Domain.Gameplay.Events;

    public class GameLoopControllerTests
    {
        private GameLoopController _gameLoopController;
        private EventBus _eventBus;

        [SetUp]
        public void Setup()
        {
            _eventBus = new EventBus();
            _gameLoopController = new GameLoopController(_eventBus);
        }

        [Test]
        public void StartRun_SetsIsRunningTrueAndCurrentDayToOne()
        {
            // Act
            _gameLoopController.StartRun();

            // Assert
            Assert.IsTrue(_gameLoopController.IsRunning, "IsRunning should be true after StartRun");
            Assert.AreEqual(1, _gameLoopController.CurrentDay, "CurrentDay should be 1 after StartRun");
        }

        [Test]
        public void GoToNight_IncrementsCurrentNight()
        {
            // Arrange
            _gameLoopController.StartRun();
            var initialNight = _gameLoopController.CurrentNight;

            // Act
            _gameLoopController.GoToNight();

            // Assert
            Assert.AreEqual(initialNight + 1, _gameLoopController.CurrentNight, "CurrentNight should increment by 1");
        }

        [Test]
        public void GoToDay_IncrementsCurrentDay()
        {
            // Arrange
            _gameLoopController.StartRun();
            var initialDay = _gameLoopController.CurrentDay;

            // Act
            _gameLoopController.GoToDay();

            // Assert
            Assert.AreEqual(initialDay + 1, _gameLoopController.CurrentDay, "CurrentDay should increment by 1");
        }

        [Test]
        public void EndRun_PublishesGameOverEventWithCorrectVictoryValue()
        {
            // Arrange
            _gameLoopController.StartRun();
            GameOverEvent receivedEvent = default;
            var eventReceived = false;

            void GameOverEventHandler(GameOverEvent evt)
            {
                receivedEvent = evt;
                eventReceived = true;
            }

            _eventBus.Subscribe<GameOverEvent>(GameOverEventHandler);

            // Act
            _gameLoopController.EndRun(true);

            // Assert
            Assert.IsTrue(eventReceived, "GameOverEvent should be published");
            Assert.IsTrue(receivedEvent.Victory, "GameOverEvent.Victory should be true");
            Assert.IsFalse(_gameLoopController.IsRunning, "IsRunning should be false after EndRun");
        }

        [Test]
        public void EndRun_PublishesGameOverEventWithVictoryFalse()
        {
            // Arrange
            _gameLoopController.StartRun();
            GameOverEvent receivedEvent = default;
            var eventReceived = false;

            void GameOverEventHandler(GameOverEvent evt)
            {
                receivedEvent = evt;
                eventReceived = true;
            }

            _eventBus.Subscribe<GameOverEvent>(GameOverEventHandler);

            // Act
            _gameLoopController.EndRun(false);

            // Assert
            Assert.IsTrue(eventReceived, "GameOverEvent should be published");
            Assert.IsFalse(receivedEvent.Victory, "GameOverEvent.Victory should be false");
        }
    }
}
