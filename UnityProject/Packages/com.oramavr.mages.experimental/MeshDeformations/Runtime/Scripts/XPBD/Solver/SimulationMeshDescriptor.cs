namespace MAGES.MeshDeformations
{
    using Unity.Burst;

    /// <summary>
    /// A descriptor for runtime data of simulation meshes.
    /// </summary>
    [BurstCompile]
    internal struct SimulationMeshDescriptor
    {
        /// <summary>
        /// The starting index of particles for this simulation mesh.
        /// </summary>
        public int ParticleStartIndex;

        /// <summary>
        /// The count of particles in the simulation mesh.
        /// </summary>
        public int ParticleCount;

        /// <summary>
        /// The total available space for new particle data before resizing is required.
        /// </summary>
        public int TotalParticleSpaceCount;

        /// <summary>
        /// The starting index of distance constraints for this simulation mesh.
        /// </summary>
        public int DistanceConstraintStartIndex;

        /// <summary>
        /// The count of distance constraints in the simulation mesh.
        /// </summary>
        public int DistanceConstraintCount;

        /// <summary>
        /// The total available space for new distance constraint data before resizing is required.
        /// </summary>
        public int TotalDistanceConstraintSpaceCount;

        /// <summary>
        /// The starting index of volume constraints for this simulation mesh.
        /// </summary>
        public int VolumeConstraintStartIndex;

        /// <summary>
        /// The count of volume constraints in the simulation mesh.
        /// </summary>
        public int VolumeConstraintCount;

        /// <summary>
        /// The total available space for new volume constraint data before resizing is required.
        /// </summary>
        public int TotalVolumeConstraintSpaceCount;

        /// <summary>
        /// The starting index of shape matching constraints for this simulation mesh.
        /// </summary>
        public int ShapeMatchingConstraintStartIndex;

        /// <summary>
        /// The count of shape matching constraints in the simulation mesh.
        /// </summary>
        public int ShapeMatchingConstraintCount;

        /// <summary>
        /// The total available space for new shape matching constraint data before resizing is required.
        /// </summary>
        public int TotalShapeMatchingConstraintSpaceCount;

        /// <summary>
        /// The starting index of collinear constraints for this simulation mesh.
        /// </summary>
        public int CollinearConstraintStartIndex;

        /// <summary>
        /// The count of collinear constraints in the simulation mesh.
        /// </summary>
        public int CollinearConstraintCount;

        /// <summary>
        /// The total available space for new collinear constraints data before resizing is required.
        /// </summary>
        public int TotalCollinearConstraintSpaceCount;

        /// <summary>
        /// The starting index of physics materials for this simulation mesh.
        /// </summary>
        public int PhysicsMaterialStartIndex;

        /// <summary>
        /// The count of physics materials in the simulation mesh.
        /// </summary>
        public int PhysicsMaterialCount;

        /// <summary>
        /// The total available space for new physics material data before resizing is required.
        /// </summary>
        public int TotalPhysicsMaterialSpaceCount;

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"ParticleStartIndex: {ParticleStartIndex}, ParticleCount: {ParticleCount}, TotalParticleSpaceCount: {TotalParticleSpaceCount}, " +
                   $"DistanceConstraintStartIndex: {DistanceConstraintStartIndex}, DistanceConstraintCount: {DistanceConstraintCount}, TotalDistanceConstraintSpaceCount: {TotalDistanceConstraintSpaceCount}, " +
                   $"VolumeConstraintStartIndex: {VolumeConstraintStartIndex}, VolumeConstraintCount: {VolumeConstraintCount}, TotalVolumeConstraintSpaceCount: {TotalVolumeConstraintSpaceCount}, " +
                   $"ShapeMatchingConstraintStartIndex: {ShapeMatchingConstraintStartIndex}, ShapeMatchingConstraintCount: {ShapeMatchingConstraintCount}, TotalShapeMatchingConstraintSpaceCount: {TotalShapeMatchingConstraintSpaceCount}, " +
                   $"PhysicsMaterialStartIndex: {PhysicsMaterialStartIndex}, PhysicsMaterialCount: {PhysicsMaterialCount}, TotalPhysicsMaterialSpaceCount: {TotalPhysicsMaterialSpaceCount}";
        }
    }
}
