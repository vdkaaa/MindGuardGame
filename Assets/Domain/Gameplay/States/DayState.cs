namespace MindGuard.Domain.Gameplay.States
{
    using MindGuard.Core.StateMachine;
    using MindGuard.Core.EventBus;
    using MindGuard.Core.Logging;
    using MindGuard.Domain.Gameplay.Events;

    // TODO: Forward declaration - IEconomySystem will be defined in economy system module
    // using MindGuard.Domain.Economy;

    /// <summary>
    /// State representing the day phase of the game loop.
    /// During the day, the player manages their economy and builds towers.
    /// </summary>
    public class DayState : IState<GameLoopController>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger _logger;
        // TODO: private readonly IEconomySystem _economy;

        public DayState(IEventBus eventBus, ILogger logger = null)
        {
            _eventBus = eventBus;
            _logger = logger;
            // TODO: _economy = economy;
        }

        public void Enter(GameLoopController owner)
        {
            _logger?.Log("Day started");
            
            _eventBus.Publish(new DayStartedEvent(1));
            // TODO: Replace with actual day number from game loop
        }

        public void Update(GameLoopController owner)
        {
            // TODO: Implement day phase logic
        }

        public void Exit(GameLoopController owner)
        {
            _logger?.Log("Day ended");
        }
    }
}
