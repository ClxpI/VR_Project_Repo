namespace MAGES.MeshDeformations
{
    /// <summary>
    /// A shape matching constraint for a particle.
    /// </summary>
    [System.Serializable]
    public struct ShapeMatchingConstraint
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
        /// Particle index.
        /// </summary>
        public int Particle;

        /// <summary>
        /// Physics material index.
        /// </summary>
        public int PhysicsMaterialIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeMatchingConstraint"/> struct.
        /// </summary>
        /// <param name="particle">The particle index.</param>
        public ShapeMatchingConstraint(int particle)
        {
            Enabled = true;
            Breakable = false;
            Particle = particle;

            PhysicsMaterialIndex = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeMatchingConstraint"/> struct.
        /// </summary>
        /// <param name="particle">The particle index.</param>
        /// <param name="physicsMaterial">Physics material index.</param>
        public ShapeMatchingConstraint(int particle, int physicsMaterial)
        {
            Enabled = true;
            Breakable = false;
            Particle = particle;

            PhysicsMaterialIndex = physicsMaterial;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeMatchingConstraint"/> struct.
        /// </summary>
        /// <param name="particle">The particle index.</param>
        /// <param name="breakable">Defines whether this constraint can be broken.</param>
        /// <param name="enabled">Whether this constraint is enabled.</param>
        public ShapeMatchingConstraint(int particle, bool breakable, bool enabled = true)
        {
            Enabled = true;
            Breakable = false;
            Particle = particle;

            PhysicsMaterialIndex = 0;

            Breakable = breakable;
            Enabled = enabled;
        }
    }
}