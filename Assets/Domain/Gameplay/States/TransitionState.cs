namespace MindGuard.Domain.Gameplay.States
{
    using UnityEngine;
    using MindGuard.Core.StateMachine;
    using MindGuard.Core.EventBus;
    using MindGuard.Domain.Gameplay.Events;
    using MindGuard.Domain.Gameplay;

    /// <summary>
    /// State representing a transition phase between day and night.
    /// Used for animations, visual effects, or UI updates between phases.
    /// </summary>
    public class TransitionState : IState<GameLoopController>
    {
        private readonly IEventBus _eventBus;

        public TransitionState(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Enter(GameLoopController owner)
        {
            Debug.Log("Transition started");
            
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
