namespace MAGES.MeshDeformations
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;

    /// <summary>
    /// Copy buffer job.
    /// </summary>
    /// <typeparam name="T">The type of the buffer data.</typeparam>
    [BurstCompile]
    internal struct CopyBufferJob<T> : IJobParallelFor
        where T : struct
    {
        /// <summary>
        /// Source array.
        /// </summary>
        [ReadOnly]
        public NativeArray<T> Source;

        /// <summary>
        /// Destination array.
        /// </summary>
        [WriteOnly]
        public NativeArray<T> Destination;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int index)
        {
            Destination[index] = Source[index];
        }
    }
}