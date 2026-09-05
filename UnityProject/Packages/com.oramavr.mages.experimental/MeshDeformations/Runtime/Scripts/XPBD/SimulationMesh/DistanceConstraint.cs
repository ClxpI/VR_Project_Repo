namespace MAGES.MeshDeformations
{
    /// <summary>
    /// A distance constraint between two particles.
    /// </summary>
    [System.Serializable]
    public struct DistanceConstraint
    {
        /// <summary>
        /// Defines whether this constraint is enabled.
        /// </summary>
        public bool Enabled;

        /// <summary>
        /// Defines whether this constraint can be broken.
        /// </summary>
        public bool Breakable;

        /// <summary>
        /// Particle A index.
        /// </summary>
        public int ParticleA;

        /// <summary>
        /// Particle B index.
        /// </summary>
        public int ParticleB;

        /// <summary>
        /// Physics material index.
        /// </summary>
        public int PhysicsMaterialIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceConstraint"/> struct.
        /// </summary>
        /// <param name="particleA">ParticleA index.</param>
        /// <param name="particleB">ParticleB index.</param>
        public DistanceConstraint(int particleA, int particleB)
        {
            Enabled = true;
            Breakable = false;
            ParticleA = particleA;
            ParticleB = particleB;

            PhysicsMaterialIndex = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceConstraint"/> struct.
        /// </summary>
        /// <param name="particleA">ParticleA index.</param>
        /// <param name="particleB">ParticleB index.</param>
        /// <param name="physicsMaterial">Physics material index.</param>
        public DistanceConstraint(int particleA, int particleB, int physicsMaterial)
        {
            Enabled = true;
            Breakable = false;
            ParticleA = particleA;
            ParticleB = particleB;

            PhysicsMaterialIndex = physicsMaterial;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceConstraint"/> struct.
        /// </summary>
        /// <param name="particleA">ParticleA index.</param>
        /// <param name="particleB">ParticleB index.</param>
        /// <param name="breakable">Defines whether this constraint can be broken.</param>
        /// <param name="enabled">Whether this constraint is enabled.</param>
        public DistanceConstraint(int particleA, int particleB, bool breakable, bool enabled = true)
        {
            Enabled = true;
            Breakable = false;
            ParticleA = particleA;
            ParticleB = particleB;

            PhysicsMaterialIndex = 0;

            Breakable = breakable;
            Enabled = enabled;
        }
    }
}
