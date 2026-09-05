namespace MAGES.MeshDeformations.Editor
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Custom Inspector for <see cref="Cutter"/> Component.
    /// </summary>
    [CustomEditor(typeof(Cutter))]
    internal class CutterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        /// <summary>
        /// Unity's OnSceneGUI method.
        /// </summary>
        public void OnSceneGUI()
        {
            var iDBVdZcGNccVAaAePFOvRBDFExInDAmS = Handles.zTest;
            var OhCMKAPxynvKVuKnYYHJODYwIkPnvZBc = Handles.matrix;
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
            Handles.matrix = Matrix4x4.identity;
            Handles.DrawSolidDisc(((Cutter)target).transform.position, ((Cutter)target).transform.forward, ((Cutter)target).CutRadius);
            Handles.zTest = iDBVdZcGNccVAaAePFOvRBDFExInDAmS;
            Handles.matrix = OhCMKAPxynvKVuKnYYHJODYwIkPnvZBc;
        }
    }
}