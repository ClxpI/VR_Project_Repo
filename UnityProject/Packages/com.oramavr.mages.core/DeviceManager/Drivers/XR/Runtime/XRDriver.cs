namespace MAGES.Drivers.XR
{
    using System;
    using System.Collections.Generic;
    using MAGES.ComponentLibrary.Framework;
    using MAGES.Utilities;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.XR;
    using UnityEngine.XR.Management;
    using CommonUsages = UnityEngine.XR.CommonUsages;

    /// <summary>
    /// A driver using Unity XR.
    /// </summary>
    [Serializable]
    public class XRDriver : IDriver
    {
        private UnityEvent<DriverEvent> onDriverEvent = new UnityEvent<DriverEvent>();

        private XRLoaderOffset? activeXRLoaderOffset = null;

        private VirtualController leftController;
        private VirtualController rightController;

        private ComponentLibrary.Framework.Rig possessedRig;

        // Driver status.
        private bool isAvailable = false;

        // Head Device status
        private Pose headPose = Pose.identity;
        private bool isHeadDeviceAvailable = false;

        private bool isInitialized = false;
        private bool hasReceivedDisplayFocusEvent = false;

        private float mainCameraFOV = 60f;

        /// <inheritdoc cref="IDriver.Name"/>
        public string Name { get; } = "XR";

        /// <inheritdoc/>
        public int DefaultSortOrder => 100;

        /// <inheritdoc cref="IDriver.OnDriverEvent"/>
        public UnityEvent<DriverEvent> OnDriverEvent => onDriverEvent;

        /// <inheritdoc cref="IDriver.DriverType"/>
        public DriverType DriverType => DriverType.Spatial;

        /// <inheritdoc cref="IDriver.IsAvailable"/>
        public bool IsAvailable => isAvailable;

        /// <summary>
        /// Gets or sets the offset of the controllers for different loaders.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("The offset of the controllers for different loaders.")]
        public List<XRLoaderOffset> XRLoaderOffsets { get; set; } = new List<XRLoaderOffset>()
        {
            new XRLoaderOffset { Loader = "OculusLoader", LeftControllerOffset = Pose.identity, RightControllerOffset = Pose.identity, HeadOffset = Pose.identity },
            new XRLoaderOffset { Loader = "OpenXRLoader", LeftControllerOffset = new Pose(new Vector3(0f, -0.04f, 0.07f), new Quaternion(0.42261827f, 0, 0, 0.906307876f)), RightControllerOffset = new Pose(new Vector3(0f, -0.04f, 0.07f), new Quaternion(0.42261827f, 0, 0, 0.906307876f)), HeadOffset = Pose.identity },
        };

        private Camera MainCamera
        {
            get
            {
                var cam = Camera.main;
                if (cam != null)
                {
                    return cam;
                }

                if (Hub.Instance != null && Hub.Instance.Rig != null && Hub.Instance.Rig.HeadCamera != null)
                {
                    return Hub.Instance.Rig.HeadCamera;
                }

                if (Camera.allCameras.Length > 0)
                {
                    return Camera.allCameras[0];
                }

                return null;
            }
        }

        /// <inheritdoc cref="IDriver.Initialize/>
        public void Initialize()
        {
            isAvailable = false;
            headPose = Pose.identity;
            isHeadDeviceAvailable = false;
            isInitialized = false;

            var mainCam = MainCamera;
            if (mainCam != null)
            {
                mainCameraFOV = mainCam.fieldOfView;
            }
        }

        /// <inheritdoc cref="IDriver.Possess(Rig)"/>
        public void Possess(Rig rig)
        {
            if (rig == null)
            {
                throw new ArgumentNullException("The rig parameter passed on XR Driver's Possess(Rig) should not be null.");
            }

            // Find loader and set offset
            activeXRLoaderOffset = null;
            IReadOnlyList<XRLoader> loaders = XRGeneralSettings.Instance?.Manager?.activeLoaders;
            if (loaders != null && loaders.Count > 0)
            {
                XRLoader activeLoader = loaders[0];
                var offsetIndex = XRLoaderOffsets.FindIndex(offset => offset.Loader == activeLoader.name);
                if (offsetIndex >= 0)
                {
                    activeXRLoaderOffset = XRLoaderOffsets[offsetIndex];
                }
            }

            isHeadDeviceAvailable = false;
            leftController = new VirtualController(this, VirtualController.ControllerHandedness.Left, rig, activeXRLoaderOffset.HasValue ? activeXRLoaderOffset.Value.LeftControllerOffset : Pose.identity, onDriverEvent);
            rightController = new VirtualController(this, VirtualController.ControllerHandedness.Right, rig, activeXRLoaderOffset.HasValue ? activeXRLoaderOffset.Value.RightControllerOffset : Pose.identity, onDriverEvent);
            possessedRig = rig;
        }

        /// <inheritdoc cref="IDriver.Update(UpdatePhase)"/>
        public void Update(UpdatePhase updatePhase)
        {
            if (!isInitialized)
            {
                if (updatePhase == UpdatePhase.Update)
                {
                    LateInitialize();
                    isInitialized = true;
                }

                return;
            }

            if (!hasReceivedDisplayFocusEvent)
            {
                var headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
                if (headDevice.isValid)
                {
                    OnDisplayFocusChanged(true);
                }
            }

            // Driver is not active, so do nothing
            if (possessedRig == null)
            {
                return;
            }

            // Update only on Update and OnPreRender
            switch (updatePhase)
            {
                case UpdatePhase.Update:
                case UpdatePhase.OnPreRender:
                    // Head
                    UpdateHead();

                    // Controllers
                    leftController.Update();
                    rightController.Update();

                    // Send driver status events
                    SendDriverEvent(DriverEvent.Kind.DeviceStatus, DriverEvent.SourceDevice.Unknown, DriverEvent.ValueType.DigitalState, isAvailable);
                    break;
            }
        }

        /// <inheritdoc cref="IDriver.UnPossess"/>
        public void UnPossess()
        {
            if (possessedRig == null)
            {
                return;
            }

            var activeLoader = XRGeneralSettings.Instance?.Manager?.activeLoader;
            if (activeLoader != null)
            {
                var displaySubsystem = activeLoader.GetLoadedSubsystem<XRDisplaySubsystem>();
                if (displaySubsystem != null)
                {
                    displaySubsystem.Stop();
                }
            }

            var mainCam = MainCamera;
            if (mainCam != null)
            {
                mainCam.fieldOfView = mainCameraFOV;
            }

            // Send status change for head.
            SendDriverEvent(DriverEvent.Kind.DeviceStatus, DriverEvent.SourceDevice.HeadMountedDisplay, DriverEvent.ValueType.DigitalStateChange, false);

            // Send status change for controllers.
            leftController.OnUnPossess();
            rightController.OnUnPossess();

            leftController = null;
            rightController = null;
            possessedRig = null;
        }

        /// <inheritdoc cref="IDriver.Deinitialize"/>
        public void Deinitialize()
        {
            if (isInitialized)
            {
                var activeLoader = XRGeneralSettings.Instance?.Manager?.activeLoader;
                if (activeLoader != null)
                {
                    var displaySubsystem = activeLoader.GetLoadedSubsystem<XRDisplaySubsystem>();
                    displaySubsystem.displayFocusChanged -= OnDisplayFocusChanged;
                }

                DeInitializeXRLoader();

                isInitialized = false;
            }
        }

        /// <inheritdoc cref="IDriver.TrySendHapticImpulse(DeviceManagerModule.Device, float, float)"/>
        public bool TrySendHapticImpulse(DeviceManagerModule.Device device, float intensity, float duration)
        {
            UnityEngine.XR.InputDevice inputDevice;
            switch (device)
            {
                case DeviceManagerModule.Device.HeadMountedDisplay:
                    inputDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
                    break;
                case DeviceManagerModule.Device.LeftController:
                    inputDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                    break;
                case DeviceManagerModule.Device.RightController:
                    inputDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                    break;
                default:
                    return false;
            }

            if (!inputDevice.isValid)
            {
                return false;
            }

            if (inputDevice.TryGetHapticCapabilities(out HapticCapabilities capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    inputDevice.SendHapticImpulse(0, intensity, duration);
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc cref="IDriver.IsDeviceAvailable"/>
        public bool IsDeviceAvailable(DeviceManagerModule.Device device)
        {
            switch (device)
            {
                case DeviceManagerModule.Device.HeadMountedDisplay:
                    return InputDevices.GetDeviceAtXRNode(XRNode.Head).isValid;
                case DeviceManagerModule.Device.LeftController:
                    return leftController.IsAvailable;
                case DeviceManagerModule.Device.RightController:
                    return rightController.IsAvailable;
                default:
                    return false;
            }
        }

        // Called in the first update frame.
        private void LateInitialize()
        {
            InitializeXRLoader();
            isInitialized = true;

            var activeLoader = XRGeneralSettings.Instance?.Manager?.activeLoader;
            if (activeLoader != null)
            {
                var displaySubsystem = activeLoader.GetLoadedSubsystem<XRDisplaySubsystem>();

                displaySubsystem.displayFocusChanged += OnDisplayFocusChanged;
            }
        }

        private void InitializeXRLoader()
        {
            if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
            {
                XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
                if (XRGeneralSettings.Instance.Manager.activeLoaders.Count > 0)
                {
                    XRGeneralSettings.Instance.Manager.StartSubsystems();
                }
            }
        }

        private void DeInitializeXRLoader()
        {
            if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
            {
                if (XRGeneralSettings.Instance.Manager.isInitializationComplete && XRGeneralSettings.Instance.Manager.activeLoader != null)
                {
                    XRGeneralSettings.Instance.Manager.DeinitializeLoader();
                }
            }

            hasReceivedDisplayFocusEvent = false;
            isAvailable = false;
        }

        private void OnDisplayFocusChanged(bool displayFocus)
        {
            hasReceivedDisplayFocusEvent = true;

            // Change of focus.
            if (isAvailable != displayFocus)
            {
                isAvailable = displayFocus;
                SendDriverEvent(DriverEvent.Kind.DriverStatus, DriverEvent.SourceDevice.Unknown, DriverEvent.ValueType.DigitalStateChange, displayFocus);
            }
        }

        private void UpdateHead()
        {
            // Head
            if (possessedRig == null)
            {
                return;
            }

            var headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);

            var isHeadDeviceAvailable = headDevice.isValid;
            if (this.isHeadDeviceAvailable != isHeadDeviceAvailable)
            {
                this.isHeadDeviceAvailable = isHeadDeviceAvailable;
                SendDriverEvent(DriverEvent.Kind.DeviceStatus, DriverEvent.SourceDevice.HeadMountedDisplay, DriverEvent.ValueType.DigitalStateChange, this.isHeadDeviceAvailable);
                return;
            }

            SendDriverEvent(DriverEvent.Kind.DeviceStatus, DriverEvent.SourceDevice.HeadMountedDisplay, DriverEvent.ValueType.DigitalState, this.isHeadDeviceAvailable);

            if (this.isHeadDeviceAvailable && possessedRig.HeadPoint != null)
            {
                if (headDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 headPosition))
                {
                    headPose.position = headPosition;
                }

                if (headDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion headRotation))
                {
                    headPose.rotation = headRotation;
                }

                possessedRig.HeadPoint.SetLocalPose(headPose);
                possessedRig.HeadPoint.TransformBySelfLocalSpace(activeXRLoaderOffset.HasValue ? activeXRLoaderOffset.Value.HeadOffset : Pose.identity);
            }
        }

        private void SendDriverEvent(DriverEvent.Kind kind, DriverEvent.SourceDevice device, DriverEvent.ValueType valueType, object value)
        {
            DriverEvent evt = DriverEvent.GetFromPool();
            evt.Driver = this;
            evt.EventKind = kind;
            evt.EventSource = device;
            evt.EventValueType = valueType;
            evt.EventValue = value;
            onDriverEvent.Invoke(evt);
            evt.ReturnToPool();
        }

        private class VirtualController
        {
            private Pose currentPose = Pose.identity;

            private bool isInitializedCorrectly = false;

            private IDriver driver;
            private XRNode xrnode;

            private DriverEvent.SourceDevice sourceDevice;
            private Transform scenePuppet;

            private Pose poseOffset = Pose.identity;

            private UnityEvent<DriverEvent> onEvent;

            private bool isDeviceAvailable = false;

            // All digital buttons
            private Button gripButton;
            private Button triggerButton;
            private Button primaryButton; // A button
            private Button secondaryButton; // B button
            private Button primaryAxisButton; // Thumbstick button

            public VirtualController(IDriver driver, ControllerHandedness controllerSide, Rig rig, Pose offset, UnityEvent<DriverEvent> onEvent)
            {
                switch (controllerSide)
                {
                    case ControllerHandedness.Left:
                        xrnode = XRNode.LeftHand;
                        sourceDevice = DriverEvent.SourceDevice.LeftController;
                        scenePuppet = rig.LeftControllerPoint.transform;
                        break;
                    case ControllerHandedness.Right:
                        xrnode = XRNode.RightHand;
                        sourceDevice = DriverEvent.SourceDevice.RightController;
                        scenePuppet = rig.RightControllerPoint.transform;
                        break;
                    default:
                        return;
                }

                this.driver = driver;
                poseOffset = offset;
                this.onEvent = onEvent;
                isInitializedCorrectly = true;
            }

            /// <summary>
            /// The handiness of the controller.
            /// </summary>
            public enum ControllerHandedness
            {
                /// <summary>
                /// Left hand controller.
                /// </summary>
                Left,

                /// <summary>
                /// Right hand controller.
                /// </summary>
                Right,
            }

            public bool IsAvailable => isDeviceAvailable;

            public void Update()
            {
                if (!isInitializedCorrectly)
                {
                    return;
                }

                UpdatePuppetPositionAndRotation();
                CheckAndSendEvents();
            }

            public void OnUnPossess()
            {
                // Send device switched off event.
                if (isDeviceAvailable)
                {
                    SendDriverEvent(DriverEvent.Kind.DeviceStatus, sourceDevice, DriverEvent.Feature.Trigger, DriverEvent.ValueType.DigitalStateChange, false);
                }
            }

            private void UpdatePuppetPositionAndRotation()
            {
                if (scenePuppet != null)
                {
                    var inputDevice = InputDevices.GetDeviceAtXRNode(xrnode);

                    if (!inputDevice.isValid)
                    {
                        return;
                    }

                    // Update the puppet's position and rotation
                    if (inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 controllerPosition))
                    {
                        currentPose.position = controllerPosition;
                    }

                    if (inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion controllerRotation))
                    {
                        currentPose.rotation = controllerRotation;
                    }

                    scenePuppet.SetLocalPose(currentPose);
                    scenePuppet.TransformBySelfLocalSpace(poseOffset);
                }
            }

            private void CheckAndSendEvents()
            {
                var inputDevice = InputDevices.GetDeviceAtXRNode(xrnode);

                // Send Device Status events
                if (inputDevice.isValid != isDeviceAvailable)
                {
                    isDeviceAvailable = inputDevice.isValid;
                    SendDriverEvent(DriverEvent.Kind.DeviceStatus, sourceDevice, DriverEvent.Feature.Trigger, DriverEvent.ValueType.DigitalStateChange, isDeviceAvailable);
                }

                SendDriverEvent(DriverEvent.Kind.DeviceStatus, sourceDevice, DriverEvent.Feature.Trigger, DriverEvent.ValueType.DigitalState, isDeviceAvailable);

                // Call the update method to all buttons to reset their values for new frame checks
                ButtonsUpdate();

                // --- Send digital button events ---

                // Grip Button
                {
                    if (inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out var btnVal))
                    {
                        gripButton.IsPressed = btnVal;
                    }

                    // Send state change events
                    if (gripButton.WasPressedThisFrame)
                    {
                        SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.Grip, DriverEvent.ValueType.DigitalStateChange, true);
                    }

                    if (gripButton.WasReleasedThisFrame)
                    {
                        SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.Grip, DriverEvent.ValueType.DigitalStateChange, false);
                    }

                    // Send state events
                    SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.Grip, DriverEvent.ValueType.DigitalState, gripButton.IsPressed);
                }

                // Trigger Button
                {
                    if (inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out var btnVal))
                    {
                        triggerButton.IsPressed = btnVal;
                    }

                    // Send state change events
                    if (triggerButton.WasPressedThisFrame)
                    {
                        SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.Trigger, DriverEvent.ValueType.DigitalStateChange, true);
                    }

                    if (triggerButton.WasReleasedThisFrame)
                    {
                        SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.Trigger, DriverEvent.ValueType.DigitalStateChange, false);
                    }

                    // Send state event
                    SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.Trigger, DriverEvent.ValueType.DigitalState, triggerButton.IsPressed);
                }

                // Primary Button
                {
                    if (inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out var btnVal))
                    {
                        primaryButton.IsPressed = btnVal;
                    }

                    // Send state change events
                    if (primaryButton.WasPressedThisFrame)
                    {
                        SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.PrimaryButton, DriverEvent.ValueType.DigitalStateChange, true);
                    }

                    if (primaryButton.WasReleasedThisFrame)
                    {
                        SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.PrimaryButton, DriverEvent.ValueType.DigitalStateChange, false);
                    }

                    // Send state event
                    SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.PrimaryButton, DriverEvent.ValueType.DigitalState, primaryButton.IsPressed);
                }

                // Secondary Button
                {
                    if (inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out var btnVal))
                    {
                        secondaryButton.IsPressed = btnVal;
                    }

                    // Send state change events
                    if (secondaryButton.WasPressedThisFrame)
                    {
                        SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.SecondaryButton, DriverEvent.ValueType.DigitalStateChange, true);
                    }

                    if (secondaryButton.WasReleasedThisFrame)
                    {
                        SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.SecondaryButton, DriverEvent.ValueType.DigitalStateChange, false);
                    }

                    // Send state event
                    SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.SecondaryButton, DriverEvent.ValueType.DigitalState, secondaryButton.IsPressed);
                }

                // Primary Axis Button
                {
                    if (inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out var btnVal))
                    {
                        primaryAxisButton.IsPressed = btnVal;
                    }

                    // Send state change events
                    if (primaryAxisButton.WasPressedThisFrame)
                    {
                        SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.Primary2DAxis, DriverEvent.ValueType.DigitalStateChange, true);
                    }

                    if (primaryAxisButton.WasReleasedThisFrame)
                    {
                        SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.Primary2DAxis, DriverEvent.ValueType.DigitalStateChange, false);
                    }

                    // Send state event
                    SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.Primary2DAxis, DriverEvent.ValueType.DigitalState, primaryAxisButton.IsPressed);
                }

                // --- Send analog events ---

                // Grip
                {
                    if (inputDevice.TryGetFeatureValue(CommonUsages.grip, out var val))
                    {
                        SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.Grip, DriverEvent.ValueType.Absolute1D, val);
                    }
                }

                // Trigger
                {
                    if (inputDevice.TryGetFeatureValue(CommonUsages.trigger, out var val))
                    {
                        SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.Trigger, DriverEvent.ValueType.Absolute1D, val);
                    }
                }

                // Primary 2DAxis
                {
                    if (inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out var val))
                    {
                        SendDriverEvent(DriverEvent.Kind.Input, sourceDevice, DriverEvent.Feature.Primary2DAxis, DriverEvent.ValueType.Absolute2D, val);
                    }
                }
            }

            private void ButtonsUpdate()
            {
                gripButton.Update();
                triggerButton.Update();
                primaryButton.Update();
                secondaryButton.Update();
                primaryAxisButton.Update();
            }

            private void SendDriverEvent(DriverEvent.Kind kind, DriverEvent.SourceDevice device, DriverEvent.Feature deviceFeature, DriverEvent.ValueType valueType, object value)
            {
                DriverEvent evt = DriverEvent.GetFromPool();
                evt.Driver = driver;
                evt.EventKind = kind;
                evt.EventSource = device;
                evt.EventSourceFeature = deviceFeature;
                evt.EventValueType = valueType;
                evt.EventValue = value;
                onEvent.Invoke(evt);
                evt.ReturnToPool();
            }

            private struct Button
            {
                private bool wasPressedThisFrame;
                private bool wasReleasedThisFrame;
                private bool isPressed;

                public bool IsPressed
                {
                    get
                    {
                        return isPressed;
                    }

                    set
                    {
                        var newPressVal = value;
                        if (!isPressed && newPressVal)
                        {
                            wasPressedThisFrame = true;
                        }
                        else if (isPressed && !newPressVal)
                        {
                            wasReleasedThisFrame = true;
                        }

                        isPressed = value;
                    }
                }

                public bool WasPressedThisFrame => wasPressedThisFrame;

                public bool WasReleasedThisFrame => wasReleasedThisFrame;

                public void Update()
                {
                    wasPressedThisFrame = false;
                    wasReleasedThisFrame = false;
                }
            }
        }
    }
}