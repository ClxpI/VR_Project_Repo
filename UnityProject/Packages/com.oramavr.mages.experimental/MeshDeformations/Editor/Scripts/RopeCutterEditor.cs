namespace MAGES.MeshDeformations.Editor
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Custom Inspector for <see cref="Cutter"/> Component.
    /// </summary>
    [CustomEditor(typeof(RopeCutter))]
    internal class RopeCutterEditor : Editor
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
            var zsSsiCEhjzhVvIkifGbkOZoKtbkPkuVI = Handles.zTest;
            var LZLzgjhadsqIPljvxXmfTDaHVLAUPIid = Handles.matrix;
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
            Handles.matrix = Matrix4x4.identity;
            Handles.DrawSolidDisc(((RopeCutter)target).transform.position, ((RopeCutter)target).transform.forward, ((RopeCutter)target).CutRadius);
            Handles.zTest = zsSsiCEhjzhVvIkifGbkOZoKtbkPkuVI;
            Handles.matrix = LZLzgjhadsqIPljvxXmfTDaHVLAUPIid;
        }
    }
}