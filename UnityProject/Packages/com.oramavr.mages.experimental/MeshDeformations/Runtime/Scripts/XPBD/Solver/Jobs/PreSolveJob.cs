namespace MAGES.MeshDeformations
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Applies External Forces.(Gravity, etc)<br/>
    /// Applies Mass Scaling.
    /// Predicts Positions.
    /// </summary>
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    internal struct PreSolveJob : IJobParallelFor
    {
        /// <summary>
        /// The simulation time step.
        /// </summary>
        [ReadOnly]
        public float TimeStep;

        /// <summary>
        /// The floor height.
        /// </summary>
        [ReadOnly]
        public float FloorHeight;

        /// <summary>
        /// The particle radius.
        /// </summary>
        [ReadOnly]
        public float ParticleRadius;

        /// <summary>
        /// The external gravity force.
        /// </summary>
        [ReadOnly]
        public float3 Gravity;

        /// <summary>
        /// The maximum velocity squared for particles to prevent tunelling.
        /// </summary>
        [ReadOnly]
        public float MaxVelocitySquared; 

        /// <summary>
        /// The enabled state of the particles.
        /// </summary>
        [ReadOnly]
        public NativeArray<bool> ParticlesEnabled;

        /// <summary>
        /// The index of the simulation mesh descriptor for each particle.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> ParticlesSimulationMeshDescriptorIndex;

        /// <summary>
        /// The index of the physics material for each particle.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> ParticlesPhysicsMaterialIndex;

        /// <summary>
        /// The original inverse mass of the particles.
        /// </summary>
        [ReadOnly]
        public NativeArray<float> ParticlesOriginalInverseMass;

        /// <summary>
        /// The adjusted inverse mass of the particles. Adjusted based on the height of the particles to improve stacking stability.
        /// </summary>
        [WriteOnly]
        public NativeArray<float> ParticlesAdjustedInverseMass;

        /// <summary>
        /// Flag for each simulation mesh to indicate if it contains any particles with infinite mass.
        /// </summary>
        [WriteOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<bool> ContainsParticlesWithInfiniteMass;

        /// <summary>
        /// The position of the particles.
        /// </summary>
        public NativeArray<float3> ParticlesPosition;

        /// <summary>
        /// The predicted position of the particles.
        /// </summary>
        public NativeArray<float3> ParticlesPredictedPosition;

        /// <summary>
        /// The velocity of the particles.
        /// </summary>
        public NativeArray<float3> ParticlesVelocity;

        

        /// <summary>
        /// The simulation mesh descriptors.
        /// </summary>
        [ReadOnly]
        public NativeArray<SimulationMeshDescriptor> SimulationMeshDescriptors;

        /// <summary>
        /// The physics materials.
        /// </summary>
        [ReadOnly]
        public NativeArray<PhysicsMaterial> PhysicsMaterials;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int particleIndex)
        {
            if (Unity.Burst.CompilerServices.Hint.Unlikely(ParticlesOriginalInverseMass[particleIndex] == 0 || !ParticlesEnabled[particleIndex]))
            {
                ParticlesAdjustedInverseMass[particleIndex] = ParticlesOriginalInverseMass[particleIndex];
                ParticlesPredictedPosition[particleIndex] = ParticlesPosition[particleIndex];
                ParticlesVelocity[particleIndex] = float3.zero;

                
                if (ParticlesEnabled[particleIndex])
                {
                    ContainsParticlesWithInfiniteMass[ParticlesSimulationMeshDescriptorIndex[particleIndex]] = true;
                }

                return;
            }

            SimulationMeshDescriptor lSLQifPTWIKWAAuggrGwrDeWBuounmQI = SimulationMeshDescriptors[ParticlesSimulationMeshDescriptorIndex[particleIndex]];

            
            ParticlesVelocity[particleIndex] += Gravity * TimeStep;

            
            ParticlesVelocity[particleIndex] *= 1.0f - PhysicsMaterials[ParticlesPhysicsMaterialIndex[particleIndex] + lSLQifPTWIKWAAuggrGwrDeWBuounmQI.PhysicsMaterialStartIndex].ParticleDamping;

            
            var gzclOTmJAuYMLrYgPzcaPQoZaHKJspqK = math.lengthsq(ParticlesVelocity[particleIndex]);
            if (gzclOTmJAuYMLrYgPzcaPQoZaHKJspqK > MaxVelocitySquared)
            {
                ParticlesVelocity[particleIndex] *= math.sqrt(MaxVelocitySquared) / math.sqrt(gzclOTmJAuYMLrYgPzcaPQoZaHKJspqK);
            }

            ParticlesPredictedPosition[particleIndex] = ParticlesPosition[particleIndex] + (ParticlesVelocity[particleIndex] * TimeStep);

            if (ParticlesPredictedPosition[particleIndex].y < FloorHeight + ParticleRadius)
            {
                ParticlesPredictedPosition[particleIndex] = new float3(ParticlesPredictedPosition[particleIndex].x, FloorHeight + ParticleRadius, ParticlesPredictedPosition[particleIndex].z);
            }

            float WfsZZpPPXlrJcFAxXdXnWlZYnanWKZJS = ParticlesOriginalInverseMass[particleIndex];
            ParticlesAdjustedInverseMass[particleIndex] = WfsZZpPPXlrJcFAxXdXnWlZYnanWKZJS;
        }
    }
}
