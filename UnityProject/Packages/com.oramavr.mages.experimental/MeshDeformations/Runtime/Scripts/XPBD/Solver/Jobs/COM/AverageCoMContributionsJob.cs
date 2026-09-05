namespace MAGES.MeshDeformations
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Averages the contributions to the Center Of Mass of each particle in the simulation mesh.
    /// </summary>
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    internal struct AverageCoMContributionsJob : IJobParallelFor
    {
        /// <summary>
        /// The total mass of each simulation mesh.
        /// </summary>
        [ReadOnly]
        public NativeArray<float> SimulationMeshTotalMass;

        /// <summary>
        /// The Center Of Mass of each simulation mesh.
        /// </summary>
        public NativeArray<float3> SimulationMeshCoM;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int descIndex)
        {
            SimulationMeshCoM[descIndex] /= SimulationMeshTotalMass[descIndex];
        }
    }
}