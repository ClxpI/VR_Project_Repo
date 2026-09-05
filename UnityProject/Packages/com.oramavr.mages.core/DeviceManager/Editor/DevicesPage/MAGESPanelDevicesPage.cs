namespace MAGES.Editor
{
    using MAGES.DeviceManager;
    using MAGES.Editor.Utilities;
    using UnityEditor;
    using UnityEditor.PackageManager;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    /// <summary>
    /// A devices set up page for the MAGESPanel.
    /// </summary>
    internal class MAGESPanelDevicesPage : BaseMAGESPanelPage
    {
        [HardAssetReference(HardAssetReferenceAttribute.Type.Asset)]
        private const string DevicesPageUIAssetPath = "Packages/com.oramavr.mages.core/DeviceManager/Editor/DevicesPage/DevicesPage.uxml";

        [HardAssetReference(HardAssetReferenceAttribute.Type.Asset)]
        private const string DeviceCardUIAssetPath = "Packages/com.oramavr.mages.core/DeviceManager/Editor/DevicesPage/DeviceCard.uxml";

        [HardAssetReference(HardAssetReferenceAttribute.Type.Asset)]
        private const string CollapseIconPath = "Packages/com.oramavr.mages.core/DeviceManager/Editor/DevicesPage/Images/DevicesPageIcon.png";

        private VisualElement root;
        private VisualElement device1;
        private VisualElement device2;

        private TitleContent title;

        /// <inheritdoc/>
        public override int Order => 900;

        /// <inheritdoc/>
        public override VisualElement Title
        {
            get
            {
                var t = CreateOrGetTitle();
                UpdateTitle(DeveloperAuthentication.Instance.HasActiveLicense);
                return t.Root;
            }
        }

        /// <inheritdoc/>
        public override void Initialize(MAGESPanel panel)
        {
            DeveloperAuthentication.Instance.LicenseGranted.AddListener(OnLicenseGranted);
            DeveloperAuthentication.Instance.LicenseRevoked.AddListener(OnLicenseRevoked);
            base.Initialize(panel);
        }

        /// <inheritdoc/>
        public override void Destroy()
        {
            base.Destroy();
            DeveloperAuthentication.Instance.LicenseGranted.RemoveListener(OnLicenseGranted);
            DeveloperAuthentication.Instance.LicenseRevoked.RemoveListener(OnLicenseRevoked);
        }

        /// <inheritdoc/>
        public override VisualElement CreateGUI(Bundle selectedBundle)
        {
            root = new VisualElement();
            root.Add(CreateDevicesCardsUI(selectedBundle));
            return root;
        }

        /// <inheritdoc cref="BaseMAGESPanelPage.TrySelect(Bundle)"/>
        public override bool TrySelect(Bundle selectedBundle)
        {
            if (!DeveloperAuthentication.Instance.IsLoggedIn)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc cref="BaseMAGESPanelPage.OnSelectedBundleChanged"/>
        public override void OnSelectedBundleChanged(Bundle newSelection)
        {
            root?.Clear();
            root.Add(CreateDevicesCardsUI(newSelection));
        }

        private void OnLicenseGranted()
        {
            UpdateTitle(true);
        }

        private void OnLicenseRevoked()
        {
            UpdateTitle(false);
        }

        private void UpdateTitle(bool selectionEnabled)
        {
            if (title != null)
            {
                if (selectionEnabled)
                {
                    title.Label.RemoveFromClassList("title-disabled");
                    title.Label.AddToClassList("title-enabled");
                    title.Icon.RemoveFromClassList("title-disabled");
                    title.Icon.AddToClassList("title-enabled");
                }
                else
                {
                    title.Label.RemoveFromClassList("title-enabled");
                    title.Label.AddToClassList("title-disabled");
                    title.Icon.RemoveFromClassList("title-enabled");
                    title.Icon.AddToClassList("title-disabled");
                }
            }
        }

        private TitleContent CreateOrGetTitle()
        {
            if (title == null)
            {
                title = new TitleContent();
                title.Root = new VisualElement();

                // Icon
                title.Icon = new VisualElement();
                title.Icon.style.backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Sprite>(CollapseIconPath));
                title.Icon.name = "icon";
                title.Icon.style.width = 32;
                title.Icon.style.height = 32;
                title.Icon.tooltip = "Devices";
                title.Root.Add(title.Icon);

                // Label
                title.Label = new Label("Devices");
                title.Label.name = "title";
                title.Root.Add(title.Label);

                title.Root.style.flexDirection = FlexDirection.Row;
                Panel.OnCollapseStateChanged.AddListener((collapsed) =>
                {
                    if (collapsed)
                    {
                        title.Root.Q<VisualElement>("icon").style.display = DisplayStyle.Flex;
                        title.Root.Q<Label>("title").style.display = DisplayStyle.None;
                        title.Root.style.alignSelf = Align.Center;
                    }
                    else
                    {
                        title.Root.Q<VisualElement>("icon").style.display = DisplayStyle.None;
                        title.Root.Q<Label>("title").style.display = DisplayStyle.Flex;
                        title.Root.style.alignSelf = Align.FlexStart;
                    }
                });
            }

            return title;
        }

        private void FixDeviceTask()
        {
            PackageManagerUtility pm = new PackageManagerUtility();
            pm.ListPackages(false, true, (PackageCollection collection) =>
            {
                foreach (var package in collection)
                {
                    if (package.name == "com.unity.xr.management")
                    {
                        return;
                    }
                }

                InstallXRPluginManagement();
            });
        }

        private void SetInitializeXROnStartup(bool enabled)
        {
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets/XR/XRGeneralSettingsPerBuildTarget.asset");
            if (asset == null)
            {
                return;
            }

            // The XR Settings contains a list for keys and a list for values. We must change the m_InitManagerOnStart property for each value.
            SerializedObject settings = new SerializedObject(asset);
            var vals = settings.FindProperty("Values");
            if (vals != null)
            {
                for (int i = 0; i < vals.arraySize; i++)
                {
                    var inst = vals.GetArrayElementAtIndex(i).objectReferenceValue;
                    SerializedObject obj = new SerializedObject(inst);
                    var property = obj.FindProperty("m_InitManagerOnStart");
                    if (property != null)
                    {
                        property.boolValue = enabled;
                        obj.ApplyModifiedProperties();
                    }
                }
            }
        }

        private VisualElement CreateDevicesCardsUI(Bundle selectedBundle)
        {
            if (selectedBundle == null || selectedBundle.DeviceManager == null)
            {
                Label errorLabel = new Label("<color=red>The selected bundle does not have a DeviceManagerModule attached.</color>");
                return errorLabel;
            }

            if (selectedBundle.DeviceManager is MAGESDeviceManagerV2)
            {
                VisualElement root = new VisualElement();
                root.style.flexDirection = FlexDirection.Column;
                root.style.flexGrow = 1;
                root.style.flexShrink = 1;

                Label warningLabel = new Label("<color=yellow><b>You are using MAGES Device Manager V2.</b><br>Device selection is handled automatically. You can modify the device priority below:</color>");
                root.Add(warningLabel);
                warningLabel.style.marginLeft = 14;
                warningLabel.style.marginBottom = 14;

                InspectorElement element = new InspectorElement(selectedBundle.DeviceManager);
                root.Add(element);

                return root;
            }

            if (selectedBundle.DeviceManager is MAGESDeviceManager)
            {
                VisualElement root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(DevicesPageUIAssetPath).Instantiate();
                var parent = root.Q<VisualElement>("samples").Q<VisualElement>("content");

                device1 = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(DeviceCardUIAssetPath).Instantiate();
                device1.Q<Label>("sample-title").text = "XR Devices";
                device1.Q<Label>("sample-description").text = "Support for devices through the Unity XR Management plugin";
                device1.Q<Button>("select").text = selectedBundle.DeviceManager.CurrentMode == DeviceManagerModule.CameraMode.UniversalXR ? "Selected" : "Select";
                device1.Q<Button>("select").clicked += () =>
                {
                    FixDeviceTask();

                    device1.Q<Button>("select").text = "Selected";
                    device2.Q<Button>("select").text = "Select";

                    device1.Q<Button>("select").SetEnabled(false);
                    device2.Q<Button>("select").SetEnabled(true);

                    selectedBundle.DeviceManager.CurrentMode = DeviceManagerModule.CameraMode.UniversalXR;
                    EditorUtility.SetDirty(selectedBundle.DeviceManager);

                    SetInitializeXROnStartup(true);
                };
                device1.Q<Button>("select").SetEnabled(selectedBundle.DeviceManager.CurrentMode != DeviceManagerModule.CameraMode.UniversalXR);
                parent.Add(device1);

                device2 = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(DeviceCardUIAssetPath).Instantiate();
                device2.Q<Label>("sample-title").text = "Mobile 3D";
                device2.Q<Label>("sample-description").text = "Support for mobile and desktop devices without the need for a XR headset";
                device2.Q<Button>("select").text = selectedBundle.DeviceManager.CurrentMode == DeviceManagerModule.CameraMode.Mobile3D ? "Selected" : "Select";
                device2.Q<Button>("select").clicked += () =>
                {
                    device1.Q<Button>("select").text = "Select";
                    device2.Q<Button>("select").text = "Selected";

                    device1.Q<Button>("select").SetEnabled(true);
                    device2.Q<Button>("select").SetEnabled(false);

                    selectedBundle.DeviceManager.CurrentMode = DeviceManagerModule.CameraMode.Mobile3D;
                    EditorUtility.SetDirty(selectedBundle.DeviceManager);

                    SetInitializeXROnStartup(false);
                };
                device2.Q<Button>("select").SetEnabled(selectedBundle.DeviceManager.CurrentMode != DeviceManagerModule.CameraMode.Mobile3D);
                parent.Add(device2);

                return root;
            }

            return new VisualElement();
        }

        private void InstallXRPluginManagement()
        {
            if (EditorUtility.DisplayDialog("Warning", "You are about to install XR Plugin Management", "Continue", "Cancel"))
            {
                PackageManagerUtility pm = new PackageManagerUtility();
                pm.InstallPackage("com.unity.xr.management", (_, success) =>
                {
                    if (!success)
                    {
                        EditorUtility.DisplayDialog("Error", "Something went wrong while trying to install XR Plugin Management. Please try again later and if the error persists, install it manually.", "Ok");
                    }
                    else
                    {
                        OpenXRPluginManagementSettingsAfterRecompile.SetEnabled(true);
                    }
                });
            }
        }

        private class TitleContent
        {
            public VisualElement Root { get; set; }

            public Label Label { get; set; }

            public VisualElement Icon { get; set; }
        }

        /// <summary>
        /// Helper class to open the XR Plugin Management settings after recompiling.
        /// </summary>
        [InitializeOnLoad]
#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1204 // Static elements should appear before instance elements
        internal static class OpenXRPluginManagementSettingsAfterRecompile
#pragma warning restore SA1204 // Static elements should appear before instance elements
#pragma warning restore SA1402 // File may only contain a single type
        {
            static OpenXRPluginManagementSettingsAfterRecompile()
            {
                if (PlayerPrefs.GetString("OpenXRSettingsAfterCompile", "false") == "true")
                {
                    PlayerPrefs.DeleteKey("OpenXRSettingsAfterCompile");
                    EditorApplication.update += AfterInstallationCompleted;
                }
            }

            /// <summary>
            /// Enable/Disable the helper.
            /// </summary>
            /// <param name="value"><see langword="true"/> to open the settings after next compile.</param>
            public static void SetEnabled(bool value)
            {
                if (value)
                {
                    PlayerPrefs.SetString("OpenXRSettingsAfterCompile", "true");
                }
                else
                {
                    PlayerPrefs.DeleteKey("OpenXRSettingsAfterCompile");
                }
            }

            private static void AfterInstallationCompleted()
            {
                EditorApplication.update -= AfterInstallationCompleted;

                // Display Success Message and Open Settings
                EditorUtility.DisplayDialog("Success", "XR Plugin management was installed successfully. You may now select the XR Plugin provider.", "Ok");
                new EditorDelay(500, () =>
                {
                    SettingsService.OpenProjectSettings("Project/XR Plug-in Management");
                });
            }
        }
    }
}
