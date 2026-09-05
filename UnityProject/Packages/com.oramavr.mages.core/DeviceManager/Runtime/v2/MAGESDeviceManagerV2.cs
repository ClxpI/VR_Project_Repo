namespace MAGES.DeviceManager
{
    using System;
    using System.Collections.Generic;
    using MAGES.ComponentLibrary.Framework;
    using MAGES.Utilities;
    using UnityEngine;

    /// <summary>
    /// MAGES device manager.
    /// </summary>
    /// <seealso cref="IDriver"/>
    [CreateAssetMenu(fileName = "MAGESDeviceManagerV2", menuName = "MAGES/DeviceManager/MAGESDeviceManagerV2")]
    public class MAGESDeviceManagerV2 : DeviceManagerModule, ILocomotionActions, IGenericInputActions, IInteractionActions, IMobile3DActions
    {
        private IDriver activeDriver = null;

        // All actions for events
        private Action<Vector2> onMove;
        private Action<float> onSnapTurn;
        private Action<float> onChangeHeight;
        private Action<Vector2> onSmoothTurn;
        private Action onPerform;
        private Action onUndo;
        private Action onEnableMovement;
        private Action onToggleOptionsMenu;
        private Action onToggleRaycast;
        private Action onSelectLeft;
        private Action onUISelectRight;
        private Action onDeactivateRight;
        private Action onActivateRight;
        private Action onDeselectRight;
        private Action onSelectRight;
        private Action onUISelectCancelLeft;
        private Action onUISelectLeft;
        private Action onDeactivateLeft;
        private Action onActivateLeft;
        private Action onDeselectLeft;
        private Action onUISelectCancelRight;
        private Action<Vector2, string> onPointerPosition;
        private Action<string, bool[]> onPointerPress;
        private Action<string, bool[]> onPointerHold;

        private float leftSelectionIntensity;
        private float leftActivationIntensity;
        private float rightSelectionIntensity;
        private float rightActivationIntensity;

        private float snapTurnVal = 0f;

        /// <inheritdoc/>
        [Obsolete("This is obsolete and will always be set to CameraMode.None. Instead listen to DeviceManager.OnDriverEvent instead.")]
        public override CameraMode CurrentMode
        {
            get
            {
                return CameraMode.None;
            }

            set
            {
                // Not supported.
            }
        }

        /// <summary>
        /// Gets the currently active driver.
        /// </summary>
        public override IDriver ActiveDriver => activeDriver;

        /// <inheritdoc/>
        public override int Version => 2;

        /// <inheritdoc/>
        public override IDriver DriverOverride
        {
            get
            {
                return base.DriverOverride;
            }

            set
            {
                base.DriverOverride = value;

                if (Application.isPlaying)
                {
                    SwitchToDriverWithHighestPriority();
                }
            }
        }

        /// <summary>
        /// Gets a sorted by priority list of all the drivers.
        /// </summary>
        [field: SerializeReference]
        [field: Tooltip("Drivers sorted by priority.\nHighest available driver will be selected.")]
        public List<IDriver> Drivers { get; private set; } = new List<IDriver>();

        /// <inheritdoc/>
        public override void Startup()
        {
            GenericActions = this;
            LocomotionActions = this;
            InteractionActions = this;
            Mobile3DActions = this;

            InitializeDrivers();

            // Switch to the driver with the highest priority.
            SwitchToDriverWithHighestPriority();
        }

        /// <inheritdoc/>
        public override void Shutdown()
        {
            if (ActiveDriver != null)
            {
                ActiveDriver.UnPossess();
            }

            DestroyDrivers();
        }

        /// <inheritdoc/>
        public override void UpdateModule(UpdatePhase updatePhase)
        {
            // Update all drivers.
            foreach (var driver in Drivers)
            {
                if (driver == null)
                {
                    continue;
                }

                driver.Update(updatePhase);
            }
        }

        /// <inheritdoc/>
        public void AddActionOnMove(Action<Vector2> moveAction)
        {
            onMove += moveAction;
        }

        /// <inheritdoc/>
        public void AddActionOnSnapTurn(Action<float> snapRotate)
        {
            onSnapTurn += snapRotate;
        }

        /// <inheritdoc/>
        public void AddActionOnChangeHeight(Action<float> changeHeight)
        {
            onChangeHeight += changeHeight;
        }

        /// <inheritdoc/>
        public void AddActionOnSmoothTurn(Action<Vector2> smoothRotate)
        {
            onSmoothTurn += smoothRotate;
        }

        /// <inheritdoc/>
        public void AddActionOnSmoothTurn(Action<float> smoothTurn)
        {
            onSmoothTurn += (val) =>
            {
                var val1D = val.x;

                // Deadzone check (-0.2f, 0.2f)
                if (Mathf.Abs(val1D) < 0.2f)
                {
                    val1D = 0f;
                }
                else
                {
                    if (val1D > 0)
                    {
                        val1D = MathUtilities.Remap(val1D, 0.2f, 1.0f, 0.0f, 1.0f);
                    }
                    else if (val1D < 0)
                    {
                        val1D = MathUtilities.Remap(val1D, -0.2f, -1.0f, 0f, -1.0f);
                    }
                }

                smoothTurn?.Invoke(val1D);
            };
        }

        /// <inheritdoc/>
        public void AddActionOnPerform(Action action)
        {
            onPerform += action;
        }

        /// <inheritdoc/>
        public void AddActionOnUndo(Action action)
        {
            onUndo += action;
        }

        /// <inheritdoc/>
        public void AddActionOnEnableMovement(Action action)
        {
            onEnableMovement += action;
        }

        /// <inheritdoc/>
        public void AddActionOnEnableOptions(Action action)
        {
            onToggleOptionsMenu += action;
        }

        /// <inheritdoc/>
        public void AddActionOnToggleRaycastActivation(Action action)
        {
            onToggleRaycast += action;
        }

        /// <inheritdoc/>
        public void AddActionOnSelectLeft(Action select)
        {
            onSelectLeft += select;
        }

        /// <inheritdoc/>
        public void AddActionOnDeselectLeft(Action deselect)
        {
            onDeselectLeft += deselect;
        }

        /// <inheritdoc/>
        public void AddActionOnActivateLeft(Action activate)
        {
            onActivateLeft += activate;
        }

        /// <inheritdoc/>
        public void AddActionOnDeactivateLeft(Action deactivate)
        {
            onDeactivateLeft += deactivate;
        }

        /// <inheritdoc/>
        public void AddActionOnUISelectLeft(Action uiSelectLeft)
        {
            onUISelectLeft += uiSelectLeft;
        }

        /// <inheritdoc/>
        public void AddActionOnUISelectCancelLeft(Action uiDeselectLeft)
        {
            onUISelectCancelLeft += uiDeselectLeft;
        }

        /// <inheritdoc/>
        public void AddActionOnSelectRight(Action select)
        {
            onSelectRight += select;
        }

        /// <inheritdoc/>
        public void AddActionOnDeselectRight(Action deselect)
        {
            onDeselectRight += deselect;
        }

        /// <inheritdoc/>
        public void AddActionOnActivateRight(Action activate)
        {
            onActivateRight += activate;
        }

        /// <inheritdoc/>
        public void AddActionOnDeactivateRight(Action deactivate)
        {
            onDeactivateRight += deactivate;
        }

        /// <inheritdoc/>
        public void AddActionOnUISelectRight(Action uiSelectLeft)
        {
            onUISelectRight += uiSelectLeft;
        }

        /// <inheritdoc/>
        public void AddActionOnUISelectCancelRight(Action uiDeselectLeft)
        {
            onUISelectCancelRight += uiDeselectLeft;
        }

        /// <inheritdoc/>
        public float GetSelectionIntensityLeft()
        {
            return leftSelectionIntensity;
        }

        /// <inheritdoc/>
        public float GetSelectionIntensityRight()
        {
            return rightSelectionIntensity;
        }

        /// <inheritdoc/>
        public float GetActivationIntensityLeft()
        {
            return leftActivationIntensity;
        }

        /// <inheritdoc/>
        public float GetActivationIntensityRight()
        {
            return rightActivationIntensity;
        }

        /// <inheritdoc/>
        public void AddActionOnPointerPosition(Action<Vector2, string> pointerMoveAction)
        {
            onPointerPosition += pointerMoveAction;
        }

        /// <inheritdoc/>
        public void AddActionOnPointer(Action<string, bool[]> pointerPressAction)
        {
            onPointerPress += pointerPressAction;
        }

        /// <inheritdoc/>
        public void AddActionOnMouseHold(Action<string, bool[]> pointerHoldAction)
        {
            onPointerHold += pointerHoldAction;
        }

        /// <summary>
        /// Updates the <see cref="Drivers"/> list with all available drivers.
        /// </summary>
        public void UpdateAvailableDrivers()
        {
            // Remove any drivers that are no longer available.
            for (int i = Drivers.Count - 1; i >= 0; i--)
            {
                if (Drivers[i] == null)
                {
                    Drivers.RemoveAt(i);
                }
            }

            List<IDriver> newDriversFound = new List<IDriver>();

            // Find all available drivers and add one of each to the list.
            Type[] driverImplementationTypes = AppDomain.CurrentDomain.GetAllTypesImplementingInterface<IDriver>();
            foreach (Type type in driverImplementationTypes)
            {
                // Does this driver already exist in the list?
                if (Drivers.Exists(x => (x != null && x.GetType() == type)))
                {
                    continue;
                }

                IDriver driver = (IDriver)Activator.CreateInstance(type);
                newDriversFound.Add(driver);
            }

            // Sort new drivers with their default sort order.
            newDriversFound.Sort((x, y) => x.DefaultSortOrder.CompareTo(y.DefaultSortOrder));

            // Add the new drivers to the list.
            Drivers.AddRange(newDriversFound);
        }

        [RunOnHubInitialize]
        private static void RegisterToggleToolOnStartup()
        {
            if (Hub.GetModule<MAGESDeviceManagerV2>() == null)
            {
                return;
            }

            Hub.GetSubModule<RuntimeToolsSubModule>().RegisterTool(
                () =>
                {
                    var dm = Hub.GetModule<MAGESDeviceManagerV2>();

                    string current = dm.ActiveDriver.DriverType.ToString();
                    string other = dm.ActiveDriver.DriverType == DriverType.NonSpatial ? DriverType.Spatial.ToString() : DriverType.NonSpatial.ToString();
                    string text = $"Force {other}";

                    return text;
                },
                () =>
                {
                    var dm = Hub.GetModule<MAGESDeviceManagerV2>();
                    var typeToPick = dm.ActiveDriver.DriverType == DriverType.NonSpatial ? DriverType.Spatial : DriverType.NonSpatial;

                    foreach (var driver in dm.Drivers)
                    {
                        if (driver.DriverType == typeToPick)
                        {
                            dm.DriverOverride = driver;
                            break;
                        }
                    }
                });
        }

        private void InitializeDrivers()
        {
            // Get the driver events hooked.
            foreach (var driver in Drivers)
            {
                driver.OnDriverEvent.AddListener(OnDriverEventReceived);
                driver.Initialize();
            }
        }

        private void DestroyDrivers()
        {
            // Get the driver events unhooked.
            foreach (var driver in Drivers)
            {
                if (driver == null)
                {
                    continue;
                }

                driver.OnDriverEvent.RemoveListener(OnDriverEventReceived);
                driver.Deinitialize();
            }
        }

        /// <summary>
        /// Switch to:<br/>
        /// - <see cref="MAGESDeviceManagerV2.ActiveDriverOverride"/> if it is available.<br/>
        /// - First available driver in the <see cref="MAGESDeviceManagerV2.Drivers"/> list.<br/>
        /// </summary>
        private void SwitchToDriverWithHighestPriority()
        {
            // Check override first.
            if (DriverOverride != null)
            {
                SwitchToDriver(DriverOverride);
                return;
            }

            // Switch to the first available driver.
            foreach (var driver in Drivers)
            {
                if (driver == null || !driver.IsAvailable)
                {
                    continue;
                }

                SwitchToDriver(driver);
                return;
            }

            // No available driver found? Switch to no driver.
            SwitchToDriver(null);
        }

        /// <summary>
        /// Force switches to <paramref name="newDriver"/>.
        /// </summary>
        /// <param name="newDriver">The driver to switch to.</param>
        private void SwitchToDriver(IDriver newDriver)
        {
            // Already on the new driver.
            if (newDriver == ActiveDriver)
            {
                return;
            }

            if (ActiveDriver != null)
            {
                ActiveDriver.UnPossess();
            }

            var oldDriver = ActiveDriver;

            if (newDriver != null)
            {
                if (Hub.Instance != null)
                {
                    if (Hub.Instance.Rig != null)
                    {
                        newDriver.Possess(Hub.Instance.Rig);
                    }
                    else
                    {
                        newDriver = null;
                    }
                }
                else
                {
                    newDriver = null;
                }
            }
            else
            {
                newDriver = null;
            }

            activeDriver = newDriver;

            // Notify listeners of the change.
            OnDriverChanged?.Invoke(new DriverChangeEventArgs(oldDriver, newDriver));
        }

        /// <summary>
        /// Called from all the drivers when they have an event.
        /// </summary>
        private void OnDriverEventReceived(DriverEvent evt)
        {
            // Sanity check for driver source
            if (evt.Driver == null)
            {
                return;
            }

            // Check if event is for driver enable/disable
            if (evt.EventKind == DriverEvent.Kind.DriverStatus && evt.EventValueType == DriverEvent.ValueType.DigitalStateChange)
            {
                SwitchToDriverWithHighestPriority();
                OnDriverEvent?.Invoke(evt);
                return;
            }

            // Forward input events, only from active driver.
            if (evt.Driver != ActiveDriver)
            {
                return;
            }

            // Call the old DeviceManager's functions for compatibility.
            CastAndCallOldFunctions(evt);

            // Forward the event to DeviceManager's OnDriverEvent.
            OnDriverEvent?.Invoke(evt);
        }

        // TODO: Remove this function when support for DeviceManager's functions is removed.
        private void CastAndCallOldFunctions(DriverEvent evt)
        {
            if (evt.EventKind == DriverEvent.Kind.Input)
            {
                switch (evt.EventSource)
                {
                    case DriverEvent.SourceDevice.LeftController:
                        switch (evt.EventSourceFeature)
                        {
                            case DriverEvent.Feature.Grip:
                                if (evt.EventValueType == DriverEvent.ValueType.DigitalStateChange)
                                {
                                    if (evt.Value<bool>())
                                    {
                                        onSelectLeft?.Invoke();
                                    }
                                    else
                                    {
                                        onDeselectLeft?.Invoke();
                                    }
                                }
                                else if (evt.EventValueType == DriverEvent.ValueType.Absolute1D)
                                {
                                    leftSelectionIntensity = evt.Value<float>();
                                }

                                break;
                            case DriverEvent.Feature.Trigger:
                                if (evt.EventValueType == DriverEvent.ValueType.DigitalStateChange)
                                {
                                    if (evt.Value<bool>())
                                    {
                                        onActivateLeft?.Invoke();
                                        onUISelectLeft?.Invoke();
                                    }
                                    else
                                    {
                                        onDeactivateLeft?.Invoke();
                                        onUISelectCancelLeft?.Invoke();
                                    }
                                }
                                else if (evt.EventValueType == DriverEvent.ValueType.Absolute1D)
                                {
                                    leftActivationIntensity = evt.Value<float>();
                                }

                                break;
                            case DriverEvent.Feature.Primary2DAxis:
                                if (evt.EventValueType == DriverEvent.ValueType.Absolute2D)
                                {
                                    onMove?.Invoke(evt.Value<Vector2>());
                                }
                                else if (evt.EventValueType == DriverEvent.ValueType.DigitalStateChange && !evt.Value<bool>()) // On release
                                {
                                    onEnableMovement?.Invoke();
                                }

                                break;
                        }

                        break;
                    case DriverEvent.SourceDevice.RightController:
                        switch (evt.EventSourceFeature)
                        {
                            case DriverEvent.Feature.Grip:
                                if (evt.EventValueType == DriverEvent.ValueType.DigitalStateChange)
                                {
                                    if (evt.Value<bool>())
                                    {
                                        onSelectRight?.Invoke();
                                    }
                                    else
                                    {
                                        onDeselectRight?.Invoke();
                                    }
                                }
                                else if (evt.EventValueType == DriverEvent.ValueType.Absolute1D)
                                {
                                    rightSelectionIntensity = evt.Value<float>();
                                }

                                break;
                            case DriverEvent.Feature.Trigger:
                                if (evt.EventValueType == DriverEvent.ValueType.DigitalStateChange)
                                {
                                    if (evt.Value<bool>())
                                    {
                                        onActivateRight?.Invoke();
                                        onUISelectRight?.Invoke();
                                    }
                                    else
                                    {
                                        onDeactivateRight?.Invoke();
                                        onUISelectCancelRight?.Invoke();
                                    }
                                }
                                else if (evt.EventValueType == DriverEvent.ValueType.Absolute1D)
                                {
                                    rightActivationIntensity = evt.Value<float>();
                                }

                                break;
                            case DriverEvent.Feature.Primary2DAxis:
                                if (evt.EventValueType == DriverEvent.ValueType.Absolute2D)
                                {
                                    var val = evt.Value<Vector2>();

                                    // Snap Turn
                                    // Behave like button press
                                    if (val.x > 0.5f && snapTurnVal < 0.5f)
                                    {
                                        onSnapTurn?.Invoke(1.0f);
                                    }
                                    else if (val.x < -0.5f && snapTurnVal > -0.5f)
                                    {
                                        onSnapTurn?.Invoke(-1.0f);
                                    }

                                    snapTurnVal = val.x;

                                    // Smooth Turn
                                    onSmoothTurn?.Invoke(val);

                                    // Change height
                                    {
                                        // NOTE: adjust deadzone if needed.
                                        var val1D = val.y;

                                        // Deadzone check (-0.2f, 0.2f)
                                        if (Mathf.Abs(val1D) < 0.2f)
                                        {
                                            val1D = 0f;
                                        }
                                        else
                                        {
                                            // Remap values
                                            if (val1D > 0)
                                            {
                                                val1D = MathUtilities.Remap(val1D, 0.2f, 1.0f, 0.0f, 1.0f);
                                            }
                                            else if (val1D < 0)
                                            {
                                                val1D = MathUtilities.Remap(val1D, -0.2f, -1.0f, 0f, -1.0f);
                                            }
                                        }

                                        onChangeHeight?.Invoke(val1D);
                                    }
                                }
                                else if (evt.EventValueType == DriverEvent.ValueType.DigitalStateChange && !evt.Value<bool>()) // On release
                                {
                                    onToggleRaycast?.Invoke();
                                    onToggleOptionsMenu?.Invoke();
                                }

                                break;
                        }

                        break;
                }
            }
        }

        private void Reset()
        {
            UpdateAvailableDrivers();
        }
    }
}
