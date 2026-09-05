namespace MAGES.MeshDeformations
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Initializes the corrections buffer with default (0) value.
    /// </summary>
    [BurstCompile]
    public struct InitializeCorrectionsBufferJob : IJobParallelFor
    {
        /// <summary>
        /// The corrections buffer.
        /// </summary>
        [WriteOnly]
        public NativeArray<float3> ParticlesCorrections;

        /// <summary>
        /// The corrections count buffer.
        /// </summary>
        [WriteOnly]
        public NativeArray<int> ParticlesCorrectionsCount;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int particleIndex)
        {
            ParticlesCorrectionsCount[particleIndex] = 0;
            ParticlesCorrections[particleIndex] = float3.zero;
        }
    }
}
