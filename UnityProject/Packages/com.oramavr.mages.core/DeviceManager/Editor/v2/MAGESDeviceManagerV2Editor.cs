namespace MAGES.DeviceManager.Editor
{
    using MAGES.Editor.Utilities;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;

    /// <summary>
    /// Custom Editor for <see cref="MAGESDeviceManagerV2"/>.
    /// </summary>
    [CustomEditor(typeof(MAGESDeviceManagerV2))]
    public class MAGESDeviceManagerV2Editor : UnityEditor.Editor
    {
        [HardAssetReference(HardAssetReferenceAttribute.Type.Asset)]
        private const string UXMLAssetPath = "Packages/com.oramavr.mages.core/DeviceManager/Editor/v2/MAGESDeviceManagerV2Editor.uxml";

        private VisualElement root;
        private VisualTreeAsset uxml;

        private Label activeDriverInfo;
        private DropdownField driverOverrideDropdown;

        /// <inheritdoc/>
        public override VisualElement CreateInspectorGUI()
        {
            MAGESDeviceManagerV2 deviceManager = (MAGESDeviceManagerV2)target;

            root = new VisualElement();
            uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXMLAssetPath);

            if (uxml == null)
            {
                InspectorElement.FillDefaultInspector(root, serializedObject, this);
                return root;
            }

            uxml.CloneTree(root);

            // Active driver info
            activeDriverInfo = root.Q<Label>("active-driver");
            UpdateActiveDriverInfo();

            // Drivers
            driverOverrideDropdown = root.Q<DropdownField>("driver-override");
            PopulateDriverOverrideDropdown();
            driverOverrideDropdown.value = deviceManager.DriverOverride != null ? deviceManager.DriverOverride.Name : "None";
            driverOverrideDropdown.RegisterValueChangedCallback((evt) =>
            {
                if (evt.newValue == "None")
                {
                    deviceManager.DriverOverride = null;
                }
                else
                {
                    deviceManager.DriverOverride = deviceManager.Drivers.Find((driver) => driver.Name == evt.newValue);

                    if (deviceManager.DriverOverride == null)
                    {
                        driverOverrideDropdown.SetValueWithoutNotify("None");
                    }
                }

                serializedObject.Update();
            });

            ListView driversListView = root.Q<ListView>("drivers");
            if (driversListView != null)
            {
                driversListView.makeItem = () =>
                {
                    return new PropertyField();
                };
                driversListView.bindItem = (element, i) =>
                {
                    SerializedProperty property = serializedObject.FindFieldProperty("Drivers").GetArrayElementAtIndex(i);
                    IDriver driver = property.managedReferenceValue as IDriver;

                    PropertyField field = (PropertyField)element;
                    field.Label($"<b>{(driver == null ? "Missing Driver" : driver.Name)}</b>{(driver == null ? string.Empty : $" ({driver.DriverType} Driver)")}").BindProperty(property);
                };
                driversListView.BindProperty(serializedObject.FindFieldProperty("Drivers"));

                Button refreshDriversBtn = root.Q<Button>("refresh-drivers");
                refreshDriversBtn.clicked += () =>
                {
                    Undo.RecordObject(deviceManager, "Update Available Drivers");
                    deviceManager.UpdateAvailableDrivers();
                    serializedObject.Update();
                };
            }

            return root;
        }

        private void OnSceneGUI()
        {
            UpdateActiveDriverInfo();
        }

        private void PopulateDriverOverrideDropdown()
        {
            MAGESDeviceManagerV2 deviceManager = (MAGESDeviceManagerV2)target;

            // Add options
            if (driverOverrideDropdown != null)
            {
                driverOverrideDropdown.choices.Clear();

                driverOverrideDropdown.choices.Add("None");

                // Get all available drivers from the device manager
                foreach (IDriver driver in deviceManager.Drivers)
                {
                    driverOverrideDropdown.choices.Add(driver.Name);
                }
            }
        }

        private void UpdateActiveDriverInfo()
        {
            MAGESDeviceManagerV2 deviceManager = (MAGESDeviceManagerV2)target;

            if (activeDriverInfo != null)
            {
                if (deviceManager.ActiveDriver == null)
                {
                    activeDriverInfo.text = "Active Driver: <b>-</b>";
                }
                else
                {
                    activeDriverInfo.text = $"Active Driver: <b>{deviceManager.ActiveDriver.Name}</b> ({deviceManager.ActiveDriver.DriverType} Driver)";
                }
            }
        }
    }
}
