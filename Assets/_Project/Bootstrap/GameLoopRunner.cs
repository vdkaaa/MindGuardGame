namespace MindGuard.Bootstrap
{
    using UnityEngine;
    using MindGuard.Core.EventBus;
    using MindGuard.Domain.Gameplay;

    /// <summary>
    /// Bridges the GameLoopController to Unity.
    /// This is the ONLY MonoBehaviour that directly touches the domain layer.
    /// Acts as an adapter between Unity's lifecycle and the game loop logic.
    /// </summary>
    public class GameLoopRunner : MonoBehaviour
    {
        private GameLoopController _controller;

        private void Start()
        {
            // Get the event bus from the bootstrap container
            var eventBus = GameBootstrapper.Services.Get<IEventBus>();

            // Create the game loop controller
            _controller = new GameLoopController(eventBus);

            // Start the game loop
            _controller.StartRun();
        }

        private void Update()
        {
            // Update the game loop every frame
            if (_controller != null)
            {
                _controller.Update();
            }
        }

        /// <summary>
        /// Context menu command to transition to night phase (for testing).
        /// </summary>
        [ContextMenu("Go To Night")]
        private void GoToNight()
        {
            if (_controller != null && _controller.IsRunning)
            {
                _controller.GoToNight();
            }
        }

        /// <summary>
        /// Context menu command to transition to day phase (for testing).
        /// </summary>
        [ContextMenu("Go To Day")]
        private void GoToDay()
        {
            if (_controller != null && _controller.IsRunning)
            {
                _controller.GoToDay();
            }
        }
    }
}
