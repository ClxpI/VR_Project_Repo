namespace MAGES.MeshDeformations
{
    /// <summary>
    /// A constraint trying to keep two particles a certain distance apart.
    /// </summary>
    public struct DistanceJointConstraint
    {
        /// <summary>
        /// ParticleA Index.
        /// </summary>
        public int ParticleA;

        /// <summary>
        /// ParticleB Index.
        /// </summary>
        public int ParticleB;

        /// <summary>
        /// ParticleC Index.
        /// </summary>
        public float RestLength;

        /// <summary>
        /// ParticleD Index.
        /// </summary>
        public float Compliance;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceJointConstraint"/> struct.
        /// </summary>
        /// <param name="particleA">The particle A index.</param>
        /// <param name="particleB">The particle B index.</param>
        /// <param name="restLength">The rest length.</param>
        public DistanceJointConstraint(int particleA, int particleB, float restLength)
        {
            ParticleA = particleA;
            ParticleB = particleB;
            RestLength = restLength;
            Compliance = 0.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceJointConstraint"/> struct.
        /// </summary>
        /// <param name="particleA">The particle A index.</param>
        /// <param name="particleB">The particle B index.</param>
        /// <param name="restLength">The rest length.</param>
        /// <param name="compliance">The constraint compliance.</param>
        public DistanceJointConstraint(int particleA, int particleB, float restLength, float compliance)
        {
            ParticleA = particleA;
            ParticleB = particleB;
            RestLength = restLength;
            Compliance = compliance;
        }
    }
}