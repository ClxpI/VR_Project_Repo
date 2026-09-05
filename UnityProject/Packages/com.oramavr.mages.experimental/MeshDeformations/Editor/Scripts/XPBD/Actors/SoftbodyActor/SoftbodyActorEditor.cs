namespace MAGES.MeshDeformations.Editor
{
    using UnityEditor;
    using UnityEditor.EditorTools;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    /// <summary>
    /// Custom Inspector for <see cref="SoftbodyActor"/>.
    /// </summary>
    [CustomEditor(typeof(SoftbodyActor))]
    internal class SoftbodyActorEditor : Editor
    {
        private static readonly string mUnmYWavAVBAdcCYNuJjEkCZhdtIRXuZ = "Packages/com.oramavr.mages.experimental/MeshDeformations/Editor/UI/Actors/SoftbodyActor/SoftbodyActorEditor.uxml";

        /// <inheritdoc/>
        public override VisualElement CreateInspectorGUI()
        {
            var XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY = target as SoftbodyActor;
            var JJnkXOapLzZvbpfLIrIYFlmJiAXdICri = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(mUnmYWavAVBAdcCYNuJjEkCZhdtIRXuZ);

            if (JJnkXOapLzZvbpfLIrIYFlmJiAXdICri == null)
            {
                Debug.LogError($"Failed to load UXML file at path: {mUnmYWavAVBAdcCYNuJjEkCZhdtIRXuZ}");
                return base.CreateInspectorGUI();
            }

            var coVcJHGMqUdOithrSCNshAsFtkrUjSAX = JJnkXOapLzZvbpfLIrIYFlmJiAXdICri.Instantiate();

            
            coVcJHGMqUdOithrSCNshAsFtkrUjSAX.Q<Button>("edit-particles").clicked += () =>
            {
                if (ToolManager.activeToolType == typeof(BasePhysicsActorEditorTool))
                {
                    DgpiohpNYNaqZBTlaPcOELdkDVuDQLPP();
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
                        NnxTxMSDDlVPuVzwfbxjRxGQlgHBgvab();
                    }
                }
            };

            
            var aepBPGOUBjDWhCTBcPvrVLxkclCDWgYB = coVcJHGMqUdOithrSCNshAsFtkrUjSAX.Q<ObjectField>("physics-world");
            aepBPGOUBjDWhCTBcPvrVLxkclCDWgYB.objectType = typeof(PhysicsWorld);
            aepBPGOUBjDWhCTBcPvrVLxkclCDWgYB.value = XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY.PhysicsWorld;
            aepBPGOUBjDWhCTBcPvrVLxkclCDWgYB.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY, "Changed Property in SoftbodyActor");
                XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY.PhysicsWorld = evt.newValue as PhysicsWorld;
                EditorUtility.SetDirty(XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY);
            });

            var nvOYbsrYVbufoFZxQhxrkEkFYJFbjoIt = coVcJHGMqUdOithrSCNshAsFtkrUjSAX.Q<ObjectField>("simulation-mesh");
            nvOYbsrYVbufoFZxQhxrkEkFYJFbjoIt.objectType = typeof(SimulationMesh);
            nvOYbsrYVbufoFZxQhxrkEkFYJFbjoIt.value = XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY.SharedSimulationMesh;
            nvOYbsrYVbufoFZxQhxrkEkFYJFbjoIt.RegisterValueChangedCallback((EventCallback<ChangeEvent<Object>>)(evt =>
            {
                Undo.RecordObject(XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY, "Changed Property in SoftbodyActor");
                XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY.SharedSimulationMesh = evt.newValue as SimulationMesh;
                EditorUtility.SetDirty(XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY);
            }));

            var oXmPcwnKoZNMJjFzSGMmfLQuKIwNYiic = coVcJHGMqUdOithrSCNshAsFtkrUjSAX.Q<UnsignedIntegerField>("layer");
            oXmPcwnKoZNMJjFzSGMmfLQuKIwNYiic.value = XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY.Layer;
            oXmPcwnKoZNMJjFzSGMmfLQuKIwNYiic.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY, "Changed Property in SoftbodyActor");
                XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY.Layer = evt.newValue;
                EditorUtility.SetDirty(XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY);
            });

            var RlmlLcmKtuuvpumSozxWdSdXTCYsosdB = coVcJHGMqUdOithrSCNshAsFtkrUjSAX.Q<Toggle>("update-kinematic-particles");
            RlmlLcmKtuuvpumSozxWdSdXTCYsosdB.value = XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY.UpdateKinematicParticles;
            RlmlLcmKtuuvpumSozxWdSdXTCYsosdB.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY, "Changed Property in SoftbodyActor");
                XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY.UpdateKinematicParticles = evt.newValue;
                EditorUtility.SetDirty(XBRcqUJudZuHEpYTmJTgoFlqPpWrgvHY);
            });

            return coVcJHGMqUdOithrSCNshAsFtkrUjSAX;
        }

        private void DgpiohpNYNaqZBTlaPcOELdkDVuDQLPP()
        {
            if (ToolManager.activeToolType == typeof(BasePhysicsActorEditorTool))
            {
                ToolManager.RestorePreviousPersistentTool();
            }
        }

        private void NnxTxMSDDlVPuVzwfbxjRxGQlgHBgvab()
        {
            if (ToolManager.activeToolType != typeof(BasePhysicsActorEditorTool))
            {
                ToolManager.SetActiveTool<BasePhysicsActorEditorTool>();
            }
        }

        private void IGCfhgrkjxjyAmQzYlKMFCBzvNfFMLsX()
        {
            DgpiohpNYNaqZBTlaPcOELdkDVuDQLPP();
        }
    }
}
