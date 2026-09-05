namespace MAGES.MeshDeformations
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Applies the corrections job with the average method.
    /// </summary>
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    public struct ApplyCorrectionsJob : IJobParallelFor
    {
        /// <summary>
        /// The relaxation factor to apply to the corrections.
        /// </summary>
        [ReadOnly]
        public float RelaxationFactor;

        /// <summary>
        /// The target array to apply the corrections.
        /// </summary>
        public NativeArray<float3> ApplyTarget;

        /// <summary>
        /// The array of corrections to apply.
        /// </summary>
        [ReadOnly]
        public NativeArray<float3> ParticlesCorrections;

        /// <summary>
        /// The count with wich each correction will be divided in order to apply the average of it.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> ParticlesCorrectionsCount;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int particleIndex)
        {
            if (Unity.Burst.CompilerServices.Hint.Unlikely(ParticlesCorrectionsCount[particleIndex] == 0))
            {
                return;
            }

            ApplyTarget[particleIndex] += RelaxationFactor * (ParticlesCorrections[particleIndex] / ParticlesCorrectionsCount[particleIndex]);
        }
    }
}
