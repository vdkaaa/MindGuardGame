namespace MindGuard.Domain.Gameplay.Events
{
    /// <summary>
    /// Event published when the day phase starts.
    /// </summary>
    public record DayStartedEvent(int DayNumber);

    /// <summary>
    /// Event published when the night phase starts.
    /// </summary>
    public record NightStartedEvent(int NightNumber);

    /// <summary>
    /// Event published when a transition phase starts.
    /// </summary>
    public record TransitionStartedEvent();

    /// <summary>
    /// Event published when the game ends.
    /// </summary>
    public record GameOverEvent(bool Victory);
}
