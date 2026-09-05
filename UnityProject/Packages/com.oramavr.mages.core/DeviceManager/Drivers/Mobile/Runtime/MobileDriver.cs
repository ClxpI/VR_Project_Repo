namespace MAGES.Drivers.Mobile
{
    using System;
    using MAGES.ComponentLibrary.Framework;
    using MAGES.DeviceManager;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// A driver for mobile devices.
    /// </summary>
    [Serializable]
    [UnityEngine.Scripting.Preserve]
    public class MobileDriver : IDriver
    {
        [HardAssetReference(HardAssetReferenceAttribute.Type.Resource)]
        private static readonly string ScreenSticksUIPrefabPath = "MobileNavigation/MobileNavigation";

        private UnityEvent<DriverEvent> onDriverEvent = new UnityEvent<DriverEvent>();

        private GameObject instantiatedScreenSticksUI = null;
        private Rig possessedRig;

        private Vector2 rotateStickValue = Vector2.zero;
        private Vector2 moveStickValue = Vector2.zero;

        [Tooltip("The prefab for the screen sticks. If left empty will try to load the default prefab from the resources.")]
        [SerializeField]
        private GameObject screenSticksUIPrefab = null;

        /// <inheritdoc/>
        public string Name => "Mobile";

        /// <inheritdoc/>
        public int DefaultSortOrder => 200;

        /// <inheritdoc/>
        public bool IsAvailable => ScreenSticksUIPrefab != null;

        /// <inheritdoc/>
        public DriverType DriverType => DriverType.NonSpatial;

        /// <inheritdoc/>
        public UnityEvent<DriverEvent> OnDriverEvent => onDriverEvent;

        /// <summary>
        /// Gets the GameObject to spawn for the screen sticks. If left empty will try to load the default prefab from the resources.
        /// </summary>
        public GameObject ScreenSticksUIPrefab
        {
            get
            {
                if (screenSticksUIPrefab == null)
                {
                    screenSticksUIPrefab = Resources.Load<GameObject>(ScreenSticksUIPrefabPath);
                }

                return screenSticksUIPrefab;
            }
        }

        /// <summary>
        /// Gets or sets the head's height from the origin of the rig.
        /// </summary>
        [field: Tooltip("The camera height (meters).")]
        [field: SerializeField]
        public float HeadHeight { get; set; } = 1.75f;

        /// <summary>
        /// Gets or sets the camera rotation speed.
        /// </summary>
        [field: Tooltip("The camera rotation speed.")]
        [field: SerializeField]
        public float CameraRotationSpeed { get; set; } = 100f;

        /// <inheritdoc cref="IDriver.Possess(Rig)"/>
        public void Possess(Rig rig)
        {
            if (rig == null)
            {
                throw new ArgumentNullException("The rig parameter passed on Mobile Driver's Possess(Rig) should not be null.");
            }

            possessedRig = rig;

            instantiatedScreenSticksUI = GameObject.Instantiate(ScreenSticksUIPrefab);

            // Find the screen sticks
            var screenSticks = instantiatedScreenSticksUI.GetComponentsInChildren<MAGESScreenStick>();
            foreach (var screenStick in screenSticks)
            {
                // Set screen stick actions
                if (screenStick.StickType == MAGESScreenStick.MovementType.MoveCamera)
                {
                    screenStick.OnValueChanged += OnMoveScreenStick;
                }
                else if (screenStick.StickType == MAGESScreenStick.MovementType.RotateCamera)
                {
                    screenStick.OnValueChanged += OnRotateScreenStick;
                }
            }

            possessedRig.HeadPoint.transform.localRotation = Quaternion.identity; // Reset head rotation
            possessedRig.HeadPoint.transform.position = possessedRig.RootTransform.TransformPoint(Vector3.up * HeadHeight); // Fix head position based on given height

            moveStickValue = Vector2.zero;
            rotateStickValue = Vector2.zero;

            // Send device status events for the head.
            SendDriverEvent(DriverEvent.Kind.DeviceStatus, DriverEvent.SourceDevice.HeadMountedDisplay, DriverEvent.ValueType.DigitalStateChange, true);
        }

        /// <inheritdoc cref="IDriver.UnPossess"/>
        public void UnPossess()
        {
            // Clean up the rig
            if (instantiatedScreenSticksUI != null)
            {
                GameObject.Destroy(instantiatedScreenSticksUI);
                instantiatedScreenSticksUI = null;
            }

            // Send device status events for the head.
            SendDriverEvent(DriverEvent.Kind.DeviceStatus, DriverEvent.SourceDevice.HeadMountedDisplay, DriverEvent.ValueType.DigitalStateChange, false);

            moveStickValue = Vector2.zero;
            rotateStickValue = Vector2.zero;
            possessedRig = null;
        }

        /// <inheritdoc cref="IDriver.Update(UpdatePhase)"/>
        public void Update(UpdatePhase updatePhase)
        {
            // This driver is not the active driver.
            if (possessedRig == null)
            {
                return;
            }

            if (updatePhase == UpdatePhase.Update)
            {
                // Send device status events.
                UpdateHead();
                UpdateControllerLeft();
                UpdateControllerRight();

                // Send move screen stick events
                {
                    SendDriverEvent(DriverEvent.Kind.Input, DriverEvent.SourceDevice.LeftController, DriverEvent.Feature.Primary2DAxis, DriverEvent.ValueType.Absolute2D, moveStickValue);
                }

                // Send rotate screen stick events
                {
                    // Split 2 axis:

                    // X axis corresponds to right controller 2D axis x.
                    SendDriverEvent(DriverEvent.Kind.Input, DriverEvent.SourceDevice.RightController, DriverEvent.Feature.Primary2DAxis, DriverEvent.ValueType.Absolute2D, new Vector2(rotateStickValue.x, 0f));

                    // Y axis does not send event, instead rotates head directly.
                    possessedRig.HeadPoint.RotateAround(possessedRig.HeadPoint.position, possessedRig.HeadPoint.right, -rotateStickValue.y * CameraRotationSpeed * Time.deltaTime);
                }
            }
        }

        /// <inheritdoc cref="IDriver.Initialize"/>
        public void Initialize()
        {
            moveStickValue = Vector2.zero;
            rotateStickValue = Vector2.zero;
        }

        /// <inheritdoc cref="IDriver.Deinitialize"/>
        public void Deinitialize()
        {
            moveStickValue = Vector2.zero;
            rotateStickValue = Vector2.zero;
        }

        /// <inheritdoc cref="IDriver.TrySendHapticImpulse(DeviceManagerModule.Device, float, float)"/>
        public bool TrySendHapticImpulse(DeviceManagerModule.Device device, float intensity, float duration)
        {
            return false; // No haptic feedback on mobile
        }

        /// <inheritdoc cref="IDriver.IsDeviceAvailable(DeviceManagerModule.Device)"/>
        public bool IsDeviceAvailable(DeviceManagerModule.Device device)
        {
            switch (device)
            {
                case DeviceManagerModule.Device.LeftController:
                case DeviceManagerModule.Device.RightController:
                case DeviceManagerModule.Device.HeadMountedDisplay:
                    return true;
                default:
                    return false;
            }
        }

        private void UpdateHead()
        {
            SendDriverEvent(DriverEvent.Kind.DeviceStatus, DriverEvent.SourceDevice.HeadMountedDisplay, DriverEvent.ValueType.DigitalState, true);
        }

        private void UpdateControllerLeft()
        {
            SendDriverEvent(DriverEvent.Kind.DeviceStatus, DriverEvent.SourceDevice.LeftController, DriverEvent.ValueType.DigitalState, false);
        }

        private void UpdateControllerRight()
        {
            SendDriverEvent(DriverEvent.Kind.DeviceStatus, DriverEvent.SourceDevice.RightController, DriverEvent.ValueType.DigitalState, false);
        }

        private void OnMoveScreenStick(Vector2 value)
        {
            if (possessedRig == null)
            {
                return;
            }

            moveStickValue = value;
        }

        private void OnRotateScreenStick(Vector2 value)
        {
            if (possessedRig == null)
            {
                return;
            }

            rotateStickValue = value;
        }

        private void SendDriverEvent(DriverEvent.Kind kind, DriverEvent.SourceDevice device, DriverEvent.Feature feature, DriverEvent.ValueType valueType, object value)
        {
            DriverEvent evt = DriverEvent.GetFromPool();
            evt.Driver = this;
            evt.EventKind = kind;
            evt.EventSource = device;
            evt.EventSourceFeature = feature;
            evt.EventValueType = valueType;
            evt.EventValue = value;
            onDriverEvent.Invoke(evt);
            evt.ReturnToPool();
        }

        private void SendDriverEvent(DriverEvent.Kind kind, DriverEvent.SourceDevice device, DriverEvent.ValueType valueType, object value)
        {
            // In this case event source feature is not used so set it to something irrelevant.
            SendDriverEvent(kind, device, DriverEvent.Feature.Trigger, valueType, value);
        }
    }
}
