namespace MAGES.MeshDeformations.Utilities
{
    using Unity.Mathematics;
    using UnityEngine;

    /// <summary>
    /// Debug utility class.
    /// </summary>
    public static class Debug
    {
        /// <summary>
        /// Draws a point in the scene view
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="size">The size.</param>
        public static void DrawPoint(in float3 position, float duration = 0f, float size = .1f)
        {
            UnityEngine.Debug.DrawLine(position - new float3(size, 0, 0), position + new float3(size, 0, 0), Color.red, duration);
            UnityEngine.Debug.DrawLine(position - new float3(0, size, 0), position + new float3(0, size, 0), Color.green, duration);
            UnityEngine.Debug.DrawLine(position - new float3(0, 0, size), position + new float3(0, 0, size), Color.blue, duration);
        }

        /// <summary>
        /// Draws an axis gizmo at the specified position.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="forward">Forward vector.</param>
        /// <param name="up">Up vector.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="size">Gizmo scale.</param>
        public static void DrawAxis(in float3 position, in float3 forward, in float3 up, in float duration = 0f, in float size = .1f)
        {
            var nQoXAavmvDHpIeAkMZrjGgHmNDoHWUIi = math.cross(forward, up);
            DrawAxis(position, forward, up, nQoXAavmvDHpIeAkMZrjGgHmNDoHWUIi, duration, size);
        }

        /// <summary>
        /// Draws an axis gizmo at the specified position.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="forward">Forward vector.</param>
        /// <param name="up">Up vector.</param>
        /// <param name="right">Right vector.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="size">Gizmo scale.</param>
        public static void DrawAxis(in float3 position, in float3 forward, in float3 up, in float3 right, in float duration = 0f, in float size = .1f)
        {
            UnityEngine.Debug.DrawLine(position, position + (size * math.normalize(forward)), Color.blue, duration);
            UnityEngine.Debug.DrawLine(position, position + (size * math.normalize(up)), Color.green, duration);
            UnityEngine.Debug.DrawLine(position, position + (size * math.normalize(right)), Color.red, duration);
        }

        public static void DrawAABB(in float3 min, in float3 max, in Color color, in float duration)
        {
            
            float3 lQtaIyaXTnareNuGbFdsDACPBHKRNfez = new float3(min.x, min.y, min.z);  
            float3 eCazvrnFEAcgFobGkKjtLEynQVJYQvip = new float3(max.x, min.y, min.z);  
            float3 XQmMzImesgtdTaFIMGTTdLOtdrBnGoVf = new float3(max.x, max.y, min.z);  
            float3 CeSJSECROJoeSMwnFHlRpMzYjaikeMVf = new float3(min.x, max.y, min.z);  

            float3 WlbvwWXeJGEvbpSNZAxeGGXXbIODsNFd = new float3(min.x, min.y, max.z);  
            float3 ggIWgLgYqbuhGIkqyYWOHFVbXprosfck = new float3(max.x, min.y, max.z);  
            float3 sXWbyHWlUnRzzgGhyBZwvHqdjFtkCHos = new float3(max.x, max.y, max.z);  
            float3 KeGowFhLbhUEKNSnRatJYgaTKznwCcMJ = new float3(min.x, max.y, max.z);  

            
            
            UnityEngine.Debug.DrawLine(lQtaIyaXTnareNuGbFdsDACPBHKRNfez, eCazvrnFEAcgFobGkKjtLEynQVJYQvip, color, duration);  
            UnityEngine.Debug.DrawLine(eCazvrnFEAcgFobGkKjtLEynQVJYQvip, XQmMzImesgtdTaFIMGTTdLOtdrBnGoVf, color, duration);  
            UnityEngine.Debug.DrawLine(XQmMzImesgtdTaFIMGTTdLOtdrBnGoVf, CeSJSECROJoeSMwnFHlRpMzYjaikeMVf, color, duration);  
            UnityEngine.Debug.DrawLine(CeSJSECROJoeSMwnFHlRpMzYjaikeMVf, lQtaIyaXTnareNuGbFdsDACPBHKRNfez, color, duration);  

            
            UnityEngine.Debug.DrawLine(WlbvwWXeJGEvbpSNZAxeGGXXbIODsNFd, ggIWgLgYqbuhGIkqyYWOHFVbXprosfck, color, duration);  
            UnityEngine.Debug.DrawLine(ggIWgLgYqbuhGIkqyYWOHFVbXprosfck, sXWbyHWlUnRzzgGhyBZwvHqdjFtkCHos, color, duration);  
            UnityEngine.Debug.DrawLine(sXWbyHWlUnRzzgGhyBZwvHqdjFtkCHos, KeGowFhLbhUEKNSnRatJYgaTKznwCcMJ, color, duration);  
            UnityEngine.Debug.DrawLine(KeGowFhLbhUEKNSnRatJYgaTKznwCcMJ, WlbvwWXeJGEvbpSNZAxeGGXXbIODsNFd, color, duration);  

            
            UnityEngine.Debug.DrawLine(lQtaIyaXTnareNuGbFdsDACPBHKRNfez, WlbvwWXeJGEvbpSNZAxeGGXXbIODsNFd, color, duration);  
            UnityEngine.Debug.DrawLine(eCazvrnFEAcgFobGkKjtLEynQVJYQvip, ggIWgLgYqbuhGIkqyYWOHFVbXprosfck, color, duration);  
            UnityEngine.Debug.DrawLine(XQmMzImesgtdTaFIMGTTdLOtdrBnGoVf, sXWbyHWlUnRzzgGhyBZwvHqdjFtkCHos, color, duration);  
            UnityEngine.Debug.DrawLine(CeSJSECROJoeSMwnFHlRpMzYjaikeMVf, KeGowFhLbhUEKNSnRatJYgaTKznwCcMJ, color, duration);  
        }

        /// <summary>
        /// Draws a tetrahedron in the scene.
        /// </summary>
        /// <param name="posA">Position of the first vertex.</param>
        /// <param name="posB">Position of the second vertex.</param>
        /// <param name="posC">Position of the third vertex.</param>
        /// <param name="posD">Position of the fourth vertex.</param>
        /// <param name="color">Color of the tetrahedron lines.</param>
        /// <param name="duration">Duration for which the lines will be visible.</param>
        public static void DrawTetrahedron(in float3 posA, in float3 posB, in float3 posC, in float3 posD, in Color color, in float duration)
        {
            
            UnityEngine.Debug.DrawLine(posA, posB, color, duration);
            UnityEngine.Debug.DrawLine(posA, posC, color, duration);
            UnityEngine.Debug.DrawLine(posA, posD, color, duration);
            UnityEngine.Debug.DrawLine(posB, posC, color, duration);
            UnityEngine.Debug.DrawLine(posB, posD, color, duration);
            UnityEngine.Debug.DrawLine(posC, posD, color, duration);
        }
    }
}