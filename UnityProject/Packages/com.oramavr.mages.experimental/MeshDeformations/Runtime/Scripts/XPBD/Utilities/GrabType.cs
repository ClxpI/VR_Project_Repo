namespace MAGES.MeshDeformations
{
    /// <summary>
    /// The grab type.
    /// </summary>
    /// <seealso cref="SimpleParticleGrabber"/>
    public enum GrabType
    {
        /// <summary>
        /// Grab the closest particle only.
        /// </summary>
        Closest,

        /// <summary>
        /// Grab all the particles in the grabber's radius.
        /// </summary>
        AllInRadius,
    }
}
