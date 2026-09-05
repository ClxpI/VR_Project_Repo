namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;
    using UnityEngine;

    /// <summary>
    /// An actor for simulating Rigidbody physics.
    /// </summary>
    [AddComponentMenu("MAGES/Mesh Deformations/Actors/Rigidbody Actor")]
    public class RigidbodyActor : BasePhysicsActor
    {
        
        private float3 frUAwTgLcPwEhlpJISlxHpXOvgBuBpHQ;
        private quaternion UPflnmxvrJwhFKOhtvtyfjTfWMpDlTNI;

        private float3 ZXqwkibeFsUKYYLmEpOgXBZjLuIMBNkS;
        private quaternion PAqmxjHaJHgGhUjYsfRowBDiuinDZSuq;

        private float4x4 CCfFHKzYyCSGckoWicbnjfKlgZXtLEMr;

        [SerializeField]
        private bool canCut = false;
        [SerializeField]
        private uint layer = 1;
        [SerializeField]
        private bool isKinematic = false;

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
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.PhysicsWorld.UpdatePhase(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj, KQtbGiTeLLAARqSMYAQykDuCGIznPEeF());
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
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.PhysicsWorld.UpdatePhase(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj, KQtbGiTeLLAARqSMYAQykDuCGIznPEeF());
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this rigidbody is kinematic as a whole (ie all the particles of it are kinematic).
        /// </summary>
        public bool IsKinematic
        {
            get => isKinematic;
            set => isKinematic = value;
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

            ZXqwkibeFsUKYYLmEpOgXBZjLuIMBNkS = transform.position;
            PAqmxjHaJHgGhUjYsfRowBDiuinDZSuq = transform.rotation;
            CCfFHKzYyCSGckoWicbnjfKlgZXtLEMr = transform.worldToLocalMatrix;
            PhysicsWorld.AddSimulationMesh(SimulationMesh, transform.localToWorldMatrix, KQtbGiTeLLAARqSMYAQykDuCGIznPEeF());

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

            if (IsKinematic)
            {
                wEJWjuHRZpBBzbVgXyarlKPHViEKWVRo().Complete();
            }
            else
            {
                
                float3 zMMsRgFfSPcaRLDpjVmzzkSwJnMHpMck = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.GetCenterOfMassPosition();
                quaternion ClhjetVtAmoxqZeRuAPybOisyJLkkuJW = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.GetCenterOfMassRotation();

                float3 cdpeORzACqZwAjMjTdlFnjxGIkoTcCcB = zMMsRgFfSPcaRLDpjVmzzkSwJnMHpMck + math.mul(ClhjetVtAmoxqZeRuAPybOisyJLkkuJW, frUAwTgLcPwEhlpJISlxHpXOvgBuBpHQ);
                quaternion lsjxaJAhtJCWWsaCpEhXmmBglItsOFul = math.mul(ClhjetVtAmoxqZeRuAPybOisyJLkkuJW, UPflnmxvrJwhFKOhtvtyfjTfWMpDlTNI);

                if (!cdpeORzACqZwAjMjTdlFnjxGIkoTcCcB.IsNaN())
                {
                    transform.position = cdpeORzACqZwAjMjTdlFnjxGIkoTcCcB;
                }

                transform.rotation = lsjxaJAhtJCWWsaCpEhXmmBglItsOFul;
            }
        }

        /// <inheritdoc/>
        protected override void OnAddedToPhysicsWorld()
        {
            float3 LOgKGGtEfhsCkVsaXPRssSTsKeCLJTJZ = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.GetCenterOfMassPosition();
            quaternion dPtbiFjEvOCppemBtmbTdUzcgrhNRKVS = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.GetCenterOfMassRotation();

            frUAwTgLcPwEhlpJISlxHpXOvgBuBpHQ = ZXqwkibeFsUKYYLmEpOgXBZjLuIMBNkS - LOgKGGtEfhsCkVsaXPRssSTsKeCLJTJZ;
            UPflnmxvrJwhFKOhtvtyfjTfWMpDlTNI = math.mul(math.inverse(dPtbiFjEvOCppemBtmbTdUzcgrhNRKVS), PAqmxjHaJHgGhUjYsfRowBDiuinDZSuq);

            base.OnAddedToPhysicsWorld();
        }

        private uint KQtbGiTeLLAARqSMYAQykDuCGIznPEeF()
        {
            var EXKGfZQpQKeBlDbUSCTjNqyGKJCoLmEl = PhaseOptions.None;
            if (CanCut)
            {
                EXKGfZQpQKeBlDbUSCTjNqyGKJCoLmEl |= PhaseOptions.CanCut;
            }

            return PhysicsWorld.CreatePhase(Layer, EXKGfZQpQKeBlDbUSCTjNqyGKJCoLmEl);
        }

        private JobHandle wEJWjuHRZpBBzbVgXyarlKPHViEKWVRo(JobHandle dependsOn = default)
        {
            var qITErkltyyhStPhUDAbvpAcZrshIqhZC = new QRSilJDWGtjdeTEqHhHGIivKizQVAJDX
            {
                InitialWorldToLocalMatrix = CCfFHKzYyCSGckoWicbnjfKlgZXtLEMr,
                LocalToWorldMatrix = transform.localToWorldMatrix,

                ParticlesInitialPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition,
                ParticlesPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position,
                ParticlesInverseMass = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InverseMass,
            };

            return qITErkltyyhStPhUDAbvpAcZrshIqhZC.Schedule(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Count, 64, dependsOn);
        }

        [BurstCompile]
        private struct QRSilJDWGtjdeTEqHhHGIivKizQVAJDX : IJobParallelFor
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

                float3 iNYMXkzlMIBbNilSAwXJbznXDMcWqkJz = ParticlesInitialPosition[particleIndex];

                iNYMXkzlMIBbNilSAwXJbznXDMcWqkJz = math.mul(InitialWorldToLocalMatrix, new float4(iNYMXkzlMIBbNilSAwXJbznXDMcWqkJz, 1.0f)).xyz;
                iNYMXkzlMIBbNilSAwXJbznXDMcWqkJz = math.mul(LocalToWorldMatrix, new float4(iNYMXkzlMIBbNilSAwXJbznXDMcWqkJz, 1.0f)).xyz;

                ParticlesPosition[particleIndex] = iNYMXkzlMIBbNilSAwXJbznXDMcWqkJz;
            }
        }
    }
}
