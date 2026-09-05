namespace MAGES.MeshDeformations
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Mathematics;

    /// <summary>
    /// Runtime particle data.
    /// </summary>
    [BurstCompile]
    public struct RuntimeParticleDataSlice
    {
        /// <summary>
        /// Enabled array slice.
        /// </summary>
        public NativeSlice<bool> Enabled;

        /// <summary>
        /// Breakable array slice.
        /// </summary>
        public NativeSlice<bool> Breakable;

        /// <summary>
        /// PhysicsMaterialIndex array slice.
        /// </summary>
        public NativeSlice<int> PhysicsMaterialIndex;

        /// <summary>
        /// Initial particle positions.
        /// </summary>
        public NativeSlice<float3> InitialPosition;

        /// <summary>
        /// Current particle positions.
        /// </summary>
        public NativeSlice<float3> Position;

        /// <summary>
        /// Particle velocities.
        /// </summary>
        public NativeSlice<float3> Velocity;

        /// <summary>
        /// Inverse mass of particles.
        /// </summary>
        public NativeSlice<float> InverseMass;

        /// <summary>
        /// Adjusted inverse mass of particles.
        /// </summary>
        public NativeSlice<float> AdjustedInverseMass;

        /// <summary>
        /// The particles phase.
        /// </summary>
        public NativeSlice<uint> Phase;

        /// <summary>
        /// The simulation mesh descriptor index.
        /// </summary>
        public NativeSlice<int> SimulationMeshDescriptorIndex;

        /// <summary>
        /// The position corrections buffer.
        /// </summary>
        public NativeSlice<float3> PositionCorrections;

        /// <summary>
        /// The position corrections count.
        /// </summary>
        public NativeSlice<int> PositionCorrectionsCount;

        /// <summary>
        /// The particle locks used for atomic operations.
        /// </summary>
        public NativeSlice<int> Locks;

        /// <summary>
        /// Gets the particle data count.
        /// </summary>
        public int Count => Enabled.Length;
    }
}
