namespace MAGES.Recorder.AnalyticsCapture
{
    using MAGES.Analytics;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Captures analytics events emitted by the analytics module.
    /// </summary>
    public class AnalyticsCaptor : ICaptor
    {
        private RecorderModule rm;

        /// <summary>
        /// Gets a value indicating whether the captor needs to be polled.
        /// </summary>
        public bool IsPolling => false;

        /// <summary>
        /// Initializes this captor.
        /// </summary>
        public void Init()
        {
            rm = Hub.Instance.Get<RecorderModule>();

            var am = Hub.Instance.Get<MAGESAnalytics>();
            if (am == null)
            {
                return;
            }

            // Get the events emitted by the analytics module before the recorder module starts recording.
            foreach (var evt in am.Events)
            {
                if (evt.Value is Analytics.Event aevt)
                {
                    if (aevt.GetTag() == "Event")
                    {
                        rm.Events.Add(() =>
                        {
                            WriteEvent(AnalyticsEvent.AnalyticsEventType.EventTriggered, aevt.GetData<string>("Name"), aevt);
                        });
                    }
                }
            }

            // Register on the hooks provided by the analytics module.
            am.OnEventTriggered.AddListener(OnEventTriggered);
            am.OnObjectiveProcessed.AddListener(OnObjectiveProcessed);
            am.OnErrorTriggered.AddListener(OnErrorTriggered);
            am.OnScoreInitialized.AddListener(OnScoreInitialized);
            am.OnScoreChanged.AddListener(OnScoreChanged);
        }

        /// <summary>
        /// Deinitialize the encoder.
        /// </summary>
        public void Deinit()
        {
        }

        /// <summary>
        /// Polls the captor for new data.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <remarks>
        /// This method will only be called if <see cref="IsPolling"/> is true.
        /// </remarks>
        public void Poll(double timestamp)
        {
        }

        private void OnEventTriggered(string name, object value)
        {
            string savedName = name;
            object savedValue = value;

            rm.Events.Add(() =>
            {
                WriteEvent(AnalyticsEvent.AnalyticsEventType.EventTriggered, savedName, savedValue);
            });
        }

        private void OnObjectiveProcessed(string name, object value)
        {
            string savedName = name;
            object savedValue = value;

            rm.Events.Add(() =>
            {
                WriteEvent(AnalyticsEvent.AnalyticsEventType.ObjectiveProcessed, savedName, savedValue);
            });
        }

        private void OnErrorTriggered(string name, object value)
        {
            string savedName = name;
            object savedValue = value;

            rm.Events.Add(() =>
            {
                WriteEvent(AnalyticsEvent.AnalyticsEventType.ErrorTriggered, savedName, savedValue);
            });
        }

        private void OnScoreInitialized(string name, object value)
        {
            string savedName = name;
            object savedValue = value;

            rm.Events.Add(() =>
            {
                WriteEvent(AnalyticsEvent.AnalyticsEventType.ScoreInitialized, savedName, savedValue);
            });
        }

        private void OnScoreChanged(string name, object value)
        {
            string savedName = name;
            object savedValue = value;

            rm.Events.Add(() =>
            {
                WriteEvent(AnalyticsEvent.AnalyticsEventType.ScoreChanged, savedName, savedValue);
            });
        }

        private void WriteEvent(AnalyticsEvent.AnalyticsEventType type, string name, object value)
        {
            if (value is not Analytics.Event)
            {
                return;
            }

            var evt = value as Analytics.Event;
            var jobject = new JObject
            {
                evt.Export(),
            };
            var jsonString = jobject.ToString(Newtonsoft.Json.Formatting.None);

            rm.RecorderInstance.Add(new AnalyticsEvent(
                    type,
                    name,
                    jsonString,
                    rm.RecorderInstance.CurrentFrame,
                    rm.RecorderInstance.Cursor));
        }
    }
}
