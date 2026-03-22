namespace MindGuard.Core.StateMachine
{
    /// <summary>
    /// Generic state machine that manages state transitions and updates.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner object</typeparam>
    public class StateMachine<TOwner>
    {
        private readonly TOwner _owner;
        private IState<TOwner> _currentState;

        /// <summary>
        /// Gets the current active state, or null if no state is active.
        /// </summary>
        public IState<TOwner> CurrentState => _currentState;

        /// <summary>
        /// Initializes a new state machine with an owner.
        /// </summary>
        /// <param name="owner">The owner object of this state machine</param>
        public StateMachine(TOwner owner)
        {
            _owner = owner;
            _currentState = null;
        }

        /// <summary>
        /// Changes to a new state, exiting the current state and entering the new one.
        /// </summary>
        /// <param name="newState">The new state to transition to</param>
        public void ChangeState(IState<TOwner> newState)
        {
            // Exit the current state if one is active
            if (_currentState != null)
            {
                _currentState.Exit(_owner);
            }

            // Set the new state
            _currentState = newState;

            // Enter the new state
            if (_currentState != null)
            {
                _currentState.Enter(_owner);
            }
        }

        /// <summary>
        /// Updates the current state. Should be called every frame.
        /// </summary>
        public void Update()
        {
            if (_currentState != null)
            {
                _currentState.Update(_owner);
            }
        }
    }
}
