namespace MAGES.Tests
{
    using MAGES.Analytics;
    using MAGES.DataContainer;
    using MAGES.SceneGraph;
    using NUnit.Framework;
    using UnityEngine;

    /// <summary>
    /// Class for testing the analytics events.
    /// </summary>
    public class TestEvents
    {
        /// <summary>
        /// Test for the Action Performed event.
        /// </summary>
        [Test]
        public void TestActionPerformedEvent()
        {
        }

        /// <summary>
        /// Test for the Action Skipped event.
        /// </summary>
        [Test]
        public void TestActionSkippedEvent()
        {
        }

        /// <summary>
        /// Test for the Action Undone event.
        /// </summary>
        [Test]
        public void TestActionUndoneEvent()
        {
        }

        /// <summary>
        /// Test for the Session Started event.
        /// </summary>
        [Test]
        public void TestSessionStartedEvent()
        {
        }

        /// <summary>
        /// Test for the Action Performed event.
        /// </summary>
        [Test]
        public void TestSesionEndedEvent()
        {
        }

        /// <summary>
        /// Test for the OnCollision event.
        /// </summary>
        [Test]
        public void TestOnCollisionEvent()
        {
        }

        /// <summary>
        /// Test for the OnInteraction event.
        /// </summary>
        [Test]
        public void TestOnInteractionEvent()
        {
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
