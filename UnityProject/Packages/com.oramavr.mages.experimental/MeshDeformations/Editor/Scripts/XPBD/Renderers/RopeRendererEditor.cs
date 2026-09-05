namespace MAGES.MeshDeformations.Editor
{
    using MAGES.Editor.Utilities;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    /// <summary>
    /// Custom Inspector for <see cref="RopeRenderer"/>.
    /// </summary>
    [CustomEditor(typeof(RopeRenderer))]
    internal class RopeRendererEditor : Editor
    {
        private static readonly string GwVsLxekqplwEjYeoxYWMeCVTgOojCRf = "Packages/com.oramavr.mages.experimental/MeshDeformations/Editor/UI/Renderers/RopeRendererEditor.uxml";

        private VisualElement CKYJObhGNPBVEnvAExsmbmFFRQZzfnbs;

        /// <inheritdoc/>
        public override VisualElement CreateInspectorGUI()
        {
            var XOFvCVXljWjVvjeGSsrOhkrWGyfYECSe = target as RopeRenderer;
            var tTRmdCQJeBqriiGDOhsmcLhowoOLZQhg = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(GwVsLxekqplwEjYeoxYWMeCVTgOojCRf);

            if (tTRmdCQJeBqriiGDOhsmcLhowoOLZQhg == null)
            {
                Debug.LogError($"Failed to load UXML file at path: {GwVsLxekqplwEjYeoxYWMeCVTgOojCRf}");
                return base.CreateInspectorGUI();
            }

            CKYJObhGNPBVEnvAExsmbmFFRQZzfnbs = tTRmdCQJeBqriiGDOhsmcLhowoOLZQhg.Instantiate();

            var WpzonkisILkpeOtZPDMyCzNKshkxWZUL = CKYJObhGNPBVEnvAExsmbmFFRQZzfnbs.Q<FloatField>("thickness");
            WpzonkisILkpeOtZPDMyCzNKshkxWZUL.value = XOFvCVXljWjVvjeGSsrOhkrWGyfYECSe.Thickness;
            WpzonkisILkpeOtZPDMyCzNKshkxWZUL.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(XOFvCVXljWjVvjeGSsrOhkrWGyfYECSe, "Changed Property in RigidbodyActor");
                XOFvCVXljWjVvjeGSsrOhkrWGyfYECSe.Thickness = evt.newValue;
                EditorUtility.SetDirty(XOFvCVXljWjVvjeGSsrOhkrWGyfYECSe);
            });

            var ZnidXpdnasnHSrGATZlnJNqujQmxjxPu = CKYJObhGNPBVEnvAExsmbmFFRQZzfnbs.Q<IntegerField>("smoothing-points");
            ZnidXpdnasnHSrGATZlnJNqujQmxjxPu.value = XOFvCVXljWjVvjeGSsrOhkrWGyfYECSe.SmoothingPointsPerSegment;
            ZnidXpdnasnHSrGATZlnJNqujQmxjxPu.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(XOFvCVXljWjVvjeGSsrOhkrWGyfYECSe, "Changed Property in RigidbodyActor");
                XOFvCVXljWjVvjeGSsrOhkrWGyfYECSe.SmoothingPointsPerSegment = evt.newValue;
                EditorUtility.SetDirty(XOFvCVXljWjVvjeGSsrOhkrWGyfYECSe);

                ZnidXpdnasnHSrGATZlnJNqujQmxjxPu.SetValueWithoutNotify(XOFvCVXljWjVvjeGSsrOhkrWGyfYECSe.SmoothingPointsPerSegment);
            });

            var OQOuoPqOGxBaudiRooPIbSlyLqkgdYSN = CKYJObhGNPBVEnvAExsmbmFFRQZzfnbs.Q<IntegerField>("slices");
            OQOuoPqOGxBaudiRooPIbSlyLqkgdYSN.value = XOFvCVXljWjVvjeGSsrOhkrWGyfYECSe.Slices;
            OQOuoPqOGxBaudiRooPIbSlyLqkgdYSN.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(XOFvCVXljWjVvjeGSsrOhkrWGyfYECSe, "Changed Property in RigidbodyActor");
                XOFvCVXljWjVvjeGSsrOhkrWGyfYECSe.Slices = evt.newValue;
                EditorUtility.SetDirty(XOFvCVXljWjVvjeGSsrOhkrWGyfYECSe);

                OQOuoPqOGxBaudiRooPIbSlyLqkgdYSN.SetValueWithoutNotify(XOFvCVXljWjVvjeGSsrOhkrWGyfYECSe.Slices);
            });

            var drfJRLJxcpKDrQkBoyXExXuNbXSYuRcL = CKYJObhGNPBVEnvAExsmbmFFRQZzfnbs.Q<PropertyField>("generate-end-caps");
            drfJRLJxcpKDrQkBoyXExXuNbXSYuRcL.BindProperty(serializedObject.FindFieldProperty("GenerateEndCaps"));

            var NjbQhHvmjdPToQMTiCCJnWcRgjyNVqlH = CKYJObhGNPBVEnvAExsmbmFFRQZzfnbs.Q<PropertyField>("rendering-layer-mask");
            NjbQhHvmjdPToQMTiCCJnWcRgjyNVqlH.BindProperty(serializedObject.FindFieldProperty("RenderingLayerMask"));

            var WVoFHCNAhSURTkCmAIjLCECAOEDBrKgT = CKYJObhGNPBVEnvAExsmbmFFRQZzfnbs.Q<PropertyField>("cast-shadows");
            WVoFHCNAhSURTkCmAIjLCECAOEDBrKgT.BindProperty(serializedObject.FindFieldProperty("CastShadows"));

            var OYvkbhiObnyzPDAakAvJOUUqNouZnWeu = CKYJObhGNPBVEnvAExsmbmFFRQZzfnbs.Q<PropertyField>("receive-shadows");
            OYvkbhiObnyzPDAakAvJOUUqNouZnWeu.BindProperty(serializedObject.FindFieldProperty("ReceiveShadows"));

            var gGTiZUOUHMREVVsQhKZlTUHHJGqsIyhm = CKYJObhGNPBVEnvAExsmbmFFRQZzfnbs.Q<PropertyField>("material");
            gGTiZUOUHMREVVsQhKZlTUHHJGqsIyhm.BindProperty(serializedObject.FindFieldProperty("Material"));

            return CKYJObhGNPBVEnvAExsmbmFFRQZzfnbs;
        }
    }
}
