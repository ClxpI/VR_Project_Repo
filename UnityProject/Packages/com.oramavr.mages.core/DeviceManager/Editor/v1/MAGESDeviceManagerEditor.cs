namespace MAGES.DeviceManager.Editor
{
    using System;
    using MAGES.Editor;
    using MAGES.Editor.Utilities;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;

    /// <summary>
    /// Custom editor for MAGES DeviceManager.
    /// </summary>
    [CustomEditor(typeof(MAGESDeviceManager))]
    [CanEditMultipleObjects]
    public class MAGESDeviceManagerEditor : Editor
    {
        [HardAssetReference(HardAssetReferenceAttribute.Type.Asset)]
        private const string UXMLAssetPath = "Packages/com.oramavr.mages.core/DeviceManager/Editor/v1/MAGESDeviceManagerEditor.uxml";

        private VisualElement root;
        private Button pluginInstallButtonXR;
        private FloatField cameraHeight;
        private PropertyField controllerOffsets;
        private PropertyField mobileNavigationUI;

        /// <summary>
        /// Creates a visual element that represents the InputControllerEditor.
        /// </summary>
        /// <returns>A copy of the new visual element.</returns>
        public override VisualElement CreateInspectorGUI()
        {
            base.CreateInspectorGUI();
            root = new VisualElement();
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXMLAssetPath);
            visualTree.CloneTree(root);
            pluginInstallButtonXR = root.Q<Button>("XRPlugin");
#if XR_MANAGEMENT
            pluginInstallButtonXR.style.display = DisplayStyle.None;
#endif

            root.Q<PropertyField>("InputController")?.BindProperty(serializedObject.FindProperty("inputActions"));

            controllerOffsets = root.Q<PropertyField>("XRLoaderOffsets");
            controllerOffsets.BindProperty(serializedObject.FindFieldProperty("XRLoaderOffsets"));
#if !XR_MANAGEMENT
            controllerOffsets.style.display = DisplayStyle.None;
#endif

            var xr_ModeEnum = root.Q<EnumField>("XR_Mode");
            cameraHeight = root.Q<FloatField>("CamHeight");
            mobileNavigationUI = root.Q<PropertyField>("OverrideMobileNavigationUI");
            EnableCameraHeightOption((DeviceManagerModule.CameraMode)xr_ModeEnum.value == DeviceManagerModule.CameraMode.UniversalXR);
            xr_ModeEnum.RegisterValueChangedCallback(CheckXR_Management);
            xr_ModeEnum.RegisterValueChangedCallback(UpdateMobileNavigation);
            UpdateXR_Management(((MAGESDeviceManager)target).CurrentMode);
            SetupXRButton();
            return root;
        }

        private void UpdateMobileNavigation(ChangeEvent<Enum> evt)
        {
            if ((DeviceManagerModule.CameraMode)evt.newValue == DeviceManagerModule.CameraMode.Mobile3D)
            {
                mobileNavigationUI.style.display = DisplayStyle.Flex;
            }
            else
            {
                mobileNavigationUI.style.display = DisplayStyle.None;
            }
        }

        private void CheckXR_Management(ChangeEvent<Enum> eventXR)
        {
            DeviceManagerModule.CameraMode newValue = (DeviceManagerModule.CameraMode)eventXR.newValue;
            UpdateXR_Management(newValue);
        }

        private void SetupXRButton()
        {
            pluginInstallButtonXR.text = "Install XR Plugin";
            pluginInstallButtonXR.clickable.clicked += InstallXRPlugin;
        }

        private void UpdateXR_Management(DeviceManagerModule.CameraMode newValue)
        {
            if (newValue == DeviceManagerModule.CameraMode.UniversalXR)
            {
                EnableCameraHeightOption(false);
#if !XR_MANAGEMENT
                pluginInstallButtonXR.style.display = DisplayStyle.Flex;
                pluginInstallButtonXR.style.flexDirection = FlexDirection.Column;

#else
                pluginInstallButtonXR.style.display = DisplayStyle.None;
#endif
            }
            else
            {
                pluginInstallButtonXR.style.display = DisplayStyle.None;
                EnableCameraHeightOption(true);
            }
        }

        private void EnableCameraHeightOption(bool enabled)
        {
            cameraHeight.style.display = enabled ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void InstallXRPlugin()
        {
            bool windowResult =
                EditorUtility.DisplayDialog(
                    "XR Devices Installation/Selection",
                    "After XR installation is finished, please select the supported devices for your project",
                    "Ok",
                    "Cancel");

            if (!windowResult)
            {
                return;
            }

            PackageManagerUtility pm = new PackageManagerUtility();
            pm.InstallPackage("com.unity.xr.management", (_, success) =>
            {
                if (success)
                {
                    SettingsService.OpenProjectSettings("Project/XR Plug-in Management");
                }
            });
        }
    }
}
