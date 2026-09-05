namespace MAGES.MeshDeformations.Editor
{
    /// <summary>
    /// Options for generating distance constraints.
    /// </summary>
    internal enum DistanceConstraintsGenerationOptions
    {
        /// <summary>
        /// Dont generate distance constraints.
        /// </summary>
        DontGenerate,

        /// <summary>
        /// Generate distance constraints for all pairs of vertices.
        /// </summary>
        OnePerEdge,
    }
}
