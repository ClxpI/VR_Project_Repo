namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Calculates the contribution of each particle's position to the center of mass of the simulation mesh.
    /// </summary>
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    internal struct CalculateParticlesContributionToCoMsJob : IJobParallelFor
    {
        /// <summary>
        /// The enabled state of each particle.
        /// </summary>
        [ReadOnly]
        public NativeArray<bool> ParticlesEnabled;

        /// <summary>
        /// The predicted position of each particle.
        /// </summary>
        [ReadOnly]
        public NativeArray<float3> ParticlesPredictedPosition;

        /// <summary>
        /// The inverse mass of each particle.
        /// </summary>
        [ReadOnly]
        public NativeArray<float> ParticlesInverseMass;

        /// <summary>
        /// The index of the simulation mesh descriptor for each particle.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> ParticlesSimulationMeshDescriptorIndex;

        /// <summary>
        /// Flag indicating if the simulation mesh contains any particles with infinite mass.
        /// </summary>
        [ReadOnly]
        public NativeArray<bool> SimulationMeshContainsParticlesWithInfiniteMass;

        /// <summary>
        /// Locking array for Interlocked operations.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<int> SimulationMeshLocks;

        /// <summary>
        /// The total mass of each simulation mesh.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<float> SimulationMeshTotalMass;

        /// <summary>
        /// The center of mass of each simulation mesh.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<float3> SimulationMeshCoM;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int particleIndex)
        {
            if (Unity.Burst.CompilerServices.Hint.Unlikely(!ParticlesEnabled[particleIndex]))
            {
                return;
            }

            
            var WQraspXnDqqAeYquophxlNgtZlMBioFQ = ParticlesSimulationMeshDescriptorIndex[particleIndex];

            
            if (SimulationMeshContainsParticlesWithInfiniteMass[WQraspXnDqqAeYquophxlNgtZlMBioFQ])
            {
                
                if (ParticlesInverseMass[particleIndex] >= math.EPSILON)
                {
                    return;
                }

                
                SimulationMeshTotalMass.InterlockedAddFloat(SimulationMeshLocks, WQraspXnDqqAeYquophxlNgtZlMBioFQ, 1.0f);
                SimulationMeshCoM.InterlockedAddFloat3(SimulationMeshLocks, WQraspXnDqqAeYquophxlNgtZlMBioFQ, ParticlesPredictedPosition[particleIndex]);
            }
            else
            {
                float AklmWGpUuXrLcOGhoQXoslUGcOGApaVa = 1.0f / ParticlesInverseMass[particleIndex];

                SimulationMeshTotalMass.InterlockedAddFloat(SimulationMeshLocks, WQraspXnDqqAeYquophxlNgtZlMBioFQ, AklmWGpUuXrLcOGhoQXoslUGcOGApaVa);
                SimulationMeshCoM.InterlockedAddFloat3(SimulationMeshLocks, WQraspXnDqqAeYquophxlNgtZlMBioFQ, AklmWGpUuXrLcOGhoQXoslUGcOGApaVa * ParticlesPredictedPosition[particleIndex]);
            }
        }
    }
}