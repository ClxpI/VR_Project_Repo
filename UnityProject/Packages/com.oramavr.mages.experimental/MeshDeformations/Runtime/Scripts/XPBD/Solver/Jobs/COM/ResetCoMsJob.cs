namespace MAGES.MeshDeformations
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Reset the default values of the CoM, Covariance Matrix and Total Mass of the simulation mesh, before being recalculated.
    /// </summary>
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    internal struct ResetCoMsJob : IJobParallelFor
    {
        /// <summary>
        /// The total mass of each simulation mesh.
        /// </summary>
        [WriteOnly]
        public NativeArray<float> SimulationMeshTotalMass;

        /// <summary>
        /// The center of mass of each simulation mesh.
        /// </summary>
        [WriteOnly]
        public NativeArray<float3> SimulationMeshCoM;

        /// <summary>
        /// The covariance matrix of each simulation mesh.
        /// </summary>
        [WriteOnly]
        public NativeArray<float3x3> SimulationMeshCovarianceMatrix;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int descIndex)
        {
            SimulationMeshTotalMass[descIndex] = 0;
            SimulationMeshCoM[descIndex] = float3.zero;
            SimulationMeshCovarianceMatrix[descIndex] = float3x3.zero;
        }
    }
}