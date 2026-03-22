namespace MindGuard.Domain.Gameplay.States
{
    using MindGuard.Core.StateMachine;
    using MindGuard.Core.EventBus;
    using MindGuard.Core.Logging;
    using MindGuard.Domain.Gameplay.Events;

    /// <summary>
    /// State representing a transition phase between day and night.
    /// Used for animations, visual effects, or UI updates between phases.
    /// </summary>
    public class TransitionState : IState<GameLoopController>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger _logger;

        public TransitionState(IEventBus eventBus, ILogger logger = null)
        {
            _eventBus = eventBus;
            _logger = logger;
        }

        public void Enter(GameLoopController owner)
        {
            _logger?.Log("Transition started");
            
            _eventBus.Publish(new TransitionStartedEvent());
        }

        public void Update(GameLoopController owner)
        {
            // TODO: Implement transition phase logic
        }

        public void Exit(GameLoopController owner)
        {
            // No action needed on exit
        }
    }
}
