namespace MAGES.MeshDeformations.Editor
{
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;

    /// <summary>
    /// Custom Editor for <see cref="SimulationMesh"/>.
    /// </summary>
    [CustomEditor(typeof(SimulationMesh))]
    public class SimulationMeshEditor : Editor
    {
        /// <inheritdoc/>
        public override VisualElement CreateInspectorGUI()
        {
            var ffenlAMiAMxcaopMspJuIfEdPKivCQGu = new VisualElement();

            HelpBox GWtlfNruNzLBcjSZrgKBfRucNYYhKGgJ = new HelpBox();
            GWtlfNruNzLBcjSZrgKBfRucNYYhKGgJ.messageType = HelpBoxMessageType.Info;
            GWtlfNruNzLBcjSZrgKBfRucNYYhKGgJ.text = $"Particles: {(target as SimulationMesh).Particles.Length}\n" +
                       $"Distance Constraints: {(target as SimulationMesh).DistanceConstraints.Length}\n" +
                       $"Volume Constraints: {(target as SimulationMesh).VolumeConstraints.Length}\n" +
                       $"Shape Matching Constraints: {(target as SimulationMesh).ShapeMatchingConstraints.Length}";

            var juXawIHWHuYHahFxwrojTNcvqpMucaGY = new PropertyField();
            juXawIHWHuYHahFxwrojTNcvqpMucaGY.BindProperty(serializedObject.FindProperty("physicsMaterials"));

            ffenlAMiAMxcaopMspJuIfEdPKivCQGu.Add(GWtlfNruNzLBcjSZrgKBfRucNYYhKGgJ);
            ffenlAMiAMxcaopMspJuIfEdPKivCQGu.Add(juXawIHWHuYHahFxwrojTNcvqpMucaGY);

            return ffenlAMiAMxcaopMspJuIfEdPKivCQGu;
        }
    }
}
