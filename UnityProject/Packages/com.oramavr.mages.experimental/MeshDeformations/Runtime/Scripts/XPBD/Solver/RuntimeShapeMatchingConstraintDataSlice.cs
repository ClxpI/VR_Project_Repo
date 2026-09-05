namespace MAGES.MeshDeformations
{
    using Unity.Collections;

    /// <summary>
    /// Slice of <see cref="RuntimeShapeMatchingConstraintData"/>.
    /// </summary>
    public struct RuntimeShapeMatchingConstraintDataSlice
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
        /// Particle array slice.
        /// </summary>
        public NativeSlice<int> Particle;

        /// <summary>
        /// SimulationMeshDescriptorIndex array slice.
        /// </summary>
        public NativeSlice<int> SimulationMeshDescriptorIndex;

        /// <summary>
        /// Gets the slice size.
        /// </summary>
        public int Count => Enabled.Length;
    }
}
