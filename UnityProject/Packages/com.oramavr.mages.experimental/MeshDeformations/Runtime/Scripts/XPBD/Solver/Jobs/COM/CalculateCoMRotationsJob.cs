namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Calculate the rotation of the center of mass for each simulation mesh.
    /// </summary>
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    internal struct CalculateCoMRotationsJob : IJobParallelFor
    {
        /// <summary>
        /// The covariance matrix for each simulation mesh.
        /// </summary>
        [ReadOnly]
        public NativeArray<float3x3> SimulationMeshCovarianceMatrix;

        /// <summary>
        /// The rotation of the center of mass for each simulation mesh.
        /// </summary>
        public NativeArray<quaternion> SimulationMeshCoMRotation;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int descIndex)
        {
            
            var nTOJtqOQxUthqDSPaRizjMauEjOfyMpT = SimulationMeshCovarianceMatrix[descIndex];

            
            SimulationMeshCoMRotation[descIndex] = SVD.SVDRotation(nTOJtqOQxUthqDSPaRizjMauEjOfyMpT);
        }
    }
}