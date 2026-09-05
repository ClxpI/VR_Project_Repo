namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Applies contact friction to the particles.
    /// </summary>
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    internal struct ApplyContactFrictionJob : IJobParallelForDefer
    {
        /// <summary>
        /// The particle radius.
        /// </summary>
        [ReadOnly]
        public float ParticleRadius;

        /// <summary>
        /// The contact pairs to resolve.
        /// </summary>
        [ReadOnly]
        public NativeArray<ContactPair> ContactPairs;

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
        /// THe index of the simulation mesh descriptor for each particle.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> ParticlesSimulationMeshDescriptorIndex;

        /// <summary>
        /// The index of the physics material for each particle.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> ParticlesPhysicsMaterialIndex;

        /// <summary>
        /// The enabled state of the particles.
        /// </summary>
        
        [ReadOnly]
        public NativeArray<bool> ParticlesEnabled;

        /// <summary>
        /// The predicted position of the particles before contact solving.
        /// </summary>
        [ReadOnly]
        public NativeArray<float3> ParticlesPredictedPositionBeforeCollisions;

        /// <summary>
        /// The predicted position of the particles after contact solving.
        /// </summary>
        [ReadOnly]
        public NativeArray<float3> ParticlesPredictedPositionAfterCollisions;

        /// <summary>
        /// The phase of the particles.
        /// </summary>
        [ReadOnly]
        public NativeArray<uint> ParticlesPhase;

        /// <summary>
        /// The inverse mass of the particles.
        /// </summary>
        [ReadOnly]
        public NativeArray<float> ParticlesInverseMass;

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

            ContactPair ilKYHvHiwxscLbflcJdtQpxqdxrImIgl = ContactPairs[contactIndex];

            int iVLzniRIcdajuLIYOMscrRLTHcCBThoa = ilKYHvHiwxscLbflcJdtQpxqdxrImIgl.Particle;
            int oclgUcpGvAisbmEurzzaTzSnQRBoIsZu = ilKYHvHiwxscLbflcJdtQpxqdxrImIgl.OtherParticle;

            if (ParticlesPhase[iVLzniRIcdajuLIYOMscrRLTHcCBThoa] == ParticlesPhase[oclgUcpGvAisbmEurzzaTzSnQRBoIsZu] && (ParticlesPhase[iVLzniRIcdajuLIYOMscrRLTHcCBThoa] >> 7 != 1))
            {
                return;
            }

            if (Unity.Burst.CompilerServices.Hint.Unlikely(!ParticlesEnabled[iVLzniRIcdajuLIYOMscrRLTHcCBThoa] || !ParticlesEnabled[oclgUcpGvAisbmEurzzaTzSnQRBoIsZu]))
            {
                return;
            }

            var VzzogQmjsxFUCkBqmsjJVTArsNkOFTsR = ParticlesInverseMass[iVLzniRIcdajuLIYOMscrRLTHcCBThoa] + ParticlesInverseMass[oclgUcpGvAisbmEurzzaTzSnQRBoIsZu];

            if (Unity.Burst.CompilerServices.Hint.Unlikely(VzzogQmjsxFUCkBqmsjJVTArsNkOFTsR == 0))
            {
                return;
            }

            var EjuygGwdcvzedkVzVZCCpgUEQBVlVVXC = (ParticlesPredictedPositionAfterCollisions[iVLzniRIcdajuLIYOMscrRLTHcCBThoa] - ParticlesPredictedPositionBeforeCollisions[iVLzniRIcdajuLIYOMscrRLTHcCBThoa]) - (ParticlesPredictedPositionAfterCollisions[oclgUcpGvAisbmEurzzaTzSnQRBoIsZu] - ParticlesPredictedPositionBeforeCollisions[oclgUcpGvAisbmEurzzaTzSnQRBoIsZu]);
            var nePDcoYcUuxBDOcUuQSfcmfRlaqccURD = math.length(EjuygGwdcvzedkVzVZCCpgUEQBVlVVXC);

            
            var JUDCmqEYQmNigHnpVdjyRTaLtNRiqbTk = (PhysicsMaterials[ParticlesPhysicsMaterialIndex[iVLzniRIcdajuLIYOMscrRLTHcCBThoa]].StaticFriction + PhysicsMaterials[ParticlesPhysicsMaterialIndex[oclgUcpGvAisbmEurzzaTzSnQRBoIsZu]].StaticFriction) / 2;
            var JYjeWgdrkmdoFZgvfnoQvcCUzPoKZiQM = (PhysicsMaterials[ParticlesPhysicsMaterialIndex[iVLzniRIcdajuLIYOMscrRLTHcCBThoa]].KineticFriction + PhysicsMaterials[ParticlesPhysicsMaterialIndex[oclgUcpGvAisbmEurzzaTzSnQRBoIsZu]].KineticFriction) / 2;

            var JEdIvPDBqitNPzmGzHmipbhPQFxqLtkb = (2 * ParticleRadius) - ilKYHvHiwxscLbflcJdtQpxqdxrImIgl.Separation;

            float3 ATxNCgaTIdnedfbdManzNbGYjXGCJFNF;
            if (nePDcoYcUuxBDOcUuQSfcmfRlaqccURD < JUDCmqEYQmNigHnpVdjyRTaLtNRiqbTk * JEdIvPDBqitNPzmGzHmipbhPQFxqLtkb)
            {
                ATxNCgaTIdnedfbdManzNbGYjXGCJFNF = (ParticlesInverseMass[iVLzniRIcdajuLIYOMscrRLTHcCBThoa] / VzzogQmjsxFUCkBqmsjJVTArsNkOFTsR) * EjuygGwdcvzedkVzVZCCpgUEQBVlVVXC;
            }
            else
            {
                ATxNCgaTIdnedfbdManzNbGYjXGCJFNF = (ParticlesInverseMass[iVLzniRIcdajuLIYOMscrRLTHcCBThoa] / VzzogQmjsxFUCkBqmsjJVTArsNkOFTsR) * EjuygGwdcvzedkVzVZCCpgUEQBVlVVXC * math.min((JYjeWgdrkmdoFZgvfnoQvcCUzPoKZiQM * JEdIvPDBqitNPzmGzHmipbhPQFxqLtkb) / nePDcoYcUuxBDOcUuQSfcmfRlaqccURD, 1f);
            }

            
            
            
            ParticleCorrections.InterlockedAddFloat3(ParticleLocks, iVLzniRIcdajuLIYOMscrRLTHcCBThoa, ATxNCgaTIdnedfbdManzNbGYjXGCJFNF);
            ParticleCorrectionsCount.InterlockedAdd(iVLzniRIcdajuLIYOMscrRLTHcCBThoa, 1);
        }
    }
}