namespace MAGES.MeshDeformations.Editor
{
    using UnityEditor;
    using UnityEditor.EditorTools;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    /// <summary>
    /// Custom Inspector for <see cref="RigidbodyActor"/>.
    /// </summary>
    [CustomEditor(typeof(RigidbodyActor))]
    internal class RigidbodyActorEditor : Editor
    {
        private static readonly string RduVAFRbGxUeoQxmVIMemFtbwqYaFjYw = "Packages/com.oramavr.mages.experimental/MeshDeformations/Editor/UI/Actors/RigidbodyActor/RigidbodyActorEditor.uxml";

        private VisualElement grUGFDliJBFGeAKQXlsTfICaTkgzINUe;

        /// <inheritdoc/>
        public override VisualElement CreateInspectorGUI()
        {
            var CxMhRTivyknDDuIOOtVAPtdcuEIUpezT = target as RigidbodyActor;
            var aeHhoDfGwPOlMGFYqeXMYBMczvdhoJsQ = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(RduVAFRbGxUeoQxmVIMemFtbwqYaFjYw);

            if (aeHhoDfGwPOlMGFYqeXMYBMczvdhoJsQ == null)
            {
                Debug.LogError($"Failed to load UXML file at path: {RduVAFRbGxUeoQxmVIMemFtbwqYaFjYw}");
                return base.CreateInspectorGUI();
            }

            grUGFDliJBFGeAKQXlsTfICaTkgzINUe = aeHhoDfGwPOlMGFYqeXMYBMczvdhoJsQ.Instantiate();

            
            grUGFDliJBFGeAKQXlsTfICaTkgzINUe.Q<Button>("edit-particles").clicked += () =>
            {
                if (ToolManager.activeToolType == typeof(BasePhysicsActorEditorTool))
                {
                    vkifbekknHBhUkktYSrsIgdMCgcMffpg();
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
                        ajwiJcCzmQpRKefAXXmxkjgeastrqcYN();
                    }
                }
            };

            
            var lKwxSuWELIsmPKJXudDpDBOfpvoTGKXy = grUGFDliJBFGeAKQXlsTfICaTkgzINUe.Q<ObjectField>("physics-world");
            lKwxSuWELIsmPKJXudDpDBOfpvoTGKXy.objectType = typeof(PhysicsWorld);
            lKwxSuWELIsmPKJXudDpDBOfpvoTGKXy.value = CxMhRTivyknDDuIOOtVAPtdcuEIUpezT.PhysicsWorld;
            lKwxSuWELIsmPKJXudDpDBOfpvoTGKXy.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(CxMhRTivyknDDuIOOtVAPtdcuEIUpezT, "Changed Property in RigidbodyActor");
                CxMhRTivyknDDuIOOtVAPtdcuEIUpezT.PhysicsWorld = evt.newValue as PhysicsWorld;
                EditorUtility.SetDirty(CxMhRTivyknDDuIOOtVAPtdcuEIUpezT);
            });

            var JfpXIlfzIrtmXBSOfhciyjFbMtDiySGe = grUGFDliJBFGeAKQXlsTfICaTkgzINUe.Q<ObjectField>("simulation-mesh");
            JfpXIlfzIrtmXBSOfhciyjFbMtDiySGe.objectType = typeof(SimulationMesh);
            JfpXIlfzIrtmXBSOfhciyjFbMtDiySGe.value = CxMhRTivyknDDuIOOtVAPtdcuEIUpezT.SharedSimulationMesh;
            JfpXIlfzIrtmXBSOfhciyjFbMtDiySGe.RegisterValueChangedCallback((EventCallback<ChangeEvent<Object>>)(evt =>
            {
                Undo.RecordObject(CxMhRTivyknDDuIOOtVAPtdcuEIUpezT, "Changed Property in RigidbodyActor");
                CxMhRTivyknDDuIOOtVAPtdcuEIUpezT.SharedSimulationMesh = evt.newValue as SimulationMesh;
                EditorUtility.SetDirty(CxMhRTivyknDDuIOOtVAPtdcuEIUpezT);
            }));

            var YcFUNrscxfqmExDDaZQBDDMBqCVlEJLS = grUGFDliJBFGeAKQXlsTfICaTkgzINUe.Q<Toggle>("can-cut");
            YcFUNrscxfqmExDDaZQBDDMBqCVlEJLS.value = CxMhRTivyknDDuIOOtVAPtdcuEIUpezT.CanCut;
            YcFUNrscxfqmExDDaZQBDDMBqCVlEJLS.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(CxMhRTivyknDDuIOOtVAPtdcuEIUpezT, "Changed Property in RigidbodyActor");
                CxMhRTivyknDDuIOOtVAPtdcuEIUpezT.CanCut = evt.newValue;
                EditorUtility.SetDirty(CxMhRTivyknDDuIOOtVAPtdcuEIUpezT);
            });

            var tqZNnAhumRAiwKksMBmoNoWejrjhFiEZ = grUGFDliJBFGeAKQXlsTfICaTkgzINUe.Q<UnsignedIntegerField>("layer");
            tqZNnAhumRAiwKksMBmoNoWejrjhFiEZ.value = CxMhRTivyknDDuIOOtVAPtdcuEIUpezT.Layer;
            tqZNnAhumRAiwKksMBmoNoWejrjhFiEZ.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(CxMhRTivyknDDuIOOtVAPtdcuEIUpezT, "Changed Property in RigidbodyActor");
                CxMhRTivyknDDuIOOtVAPtdcuEIUpezT.Layer = evt.newValue;
                EditorUtility.SetDirty(CxMhRTivyknDDuIOOtVAPtdcuEIUpezT);
            });

            var pAlIUWzqFxrJnKopvVCftuuaxTekedCY = grUGFDliJBFGeAKQXlsTfICaTkgzINUe.Q<Toggle>("is-kinematic");
            pAlIUWzqFxrJnKopvVCftuuaxTekedCY.value = CxMhRTivyknDDuIOOtVAPtdcuEIUpezT.IsKinematic;
            pAlIUWzqFxrJnKopvVCftuuaxTekedCY.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(CxMhRTivyknDDuIOOtVAPtdcuEIUpezT, "Changed Property in RigidbodyActor");
                CxMhRTivyknDDuIOOtVAPtdcuEIUpezT.IsKinematic = evt.newValue;
                EditorUtility.SetDirty(CxMhRTivyknDDuIOOtVAPtdcuEIUpezT);
                qMpbJTAgSpEaOtbqDeDBBYZaNhelpynC(evt.newValue);
            });

            var hLEOqdoEHeJbFsxgOyILEidavYksLIIA = grUGFDliJBFGeAKQXlsTfICaTkgzINUe.Q<Toggle>("update-kinematic-particles");
            hLEOqdoEHeJbFsxgOyILEidavYksLIIA.value = CxMhRTivyknDDuIOOtVAPtdcuEIUpezT.UpdateKinematicParticles;
            hLEOqdoEHeJbFsxgOyILEidavYksLIIA.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(CxMhRTivyknDDuIOOtVAPtdcuEIUpezT, "Changed Property in RigidbodyActor");
                CxMhRTivyknDDuIOOtVAPtdcuEIUpezT.UpdateKinematicParticles = evt.newValue;
                EditorUtility.SetDirty(CxMhRTivyknDDuIOOtVAPtdcuEIUpezT);
            });

            qMpbJTAgSpEaOtbqDeDBBYZaNhelpynC(CxMhRTivyknDDuIOOtVAPtdcuEIUpezT.IsKinematic);

            return grUGFDliJBFGeAKQXlsTfICaTkgzINUe;
        }

        private void vkifbekknHBhUkktYSrsIgdMCgcMffpg()
        {
            if (ToolManager.activeToolType == typeof(BasePhysicsActorEditorTool))
            {
                ToolManager.RestorePreviousPersistentTool();
            }
        }

        private void ajwiJcCzmQpRKefAXXmxkjgeastrqcYN()
        {
            if (ToolManager.activeToolType != typeof(BasePhysicsActorEditorTool))
            {
                ToolManager.SetActiveTool<BasePhysicsActorEditorTool>();
            }
        }

        private void qMpbJTAgSpEaOtbqDeDBBYZaNhelpynC(bool enabled)
        {
            var zFdEOjdTBAGMnxHOXIZhKCewxcagEqhP = grUGFDliJBFGeAKQXlsTfICaTkgzINUe.Q<Toggle>("update-kinematic-particles");
            zFdEOjdTBAGMnxHOXIZhKCewxcagEqhP.style.display = enabled ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void rSOAueIclGguNEgntqapECtNLgZidRnF()
        {
        }

        private void KYCpgefjIsYleCRhBoQLsWaBPKgEwhHH()
        {
            vkifbekknHBhUkktYSrsIgdMCgcMffpg();
        }
    }
}
