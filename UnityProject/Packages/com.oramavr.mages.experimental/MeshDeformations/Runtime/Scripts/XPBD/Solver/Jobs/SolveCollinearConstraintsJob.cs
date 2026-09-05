namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Solver for <see cref="CollinearConstraint"/>.
    /// </summary>
    [BurstCompile]
    internal struct SolveCollinearConstraintsJob : IJobParallelFor
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
        [ReadOnly]
        public NativeArray<float> ParticlesInverseMass;

        /// <summary>
        /// The collinear constraints enabled state.
        /// </summary>
        public NativeArray<bool> CollinearConstraintsEnabled;

        /// <summary>
        /// The collinear constraints breakable state.
        /// </summary>
        [ReadOnly]
        public NativeArray<bool> CollinearConstraintsBreakable;

        /// <summary>
        /// The collinear constraints physics material index.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> CollinearConstraintsPhysicsMaterialIndex;

        /// <summary>
        /// The collinear constraints simulation mesh descriptor index.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> CollinearConstraintsSimulationMeshDescriptorIndex;

        /// <summary>
        /// The collinear constraints particle A.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> CollinearConstraintsParticleA;

        /// <summary>
        /// The collinear constraints particle B.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> CollinearConstraintsParticleB;

        /// <summary>
        /// The collinear constraints particle C.
        /// </summary>
        [ReadOnly]
        public NativeArray<int> CollinearConstraintsParticleC;

        

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

        /// <inheritdoc/>
        public void Execute(int collinearConstraintIndex)
        {
            if (Unity.Burst.CompilerServices.Hint.Unlikely(!CollinearConstraintsEnabled[collinearConstraintIndex]))
            {
                return;
            }

            SimulationMeshDescriptor dwyYJIQpjiZbnAKnCKyeUevbdsxdXtfJ = SimulationMeshDescriptors[CollinearConstraintsSimulationMeshDescriptorIndex[collinearConstraintIndex]];

            int tiPViZBqvfGfIVmIhrfqZQVlVNkUtAmZ = CollinearConstraintsParticleA[collinearConstraintIndex] + dwyYJIQpjiZbnAKnCKyeUevbdsxdXtfJ.ParticleStartIndex;
            int aSAaILIWBhrfeUfkWmLruUhefLmqCtjZ = CollinearConstraintsParticleB[collinearConstraintIndex] + dwyYJIQpjiZbnAKnCKyeUevbdsxdXtfJ.ParticleStartIndex;
            int LcjGtSxWXptIosqgWzFGQlRmDaidWcrZ = CollinearConstraintsParticleC[collinearConstraintIndex] + dwyYJIQpjiZbnAKnCKyeUevbdsxdXtfJ.ParticleStartIndex;

            var gAPiltdwXGkllBQjVGpAFxiWhjsranjO = ParticlesPredictedPosition[tiPViZBqvfGfIVmIhrfqZQVlVNkUtAmZ];
            var LcjNoJucgSjpZgdtKebVLnYnTKATDRjJ = ParticlesPredictedPosition[aSAaILIWBhrfeUfkWmLruUhefLmqCtjZ];
            var ASKZxOjlpgWwvLWkyMPCTJxRqZVktYra = ParticlesPredictedPosition[LcjGtSxWXptIosqgWzFGQlRmDaidWcrZ];

            var GArbDBpJAjFIDlGvakJNoVDNOsFmkcmT = math.normalize(LcjNoJucgSjpZgdtKebVLnYnTKATDRjJ - gAPiltdwXGkllBQjVGpAFxiWhjsranjO);
            var gorffDSHaXafdxlBoYiLLVyPtXkWWDlT = math.normalize(ASKZxOjlpgWwvLWkyMPCTJxRqZVktYra - LcjNoJucgSjpZgdtKebVLnYnTKATDRjJ);

            var RhNNrtosARrNPSFBagydbajIDvyRIkcX = math.length(LcjNoJucgSjpZgdtKebVLnYnTKATDRjJ - gAPiltdwXGkllBQjVGpAFxiWhjsranjO);
            var FCuBGMxAHvgOTpMfNbPjeBmCBEJXdeoY = math.length(ASKZxOjlpgWwvLWkyMPCTJxRqZVktYra - LcjNoJucgSjpZgdtKebVLnYnTKATDRjJ);
            var QbiXiMRjUCqTUVyHHwxoVMukwxUzNfrH = math.length(gAPiltdwXGkllBQjVGpAFxiWhjsranjO - ASKZxOjlpgWwvLWkyMPCTJxRqZVktYra);

            if (RhNNrtosARrNPSFBagydbajIDvyRIkcX < math.EPSILON || FCuBGMxAHvgOTpMfNbPjeBmCBEJXdeoY < math.EPSILON || QbiXiMRjUCqTUVyHHwxoVMukwxUzNfrH < math.EPSILON)
            {
                return;
            }

            
            var comGTSopAkSnpstCgggxYIPqHVdXPpqs = math.dot(GArbDBpJAjFIDlGvakJNoVDNOsFmkcmT, gorffDSHaXafdxlBoYiLLVyPtXkWWDlT);
            var siKdiTYTXqIqETKuvdJBRjlnrPXDuSFw = math.acos(comGTSopAkSnpstCgggxYIPqHVdXPpqs);

            if (siKdiTYTXqIqETKuvdJBRjlnrPXDuSFw < math.EPSILON)
            {
                return;
            }

            
            var YmczoNobEcSfmWwCHIZgZgljksesemRp = PhysicsMaterials[CollinearConstraintsPhysicsMaterialIndex[collinearConstraintIndex] + dwyYJIQpjiZbnAKnCKyeUevbdsxdXtfJ.PhysicsMaterialStartIndex].CollinearConstraintInverseStiffness / TimeStepSquared;

            if (math.abs(comGTSopAkSnpstCgggxYIPqHVdXPpqs) < math.EPSILON)
            {
                return;
            }

            var wMxTGHFgaWHoiqkOuvRsaTeCaYaGjztW = (gorffDSHaXafdxlBoYiLLVyPtXkWWDlT - (GArbDBpJAjFIDlGvakJNoVDNOsFmkcmT * comGTSopAkSnpstCgggxYIPqHVdXPpqs)) / math.length(gAPiltdwXGkllBQjVGpAFxiWhjsranjO - ASKZxOjlpgWwvLWkyMPCTJxRqZVktYra);
            var qrFsWfmJmipXaIFeKTJzPmGMvButHhpt = (GArbDBpJAjFIDlGvakJNoVDNOsFmkcmT - (gorffDSHaXafdxlBoYiLLVyPtXkWWDlT * comGTSopAkSnpstCgggxYIPqHVdXPpqs)) / math.length(LcjNoJucgSjpZgdtKebVLnYnTKATDRjJ - ASKZxOjlpgWwvLWkyMPCTJxRqZVktYra);
            var sAQqarOBEXBRXLdtkDmVFNXYucfEvMXR = -wMxTGHFgaWHoiqkOuvRsaTeCaYaGjztW - qrFsWfmJmipXaIFeKTJzPmGMvButHhpt;

            
            var XUzlYmFMeIGDtWNesIjgmdHMNZwcNAdY = ParticlesInverseMass[tiPViZBqvfGfIVmIhrfqZQVlVNkUtAmZ] * math.lengthsq(wMxTGHFgaWHoiqkOuvRsaTeCaYaGjztW);
            XUzlYmFMeIGDtWNesIjgmdHMNZwcNAdY += ParticlesInverseMass[aSAaILIWBhrfeUfkWmLruUhefLmqCtjZ] * math.lengthsq(qrFsWfmJmipXaIFeKTJzPmGMvButHhpt);
            XUzlYmFMeIGDtWNesIjgmdHMNZwcNAdY += ParticlesInverseMass[LcjGtSxWXptIosqgWzFGQlRmDaidWcrZ] * math.lengthsq(sAQqarOBEXBRXLdtkDmVFNXYucfEvMXR);

            if (XUzlYmFMeIGDtWNesIjgmdHMNZwcNAdY < math.EPSILON)
            {
                return;
            }

            
            var BTTgTvPIPXQDBJpFQjXhSoLijrmIvijv = math.sqrt(1 - math.pow(comGTSopAkSnpstCgggxYIPqHVdXPpqs, 2));

            if (!float.IsNormal(BTTgTvPIPXQDBJpFQjXhSoLijrmIvijv))
            {
                return;
            }

            BTTgTvPIPXQDBJpFQjXhSoLijrmIvijv = math.clamp(BTTgTvPIPXQDBJpFQjXhSoLijrmIvijv, -1.0f, 1.0f);

            var uFGgCxAVKWRBbQnfeqlvfnPNxZRHKvEW = -(ParticlesInverseMass[tiPViZBqvfGfIVmIhrfqZQVlVNkUtAmZ] * BTTgTvPIPXQDBJpFQjXhSoLijrmIvijv * siKdiTYTXqIqETKuvdJBRjlnrPXDuSFw / (XUzlYmFMeIGDtWNesIjgmdHMNZwcNAdY + YmczoNobEcSfmWwCHIZgZgljksesemRp)) * wMxTGHFgaWHoiqkOuvRsaTeCaYaGjztW;
            var PuflSHWtxZMOVhYujQgeHhGFmBNdbIqq = -(ParticlesInverseMass[aSAaILIWBhrfeUfkWmLruUhefLmqCtjZ] * BTTgTvPIPXQDBJpFQjXhSoLijrmIvijv * siKdiTYTXqIqETKuvdJBRjlnrPXDuSFw / (XUzlYmFMeIGDtWNesIjgmdHMNZwcNAdY + YmczoNobEcSfmWwCHIZgZgljksesemRp)) * qrFsWfmJmipXaIFeKTJzPmGMvButHhpt;
            var OZHsQqdcHrwLmTZUIVbUkoQbbllubMei = -(ParticlesInverseMass[LcjGtSxWXptIosqgWzFGQlRmDaidWcrZ] * BTTgTvPIPXQDBJpFQjXhSoLijrmIvijv * siKdiTYTXqIqETKuvdJBRjlnrPXDuSFw / (XUzlYmFMeIGDtWNesIjgmdHMNZwcNAdY + YmczoNobEcSfmWwCHIZgZgljksesemRp)) * sAQqarOBEXBRXLdtkDmVFNXYucfEvMXR;

            
            ParticleCorrections.InterlockedAddFloat3(ParticleLocks, tiPViZBqvfGfIVmIhrfqZQVlVNkUtAmZ, uFGgCxAVKWRBbQnfeqlvfnPNxZRHKvEW);
            ParticleCorrectionsCount.InterlockedAdd(tiPViZBqvfGfIVmIhrfqZQVlVNkUtAmZ, 1);
            ParticleCorrections.InterlockedAddFloat3(ParticleLocks, aSAaILIWBhrfeUfkWmLruUhefLmqCtjZ, PuflSHWtxZMOVhYujQgeHhGFmBNdbIqq);
            ParticleCorrectionsCount.InterlockedAdd(aSAaILIWBhrfeUfkWmLruUhefLmqCtjZ, 1);
            ParticleCorrections.InterlockedAddFloat3(ParticleLocks, LcjGtSxWXptIosqgWzFGQlRmDaidWcrZ, OZHsQqdcHrwLmTZUIVbUkoQbbllubMei);
            ParticleCorrectionsCount.InterlockedAdd(LcjGtSxWXptIosqgWzFGQlRmDaidWcrZ, 1);
        }
    }
}