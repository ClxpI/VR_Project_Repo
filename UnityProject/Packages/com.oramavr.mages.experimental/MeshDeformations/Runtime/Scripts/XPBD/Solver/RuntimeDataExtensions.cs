namespace MAGES.MeshDeformations
{
    using System.Diagnostics;
    using Unity.Collections;

    /// <summary>
    /// Runtime Data Extensions.
    /// </summary>
    internal static class RuntimeDataExtensions
    {
        /// <summary>
        /// Slices the <see cref="RuntimeParticleData"/>.
        /// </summary>
        /// <param name="data">Runtime Data.</param>
        /// <param name="start">Start index.</param>
        /// <param name="count">Count.</param>
        /// <returns>The slice.</returns>
        internal static RuntimeParticleDataSlice Slice(this RuntimeParticleData data, int start, int count)
        {
            Debug.Assert(start >= 0 && start + count <= data.Count, "The slice range is out of bounds.");

            var tkYxQLRGrwbIYbYELXfgRrYDuwHgRAdI = new RuntimeParticleDataSlice
            {
                Enabled = data.Enabled.Slice(start, count),
                Breakable = data.Breakable.Slice(start, count),
                PhysicsMaterialIndex = data.PhysicsMaterialIndex.Slice(start, count),

                InitialPosition = data.InitialPosition.Slice(start, count),
                Position = data.Position.Slice(start, count),
                Velocity = data.Velocity.Slice(start, count),

                InverseMass = data.InverseMass.Slice(start, count),
                AdjustedInverseMass = data.AdjustedInverseMass.Slice(start),
                Phase = data.Phase.Slice(start, count),

                SimulationMeshDescriptorIndex = data.SimulationMeshDescriptorIndex.Slice(start, count),

                PositionCorrections = data.PositionCorrections.Slice(start, count),
                PositionCorrectionsCount = data.PositionCorrectionsCount.Slice(start, count),
                Locks = data.ParticleLocks.Slice(start, count),
            };

            return tkYxQLRGrwbIYbYELXfgRrYDuwHgRAdI;
        }

        /// <summary>
        /// Slices the <see cref="RuntimeDistanceConstraintData"/>.
        /// </summary>
        /// <param name="data">Runtime Data.</param>
        /// <param name="start">Start index.</param>
        /// <param name="count">Count.</param>
        /// <returns>The slice.</returns>
        internal static RuntimeDistanceConstraintDataSlice Slice(this RuntimeDistanceConstraintData data, int start, int count)
        {
            Debug.Assert(start >= 0 && start + count <= data.Count, "The slice range is out of bounds.");

            var gZDeHhgmBwekgBRgVpbxczmhbFMXwPAR = new RuntimeDistanceConstraintDataSlice
            {
                Enabled = data.Enabled.Slice(start, count),
                Breakable = data.Breakable.Slice(start, count),

                PhysicsMaterialIndex = data.PhysicsMaterialIndex.Slice(start, count),

                ParticleA = data.ParticleA.Slice(start, count),
                ParticleB = data.ParticleB.Slice(start, count),

                RestDistance = data.RestDistance.Slice(start, count),

                SimulationMeshDescriptorIndex = data.SimulationMeshDescriptorIndex.Slice(start, count),
            };

            return gZDeHhgmBwekgBRgVpbxczmhbFMXwPAR;
        }

        /// <summary>
        /// Slices the <see cref="RuntimeVolumeConstraintData"/>.
        /// </summary>
        /// <param name="data">Runtime Data.</param>
        /// <param name="start">Start index.</param>
        /// <param name="count">Count.</param>
        /// <returns>The slice.</returns>
        internal static RuntimeVolumeConstraintDataSlice Slice(this RuntimeVolumeConstraintData data, int start, int count)
        {
            Debug.Assert(start >= 0 && start + count <= data.Count, "The slice range is out of bounds.");

            var YSALmmdxFAmLnRrRWQdgVMcGhXXoeGrO = new RuntimeVolumeConstraintDataSlice
            {
                Enabled = data.Enabled.Slice(start, count),
                Breakable = data.Breakable.Slice(start, count),

                PhysicsMaterialIndex = data.PhysicsMaterialIndex.Slice(start, count),

                ParticleA = data.ParticleA.Slice(start, count),
                ParticleB = data.ParticleB.Slice(start, count),
                ParticleC = data.ParticleC.Slice(start, count),
                ParticleD = data.ParticleD.Slice(start, count),

                RestVolume = data.RestVolume.Slice(start, count),

                SimulationMeshDescriptorIndex = data.SimulationMeshDescriptorIndex.Slice(start, count),
            };

            return YSALmmdxFAmLnRrRWQdgVMcGhXXoeGrO;
        }

        /// <summary>
        /// Slices the <see cref="RuntimeShapeMatchingConstraintData"/>.
        /// </summary>
        /// <param name="data">Runtime Data.</param>
        /// <param name="start">Start index.</param>
        /// <param name="count">Count.</param>
        /// <returns>The slice.</returns>
        internal static RuntimeShapeMatchingConstraintDataSlice Slice(this RuntimeShapeMatchingConstraintData data, int start, int count)
        {
            Debug.Assert(start >= 0 && start + count <= data.Count, "The slice range is out of bounds.");

            var GUYcoXgAOKgrnPUYfLKtdlUkgajQhgfH = new RuntimeShapeMatchingConstraintDataSlice
            {
                Enabled = data.Enabled.Slice(start, count),
                Breakable = data.Breakable.Slice(start, count),

                PhysicsMaterialIndex = data.PhysicsMaterialIndex.Slice(start, count),

                Particle = data.Particle.Slice(start, count),

                SimulationMeshDescriptorIndex = data.SimulationMeshDescriptorIndex.Slice(start, count),
            };

            return GUYcoXgAOKgrnPUYfLKtdlUkgajQhgfH;
        }

        /// <summary>
        /// Slices the <see cref="RuntimeCollinearConstraintData"/>.
        /// </summary>
        /// <param name="data">Runtime Data.</param>
        /// <param name="start">Start index.</param>
        /// <param name="count">Count.</param>
        /// <returns>The slice.</returns>
        internal static RuntimeCollinearConstraintDataSlice Slice(this RuntimeCollinearConstraintData data, int start, int count)
        {
            Debug.Assert(start >= 0 && start + count <= data.Count, "The slice range is out of bounds.");

            var SObbaECGDSWFlrbSWgHfhckaLlIHLxtV = new RuntimeCollinearConstraintDataSlice
            {
                Enabled = data.Enabled.Slice(start, count),
                Breakable = data.Breakable.Slice(start, count),

                PhysicsMaterialIndex = data.PhysicsMaterialIndex.Slice(start, count),

                ParticleA = data.ParticleA.Slice(start, count),
                ParticleB = data.ParticleB.Slice(start, count),
                ParticleC = data.ParticleC.Slice(start, count),

                SimulationMeshDescriptorIndex = data.SimulationMeshDescriptorIndex.Slice(start, count),
            };

            return SObbaECGDSWFlrbSWgHfhckaLlIHLxtV;
        }
    }
}
