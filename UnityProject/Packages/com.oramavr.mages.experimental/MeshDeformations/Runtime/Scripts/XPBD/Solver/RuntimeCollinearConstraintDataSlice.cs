namespace MAGES.MeshDeformations
{
    using Unity.Burst;
    using Unity.Collections;

    /// <summary>
    /// Slice of <see cref="RuntimeCollinearConstraintData"/>.
    /// </summary>
    [BurstCompile]
    public struct RuntimeCollinearConstraintDataSlice
    {
        /// <summary>
        /// Indicates whether each constraint is enabled.
        /// </summary>
        public NativeSlice<bool> Enabled;

        /// <summary>
        /// Indicates whether each constraint is breakable.
        /// </summary>
        public NativeSlice<bool> Breakable;

        /// <summary>
        /// The index of the physics material for each constraint.
        /// </summary>
        public NativeSlice<int> PhysicsMaterialIndex;

        /// <summary>
        /// The index of the first particle involved in each constraint.
        /// </summary>
        public NativeSlice<int> ParticleA;

        /// <summary>
        /// The index of the second particle involved in each constraint.
        /// </summary>
        public NativeSlice<int> ParticleB;

        /// <summary>
        /// The index of the third particle involved in each constraint.
        /// </summary>
        public NativeSlice<int> ParticleC;

        /// <summary>
        /// The index of the simulation mesh descriptor for each constraint.
        /// </summary>
        public NativeSlice<int> SimulationMeshDescriptorIndex;

        /// <summary>
        /// Gets the number of constraints.
        /// </summary>
        public int Count => Enabled.Length;
    }
}