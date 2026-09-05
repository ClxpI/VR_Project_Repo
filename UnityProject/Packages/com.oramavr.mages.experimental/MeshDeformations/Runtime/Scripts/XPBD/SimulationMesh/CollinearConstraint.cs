namespace MAGES.MeshDeformations
{
    /// <summary>
    /// A constraint that tries to keep 3 particles collinear.
    /// </summary>
    [System.Serializable]
    public struct CollinearConstraint
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
        /// Particle C index.
        /// </summary>
        public int ParticleC;

        /// <summary>
        /// Physics material index.
        /// </summary>
        public int PhysicsMaterialIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollinearConstraint"/> struct.
        /// </summary>
        /// <param name="particleA">ParticleA index.</param>
        /// <param name="particleB">ParticleB index.</param>
        /// <param name="particleC">ParticleC index.</param>
        public CollinearConstraint(int particleA, int particleB, int particleC)
        {
            Enabled = true;
            Breakable = false;
            ParticleA = particleA;
            ParticleB = particleB;
            ParticleC = particleC;

            PhysicsMaterialIndex = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollinearConstraint"/> struct.
        /// </summary>
        /// <param name="particleA">ParticleA index.</param>
        /// <param name="particleB">ParticleB index.</param>
        /// <param name="particleC">ParticleC index.</param>
        /// <param name="physicsMaterial">Physics material index.</param>
        public CollinearConstraint(int particleA, int particleB, int particleC, int physicsMaterial)
        {
            Enabled = true;
            Breakable = false;
            ParticleA = particleA;
            ParticleB = particleB;
            ParticleC = particleC;

            PhysicsMaterialIndex = physicsMaterial;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollinearConstraint"/> struct.
        /// </summary>
        /// <param name="particleA">ParticleA index.</param>
        /// <param name="particleB">ParticleB index.</param>
        /// <param name="particleC">ParticleC index.</param>
        /// <param name="breakable">Defines whether this constraint can be broken.</param>
        /// <param name="enabled">Whether this constraint is enabled.</param>
        public CollinearConstraint(int particleA, int particleB, int particleC, bool breakable, bool enabled = true)
        {
            Enabled = true;
            Breakable = false;
            ParticleA = particleA;
            ParticleB = particleB;
            ParticleC = particleC;

            PhysicsMaterialIndex = 0;

            Breakable = breakable;
            Enabled = enabled;
        }
    }
}
