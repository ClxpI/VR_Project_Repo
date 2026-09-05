namespace MAGES.MeshDeformations.Editor
{
    using UnityEditor;
    using UnityEngine.UIElements;

    [CustomEditor(typeof(TetrahedralMesh))]
    public class TetrahedralMeshEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var VIegcqfKitGWUjJJMgBmtuzNECcGfaFF = target as TetrahedralMesh;

            VisualElement fKNHjQauWrOjBqRdzAeKalraXShndSDT = new VisualElement();
            fKNHjQauWrOjBqRdzAeKalraXShndSDT.Add(new Label($"Vertices: {VIegcqfKitGWUjJJMgBmtuzNECcGfaFF.VertexCount}"));
            
            fKNHjQauWrOjBqRdzAeKalraXShndSDT.Add(new Label($"Tetrahedra: {VIegcqfKitGWUjJJMgBmtuzNECcGfaFF.Tetrahedra.Length/4}"));
            fKNHjQauWrOjBqRdzAeKalraXShndSDT.Add(new Label($"SubMeshes: {VIegcqfKitGWUjJJMgBmtuzNECcGfaFF.SubMeshCount}"));
            for(int WhQMFprMoVbsgxugyMWhUBGbXPLmdYgR=0; WhQMFprMoVbsgxugyMWhUBGbXPLmdYgR<VIegcqfKitGWUjJJMgBmtuzNECcGfaFF.SubMeshCount; WhQMFprMoVbsgxugyMWhUBGbXPLmdYgR++)
            {
                var xuJRwNBNSNHcqIiZQNOisSofbZobQaHk = VIegcqfKitGWUjJJMgBmtuzNECcGfaFF.GetSubMesh(WhQMFprMoVbsgxugyMWhUBGbXPLmdYgR);
                var zefxRnARAUBnibguQhJvZmtYLAKLcfhG = new Label($"[{WhQMFprMoVbsgxugyMWhUBGbXPLmdYgR}]: Tetrahedra: {xuJRwNBNSNHcqIiZQNOisSofbZobQaHk.IndexCount/4}");
                zefxRnARAUBnibguQhJvZmtYLAKLcfhG.style.marginLeft = 10;
                fKNHjQauWrOjBqRdzAeKalraXShndSDT.Add(zefxRnARAUBnibguQhJvZmtYLAKLcfhG);
            }

            return fKNHjQauWrOjBqRdzAeKalraXShndSDT;
        }
    }
}