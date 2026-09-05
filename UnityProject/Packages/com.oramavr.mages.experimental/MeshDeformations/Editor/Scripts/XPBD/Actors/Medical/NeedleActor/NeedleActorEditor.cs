namespace MAGES.MeshDeformations.Editor
{
    using UnityEditor;
    using UnityEditor.EditorTools;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    /// <summary>
    /// Custom Inspector for <see cref="NeedleActor"/>.
    /// </summary>
    [CustomEditor(typeof(NeedleActor))]
    internal class NeedleActorEditor : Editor
    {
        private static readonly string rvnneNhxyUMjRntiIZcxVnFsIigBmojO = "Packages/com.oramavr.mages.experimental/MeshDeformations/Editor/UI/Actors/Medical/NeedleActor/NeedleActorEditor.uxml";

        private VisualElement cdXJVRzNXowVswTWdYgxUWThIdkWYaOW;

        /// <inheritdoc/>
        public override VisualElement CreateInspectorGUI()
        {
            var SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY = target as NeedleActor;
            var DWNxkaXTZHHtgkYyTzEBXzpKkrHsOEoN = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(rvnneNhxyUMjRntiIZcxVnFsIigBmojO);

            if (DWNxkaXTZHHtgkYyTzEBXzpKkrHsOEoN == null)
            {
                Debug.LogError($"Failed to load UXML file at path: {rvnneNhxyUMjRntiIZcxVnFsIigBmojO}");
                return base.CreateInspectorGUI();
            }

            cdXJVRzNXowVswTWdYgxUWThIdkWYaOW = DWNxkaXTZHHtgkYyTzEBXzpKkrHsOEoN.Instantiate();

            
            cdXJVRzNXowVswTWdYgxUWThIdkWYaOW.Q<Button>("edit-particles").clicked += () =>
            {
                if (ToolManager.activeToolType == typeof(BasePhysicsActorEditorTool))
                {
                    qGTnOOEnZlpXNDcQPWxlDbJkuZOaFxkR();
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
                        BEulTAuQfslgUNJeYJTXUQqfOynfWRuR();
                    }
                }
            };

            
            var reMXeaGhyLfqnGkWEnXXKOSzsORGypAM = cdXJVRzNXowVswTWdYgxUWThIdkWYaOW.Q<ObjectField>("physics-world");
            reMXeaGhyLfqnGkWEnXXKOSzsORGypAM.objectType = typeof(PhysicsWorld);
            reMXeaGhyLfqnGkWEnXXKOSzsORGypAM.value = SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.PhysicsWorld;
            reMXeaGhyLfqnGkWEnXXKOSzsORGypAM.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY, "Changed Property in RigidbodyActor");
                SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.PhysicsWorld = evt.newValue as PhysicsWorld;
                EditorUtility.SetDirty(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY);
            });

            var AmiUOllTOFserNdLWkoXEXzPplhTHoqM = cdXJVRzNXowVswTWdYgxUWThIdkWYaOW.Q<ListView>("needle-points");
            AmiUOllTOFserNdLWkoXEXzPplhTHoqM.showAddRemoveFooter = true;
            AmiUOllTOFserNdLWkoXEXzPplhTHoqM.itemsSource = SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.NeedlePoints;
            AmiUOllTOFserNdLWkoXEXzPplhTHoqM.makeItem = () =>
            {
                var hvOnnyjXAvFbwbacRIBvNvdmGGqLRdZx = new ObjectField();
                hvOnnyjXAvFbwbacRIBvNvdmGGqLRdZx.objectType = typeof(Transform);
                hvOnnyjXAvFbwbacRIBvNvdmGGqLRdZx.style.marginRight = 2;
                hvOnnyjXAvFbwbacRIBvNvdmGGqLRdZx.style.marginLeft = 2;

                return hvOnnyjXAvFbwbacRIBvNvdmGGqLRdZx;
            };
            AmiUOllTOFserNdLWkoXEXzPplhTHoqM.bindItem = (ve, i) =>
            {
                var ntMGPMoBvaSYrgBgohXLpzBZAqnDDcGX = (ObjectField)ve;
                ntMGPMoBvaSYrgBgohXLpzBZAqnDDcGX.SetValueWithoutNotify(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.NeedlePoints[i]);
                ntMGPMoBvaSYrgBgohXLpzBZAqnDDcGX.label = $"Element {i}";
                ve.RegisterCallback<ChangeEvent<Object>>(evt =>
                {
                    Undo.RecordObject(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY, "Changed Property in RigidbodyActor");
                    var ZJtgheswphaZKKWIoIBVBTofKdjuJMJx = SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.NeedlePoints;
                    ZJtgheswphaZKKWIoIBVBTofKdjuJMJx[i] = evt.newValue as Transform;
                    SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.NeedlePoints = ZJtgheswphaZKKWIoIBVBTofKdjuJMJx;
                    EditorUtility.SetDirty(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY);
                });
            };
            AmiUOllTOFserNdLWkoXEXzPplhTHoqM.itemsRemoved += (i) =>
            {
                EditorApplication.delayCall += () =>
                {
                    Undo.RecordObject(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY, "Changed Property in RigidbodyActor");
                    SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.RebuildSimulationMesh();
                    EditorUtility.SetDirty(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY);
                };
            };
            AmiUOllTOFserNdLWkoXEXzPplhTHoqM.itemsAdded += (i) =>
            {
                Undo.RecordObject(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY, "Changed Property in RigidbodyActor");
                SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.RebuildSimulationMesh();
                EditorUtility.SetDirty(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY);
            };

            var DWRZMwWrWUNjVQpAzCPpYhtBLQEQbnFt = cdXJVRzNXowVswTWdYgxUWThIdkWYaOW.Q<FloatField>("mass");
            DWRZMwWrWUNjVQpAzCPpYhtBLQEQbnFt.value = SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.Mass;
            DWRZMwWrWUNjVQpAzCPpYhtBLQEQbnFt.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY, "Changed Property in ClothActor");
                SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.Mass = evt.newValue;
                EditorUtility.SetDirty(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY);
            });

            var ymSDvRaIPRxHRjkqavpzCqGuTupHuIjs = cdXJVRzNXowVswTWdYgxUWThIdkWYaOW.Q<FloatField>("damping");
            ymSDvRaIPRxHRjkqavpzCqGuTupHuIjs.value = SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.Damping;
            ymSDvRaIPRxHRjkqavpzCqGuTupHuIjs.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY, "Changed Property in ClothActor");
                SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.Damping = evt.newValue;
                EditorUtility.SetDirty(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY);
            });

            var hPJaNullsQRBvrIKAHukgRTdDxcNIckO = cdXJVRzNXowVswTWdYgxUWThIdkWYaOW.Q<Toggle>("can-cut");
            hPJaNullsQRBvrIKAHukgRTdDxcNIckO.value = SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.CanCut;
            hPJaNullsQRBvrIKAHukgRTdDxcNIckO.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY, "Changed Property in RigidbodyActor");
                SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.CanCut = evt.newValue;
                EditorUtility.SetDirty(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY);
            });

            var EyevmjfEgxVVJCPlRoVtdDJZZJRPVuQK = cdXJVRzNXowVswTWdYgxUWThIdkWYaOW.Q<UnsignedIntegerField>("layer");
            EyevmjfEgxVVJCPlRoVtdDJZZJRPVuQK.value = SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.Layer;
            EyevmjfEgxVVJCPlRoVtdDJZZJRPVuQK.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY, "Changed Property in RigidbodyActor");
                SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.Layer = evt.newValue;
                EditorUtility.SetDirty(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY);
            });

            var DdFHGYBzpkGhjLmfKTHZDKKeUPgfAoAz = cdXJVRzNXowVswTWdYgxUWThIdkWYaOW.Q<Toggle>("is-kinematic");
            DdFHGYBzpkGhjLmfKTHZDKKeUPgfAoAz.value = SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.IsKinematic;
            DdFHGYBzpkGhjLmfKTHZDKKeUPgfAoAz.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY, "Changed Property in RigidbodyActor");
                SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.IsKinematic = evt.newValue;
                EditorUtility.SetDirty(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY);
                spECJIApmYhjYDWTaKOGyTAkUZNgqPkC(evt.newValue);
            });

            var otbsCxLMktvYPiQNzrDSMDuVgAptOOKD = cdXJVRzNXowVswTWdYgxUWThIdkWYaOW.Q<Toggle>("update-kinematic-particles");
            otbsCxLMktvYPiQNzrDSMDuVgAptOOKD.value = SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.UpdateKinematicParticles;
            otbsCxLMktvYPiQNzrDSMDuVgAptOOKD.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY, "Changed Property in RigidbodyActor");
                SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.UpdateKinematicParticles = evt.newValue;
                EditorUtility.SetDirty(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY);
            });

            spECJIApmYhjYDWTaKOGyTAkUZNgqPkC(SkonTlwGovRTcnfqtTGVQlVGTcXhiSkY.IsKinematic);

            return cdXJVRzNXowVswTWdYgxUWThIdkWYaOW;
        }

        private void qGTnOOEnZlpXNDcQPWxlDbJkuZOaFxkR()
        {
            if (ToolManager.activeToolType == typeof(BasePhysicsActorEditorTool))
            {
                ToolManager.RestorePreviousPersistentTool();
            }
        }

        private void BEulTAuQfslgUNJeYJTXUQqfOynfWRuR()
        {
            if (ToolManager.activeToolType != typeof(BasePhysicsActorEditorTool))
            {
                ToolManager.SetActiveTool<BasePhysicsActorEditorTool>();
            }
        }

        private void spECJIApmYhjYDWTaKOGyTAkUZNgqPkC(bool enabled)
        {
            var jdPJHNuqAYFSRgbocJWsuhLTdHgDWvPM = cdXJVRzNXowVswTWdYgxUWThIdkWYaOW.Q<Toggle>("update-kinematic-particles");
            jdPJHNuqAYFSRgbocJWsuhLTdHgDWvPM.style.display = enabled ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void GBHKKUCSQMgNiZXkPppjPQsjLtzWlTNn()
        {
            
            if (!Application.isPlaying)
            {
                var FTDfVkSUAGFEXejsMWmmUqJccKDcssoN = target as NeedleActor;
                if (FTDfVkSUAGFEXejsMWmmUqJccKDcssoN.NeedlePoints == null || FTDfVkSUAGFEXejsMWmmUqJccKDcssoN.NeedlePoints.Count == 0 || FTDfVkSUAGFEXejsMWmmUqJccKDcssoN.SharedSimulationMesh == null)
                {
                    return;
                }

                if (FTDfVkSUAGFEXejsMWmmUqJccKDcssoN.NeedlePoints.Count != FTDfVkSUAGFEXejsMWmmUqJccKDcssoN.SharedSimulationMesh.Particles.Length)
                {
                    Undo.RecordObject(FTDfVkSUAGFEXejsMWmmUqJccKDcssoN, "Changed Property in RigidbodyActor");
                    FTDfVkSUAGFEXejsMWmmUqJccKDcssoN.RebuildSimulationMesh();
                    EditorUtility.SetDirty(FTDfVkSUAGFEXejsMWmmUqJccKDcssoN);
                    return;
                }

                for (int mbgrFvrGcyvFxNCkcUOMxRXpsnjdqhgc = 0; mbgrFvrGcyvFxNCkcUOMxRXpsnjdqhgc < FTDfVkSUAGFEXejsMWmmUqJccKDcssoN.SharedSimulationMesh.Particles.Length; mbgrFvrGcyvFxNCkcUOMxRXpsnjdqhgc++)
                {
                    if (FTDfVkSUAGFEXejsMWmmUqJccKDcssoN.transform.TransformPoint(FTDfVkSUAGFEXejsMWmmUqJccKDcssoN.SharedSimulationMesh.Particles[mbgrFvrGcyvFxNCkcUOMxRXpsnjdqhgc].Position) != FTDfVkSUAGFEXejsMWmmUqJccKDcssoN.NeedlePoints[mbgrFvrGcyvFxNCkcUOMxRXpsnjdqhgc].transform.position)
                    {
                        Undo.RecordObject(FTDfVkSUAGFEXejsMWmmUqJccKDcssoN, "Changed Property in RigidbodyActor");
                        FTDfVkSUAGFEXejsMWmmUqJccKDcssoN.RebuildSimulationMesh();
                        EditorUtility.SetDirty(FTDfVkSUAGFEXejsMWmmUqJccKDcssoN);
                        return;
                    }
                }
            }
        }

        private void ViTnyQGBAKHShNmuRDGzWcEpWGATYioo()
        {
            qGTnOOEnZlpXNDcQPWxlDbJkuZOaFxkR();
        }
    }
}
