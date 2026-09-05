namespace MAGES.MeshDeformations.Editor
{
    using System.IO;
    using UnityEditor;
    using UnityEditor.EditorTools;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;
    using UnityEngine.Windows;

    /// <summary>
    /// Custom Inspector for <see cref="DynamicSoftbodyActor"/>.
    /// </summary>
    [CustomEditor(typeof(DynamicSoftbodyActor))]
    internal class DynamicSoftbodyActorEditor : Editor
    {
        private static readonly string eIbVMUZnJjGXPRXntPBQUfqAfRlVSrUL = "Packages/com.oramavr.mages.experimental/MeshDeformations/Editor/UI/Actors/DynamicSoftbodyActor/DynamicSoftbodyActorEditor.uxml";

        /// <inheritdoc/>
        public override VisualElement CreateInspectorGUI()
        {
            var LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT = target as DynamicSoftbodyActor;
            var RJdiJGimWzlZXDiNhZDmZceWhOjIHOaz = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(eIbVMUZnJjGXPRXntPBQUfqAfRlVSrUL);

            if (RJdiJGimWzlZXDiNhZDmZceWhOjIHOaz == null)
            {
                Debug.LogError($"Failed to load UXML file at path: {eIbVMUZnJjGXPRXntPBQUfqAfRlVSrUL}");
                return base.CreateInspectorGUI();
            }

            var jDbjhEbaUOTWVUmSuYKbIXnmkTqrSYll = RJdiJGimWzlZXDiNhZDmZceWhOjIHOaz.Instantiate();

            
            jDbjhEbaUOTWVUmSuYKbIXnmkTqrSYll.Q<Button>("edit-particles").clicked += () =>
            {
                if (ToolManager.activeToolType == typeof(BasePhysicsActorEditorTool))
                {
                    kWiXhZCEpMOyDKzHCjDkGAPqKsaTLUeQ();
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
                        REzPeQCZdUiQwVdvffaxeAwuIrBDGsHZ();
                    }
                }
            };

            
            var AUptmjLjfNSLJKtrcemzRwMXlCxhJdLb = jDbjhEbaUOTWVUmSuYKbIXnmkTqrSYll.Q<ObjectField>("physics-world");
            AUptmjLjfNSLJKtrcemzRwMXlCxhJdLb.objectType = typeof(PhysicsWorld);
            AUptmjLjfNSLJKtrcemzRwMXlCxhJdLb.value = LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.PhysicsWorld;
            AUptmjLjfNSLJKtrcemzRwMXlCxhJdLb.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT, "Changed Property in DynamicSoftbodyActor");
                LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.PhysicsWorld = evt.newValue as PhysicsWorld;
                EditorUtility.SetDirty(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT);
            });

            var VyIeddJXXOArsGWWZMlukdSCDIMxoAVv = jDbjhEbaUOTWVUmSuYKbIXnmkTqrSYll.Q<ObjectField>("simulation-mesh");
            VyIeddJXXOArsGWWZMlukdSCDIMxoAVv.objectType = typeof(SimulationMesh);
            VyIeddJXXOArsGWWZMlukdSCDIMxoAVv.value = LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.SharedSimulationMesh;
            VyIeddJXXOArsGWWZMlukdSCDIMxoAVv.RegisterValueChangedCallback((EventCallback<ChangeEvent<Object>>)(evt =>
            {
                Undo.RecordObject(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT, "Changed Property in DynamicSoftbodyActor");
                LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.SharedSimulationMesh = evt.newValue as SimulationMesh;
                EditorUtility.SetDirty(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT);
            }));

            var tYmvrNrbkYSYdcmOWVXlcysBpvktNxhv = jDbjhEbaUOTWVUmSuYKbIXnmkTqrSYll.Q<UnsignedIntegerField>("layer");
            tYmvrNrbkYSYdcmOWVXlcysBpvktNxhv.value = LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.Layer;
            tYmvrNrbkYSYdcmOWVXlcysBpvktNxhv.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT, "Changed Property in DynamicSoftbodyActor");
                LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.Layer = evt.newValue;
                EditorUtility.SetDirty(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT);
            });

            var FGelfSypsuPvwVAuSthVLmHFJssDHrUN = jDbjhEbaUOTWVUmSuYKbIXnmkTqrSYll.Q<Toggle>("update-kinematic-particles");
            FGelfSypsuPvwVAuSthVLmHFJssDHrUN.value = LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.UpdateKinematicParticles;
            FGelfSypsuPvwVAuSthVLmHFJssDHrUN.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT, "Changed Property in DynamicSoftbodyActor");
                LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.UpdateKinematicParticles = evt.newValue;
                EditorUtility.SetDirty(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT);
            });

            var YBleBtRZskvSBmxuRsqIOikJxrrsjOWE = jDbjhEbaUOTWVUmSuYKbIXnmkTqrSYll.Q<Toggle>("remove-residual-distance-constraints");
            YBleBtRZskvSBmxuRsqIOikJxrrsjOWE.value = LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.RemoveResidualDistanceConstraints;
            YBleBtRZskvSBmxuRsqIOikJxrrsjOWE.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT, "Changed Property in DynamicSoftbodyActor");
                LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.RemoveResidualDistanceConstraints = evt.newValue;
                EditorUtility.SetDirty(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT);
            });

            var xWgZRRqpKLnAgkOklXketyJvSqcqokbD = jDbjhEbaUOTWVUmSuYKbIXnmkTqrSYll.Q<Toggle>("remove-volume-constraints-with-limited-neighbors");
            xWgZRRqpKLnAgkOklXketyJvSqcqokbD.value = LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.RemoveVolumeConstraintsWithLimitedNeighbors;
            xWgZRRqpKLnAgkOklXketyJvSqcqokbD.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT, "Changed Property in DynamicSoftbodyActor");
                LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.RemoveVolumeConstraintsWithLimitedNeighbors = evt.newValue;
                EditorUtility.SetDirty(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT);
            });

            var dNQCtfqGVttKHmEDgytXVsOBXwdjpmVT = jDbjhEbaUOTWVUmSuYKbIXnmkTqrSYll.Q<SliderInt>("min-number-of-neihbors-before-removal");
            dNQCtfqGVttKHmEDgytXVsOBXwdjpmVT.value = LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.MinNeighborsBeforeVolumeConstraintsRemoval;
            dNQCtfqGVttKHmEDgytXVsOBXwdjpmVT.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT, "Changed Property in DynamicSoftbodyActor");
                LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.MinNeighborsBeforeVolumeConstraintsRemoval = evt.newValue;
                EditorUtility.SetDirty(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT);
            });

            
            var OeiqDQsUCbdMigbnjuKbSurbpybZtsqP = jDbjhEbaUOTWVUmSuYKbIXnmkTqrSYll.Q<Toggle>("smooth-shading");
            OeiqDQsUCbdMigbnjuKbSurbpybZtsqP.value = LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.SmoothShading;
            OeiqDQsUCbdMigbnjuKbSurbpybZtsqP.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT, "Changed Property in DynamicSoftbodyActor");
                LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.SmoothShading = evt.newValue;
                EditorUtility.SetDirty(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT);
            });

            var lOlxfBKQMZeesCCeqWySvpdJoNHoVkUz = jDbjhEbaUOTWVUmSuYKbIXnmkTqrSYll.Q<Toggle>("generate-faces-in-physics-material-boundaries");
            lOlxfBKQMZeesCCeqWySvpdJoNHoVkUz.value = LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.GenerateFacesInPhysicsMaterialBoundaries;
            lOlxfBKQMZeesCCeqWySvpdJoNHoVkUz.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT, "Changed Property in DynamicSoftbodyActor");
                LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT.GenerateFacesInPhysicsMaterialBoundaries = evt.newValue;
                EditorUtility.SetDirty(LrnLgEQGDaLWkXJEdqMKkMBXfIwTgPPT);
            });

            jDbjhEbaUOTWVUmSuYKbIXnmkTqrSYll.Q<Button>("import-uv-map").clicked += zOXsEfZPYcpcptykLZflCRiWiHWmLAPB;

            return jDbjhEbaUOTWVUmSuYKbIXnmkTqrSYll;
        }

        private void kWiXhZCEpMOyDKzHCjDkGAPqKsaTLUeQ()
        {
            if (ToolManager.activeToolType == typeof(BasePhysicsActorEditorTool))
            {
                ToolManager.RestorePreviousPersistentTool();
            }
        }

        private void REzPeQCZdUiQwVdvffaxeAwuIrBDGsHZ()
        {
            if (ToolManager.activeToolType != typeof(BasePhysicsActorEditorTool))
            {
                ToolManager.SetActiveTool<BasePhysicsActorEditorTool>();
            }
        }

        private void zOXsEfZPYcpcptykLZflCRiWiHWmLAPB()
        {
            string xcGAUIAUBcFsvrNXFLLxpkFwMHgvHHaO = EditorUtility.OpenFilePanel("Import UV Maps", string.Empty, "fbx");

            if (string.IsNullOrEmpty(xcGAUIAUBcFsvrNXFLLxpkFwMHgvHHaO))
            {
                EditorUtility.DisplayDialog("Error", "No file selected. UV maps not imported.", "Ok");
                return;
            }

            var RXQCdmRGITKUnSZktyOFKjbpxIFbEOPm = target as DynamicSoftbodyActor;

            xcGAUIAUBcFsvrNXFLLxpkFwMHgvHHaO = xcGAUIAUBcFsvrNXFLLxpkFwMHgvHHaO.Substring(xcGAUIAUBcFsvrNXFLLxpkFwMHgvHHaO.IndexOf("Assets"));
            Debug.Log(xcGAUIAUBcFsvrNXFLLxpkFwMHgvHHaO);

            Mesh oMDTRCBRzjNXQjfQhgjRanIXUiWafrSz = AssetDatabase.LoadAssetAtPath<Mesh>(xcGAUIAUBcFsvrNXFLLxpkFwMHgvHHaO);
            Debug.Log(oMDTRCBRzjNXQjfQhgjRanIXUiWafrSz.vertexCount);

            RXQCdmRGITKUnSZktyOFKjbpxIFbEOPm.GenerateUVMapsFromMesh(oMDTRCBRzjNXQjfQhgjRanIXUiWafrSz);
            EditorUtility.SetDirty(RXQCdmRGITKUnSZktyOFKjbpxIFbEOPm);
        }

        private void irTieifIsSXFjhsuaFFnIzBrppmUHKOu()
        {
        }

        private void DCjmSlPOtUvHzSBtgxMaTQDFJrxBcmJp()
        {
            kWiXhZCEpMOyDKzHCjDkGAPqKsaTLUeQ();
        }
    }
}
