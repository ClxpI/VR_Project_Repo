namespace MAGES.MeshDeformations
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;

    /// <summary>
    /// Reset the flag that indicates if the simulation mesh has any particles with infinite mass.
    /// </summary>
    [BurstCompile]
    internal struct ResetContainsParticlesWithInfiniteMassFlagJob : IJobParallelFor
    {
        /// <summary>
        /// The flags array.
        /// </summary>
        [WriteOnly]
        public NativeArray<bool> ContainsParticlesWithInfiniteMass;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int index)
        {
            ContainsParticlesWithInfiniteMass[index] = false;
        }
    }
}
