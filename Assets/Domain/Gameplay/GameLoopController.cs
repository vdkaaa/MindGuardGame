namespace MindGuard.Domain.Gameplay
{
    using MindGuard.Core.StateMachine;
    using MindGuard.Core.EventBus;
    using MindGuard.Domain.Gameplay.States;
    using MindGuard.Domain.Gameplay.Events;

    /// <summary>
    /// Controls the game loop by managing state transitions between Day, Night, and Transition phases.
    /// Wires the StateMachine to the game's lifecycle.
    /// </summary>
    public class GameLoopController
    {
        private readonly IEventBus _eventBus;
        private StateMachine<GameLoopController> _stateMachine;
        private int _currentDay;
        private int _currentNight;
        private bool _isRunning;

        public int CurrentDay => _currentDay;
        public int CurrentNight => _currentNight;
        public bool IsRunning => _isRunning;

        /// <summary>
        /// Initializes a new GameLoopController with an event bus.
        /// </summary>
        /// <param name="eventBus">The event bus for publish/subscribe events</param>
        public GameLoopController(IEventBus eventBus)
        {
            _eventBus = eventBus;
            _currentDay = 0;
            _currentNight = 0;
            _isRunning = false;
        }

        /// <summary>
        /// Starts a new game run.
        /// </summary>
        public void StartRun()
        {
            _isRunning = true;
            _currentDay = 1;
            _currentNight = 0;

            // Initialize the state machine
            _stateMachine = new StateMachine<GameLoopController>(this);

            // Start with the day state
            var dayState = new DayState(_eventBus);
            _stateMachine.ChangeState(dayState);
        }

        /// <summary>
        /// Updates the current state.
        /// Should be called every frame while the game is running.
        /// </summary>
        public void Update()
        {
            if (_isRunning && _stateMachine != null)
            {
                _stateMachine.Update();
            }
        }

        /// <summary>
        /// Transitions to the night phase.
        /// </summary>
        public void GoToNight()
        {
            _currentNight++;
            var nightState = new NightState(_eventBus);
            _stateMachine.ChangeState(nightState);
        }

        /// <summary>
        /// Transitions to the day phase.
        /// </summary>
        public void GoToDay()
        {
            _currentDay++;
            var dayState = new DayState(_eventBus);
            _stateMachine.ChangeState(dayState);
        }

        /// <summary>
        /// Ends the current game run.
        /// </summary>
        /// <param name="victory">Whether the player won or lost</param>
        public void EndRun(bool victory)
        {
            _isRunning = false;
            var gameOverEvent = new GameOverEvent { Victory = victory };
            _eventBus.Publish(gameOverEvent);
        }
    }
}
