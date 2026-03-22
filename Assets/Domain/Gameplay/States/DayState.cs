namespace MindGuard.Domain.Gameplay.States
{
    using UnityEngine;
    using MindGuard.Core.StateMachine;
    using MindGuard.Core.EventBus;
    using MindGuard.Domain.Gameplay.Events;

    // TODO: Forward declaration - IEconomySystem will be defined in economy system module
    // using MindGuard.Domain.Economy;
    // TODO: GameLoopController needs to be created - placeholder for now
    public class GameLoopController { }

    /// <summary>
    /// State representing the day phase of the game loop.
    /// During the day, the player manages their economy and builds towers.
    /// </summary>
    public class DayState : IState<GameLoopController>
    {
        private readonly IEventBus _eventBus;
        // TODO: private readonly IEconomySystem _economy;

        public DayState(IEventBus eventBus)
        {
            _eventBus = eventBus;
            // TODO: _economy = economy;
        }

        public void Enter(GameLoopController owner)
        {
            Debug.Log("Day started");
            
            _eventBus.Publish(new DayStartedEvent { DayNumber = 1 });
            // TODO: Replace with actual day number from game loop
        }

        public void Update(GameLoopController owner)
        {
            // TODO: Implement day phase logic
        }

        public void Exit(GameLoopController owner)
        {
            Debug.Log("Day ended");
        }
    }
}
