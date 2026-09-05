namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Solves volume constraints.
    /// </summary>
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    internal struct SolveVolumeConstraintsJob : IJobParallelFor
    {
        /// <summary>
        /// The simulation time step squared.
        /// </summary>
        [ReadOnly]
        public float TimeStepSquared;

        

        /// <summary>
        /// The enabled particles.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<bool> ParticlesEnabled;

        /// <summary>
        /// THe predicted particle positions.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<float3> ParticlesPredictedPosition;

        /// <summary>
        /// The inverse mass of the particles.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<float> ParticlesInverseMass;

        

        /// <summary>
        /// The enabled volume constraints.
        /// </summary>
        public NativeArray<bool> VolumeConstraintsEnabled;

        /// <summary>
        /// The breakable volume constraints.
        /// </summary>
        [ReadOnly]
        public NativeArray<bool> VolumeConstraintsBreakable;

        /// <summary>
        /// The physics material index of the volume constraints.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> VolumeConstraintsPhysicsMaterialIndex;

        /// <summary>
        /// The volume constraints particle A index.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> VolumeConstraintsParticleA;

        /// <summary>
        /// The volume constraints particle B index.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> VolumeConstraintsParticleB;

        /// <summary>
        /// The volume constraints particle C index.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> VolumeConstraintsParticleC;

        /// <summary>
        /// The volume constraints particle D index.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> VolumeConstraintsParticleD;

        /// <summary>
        /// The volume constraints volume at rest configuration.
        /// </summary>
        [ReadOnly]
        public NativeArray<float> VolumeConstraintsRestVolume;

        /// <summary>
        /// The volume constraints simulation mesh descriptor index.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> VolumeConstraintsSimulationMeshDescriptorIndex;

        

        /// <summary>
        /// The simulation mesh descriptors.
        /// </summary>
        [ReadOnly]
        public NativeArray<SimulationMeshDescriptor> SimulationMeshDescriptors;

        /// <summary>
        /// The flags indicating if the volume constraints enabled state has changed.
        /// </summary>
        [WriteOnly]
        [NativeDisableContainerSafetyRestriction]
        public NativeArray<bool> VolumeConstraintsEnabledStateHasChanged;

        /// <summary>
        /// The physics materials.
        /// </summary>
        [ReadOnly]
        public NativeArray<PhysicsMaterial> PhysicsMaterials;

        

        /// <summary>
        /// The particle corrections buffer.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<float3> ParticleCorrections;

        /// <summary>
        /// The particle corrections count.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<int> ParticleCorrectionsCount;

        /// <summary>
        /// The particle locks, for interlocked operations.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<int> ParticleLocks;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int volumeConstraintIndex)
        {
            
            
            
            
            
            

            
            if (Unity.Burst.CompilerServices.Hint.Unlikely(!VolumeConstraintsEnabled[volumeConstraintIndex]))
            {
                return;
            }

            float biVPkInPdkXKwXPCeDAIeIFRtTjgoiGc, TASpqpldeOMjAKDsavoXaXcDlMkCwHOg, JWNqugVQrLQrrZckldhUbYTZWgzfZZiI, AUksbTzTbNDJArbUCRFAlLlUzhIXRIgE, lHFHpcYttaFCygSbBLymVvlsYSaEQhiO;

            SimulationMeshDescriptor hUbORgTrEXGZKtWndhNkTObBCSmgeQUU = SimulationMeshDescriptors[VolumeConstraintsSimulationMeshDescriptorIndex[volumeConstraintIndex]];

            int gdsxgYEMbOSPRgADZCbwbeiamSWvljjz = VolumeConstraintsParticleA[volumeConstraintIndex] + hUbORgTrEXGZKtWndhNkTObBCSmgeQUU.ParticleStartIndex;
            int fAhYbrPOuTFVjakuxIoWLcGJTNCpFvwr = VolumeConstraintsParticleB[volumeConstraintIndex] + hUbORgTrEXGZKtWndhNkTObBCSmgeQUU.ParticleStartIndex;
            int DcVzbDVAVtJWxEXDPmiPcPCPOABivkFY = VolumeConstraintsParticleC[volumeConstraintIndex] + hUbORgTrEXGZKtWndhNkTObBCSmgeQUU.ParticleStartIndex;
            int tPmKBEnYxiLHiuLPfJJOBQFjvxyIxDGl = VolumeConstraintsParticleD[volumeConstraintIndex] + hUbORgTrEXGZKtWndhNkTObBCSmgeQUU.ParticleStartIndex;

            if (Unity.Burst.CompilerServices.Hint.Unlikely(!ParticlesEnabled[gdsxgYEMbOSPRgADZCbwbeiamSWvljjz] || !ParticlesEnabled[fAhYbrPOuTFVjakuxIoWLcGJTNCpFvwr] || !ParticlesEnabled[DcVzbDVAVtJWxEXDPmiPcPCPOABivkFY] || !ParticlesEnabled[tPmKBEnYxiLHiuLPfJJOBQFjvxyIxDGl]))
            {
                VolumeConstraintsEnabled[volumeConstraintIndex] = false;
                VolumeConstraintsEnabledStateHasChanged[VolumeConstraintsSimulationMeshDescriptorIndex[volumeConstraintIndex]] = true;
                return;
            }

            biVPkInPdkXKwXPCeDAIeIFRtTjgoiGc = PhysicsMaterials[VolumeConstraintsPhysicsMaterialIndex[volumeConstraintIndex] + hUbORgTrEXGZKtWndhNkTObBCSmgeQUU.PhysicsMaterialStartIndex].VolumeConstraintInverseStiffness / TimeStepSquared;

            TASpqpldeOMjAKDsavoXaXcDlMkCwHOg = Math.TetrahedronVolume(ParticlesPredictedPosition[gdsxgYEMbOSPRgADZCbwbeiamSWvljjz], ParticlesPredictedPosition[fAhYbrPOuTFVjakuxIoWLcGJTNCpFvwr], ParticlesPredictedPosition[DcVzbDVAVtJWxEXDPmiPcPCPOABivkFY], ParticlesPredictedPosition[tPmKBEnYxiLHiuLPfJJOBQFjvxyIxDGl]);

            JWNqugVQrLQrrZckldhUbYTZWgzfZZiI = TASpqpldeOMjAKDsavoXaXcDlMkCwHOg - VolumeConstraintsRestVolume[volumeConstraintIndex];

            if (JWNqugVQrLQrrZckldhUbYTZWgzfZZiI == 0)
            {
                return;
            }

            
            float3 JlDbZAPospcWANTDEiuBcoGRxKUvZybc = math.cross(ParticlesPredictedPosition[DcVzbDVAVtJWxEXDPmiPcPCPOABivkFY] - ParticlesPredictedPosition[fAhYbrPOuTFVjakuxIoWLcGJTNCpFvwr], ParticlesPredictedPosition[tPmKBEnYxiLHiuLPfJJOBQFjvxyIxDGl] - ParticlesPredictedPosition[fAhYbrPOuTFVjakuxIoWLcGJTNCpFvwr]) / 6f;
            float3 GQVrUZbnZXfAurghWsKWpvVqwMczDWHG = math.cross(ParticlesPredictedPosition[tPmKBEnYxiLHiuLPfJJOBQFjvxyIxDGl] - ParticlesPredictedPosition[gdsxgYEMbOSPRgADZCbwbeiamSWvljjz], ParticlesPredictedPosition[DcVzbDVAVtJWxEXDPmiPcPCPOABivkFY] - ParticlesPredictedPosition[gdsxgYEMbOSPRgADZCbwbeiamSWvljjz]) / 6f;
            float3 RJeHAEqhVZapPkblJffsJkymhkflKdVa = math.cross(ParticlesPredictedPosition[fAhYbrPOuTFVjakuxIoWLcGJTNCpFvwr] - ParticlesPredictedPosition[gdsxgYEMbOSPRgADZCbwbeiamSWvljjz], ParticlesPredictedPosition[tPmKBEnYxiLHiuLPfJJOBQFjvxyIxDGl] - ParticlesPredictedPosition[gdsxgYEMbOSPRgADZCbwbeiamSWvljjz]) / 6f;
            float3 dWlqZTjjvuSktEpbAskXzbEQsyJxRcIT = math.cross(ParticlesPredictedPosition[DcVzbDVAVtJWxEXDPmiPcPCPOABivkFY] - ParticlesPredictedPosition[gdsxgYEMbOSPRgADZCbwbeiamSWvljjz], ParticlesPredictedPosition[fAhYbrPOuTFVjakuxIoWLcGJTNCpFvwr] - ParticlesPredictedPosition[gdsxgYEMbOSPRgADZCbwbeiamSWvljjz]) / 6f;

            lHFHpcYttaFCygSbBLymVvlsYSaEQhiO = ParticlesInverseMass[gdsxgYEMbOSPRgADZCbwbeiamSWvljjz] * math.lengthsq(JlDbZAPospcWANTDEiuBcoGRxKUvZybc);
            lHFHpcYttaFCygSbBLymVvlsYSaEQhiO += ParticlesInverseMass[fAhYbrPOuTFVjakuxIoWLcGJTNCpFvwr] * math.lengthsq(GQVrUZbnZXfAurghWsKWpvVqwMczDWHG);
            lHFHpcYttaFCygSbBLymVvlsYSaEQhiO += ParticlesInverseMass[DcVzbDVAVtJWxEXDPmiPcPCPOABivkFY] * math.lengthsq(RJeHAEqhVZapPkblJffsJkymhkflKdVa);
            lHFHpcYttaFCygSbBLymVvlsYSaEQhiO += ParticlesInverseMass[tPmKBEnYxiLHiuLPfJJOBQFjvxyIxDGl] * math.lengthsq(dWlqZTjjvuSktEpbAskXzbEQsyJxRcIT);

            if (lHFHpcYttaFCygSbBLymVvlsYSaEQhiO == 0)
            {
                return;
            }

            AUksbTzTbNDJArbUCRFAlLlUzhIXRIgE = -JWNqugVQrLQrrZckldhUbYTZWgzfZZiI / (lHFHpcYttaFCygSbBLymVvlsYSaEQhiO + biVPkInPdkXKwXPCeDAIeIFRtTjgoiGc);

            
            
            
            ParticleCorrections.InterlockedAddFloat3(ParticleLocks, gdsxgYEMbOSPRgADZCbwbeiamSWvljjz, AUksbTzTbNDJArbUCRFAlLlUzhIXRIgE * ParticlesInverseMass[gdsxgYEMbOSPRgADZCbwbeiamSWvljjz] * JlDbZAPospcWANTDEiuBcoGRxKUvZybc);
            ParticleCorrectionsCount.InterlockedAdd(gdsxgYEMbOSPRgADZCbwbeiamSWvljjz, 1);
            ParticleCorrections.InterlockedAddFloat3(ParticleLocks, fAhYbrPOuTFVjakuxIoWLcGJTNCpFvwr, AUksbTzTbNDJArbUCRFAlLlUzhIXRIgE * ParticlesInverseMass[fAhYbrPOuTFVjakuxIoWLcGJTNCpFvwr] * GQVrUZbnZXfAurghWsKWpvVqwMczDWHG);
            ParticleCorrectionsCount.InterlockedAdd(fAhYbrPOuTFVjakuxIoWLcGJTNCpFvwr, 1);
            ParticleCorrections.InterlockedAddFloat3(ParticleLocks, DcVzbDVAVtJWxEXDPmiPcPCPOABivkFY, AUksbTzTbNDJArbUCRFAlLlUzhIXRIgE * ParticlesInverseMass[DcVzbDVAVtJWxEXDPmiPcPCPOABivkFY] * RJeHAEqhVZapPkblJffsJkymhkflKdVa);
            ParticleCorrectionsCount.InterlockedAdd(DcVzbDVAVtJWxEXDPmiPcPCPOABivkFY, 1);
            ParticleCorrections.InterlockedAddFloat3(ParticleLocks, tPmKBEnYxiLHiuLPfJJOBQFjvxyIxDGl, AUksbTzTbNDJArbUCRFAlLlUzhIXRIgE * ParticlesInverseMass[tPmKBEnYxiLHiuLPfJJOBQFjvxyIxDGl] * dWlqZTjjvuSktEpbAskXzbEQsyJxRcIT);
            ParticleCorrectionsCount.InterlockedAdd(tPmKBEnYxiLHiuLPfJJOBQFjvxyIxDGl, 1);
        }
    }
}