namespace MAGES.MeshDeformations.Utilities
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Jobs;

    /// <summary>
    /// Initializes a counter to 0.
    /// </summary>
    [BurstCompile]
    public struct InitializeCounterJob : IJob
    {
        /// <summary>
        /// The counter to initialize.
        /// </summary>
        [NativeDisableContainerSafetyRestriction]
        [NativeDisableUnsafePtrRestriction]
        public NativeReference<int> Counter;

        /// <inheritdoc cref="IJob.Execute"/>
        public void Execute()
        {
            Counter.Value = 0;
        }
    }
}