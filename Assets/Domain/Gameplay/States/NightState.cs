namespace MindGuard.Domain.Gameplay.States
{
    using UnityEngine;
    using MindGuard.Core.StateMachine;
    using MindGuard.Core.EventBus;
    using MindGuard.Domain.Gameplay.Events;
    using MindGuard.Domain.Gameplay;

    /// <summary>
    /// State representing the night phase of the game loop.
    /// During the night, enemies attack the player's base.
    /// </summary>
    public class NightState : IState<GameLoopController>
    {
        private readonly IEventBus _eventBus;

        public NightState(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Enter(GameLoopController owner)
        {
            Debug.Log("Night started");
            
            _eventBus.Publish(new NightStartedEvent { NightNumber = 1 });
            // TODO: Replace with actual night number from game loop
        }

        public void Update(GameLoopController owner)
        {
            // TODO: Implement night phase logic
        }

        public void Exit(GameLoopController owner)
        {
            Debug.Log("Night ended");
        }
    }
}
