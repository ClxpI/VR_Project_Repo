namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Mathematics;
    using UnityEngine;

    /// <summary>
    /// A mesh containing tetrahedra.
    /// </summary>
    [CreateAssetMenu(menuName = "MAGES/TetrahedralMesh")]
    public class TetrahedralMesh : ScriptableObject
    {
        [SerializeField]
        private float3[] vertices = new float3[0];

        [SerializeField]
        private int[] tetrahedra = new int[0];

        [SerializeField]
        [HideInInspector]
        private bool neighborsDirty = false;

        [SerializeField]
        private int[] neighbors = new int[0];

        [SerializeField]
        [HideInInspector]
        private TetrahedralSubMeshDescriptor[] subMeshDescriptors = new TetrahedralSubMeshDescriptor[0];

        /// <summary>
        /// Gets the vertex count.
        /// </summary>
        public int VertexCount => vertices.Length;

        /// <summary>
        /// Gets the vertices.
        /// </summary>
        public float3[] Vertices => vertices;

        /// <summary>
        /// Gets the tetrahedra.
        /// </summary>
        public int[] Tetrahedra => tetrahedra;

        /// <summary>
        /// Gets the tetrahedra neighbors.
        /// </summary>
        public int[] Neighbors
        {
            get
            {
                if (neighborsDirty)
                {
                    RecalculateNeighbors();
                }

                return neighbors;
            }
        }

        /// <summary>
        /// Gets or sets the SubMesh count.
        /// </summary>
        public int SubMeshCount
        {
            get
            {
                return math.max(subMeshDescriptors.Length, 1);
            }

            set
            {
                var VDkZpTJXEjbOeNUBnDsbFEgEqhVdimev = new TetrahedralSubMeshDescriptor[value];
                subMeshDescriptors.CopyTo(VDkZpTJXEjbOeNUBnDsbFEgEqhVdimev, 0);
                subMeshDescriptors = VDkZpTJXEjbOeNUBnDsbFEgEqhVdimev;
            }
        }

        /// <summary>
        /// Clears the mesh.
        /// </summary>
        public void Clear()
        {
            vertices = null;
            tetrahedra = null;
            neighbors = null;
            neighborsDirty = false;
        }

        /// <summary>
        /// Sets the mesh vertices.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        public void SetVertices(float3[] vertices)
        {
            Clear();
            this.vertices = vertices;
        }

        /// <summary>
        /// Sets the tetrahedra of the mesh.
        /// </summary>
        /// <param name="tetrahedra">The tetrahedra.</param>
        /// <param name="recalculateNeighbors">Whether or not to recalculate neighbors.</param>
        public void SetTetrahedra(int[] tetrahedra, bool recalculateNeighbors = true)
        {
            UnityEngine.Debug.Assert(RBNjGjnGFkybRycBIvHqroRaSduAyWZD(tetrahedra), "Some vertex indices are out of bounds. Could not set tetrahedra of {name}");

            
            this.tetrahedra = tetrahedra;

            
            neighborsDirty = true;
            if (recalculateNeighbors)
            {
                RecalculateNeighbors();
            }
        }

        /// <summary>
        /// Sets a sub mesh.
        /// </summary>
        /// <param name="submeshIndex">The sub mesh index.</param>
        /// <param name="desc">The sub mesh descriptor.</param>
        public void SetSubMesh(int submeshIndex, TetrahedralSubMeshDescriptor desc)
        {
            UnityEngine.Debug.Assert(submeshIndex >= 0 && submeshIndex < SubMeshCount, "Invalid submeshIndex. Increase SubMeshCount before adding a new submesh.");
            subMeshDescriptors[submeshIndex] = desc;
        }

        /// <summary>
        /// Gets the submesh with the given index.
        /// </summary>
        /// <param name="index">The submesh index.</param>
        /// <returns>The submesh with the given index.</returns>
        public TetrahedralSubMeshDescriptor GetSubMesh(int index)
        {
            UnityEngine.Debug.Assert(index >= 0 && index < SubMeshCount, "Invalid submeshIndex.");

            if (subMeshDescriptors.Length == 0)
            {
                return new TetrahedralSubMeshDescriptor()
                {
                    IndexStart = 0,
                    IndexCount = tetrahedra.Length,
                };
            }

            return subMeshDescriptors[index];
        }

        /// <summary>
        /// Recalculates tetrahedron neighbors.
        /// </summary>
        [ContextMenu("Recalculate Neighbors")]
        public void RecalculateNeighbors()
        {
            neighborsDirty = false;
            neighbors = MeshUtilities.FindTetrahedronNeighbors(tetrahedra);
        }

        private bool RBNjGjnGFkybRycBIvHqroRaSduAyWZD(in int[] tetrahedra)
        {
            for (int iWEFzTBXsharQGLfJgURzSgGebpafyFv = 0; iWEFzTBXsharQGLfJgURzSgGebpafyFv < tetrahedra.Length; iWEFzTBXsharQGLfJgURzSgGebpafyFv++)
            {
                if (!HUqJLByLLgHLAIuUzrQoKAlNsaxAxAIt(tetrahedra[iWEFzTBXsharQGLfJgURzSgGebpafyFv]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool HUqJLByLLgHLAIuUzrQoKAlNsaxAxAIt(int index)
        {
            return index >= 0 && index < vertices.Length;
        }
    }
}
