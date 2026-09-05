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
    internal struct InitializeBufferJob<T> : IJobParallelFor
        where T : struct
    {
        /// <summary>
        /// The value to initialize the buffer with.
        /// </summary>
        [ReadOnly]
        public T Value;

        /// <summary>
        /// Source array.
        /// </summary>
        [WriteOnly]
        public NativeArray<T> Buffer;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int index)
        {
            Buffer[index] = Value;
        }
    }
}