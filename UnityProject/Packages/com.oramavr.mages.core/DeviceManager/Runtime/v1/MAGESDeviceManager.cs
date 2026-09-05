namespace MAGES.DeviceManager
{
    using System;
    using System.Linq;
    using MAGES.Utilities;
    using UnityEngine;
#if XR_MANAGEMENT
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.XR;
    using UnityEngine.XR.Management;
#endif

    /// <summary>
    /// MAGES implementation for Device Management.
    /// </summary>
    [CreateAssetMenu(fileName = "MAGESDeviceManager", menuName = "MAGES/DeviceManager/MAGESDeviceManager")]
    [Obsolete("MAGESDeviceManager is deprecated. Please use MAGESDeviceManagerV2 instead.", false)]
    public class MAGESDeviceManager : DeviceManagerModule
    {
        private const float Mobile3DCameraRotationSpeed = 100f;

        [HardAssetReference(HardAssetReferenceAttribute.Type.Resource)]
        private static readonly string MobileNavigationAssetPath = "MobileNavigation/MobileNavigation";

        /// <summary>
        /// Reference to the MAGES InputController.
        /// </summary>
        [SerializeField]
        private InputController inputActions;

        private Vector2 smoothTurnInput = Vector2.zero;

        private MAGESScreenStick[] sticks = new MAGESScreenStick[0];
        private GameObject instantiatedMobileNavigation = null;

        private XRLoaderOffset currentLoaderOffset = XRLoaderOffset.Identity;
        private Pose currentHeadPose = Pose.identity;
        private Pose currentLeftControllerPose = Pose.identity;
        private Pose currentRightControllerPose = Pose.identity;

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

        /// <summary>
        /// Gets or sets the MAGES InputController.
        /// </summary>
        public InputController ControllerActions
        {
            get => inputActions;
            set => inputActions = value;
        }

        /// <summary>
        /// Initializes the DeviceManager and InputController of MAGES.
        /// </summary>
        public override void Startup()
        {
            InteractionActions = inputActions;
            LocomotionActions = inputActions;
            GenericActions = inputActions;
            Mobile3DActions = inputActions;
            inputActions.Startup();

            SetupXR();

            SetupInputDevices(CurrentMode);

            LocomotionActions.AddActionOnSmoothTurn(OnSmoothTurn);
        }

        /// <summary>
        /// Shutdowns the DeviceManager and InputController of MAGES.
        /// </summary>
        public override void Shutdown()
        {
            InteractionActions = null;
            LocomotionActions = null;
            GenericActions = null;
            inputActions.Shutdown();

            // Remove callbacks from mobile navigation
            foreach (var stick in sticks)
            {
                if (stick == null)
                {
                    continue;
                }

                if (stick.StickType == MAGESScreenStick.MovementType.MoveCamera)
                {
                    stick.OnValueChanged -= ControllerActions.CustomMoveCamera;
                }
                else if (stick.StickType == MAGESScreenStick.MovementType.RotateCamera)
                {
                    stick.OnValueChanged -= ControllerActions.CustomRotateCamera;
                }
            }

            // Destroy mobile navigation if neccessary.
            if (instantiatedMobileNavigation != null)
            {
                Destroy(instantiatedMobileNavigation);
            }
        }

        /// <inheritdoc/>
        public override void UpdateModule(UpdatePhase updatePhase)
        {
            switch (CurrentMode)
            {
                case CameraMode.UniversalXR:
                    switch (updatePhase)
                    {
                        case UpdatePhase.Update:
                        case UpdatePhase.OnPreRender:

                            // Update head position/rotation
                            var headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
                            if (headDevice.isValid)
                            {
                                if (headDevice.TryGetFeatureValue(CommonUsages.devicePosition, out var pos))
                                {
                                    currentHeadPose.position = pos;
                                }

                                if (headDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out var rot))
                                {
                                    currentHeadPose.rotation = rot;
                                }

                                // Apply position/rotation to head.
                                Hub.Instance.Rig.HeadPoint.transform.SetLocalPose(currentHeadPose);
                                Hub.Instance.Rig.HeadPoint.transform.TransformBySelfLocalSpace(currentLoaderOffset.HeadOffset);
                            }

                            // Update left controller position/rotation.
                            var leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                            if (leftController.isValid)
                            {
                                if (leftController.TryGetFeatureValue(CommonUsages.devicePosition, out var pos))
                                {
                                    currentLeftControllerPose.position = pos;
                                }

                                if (leftController.TryGetFeatureValue(CommonUsages.deviceRotation, out var rot))
                                {
                                    currentLeftControllerPose.rotation = rot;
                                }

                                // Apply position/rotation to head.
                                Hub.Instance.Rig.LeftControllerPoint.transform.SetLocalPose(currentLeftControllerPose);
                                Hub.Instance.Rig.LeftControllerPoint.transform.TransformBySelfLocalSpace(currentLoaderOffset.LeftControllerOffset);
                            }

                            // Update right controller position/rotation.
                            var rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                            if (rightController.isValid)
                            {
                                if (rightController.TryGetFeatureValue(CommonUsages.devicePosition, out var pos))
                                {
                                    currentRightControllerPose.position = pos;
                                }

                                if (rightController.TryGetFeatureValue(CommonUsages.deviceRotation, out var rot))
                                {
                                    currentRightControllerPose.rotation = rot;
                                }

                                // Apply position/rotation to head.
                                Hub.Instance.Rig.RightControllerPoint.transform.SetLocalPose(currentRightControllerPose);
                                Hub.Instance.Rig.RightControllerPoint.transform.TransformBySelfLocalSpace(currentLoaderOffset.RightControllerOffset);
                            }

                            break;
                    }

                    break;

                case CameraMode.Mobile3D:
                    // Update camera rotation
                    if (updatePhase == UpdatePhase.Update)
                    {
                        Hub.Instance.Rig.HeadPoint.RotateAround(Hub.Instance.Rig.HeadPoint.position, Hub.Instance.Rig.HeadPoint.right, -smoothTurnInput.y * Mobile3DCameraRotationSpeed * Time.deltaTime);
                    }

                    break;
            }
        }

        /// <inheritdoc/>
        public override bool IsDeviceAvailable(Device device)
        {
            switch (device)
            {
#if XR_MANAGEMENT
                case DeviceManagerModule.Device.HeadMountedDisplay:
                    return InputDevices.GetDeviceAtXRNode(XRNode.Head).isValid;
                case DeviceManagerModule.Device.LeftController:
                    return InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).isValid;
                case DeviceManagerModule.Device.RightController:
                    return InputDevices.GetDeviceAtXRNode(XRNode.RightHand).isValid;
#endif
                default:
                    return false;
            }
        }

        /// <inheritdoc/>
        public override bool TrySendHapticImpulse(Device device, float intensity = 0.8f, float duration = 0.1f)
        {
#if XR_MANAGEMENT
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
#endif
            return false;
        }

        private void SetupInputDevices(CameraMode mode)
        {
            SetupHead(Hub.Instance.Rig.HeadPoint.gameObject);
            SetupControllers();

#if XR_MANAGEMENT
            if (mode == CameraMode.UniversalXR)
            {
                Hub.Instance.StartCoroutine(SetLoaderOffsets());
            }
#endif
        }

        private void SetupHead(GameObject headGameObject)
        {
            switch (CurrentMode)
            {
                case CameraMode.XREmulator:
                    break;

                case CameraMode.Mobile3D:
                    // Only adjust camera height.
                    headGameObject.transform.localPosition = new Vector3(0, CameraHeight, 0);
                    break;
            }
        }

        private void SetupControllers()
        {
            switch (CurrentMode)
            {
                case CameraMode.XREmulator:
                    break;

                case CameraMode.Mobile3D:

                    if (MobileNavigationUI == null)
                    {
                        instantiatedMobileNavigation = Resources.Load<GameObject>(MobileNavigationAssetPath);
                    }
                    else
                    {
                        instantiatedMobileNavigation = MobileNavigationUI;
                    }

                    if (instantiatedMobileNavigation == null)
                    {
                        break;
                    }

                    instantiatedMobileNavigation = Instantiate(instantiatedMobileNavigation);
                    instantiatedMobileNavigation.name = "MobileNavigation";

                    if (instantiatedMobileNavigation == null)
                    {
                        break;
                    }

                    instantiatedMobileNavigation.transform.SetParent(Hub.Instance.Rig.transform);

                    // Attach callbacks to mobile navigation
                    sticks = instantiatedMobileNavigation.transform.GetComponentsInChildren<MAGESScreenStick>();

                    foreach (MAGESScreenStick stick in sticks)
                    {
                        if (stick.StickType == MAGESScreenStick.MovementType.MoveCamera)
                        {
                            stick.OnValueChanged += ControllerActions.CustomMoveCamera;
                        }
                        else if (stick.StickType == MAGESScreenStick.MovementType.RotateCamera)
                        {
                            stick.OnValueChanged += ControllerActions.CustomRotateCamera;
                        }
                    }

                    break;
            }
        }

        private IEnumerator SetLoaderOffsets()
        {
#if XR_MANAGEMENT
            IReadOnlyList<XRLoader> loaders = null;
            while (loaders == null || loaders.Count == 0)
            {
                loaders = XRGeneralSettings.Instance?.Manager?.activeLoaders;
                yield return null;
            }

            XRLoader activeLoader = loaders[0];

            var offsetIndex = XRLoaderOffsets.FindIndex(offset => offset.Loader == activeLoader.name);
            if (offsetIndex >= 0)
            {
                currentLoaderOffset = XRLoaderOffsets[offsetIndex];
            }
#endif
        }

        private void SetupXR()
        {
#if XR_MANAGEMENT
            switch (CurrentMode)
            {
                case CameraMode.UniversalXR:
                    Hub.Instance.StartCoroutine(StartXRCoroutine());
                    break;
                default:
                    Hub.Instance.StartCoroutine(StopXRCoroutine());
                    break;
            }
#endif
        }

        private void OnSmoothTurn(Vector2 input)
        {
            smoothTurnInput = input;
        }

#if XR_MANAGEMENT

        private IEnumerator StartXRCoroutine()
        {
            XRLoader m_SelectedXRLoader;
            if (XRGeneralSettings.Instance?.Manager?.activeLoader != null)
            {
                yield break;
            }
            else
            {
                if (XRGeneralSettings.Instance == null || XRGeneralSettings.Instance.Manager == null || XRGeneralSettings.Instance.Manager.activeLoaders == null || XRGeneralSettings.Instance.Manager.activeLoaders.Count == 0)
                {
                    Debug.LogError("No XR Loader is active. Please make sure that you have one or more XR Loaders active in the XR Plugin Management section of the Project Settings.");
                    yield break;
                }

                m_SelectedXRLoader = XRGeneralSettings.Instance.Manager.activeLoaders[0];
            }

            var initSuccess = m_SelectedXRLoader.Initialize();
            if (initSuccess)
            {
                yield return null;
                var startSuccess = m_SelectedXRLoader.Start();
                if (!startSuccess)
                {
                    yield return null;
                    m_SelectedXRLoader.Deinitialize();
                }
            }
        }

        private IEnumerator StopXRCoroutine()
        {
            yield return null;

            var activeLoader = XRGeneralSettings.Instance?.Manager?.activeLoader;
            if (activeLoader == null)
            {
                yield break;
            }

            activeLoader.Stop();
            activeLoader.Deinitialize();

            yield break;
        }
#endif

        /// <summary>
        /// Offset for controllers for a specific loader.
        /// </summary>
        [System.Serializable]
        public struct XRLoaderOffset
        {
            /// <summary>
            /// The loader this offset is for.
            /// </summary>
            public string Loader;

            /// <summary>
            /// The camera offset.
            /// </summary>
            public Pose HeadOffset;

            /// <summary>
            /// The left controller's offset.
            /// </summary>
            public Pose LeftControllerOffset;

            /// <summary>
            /// The right controller's offset.
            /// </summary>
            public Pose RightControllerOffset;

            private static readonly XRLoaderOffset IdentityOffset = new XRLoaderOffset { Loader = string.Empty, HeadOffset = Pose.identity, LeftControllerOffset = Pose.identity, RightControllerOffset = Pose.identity };

            /// <summary>
            /// Gets the zero position, and identity rotation offset.
            /// </summary>
            public static XRLoaderOffset Identity => IdentityOffset;

            /// <inheritdoc/>
            public override string ToString()
            {
                return $"[{Loader}] - Head: {HeadOffset}, Left: {LeftControllerOffset}, Right: {RightControllerOffset}";
            }
        }
    }
}
