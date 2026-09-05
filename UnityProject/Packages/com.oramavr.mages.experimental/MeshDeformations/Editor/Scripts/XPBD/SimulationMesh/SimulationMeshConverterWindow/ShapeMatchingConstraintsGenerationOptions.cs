namespace MAGES.MeshDeformations.Editor
{
    /// <summary>
    /// Options for the generation of shape matching constraints.
    /// </summary>
    internal enum ShapeMatchingConstraintsGenerationOptions
    {
        /// <summary>
        /// Do not generate shape matching constraints.
        /// </summary>
        DontGenerate,

        /// <summary>
        /// Generate one shape matching constraint per particle.
        /// </summary>
        OnePerParticle,
    }
}