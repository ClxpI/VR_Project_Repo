namespace MAGES.MeshDeformations
{
    using Unity.Burst;
    using Unity.Collections;

    /// <summary>
    /// Runtime volume constraint data.
    /// </summary>
    [BurstCompile]
    public struct RuntimeVolumeConstraintDataSlice
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
        /// Particle A array slice.
        /// </summary>
        public NativeSlice<int> ParticleA;

        /// <summary>
        /// Particle B array slice.
        /// </summary>
        public NativeSlice<int> ParticleB;

        /// <summary>
        /// Particle C array slice.
        /// </summary>
        public NativeSlice<int> ParticleC;

        /// <summary>
        /// Particle D array slice.
        /// </summary>
        public NativeSlice<int> ParticleD;

        /// <summary>
        /// The volume of the constraints at rest configuration.
        /// </summary>
        public NativeSlice<float> RestVolume;

        /// <summary>
        /// The simulation mesh descriptor index.
        /// </summary>
        public NativeSlice<int> SimulationMeshDescriptorIndex;

        /// <summary>
        /// Gets the data count.
        /// </summary>
        public int Count => Enabled.Length;
    }
}
