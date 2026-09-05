namespace MAGES.MeshDeformations
{
    using Unity.Mathematics;

    /// <summary>
    /// A pair of particles in contact. Used for particle - particle collision solving.
    /// </summary>
    public struct ContactPair
    {
        /// <summary>
        /// The particle for which this pair is generated.
        /// </summary>
        public int Particle;

        /// <summary>
        /// The other particle that is contacting this.
        /// </summary>
        public int OtherParticle;

        /// <summary>
        /// The point of contact.
        /// </summary>
        public float3 ContactPoint;    

        /// <summary>
        /// The distance between the particles at contact point.
        /// </summary>
        public float Separation;       

        /// <summary>
        /// The contact normal.
        /// </summary>
        public float3 ContactNormal; 
    }
}