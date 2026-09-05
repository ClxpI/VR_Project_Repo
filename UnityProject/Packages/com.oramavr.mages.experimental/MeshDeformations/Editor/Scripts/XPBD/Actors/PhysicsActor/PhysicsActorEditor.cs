namespace MAGES.MeshDeformations.Editor
{
    using UnityEditor;
    using UnityEditor.EditorTools;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    /// <summary>
    /// Custom Inspector for <see cref="PhysicsActor"/>.
    /// </summary>
    [CustomEditor(typeof(PhysicsActor))]
    internal class PhysicsActorEditor : Editor
    {
        private static readonly string yxugCRHrZYFleMrlvVveMvogRyZnRKfd = "Packages/com.oramavr.mages.experimental/MeshDeformations/Editor/UI/Actors/PhysicsActor/PhysicsActorEditor.uxml";

        /// <inheritdoc/>
        public override VisualElement CreateInspectorGUI()
        {
            var uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs = target as PhysicsActor;
            var itmdbrPJdbdGbxeUHbWUYdEnwJedrlWF = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(yxugCRHrZYFleMrlvVveMvogRyZnRKfd);

            if (itmdbrPJdbdGbxeUHbWUYdEnwJedrlWF == null)
            {
                Debug.LogError($"Failed to load UXML file at path: {yxugCRHrZYFleMrlvVveMvogRyZnRKfd}");
                return base.CreateInspectorGUI();
            }

            var JOoPUpWqTylpkgUOiwcmEIDiQQkOYrGD = itmdbrPJdbdGbxeUHbWUYdEnwJedrlWF.Instantiate();

            
            JOoPUpWqTylpkgUOiwcmEIDiQQkOYrGD.Q<Button>("edit-particles").clicked += () =>
            {
                if (ToolManager.activeToolType == typeof(BasePhysicsActorEditorTool))
                {
                    QfQOApRmqvQQgYaMferTQxuEsHhJgZtv();
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
                        PBSSrNLZSDByFhmsKZTsVaPDVFuHYGyp();
                    }
                }
            };

            
            var xBjKeVSatemHuMSsXihFzeYKNHfsUFmD = JOoPUpWqTylpkgUOiwcmEIDiQQkOYrGD.Q<ObjectField>("physics-world");
            xBjKeVSatemHuMSsXihFzeYKNHfsUFmD.objectType = typeof(PhysicsWorld);
            xBjKeVSatemHuMSsXihFzeYKNHfsUFmD.value = uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs.PhysicsWorld;
            xBjKeVSatemHuMSsXihFzeYKNHfsUFmD.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs, "Changed Property in PhysicsActor");
                uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs.PhysicsWorld = evt.newValue as PhysicsWorld;
                EditorUtility.SetDirty(uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs);
            });

            var lOtdDQyVbVacFavJUtYtatutCIBsvtEy = JOoPUpWqTylpkgUOiwcmEIDiQQkOYrGD.Q<ObjectField>("simulation-mesh");
            lOtdDQyVbVacFavJUtYtatutCIBsvtEy.objectType = typeof(SimulationMesh);
            lOtdDQyVbVacFavJUtYtatutCIBsvtEy.value = uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs.SharedSimulationMesh;
            lOtdDQyVbVacFavJUtYtatutCIBsvtEy.RegisterValueChangedCallback((EventCallback<ChangeEvent<Object>>)(evt =>
            {
                Undo.RecordObject(uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs, "Changed Property in PhysicsActor");
                uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs.SharedSimulationMesh = evt.newValue as SimulationMesh;
                EditorUtility.SetDirty(uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs);
            }));

            var FgZiLGzxcaKcccxrApDttuPXLWQBKIyj = JOoPUpWqTylpkgUOiwcmEIDiQQkOYrGD.Q<Toggle>("can-cut");
            FgZiLGzxcaKcccxrApDttuPXLWQBKIyj.value = uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs.CanCut;
            FgZiLGzxcaKcccxrApDttuPXLWQBKIyj.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs, "Changed Property in PhysicsActor");
                uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs.CanCut = evt.newValue;
                EditorUtility.SetDirty(uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs);
            });

            var WDzkUKFJjvcuYpfCkFahpgzkNtKGwSYa = JOoPUpWqTylpkgUOiwcmEIDiQQkOYrGD.Q<Toggle>("self-collision");
            WDzkUKFJjvcuYpfCkFahpgzkNtKGwSYa.value = uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs.SelfCollision;
            WDzkUKFJjvcuYpfCkFahpgzkNtKGwSYa.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs, "Changed Property in PhysicsActor");
                uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs.SelfCollision = evt.newValue;
                EditorUtility.SetDirty(uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs);
            });

            var tSLMimNuGlxwXMAdmErmReoyWOdJlHAN = JOoPUpWqTylpkgUOiwcmEIDiQQkOYrGD.Q<UnsignedIntegerField>("layer");
            tSLMimNuGlxwXMAdmErmReoyWOdJlHAN.value = uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs.Layer;
            tSLMimNuGlxwXMAdmErmReoyWOdJlHAN.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs, "Changed Property in PhysicsActor");
                uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs.Layer = evt.newValue;
                EditorUtility.SetDirty(uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs);
            });

            var zemHLvNoigXKYmumHjDsCAgdpPGRvdpX = JOoPUpWqTylpkgUOiwcmEIDiQQkOYrGD.Q<Toggle>("update-kinematic-particles");
            zemHLvNoigXKYmumHjDsCAgdpPGRvdpX.value = uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs.UpdateKinematicParticles;
            zemHLvNoigXKYmumHjDsCAgdpPGRvdpX.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs, "Changed Property in PhysicsActor");
                uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs.UpdateKinematicParticles = evt.newValue;
                EditorUtility.SetDirty(uBcuSeJRIttNNDVoOpoeHYRVYWnQWAPs);
            });

            return JOoPUpWqTylpkgUOiwcmEIDiQQkOYrGD;
        }

        private void QfQOApRmqvQQgYaMferTQxuEsHhJgZtv()
        {
            if (ToolManager.activeToolType == typeof(BasePhysicsActorEditorTool))
            {
                ToolManager.RestorePreviousPersistentTool();
            }
        }

        private void PBSSrNLZSDByFhmsKZTsVaPDVFuHYGyp()
        {
            if (ToolManager.activeToolType != typeof(BasePhysicsActorEditorTool))
            {
                ToolManager.SetActiveTool<BasePhysicsActorEditorTool>();
            }
        }

        private void ZVqPYvjoCsNOcSsYHndgGKEeWQymZAhb()
        {
        }

        private void QLdsTTStqePaqAdgakuLRBkfVgEwEUJK()
        {
            QfQOApRmqvQQgYaMferTQxuEsHhJgZtv();
        }
    }
}
