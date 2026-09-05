namespace MAGES.MeshDeformations
{
    /// <summary>
    /// A tetrahedral volume constraint between four particles.
    /// </summary>
    [System.Serializable]
    public struct VolumeConstraint
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
        /// Particle D index.
        /// </summary>
        public int ParticleD;

        /// <summary>
        /// Physics material index.
        /// </summary>
        public int PhysicsMaterialIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeConstraint"/> struct.
        /// </summary>
        /// <param name="particleA">ParticleA index.</param>
        /// <param name="particleB">ParticleB index.</param>
        /// <param name="particleC">ParticleC index.</param>
        /// <param name="particleD">ParticleD index.</param>
        public VolumeConstraint(int particleA, int particleB, int particleC, int particleD)
        {
            Enabled = true;
            Breakable = false;
            ParticleA = particleA;
            ParticleB = particleB;
            ParticleC = particleC;
            ParticleD = particleD;

            PhysicsMaterialIndex = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeConstraint"/> struct.
        /// </summary>
        /// <param name="particleA">ParticleA index.</param>
        /// <param name="particleB">ParticleB index.</param>
        /// <param name="particleC">ParticleC index.</param>
        /// <param name="particleD">ParticleD index.</param>
        /// <param name="physicsMaterial">Physics material index.</param>
        public VolumeConstraint(int particleA, int particleB, int particleC, int particleD, int physicsMaterial)
        {
            Enabled = true;
            Breakable = false;
            ParticleA = particleA;
            ParticleB = particleB;
            ParticleC = particleC;
            ParticleD = particleD;

            PhysicsMaterialIndex = physicsMaterial;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeConstraint"/> struct.
        /// </summary>
        /// <param name="particleA">ParticleA index.</param>
        /// <param name="particleB">ParticleB index.</param>
        /// <param name="particleC">ParticleC index.</param>
        /// <param name="particleD">ParticleD index.</param>
        /// <param name="breakable">Defines whether this constraint can be broken.</param>
        /// <param name="enabled">Whether this constraint is enabled.</param>
        public VolumeConstraint(int particleA, int particleB, int particleC, int particleD, bool breakable, bool enabled = true)
        {
            Enabled = true;
            Breakable = false;
            ParticleA = particleA;
            ParticleB = particleB;
            ParticleC = particleC;
            ParticleD = particleD;

            PhysicsMaterialIndex = 0;

            Breakable = breakable;
            Enabled = enabled;
        }
    }
}
