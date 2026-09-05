namespace MAGES.MeshDeformations
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Updates velocities.<br/>
    /// Applies internal forces.<br/>
    /// Updates positions or applies sleeping.
    /// </summary>
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    public struct PostSolveJob : IJobParallelFor
    {
        /// <summary>
        /// The simulation time step.
        /// </summary>
        [ReadOnly]
        public float TimeStep;

        /// <summary>
        /// The velocity below which a particle is considered to be sleeping.
        /// </summary>
        [ReadOnly]
        public float SleepVelocityThresholdSquared;

        /// <summary>
        /// The predicted position of each particle.
        /// </summary>
        public NativeArray<float3> ParticlesPredictedPosition;

        /// <summary>
        /// The current position of each particle.
        /// </summary>
        public NativeArray<float3> ParticlesPosition;

        /// <summary>
        /// The velocity of each particle.
        /// </summary>
        [WriteOnly]
        public NativeArray<float3> ParticlesVelocity;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int particleIndex)
        {
            var UhHefqFXFYxYaVulPRBuxZOLEfuWAdgT = (ParticlesPredictedPosition[particleIndex] - ParticlesPosition[particleIndex]) / TimeStep;

            ParticlesVelocity[particleIndex] = UhHefqFXFYxYaVulPRBuxZOLEfuWAdgT;

            

            
            if (math.lengthsq(UhHefqFXFYxYaVulPRBuxZOLEfuWAdgT) < SleepVelocityThresholdSquared)
            {
                ParticlesPredictedPosition[particleIndex] = ParticlesPosition[particleIndex];
            }

            
            ParticlesPosition[particleIndex] = ParticlesPredictedPosition[particleIndex];
        }
    }
}
