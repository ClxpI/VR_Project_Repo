namespace MAGES.MeshDeformations.Editor
{
    using UnityEditor;
    using UnityEditor.EditorTools;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    /// <summary>
    /// Custom Inspector for <see cref="ClothActor"/>.
    /// </summary>
    [CustomEditor(typeof(ClothActor))]
    internal class ClothActorEditor : Editor
    {
        private static readonly string UNgGTxqootEOmjNixwYtafUWjLkoOrUH = "Packages/com.oramavr.mages.experimental/MeshDeformations/Editor/UI/Actors/ClothActor/ClothActorEditor.uxml";

        /// <inheritdoc/>
        public override VisualElement CreateInspectorGUI()
        {
            var UTUplkgRNVMfLVkfuDGSehLruokXTcEU = target as ClothActor;
            var vAxoWldJiBdKWNOYEkMLQsbbnbtUXKxd = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UNgGTxqootEOmjNixwYtafUWjLkoOrUH);

            if (vAxoWldJiBdKWNOYEkMLQsbbnbtUXKxd == null)
            {
                Debug.LogError($"Failed to load UXML file at path: {UNgGTxqootEOmjNixwYtafUWjLkoOrUH}");
                return base.CreateInspectorGUI();
            }

            var IqxDNKxkEcOZFpPyUhjyGcEHlnnJcUVg = vAxoWldJiBdKWNOYEkMLQsbbnbtUXKxd.Instantiate();

            
            IqxDNKxkEcOZFpPyUhjyGcEHlnnJcUVg.Q<Button>("edit-particles").clicked += () =>
            {
                if (ToolManager.activeToolType == typeof(BasePhysicsActorEditorTool))
                {
                    vMSjNANeIdmjtTaLOsHiPZUgpuSbNhOA();
                }
                else
                {
                    if (Application.isPlaying)
                    {
                        SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Particle Editing tool unavailable in play mode"), .5f);
                        return;
                    }
                    else
                    {
                        FPEFjwETTXfEmtjgcLxHFoQWwXcAaVkV();
                    }
                }
            };

            
            var sAnZuKLKfFMQdHcuOyjogwNGBzYDUvmk = IqxDNKxkEcOZFpPyUhjyGcEHlnnJcUVg.Q<ObjectField>("physics-world");
            sAnZuKLKfFMQdHcuOyjogwNGBzYDUvmk.objectType = typeof(PhysicsWorld);
            sAnZuKLKfFMQdHcuOyjogwNGBzYDUvmk.value = UTUplkgRNVMfLVkfuDGSehLruokXTcEU.PhysicsWorld;
            sAnZuKLKfFMQdHcuOyjogwNGBzYDUvmk.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(UTUplkgRNVMfLVkfuDGSehLruokXTcEU, "Changed Property in ClothActor");
                UTUplkgRNVMfLVkfuDGSehLruokXTcEU.PhysicsWorld = evt.newValue as PhysicsWorld;
                EditorUtility.SetDirty(UTUplkgRNVMfLVkfuDGSehLruokXTcEU);
            });

            var iYIFJaPdQJuOdjmCeKxbmIFFnnLNwiWT = IqxDNKxkEcOZFpPyUhjyGcEHlnnJcUVg.Q<ObjectField>("source-mesh");
            iYIFJaPdQJuOdjmCeKxbmIFFnnLNwiWT.objectType = typeof(Mesh);
            iYIFJaPdQJuOdjmCeKxbmIFFnnLNwiWT.value = UTUplkgRNVMfLVkfuDGSehLruokXTcEU.SourceMesh;
            iYIFJaPdQJuOdjmCeKxbmIFFnnLNwiWT.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(UTUplkgRNVMfLVkfuDGSehLruokXTcEU, "Changed Property in ClothActor");
                UTUplkgRNVMfLVkfuDGSehLruokXTcEU.SourceMesh = evt.newValue as Mesh;
                EditorUtility.SetDirty(UTUplkgRNVMfLVkfuDGSehLruokXTcEU);
            });

            var cbnJCqZkTkbXTqfCOBVPzTDQSLBumSAt = IqxDNKxkEcOZFpPyUhjyGcEHlnnJcUVg.Q<FloatField>("mass");
            cbnJCqZkTkbXTqfCOBVPzTDQSLBumSAt.value = UTUplkgRNVMfLVkfuDGSehLruokXTcEU.Mass;
            cbnJCqZkTkbXTqfCOBVPzTDQSLBumSAt.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(UTUplkgRNVMfLVkfuDGSehLruokXTcEU, "Changed Property in ClothActor");
                UTUplkgRNVMfLVkfuDGSehLruokXTcEU.Mass = evt.newValue;
                EditorUtility.SetDirty(UTUplkgRNVMfLVkfuDGSehLruokXTcEU);
            });

            var cWynrVdfCJjsctqPCWBoIYCrIkpgtpYi = IqxDNKxkEcOZFpPyUhjyGcEHlnnJcUVg.Q<FloatField>("damping");
            cWynrVdfCJjsctqPCWBoIYCrIkpgtpYi.value = UTUplkgRNVMfLVkfuDGSehLruokXTcEU.Damping;
            cWynrVdfCJjsctqPCWBoIYCrIkpgtpYi.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(UTUplkgRNVMfLVkfuDGSehLruokXTcEU, "Changed Property in ClothActor");
                UTUplkgRNVMfLVkfuDGSehLruokXTcEU.Damping = evt.newValue;
                EditorUtility.SetDirty(UTUplkgRNVMfLVkfuDGSehLruokXTcEU);
            });

            var BhnApdTHHVVSOGcPcgbPiBUxdTmVVSPb = IqxDNKxkEcOZFpPyUhjyGcEHlnnJcUVg.Q<Toggle>("self-collision");
            BhnApdTHHVVSOGcPcgbPiBUxdTmVVSPb.value = UTUplkgRNVMfLVkfuDGSehLruokXTcEU.SelfCollision;
            BhnApdTHHVVSOGcPcgbPiBUxdTmVVSPb.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(UTUplkgRNVMfLVkfuDGSehLruokXTcEU, "Changed Property in ClothActor");
                UTUplkgRNVMfLVkfuDGSehLruokXTcEU.SelfCollision = evt.newValue;
                EditorUtility.SetDirty(UTUplkgRNVMfLVkfuDGSehLruokXTcEU);
            });

            var NgVgGiuTDueKGrqmZqhYcFesPqGVfyCV = IqxDNKxkEcOZFpPyUhjyGcEHlnnJcUVg.Q<UnsignedIntegerField>("layer");
            NgVgGiuTDueKGrqmZqhYcFesPqGVfyCV.value = UTUplkgRNVMfLVkfuDGSehLruokXTcEU.Layer;
            NgVgGiuTDueKGrqmZqhYcFesPqGVfyCV.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(UTUplkgRNVMfLVkfuDGSehLruokXTcEU, "Changed Property in ClothActor");
                UTUplkgRNVMfLVkfuDGSehLruokXTcEU.Layer = evt.newValue;
                EditorUtility.SetDirty(UTUplkgRNVMfLVkfuDGSehLruokXTcEU);
            });

            var hUURBWMTgREPSKQUVcTlryJITYqLzeKG = IqxDNKxkEcOZFpPyUhjyGcEHlnnJcUVg.Q<Toggle>("update-kinematic-particles");
            hUURBWMTgREPSKQUVcTlryJITYqLzeKG.value = UTUplkgRNVMfLVkfuDGSehLruokXTcEU.UpdateKinematicParticles;
            hUURBWMTgREPSKQUVcTlryJITYqLzeKG.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(UTUplkgRNVMfLVkfuDGSehLruokXTcEU, "Changed Property in ClothActor");
                UTUplkgRNVMfLVkfuDGSehLruokXTcEU.UpdateKinematicParticles = evt.newValue;
                EditorUtility.SetDirty(UTUplkgRNVMfLVkfuDGSehLruokXTcEU);
            });

            return IqxDNKxkEcOZFpPyUhjyGcEHlnnJcUVg;
        }

        private void vMSjNANeIdmjtTaLOsHiPZUgpuSbNhOA()
        {
            if (ToolManager.activeToolType == typeof(BasePhysicsActorEditorTool))
            {
                ToolManager.RestorePreviousPersistentTool();
            }
        }

        private void FPEFjwETTXfEmtjgcLxHFoQWwXcAaVkV()
        {
            if (ToolManager.activeToolType != typeof(BasePhysicsActorEditorTool))
            {
                ToolManager.SetActiveTool<BasePhysicsActorEditorTool>();
            }
        }

        private void hQBWAdrqmaZruRReFWElFbmGjQbQewlF()
        {
            vMSjNANeIdmjtTaLOsHiPZUgpuSbNhOA();
        }
    }
}
