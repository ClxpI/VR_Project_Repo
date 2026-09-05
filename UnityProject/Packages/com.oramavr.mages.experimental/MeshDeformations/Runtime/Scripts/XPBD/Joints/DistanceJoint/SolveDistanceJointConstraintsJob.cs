namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Solves distance constraints.
    /// </summary>
    [BurstCompile]
    internal struct SolveDistanceJointConstraintsJob : IJobParallelFor
    {
        /// <summary>
        /// The simulation time step squared.
        /// </summary>
        [ReadOnly]
        public float TimeStepSquared;

        

        /// <summary>
        /// The particles enabled state.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<bool> ParticlesEnabled;

        /// <summary>
        /// The particles predicted position.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<float3> ParticlesPredictedPosition;

        /// <summary>
        /// The particles inverse mass.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<float> ParticlesInverseMass;

        

        /// <summary>
        /// The constraints enabled state.
        /// </summary>
        public NativeArray<bool> ConstraintsEnabled;

        /// <summary>
        /// The constraints breakable state.
        /// </summary>
        [ReadOnly]
        public NativeArray<bool> ConstraintsBreakable;

        /// <summary>
        /// The constraints breakable state.
        /// </summary>
        [ReadOnly]
        public NativeArray<float> ConstraintsBreakForce;

        /// <summary>
        /// The constraints particle A.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> ConstraintsParticleA;

        /// <summary>
        /// The constraints particle B.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> ConstraintsParticleB;

        /// <summary>
        /// The distance constraints rest distance.
        /// </summary>
        [ReadOnly]
        public NativeArray<float> ConstraintsRestDistance;

        /// <summary>
        /// The compliance of each constraint.
        /// </summary>
        [ReadOnly]
        public NativeArray<float> ConstraintsCompliance;

        

        /// <summary>
        /// The particle corrections.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<float3> ParticleCorrections;

        /// <summary>
        /// The particle corrections count.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<int> ParticleCorrectionsCount;

        /// <summary>
        /// The particle locks.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<int> ParticleLocks;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int distanceConstraintIndex)
        {
            if (Unity.Burst.CompilerServices.Hint.Unlikely(!ConstraintsEnabled[distanceConstraintIndex]))
            {
                return;
            }

            int gLFdDHYTbGwxVVjBdkiWbgQOVNqvVlfl = ConstraintsParticleA[distanceConstraintIndex];
            int LWjQvwvLImZkhLpBxVLtJbDLnjTjRCHn = ConstraintsParticleB[distanceConstraintIndex];

            if (Unity.Burst.CompilerServices.Hint.Unlikely(gLFdDHYTbGwxVVjBdkiWbgQOVNqvVlfl < 0 || LWjQvwvLImZkhLpBxVLtJbDLnjTjRCHn < 0 || LWjQvwvLImZkhLpBxVLtJbDLnjTjRCHn > ParticlesEnabled.Length || gLFdDHYTbGwxVVjBdkiWbgQOVNqvVlfl > ParticlesEnabled.Length))
            {
                ConstraintsEnabled[distanceConstraintIndex] = false;
                return;
            }

            if (Unity.Burst.CompilerServices.Hint.Unlikely(!ParticlesEnabled[gLFdDHYTbGwxVVjBdkiWbgQOVNqvVlfl] || !ParticlesEnabled[LWjQvwvLImZkhLpBxVLtJbDLnjTjRCHn]))
            {
                ConstraintsEnabled[distanceConstraintIndex] = false;
                return;
            }

            var JGwOuHUjJaHhmrhoqPeLVSHktCijYeLn = ParticlesInverseMass[gLFdDHYTbGwxVVjBdkiWbgQOVNqvVlfl] + ParticlesInverseMass[LWjQvwvLImZkhLpBxVLtJbDLnjTjRCHn];

            if (Unity.Burst.CompilerServices.Hint.Unlikely(JGwOuHUjJaHhmrhoqPeLVSHktCijYeLn == 0))
            {
                return;
            }

            
            float3 NBJDyLdDkzbeBuXdRMqxCabKpcAbafqS = ParticlesPredictedPosition[gLFdDHYTbGwxVVjBdkiWbgQOVNqvVlfl] - ParticlesPredictedPosition[LWjQvwvLImZkhLpBxVLtJbDLnjTjRCHn];

            float ytYAjSTKWEMDiltQPAwuitwWiNUNnEAA = math.length(NBJDyLdDkzbeBuXdRMqxCabKpcAbafqS);

            
            if (Unity.Burst.CompilerServices.Hint.Unlikely(ConstraintsBreakable[distanceConstraintIndex]))
            {
                var kXlozASStqfACjgpZpzdrVUWWwFNUBne = ytYAjSTKWEMDiltQPAwuitwWiNUNnEAA - ConstraintsRestDistance[distanceConstraintIndex];

                
                if (Unity.Burst.CompilerServices.Hint.Unlikely(math.lengthsq(kXlozASStqfACjgpZpzdrVUWWwFNUBne / TimeStepSquared) > ConstraintsBreakForce[distanceConstraintIndex]))
                {
                    ConstraintsEnabled[distanceConstraintIndex] = false;
                }
            }

            if (Unity.Burst.CompilerServices.Hint.Unlikely(ytYAjSTKWEMDiltQPAwuitwWiNUNnEAA == 0))
            {
                return;
            }

            NBJDyLdDkzbeBuXdRMqxCabKpcAbafqS = (1.0f / ytYAjSTKWEMDiltQPAwuitwWiNUNnEAA) * NBJDyLdDkzbeBuXdRMqxCabKpcAbafqS;
            float dPEjTOLBdtcoDAkPZVOHArhuZSLMjkfT = ytYAjSTKWEMDiltQPAwuitwWiNUNnEAA - ConstraintsRestDistance[distanceConstraintIndex];

            float mDtTymfTYKcaTDFhUfCHavpHrrQOXkXF = -dPEjTOLBdtcoDAkPZVOHArhuZSLMjkfT / (JGwOuHUjJaHhmrhoqPeLVSHktCijYeLn + (ConstraintsCompliance[distanceConstraintIndex] / TimeStepSquared));

            
            
            
            ParticleCorrections.InterlockedAddFloat3(ParticleLocks, gLFdDHYTbGwxVVjBdkiWbgQOVNqvVlfl, mDtTymfTYKcaTDFhUfCHavpHrrQOXkXF * ParticlesInverseMass[gLFdDHYTbGwxVVjBdkiWbgQOVNqvVlfl] * NBJDyLdDkzbeBuXdRMqxCabKpcAbafqS);
            ParticleCorrectionsCount.InterlockedAdd(gLFdDHYTbGwxVVjBdkiWbgQOVNqvVlfl, 1);
            ParticleCorrections.InterlockedAddFloat3(ParticleLocks, LWjQvwvLImZkhLpBxVLtJbDLnjTjRCHn, -mDtTymfTYKcaTDFhUfCHavpHrrQOXkXF * ParticlesInverseMass[LWjQvwvLImZkhLpBxVLtJbDLnjTjRCHn] * NBJDyLdDkzbeBuXdRMqxCabKpcAbafqS);
            ParticleCorrectionsCount.InterlockedAdd(LWjQvwvLImZkhLpBxVLtJbDLnjTjRCHn, 1);
        }
    }
}
