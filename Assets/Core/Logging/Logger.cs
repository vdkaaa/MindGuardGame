namespace MindGuard.Core.Logging
{
    using UnityEngine;

    /// <summary>
    /// Default logger implementation that uses Unity's Debug.Log.
    /// This is the only place in Core that should import UnityEngine for logging.
    /// </summary>
    public class UnityLogger : ILogger
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }
    }
}
