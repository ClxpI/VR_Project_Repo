namespace MAGES.MeshDeformations.Utilities
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Unity.Burst;
    using Unity.Mathematics;

    /// <summary>
    /// Cut utilities.
    /// </summary>
    internal static class CutUtilities
    {
        /// <summary>
        /// Returns a dictionary with the lerp factor between two particles and the intersection point.
        /// </summary>
        /// <param name="cuttingDisc">The cutting disc.</param>
        /// <param name="smesh">The simulation mesh.</param>
        /// <returns>
        /// The instersections.</br>
        /// arg0:</br>
        /// * arg0: The first particle index.</br>
        /// * arg1: The second particle index.</br>
        /// arg1: The interpolation factor of the intersection point.</br>
        /// </returns>
        public static Dictionary<(int, int), float> FindIntersections(in Disc cuttingDisc, SimulationMesh smesh)
        {
            Dictionary<(int, int), float> gQvmdYKjfghPxHTQcdpJYCPBOQarIkWj = new Dictionary<(int, int), float>();

            float XnFhECeAdttexqEttnwjjPDrefDTekMv;

            
            for (int fNpAsdthKPukhzSRcmHUNbrQRoxJfcyB = 0; fNpAsdthKPukhzSRcmHUNbrQRoxJfcyB < smesh.RuntimeVolumeConstraintData.Count; fNpAsdthKPukhzSRcmHUNbrQRoxJfcyB++)
            {
                if (!smesh.RuntimeVolumeConstraintData.Enabled[fNpAsdthKPukhzSRcmHUNbrQRoxJfcyB])
                {
                    continue;
                }

                int IONjvVfwHzxlBUnTlEmLvNVPWqdBrWAX = smesh.RuntimeVolumeConstraintData.ParticleA[fNpAsdthKPukhzSRcmHUNbrQRoxJfcyB];
                int mzmPeZRfkNMpJsYvXypicKXZEyeUVQiC = smesh.RuntimeVolumeConstraintData.ParticleB[fNpAsdthKPukhzSRcmHUNbrQRoxJfcyB];
                int pIBMcgMIFERUkEyjbNvbmEttNPNSDSoh = smesh.RuntimeVolumeConstraintData.ParticleC[fNpAsdthKPukhzSRcmHUNbrQRoxJfcyB];
                int IJvqzOPvDICpuQQPkokIoWkQLhjUPgPJ = smesh.RuntimeVolumeConstraintData.ParticleD[fNpAsdthKPukhzSRcmHUNbrQRoxJfcyB];

                float3 vNvDPVhgTvMPJocRUVTOFubOppNAYsBX = smesh.RuntimeParticleData.Position[IONjvVfwHzxlBUnTlEmLvNVPWqdBrWAX];
                float3 zHitHBKzHvppcLKFIPzzMLzWYQLRKyZi = smesh.RuntimeParticleData.Position[mzmPeZRfkNMpJsYvXypicKXZEyeUVQiC];
                float3 dJaDTdvlqLdsAQnKLvpoUgkraEaUExWU = smesh.RuntimeParticleData.Position[pIBMcgMIFERUkEyjbNvbmEttNPNSDSoh];
                float3 cUHffgirhwWeqdPAtUEZpjqqCclYnyaf = smesh.RuntimeParticleData.Position[IJvqzOPvDICpuQQPkokIoWkQLhjUPgPJ];

                
                if (!IntersectionsContainsEdge(IONjvVfwHzxlBUnTlEmLvNVPWqdBrWAX, mzmPeZRfkNMpJsYvXypicKXZEyeUVQiC))
                {
                    XnFhECeAdttexqEttnwjjPDrefDTekMv = tAuRVSYbtRkhDFjYrfkdwOIhoEGxFnqH(cuttingDisc, vNvDPVhgTvMPJocRUVTOFubOppNAYsBX, zHitHBKzHvppcLKFIPzzMLzWYQLRKyZi);
                    if (XnFhECeAdttexqEttnwjjPDrefDTekMv > 0)
                    {
                        AddIntersection(IONjvVfwHzxlBUnTlEmLvNVPWqdBrWAX, mzmPeZRfkNMpJsYvXypicKXZEyeUVQiC, XnFhECeAdttexqEttnwjjPDrefDTekMv);
                    }
                }

                
                if (!IntersectionsContainsEdge(mzmPeZRfkNMpJsYvXypicKXZEyeUVQiC, pIBMcgMIFERUkEyjbNvbmEttNPNSDSoh))
                {
                    XnFhECeAdttexqEttnwjjPDrefDTekMv = tAuRVSYbtRkhDFjYrfkdwOIhoEGxFnqH(cuttingDisc, zHitHBKzHvppcLKFIPzzMLzWYQLRKyZi, dJaDTdvlqLdsAQnKLvpoUgkraEaUExWU);
                    if (XnFhECeAdttexqEttnwjjPDrefDTekMv > 0)
                    {
                        AddIntersection(mzmPeZRfkNMpJsYvXypicKXZEyeUVQiC, pIBMcgMIFERUkEyjbNvbmEttNPNSDSoh, XnFhECeAdttexqEttnwjjPDrefDTekMv);
                    }
                }

                
                if (!IntersectionsContainsEdge(pIBMcgMIFERUkEyjbNvbmEttNPNSDSoh, IONjvVfwHzxlBUnTlEmLvNVPWqdBrWAX))
                {
                    XnFhECeAdttexqEttnwjjPDrefDTekMv = tAuRVSYbtRkhDFjYrfkdwOIhoEGxFnqH(cuttingDisc, dJaDTdvlqLdsAQnKLvpoUgkraEaUExWU, vNvDPVhgTvMPJocRUVTOFubOppNAYsBX);
                    if (XnFhECeAdttexqEttnwjjPDrefDTekMv > 0)
                    {
                        AddIntersection(pIBMcgMIFERUkEyjbNvbmEttNPNSDSoh, IONjvVfwHzxlBUnTlEmLvNVPWqdBrWAX, XnFhECeAdttexqEttnwjjPDrefDTekMv);
                    }
                }

                
                if (!IntersectionsContainsEdge(IONjvVfwHzxlBUnTlEmLvNVPWqdBrWAX, IJvqzOPvDICpuQQPkokIoWkQLhjUPgPJ))
                {
                    XnFhECeAdttexqEttnwjjPDrefDTekMv = tAuRVSYbtRkhDFjYrfkdwOIhoEGxFnqH(cuttingDisc, vNvDPVhgTvMPJocRUVTOFubOppNAYsBX, cUHffgirhwWeqdPAtUEZpjqqCclYnyaf);
                    if (XnFhECeAdttexqEttnwjjPDrefDTekMv > 0)
                    {
                        AddIntersection(IONjvVfwHzxlBUnTlEmLvNVPWqdBrWAX, IJvqzOPvDICpuQQPkokIoWkQLhjUPgPJ, XnFhECeAdttexqEttnwjjPDrefDTekMv);
                    }
                }

                
                if (!IntersectionsContainsEdge(mzmPeZRfkNMpJsYvXypicKXZEyeUVQiC, IJvqzOPvDICpuQQPkokIoWkQLhjUPgPJ))
                {
                    XnFhECeAdttexqEttnwjjPDrefDTekMv = tAuRVSYbtRkhDFjYrfkdwOIhoEGxFnqH(cuttingDisc, zHitHBKzHvppcLKFIPzzMLzWYQLRKyZi, cUHffgirhwWeqdPAtUEZpjqqCclYnyaf);
                    if (XnFhECeAdttexqEttnwjjPDrefDTekMv > 0)
                    {
                        AddIntersection(mzmPeZRfkNMpJsYvXypicKXZEyeUVQiC, IJvqzOPvDICpuQQPkokIoWkQLhjUPgPJ, XnFhECeAdttexqEttnwjjPDrefDTekMv);
                    }
                }

                
                if (!IntersectionsContainsEdge(pIBMcgMIFERUkEyjbNvbmEttNPNSDSoh, IJvqzOPvDICpuQQPkokIoWkQLhjUPgPJ))
                {
                    XnFhECeAdttexqEttnwjjPDrefDTekMv = tAuRVSYbtRkhDFjYrfkdwOIhoEGxFnqH(cuttingDisc, dJaDTdvlqLdsAQnKLvpoUgkraEaUExWU, cUHffgirhwWeqdPAtUEZpjqqCclYnyaf);
                    if (XnFhECeAdttexqEttnwjjPDrefDTekMv > 0)
                    {
                        AddIntersection(pIBMcgMIFERUkEyjbNvbmEttNPNSDSoh, IJvqzOPvDICpuQQPkokIoWkQLhjUPgPJ, XnFhECeAdttexqEttnwjjPDrefDTekMv);
                    }
                }
            }

            
            for (int QEBXqloPrnKJVelldSVJBEFPJQOBiEsM = 0; QEBXqloPrnKJVelldSVJBEFPJQOBiEsM < smesh.RuntimeDistanceConstraintData.Count; QEBXqloPrnKJVelldSVJBEFPJQOBiEsM++)
            {
                if (!smesh.RuntimeDistanceConstraintData.Enabled[QEBXqloPrnKJVelldSVJBEFPJQOBiEsM])
                {
                    continue;
                }

                int GdgvlMYhcopCAMXuZnbZYlzqUhJYWGyB = smesh.RuntimeDistanceConstraintData.ParticleA[QEBXqloPrnKJVelldSVJBEFPJQOBiEsM];
                int gBAqtEBJMKjMdYfdkrpmkyDagdGPESXv = smesh.RuntimeDistanceConstraintData.ParticleB[QEBXqloPrnKJVelldSVJBEFPJQOBiEsM];

                float3 AMJjFcIxbYdjsQTerwRDrxvrsKobjOVi = smesh.RuntimeParticleData.Position[GdgvlMYhcopCAMXuZnbZYlzqUhJYWGyB];
                float3 YVJJnyDbdlPuqIYBcHfaSedCPSIcqogC = smesh.RuntimeParticleData.Position[gBAqtEBJMKjMdYfdkrpmkyDagdGPESXv];

                
                if (!IntersectionsContainsEdge(GdgvlMYhcopCAMXuZnbZYlzqUhJYWGyB, gBAqtEBJMKjMdYfdkrpmkyDagdGPESXv))
                {
                    XnFhECeAdttexqEttnwjjPDrefDTekMv = tAuRVSYbtRkhDFjYrfkdwOIhoEGxFnqH(cuttingDisc, AMJjFcIxbYdjsQTerwRDrxvrsKobjOVi, YVJJnyDbdlPuqIYBcHfaSedCPSIcqogC);
                    if (XnFhECeAdttexqEttnwjjPDrefDTekMv > 0)
                    {
                        AddIntersection(GdgvlMYhcopCAMXuZnbZYlzqUhJYWGyB, gBAqtEBJMKjMdYfdkrpmkyDagdGPESXv, XnFhECeAdttexqEttnwjjPDrefDTekMv);
                    }
                }
            }

            bool IntersectionsContainsEdge(int p1, int p2)
            {
                if (p1 < p2)
                {
                    return gQvmdYKjfghPxHTQcdpJYCPBOQarIkWj.ContainsKey((p1, p2));
                }
                else
                {
                    return gQvmdYKjfghPxHTQcdpJYCPBOQarIkWj.ContainsKey((p2, p1));
                }
            }

            void AddIntersection(int p1, int p2, float t)
            {
                if (p1 < p2)
                {
                    gQvmdYKjfghPxHTQcdpJYCPBOQarIkWj[(p1, p2)] = t;
                }
                else
                {
                    gQvmdYKjfghPxHTQcdpJYCPBOQarIkWj[(p2, p1)] = 1f - t;
                }
            }

            return gQvmdYKjfghPxHTQcdpJYCPBOQarIkWj;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        private static float tAuRVSYbtRkhDFjYrfkdwOIhoEGxFnqH(in Disc cuttingDisc, float3 p0, float3 p1)
        {
            if (cuttingDisc.LineSegmentIntersection(p0, p1, out float3 intersectionPoint))
            {
                var ceFTLZlAphqbficQoPaNoIfidImpfAIx = math.distance(p0, intersectionPoint) / math.distance(p0, p1);
                return ceFTLZlAphqbficQoPaNoIfidImpfAIx;
            }

            return -1f;
        }
    }
}