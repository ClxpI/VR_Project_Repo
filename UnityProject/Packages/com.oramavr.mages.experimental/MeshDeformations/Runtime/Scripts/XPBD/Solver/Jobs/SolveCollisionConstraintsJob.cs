namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Solves the collision constraints.
    /// </summary>
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    internal struct SolveCollisionConstraintsJob : IJobParallelForDefer
    {
        /// <summary>
        /// The radius of the particles.
        /// </summary>
        [ReadOnly]
        public float ParticleRadius;

        /// <summary>
        /// The simulation time step squared.
        /// </summary>
        [ReadOnly]
        public float TimeStepSquared;

        /// <summary>
        /// The enabled state of the particles.
        /// </summary>
        
        [NativeDisableParallelForRestriction]
        public NativeArray<bool> ParticlesEnabled;

        /// <summary>
        /// The breakable state of the particles.
        /// </summary>
        [ReadOnly]
        public NativeArray<bool> ParticlesBreakable;

        /// <summary>
        /// The predicted positions of the particles.
        /// </summary>
        [ReadOnly]
        public NativeArray<float3> ParticlesPredictedPosition;

        /// <summary>
        /// The inverse mass of the particles.
        /// </summary>
        [ReadOnly]
        public NativeArray<float> ParticlesInverseMass;

        /// <summary>
        /// The phase of the particles.
        /// </summary>
        [ReadOnly]
        public NativeArray<uint> ParticlesPhase;

        /// <summary>
        /// The index of the physics material for each particle.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> ParticlesPhysicsMaterialIndex;

        /// <summary>
        /// THe index of the simulation mesh descriptor for each particle.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> ParticlesSimulationMeshDescriptorIndex;

        /// <summary>
        /// The physics materials.
        /// </summary>
        [ReadOnly]
        public NativeArray<PhysicsMaterial> PhysicsMaterials;

        /// <summary>
        /// The simulation mesh descriptors.
        /// </summary>
        [ReadOnly]
        public NativeArray<SimulationMeshDescriptor> SimulationMeshDescriptors;

        /// <summary>
        /// The flag to indicate if the enabled state of the particles of a simulation mesh has changed.
        /// </summary>
        [WriteOnly]
        [NativeDisableContainerSafetyRestriction]
        public NativeArray<bool> ParticlesEnabledStateHasChanged;

        

        /// <summary>
        /// The contact pairs to resolve.
        /// </summary>
        [ReadOnly]
        public NativeArray<ContactPair> ContactPairs;

        

        /// <summary>
        /// The corrections for the particles.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<float3> ParticleCorrections;

        /// <summary>
        /// The number of corrections for each particle.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<int> ParticleCorrectionsCount;

        /// <summary>
        /// The locking array for interlocked operations.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<int> ParticleLocks;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int contactIndex)
        {
            if (contactIndex >= ContactPairs.Length)
            {
                return;
            }

            ContactPair hiuEysQVRuKbzkgHWsntvPyFQEzKttdp = ContactPairs[contactIndex];

            int RtRVojxmELBhAETlURBMSfvhWDiEXTYc = hiuEysQVRuKbzkgHWsntvPyFQEzKttdp.Particle;
            int hSNBYurcjPqRZERJipMLZiQaLXwPDXsk = hiuEysQVRuKbzkgHWsntvPyFQEzKttdp.OtherParticle;

            if (Unity.Burst.CompilerServices.Hint.Unlikely(ParticlesPhase[RtRVojxmELBhAETlURBMSfvhWDiEXTYc] == 0 || ParticlesPhase[hSNBYurcjPqRZERJipMLZiQaLXwPDXsk] == 0))
            {
                return;
            }

            if (ParticlesPhase[RtRVojxmELBhAETlURBMSfvhWDiEXTYc] == ParticlesPhase[hSNBYurcjPqRZERJipMLZiQaLXwPDXsk] && (ParticlesPhase[RtRVojxmELBhAETlURBMSfvhWDiEXTYc] >> 7 != 1))
            {
                return;
            }

            if (Unity.Burst.CompilerServices.Hint.Unlikely(!ParticlesEnabled[RtRVojxmELBhAETlURBMSfvhWDiEXTYc] || !ParticlesEnabled[hSNBYurcjPqRZERJipMLZiQaLXwPDXsk]))
            {
                return;
            }

            var QzvwPxClxoWrRiEiZafzEWGKGsJNydxp = ParticlesInverseMass[RtRVojxmELBhAETlURBMSfvhWDiEXTYc] + ParticlesInverseMass[hSNBYurcjPqRZERJipMLZiQaLXwPDXsk];

            if (Unity.Burst.CompilerServices.Hint.Unlikely(QzvwPxClxoWrRiEiZafzEWGKGsJNydxp == 0))
            {
                return;
            }

            
            float3 FmzEvlMnJNPYPpGpIxIOiHEEtBEmbBDs = ParticlesPredictedPosition[RtRVojxmELBhAETlURBMSfvhWDiEXTYc] - ParticlesPredictedPosition[hSNBYurcjPqRZERJipMLZiQaLXwPDXsk];

            float NAObaUWVGcwCnRHqnPbnKRVkAYGatDnr = math.length(FmzEvlMnJNPYPpGpIxIOiHEEtBEmbBDs);

            if (Unity.Burst.CompilerServices.Hint.Unlikely(NAObaUWVGcwCnRHqnPbnKRVkAYGatDnr == 0))
            {
                return;
            }

            FmzEvlMnJNPYPpGpIxIOiHEEtBEmbBDs = (1.0f / NAObaUWVGcwCnRHqnPbnKRVkAYGatDnr) * FmzEvlMnJNPYPpGpIxIOiHEEtBEmbBDs;
            float kcZQQVevWNKvXbwnBQlKdnDROZITPrYY = NAObaUWVGcwCnRHqnPbnKRVkAYGatDnr - (2 * ParticleRadius);

            if (Unity.Burst.CompilerServices.Hint.Unlikely(kcZQQVevWNKvXbwnBQlKdnDROZITPrYY > 0))
            {
                return;
            }

            float YigkHgUpbHhJjAmxEhrdzXyEQhYBwpUw = -kcZQQVevWNKvXbwnBQlKdnDROZITPrYY / (QzvwPxClxoWrRiEiZafzEWGKGsJNydxp + (0 / .1f));

            float3 sfdejOXrxNQZSGUlECtZGdvLOFKVoLWl = YigkHgUpbHhJjAmxEhrdzXyEQhYBwpUw * ParticlesInverseMass[RtRVojxmELBhAETlURBMSfvhWDiEXTYc] * FmzEvlMnJNPYPpGpIxIOiHEEtBEmbBDs;

            if (Unity.Burst.CompilerServices.Hint.Unlikely(ParticlesBreakable[RtRVojxmELBhAETlURBMSfvhWDiEXTYc]))
            {
                PhysicsMaterial kNOBcVGQOUUuqBbnVkNYpyHADCkDsCvp = PhysicsMaterials[ParticlesPhysicsMaterialIndex[RtRVojxmELBhAETlURBMSfvhWDiEXTYc] + SimulationMeshDescriptors[ParticlesSimulationMeshDescriptorIndex[RtRVojxmELBhAETlURBMSfvhWDiEXTYc]].PhysicsMaterialStartIndex];

                
                if (Unity.Burst.CompilerServices.Hint.Unlikely(math.lengthsq(sfdejOXrxNQZSGUlECtZGdvLOFKVoLWl / TimeStepSquared) > kNOBcVGQOUUuqBbnVkNYpyHADCkDsCvp.CutForceThreshold && ((ParticlesPhase[hSNBYurcjPqRZERJipMLZiQaLXwPDXsk] & (1 << 6)) >> 6) == 1))
                {
                    ParticlesEnabledStateHasChanged[ParticlesSimulationMeshDescriptorIndex[RtRVojxmELBhAETlURBMSfvhWDiEXTYc]] = true;
                    ParticlesEnabled[RtRVojxmELBhAETlURBMSfvhWDiEXTYc] = false;
                }
            }

            
            
            
            ParticleCorrections.InterlockedAddFloat3(ParticleLocks, RtRVojxmELBhAETlURBMSfvhWDiEXTYc, sfdejOXrxNQZSGUlECtZGdvLOFKVoLWl);
            ParticleCorrectionsCount.InterlockedAdd(RtRVojxmELBhAETlURBMSfvhWDiEXTYc, 1);
        }
    }
}
