namespace MAGES.Recorder.AnalyticsCapture
{
    using System.Linq;
    using MAGES.Analytics;
    using MAGES.UIs;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using UnityEngine;

    /// <summary>
    /// An interpreter for the analytics events.
    /// </summary>
    [InterpretsEvent(typeof(AnalyticsEvent))]
    public class AnalyticsInterpreter : IInterpreter
    {
        private MAGESAnalytics am;
        private RecorderModule rm;

        /// <summary>
        /// Gets a value indicating whether the interpreter is enabled and should receive update events.
        /// </summary>
        public bool Enabled => true;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            rm = Hub.Instance.Get<RecorderModule>();
            am = Hub.Instance.Get<MAGESAnalytics>();
            if (am == null)
            {
                return;
            }

            am.InitializeRuntime = false;
            am.Clean();
        }

        /// <summary>
        /// Deinitializes this instance.
        /// </summary>
        public void Deinitialize()
        {
        }

        /// <summary>
        /// Interprets the event.
        /// </summary>
        /// <param name="evt">The event.</param>
        public void Interpret(ICapturedEvent evt)
        {
            var aevt = evt as AnalyticsEvent;

            switch (aevt.Type)
            {
                case AnalyticsEvent.AnalyticsEventType.EventTriggered:
                    InterpretEventTriggered(aevt);
                    break;
                case AnalyticsEvent.AnalyticsEventType.ObjectiveProcessed:
                    InterpretObjectiveProcessed(aevt);
                    break;
                case AnalyticsEvent.AnalyticsEventType.ErrorTriggered:
                    InterpretErrorTriggered(aevt);
                    break;
                case AnalyticsEvent.AnalyticsEventType.ScoreInitialized:
                    InterpretScoreInitialized(aevt);
                    break;
                case AnalyticsEvent.AnalyticsEventType.ScoreChanged:
                    InterpretScoreChanged(aevt);
                    break;
            }
        }

        /// <summary>
        /// Called at the end of the physics update.
        /// </summary>
        public void FixedUpdate()
        {
        }

        /// <summary>
        /// Called at the start of the update loop.
        /// </summary>
        public void LateUpdate()
        {
        }

        /// <summary>
        /// Called at the end of the update loop.
        /// </summary>
        public void Update()
        {
        }

        private void InterpretEventTriggered(AnalyticsEvent aevt)
        {
            rm.PlayerInstance.Schedule(
                () =>
                {
                    JObject json = null;

                    try
                    {
                        json = JObject.Parse(aevt.JsonString);
                    }
                    catch (JsonReaderException e)
                    {
                        Debug.LogError($"Exception: {e.Message}. Failed to parse JSON string: {aevt.JsonString}");
                        return;
                    }

                    if (json == null)
                    {
                        Debug.LogError($"Failed to parse JSON string: {aevt.JsonString}");
                        return;
                    }

                    // Handle operation report event separately.
                    if (aevt.Name == "OperationReport")
                    {
                        // Retrieve the operation report event data.
                        string operationName = json[aevt.Name][0]["OperationName"].ToString();
                        string username = json[aevt.Name][0]["Username"].ToString();
                        float finalScore = (float)json[aevt.Name][0]["FinalScore"];
                        double duration = (double)json[aevt.Name][0]["Duration"];
                        int errors = (int)json[aevt.Name][0]["Errors"];
                        int objectives = (int)json[aevt.Name][0]["Objectives"];
                        int events = (int)json[aevt.Name][0]["Events"];
                        int scores = (int)json[aevt.Name][0]["Scores"];

                        // Create the operation report event and attach default data. (We explicitly attach them to preserve type)
                        // NOTE: Here we get the event and attach data to it, instead of creating a new one.
                        //       That's because in the playback the operation report event gets created by the analytics module.
                        am.GetEvent<Analytics.Event>($"OperationReport")
                            .SetPrefix("mages.analytics.report")
                            .SetTag("Report")
                            .AttachData("OperationName", operationName)
                            .AttachData("Username", username)
                            .AttachData("FinalScore", finalScore)
                            .AttachData("Duration", duration)
                            .AttachData("Errors", errors)
                            .AttachData("Objectives", objectives)
                            .AttachData("Events", events)
                            .AttachData("Scores", scores);

                        return;
                    }

                    // Retrieve the default event data.
                    string name = json[aevt.Name][0]["Name"].ToString();
                    double timestamp = (double)json[aevt.Name][0]["Timestamp"];

                    // Create the event and attach default data. (We explicitly attach them to preserve type)
                    am.AddEvent<Analytics.Event>(aevt.Name)
                        .SetPrefix("mages.analytics.events")
                        .SetTag("Event")
                        .AttachData("Name", name)
                        .AttachData("Timestamp", timestamp);

                    // Attach additional custom data.
                    foreach (JProperty payload in json[aevt.Name][0].Cast<JProperty>())
                    {
                        if (payload.Name == "Name" || payload.Name == "Timestamp")
                        {
                            continue;
                        }

                        am.GetEvent<Analytics.Event>(aevt.Name)
                            .AttachData(payload.Name, payload.Value.ToString());
                    }
                },
                aevt.CaptureTime);
        }

        private void InterpretErrorTriggered(AnalyticsEvent aevt)
        {
            rm.PlayerInstance.Schedule(
                () =>
                {
                    JObject json = null;

                    try
                    {
                        json = JObject.Parse(aevt.JsonString);
                    }
                    catch (JsonReaderException e)
                    {
                        Debug.LogError($"Exception: {e.Message}. Failed to parse JSON string: {aevt.JsonString}");
                        return;
                    }

                    if (json == null)
                    {
                        Debug.LogError($"Failed to parse JSON string: {aevt.JsonString}");
                        return;
                    }

                    // Retrieve the default error data.
                    string name = json[aevt.Name][0]["Name"].ToString();
                    double timestamp = (double)json[aevt.Name][0]["Timestamp"];
                    string actionName = json[aevt.Name][0]["Action"].ToString();
                    string type = json[aevt.Name][0]["Type"].ToString();
                    int penalty = (int)json[aevt.Name][0]["Penalty(%)"];
                    string info = json[aevt.Name][0]["Info"].ToString();

                    // Create the error event and attach default error data. (We explicitly attach them to preserve type)
                    am.AddEvent<Analytics.Event>(aevt.Name)
                        .SetPrefix("mages.analytics.errors")
                        .SetTag("Error")
                        .AttachData("Name", name)
                        .AttachData("Timestamp", timestamp)
                        .AttachData("Action", actionName)
                        .AttachData("Type", type)
                        .AttachData("Penalty(%)", penalty)
                        .AttachData("Info", info);

                    // Attach additional custom data.
                    foreach (JProperty payload in json[aevt.Name][0].Cast<JProperty>())
                    {
                        if (payload.Name == "Name" || payload.Name == "Timestamp" || payload.Name == "Action"
                            || payload.Name == "Type" || payload.Name == "Penalty(%)" || payload.Name == "Info")
                        {
                            continue;
                        }

                        am.GetEvent<Analytics.Event>(aevt.Name)
                            .AttachData(payload.Name, payload.Value.ToString());
                    }

                    // Show notification if enabled.
                    if (am.RuntimeAnalyticsNotifications)
                    {
                        NotificationSystem.Instance.SpawnNotification(NotificationSystem.NotificationType.Error, info, true);
                    }
                },
                aevt.CaptureTime);
        }

        private void InterpretObjectiveProcessed(AnalyticsEvent aevt)
        {
            rm.PlayerInstance.Schedule(
                () =>
                {
                    JObject json = null;

                    try
                    {
                        json = JObject.Parse(aevt.JsonString);
                    }
                    catch (JsonReaderException e)
                    {
                        Debug.LogError($"Exception: {e.Message}. Failed to parse JSON string: {aevt.JsonString}");
                        return;
                    }

                    if (json == null)
                    {
                        Debug.LogError($"Failed to parse JSON string: {aevt.JsonString}");
                        return;
                    }

                    // Retrieve the default objective data.
                    string name = json[aevt.Name][0]["Name"].ToString();
                    double timestamp = (double)json[aevt.Name][0]["Timestamp"];
                    bool succeeded = (bool)json[aevt.Name][0]["Succeeded"];
                    string description = json[aevt.Name][0]["Description"].ToString();

                    // Create the objective event and attach default objective data. (We explicitly attach them to preserve type)
                    am.AddEvent<Analytics.Event>(aevt.Name)
                        .SetPrefix("mages.analytics.objectives")
                        .SetTag("Objective")
                        .AttachData("Name", name)
                        .AttachData("Timestamp", timestamp)
                        .AttachData("Succeeded", succeeded)
                        .AttachData("Description", description);
                },
                aevt.CaptureTime);
        }

        private void InterpretScoreInitialized(AnalyticsEvent aevt)
        {
            rm.PlayerInstance.Schedule(
                () =>
                {
                    JObject json = null;

                    try
                    {
                        json = JObject.Parse(aevt.JsonString);
                    }
                    catch (JsonReaderException e)
                    {
                        Debug.LogError($"Exception: {e.Message}. Failed to parse JSON string: {aevt.JsonString}");
                        return;
                    }

                    if (json == null)
                    {
                        Debug.LogError($"Failed to parse JSON string: {aevt.JsonString}");
                        return;
                    }

                    // Retrieve the default score data.
                    string name = json[aevt.Name][0]["Name"].ToString();
                    double timestamp = (double)json[aevt.Name][0]["Timestamp"];
                    int count = (int)json[aevt.Name][0]["count"];
                    float initial = (float)json[aevt.Name][0]["initial"];
                    float final = (float)json[aevt.Name][0]["final"];

                    // Create the score event and attach default score data. (We explicitly attach them to preserve type)
                    am.AddEvent<Analytics.Event>(aevt.Name)
                        .SetPrefix("mages.analytics.score")
                        .SetTag("Score")
                        .AttachData("Name", name)
                        .AttachData("Timestamp", timestamp)
                        .AttachData("count", count)
                        .AttachData("initial", initial)
                        .AttachData("final", final);
                },
                aevt.CaptureTime);
        }

        private void InterpretScoreChanged(AnalyticsEvent aevt)
        {
            rm.PlayerInstance.Schedule(
                () =>
                {
                    JObject json = null;

                    try
                    {
                        json = JObject.Parse(aevt.JsonString);
                    }
                    catch (JsonReaderException e)
                    {
                        Debug.LogError($"Exception: {e.Message}. Failed to parse JSON string: {aevt.JsonString}");
                        return;
                    }

                    if (json == null)
                    {
                        Debug.LogError($"Failed to parse JSON string: {aevt.JsonString}");
                        return;
                    }

                    // Retrieve the default score data.
                    int count = (int)json[aevt.Name][0]["count"];
                    float initial = (float)json[aevt.Name][0]["initial"];
                    float final = (float)json[aevt.Name][0]["final"];

                    // Create the score event and attach default score data. (We explicitly attach them to preserve type)
                    am.GetEvent<Analytics.Event>(aevt.Name)
                        .AttachData("count", count)
                        .AttachData("initial", initial)
                        .AttachData("final", final);
                },
                aevt.CaptureTime);
        }
    }
}
