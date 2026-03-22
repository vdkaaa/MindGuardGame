namespace MindGuard.Core.Logging
{
    /// <summary>
    /// Interface for logging to allow domain layer to log without importing UnityEngine.
    /// Implements abstraction layer between domain logic and Unity-specific functionality.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a message at Info level.
        /// </summary>
        /// <param name="message">The message to log</param>
        void Log(string message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log</param>
        void LogWarning(string message);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The error message to log</param>
        void LogError(string message);
    }
}
