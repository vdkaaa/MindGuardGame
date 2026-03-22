namespace MindGuard.Tests
{
    using NUnit.Framework;
    using MindGuard.Core.StateMachine;

    public class StateMachineTests
    {
        /// <summary>
        /// Simple owner class for testing purposes.
        /// </summary>
        private class TestOwner
        {
        }

        /// <summary>
        /// Mock state that tracks method calls.
        /// </summary>
        private class MockState : IState<TestOwner>
        {
            public bool EnterCalled { get; private set; }
            public bool UpdateCalled { get; private set; }
            public bool ExitCalled { get; private set; }

            public void Enter(TestOwner owner)
            {
                EnterCalled = true;
            }

            public void Update(TestOwner owner)
            {
                UpdateCalled = true;
            }

            public void Exit(TestOwner owner)
            {
                ExitCalled = true;
            }

            public void Reset()
            {
                EnterCalled = false;
                UpdateCalled = false;
                ExitCalled = false;
            }
        }

        private StateMachine<TestOwner> _stateMachine;
        private TestOwner _owner;

        [SetUp]
        public void Setup()
        {
            _owner = new TestOwner();
            _stateMachine = new StateMachine<TestOwner>(_owner);
        }

        [Test]
        public void ChangeState_CallsEnterOnNewState()
        {
            // Arrange
            var mockState = new MockState();

            // Act
            _stateMachine.ChangeState(mockState);

            // Assert
            Assert.IsTrue(mockState.EnterCalled, "Enter should be called when changing to a new state");
        }

        [Test]
        public void ChangeState_CallsExitOnPreviousState()
        {
            // Arrange
            var firstState = new MockState();
            var secondState = new MockState();

            // Act
            _stateMachine.ChangeState(firstState);
            _stateMachine.ChangeState(secondState);

            // Assert
            Assert.IsTrue(firstState.ExitCalled, "Exit should be called on the previous state");
        }

        [Test]
        public void Update_CallsUpdateOnCurrentState()
        {
            // Arrange
            var mockState = new MockState();
            _stateMachine.ChangeState(mockState);
            mockState.Reset();

            // Act
            _stateMachine.Update();

            // Assert
            Assert.IsTrue(mockState.UpdateCalled, "Update should be called on the current state");
        }

        [Test]
        public void ChangeState_FromNullDoesNotThrow()
        {
            // Arrange
            var mockState = new MockState();

            // Act & Assert
            Assert.DoesNotThrow(() => _stateMachine.ChangeState(mockState));
        }
    }
}
