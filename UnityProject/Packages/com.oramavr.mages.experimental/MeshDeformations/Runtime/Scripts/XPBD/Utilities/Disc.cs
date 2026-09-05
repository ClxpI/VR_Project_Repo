namespace MAGES.MeshDeformations
{
    using System.Diagnostics;
    using Unity.Burst;
    using Unity.Mathematics;

    /// <summary>
    /// A disc in 3D space.
    /// </summary>
    [BurstCompile]
    public struct Disc
    {
        /// <summary>
        /// The position of the disc.
        /// </summary>
        public float3 Position;

        /// <summary>
        /// The normal of the disc.
        /// </summary>
        public float3 Normal;

        /// <summary>
        /// The radius of the disc.
        /// </summary>
        public float Radius;

        /// <summary>
        /// Initializes a new instance of the <see cref="Disc"/> struct.
        /// </summary>
        /// <param name="position">The position of the disc.</param>
        /// <param name="normal">The normal of the disc.</param>
        /// <param name="radius">The radius of the disc.</param>
        public Disc(float3 position, float3 normal, float radius)
        {
            Position = position;
            Normal = normal;
            Radius = radius;
        }

        /// <summary>
        /// Gets the side of the point relative to the disc.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns><see langword="true"/> if the point is on the side the normal is facing.</returns>
        public bool GetSideOfPoint(float3 point)
        {
            var aaBZBSpLenDYEMgfVYZHiwjKdtEzbEjl = math.dot(Normal, point - Position);

            return aaBZBSpLenDYEMgfVYZHiwjKdtEzbEjl > 0;
        }

        /// <summary>
        /// Gets the intersection point of a line segment with the disc if exists.
        /// </summary>
        /// <param name="pointA">The first point defining the line segment.</param>
        /// <param name="pointB">The second point defining the line segment.</param>
        /// <param name="intersection">The interpolation factor t of the intersection point. IntersectionPoint = <paramref name="pointA"/> + t*(<paramref name="pointB"/> - <paramref name="pointA"/>)</param>
        /// <returns><see langword="true"/> if an intersection exists.</returns>
        public bool LineSegmentIntersection(float3 pointA, float3 pointB, out float3 intersection)
        {
            var QvcxbxWhVFbYEtErQisfRyXNZhhidbMg = math.dot(pointA - Position, Normal);
            var XyYZAkkfDcqFmDOWXxUxkMocAUjxTqhF = math.dot(pointB - Position, Normal);

            
            if (math.abs(QvcxbxWhVFbYEtErQisfRyXNZhhidbMg) <= math.EPSILON || math.abs(XyYZAkkfDcqFmDOWXxUxkMocAUjxTqhF) <= math.EPSILON)
            {
                UnityEngine.Debug.LogError("Coincident point on the disc.");
                intersection = float3.zero;
                return false;
            }

            
            if (math.sign(QvcxbxWhVFbYEtErQisfRyXNZhhidbMg) == math.sign(XyYZAkkfDcqFmDOWXxUxkMocAUjxTqhF))
            {
                intersection = float3.zero;
                return false;
            }

            intersection = Utilities.Math.LineSegmentPlaneIntersection(pointA, pointB, Position, Normal);

            
            float ierIvKWClbpTkVGzbXfvgMfmhqZhdAOQ = math.length(math.cross(Normal, intersection - Position));
            if (ierIvKWClbpTkVGzbXfvgMfmhqZhdAOQ > Radius)
            {
                return false;
            }

            return true;
        }
    }
}
