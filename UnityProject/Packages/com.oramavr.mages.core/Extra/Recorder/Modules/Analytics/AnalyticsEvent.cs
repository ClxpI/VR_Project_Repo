namespace MAGES.Recorder
{
    using UnityEngine;

    /// <summary>
    /// Represents an event captured by the analytics module.
    /// </summary>
    [System.Serializable]
    [UnityEngine.Scripting.Preserve]
    public class AnalyticsEvent : ICapturedEvent
    {
        [SerializeField]
        private AnalyticsEventType type;

        [SerializeField]
        private string name;

        [SerializeField]
        private double createTime;

        [SerializeField]
        private double captureTime;

        [SerializeField]
        private long createFrame;

        [SerializeField]
        private string jsonString;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsEvent"/> class.
        /// </summary>
        /// <param name="type">The type of the analytics event.</param>
        /// <param name="name">The name of the event.</param>
        /// <param name="jsonString">The serialized json string of the analytic.</param>
        /// <param name="frameCount">The frame count.</param>
        /// <param name="captureTime">The timestamp.</param>
        public AnalyticsEvent(AnalyticsEventType type, string name, string jsonString, long frameCount, double captureTime)
        {
            this.type = type;
            this.name = name;
            this.captureTime = captureTime;
            this.jsonString = jsonString;
            createFrame = frameCount;
            createTime = Time.realtimeSinceStartupAsDouble;
        }

        /// <summary>
        /// Represents the type of analytics event.
        /// </summary>
        [System.Serializable]
        public enum AnalyticsEventType
        {
            /// <summary>
            /// An event was triggered.
            /// </summary>
            EventTriggered,

            /// <summary>
            /// An objective was processed.
            /// </summary>
            ObjectiveProcessed,

            /// <summary>
            /// An error was triggered.
            /// </summary>
            ErrorTriggered,

            /// <summary>
            /// An action score was changed.
            /// </summary>
            ScoreChanged,

            /// <summary>
            /// An action score was initialized.
            /// </summary>
            ScoreInitialized,
        }

        /// <summary>
        /// Gets the estimated time (wall-clock time) when the event was created (NOT captured).
        /// </summary>
        public double CreateTime => createTime;

        /// <summary>
        /// Gets the estimated time (wall-clock time) when the event was captured.
        /// </summary>
        public double CaptureTime => captureTime;

        /// <summary>
        /// Gets the frame the event was captured on.
        /// </summary>
        public long CreateFrame => createFrame;

        /// <summary>
        /// Gets the JSON string representation of the event.
        /// </summary>
        public string JsonString => jsonString;

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Gets the type of event.
        /// </summary>
        public AnalyticsEventType Type => type;
    }
}
