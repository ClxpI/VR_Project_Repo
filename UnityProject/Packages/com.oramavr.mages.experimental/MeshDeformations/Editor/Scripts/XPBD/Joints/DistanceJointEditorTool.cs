namespace MAGES.MeshDeformations.Editor
{
    using System;
    using Unity.Mathematics;
    using UnityEditor;
    using UnityEditor.EditorTools;
    using UnityEngine;

    /// <summary>
    /// Distance joint editing tool.
    /// </summary>
    [EditorTool("Edit Joint", typeof(DistanceJoint))]
    public class DistanceJointEditorTool : EditorTool, IDrawSelectedHandles
    {
        private const string VtaEngfMJuGyxRuRhlfQBuDWrwvAidTJ = "Packages/com.oramavr.mages.experimental/MeshDeformations/Editor/Icons/edit-distance-joint.png";
        private static float oKHpjAHwHGocyBhpJWZrPyndvmKWjfxU = .1f;

        private DistanceJoint wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON;

        private GUIContent TqUGFQJTvuedCzMzdBCxpTIuUExJBlbh;

        /// <inheritdoc/>
        public override GUIContent toolbarIcon => TqUGFQJTvuedCzMzdBCxpTIuUExJBlbh;

        /// <inheritdoc/>
        public override void OnActivated()
        {
            SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Entering Joint Tool"), .5f);
        }

        /// <inheritdoc/>
        public override void OnWillBeDeactivated()
        {
            SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Exiting Joint Tool"), .5f);
        }

        /// <inheritdoc/>
        public override void OnToolGUI(EditorWindow window)
        {
            DistanceJoint dFvvWwhSegeODzHtXqZJiECOehLmDHzv = target as DistanceJoint;

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

            BasePhysicsActor aosEMoHvhCDrNpcsYWdsliGmUXbbimJY = dFvvWwhSegeODzHtXqZJiECOehLmDHzv.Actor;
            BasePhysicsActor JeOwfeZsujjxcNZsSFvJfUwdfYeqCdGC = dFvvWwhSegeODzHtXqZJiECOehLmDHzv.ConnectedActor;

            if (aosEMoHvhCDrNpcsYWdsliGmUXbbimJY != null && aosEMoHvhCDrNpcsYWdsliGmUXbbimJY.SharedSimulationMesh != null)
            {
                var ngVvTstFvQiIJDVjSlqqzANgatrixJNw = MxCtfOmfGzHTEMMMkEuOGBsChbeoQUFD(aosEMoHvhCDrNpcsYWdsliGmUXbbimJY, dFvvWwhSegeODzHtXqZJiECOehLmDHzv.SimulationMeshParticleIndex);
                if (ngVvTstFvQiIJDVjSlqqzANgatrixJNw != dFvvWwhSegeODzHtXqZJiECOehLmDHzv.SimulationMeshParticleIndex)
                {
                    Undo.RecordObject(dFvvWwhSegeODzHtXqZJiECOehLmDHzv, "Modified property in DistanceConstraint");
                    dFvvWwhSegeODzHtXqZJiECOehLmDHzv.SimulationMeshParticleIndex = ngVvTstFvQiIJDVjSlqqzANgatrixJNw;
                    EditorUtility.SetDirty(dFvvWwhSegeODzHtXqZJiECOehLmDHzv);
                }
            }

            if (JeOwfeZsujjxcNZsSFvJfUwdfYeqCdGC != null && JeOwfeZsujjxcNZsSFvJfUwdfYeqCdGC.SharedSimulationMesh != null)
            {
                var TYjATZAbRiNkoKLvjYQIfbKkbFOIukSi = MxCtfOmfGzHTEMMMkEuOGBsChbeoQUFD(JeOwfeZsujjxcNZsSFvJfUwdfYeqCdGC, dFvvWwhSegeODzHtXqZJiECOehLmDHzv.ConnectedSimulationMeshParticleIndex);
                if (TYjATZAbRiNkoKLvjYQIfbKkbFOIukSi != dFvvWwhSegeODzHtXqZJiECOehLmDHzv.ConnectedSimulationMeshParticleIndex)
                {
                    Undo.RecordObject(dFvvWwhSegeODzHtXqZJiECOehLmDHzv, "Modified property in DistanceConstraint");
                    dFvvWwhSegeODzHtXqZJiECOehLmDHzv.ConnectedSimulationMeshParticleIndex = TYjATZAbRiNkoKLvjYQIfbKkbFOIukSi;
                    EditorUtility.SetDirty(dFvvWwhSegeODzHtXqZJiECOehLmDHzv);
                }
            }
        }

        /// <inheritdoc cref="IDrawSelectedHandles.OnDrawHandles"/>
        public void OnDrawHandles()
        {
            wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON = target as DistanceJoint;

            if (Application.isPlaying)
            {
                return;
            }

            if (wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.Actor == null || wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.Actor.SharedSimulationMesh == null || wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.ConnectedActor == null || wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.ConnectedActor.SharedSimulationMesh == null)
            {
                return;
            }

            if ((wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.SimulationMeshParticleIndex < 0 || wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.SimulationMeshParticleIndex >= wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.Actor.SharedSimulationMesh.Particles.Length) || (wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.ConnectedSimulationMeshParticleIndex < 0 || wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.ConnectedSimulationMeshParticleIndex >= wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.ConnectedActor.SharedSimulationMesh.Particles.Length))
            {
                return;
            }

            Vector3 TKHQAcPeHebgVJMmFVuwSAdOZwgPyOjv = wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.Actor.transform.localToWorldMatrix.MultiplyPoint3x4(wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.Actor.SharedSimulationMesh.Particles[wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.SimulationMeshParticleIndex].Position);
            Vector3 WlWdvqnfSjBJZXDYYSpXRLNWCpgZGWqF = wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.ConnectedActor.transform.localToWorldMatrix.MultiplyPoint(wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.ConnectedActor.SharedSimulationMesh.Particles[wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.ConnectedSimulationMeshParticleIndex].Position);

            Handles.color = Color.green;
            Handles.DrawLine(TKHQAcPeHebgVJMmFVuwSAdOZwgPyOjv, WlWdvqnfSjBJZXDYYSpXRLNWCpgZGWqF);

            Handles.color = Color.white;
            var PPEEcwMUhKOsbpmhoHDNybANuSBjWuma = (WlWdvqnfSjBJZXDYYSpXRLNWCpgZGWqF - TKHQAcPeHebgVJMmFVuwSAdOZwgPyOjv).normalized;
            Handles.DrawLine(TKHQAcPeHebgVJMmFVuwSAdOZwgPyOjv, TKHQAcPeHebgVJMmFVuwSAdOZwgPyOjv + (wKUpBerfEgjnaGCBrMnGtHaqHLtLmUON.TargetDistance * PPEEcwMUhKOsbpmhoHDNybANuSBjWuma));
        }

        private int MxCtfOmfGzHTEMMMkEuOGBsChbeoQUFD(BasePhysicsActor physicsActor, int selectedParticle)
        {
            if (physicsActor == null)
            {
                return -1;
            }

            var gGcYlpgLZiuaDIQDUMWluKCLmRfQnNMT = physicsActor.SharedSimulationMesh;

            if (gGcYlpgLZiuaDIQDUMWluKCLmRfQnNMT == null)
            {
                return -1;
            }

            float xmHUNCGAKTxMnNuzUqWJZbvYRDIqVmJv = physicsActor != null && physicsActor.PhysicsWorld != null ? physicsActor.PhysicsWorld.ParticleRadius * 2.0f : oKHpjAHwHGocyBhpJWZrPyndvmKWjfxU;

            var ihITnsldwomvtQkeaDrwZdjhvunJoJhj = Handles.zTest;
            Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
            var aNugQGYRkUxLDPpOwtmaygdfZiPkeWrK = Handles.color;

            var aHlDZTmoGHKipnoRXbUflBlDQideBQXN = new (int, Particle)[gGcYlpgLZiuaDIQDUMWluKCLmRfQnNMT.Particles.Length];

            for (int RFLMXwNQDlVRYTvqlYmTbuozTrMGyrXM = 0; RFLMXwNQDlVRYTvqlYmTbuozTrMGyrXM < gGcYlpgLZiuaDIQDUMWluKCLmRfQnNMT.Particles.Length; RFLMXwNQDlVRYTvqlYmTbuozTrMGyrXM++)
            {
                aHlDZTmoGHKipnoRXbUflBlDQideBQXN[RFLMXwNQDlVRYTvqlYmTbuozTrMGyrXM] = (RFLMXwNQDlVRYTvqlYmTbuozTrMGyrXM, gGcYlpgLZiuaDIQDUMWluKCLmRfQnNMT.Particles[RFLMXwNQDlVRYTvqlYmTbuozTrMGyrXM]);
            }

            Array.Sort(aHlDZTmoGHKipnoRXbUflBlDQideBQXN, CompareParticlesByDepthToCamera);

            for (int yPcPIHpxlfzdWKCzXZjNHnkbFrHnuaby = aHlDZTmoGHKipnoRXbUflBlDQideBQXN.Length - 1; yPcPIHpxlfzdWKCzXZjNHnkbFrHnuaby >= 0; yPcPIHpxlfzdWKCzXZjNHnkbFrHnuaby--)
            {
                Vector3 sXvZNPMqOiQMppwFZiDTsdPKuWGTjLfT = physicsActor.transform.localToWorldMatrix.MultiplyPoint3x4(aHlDZTmoGHKipnoRXbUflBlDQideBQXN[yPcPIHpxlfzdWKCzXZjNHnkbFrHnuaby].Item2.Position);

                float DdDNoEJsCnspNJGJFxJAgUKsAzVyrksS = HandleUtility.GetHandleSize(sXvZNPMqOiQMppwFZiDTsdPKuWGTjLfT);

                if (aHlDZTmoGHKipnoRXbUflBlDQideBQXN[yPcPIHpxlfzdWKCzXZjNHnkbFrHnuaby].Item1 == selectedParticle)
                {
                    Handles.color = Color.red;
                }
                else
                {
                    Handles.color = Color.green;
                }

                Handles.SphereHandleCap(0, sXvZNPMqOiQMppwFZiDTsdPKuWGTjLfT, Quaternion.identity, xmHUNCGAKTxMnNuzUqWJZbvYRDIqVmJv > DdDNoEJsCnspNJGJFxJAgUKsAzVyrksS ? DdDNoEJsCnspNJGJFxJAgUKsAzVyrksS : xmHUNCGAKTxMnNuzUqWJZbvYRDIqVmJv, EventType.Repaint);
            }

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                var GFOGcMJOgMQkHhprVnXFzgHTUEpSMndu = UmLdQjOePfbfpFewwUaIMGLjqpucrkKZ(physicsActor, xmHUNCGAKTxMnNuzUqWJZbvYRDIqVmJv / 2.0f);

                
                if (GFOGcMJOgMQkHhprVnXFzgHTUEpSMndu >= 0)
                {
                    Event.current.Use();
                    return GFOGcMJOgMQkHhprVnXFzgHTUEpSMndu;
                }
            }

            Handles.zTest = ihITnsldwomvtQkeaDrwZdjhvunJoJhj;
            Handles.color = aNugQGYRkUxLDPpOwtmaygdfZiPkeWrK;

            return selectedParticle;

            int CompareParticlesByDepthToCamera((int, Particle) a, (int, Particle) b)
            {
                float4x4 zXhLOZYPeKQFiMjAvXSvLMvCuEhONgbM = physicsActor.transform.localToWorldMatrix;

                float3 tImRHbwLEEOGdOSCdspAdBjspXsmdFvL = math.mul(zXhLOZYPeKQFiMjAvXSvLMvCuEhONgbM, new float4(a.Item2.Position, 1.0f)).xyz;
                float3 aMZeEuLqrdueUbENkGiKcImxwmgKvyoA = math.mul(zXhLOZYPeKQFiMjAvXSvLMvCuEhONgbM, new float4(b.Item2.Position, 1.0f)).xyz;

                return math.distancesq(tImRHbwLEEOGdOSCdspAdBjspXsmdFvL, Camera.current.transform.position).CompareTo(math.distancesq(aMZeEuLqrdueUbENkGiKcImxwmgKvyoA, Camera.current.transform.position));
            }
        }

        private int UmLdQjOePfbfpFewwUaIMGLjqpucrkKZ(in BasePhysicsActor actor, float nodeSelectionRadius)
        {
            var uimQuMVPgfSXTAFjusCdLRVqMhizFdZX = -1;
            var kAEoHTGMqtZFQUUkmvTjrCrZBubnlHrj = float.MaxValue;
            Ray aYOfxSwbZKyyyYcwrIGVwhjLmPkWpvRV = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            if (actor == null || actor.SharedSimulationMesh == null)
            {
                return uimQuMVPgfSXTAFjusCdLRVqMhizFdZX;
            }

            for (int bbJBpqWuHpYZFZgpPkicEdrPqgjxqcgN = 0; bbJBpqWuHpYZFZgpPkicEdrPqgjxqcgN < actor.SharedSimulationMesh.Particles.Length; bbJBpqWuHpYZFZgpPkicEdrPqgjxqcgN++)
            {
                Vector3 TWqZQQcEdZhEhjggkPYVCJDbNusqqsaZ = actor.transform.localToWorldMatrix.MultiplyPoint3x4(actor.SharedSimulationMesh.Particles[bbJBpqWuHpYZFZgpPkicEdrPqgjxqcgN].Position);
                float fkhcHPKxNIcUCpMxfnkcTACJhuhmXQDj = HandleUtility.GetHandleSize(TWqZQQcEdZhEhjggkPYVCJDbNusqqsaZ) / 2.0f;

                
                if (HandleUtility.DistanceToCircle(TWqZQQcEdZhEhjggkPYVCJDbNusqqsaZ, nodeSelectionRadius < fkhcHPKxNIcUCpMxfnkcTACJhuhmXQDj ? nodeSelectionRadius : fkhcHPKxNIcUCpMxfnkcTACJhuhmXQDj) == 0f)
                {
                    var ybtNBZkDFpGwUlHlccqLAGjMbDFfoira = aYOfxSwbZKyyyYcwrIGVwhjLmPkWpvRV.direction;
                    var elcVQpQOqeAFULKhnrYIXxFsHzMamKId = Vector3.Project(TWqZQQcEdZhEhjggkPYVCJDbNusqqsaZ - aYOfxSwbZKyyyYcwrIGVwhjLmPkWpvRV.origin, aYOfxSwbZKyyyYcwrIGVwhjLmPkWpvRV.direction);

                    if (Vector3.Dot(ybtNBZkDFpGwUlHlccqLAGjMbDFfoira, elcVQpQOqeAFULKhnrYIXxFsHzMamKId) > 0)
                    {
                        var LKZUIflabFuacvBVBOEwEnfmkUIlVTzO = elcVQpQOqeAFULKhnrYIXxFsHzMamKId.magnitude;

                        if (LKZUIflabFuacvBVBOEwEnfmkUIlVTzO < kAEoHTGMqtZFQUUkmvTjrCrZBubnlHrj)
                        {
                            kAEoHTGMqtZFQUUkmvTjrCrZBubnlHrj = LKZUIflabFuacvBVBOEwEnfmkUIlVTzO;
                            uimQuMVPgfSXTAFjusCdLRVqMhizFdZX = bbJBpqWuHpYZFZgpPkicEdrPqgjxqcgN;
                        }
                    }
                }
            }

            return uimQuMVPgfSXTAFjusCdLRVqMhizFdZX;
        }

        private void WMlFDnOxJCAUWiMXBzcgMbfhXnMmIAgO()
        {
            Texture BjuYEIerNNsCWtEVvELsoDqkdeFgWECt = AssetDatabase.LoadAssetAtPath<Texture>(VtaEngfMJuGyxRuRhlfQBuDWrwvAidTJ);
            TqUGFQJTvuedCzMzdBCxpTIuUExJBlbh = new GUIContent(BjuYEIerNNsCWtEVvELsoDqkdeFgWECt, "Edit Joint");
        }

        private void baLbrhfmBtelZrADPtzounBVyYFbjnjn()
        {
            TqUGFQJTvuedCzMzdBCxpTIuUExJBlbh = null;
        }
    }
}
