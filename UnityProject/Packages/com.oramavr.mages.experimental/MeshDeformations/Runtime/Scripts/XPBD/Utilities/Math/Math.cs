namespace MAGES.MeshDeformations.Utilities
{
    using System;
    using System.Runtime.CompilerServices;
    using Unity.Burst;
    using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.Assertions;
    using UnityEngine.EventSystems;

    /// <summary>
    /// A collection of math utility functions.
    /// </summary>
    public static class Math
    {
        /// <summary>
        /// Calculates the outer product of two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The outer product matrix.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3x3 OuterProduct(float3 a, float3 b)
        {
            return new float3x3(b.x * a, b.y * a, b.z * a);
        }

        /// <summary>
        /// Calculates the volume of a tetrahedron defined by four points.
        /// </summary>
        /// <param name="p1">The first point of the tetrahedron.</param>
        /// <param name="p2">The second point of the tetrahedron.</param>
        /// <param name="p3">The third point of the tetrahedron.</param>
        /// <param name="p4">The fourth point of the tetrahedron.</param>
        /// <returns>The volume of the tetrahedron.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TetrahedronVolume(in float3 p1, in float3 p2, in float3 p3, in float3 p4)
        {
            
            return math.dot(math.cross(p3 - p1, p2 - p1), p4 - p1) / 6.0f;
        }

        /// <summary>
        /// Determines if a point is inside a tetrahedron.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <param name="p0">The first vertex of the tetrahedron.</param>
        /// <param name="p1">The second vertex of the tetrahedron.</param>
        /// <param name="p2">The third vertex of the tetrahedron.</param>
        /// <param name="p3">The fourth vertex of the tetrahedron.</param>
        /// <returns>True if the point is inside the tetrahedron; otherwise, false.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPointInsideTetrahedron(in float3 point, in float3 p0, in float3 p1, in float3 p2, in float3 p3)
        {
            return ArePointsOnTheSameSideOfTriangle(point, p2, p0, p1, p3) &&
                   ArePointsOnTheSameSideOfTriangle(point, p0, p1, p2, p3) &&
                   ArePointsOnTheSameSideOfTriangle(point, p1, p2, p0, p3) &&
                   ArePointsOnTheSameSideOfTriangle(point, p3, p0, p2, p1);
         }

        /// <summary>
        /// Calculates the barycentric coordinates of a point within a tetrahedron.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <param name="t0">The first vertex of the tetrahedron.</param>
        /// <param name="t1">The second vertex of the tetrahedron.</param>
        /// <param name="t2">The third vertex of the tetrahedron.</param>
        /// <param name="t3">The fourth vertex of the tetrahedron.</param>
        /// <param name="barycentricCoordinates">The barycentric coordinates of the point within the tetrahedron.</param>
        /// <returns><see langword="true"/> if barycentric coordinates were calculated successfully.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TetrahedronBarycentricCoordinates(in float3 point, in float3 t0, in float3 t1, in float3 t2, in float3 t3, out float4 barycentricCoordinates)
        {
            
            float3 UQOTeZLCUmrFqoySHrnERamLfRRpEKAc = t1 - t0;
            float3 NTctAPjZUtOdcyMDeCzYByMCPZKKytUM = t2 - t0;
            float3 BxjoUsxiwghpNNJKLkACArrkBmomoDVB = t3 - t0;

            float3x3 MZBjEXFjoCFacFDFyWXEJAZfkJzNFDRp = new float3x3
            {
                c0 = new float3(t0 - t3),
                c1 = new float3(t1 - t3),
                c2 = new float3(t2 - t3),
            };

            if (math.abs(math.determinant(MZBjEXFjoCFacFDFyWXEJAZfkJzNFDRp)) < math.EPSILON)
            {
                
                if (math.distancesq(point, t0) < math.EPSILON)
                {
                    barycentricCoordinates = new float4(1.0f, 0, 0, 0);
                    return true;
                }

                if (math.distancesq(point, t1) < math.EPSILON)
                {
                    barycentricCoordinates = new float4(0f, 1.0f, 0, 0);
                    return true;
                }

                if (math.distancesq(point, t2) < math.EPSILON)
                {
                    barycentricCoordinates = new float4(0f, 0, 1.0f, 0);
                    return true;
                }

                if (math.distancesq(point, t3) < math.EPSILON)
                {
                    barycentricCoordinates = new float4(0f, 0, 0, 1.0f);
                    return true;
                }

                barycentricCoordinates = new float4(0, 0, 0, 0);
                return false;
            }

            MZBjEXFjoCFacFDFyWXEJAZfkJzNFDRp = math.inverse(MZBjEXFjoCFacFDFyWXEJAZfkJzNFDRp);

            float3 uzsOmSBfCWwsLGLYHuoayUzLYpBCFJCS = math.mul(MZBjEXFjoCFacFDFyWXEJAZfkJzNFDRp, point - t3);
            barycentricCoordinates = new float4(uzsOmSBfCWwsLGLYHuoayUzLYpBCFJCS, 1.0f - uzsOmSBfCWwsLGLYHuoayUzLYpBCFJCS.x - uzsOmSBfCWwsLGLYHuoayUzLYpBCFJCS.y - uzsOmSBfCWwsLGLYHuoayUzLYpBCFJCS.z);

            return true;
        }

        /// <summary>
        /// Interpolates a point within a tetrahedron using barycentric coordinates.
        /// </summary>
        /// <param name="p0">The first vertex of the tetrahedron.</param>
        /// <param name="p1">The second vertex of the tetrahedron.</param>
        /// <param name="p2">The third vertex of the tetrahedron.</param>
        /// <param name="p3">The fourth vertex of the tetrahedron.</param>
        /// <param name="barycentricCoordinates">The barycentric coordinates.</param>
        /// <returns>The interpolated point within the tetrahedron.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 TetrahedronBarycentricInterpolation(in float3 p0, in float3 p1, in float3 p2, in float3 p3, float4 barycentricCoordinates)
        {
            return (barycentricCoordinates.x * p0) + (barycentricCoordinates.y * p1) + (barycentricCoordinates.z * p2) + (barycentricCoordinates.w * p3);
        }

        /// <summary>
        /// Calculates the barycentric coordinates of a point within a triangle.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <param name="triA">The first vertex of the triangle.</param>
        /// <param name="triB">The second vertex of the triangle.</param>
        /// <param name="triC">The third vertex of the triangle.</param>
        /// <returns>The barycentric coordinates of the point within the triangle.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 TriangleBarycentricCoordinates(float3 point, float3 triA, float3 triB, float3 triC)
        {
            
            float3 rLuaZBmaRflZoPDxosGZUmLXVgAQvYEo = triB - triA;
            float3 cWcuBUHOcciFvUTpWeRVBtQYAtrKBBMG = triC - triA;
            float3 lYjEfqiZNbOEeQOuPtBjJCVKXUHqTRza = point - triA;

            
            float pGgXlhHdtfnwVtZLqhQAzcUfNbPKhnLX = math.dot(rLuaZBmaRflZoPDxosGZUmLXVgAQvYEo, rLuaZBmaRflZoPDxosGZUmLXVgAQvYEo);
            float QqIdlWdZKFjCBmYqcnbqPvjEiLKQsDzi = math.dot(rLuaZBmaRflZoPDxosGZUmLXVgAQvYEo, cWcuBUHOcciFvUTpWeRVBtQYAtrKBBMG);
            float nVUZRbUZaACWchngqBXYWBBGkNksfUQU = math.dot(cWcuBUHOcciFvUTpWeRVBtQYAtrKBBMG, cWcuBUHOcciFvUTpWeRVBtQYAtrKBBMG);
            float QHpwOEirvOhkkfgsuuTBRnhDJpmfdVsM = math.dot(lYjEfqiZNbOEeQOuPtBjJCVKXUHqTRza, rLuaZBmaRflZoPDxosGZUmLXVgAQvYEo);
            float FTyTKRQOinQJojfvfFvgvKXOxsIgozmQ = math.dot(lYjEfqiZNbOEeQOuPtBjJCVKXUHqTRza, cWcuBUHOcciFvUTpWeRVBtQYAtrKBBMG);

            
            float toIoWKXGRZUvbWJvUCqXaxAahnozQqWP = (pGgXlhHdtfnwVtZLqhQAzcUfNbPKhnLX * nVUZRbUZaACWchngqBXYWBBGkNksfUQU) - (QqIdlWdZKFjCBmYqcnbqPvjEiLKQsDzi * QqIdlWdZKFjCBmYqcnbqPvjEiLKQsDzi);

            
            float muaiHkoZbSnxJFLeJajDXLUpignpUGre = ((nVUZRbUZaACWchngqBXYWBBGkNksfUQU * QHpwOEirvOhkkfgsuuTBRnhDJpmfdVsM) - (QqIdlWdZKFjCBmYqcnbqPvjEiLKQsDzi * FTyTKRQOinQJojfvfFvgvKXOxsIgozmQ)) / toIoWKXGRZUvbWJvUCqXaxAahnozQqWP;
            float hsWXZqpLVxSoDoLPIwHaOjpAqugjuUGG = ((pGgXlhHdtfnwVtZLqhQAzcUfNbPKhnLX * FTyTKRQOinQJojfvfFvgvKXOxsIgozmQ) - (QqIdlWdZKFjCBmYqcnbqPvjEiLKQsDzi * QHpwOEirvOhkkfgsuuTBRnhDJpmfdVsM)) / toIoWKXGRZUvbWJvUCqXaxAahnozQqWP;
            float xnGKJCcSspCbycmIdctiVHRcgQzQTmpL = 1.0f - muaiHkoZbSnxJFLeJajDXLUpignpUGre - hsWXZqpLVxSoDoLPIwHaOjpAqugjuUGG;

            return new float3(xnGKJCcSspCbycmIdctiVHRcgQzQTmpL, muaiHkoZbSnxJFLeJajDXLUpignpUGre, hsWXZqpLVxSoDoLPIwHaOjpAqugjuUGG);
        }

        /// <summary>
        /// Interpolates a point within a triangle using barycentric coordinates.
        /// </summary>
        /// <param name="p0">The first vertex of the triangle.</param>
        /// <param name="p1">The second vertex of the triangle.</param>
        /// <param name="p2">The third vertex of the triangle.</param>
        /// <param name="barycentricCoordinates">The barycentric coordinates.</param>
        /// <returns>The interpolated point within the triangle.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 TriangleBarycentricInterpolation(in float3 p0, in float3 p1, in float3 p2, in float3 barycentricCoordinates)
        {
            return (barycentricCoordinates.x * p0) + (barycentricCoordinates.y * p1) + (barycentricCoordinates.z * p2);
        }

        /// <summary>
        /// Checks if two points are on the same side of a triangle defined by three vertices.
        /// </summary>
        /// <param name="p1">The first point to test.</param>
        /// <param name="p2">The second point to test.</param>
        /// <param name="tri1">The first vertex of the triangle.</param>
        /// <param name="tri2">The second vertex of the triangle.</param>
        /// <param name="tri3">The third vertex of the triangle.</param>
        /// <returns>True if both points are on the same side of the triangle; otherwise, false.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ArePointsOnTheSameSideOfTriangle(float3 p1, float3 p2, float3 tri1, float3 tri2, float3 tri3)
        {
            var CLlsyymEOuFvkStCWvvsaxPaKBXMgATL = math.cross(tri2 - tri1, tri3 - tri1);
            var teQizsibEXzXnsLuSXunGBvAukDZkFQq = math.dot(CLlsyymEOuFvkStCWvvsaxPaKBXMgATL, p2 - tri1);
            var nCplXYjgsCTCmqqOmSRvHoJlYrvpBmgi = math.dot(CLlsyymEOuFvkStCWvvsaxPaKBXMgATL, p1 - tri1);

            return math.sign(teQizsibEXzXnsLuSXunGBvAukDZkFQq) == math.sign(nCplXYjgsCTCmqqOmSRvHoJlYrvpBmgi) || (math.abs(nCplXYjgsCTCmqqOmSRvHoJlYrvpBmgi) <= math.EPSILON) || (math.abs(teQizsibEXzXnsLuSXunGBvAukDZkFQq) <= math.EPSILON);
        }

        /// <summary>
        /// Determines which side of a triangle a point is on.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <param name="tri1">The first vertex of the triangle.</param>
        /// <param name="tri2">The second vertex of the triangle.</param>
        /// <param name="tri3">The third vertex of the triangle.</param>
        /// <returns>True if the point is on the same side as the normal of the triangle; otherwise, false.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetSideOfTriangle(float3 point, float3 tri1, float3 tri2, float3 tri3)
        {
            var fopQNnGNnFbGYuvfvKrckVAREWtkjloG = math.cross(tri2 - tri1, tri3 - tri1);
            var PVvjzOjIJWtZqZvhJocaTpBjoydkxKuQ = math.dot(fopQNnGNnFbGYuvfvKrckVAREWtkjloG, point - tri1);

            return math.sign(PVvjzOjIJWtZqZvhJocaTpBjoydkxKuQ) > 0;
        }

        /// <summary>
        /// Determines if a point is inside the triangle defined by three vertices.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <param name="v0">The first vertex of the triangle.</param>
        /// <param name="v1">The second vertex of the triangle.</param>
        /// <param name="v2">The third vertex of the triangle.</param>
        /// <returns>True if the point is inside the triangle; otherwise, false.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPointInsideTriangle(float3 point, float3 v0, float3 v1, float3 v2)
        {
            
            float3 jsMzVkJKKNgclucIFXBQesAlvLtOAoIr = math.cross(v1 - v0, point - v0);
            float3 kokHdPrCpxuFzxrjBnvzOdFOKptXUpuh = math.cross(v2 - v1, point - v1);
            float3 CyhHjAASJbmGmfInqvgQwWOkJSBlEtOj = math.cross(v0 - v2, point - v2);

            return math.dot(jsMzVkJKKNgclucIFXBQesAlvLtOAoIr, kokHdPrCpxuFzxrjBnvzOdFOKptXUpuh) >= 0 && math.dot(kokHdPrCpxuFzxrjBnvzOdFOKptXUpuh, CyhHjAASJbmGmfInqvgQwWOkJSBlEtOj) >= 0;
        }

        /// <summary>
        /// Calculates the intersection point of a line segment and a plane.
        /// </summary>
        /// <param name="lineSegmentStart">The start point of the line segment.</param>
        /// <param name="lineSegmentEnd">The end point of the line segment.</param>
        /// <param name="planePoint">A point on the plane.</param>
        /// <param name="planeNormal">The normal vector of the plane.</param>
        /// <returns>The intersection point as a float3, if it exists.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 LineSegmentPlaneIntersection(float3 lineSegmentStart, float3 lineSegmentEnd, float3 planePoint, float3 planeNormal)
        {
            float3 CosAAqpytpFwaAOOEBWbsauBkbAwItaU = lineSegmentEnd - lineSegmentStart;
            float VzRpXorjWCITCYqTXqnyPhXSxroXJAco = math.dot(planeNormal, planePoint - lineSegmentStart) / math.dot(planeNormal, CosAAqpytpFwaAOOEBWbsauBkbAwItaU);
            return lineSegmentStart + (VzRpXorjWCITCYqTXqnyPhXSxroXJAco * CosAAqpytpFwaAOOEBWbsauBkbAwItaU);
        }

        /// <summary>
        /// Calculates the intersection point of a line segment and a plane.
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <param name="planePoint">A point on the plane.</param>
        /// <param name="planeNormal">The normal vector of the plane.</param>
        /// <returns>The intersection point as a float3, if it exists.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 RayPlaneIntersection(Ray ray, float3 planePoint, float3 planeNormal)
        {
            float UDaROzySPJjyneHStXzQvDaNNockFCrx = math.dot(planeNormal, planePoint - (float3)ray.origin) / math.dot(planeNormal, ray.direction);
            return ray.origin + (UDaROzySPJjyneHStXzQvDaNNockFCrx * ray.direction);
        }

        /// <summary>
        /// Determines if a line segment intersects with a triangle and calculates the barycentric coordinates of the intersection point.
        /// </summary>
        /// <param name="triA">The first vertex of the triangle.</param>
        /// <param name="triB">The second vertex of the triangle.</param>
        /// <param name="triC">The third vertex of the triangle.</param>
        /// <param name="lineSegmentPointA">The start point of the line segment.</param>
        /// <param name="lineSegmentPointB">The end point of the line segment.</param>
        /// <param name="barycentricCoords">The barycentric coordinates of the intersection point, if it exists.</param>
        /// <returns>True if the line segment intersects the triangle; otherwise, false.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LineSegmentTriangleIntersection(in float3 triA, in float3 triB, in float3 triC, in float3 lineSegmentPointA, in float3 lineSegmentPointB, out float3 barycentricCoords)
        {
            barycentricCoords = float3.zero;

            if (ArePointsOnTheSameSideOfTriangle(lineSegmentPointA, lineSegmentPointB, triA, triB, triC))
            {
                return false;
            }

            float3 sfgQeVeClIHJbLDLNRUhOyMstubajKjw = TriangleNormal(triA, triB, triC);

            float3 bTzlNlRVAEgEpcLQWHQVKVwKKqcxscpJ = LineSegmentPlaneIntersection(lineSegmentPointA, lineSegmentPointB, triA, sfgQeVeClIHJbLDLNRUhOyMstubajKjw);

            if (!IsPointInsideTriangle(bTzlNlRVAEgEpcLQWHQVKVwKKqcxscpJ, triA, triB, triC))
            {
                return false;
            }

            barycentricCoords = TriangleBarycentricCoordinates(bTzlNlRVAEgEpcLQWHQVKVwKKqcxscpJ, triA, triB, triC);
            return true;
        }

        /// <summary>
        /// Checks if a ray intersects a triangle.
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <param name="triA">The first vertex of the triangle.</param>
        /// <param name="triB">The second vertex of the triangle.</param>
        /// <param name="triC">The third vertex of the triangle.</param>
        /// <param name="intersectionPoint">The intersection point.</param>
        /// <returns>True if intersects.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RayTriangleIntersection(in Ray ray, in float3 triA, in float3 triB, in float3 triC, out float3 intersectionPoint)
        {
            float3 jxMxJFhqVTbsrFevIZTMsaauNqbTXawV = TriangleNormal(triA, triB, triC);
            intersectionPoint = RayPlaneIntersection(ray, triA, jxMxJFhqVTbsrFevIZTMsaauNqbTXawV);

            if (!IsPointInsideTriangle(intersectionPoint, triA, triB, triC))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the intersection point of a ray and a tetrahedron.
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="triA"></param>
        /// <param name="triB"></param>
        /// <param name="triC"></param>
        /// <param name="intersectionPoint"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RayTetrahedronIntersection(in Ray ray, in float3 tetA, in float3 tetB, in float3 tetC, in float3 tetD, out float3 intersectionPoint)
        {
            
            if (math.dot(TriangleNormal(tetB, tetC, tetD), ray.direction) < 0 && RayTriangleIntersection(ray, tetB, tetD, tetC, out intersectionPoint))
            {
                return true;
            }

            if (math.dot(TriangleNormal(tetA, tetC, tetD), ray.direction) < 0 && RayTriangleIntersection(ray, tetA, tetC, tetD, out intersectionPoint))
            {
                return true;
            }

            if (math.dot(TriangleNormal(tetA, tetD, tetB), ray.direction) < 0 && RayTriangleIntersection(ray, tetA, tetD, tetB, out intersectionPoint))
            {
                return true;
            }

            if (math.dot(TriangleNormal(tetA, tetB, tetC), ray.direction) < 0 && RayTriangleIntersection(ray, tetA, tetB, tetC, out intersectionPoint))
            {
                return true;
            }

            intersectionPoint = float3.zero;
            return false;
        }

        /// <summary>
        /// Calculates the normal vector of a triangle defined by three points.
        /// </summary>
        /// <param name="p1">The first point of the triangle.</param>
        /// <param name="p2">The second point of the triangle.</param>
        /// <param name="p3">The third point of the triangle.</param>
        /// <returns>The normal vector of the triangle.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 TriangleNormal(in float3 p1, in float3 p2, in float3 p3)
        {
            return math.normalize(math.cross(p2 - p1, p3 - p1));
        }

        /// <summary>
        /// Extracts the rotation component from a transformation matrix as a quaternion.
        /// </summary>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The rotation as a quaternion.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion ExtractRotation(this Matrix4x4 matrix)
        {
            float3 FaqmExUrCxvNjpZRPIBUpTlKGnrRClpo;
            FaqmExUrCxvNjpZRPIBUpTlKGnrRClpo.x = matrix.m02;
            FaqmExUrCxvNjpZRPIBUpTlKGnrRClpo.y = matrix.m12;
            FaqmExUrCxvNjpZRPIBUpTlKGnrRClpo.z = matrix.m22;

            Vector3 FFzmIeWAdxWAmwdgpSafGAzwiaUugBkH;
            FFzmIeWAdxWAmwdgpSafGAzwiaUugBkH.x = matrix.m01;
            FFzmIeWAdxWAmwdgpSafGAzwiaUugBkH.y = matrix.m11;
            FFzmIeWAdxWAmwdgpSafGAzwiaUugBkH.z = matrix.m21;

            return Quaternion.LookRotation(FaqmExUrCxvNjpZRPIBUpTlKGnrRClpo, FFzmIeWAdxWAmwdgpSafGAzwiaUugBkH);
        }

        /// <summary>
        /// Extracts the position component from a transformation matrix.
        /// </summary>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The position vector.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 ExtractPosition(this Matrix4x4 matrix)
        {
            Vector3 iAceckbdCRdIJvmQYocWOTnOSrwNqsJu;
            iAceckbdCRdIJvmQYocWOTnOSrwNqsJu.x = matrix.m03;
            iAceckbdCRdIJvmQYocWOTnOSrwNqsJu.y = matrix.m13;
            iAceckbdCRdIJvmQYocWOTnOSrwNqsJu.z = matrix.m23;
            return iAceckbdCRdIJvmQYocWOTnOSrwNqsJu;
        }

        /// <summary>
        /// Extracts the scale component from a transformation matrix.
        /// </summary>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The scale vector.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 ExtractScale(this Matrix4x4 matrix)
        {
            Vector3 ptqxbmviTLDtcQfEYQIUqoVIxoseAHth;
            ptqxbmviTLDtcQfEYQIUqoVIxoseAHth.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
            ptqxbmviTLDtcQfEYQIUqoVIxoseAHth.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
            ptqxbmviTLDtcQfEYQIUqoVIxoseAHth.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
            return ptqxbmviTLDtcQfEYQIUqoVIxoseAHth;
        }

        /// <summary>
        /// Returns true if the <paramref name="val"/> contains NaN values.
        /// </summary>
        /// <param name="val">The float3 to check.</param>
        /// <returns>True if <paramref name="val"/> contains NaN values.</returns>
        public static bool IsNaN(in this float3 val)
        {
            return float.IsNaN(val.x) || float.IsNaN(val.y) || float.IsNaN(val.z);
        }

        /// <summary>
        /// Returns true if at least one of the components of the <paramref name="val"/> is infinity.
        /// </summary>
        /// <param name="val">The value to check for inifinity.</param>
        /// <returns>Bool if at least one of the <paramref name="val"/> values is infinity.</returns>
        public static bool IsInfinity(in this float3 val)
        {
            return float.IsInfinity(val.x) || float.IsInfinity(val.y) || float.IsInfinity(val.z);
        }

        /// <summary>
        /// Calculates a point along a catmull-rom spline.
        /// </summary>
        /// <param name="p0">Control point P0.</param>
        /// <param name="p1">Control point P1.</param>
        /// <param name="p2">Control point P2.</param>
        /// <param name="p3">Control point P3.</param>
        /// <param name="t">Interpolation Factor.</param>
        /// <returns>The interpolated point along the spline.</returns>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 CatmullRomSpline(in float3 p0, in float3 p1, in float3 p2, in float3 p3, float t)
        {
            float UvhtuJBnHwOwDHgYuBQbXoIQxoZqufic = t * t;
            float sLPaghUOYIjEbkgTJvhqARRlSxskDhdI = UvhtuJBnHwOwDHgYuBQbXoIQxoZqufic * t;

            return 0.5f * ((2f * p1) +
                           ((-p0 + p2) * t) +
                           (((2f * p0) - (5f * p1) + (4f * p2) - p3) * UvhtuJBnHwOwDHgYuBQbXoIQxoZqufic) +
                           ((-p0 + (3f * p1) - (3f * p2) + p3) * sLPaghUOYIjEbkgTJvhqARRlSxskDhdI));
        }

        /// <summary>
        /// Tests if two float3 vectors are approximately equal.
        /// </summary>
        /// <param name="vec">The vector1.</param>
        /// <param name="comp">The vector2.</param>
        /// <returns>true if values are approximately the same.</returns>
        public static bool Approximately(in float3 vec, in float3 comp)
        {
            return (vec.x - comp.x < math.EPSILON) && (vec.y - comp.y < math.EPSILON) && (vec.z - comp.z < math.EPSILON);
        }

        /// <summary>
        /// Finds a vector perpendicular to <paramref name="v"/>.
        /// </summary>
        /// <param name="v">The input vector.</param>
        /// <returns>Returns one of the perpendicular vectors to the input vector</returns>
        public static float3 ArbitraryPerpendicularVector(in float3 v)
        {
            Assert.IsTrue(math.length(v) > 0, "Vector must have a length greater than 0.");

            var rUpGkszvOjRBkTFyPmHoUEFYmHQLOKDB = math.cross(v, new float3(1, 0, 0));

            if (Approximately(rUpGkszvOjRBkTFyPmHoUEFYmHQLOKDB, float3.zero))
            {
                rUpGkszvOjRBkTFyPmHoUEFYmHQLOKDB = math.cross(v, new float3(0, 0, 1));
            }

            return math.normalize(rUpGkszvOjRBkTFyPmHoUEFYmHQLOKDB);
        }
    }
}
