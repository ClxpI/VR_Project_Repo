namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;
    using UnityEngine;
    using Debug = UnityEngine.Debug;

    /// <summary>
    /// A softbody simulation actor.
    /// </summary>
    [AddComponentMenu("MAGES/Mesh Deformations/Actors/Softbody Actor")]
    [RequireComponent(typeof(MeshFilter))]
    public class SoftbodyActor : BasePhysicsActor
    {
        private Mesh LORPcyKupMbZDTTlzVvPnoQHeqcOPttL;
        private Mesh FJlWThAkhhYaFCrFAkedDqrlYvfcQaoq;

        private NativeArray<float3> ZJMFUkcWTsEdguhvTSuJwQqzamSEQsFv;
        private NativeArray<ubcLnfKJugQVRuDYhwkkzbIqxUOzvLVD> DzzdLJxHIDfKAJPZnWXsficFApiYwrrs;

        private float4x4 peSoBtanQprQxbrhtAWiVaZseWEOfjpB;

        [SerializeField]
        private uint layer = 1;

        /// <summary>
        /// Gets or sets the layer used for collision filtering.
        /// </summary>
        public uint Layer
        {
            get
            {
                return layer;
            }

            set
            {
                if (layer == value)
                {
                    return;
                }

                layer = value;
                if (MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj != null && MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld)
                {
                    uint xlAQSJAIaiQiXTBodCRIFSJDCYlRnYMi = PhysicsWorld.CreatePhase(Layer, PhaseOptions.None);
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.PhysicsWorld.UpdatePhase(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj, xlAQSJAIaiQiXTBodCRIFSJDCYlRnYMi);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to update the kinematic particles position when this actor's transform is updated.
        /// </summary>
        [field: SerializeField]
        public bool UpdateKinematicParticles { get; set; } = true;

        /// <inheritdoc/>
        public override void AddToPhysicsWorld()
        {
            if (SharedSimulationMesh == null)
            {
                Debug.LogError("SimulationMeshSource is not assigned. Softbody actor will not be simulated.", this);
                return;
            }

            if (PhysicsWorld == null)
            {
                Debug.LogError("PhysicsWorld is not assigned. Softbody actor will not be simulated.", this);
                return;
            }

            pHnDYZkOyvrpazWpcfeFqlMwFtaXfQHi();

            uint lXWVTlsdBGCERPLolVkVtvAwkNWRwBXn = PhysicsWorld.CreatePhase(Layer, PhaseOptions.None);

            peSoBtanQprQxbrhtAWiVaZseWEOfjpB = transform.worldToLocalMatrix;
            PhysicsWorld.AddSimulationMesh(SimulationMesh, transform.localToWorldMatrix, lXWVTlsdBGCERPLolVkVtvAwkNWRwBXn);

            base.AddToPhysicsWorld();
        }

        /// <inheritdoc/>
        public override void RemoveFromPhysicsWorld()
        {
            base.RemoveFromPhysicsWorld();

            
            PhysicsWorld?.RemoveSimulationMesh(SimulationMesh);

            
            if (ZJMFUkcWTsEdguhvTSuJwQqzamSEQsFv.IsCreated)
            {
                ZJMFUkcWTsEdguhvTSuJwQqzamSEQsFv.Dispose();
            }

            if (DzzdLJxHIDfKAJPZnWXsficFApiYwrrs.IsCreated)
            {
                DzzdLJxHIDfKAJPZnWXsficFApiYwrrs.Dispose();
            }

            
            Destroy(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj);
            MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj = null;

            
            Destroy(FJlWThAkhhYaFCrFAkedDqrlYvfcQaoq);
            FJlWThAkhhYaFCrFAkedDqrlYvfcQaoq = Instantiate(LORPcyKupMbZDTTlzVvPnoQHeqcOPttL);
            FJlWThAkhhYaFCrFAkedDqrlYvfcQaoq.name = LORPcyKupMbZDTTlzVvPnoQHeqcOPttL.name;
            GetComponent<MeshFilter>().mesh = FJlWThAkhhYaFCrFAkedDqrlYvfcQaoq;
        }

        /// <inheritdoc/>
        protected override void PhysicsUpdate(float timeStep)
        {
            if (MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj == null || !MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld)
            {
                return;
            }

            if (UpdateKinematicParticles && transform.hasChanged)
            {
                lnenKqXqXWiczTBlFiTWrPiPaLSuEpNd().Complete();
                transform.hasChanged = false;
            }

            var lLNaJEqJlkGkmeYnxvcARFNWYYuZXasl = new xbYtOrwIiguZXtysjryFiXgyhPbDGXmH
            {
                WorldToLocalMatrix = transform.worldToLocalMatrix,

                ParticlesPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position,
                TetrahedraA = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleA,
                TetrahedraB = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleB,
                TetrahedraC = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleC,
                TetrahedraD = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleD,

                VertexBindData = DzzdLJxHIDfKAJPZnWXsficFApiYwrrs,
                Vertices = ZJMFUkcWTsEdguhvTSuJwQqzamSEQsFv,
            };

            lLNaJEqJlkGkmeYnxvcARFNWYYuZXasl.Schedule(FJlWThAkhhYaFCrFAkedDqrlYvfcQaoq.vertexCount, 64).Complete();

            FJlWThAkhhYaFCrFAkedDqrlYvfcQaoq.SetVertices(ZJMFUkcWTsEdguhvTSuJwQqzamSEQsFv);
            FJlWThAkhhYaFCrFAkedDqrlYvfcQaoq.RecalculateNormals();
            FJlWThAkhhYaFCrFAkedDqrlYvfcQaoq.RecalculateBounds();
        }

        private void pHnDYZkOyvrpazWpcfeFqlMwFtaXfQHi()
        {
            LORPcyKupMbZDTTlzVvPnoQHeqcOPttL = GetComponent<MeshFilter>().sharedMesh;

            FJlWThAkhhYaFCrFAkedDqrlYvfcQaoq = GetComponent<MeshFilter>().mesh;
            FJlWThAkhhYaFCrFAkedDqrlYvfcQaoq.MarkDynamic();

            NativeArray<Particle> IAgroNqMPsbAizPYeUycccewVSgloFRD = new NativeArray<Particle>(SharedSimulationMesh.Particles, Allocator.TempJob);
            NativeArray<VolumeConstraint> orCFGBQGLotmjVYuawKpRZEKgXDlNkWV = new NativeArray<VolumeConstraint>(SharedSimulationMesh.VolumeConstraints, Allocator.TempJob);

            if (ZJMFUkcWTsEdguhvTSuJwQqzamSEQsFv.IsCreated)
            {
                ZJMFUkcWTsEdguhvTSuJwQqzamSEQsFv.Dispose();
            }

            ZJMFUkcWTsEdguhvTSuJwQqzamSEQsFv = new NativeArray<Vector3>(LORPcyKupMbZDTTlzVvPnoQHeqcOPttL.vertices, Allocator.Persistent).Reinterpret<float3>();

            if (DzzdLJxHIDfKAJPZnWXsficFApiYwrrs.IsCreated)
            {
                DzzdLJxHIDfKAJPZnWXsficFApiYwrrs.Dispose();
            }

            DzzdLJxHIDfKAJPZnWXsficFApiYwrrs = new NativeArray<ubcLnfKJugQVRuDYhwkkzbIqxUOzvLVD>(LORPcyKupMbZDTTlzVvPnoQHeqcOPttL.vertexCount, Allocator.Persistent);

            var GRAPFSZwRXYgFizjZyLTCUaOPVzgDyzJ = new vIEBECnDtlWRbmNjXrDbiGXIjWCXHlVF
            {
                SimulationMeshParticlePositions = IAgroNqMPsbAizPYeUycccewVSgloFRD,
                SimulationMeshVolumeConstraints = orCFGBQGLotmjVYuawKpRZEKgXDlNkWV,
                VisualMeshVertices = ZJMFUkcWTsEdguhvTSuJwQqzamSEQsFv,
                VertexBindData = DzzdLJxHIDfKAJPZnWXsficFApiYwrrs,
            };

            GRAPFSZwRXYgFizjZyLTCUaOPVzgDyzJ.Schedule(FJlWThAkhhYaFCrFAkedDqrlYvfcQaoq.vertexCount, 64).Complete();

            IAgroNqMPsbAizPYeUycccewVSgloFRD.Dispose();
            orCFGBQGLotmjVYuawKpRZEKgXDlNkWV.Dispose();
        }

        private void pRrjiMyianYniCtJiJDSqipgYkeXanjk()
        {
            if (!Application.isPlaying && SharedSimulationMesh != null)
            {
                float3 byiEzKIenblEuaEcIhfpPWzbtqrSXLuA, mhBXMSnWZbwXPwECeGHizonBHZaPbsBe, JQNgXvtnxnwMNGRhErPBiTRZoJWvqKAX, rXCoPRkfyhoAemnOMKzDvTvfFNXnusac;

                var wDEisbnGKJNouWigldDVdOUOoxizRzOS = SharedSimulationMesh.Particles;

                float4x4 IaXhORcqCkFCMowPgZRCUAhMriQBTkYh = transform.localToWorldMatrix;

                foreach (VolumeConstraint vc in SharedSimulationMesh.VolumeConstraints)
                {
                    Gizmos.color = Color.green;

                    byiEzKIenblEuaEcIhfpPWzbtqrSXLuA = math.mul(IaXhORcqCkFCMowPgZRCUAhMriQBTkYh, new float4(wDEisbnGKJNouWigldDVdOUOoxizRzOS[vc.ParticleA].Position, 1.0f)).xyz;
                    mhBXMSnWZbwXPwECeGHizonBHZaPbsBe = math.mul(IaXhORcqCkFCMowPgZRCUAhMriQBTkYh, new float4(wDEisbnGKJNouWigldDVdOUOoxizRzOS[vc.ParticleB].Position, 1.0f)).xyz;
                    JQNgXvtnxnwMNGRhErPBiTRZoJWvqKAX = math.mul(IaXhORcqCkFCMowPgZRCUAhMriQBTkYh, new float4(wDEisbnGKJNouWigldDVdOUOoxizRzOS[vc.ParticleC].Position, 1.0f)).xyz;
                    rXCoPRkfyhoAemnOMKzDvTvfFNXnusac = math.mul(IaXhORcqCkFCMowPgZRCUAhMriQBTkYh, new float4(wDEisbnGKJNouWigldDVdOUOoxizRzOS[vc.ParticleD].Position, 1.0f)).xyz;

                    MAGES.Utilities.GizmoUtilities.DrawTetrahedron(byiEzKIenblEuaEcIhfpPWzbtqrSXLuA, mhBXMSnWZbwXPwECeGHizonBHZaPbsBe, JQNgXvtnxnwMNGRhErPBiTRZoJWvqKAX, rXCoPRkfyhoAemnOMKzDvTvfFNXnusac);
                }
            }
        }

        private JobHandle lnenKqXqXWiczTBlFiTWrPiPaLSuEpNd(JobHandle dependsOn = default)
        {
            var xyIYzNDdINwoONSicTMDXYoijwhgYQsf = new XYcgvuDJwwGETnLAENbDWNUjvDNYZmDy
            {
                InitialWorldToLocalMatrix = peSoBtanQprQxbrhtAWiVaZseWEOfjpB,
                LocalToWorldMatrix = transform.localToWorldMatrix,

                ParticlesInitialPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition,
                ParticlesPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position,
                ParticlesInverseMass = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InverseMass,
            };

            return xyIYzNDdINwoONSicTMDXYoijwhgYQsf.Schedule(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Count, 64, dependsOn);
        }

        
        [BurstCompile]
        private struct vIEBECnDtlWRbmNjXrDbiGXIjWCXHlVF : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<Particle> SimulationMeshParticlePositions;
            [ReadOnly]
            public NativeArray<VolumeConstraint> SimulationMeshVolumeConstraints;

            [ReadOnly]
            public NativeArray<float3> VisualMeshVertices;

            [WriteOnly]
            public NativeArray<ubcLnfKJugQVRuDYhwkkzbIqxUOzvLVD> VertexBindData;

            
            public void Execute(int vertexIndex)
            {
                float3 moUhaLNVSdcgfovZpovriFxQACBXnVzQ = VisualMeshVertices[vertexIndex];

                for (int eJaDBTUtIRADvMfulHOoYFyxaXSrWzYK = 0; eJaDBTUtIRADvMfulHOoYFyxaXSrWzYK < SimulationMeshVolumeConstraints.Length; eJaDBTUtIRADvMfulHOoYFyxaXSrWzYK++)
                {
                    float3 KLHJpFNxkBpGroppENAklQhjGTUKzSrb = SimulationMeshParticlePositions[SimulationMeshVolumeConstraints[eJaDBTUtIRADvMfulHOoYFyxaXSrWzYK].ParticleA].Position;
                    float3 WSwOuqbVRhHYMqxwVPtVugKrFGKjNhNr = SimulationMeshParticlePositions[SimulationMeshVolumeConstraints[eJaDBTUtIRADvMfulHOoYFyxaXSrWzYK].ParticleB].Position;
                    float3 PhbbkytAcTxkcwBNXrqPZfXiVaZeTnAN = SimulationMeshParticlePositions[SimulationMeshVolumeConstraints[eJaDBTUtIRADvMfulHOoYFyxaXSrWzYK].ParticleC].Position;
                    float3 BjtZXFfSfwIOCApRgVMSZPrSHQxYXGmD = SimulationMeshParticlePositions[SimulationMeshVolumeConstraints[eJaDBTUtIRADvMfulHOoYFyxaXSrWzYK].ParticleD].Position;

                    
                    if (Math.IsPointInsideTetrahedron(moUhaLNVSdcgfovZpovriFxQACBXnVzQ, KLHJpFNxkBpGroppENAklQhjGTUKzSrb, WSwOuqbVRhHYMqxwVPtVugKrFGKjNhNr, BjtZXFfSfwIOCApRgVMSZPrSHQxYXGmD, PhbbkytAcTxkcwBNXrqPZfXiVaZeTnAN)
                        && Math.TetrahedronBarycentricCoordinates(moUhaLNVSdcgfovZpovriFxQACBXnVzQ, KLHJpFNxkBpGroppENAklQhjGTUKzSrb, WSwOuqbVRhHYMqxwVPtVugKrFGKjNhNr, PhbbkytAcTxkcwBNXrqPZfXiVaZeTnAN, BjtZXFfSfwIOCApRgVMSZPrSHQxYXGmD, out float4 barCoords))
                    {
                        VertexBindData[vertexIndex] = new ubcLnfKJugQVRuDYhwkkzbIqxUOzvLVD
                        {
                            VolumeConstraintIndex = eJaDBTUtIRADvMfulHOoYFyxaXSrWzYK,
                            BarycentricCoordinates = barCoords,
                        };

                        return;
                    }
                }

                {
                    var LfZwdbQkinnwGTQmfadnnjAHANpKQilL = float.MaxValue;
                    var IawUvQIIfMscsGyktPOyltEnRIuTYRWp = new float4(0.0f, 0.0f, 0.0f, 0.0f);
                    var SITiPlCcQXOOMZRjiFTqpzlctfyNMmnI = -1;

                    for (int EWyilIWVPPjarnyPZDwoqKGFXtsbZSFy = 0; EWyilIWVPPjarnyPZDwoqKGFXtsbZSFy < SimulationMeshVolumeConstraints.Length; EWyilIWVPPjarnyPZDwoqKGFXtsbZSFy++)
                    {
                        var VgumBesvdajXuEGsjWNikYDJbNhfdiJZ = (SimulationMeshParticlePositions[SimulationMeshVolumeConstraints[EWyilIWVPPjarnyPZDwoqKGFXtsbZSFy].ParticleA].Position
                                         + SimulationMeshParticlePositions[SimulationMeshVolumeConstraints[EWyilIWVPPjarnyPZDwoqKGFXtsbZSFy].ParticleB].Position
                                         + SimulationMeshParticlePositions[SimulationMeshVolumeConstraints[EWyilIWVPPjarnyPZDwoqKGFXtsbZSFy].ParticleC].Position
                                         + SimulationMeshParticlePositions[SimulationMeshVolumeConstraints[EWyilIWVPPjarnyPZDwoqKGFXtsbZSFy].ParticleD].Position) / 4.0f;
                        if (math.distancesq(moUhaLNVSdcgfovZpovriFxQACBXnVzQ, VgumBesvdajXuEGsjWNikYDJbNhfdiJZ) < LfZwdbQkinnwGTQmfadnnjAHANpKQilL)
                        {
                            SITiPlCcQXOOMZRjiFTqpzlctfyNMmnI = EWyilIWVPPjarnyPZDwoqKGFXtsbZSFy;
                        }
                    }

                    float3 BdoRBWaVLxnvQKLqQUqAYilLQHtHyCpk = SimulationMeshParticlePositions[SimulationMeshVolumeConstraints[SITiPlCcQXOOMZRjiFTqpzlctfyNMmnI].ParticleA].Position;
                    float3 TGaISbIAXvoKzkHUoEtoGrsCfoGIdBUv = SimulationMeshParticlePositions[SimulationMeshVolumeConstraints[SITiPlCcQXOOMZRjiFTqpzlctfyNMmnI].ParticleB].Position;
                    float3 DZHveGKflBjUrxCJWJpMhcbWELZkLWon = SimulationMeshParticlePositions[SimulationMeshVolumeConstraints[SITiPlCcQXOOMZRjiFTqpzlctfyNMmnI].ParticleC].Position;
                    float3 DYpEFwWygYRPvTfOfzzEjgXJEJAMoDBL = SimulationMeshParticlePositions[SimulationMeshVolumeConstraints[SITiPlCcQXOOMZRjiFTqpzlctfyNMmnI].ParticleD].Position;
                    if (Math.TetrahedronBarycentricCoordinates(moUhaLNVSdcgfovZpovriFxQACBXnVzQ, BdoRBWaVLxnvQKLqQUqAYilLQHtHyCpk, TGaISbIAXvoKzkHUoEtoGrsCfoGIdBUv, DZHveGKflBjUrxCJWJpMhcbWELZkLWon, DYpEFwWygYRPvTfOfzzEjgXJEJAMoDBL, out float4 barCoords))
                    {
                        
                        VertexBindData[vertexIndex] = new ubcLnfKJugQVRuDYhwkkzbIqxUOzvLVD
                        {
                            VolumeConstraintIndex = SITiPlCcQXOOMZRjiFTqpzlctfyNMmnI,
                            BarycentricCoordinates = barCoords,
                        };
                    }
                    else
                    {
                        
                        VertexBindData[vertexIndex] = new ubcLnfKJugQVRuDYhwkkzbIqxUOzvLVD
                        {
                            VolumeConstraintIndex = 0,
                            BarycentricCoordinates = new float4(1.0f, 0.0f, 0.0f, 0.0f),
                        };
                    }
                }
            }
        }

        [BurstCompile]
        private struct xbYtOrwIiguZXtysjryFiXgyhPbDGXmH : IJobParallelFor
        {
            [ReadOnly]
            public float4x4 WorldToLocalMatrix;

            [ReadOnly]
            public NativeSlice<float3> ParticlesPosition;
            [ReadOnly]
            public NativeSlice<int> TetrahedraA;
            [ReadOnly]
            public NativeSlice<int> TetrahedraB;
            [ReadOnly]
            public NativeSlice<int> TetrahedraC;
            [ReadOnly]
            public NativeSlice<int> TetrahedraD;

            [ReadOnly]
            public NativeArray<ubcLnfKJugQVRuDYhwkkzbIqxUOzvLVD> VertexBindData;
            [WriteOnly]
            public NativeArray<float3> Vertices;

            public void Execute(int vertexIndex)
            {
                float3 sXIjljrkfSXzmKmKwZmqSEBEmvLVVKlN = ParticlesPosition[TetrahedraA[VertexBindData[vertexIndex].VolumeConstraintIndex]];
                float3 fVdvgdBCAlkOiwIgIVKMZwjzXXEPgenT = ParticlesPosition[TetrahedraB[VertexBindData[vertexIndex].VolumeConstraintIndex]];
                float3 RepfYtKavjQJZGBGMXvjOAUmryDlstgV = ParticlesPosition[TetrahedraC[VertexBindData[vertexIndex].VolumeConstraintIndex]];
                float3 RRXkfXmsGSYXiWoMoRhhGqLkbtmrAhFS = ParticlesPosition[TetrahedraD[VertexBindData[vertexIndex].VolumeConstraintIndex]];

                Vertices[vertexIndex] = math.mul(WorldToLocalMatrix, new float4(Math.TetrahedronBarycentricInterpolation(sXIjljrkfSXzmKmKwZmqSEBEmvLVVKlN, fVdvgdBCAlkOiwIgIVKMZwjzXXEPgenT, RepfYtKavjQJZGBGMXvjOAUmryDlstgV, RRXkfXmsGSYXiWoMoRhhGqLkbtmrAhFS, VertexBindData[vertexIndex].BarycentricCoordinates), 1.0f)).xyz;
            }
        }

        [BurstCompile]
        private struct ubcLnfKJugQVRuDYhwkkzbIqxUOzvLVD
        {
            public int VolumeConstraintIndex;

            public float4 BarycentricCoordinates;
        }

        [BurstCompile]
        private struct XYcgvuDJwwGETnLAENbDWNUjvDNYZmDy : IJobParallelFor
        {
            [ReadOnly]
            public float4x4 InitialWorldToLocalMatrix;

            [ReadOnly]
            public float4x4 LocalToWorldMatrix;

            [ReadOnly]
            public NativeSlice<float3> ParticlesInitialPosition;

            [ReadOnly]
            public NativeSlice<float> ParticlesInverseMass;

            [WriteOnly]
            public NativeSlice<float3> ParticlesPosition;

            public void Execute(int particleIndex)
            {
                if (ParticlesInverseMass[particleIndex] > math.EPSILON)
                {
                    return;
                }

                float3 naiYzDxBalalobCbDcHDrLOlezWHmOlv = ParticlesInitialPosition[particleIndex];

                naiYzDxBalalobCbDcHDrLOlezWHmOlv = math.mul(InitialWorldToLocalMatrix, new float4(naiYzDxBalalobCbDcHDrLOlezWHmOlv, 1.0f)).xyz;
                naiYzDxBalalobCbDcHDrLOlezWHmOlv = math.mul(LocalToWorldMatrix, new float4(naiYzDxBalalobCbDcHDrLOlezWHmOlv, 1.0f)).xyz;

                ParticlesPosition[particleIndex] = naiYzDxBalalobCbDcHDrLOlezWHmOlv;
            }
        }
    }
}
