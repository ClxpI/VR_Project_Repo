namespace MAGES.MeshDeformations
{
    using Unity.Mathematics;

    /// <summary>
    /// A particle in the simulation mesh.
    /// </summary>
    [System.Serializable]
    public struct Particle
    {
        /// <summary>
        /// Defines whether the particle is enabled.
        /// </summary>
        public bool Enabled;

        /// <summary>
        /// Defines whether the particle can be broken(removed).
        /// </summary>
        public bool Breakable;

        /// <summary>
        /// Defines whether the particle is kinematic.
        /// </summary>
        public bool Kinematic;

        /// <summary>
        /// The inverse mass of the particle.
        /// </summary>
        public float InverseMass;

        /// <summary>
        /// The physics material index.
        /// </summary>
        public int PhysicsMaterialIndex;

        /// <summary>
        /// The position of the particle.
        /// </summary>
        public float3 Position;

        /// <summary>
        /// The velocity of the particle.
        /// </summary>
        public float3 Velocity;
    }
}
