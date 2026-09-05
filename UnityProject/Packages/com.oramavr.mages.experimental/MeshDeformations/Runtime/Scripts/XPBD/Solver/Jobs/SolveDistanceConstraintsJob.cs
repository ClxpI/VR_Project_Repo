namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Solves distance constraints.
    /// </summary>
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    internal struct SolveDistanceConstraintsJob : IJobParallelFor
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
        /// The distance constraints enabled state.
        /// </summary>
        public NativeArray<bool> DistanceConstraintsEnabled;

        /// <summary>
        /// The distance constraints breakable state.
        /// </summary>
        [ReadOnly]
        public NativeArray<bool> DistanceConstraintsBreakable;

        /// <summary>
        /// The distance constraints physics material index.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> DistanceConstraintsPhysicsMaterialIndex;

        /// <summary>
        /// The distance constraints simulation mesh descriptor index.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> DistanceConstraintsSimulationMeshDescriptorIndex;

        /// <summary>
        /// The distance constraints particle A.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> DistanceConstraintsParticleA;

        /// <summary>
        /// The distance constraints particle B.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> DistanceConstraintsParticleB;

        /// <summary>
        /// The distance constraints rest distance.
        /// </summary>
        [ReadOnly]
        public NativeArray<float> DistanceConstraintsRestDistance;

        /// <summary>
        /// The flag for each simulation mesh stating if the distance constraints enabled state has changed this frame.
        /// </summary>
        [NativeDisableParallelForRestriction]
        [NativeDisableContainerSafetyRestriction]
        [WriteOnly]
        public NativeArray<bool> DistanceConstraintsEnabledStateHasChanged;

        

        /// <summary>
        /// The simulation mesh descriptors.
        /// </summary>
        [ReadOnly]
        public NativeArray<SimulationMeshDescriptor> SimulationMeshDescriptors;

        /// <summary>
        /// The physics materials.
        /// </summary>
        [ReadOnly]
        public NativeArray<PhysicsMaterial> PhysicsMaterials;

        

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
            if (Unity.Burst.CompilerServices.Hint.Unlikely(!DistanceConstraintsEnabled[distanceConstraintIndex]))
            {
                return;
            }

            SimulationMeshDescriptor lzbFIDpbRXDfyrZxfzdszcuklBomogZR = SimulationMeshDescriptors[DistanceConstraintsSimulationMeshDescriptorIndex[distanceConstraintIndex]];

            int tJyKXOoIFiqlKVKOfLIguORtBLGhiZdm = DistanceConstraintsParticleA[distanceConstraintIndex] + lzbFIDpbRXDfyrZxfzdszcuklBomogZR.ParticleStartIndex;
            int dNDaNTTTjZtMpNSIIoabPnfVeMGfXexL = DistanceConstraintsParticleB[distanceConstraintIndex] + lzbFIDpbRXDfyrZxfzdszcuklBomogZR.ParticleStartIndex;

            if (Unity.Burst.CompilerServices.Hint.Unlikely(!ParticlesEnabled[tJyKXOoIFiqlKVKOfLIguORtBLGhiZdm] || !ParticlesEnabled[dNDaNTTTjZtMpNSIIoabPnfVeMGfXexL]))
            {
                DistanceConstraintsEnabled[distanceConstraintIndex] = false;
                DistanceConstraintsEnabledStateHasChanged[DistanceConstraintsSimulationMeshDescriptorIndex[distanceConstraintIndex]] = true;
                return;
            }

            var VwNKZoOcwfCDVZWvVXslMOMtmUhUGike = ParticlesInverseMass[tJyKXOoIFiqlKVKOfLIguORtBLGhiZdm] + ParticlesInverseMass[dNDaNTTTjZtMpNSIIoabPnfVeMGfXexL];

            if (Unity.Burst.CompilerServices.Hint.Unlikely(VwNKZoOcwfCDVZWvVXslMOMtmUhUGike == 0))
            {
                return;
            }

            
            float3 QpuKGctIyXQSyhrmpYKRMKoRmQduLGBw = ParticlesPredictedPosition[tJyKXOoIFiqlKVKOfLIguORtBLGhiZdm] - ParticlesPredictedPosition[dNDaNTTTjZtMpNSIIoabPnfVeMGfXexL];

            float jSOkOBCsLMBdtFrMsjpmBEXanbOLqYjt = math.length(QpuKGctIyXQSyhrmpYKRMKoRmQduLGBw);

            if (Unity.Burst.CompilerServices.Hint.Unlikely(jSOkOBCsLMBdtFrMsjpmBEXanbOLqYjt == 0))
            {
                return;
            }

            QpuKGctIyXQSyhrmpYKRMKoRmQduLGBw = (1.0f / jSOkOBCsLMBdtFrMsjpmBEXanbOLqYjt) * QpuKGctIyXQSyhrmpYKRMKoRmQduLGBw;
            float ENAxmDwmdezclwPmShAKRLiwJalQaTot = jSOkOBCsLMBdtFrMsjpmBEXanbOLqYjt - DistanceConstraintsRestDistance[distanceConstraintIndex];

            var SHkfSaMvVJPyTGLenOqqNfuKjsaUIZrh = PhysicsMaterials[DistanceConstraintsPhysicsMaterialIndex[distanceConstraintIndex] + lzbFIDpbRXDfyrZxfzdszcuklBomogZR.PhysicsMaterialStartIndex];

            float RhRgkmTXevTzBoVhOFWrqtCqoGhZVJpx = -ENAxmDwmdezclwPmShAKRLiwJalQaTot / (VwNKZoOcwfCDVZWvVXslMOMtmUhUGike + (SHkfSaMvVJPyTGLenOqqNfuKjsaUIZrh.DistanceConstraintInverseStiffness / TimeStepSquared));

            if (Unity.Burst.CompilerServices.Hint.Unlikely(DistanceConstraintsBreakable[distanceConstraintIndex]))
            {
                
                float JVYnTDOcEBOsvMVLmiKyPhVYxKbGfIxK = math.abs(RhRgkmTXevTzBoVhOFWrqtCqoGhZVJpx) / TimeStepSquared;

                
                if (Unity.Burst.CompilerServices.Hint.Unlikely(JVYnTDOcEBOsvMVLmiKyPhVYxKbGfIxK > SHkfSaMvVJPyTGLenOqqNfuKjsaUIZrh.DistanceConstraintTearForce))
                {
                    
                    DistanceConstraintsEnabled[distanceConstraintIndex] = false;
                    DistanceConstraintsEnabledStateHasChanged[DistanceConstraintsSimulationMeshDescriptorIndex[distanceConstraintIndex]] = true;
                    return;
                }
            }

            
            
            
            ParticleCorrections.InterlockedAddFloat3(ParticleLocks, tJyKXOoIFiqlKVKOfLIguORtBLGhiZdm, RhRgkmTXevTzBoVhOFWrqtCqoGhZVJpx * ParticlesInverseMass[tJyKXOoIFiqlKVKOfLIguORtBLGhiZdm] * QpuKGctIyXQSyhrmpYKRMKoRmQduLGBw);
            ParticleCorrectionsCount.InterlockedAdd(tJyKXOoIFiqlKVKOfLIguORtBLGhiZdm, 1);
            ParticleCorrections.InterlockedAddFloat3(ParticleLocks, dNDaNTTTjZtMpNSIIoabPnfVeMGfXexL, -RhRgkmTXevTzBoVhOFWrqtCqoGhZVJpx * ParticlesInverseMass[dNDaNTTTjZtMpNSIIoabPnfVeMGfXexL] * QpuKGctIyXQSyhrmpYKRMKoRmQduLGBw);
            ParticleCorrectionsCount.InterlockedAdd(dNDaNTTTjZtMpNSIIoabPnfVeMGfXexL, 1);
        }
    }
}
