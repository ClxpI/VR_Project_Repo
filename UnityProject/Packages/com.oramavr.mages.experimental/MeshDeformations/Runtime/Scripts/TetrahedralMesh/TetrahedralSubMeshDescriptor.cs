namespace MAGES.MeshDeformations
{
    /// <summary>
    /// SubMesh Descriptor for <see cref="TetrahedralMesh"/>.
    /// </summary>
    [System.Serializable]
    public struct TetrahedralSubMeshDescriptor
    {
        /// <summary>
        /// The index of the first tetrahedron index in the <see cref="TetrahedralMesh.Tetrahedra"/> array.
        /// </summary>
        public int IndexStart;

        /// <summary>
        /// The number of tetrahedra indices in the submesh.
        /// </summary>
        public int IndexCount;
    }
}
