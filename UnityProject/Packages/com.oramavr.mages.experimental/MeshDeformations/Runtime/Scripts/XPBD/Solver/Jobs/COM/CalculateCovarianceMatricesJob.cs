namespace MAGES.MeshDeformations
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Calculates the covariance matrix for each simulation mesh.
    /// </summary>
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    internal struct CalculateCovarianceMatricesJob : IJob
    {
        /// <summary>
        /// The enabled state of the particles.
        /// </summary>
        [ReadOnly]
        public NativeArray<bool> ParticlesEnabled;

        /// <summary>
        /// The position of the particles.
        /// </summary>
        [ReadOnly]
        public NativeArray<float3> ParticlesPosition;

        /// <summary>
        /// The local position of each particle in the rest configuration.
        /// </summary>
        [ReadOnly]
        public NativeArray<float3> ParticlesLocalRestPosition;

        /// <summary>
        /// The index of the simulation mesh descriptor for each particle.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> ParticlesSimulationMeshDescriptorIndex;

        /// <summary>
        /// The center of mass of each simulation mesh.
        /// </summary>
        [ReadOnly]
        public NativeArray<float3> SimulationMeshCoM;

        /// <summary>
        /// The covariance matrix for each simulation mesh.
        /// </summary>
        public NativeArray<float3x3> SimulationMeshCovarianceMatrix;

        /// <inheritdoc cref="IJob.Execute()"/>
        public void Execute()
        {
            for (int oNlTiemUSofitClvEfKFqlnfoiwASrPr = 0; oNlTiemUSofitClvEfKFqlnfoiwASrPr < ParticlesEnabled.Length; oNlTiemUSofitClvEfKFqlnfoiwASrPr++)
            {
                if (Unity.Burst.CompilerServices.Hint.Unlikely(!ParticlesEnabled[oNlTiemUSofitClvEfKFqlnfoiwASrPr]))
                {
                    continue;
                }

                int edUzdGWpmKZRsnJlOElmtlRVvKaPzWEX = ParticlesSimulationMeshDescriptorIndex[oNlTiemUSofitClvEfKFqlnfoiwASrPr];

                float3 zQcaKGvSfAFNjYUdRIxNkxbqYVtHwQtE = SimulationMeshCoM[edUzdGWpmKZRsnJlOElmtlRVvKaPzWEX];

                SimulationMeshCovarianceMatrix[edUzdGWpmKZRsnJlOElmtlRVvKaPzWEX] += Utilities.Math.OuterProduct(ParticlesPosition[oNlTiemUSofitClvEfKFqlnfoiwASrPr] - zQcaKGvSfAFNjYUdRIxNkxbqYVtHwQtE, ParticlesLocalRestPosition[oNlTiemUSofitClvEfKFqlnfoiwASrPr]);
            }
        }
    }
}