namespace MAGES.MeshDeformations.Editor
{
    using UnityEditor;
    using UnityEditor.AssetImporters;

    /// <summary>
    /// Custom Editor for the <see cref="TetrahedralMeshImporter"/> class.
    /// </summary>
    [CustomEditor(typeof(TetrahedralMeshImporter))]
    internal class TetrahedralMeshImporterEditor : ScriptedImporterEditor
    {
        /// <inheritdoc/>
        protected override bool needsApplyRevert => true;

        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            var bkhyWLygTfMiSrfdLHWrOpnaLzrFxVUM = target as TetrahedralMeshImporter;

            if (bkhyWLygTfMiSrfdLHWrOpnaLzrFxVUM != null)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("<ScaleFactor>k__BackingField"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("<ConvertUnits>k__BackingField"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("<ImportSubMeshes>k__BackingField"));
            }

            ApplyRevertGUI();
        }

        /// <inheritdoc/>
        public override void OnDisable()
        {
            base.OnDisable();
        }

        /// <inheritdoc/>
        protected override bool CanApply()
        {
            return true;
        }

        /// <inheritdoc/>
        protected override void Apply()
        {
            base.Apply();
        }
    }
}