namespace MindGuard.Domain.Gameplay.Events
{
    /// <summary>
    /// Event published when the day phase starts.
    /// </summary>
    public struct DayStartedEvent
    {
        public int DayNumber { get; set; }
    }

    /// <summary>
    /// Event published when the night phase starts.
    /// </summary>
    public struct NightStartedEvent
    {
        public int NightNumber { get; set; }
    }

    /// <summary>
    /// Event published when a transition phase starts.
    /// </summary>
    public struct TransitionStartedEvent
    {
    }

    /// <summary>
    /// Event published when the game ends.
    /// </summary>
    public struct GameOverEvent
    {
        public bool Victory { get; set; }
    }
}
