namespace MAGES.MeshDeformations
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;

    /// <summary>
    /// Updates the particles phase.
    /// </summary>
    [BurstCompile]
    internal struct UpdateParticlesPhaseJob : IJobParallelFor
    {
        /// <summary>
        /// The new particle phase.
        /// </summary>
        [ReadOnly]
        public uint NewPhase;

        /// <summary>
        /// The start offset in the particles phase array.
        /// </summary>
        /// <remarks>Used to update particles only in a specific range.</remarks>
        [ReadOnly]
        public int StartOffset;

        /// <summary>
        /// The current particle phases.
        /// </summary>
        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<uint> ParticlesPhase;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int particleIndex)
        {
            ParticlesPhase[particleIndex + StartOffset] = NewPhase;
        }
    }
}
