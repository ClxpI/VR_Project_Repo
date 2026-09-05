namespace MAGES.DeviceManager.Editor
{
    using UnityEditor;
    using UnityEngine.UIElements;

    /// <summary>
    /// Custom editor for MAGES InputController.
    /// </summary>
    [CustomEditor(typeof(InputController))]
    [CanEditMultipleObjects]
    public class InputControllerEditor : Editor
    {
        [HardAssetReference(HardAssetReferenceAttribute.Type.Asset)]
        private const string UXMLAssetPath = "Packages/com.oramavr.mages.core/DeviceManager/Editor/v1/InputControllerEditor.uxml";

        private InputController inputController;
        private VisualElement root;

        private VisualElement twoDoF;
        private VisualElement streams;
        private VisualElement events;

        /// <summary>
        /// Creates a visual element that represents the InputControllerEditor.
        /// </summary>
        /// <returns>A copy of the new visual element.</returns>
        public override VisualElement CreateInspectorGUI()
        {
            base.CreateInspectorGUI();
            root = new VisualElement();
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXMLAssetPath);
            visualTree.CloneTree(root);
            inputController = target as InputController;

            var eventsToggle = root.Q<Toggle>("EventsToggle");
            if (eventsToggle != null)
            {
                eventsToggle.RegisterValueChangedCallback((ve) => ValueChangedGenericEvents(ve.newValue));
                eventsToggle.value = inputController.GenericEvents;
                ValueChangedGenericEvents(inputController.GenericEvents);
            }

            var interactionToggle = root.Q<Toggle>("InteractionToggle");
            if (interactionToggle != null)
            {
                interactionToggle.RegisterValueChangedCallback((ve) => ValueChangedInteraction(ve.newValue));
                interactionToggle.value = inputController.Interaction;
                ValueChangedInteraction(inputController.Interaction);
            }

            var locomotionToggle = root.Q<Toggle>("LocomotionToggle");
            if (locomotionToggle != null)
            {
                locomotionToggle.RegisterValueChangedCallback((ve) => ValueChangedLocomotion(ve.newValue));
                locomotionToggle.value = inputController.Locomotion;
                ValueChangedLocomotion(inputController.Locomotion);
            }

            var mobile3dToggle = root.Q<Toggle>("Mobile3DToggle");
            if (mobile3dToggle != null)
            {
                mobile3dToggle.RegisterValueChangedCallback((ve) => ValueChangedMobile3DEvents(ve.newValue));
                mobile3dToggle.value = inputController.Mobile3D;
                ValueChangedMobile3DEvents(inputController.Mobile3D);
            }

            Undo.undoRedoPerformed += OnUndoRedo;

            return root;
        }

        private void OnUndoRedo()
        {
            var eventsToggle = root.Q<Toggle>("EventsToggle");
            if (eventsToggle != null)
            {
                eventsToggle.value = inputController.GenericEvents;
                ValueChangedGenericEvents(inputController.GenericEvents);
            }

            var interactionToggle = root.Q<Toggle>("InteractionToggle");
            if (interactionToggle != null)
            {
                interactionToggle.value = inputController.Interaction;
                ValueChangedInteraction(inputController.Interaction);
            }

            var locomotionToggle = root.Q<Toggle>("LocomotionToggle");
            if (locomotionToggle != null)
            {
                locomotionToggle.value = inputController.Locomotion;
                ValueChangedLocomotion(inputController.Locomotion);
            }
        }

        private void ValueChangedGenericEvents(bool newValue)
        {
            InputController inputController = target as InputController;

            var oldValue = inputController.GenericEvents;
            events = root.Q<VisualElement>("Generic_Events");
            events.style.display = newValue ? DisplayStyle.Flex : DisplayStyle.None;

            if (oldValue != newValue)
            {
                Undo.RecordObject(inputController, "Changed Generic Events");
                inputController.GenericEvents = newValue;
                EditorUtility.SetDirty(inputController);
            }
        }

        private void ValueChangedInteraction(bool newValue)
        {
            InputController inputController = target as InputController;
            var oldValue = inputController.Interaction;

            events = root.Q<VisualElement>("Interaction_Events");
            events.style.display = newValue ? DisplayStyle.Flex : DisplayStyle.None;

            if (oldValue != newValue)
            {
                UnityEngine.Debug.Log("Record");
                Undo.RecordObject(inputController, "Changed Interaction");
                inputController.Interaction = newValue;
                EditorUtility.SetDirty(inputController);
            }
        }

        private void ValueChangedLocomotion(bool newValue)
        {
            InputController inputController = target as InputController;
            var oldValue = inputController.Locomotion;

            events = root.Q<VisualElement>("Locomotion_Events");
            events.style.display = newValue ? DisplayStyle.Flex : DisplayStyle.None;

            if (oldValue != newValue)
            {
                Undo.RecordObject(inputController, "Changed Locomotion");
                inputController.Locomotion = newValue;
                EditorUtility.SetDirty(inputController);
            }
        }

        private void ValueChangedMobile3DEvents(bool newValue)
        {
            (target as InputController).Mobile3D = newValue;
            events = root.Q<VisualElement>("Mobile3D_Events");
            events.style.display = newValue ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}