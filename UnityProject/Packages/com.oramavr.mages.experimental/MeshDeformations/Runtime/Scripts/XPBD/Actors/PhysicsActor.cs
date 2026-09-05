namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;
    using UnityEngine;

    /// <summary>
    /// A simple physics simulation, without rendering.
    /// </summary>
    [AddComponentMenu("MAGES/Mesh Deformations/Actors/Physics Actor")]
    public class PhysicsActor : BasePhysicsActor
    {
        private float4x4 clDuZousyKqtfgLURixPbULxVRBYBTHo;

        [SerializeField]
        private uint layer = 1;
        [SerializeField]
        private bool canCut = false;
        [SerializeField]
        private bool selfCollision = false;

        /// <summary>
        /// Gets or sets a value indicating whether this actor can be used as a cutting tool.
        /// </summary>
        public bool CanCut
        {
            get
            {
                return canCut;
            }

            set
            {
                if (canCut == value)
                {
                    return;
                }

                canCut = value;
                if (MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj != null && MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld)
                {
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.PhysicsWorld.UpdatePhase(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj, zWyyNqKoZGrKxIIfeWVKeIZtJporObyS());
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this actor can be used as a cutting tool.
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
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.PhysicsWorld.UpdatePhase(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj, zWyyNqKoZGrKxIIfeWVKeIZtJporObyS());
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
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.PhysicsWorld.UpdatePhase(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj, zWyyNqKoZGrKxIIfeWVKeIZtJporObyS());
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

            clDuZousyKqtfgLURixPbULxVRBYBTHo = transform.worldToLocalMatrix;
            PhysicsWorld.AddSimulationMesh(SimulationMesh, transform.localToWorldMatrix, zWyyNqKoZGrKxIIfeWVKeIZtJporObyS());

            base.AddToPhysicsWorld();
        }

        /// <inheritdoc/>
        public override void RemoveFromPhysicsWorld()
        {
            base.RemoveFromPhysicsWorld();

            
            PhysicsWorld?.RemoveSimulationMesh(SimulationMesh);

            
            Destroy(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj);
            MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj = null;
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
                kTchGthrNzvNvupeEcQdfJQNyXohuIVw().Complete();
                transform.hasChanged = false;
            }
        }

        private uint zWyyNqKoZGrKxIIfeWVKeIZtJporObyS()
        {
            var yotbfYqiLzExEXqeiajcUSNIsJiHaKlI = PhaseOptions.None;
            if (CanCut)
            {
                yotbfYqiLzExEXqeiajcUSNIsJiHaKlI |= PhaseOptions.CanCut;
            }

            if (SelfCollision)
            {
                yotbfYqiLzExEXqeiajcUSNIsJiHaKlI |= PhaseOptions.SelfCollisionEnabled;
            }

            return PhysicsWorld.CreatePhase(Layer, yotbfYqiLzExEXqeiajcUSNIsJiHaKlI);
        }

        private void qGOedAssZEgQYUzIqMKXvmXBBOtzYSFs()
        {
            if (!Application.isPlaying && SharedSimulationMesh != null)
            {
                float3 ldFkvkhbfFwfxmkgUSSmjftXkgJadcmL, nRKiNJZVKfqxUCoyrTjcMDSJxElTnNUr, SgZgCLNnywfrfShoQGCGEUgtvzVJnaoT, RMoODsOkOXsQiIbDcKgobTCBMfpvOObe;

                var MILIcfKAzBsQLlmJMtEwVaOpUTGYqZDX = SharedSimulationMesh.Particles;

                float4x4 rKLStldseIIoeqOHzdMLHjlAHMlgTjei = transform.localToWorldMatrix;

                foreach (VolumeConstraint vc in SharedSimulationMesh.VolumeConstraints)
                {
                    Gizmos.color = Color.green;

                    ldFkvkhbfFwfxmkgUSSmjftXkgJadcmL = math.mul(rKLStldseIIoeqOHzdMLHjlAHMlgTjei, new float4(MILIcfKAzBsQLlmJMtEwVaOpUTGYqZDX[vc.ParticleA].Position, 1.0f)).xyz;
                    nRKiNJZVKfqxUCoyrTjcMDSJxElTnNUr = math.mul(rKLStldseIIoeqOHzdMLHjlAHMlgTjei, new float4(MILIcfKAzBsQLlmJMtEwVaOpUTGYqZDX[vc.ParticleB].Position, 1.0f)).xyz;
                    SgZgCLNnywfrfShoQGCGEUgtvzVJnaoT = math.mul(rKLStldseIIoeqOHzdMLHjlAHMlgTjei, new float4(MILIcfKAzBsQLlmJMtEwVaOpUTGYqZDX[vc.ParticleC].Position, 1.0f)).xyz;
                    RMoODsOkOXsQiIbDcKgobTCBMfpvOObe = math.mul(rKLStldseIIoeqOHzdMLHjlAHMlgTjei, new float4(MILIcfKAzBsQLlmJMtEwVaOpUTGYqZDX[vc.ParticleD].Position, 1.0f)).xyz;

                    MAGES.Utilities.GizmoUtilities.DrawTetrahedron(ldFkvkhbfFwfxmkgUSSmjftXkgJadcmL, nRKiNJZVKfqxUCoyrTjcMDSJxElTnNUr, SgZgCLNnywfrfShoQGCGEUgtvzVJnaoT, RMoODsOkOXsQiIbDcKgobTCBMfpvOObe);
                }
            }
        }

        private JobHandle kTchGthrNzvNvupeEcQdfJQNyXohuIVw(JobHandle dependsOn = default)
        {
            var TIISjjSLSfrWNvoeWqvdQuYJJHihjfbY = new YiQDBlrmwnpjpAUAbXtRfrYYMRFfNGyy
            {
                InitialWorldToLocalMatrix = clDuZousyKqtfgLURixPbULxVRBYBTHo,
                LocalToWorldMatrix = transform.localToWorldMatrix,

                ParticlesInitialPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition,
                ParticlesPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position,
                ParticlesInverseMass = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InverseMass,
            };

            return TIISjjSLSfrWNvoeWqvdQuYJJHihjfbY.Schedule(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Count, 64, dependsOn);
        }

        [BurstCompile]
        private struct YiQDBlrmwnpjpAUAbXtRfrYYMRFfNGyy : IJobParallelFor
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

                float3 JlaZZpXwtddFVzLExTSKGchTNPPYPqKA = ParticlesInitialPosition[particleIndex];

                JlaZZpXwtddFVzLExTSKGchTNPPYPqKA = math.mul(InitialWorldToLocalMatrix, new float4(JlaZZpXwtddFVzLExTSKGchTNPPYPqKA, 1.0f)).xyz;
                JlaZZpXwtddFVzLExTSKGchTNPPYPqKA = math.mul(LocalToWorldMatrix, new float4(JlaZZpXwtddFVzLExTSKGchTNPPYPqKA, 1.0f)).xyz;

                ParticlesPosition[particleIndex] = JlaZZpXwtddFVzLExTSKGchTNPPYPqKA;
            }
        }
    }
}
