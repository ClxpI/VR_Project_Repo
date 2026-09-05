namespace MAGES.MeshDeformations
{
    using System.Collections.Generic;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    /// <summary>
    /// A cloth simulation.
    /// </summary>
    [AddComponentMenu("MAGES/Mesh Deformations/Actors/Cloth Actor")]
    [RequireComponent(typeof(MeshFilter))]
    public class ClothActor : BasePhysicsActor
    {
        [SerializeField]
        private Mesh sourceMesh;

        private Mesh wjiKUwEoMMaRzMcjqeXKBLvbuutCYqlZ;

        [SerializeField]
        private float mass = 1.0f;

        [SerializeField]
        private float damping = 0.01f;

        [SerializeField]
        private bool selfCollision = true;

        [SerializeField]
        private uint layer = 1;

        private float4x4 qqkYeRbVULdUuCxLOHaDNbBGrMxpUSuV;

        private NativeArray<float3> YhEQTLdsLytJyiXXmGOynfrXQlMdFmsk;

        /// <summary>
        /// Gets or sets the source mesh used for creating the simulation mesh.
        /// </summary>
        public Mesh SourceMesh
        {
            get
            {
                return sourceMesh;
            }

            set
            {
                if (sourceMesh == value)
                {
                    return;
                }

                sourceMesh = value;
                FBcJzJlYHbSqzTVoGMKGrbxaCBNdYLJX();

                if (!Application.isPlaying)
                {
                    GetComponent<MeshFilter>().sharedMesh = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total cloth mass.
        /// </summary>
        public float Mass
        {
            get
            {
                return mass;
            }

            set
            {
                value = math.max(1e-07f, value);
                value = math.min(1e+09f, value);
                if (mass == value)
                {
                    return;
                }

                mass = value;
                FBcJzJlYHbSqzTVoGMKGrbxaCBNdYLJX();
            }
        }

        /// <summary>
        /// Gets or sets the damping applied to each particle. A value in the range [0, 1].
        /// </summary>
        public float Damping
        {
            get
            {
                return damping;
            }

            set
            {
                value = math.max(0.0f, value);
                value = math.min(1.0f, value);

                if (damping == value)
                {
                    return;
                }

                damping = value;
                FBcJzJlYHbSqzTVoGMKGrbxaCBNdYLJX();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether self collision is enabled.
        /// </summary>
        public bool SelfCollision
        {
            get
            {
                return selfCollision;
            }

            set
            {
                if (selfCollision == value)
                {
                    return;
                }

                selfCollision = value;
                if (MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj != null && MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld)
                {
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.PhysicsWorld.UpdatePhase(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj, sfIxrmVRZuGPKFyoYGSJAMSKQNtHomUX());
                }
            }
        }

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
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.PhysicsWorld.UpdatePhase(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj, sfIxrmVRZuGPKFyoYGSJAMSKQNtHomUX());
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
                UnityEngine.Debug.LogError("SimulationMeshSource is not assigned. Softbody actor will not be simulated.", this);
                return;
            }

            if (PhysicsWorld == null)
            {
                UnityEngine.Debug.LogError("PhysicsWorld is not assigned. Softbody actor will not be simulated.", this);
                return;
            }

            
            GetComponent<MeshFilter>().sharedMesh = SourceMesh;
            wjiKUwEoMMaRzMcjqeXKBLvbuutCYqlZ = GetComponent<MeshFilter>().mesh;
            wjiKUwEoMMaRzMcjqeXKBLvbuutCYqlZ.MarkDynamic();
            YhEQTLdsLytJyiXXmGOynfrXQlMdFmsk = new NativeArray<float3>(wjiKUwEoMMaRzMcjqeXKBLvbuutCYqlZ.vertices.Length, Allocator.Persistent);

            qqkYeRbVULdUuCxLOHaDNbBGrMxpUSuV = transform.worldToLocalMatrix;
            PhysicsWorld.AddSimulationMesh(SimulationMesh, transform.localToWorldMatrix, sfIxrmVRZuGPKFyoYGSJAMSKQNtHomUX());

            base.AddToPhysicsWorld();
        }

        /// <inheritdoc/>
        public override void RemoveFromPhysicsWorld()
        {
            base.RemoveFromPhysicsWorld();

            
            PhysicsWorld?.RemoveSimulationMesh(SimulationMesh);

            
            if (YhEQTLdsLytJyiXXmGOynfrXQlMdFmsk.IsCreated)
            {
                YhEQTLdsLytJyiXXmGOynfrXQlMdFmsk.Dispose();
            }

            
            Destroy(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj);
            MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj = null;

            
            Destroy(wjiKUwEoMMaRzMcjqeXKBLvbuutCYqlZ);
            wjiKUwEoMMaRzMcjqeXKBLvbuutCYqlZ = Instantiate(sourceMesh);
            wjiKUwEoMMaRzMcjqeXKBLvbuutCYqlZ.name = sourceMesh.name;
            GetComponent<MeshFilter>().mesh = wjiKUwEoMMaRzMcjqeXKBLvbuutCYqlZ;
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
                pRgObWdIsYDcDaptjMCDiRvGrkuCVQoS().Complete();
                transform.hasChanged = false;
            }

            yNHPlJjOWtqQOaBZDgFKNLQAqWQBdLnQ();
        }

        /// <inheritdoc/>
        protected override void Reset()
        {
            base.Reset();
            SourceMesh = GetComponent<MeshFilter>().sharedMesh;
        }

        private uint sfIxrmVRZuGPKFyoYGSJAMSKQNtHomUX()
        {
            var GmWTpmMXKvGBunrcmUfXpRHlYCbfKWUO = PhaseOptions.None;
            if (SelfCollision)
            {
                GmWTpmMXKvGBunrcmUfXpRHlYCbfKWUO |= PhaseOptions.SelfCollisionEnabled;
            }

            return PhysicsWorld.CreatePhase(Layer, GmWTpmMXKvGBunrcmUfXpRHlYCbfKWUO);
        }

        private void yNHPlJjOWtqQOaBZDgFKNLQAqWQBdLnQ()
        {
            HGBDSLtrAYouDXdFoOSnQNVMJYmRmJQh().Complete();

            wjiKUwEoMMaRzMcjqeXKBLvbuutCYqlZ.SetVertices(YhEQTLdsLytJyiXXmGOynfrXQlMdFmsk);
            wjiKUwEoMMaRzMcjqeXKBLvbuutCYqlZ.RecalculateBounds();
            wjiKUwEoMMaRzMcjqeXKBLvbuutCYqlZ.RecalculateNormals();
        }

        private void FBcJzJlYHbSqzTVoGMKGrbxaCBNdYLJX()
        {
            if (SourceMesh == null)
            {
                SharedSimulationMesh = null;
                return;
            }

            HashSet<(int, int)> GQxOOnUVWkKVOEtnBhzMaEEWHURqPPnO = new HashSet<(int, int)>();

            var LqEWcwuFnHNRsbBzQBicoQsArtZsrris = SourceMesh.triangles;

            
            for (int svTQeQsWIdScsvLpIAmiAoKhnvKIbLod = 0; svTQeQsWIdScsvLpIAmiAoKhnvKIbLod < SourceMesh.triangles.Length; svTQeQsWIdScsvLpIAmiAoKhnvKIbLod += 3)
            {
                for (int QgrEkOMCNEiIyTvnunyBRHJVDFlcPSQW = 0; QgrEkOMCNEiIyTvnunyBRHJVDFlcPSQW < 3; QgrEkOMCNEiIyTvnunyBRHJVDFlcPSQW++)
                {
                    for (int wpeOkPRrXyROQxkwcShNQEEtBQyupHGf = QgrEkOMCNEiIyTvnunyBRHJVDFlcPSQW + 1; wpeOkPRrXyROQxkwcShNQEEtBQyupHGf < 3; wpeOkPRrXyROQxkwcShNQEEtBQyupHGf++)
                    {
                        var MJMWtQhPnHfYQbMuEGEaTfCngyiRRJle = (LqEWcwuFnHNRsbBzQBicoQsArtZsrris[svTQeQsWIdScsvLpIAmiAoKhnvKIbLod + QgrEkOMCNEiIyTvnunyBRHJVDFlcPSQW], LqEWcwuFnHNRsbBzQBicoQsArtZsrris[svTQeQsWIdScsvLpIAmiAoKhnvKIbLod + wpeOkPRrXyROQxkwcShNQEEtBQyupHGf]);
                        if (MJMWtQhPnHfYQbMuEGEaTfCngyiRRJle.Item1 > MJMWtQhPnHfYQbMuEGEaTfCngyiRRJle.Item2)
                        {
                            MJMWtQhPnHfYQbMuEGEaTfCngyiRRJle = (MJMWtQhPnHfYQbMuEGEaTfCngyiRRJle.Item2, MJMWtQhPnHfYQbMuEGEaTfCngyiRRJle.Item1);
                        }

                        if (!GQxOOnUVWkKVOEtnBhzMaEEWHURqPPnO.Contains(MJMWtQhPnHfYQbMuEGEaTfCngyiRRJle))
                        {
                            GQxOOnUVWkKVOEtnBhzMaEEWHURqPPnO.Add(MJMWtQhPnHfYQbMuEGEaTfCngyiRRJle);
                        }
                    }
                }
            }

            
            PhysicsMaterial[] cyMKpXUeuapkSSqJNcXpRVPJfTCzsZDm = new PhysicsMaterial[]
            {
                new PhysicsMaterial
                {
                    ParticleDamping = Damping,
                    DistanceConstraintInverseStiffness = 0f,
                    VolumeConstraintInverseStiffness = 0f,
                    ShapeMatchingConstraintStiffness = 1f,
                },
            };
            List<Particle> dAvOZdSJijSsJhiLanwfPhaQTwlXeDhp = new List<Particle>();
            List<DistanceConstraint> qqrQeKsfPligpfDBvIVQzfEpLGtUotsH = new List<DistanceConstraint>();

            var rqbPJmAiRRZciTorJagoKkbXMriCieRd = SourceMesh.vertices;
            var VuLvSqLFaRXBLrCgVQaOCsCKKMwPssFF = Mass / SourceMesh.vertexCount;
            for (int GXjZEqOqOCvkFNKldXeXjIMOeedDeuwM = 0; GXjZEqOqOCvkFNKldXeXjIMOeedDeuwM < SourceMesh.vertexCount; GXjZEqOqOCvkFNKldXeXjIMOeedDeuwM++)
            {
                var FLurUMJnXjVzvNQknttNBAqwUJaktyBU = new Particle
                {
                    Enabled = true,
                    Breakable = false,
                    Kinematic = false,
                    Position = rqbPJmAiRRZciTorJagoKkbXMriCieRd[GXjZEqOqOCvkFNKldXeXjIMOeedDeuwM],
                    Velocity = 0f,
                    InverseMass = 1f / VuLvSqLFaRXBLrCgVQaOCsCKKMwPssFF,
                    PhysicsMaterialIndex = 0,
                };

                dAvOZdSJijSsJhiLanwfPhaQTwlXeDhp.Add(FLurUMJnXjVzvNQknttNBAqwUJaktyBU);
            }

            foreach (var edge in GQxOOnUVWkKVOEtnBhzMaEEWHURqPPnO)
            {
                var JLtCCnktxNMxzBDKYgrJvkurvAQsQzsk = edge.Item1;
                var hYXOxkeurZQPeofsJBmVykbvznaGaDYE = edge.Item2;

                var JHQJORLIdGmCaTahAGAeZVksFjrIuSUC = new DistanceConstraint(JLtCCnktxNMxzBDKYgrJvkurvAQsQzsk, hYXOxkeurZQPeofsJBmVykbvznaGaDYE);
                JHQJORLIdGmCaTahAGAeZVksFjrIuSUC.PhysicsMaterialIndex = 0;
                qqrQeKsfPligpfDBvIVQzfEpLGtUotsH.Add(JHQJORLIdGmCaTahAGAeZVksFjrIuSUC);
            }

            var JDorWNbdZKMEMCIxKvxmSyrzEXpTsCwj = SharedSimulationMesh;
            if (JDorWNbdZKMEMCIxKvxmSyrzEXpTsCwj == null)
            {
                JDorWNbdZKMEMCIxKvxmSyrzEXpTsCwj = ScriptableObject.CreateInstance<SimulationMesh>();
#if UNITY_EDITOR
                if (!EditorUtility.IsPersistent(JDorWNbdZKMEMCIxKvxmSyrzEXpTsCwj))
                {
                    AssetDatabase.CreateAsset(JDorWNbdZKMEMCIxKvxmSyrzEXpTsCwj, AssetDatabase.GenerateUniqueAssetPath($"Assets/{name}_SimulationMesh.asset"));
                }
#endif
            }
            else
            {
                SharedSimulationMesh = null;
            }

            JDorWNbdZKMEMCIxKvxmSyrzEXpTsCwj.Clear();
            JDorWNbdZKMEMCIxKvxmSyrzEXpTsCwj.PhysicsMaterials = cyMKpXUeuapkSSqJNcXpRVPJfTCzsZDm;
            JDorWNbdZKMEMCIxKvxmSyrzEXpTsCwj.Particles = dAvOZdSJijSsJhiLanwfPhaQTwlXeDhp.ToArray();
            JDorWNbdZKMEMCIxKvxmSyrzEXpTsCwj.DistanceConstraints = qqrQeKsfPligpfDBvIVQzfEpLGtUotsH.ToArray();

#if UNITY_EDITOR
            EditorUtility.SetDirty(JDorWNbdZKMEMCIxKvxmSyrzEXpTsCwj);
#endif

            SharedSimulationMesh = JDorWNbdZKMEMCIxKvxmSyrzEXpTsCwj;
        }

        private JobHandle pRgObWdIsYDcDaptjMCDiRvGrkuCVQoS(JobHandle dependsOn = default)
        {
            var hNtzlNceVxUVaYqMqRaWDeRltlNmKaYB = new nvezVdvmAFoOcaptkWxCalNjtUAxrZuG
            {
                InitialWorldToLocalMatrix = qqkYeRbVULdUuCxLOHaDNbBGrMxpUSuV,
                LocalToWorldMatrix = transform.localToWorldMatrix,

                ParticlesInitialPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition,
                ParticlesPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position,
                ParticlesInverseMass = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InverseMass,
            };

            return hNtzlNceVxUVaYqMqRaWDeRltlNmKaYB.Schedule(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Count, 64, dependsOn);
        }

        private JobHandle HGBDSLtrAYouDXdFoOSnQNVMJYmRmJQh(JobHandle dependsOn = default)
        {
            var aUsKzMzcxjrZEqjYWxCwxCAJGgpjowKQ = new cfFPKKjEOXztIwCUOdBuncDVsMzAVuAB
            {
                WorldToLocalMatrix = transform.worldToLocalMatrix,
                Vertices = YhEQTLdsLytJyiXXmGOynfrXQlMdFmsk,
                ParticlesPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position,
            };

            unsafe
            {
                return aUsKzMzcxjrZEqjYWxCwxCAJGgpjowKQ.Schedule(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Count, 64, dependsOn);
            }
        }

        [BurstCompile]
        private struct nvezVdvmAFoOcaptkWxCalNjtUAxrZuG : IJobParallelFor
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

                float3 embKgafnwRDLeRPOUJZrwXIeydaaPDUq = ParticlesInitialPosition[particleIndex];

                embKgafnwRDLeRPOUJZrwXIeydaaPDUq = math.mul(InitialWorldToLocalMatrix, new float4(embKgafnwRDLeRPOUJZrwXIeydaaPDUq, 1.0f)).xyz;
                embKgafnwRDLeRPOUJZrwXIeydaaPDUq = math.mul(LocalToWorldMatrix, new float4(embKgafnwRDLeRPOUJZrwXIeydaaPDUq, 1.0f)).xyz;

                ParticlesPosition[particleIndex] = embKgafnwRDLeRPOUJZrwXIeydaaPDUq;
            }
        }

        [BurstCompile]

        
        private struct cfFPKKjEOXztIwCUOdBuncDVsMzAVuAB : IJobParallelFor
        {
            [ReadOnly]
            public float4x4 WorldToLocalMatrix;

            [WriteOnly]
            public NativeArray<float3> Vertices;

            [ReadOnly]
            public NativeSlice<float3> ParticlesPosition;

            public void Execute(int vertexIndex)
            {
                Vertices[vertexIndex] = math.mul(WorldToLocalMatrix, new float4(ParticlesPosition[vertexIndex], 1.0f)).xyz;
            }
        }
    }
}
