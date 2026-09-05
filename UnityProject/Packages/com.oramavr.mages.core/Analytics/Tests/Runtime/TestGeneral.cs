namespace MAGES.Tests
{
    using System;
    using MAGES.Analytics;
    using MAGES.DataContainer;
    using MAGES.SceneGraph;
    using NUnit.Framework;
    using UnityEngine;

    /// <summary>
    /// Class for testing the analytics module in general.
    /// </summary>
    public class TestGeneral
    {
        /// <summary>
        /// Test for analytics module initialization.
        /// </summary>
        [Test]
        public void TestAnalyticsInitialization()
        {
            var go = new GameObject("__test_go__");

            go.SetActive(false);

            Hub hub = go.AddComponent<Hub>();
            hub.AutoStart = false;
            hub.BaseBundle = BuildStubBundle();

            go.SetActive(true);

            AnalyticsLogger.Enable = false;

            hub.StartSystems();

            AnalyticsModule analytics = hub?.Get<AnalyticsModule>();
            Assert.IsNotNull(analytics);

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for Event AttachData function.
        /// </summary>
        [Test]
        public void TestAnalyticsEventAttachData()
        {
            GameObject go = new GameObject("__test_go__");

            go.SetActive(false);

            Hub hub = go.AddComponent<Hub>();
            hub.AutoStart = false;
            hub.BaseBundle = BuildStubBundle();

            go.SetActive(true);

            AnalyticsLogger.Enable = false;

            hub.StartSystems();

            AnalyticsModule analytics = Hub.Instance.Get<AnalyticsModule>();
            Assert.IsNotNull(analytics);

            DataContainerModule dataContainer = Hub.Instance.Get<DataContainerModule>();
            Assert.IsNotNull(dataContainer);

            dataContainer.SetSchema("test1.action.*", typeof(ListSchema));
            dataContainer.SetSchema("test1.event.*", typeof(EventSchema));

            dataContainer.StoreData($"test1.action.list", new ActionRecord { Name = "Action1", BeginTime = 0.0f, EndTime = 12.0f });
            dataContainer.StoreData($"test1.action.list", new ActionRecord { Name = "Action2", BeginTime = 0.0f, EndTime = 5.0f });
            dataContainer.StoreData($"test1.action.list", new ActionRecord { Name = "Action3", BeginTime = 0.0f, EndTime = 8.0f });
            dataContainer.StoreData($"test1.action.list", new ActionRecord { Name = "Action4", BeginTime = 13.0f, EndTime = 47.0f });
            dataContainer.StoreData($"test1.action.list", new ActionRecord { Name = "Action5", BeginTime = 13.0f, EndTime = 28.0f });
            dataContainer.StoreData($"test1.action.list", new ActionRecord { Name = "Action6", BeginTime = 48.0f, EndTime = 54.0f });

            Hub.Instance.Get<AnalyticsModule>().AddEvent<MAGES.Analytics.Event>("TestEvent1")
                .SetTag("Event")
                .SetPrefix("test1.event")
                .AttachData("Station", "CPR")
                .AttachData("Heartrate", 10)
                .AttachData("CongintiveLoad", 10.4f);

            Assert.AreEqual("CPR", dataContainer.GetData("test1.event.TestEvent1.Station"));
            Assert.AreEqual(10, dataContainer.GetData("test1.event.TestEvent1.Heartrate"));
            Assert.AreEqual(10.4f, dataContainer.GetData("test1.event.TestEvent1.CongintiveLoad"));

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the analytics module clean functionality.
        /// </summary>
        [Test]
        public void TestAnalyticsModuleClean()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            dataContainer.StoreData($"mages.analytics.actions.Action1-ID", new ActionRecord { Name = "Action1", BeginTime = 0.0f, EndTime = 12.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action2-ID", new ActionRecord { Name = "Action2", BeginTime = 0.0f, EndTime = 5.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action3-ID", new ActionRecord { Name = "Action3", BeginTime = 0.0f, EndTime = 8.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action4-ID", new ActionRecord { Name = "Action4", BeginTime = 13.0f, EndTime = 47.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action5-ID", new ActionRecord { Name = "Action5", BeginTime = 13.0f, EndTime = 28.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action6-ID", new ActionRecord { Name = "Action6", BeginTime = 48.0f, EndTime = 54.0f });

            var data = dataContainer.GetSpecificSchemaData("mages.analytics.actions.*");

            Assert.AreEqual(6, data.Count);

            analytics.Clean();

            try
            {
                data = dataContainer.GetSpecificSchemaData("mages.analytics.actions.*");
            }
            catch (ArgumentException argumentException)
            {
                Assert.AreEqual("No data found under the given key.", argumentException.Message);
            }

            dataContainer.StoreData($"mages.analytics.actions.Action1-ID", new ActionRecord { Name = "Action1", BeginTime = 0.0f, EndTime = 12.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action2-ID", new ActionRecord { Name = "Action2", BeginTime = 0.0f, EndTime = 5.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action3-ID", new ActionRecord { Name = "Action3", BeginTime = 0.0f, EndTime = 8.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action4-ID", new ActionRecord { Name = "Action4", BeginTime = 13.0f, EndTime = 47.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action5-ID", new ActionRecord { Name = "Action5", BeginTime = 13.0f, EndTime = 28.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action6-ID", new ActionRecord { Name = "Action6", BeginTime = 48.0f, EndTime = 54.0f });

            data = dataContainer.GetSpecificSchemaData("mages.analytics.actions.*");

            Assert.AreEqual(6, data.Count);

            GameObject.Destroy(go);
        }

        private GameObject InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer)
        {
            GameObject go = new GameObject("__test_go__");

            go.SetActive(false);

            Hub hub = go.AddComponent<Hub>();
            hub.AutoStart = false;
            hub.BaseBundle = BuildStubBundle();

            go.SetActive(true);

            AnalyticsLogger.Enable = false;

            hub.StartSystems();

            analytics = Hub.Instance.Get<AnalyticsModule>();
            Assert.IsNotNull(analytics);

            dataContainer = Hub.Instance.Get<DataContainerModule>();
            Assert.IsNotNull(dataContainer);

            return go;
        }

        private Bundle BuildStubBundle()
        {
            UserAuthenticationModule userAuthenticationModule = ScriptableObject.CreateInstance<StubUserAuthentication>();
            DataContainerModule dataContainerModule = ScriptableObject.CreateInstance<MAGESDataContainer>();
            InteractionSystemModule interactionSystemModule = ScriptableObject.CreateInstance<StubInteractionSystem>();
            NetworkingModule networkingModule = ScriptableObject.CreateInstance<StubNetworking>();
            AnalyticsModule analyticsModule = ScriptableObject.CreateInstance<MAGESAnalytics>();
            DeviceManagerModule deviceManagerModule = ScriptableObject.CreateInstance<StubDeviceManager>();
            SceneGraphModule sceneGraphModule = ScriptableObject.CreateInstance<StubSceneGraph>();

            Bundle bundle = ScriptableObject.CreateInstance<Bundle>();
            bundle.UserAuthentication = userAuthenticationModule;
            bundle.DataContainer = dataContainerModule;
            bundle.InteractionSystem = interactionSystemModule;
            bundle.Networking = networkingModule;
            bundle.DeviceManager = deviceManagerModule;
            bundle.SceneGraph = sceneGraphModule;
            bundle.Analytics = analyticsModule;

            return bundle;
        }
    }
}
