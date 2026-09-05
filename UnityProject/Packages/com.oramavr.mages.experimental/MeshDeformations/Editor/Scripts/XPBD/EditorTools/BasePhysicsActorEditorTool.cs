namespace MAGES.MeshDeformations.Editor
{
    using System;
    using Unity.Mathematics;
    using UnityEditor;
    using UnityEditor.EditorTools;
    using UnityEngine;

    /// <summary>
    /// A Tool for editing particles of a BasePhysicsActor.
    /// </summary>
    [EditorTool("Edit Particles", typeof(BasePhysicsActor))]
    internal class BasePhysicsActorEditorTool : EditorTool, IDrawSelectedHandles
    {
        private const string ByCjaWWcgdtEilsRDdZVRvNHDhuOrdby = "Packages/com.oramavr.mages.experimental/MeshDeformations/Editor/Icons/edit-particles.png";

        private static float VsNpVEmLRJulcJUwJiZcerOuqEztqJOA = .1f;
        private SimulationMesh ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq;
        private BasePhysicsActor yEApkDjJFeXZpQKjDjsOhrULjQkkfkJC;
        private Particle[] kyWcFedakRvmaeuxrrCoQmKBgROZtNim = new Particle[0];

        private GUIContent FJAbBhlnzzdSFxNmnbVgXceKIFGYNKet;

        /// <inheritdoc/>
        public override GUIContent toolbarIcon => FJAbBhlnzzdSFxNmnbVgXceKIFGYNKet;

        /// <inheritdoc/>
        public override void OnActivated()
        {
            SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Entering Softbody Tool"), .5f);
        }

        /// <inheritdoc/>
        public override void OnWillBeDeactivated()
        {
            SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Exiting Softbody Tool"), .5f);
        }

        /// <inheritdoc/>
        public override void OnToolGUI(EditorWindow window)
        {
            if (!(window is SceneView))
            {
                return;
            }

            if (Application.isPlaying)
            {
                ToolManager.RestorePreviousPersistentTool();
                return;
            }

            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            yEApkDjJFeXZpQKjDjsOhrULjQkkfkJC = target as BasePhysicsActor;

            if (yEApkDjJFeXZpQKjDjsOhrULjQkkfkJC == null)
            {
                return;
            }

            ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq = yEApkDjJFeXZpQKjDjsOhrULjQkkfkJC.SharedSimulationMesh;

            if (ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq == null)
            {
                return;
            }

            float ZkgAJggFRWmjBFnOUFPLsGvImVGlUJlZ = yEApkDjJFeXZpQKjDjsOhrULjQkkfkJC != null && yEApkDjJFeXZpQKjDjsOhrULjQkkfkJC.PhysicsWorld != null ? yEApkDjJFeXZpQKjDjsOhrULjQkkfkJC.PhysicsWorld.ParticleRadius * 2.0f : VsNpVEmLRJulcJUwJiZcerOuqEztqJOA;

            var oTQtJusgtjLEZhJKMXSUBSNRgDxTcCsL = Handles.zTest;
            Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
            var BJJSCQsCgZHGuqwTPjjbGPrrbbwhafOw = Handles.color;

            
            if (kyWcFedakRvmaeuxrrCoQmKBgROZtNim.Length != ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq.Particles.Length)
            {
                kyWcFedakRvmaeuxrrCoQmKBgROZtNim = new Particle[ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq.Particles.Length];
            }

            Array.Copy(ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq.Particles, kyWcFedakRvmaeuxrrCoQmKBgROZtNim, ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq.Particles.Length);
            Array.Sort(kyWcFedakRvmaeuxrrCoQmKBgROZtNim, MBAuOmvLBhFwbwqIjoCFFLQziSwvVnWs);

            for (int vjQCiLPSVCNRGnJJVUEeUikiSCOtlGPA = kyWcFedakRvmaeuxrrCoQmKBgROZtNim.Length - 1; vjQCiLPSVCNRGnJJVUEeUikiSCOtlGPA >= 0; vjQCiLPSVCNRGnJJVUEeUikiSCOtlGPA--)
            {
                Vector3 XeHDgPboHOTGfFWJtPmXdrrjMWJQWRHG = yEApkDjJFeXZpQKjDjsOhrULjQkkfkJC.transform.localToWorldMatrix.MultiplyPoint3x4(kyWcFedakRvmaeuxrrCoQmKBgROZtNim[vjQCiLPSVCNRGnJJVUEeUikiSCOtlGPA].Position);

                float bBYMDnDMlKzMcbTcUNxiZjlsqBUYSyeO = HandleUtility.GetHandleSize(XeHDgPboHOTGfFWJtPmXdrrjMWJQWRHG);

                if (kyWcFedakRvmaeuxrrCoQmKBgROZtNim[vjQCiLPSVCNRGnJJVUEeUikiSCOtlGPA].Kinematic)
                {
                    Handles.color = Color.red;
                }
                else
                {
                    Handles.color = Color.green;
                }

                Handles.SphereHandleCap(0, XeHDgPboHOTGfFWJtPmXdrrjMWJQWRHG, Quaternion.identity, ZkgAJggFRWmjBFnOUFPLsGvImVGlUJlZ > bBYMDnDMlKzMcbTcUNxiZjlsqBUYSyeO ? bBYMDnDMlKzMcbTcUNxiZjlsqBUYSyeO : ZkgAJggFRWmjBFnOUFPLsGvImVGlUJlZ, EventType.Repaint);
            }

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                var JdYnhRqHmcbsojNoErdqAoqdqrzGlPOO = NeiXLuDLypUmaEUdAEBIiGfPIzBJwypq(ZkgAJggFRWmjBFnOUFPLsGvImVGlUJlZ / 2.0f);

                if (JdYnhRqHmcbsojNoErdqAoqdqrzGlPOO >= 0)
                {
                    
                    if (Event.current.shift)
                    {
                        ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq.Particles[JdYnhRqHmcbsojNoErdqAoqdqrzGlPOO].Kinematic = false;
                    }

                    
                    else
                    {
                        ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq.Particles[JdYnhRqHmcbsojNoErdqAoqdqrzGlPOO].Kinematic = true;
                        Debug.Log($"Selected Particle {JdYnhRqHmcbsojNoErdqAoqdqrzGlPOO}");
                    }

                    EditorUtility.SetDirty(ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq);
                }

                Event.current.Use();
            }

            Handles.zTest = oTQtJusgtjLEZhJKMXSUBSNRgDxTcCsL;
            Handles.color = BJJSCQsCgZHGuqwTPjjbGPrrbbwhafOw;
        }

        /// <inheritdoc cref="IDrawSelectedHandles.OnDrawHandles"/>
        public void OnDrawHandles()
        {
            if (!Application.isPlaying)
            {
                ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq = (target as BasePhysicsActor).SharedSimulationMesh;

                if (ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq != null)
                {
                    float3 bQsPDYWcDcOiodQtCBESrTYFlsZiWxdP, XwuadEaqEPAOZeHtZxaJpVCdSpECnhpq, rlZQBcXTiRCLsNlvKxXIHVLZoNNmObQi, ZVwXvRfjoYpJRRyLfLnVKaqluLVmidSr;

                    var toPRIRONveCHsaRUNJeyiWdmgTJyTeuK = ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq.Particles;

                    float4x4 vSIGkckpIGqPXVuTNPSFxSqpXakguFcd = (target as MonoBehaviour).transform.localToWorldMatrix;

                    Handles.color = Color.green;
                    foreach (VolumeConstraint vc in ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq.VolumeConstraints)
                    {
                        bQsPDYWcDcOiodQtCBESrTYFlsZiWxdP = math.mul(vSIGkckpIGqPXVuTNPSFxSqpXakguFcd, new float4(toPRIRONveCHsaRUNJeyiWdmgTJyTeuK[vc.ParticleA].Position, 1.0f)).xyz;
                        XwuadEaqEPAOZeHtZxaJpVCdSpECnhpq = math.mul(vSIGkckpIGqPXVuTNPSFxSqpXakguFcd, new float4(toPRIRONveCHsaRUNJeyiWdmgTJyTeuK[vc.ParticleB].Position, 1.0f)).xyz;
                        rlZQBcXTiRCLsNlvKxXIHVLZoNNmObQi = math.mul(vSIGkckpIGqPXVuTNPSFxSqpXakguFcd, new float4(toPRIRONveCHsaRUNJeyiWdmgTJyTeuK[vc.ParticleC].Position, 1.0f)).xyz;
                        ZVwXvRfjoYpJRRyLfLnVKaqluLVmidSr = math.mul(vSIGkckpIGqPXVuTNPSFxSqpXakguFcd, new float4(toPRIRONveCHsaRUNJeyiWdmgTJyTeuK[vc.ParticleD].Position, 1.0f)).xyz;

                        Handles.DrawLines(new Vector3[] { bQsPDYWcDcOiodQtCBESrTYFlsZiWxdP, XwuadEaqEPAOZeHtZxaJpVCdSpECnhpq, XwuadEaqEPAOZeHtZxaJpVCdSpECnhpq, rlZQBcXTiRCLsNlvKxXIHVLZoNNmObQi, rlZQBcXTiRCLsNlvKxXIHVLZoNNmObQi, bQsPDYWcDcOiodQtCBESrTYFlsZiWxdP, bQsPDYWcDcOiodQtCBESrTYFlsZiWxdP, ZVwXvRfjoYpJRRyLfLnVKaqluLVmidSr, XwuadEaqEPAOZeHtZxaJpVCdSpECnhpq, ZVwXvRfjoYpJRRyLfLnVKaqluLVmidSr, rlZQBcXTiRCLsNlvKxXIHVLZoNNmObQi, ZVwXvRfjoYpJRRyLfLnVKaqluLVmidSr });
                    }
                }
            }
        }

        private int MBAuOmvLBhFwbwqIjoCFFLQziSwvVnWs(Particle a, Particle b)
        {
            float4x4 yXAtORqFTQHGRmFVqCjCfdDJymYMXBjj = yEApkDjJFeXZpQKjDjsOhrULjQkkfkJC.transform.localToWorldMatrix;

            float3 OcDZbnezHDZWVHLhqQdKsIUJOdyHyNRt = math.mul(yXAtORqFTQHGRmFVqCjCfdDJymYMXBjj, new float4(a.Position, 1.0f)).xyz;
            float3 kANZsltsbFBtFqsjkjnqOtIxpQcfukTW = math.mul(yXAtORqFTQHGRmFVqCjCfdDJymYMXBjj, new float4(b.Position, 1.0f)).xyz;

            return math.distancesq(OcDZbnezHDZWVHLhqQdKsIUJOdyHyNRt, Camera.current.transform.position).CompareTo(math.distancesq(kANZsltsbFBtFqsjkjnqOtIxpQcfukTW, Camera.current.transform.position));
        }

        private int NeiXLuDLypUmaEUdAEBIiGfPIzBJwypq(float nodeSelectionRadius)
        {
            var jTBEfHMLlyilUjLYLCyJvYqhgpRlOYJb = -1;
            var qnHEeSmQssPKiBbfTCAwkJAbWkqXLirt = float.MaxValue;
            Ray WagTAGisbfocZMATKVMzgWSzhuVaFzUM = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            for (int sNjSCorDBEVZftBawIbPAYUbVApaDCly = 0; sNjSCorDBEVZftBawIbPAYUbVApaDCly < ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq.Particles.Length; sNjSCorDBEVZftBawIbPAYUbVApaDCly++)
            {
                Vector3 rlCJNAjpEmURyxnWLKpeQNHoHNZHQRRl = yEApkDjJFeXZpQKjDjsOhrULjQkkfkJC.transform.localToWorldMatrix.MultiplyPoint3x4(ZpMrtLSGMiyUsdqnZUbYCufzVPTErmCq.Particles[sNjSCorDBEVZftBawIbPAYUbVApaDCly].Position);
                float VOqLiBagILJiKrdnFZdYEFxZASQSWKZy = HandleUtility.GetHandleSize(rlCJNAjpEmURyxnWLKpeQNHoHNZHQRRl) / 2.0f;

                
                if (HandleUtility.DistanceToCircle(rlCJNAjpEmURyxnWLKpeQNHoHNZHQRRl, nodeSelectionRadius < VOqLiBagILJiKrdnFZdYEFxZASQSWKZy ? nodeSelectionRadius : VOqLiBagILJiKrdnFZdYEFxZASQSWKZy) == 0f)
                {
                    var QwhJePKMrEISKGiRUiyBRIbQDteOThZU = WagTAGisbfocZMATKVMzgWSzhuVaFzUM.direction;
                    var JueTqvQDzIvrDgffPaxrOejhSfZevIIi = Vector3.Project(rlCJNAjpEmURyxnWLKpeQNHoHNZHQRRl - WagTAGisbfocZMATKVMzgWSzhuVaFzUM.origin, WagTAGisbfocZMATKVMzgWSzhuVaFzUM.direction);

                    if (Vector3.Dot(QwhJePKMrEISKGiRUiyBRIbQDteOThZU, JueTqvQDzIvrDgffPaxrOejhSfZevIIi) > 0)
                    {
                        var aiAdQqLIrOKvSCRmvupCfvzDYpIQxIAu = JueTqvQDzIvrDgffPaxrOejhSfZevIIi.magnitude;

                        if (aiAdQqLIrOKvSCRmvupCfvzDYpIQxIAu < qnHEeSmQssPKiBbfTCAwkJAbWkqXLirt)
                        {
                            qnHEeSmQssPKiBbfTCAwkJAbWkqXLirt = aiAdQqLIrOKvSCRmvupCfvzDYpIQxIAu;
                            jTBEfHMLlyilUjLYLCyJvYqhgpRlOYJb = sNjSCorDBEVZftBawIbPAYUbVApaDCly;
                        }
                    }
                }
            }

            return jTBEfHMLlyilUjLYLCyJvYqhgpRlOYJb;
        }

        private void LUjbnamgsbvvIUqOBSKJgvdIAXMnZvMv()
        {
            Texture PdEWKfYfmCngJMQgAPpBMHQCjmYoClyG = AssetDatabase.LoadAssetAtPath<Texture>(ByCjaWWcgdtEilsRDdZVRvNHDhuOrdby);
            FJAbBhlnzzdSFxNmnbVgXceKIFGYNKet = new GUIContent(PdEWKfYfmCngJMQgAPpBMHQCjmYoClyG, "Edit Particles");
        }

        private void XVyREpeBPwvBeVDsfbqjhnAWnfeDMIiw()
        {
            FJAbBhlnzzdSFxNmnbVgXceKIFGYNKet = null;
        }
    }
}
