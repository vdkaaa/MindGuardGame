namespace MindGuard.Domain.Gameplay.States
{
    using MindGuard.Core.StateMachine;
    using MindGuard.Core.EventBus;
    using MindGuard.Core.Logging;
    using MindGuard.Domain.Gameplay.Events;

    /// <summary>
    /// State representing the night phase of the game loop.
    /// During the night, enemies attack the player's base.
    /// </summary>
    public class NightState : IState<GameLoopController>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger _logger;

        public NightState(IEventBus eventBus, ILogger logger = null)
        {
            _eventBus = eventBus;
            _logger = logger;
        }

        public void Enter(GameLoopController owner)
        {
            _logger?.Log("Night started");
            
            _eventBus.Publish(new NightStartedEvent(1));
            // TODO: Replace with actual night number from game loop
        }

        public void Update(GameLoopController owner)
        {
            // TODO: Implement night phase logic
        }

        public void Exit(GameLoopController owner)
        {
            _logger?.Log("Night ended");
        }
    }
}
