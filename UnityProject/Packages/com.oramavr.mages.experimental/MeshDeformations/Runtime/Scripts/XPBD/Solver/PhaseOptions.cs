namespace MAGES.MeshDeformations
{
    using System;

    /// <summary>
    /// Particle Phase options.
    /// </summary>
    [Flags]
    public enum PhaseOptions
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Self Collision Enabled.
        /// </summary>
        SelfCollisionEnabled = 1 << 31,

        /// <summary>
        /// Whether this particle can be used for cutting other meshes.
        /// </summary>
        CanCut = 1 << 30,
    }
}
