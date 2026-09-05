namespace MAGES.MeshDeformations.Editor
{
    /// <summary>
    /// Generation options for volume constraints.
    /// </summary>
    internal enum VolumeConstraintsGenerationOptions
    {
        /// <summary>
        /// Do not generate volume constraints.
        /// </summary>
        DontGenerate,

        /// <summary>
        /// Generate one volume constraint per tetrahedron.
        /// </summary>
        OnePerTetrahedron,
    }
}