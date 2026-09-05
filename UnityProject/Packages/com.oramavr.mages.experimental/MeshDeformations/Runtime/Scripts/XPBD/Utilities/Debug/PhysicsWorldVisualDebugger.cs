#if UNITY_EDITOR
namespace MAGES.MeshDeformations
{
    using System;
    using MAGES.MeshDeformations.Utilities;
    using Unity.Mathematics;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// A visual debugger for the <see cref="PhysicsWorld"/>.
    /// </summary>
    [RequireComponent(typeof(PhysicsWorld))]
    public class PhysicsWorldVisualDebugger : MonoBehaviour
    {
        /// <summary>
        /// Gets a value indicating whether to visualize the particles.
        /// </summary>
        [Header("Particles")]
        [SerializeField]
        private bool particles = false;

        [Header("DistanceConstraints")]
        [SerializeField]
        private bool distanceConstraints = false;
        [SerializeField]
        private bool distanceConstraintsDebugLabels = false;

        [Header("CollinearConstraints")]
        [SerializeField]
        private bool collinearConstraints = false;

        [Header("VolumeConstraints")]
        [SerializeField]
        private bool volumeConstraints = false;

        private PhysicsWorld aKdTWFxAdIKOzjlGpJFJezONiglCgpWU;

        private int[] QrCLtkcnMlKlyMtxYimuJagXxFQirWnc = new int[0];
        private bool[] zHaimsIexKLuzSGMJLbWfQQNPAuZZFTN = new bool[0];
        private float3[] pZwVoXxoNCvaybTeOWoDMRDnkuQmogWz = new float3[0];
        private float[] UfjxZmZDgfRPacaooXDselDIXFwrjEPP = new float[0];
        private uint[] UmBlIyXGrCMtshCjkQcNaIvetFhHnMlt = new uint[0];

        private int[] HnTASAkWgYTayJCNXfPpHIxFMsGaProe = new int[0];
        private bool[] VjilscmFIskxvxwaygVzGupfJbFFoTtf = new bool[0];
        private int[] kUyDhBvMtPPsklZZUZgDWgNrxaUNrHSY = new int[0];
        private int[] CUIfYiBYOxJLPRaZiZwJvMCZibOZREfj = new int[0];
        private float[] zBBeutAhVjKQWDbDzyucDFEfqtgSuTIt = new float[0];

        private int[] IkmsDovsLFgmuotHIaXaDNJeFWKWVcmr = new int[0];
        private bool[] ApNFbFiIbnkJLOLXtzJkjZDjYdPNNZKk = new bool[0];
        private int[] vQsyBNQZzexmHORbAkYbbvKUdQYsnhvn = new int[0];
        private int[] QKTflfVMDLptKozPmsSoiKlaorMFBweg = new int[0];
        private int[] fquMCzRFSddpQYRkebIbmUpYOVWPtcUx = new int[0];

        private int[] jCGLcioGGyBQvAntcHjRpGeALQwWfSXh = new int[0];
        private bool[] WtEaNmHmLHSzRuBlgsfujvPBJmdNKNmU = new bool[0];
        private int[] ztQNtFOmGvXaMlCbdTqhdmVUBqxactDQ = new int[0];
        private int[] NUVodoPvqibSpQluBtQofmNTpdpBFLNp = new int[0];
        private int[] BCgFUyNGeTrNIGaGhvuegIieAQdZfrlm = new int[0];
        private int[] OCOHlMaUqLdrmZiYFqHZGDkYvntUuopB = new int[0];

        private SimulationMeshDescriptor[] mftnELIFiUEbJenvWUHpqcKjNvsnAhRR = new SimulationMeshDescriptor[0];

        private void HRvFSMsTtlpCZLpIbARWvRYAJWNxWPow()
        {
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU = GetComponent<PhysicsWorld>();
        }

        private void TcktPqZkbEgGcOkgJDUPkVPvDVuWflxa()
        {
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.PhysicsUpdate.AddListener(tuCPsnwhoUudtHfszYPhHNDAaymTCIbJ);
        }

        
        private void tuCPsnwhoUudtHfszYPhHNDAaymTCIbJ(float timeStep)
        {
            if (pZwVoXxoNCvaybTeOWoDMRDnkuQmogWz.Length != aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.ParticleData.Count)
            {
                Array.Resize(ref zHaimsIexKLuzSGMJLbWfQQNPAuZZFTN, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.ParticleData.Count);
                Array.Resize(ref QrCLtkcnMlKlyMtxYimuJagXxFQirWnc, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.ParticleData.Count);
                Array.Resize(ref pZwVoXxoNCvaybTeOWoDMRDnkuQmogWz, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.ParticleData.Count);
                Array.Resize(ref UfjxZmZDgfRPacaooXDselDIXFwrjEPP, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.ParticleData.Count);
                Array.Resize(ref UmBlIyXGrCMtshCjkQcNaIvetFhHnMlt, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.ParticleData.Count);
            }

            if (VjilscmFIskxvxwaygVzGupfJbFFoTtf.Length != aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.DistanceConstraintData.Count)
            {
                Array.Resize(ref VjilscmFIskxvxwaygVzGupfJbFFoTtf, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.DistanceConstraintData.Count);
                Array.Resize(ref kUyDhBvMtPPsklZZUZgDWgNrxaUNrHSY, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.DistanceConstraintData.Count);
                Array.Resize(ref CUIfYiBYOxJLPRaZiZwJvMCZibOZREfj, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.DistanceConstraintData.Count);
                Array.Resize(ref HnTASAkWgYTayJCNXfPpHIxFMsGaProe, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.DistanceConstraintData.Count);
                Array.Resize(ref zBBeutAhVjKQWDbDzyucDFEfqtgSuTIt, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.DistanceConstraintData.Count);
            }

            if (ApNFbFiIbnkJLOLXtzJkjZDjYdPNNZKk.Length != aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.CollinearConstraintData.Count)
            {
                Array.Resize(ref ApNFbFiIbnkJLOLXtzJkjZDjYdPNNZKk, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.CollinearConstraintData.Count);
                Array.Resize(ref vQsyBNQZzexmHORbAkYbbvKUdQYsnhvn, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.CollinearConstraintData.Count);
                Array.Resize(ref QKTflfVMDLptKozPmsSoiKlaorMFBweg, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.CollinearConstraintData.Count);
                Array.Resize(ref fquMCzRFSddpQYRkebIbmUpYOVWPtcUx, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.CollinearConstraintData.Count);
                Array.Resize(ref IkmsDovsLFgmuotHIaXaDNJeFWKWVcmr, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.CollinearConstraintData.Count);
            }

            if (WtEaNmHmLHSzRuBlgsfujvPBJmdNKNmU.Length != aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.VolumeConstraintData.Count)
            {
                Array.Resize(ref WtEaNmHmLHSzRuBlgsfujvPBJmdNKNmU, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.VolumeConstraintData.Count);
                Array.Resize(ref ztQNtFOmGvXaMlCbdTqhdmVUBqxactDQ, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.VolumeConstraintData.Count);
                Array.Resize(ref NUVodoPvqibSpQluBtQofmNTpdpBFLNp, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.VolumeConstraintData.Count);
                Array.Resize(ref BCgFUyNGeTrNIGaGhvuegIieAQdZfrlm, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.VolumeConstraintData.Count);
                Array.Resize(ref OCOHlMaUqLdrmZiYFqHZGDkYvntUuopB, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.VolumeConstraintData.Count);
                Array.Resize(ref jCGLcioGGyBQvAntcHjRpGeALQwWfSXh, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.VolumeConstraintData.Count);
            }

            if (mftnELIFiUEbJenvWUHpqcKjNvsnAhRR.Length != aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.SimulationMeshDescriptorData.Count)
            {
                Array.Resize(ref mftnELIFiUEbJenvWUHpqcKjNvsnAhRR, aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.SimulationMeshDescriptorData.Count);
            }

            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.ParticleData.Position.CopyTo(pZwVoXxoNCvaybTeOWoDMRDnkuQmogWz);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.ParticleData.Enabled.CopyTo(zHaimsIexKLuzSGMJLbWfQQNPAuZZFTN);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.ParticleData.SimulationMeshDescriptorIndex.CopyTo(QrCLtkcnMlKlyMtxYimuJagXxFQirWnc);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.ParticleData.InverseMass.CopyTo(UfjxZmZDgfRPacaooXDselDIXFwrjEPP);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.ParticleData.Phase.CopyTo(UmBlIyXGrCMtshCjkQcNaIvetFhHnMlt);

            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.DistanceConstraintData.SimulationMeshDescriptorIndex.CopyTo(HnTASAkWgYTayJCNXfPpHIxFMsGaProe);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.DistanceConstraintData.Enabled.CopyTo(VjilscmFIskxvxwaygVzGupfJbFFoTtf);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.DistanceConstraintData.ParticleA.CopyTo(kUyDhBvMtPPsklZZUZgDWgNrxaUNrHSY);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.DistanceConstraintData.ParticleB.CopyTo(CUIfYiBYOxJLPRaZiZwJvMCZibOZREfj);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.DistanceConstraintData.RestDistance.CopyTo(zBBeutAhVjKQWDbDzyucDFEfqtgSuTIt);

            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.CollinearConstraintData.SimulationMeshDescriptorIndex.CopyTo(IkmsDovsLFgmuotHIaXaDNJeFWKWVcmr);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.CollinearConstraintData.Enabled.CopyTo(ApNFbFiIbnkJLOLXtzJkjZDjYdPNNZKk);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.CollinearConstraintData.ParticleA.CopyTo(vQsyBNQZzexmHORbAkYbbvKUdQYsnhvn);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.CollinearConstraintData.ParticleB.CopyTo(QKTflfVMDLptKozPmsSoiKlaorMFBweg);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.CollinearConstraintData.ParticleC.CopyTo(fquMCzRFSddpQYRkebIbmUpYOVWPtcUx);

            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.VolumeConstraintData.SimulationMeshDescriptorIndex.CopyTo(jCGLcioGGyBQvAntcHjRpGeALQwWfSXh);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.VolumeConstraintData.Enabled.CopyTo(WtEaNmHmLHSzRuBlgsfujvPBJmdNKNmU);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.VolumeConstraintData.ParticleA.CopyTo(ztQNtFOmGvXaMlCbdTqhdmVUBqxactDQ);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.VolumeConstraintData.ParticleB.CopyTo(NUVodoPvqibSpQluBtQofmNTpdpBFLNp);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.VolumeConstraintData.ParticleC.CopyTo(BCgFUyNGeTrNIGaGhvuegIieAQdZfrlm);
            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.VolumeConstraintData.ParticleD.CopyTo(OCOHlMaUqLdrmZiYFqHZGDkYvntUuopB);

            aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.SimulationMeshDescriptorData.SimulationMeshDescriptors.CopyTo(mftnELIFiUEbJenvWUHpqcKjNvsnAhRR);
        }

        private void mzkIXfABBwUlfuvPVgdSRShccetSQiiF()
        {
            if (particles)
            {
                for (int cwMPIeeWzkSUpvuGCthVkmNOphYoQVAh = 0; cwMPIeeWzkSUpvuGCthVkmNOphYoQVAh < pZwVoXxoNCvaybTeOWoDMRDnkuQmogWz.Length; cwMPIeeWzkSUpvuGCthVkmNOphYoQVAh++)
                {
                    if (zHaimsIexKLuzSGMJLbWfQQNPAuZZFTN[cwMPIeeWzkSUpvuGCthVkmNOphYoQVAh])
                    {
                        UnityEngine.Random.InitState((int)PhysicsWorld.GetLayerFromPhase(UmBlIyXGrCMtshCjkQcNaIvetFhHnMlt[cwMPIeeWzkSUpvuGCthVkmNOphYoQVAh]));
                        var OpvFYVuUEGGKIVLHipiFwwkfETEvnDCT = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                        OpvFYVuUEGGKIVLHipiFwwkfETEvnDCT.a = UfjxZmZDgfRPacaooXDselDIXFwrjEPP[cwMPIeeWzkSUpvuGCthVkmNOphYoQVAh] <= math.EPSILON ? 0.4f : 1.0f;
                        Gizmos.color = OpvFYVuUEGGKIVLHipiFwwkfETEvnDCT;
                        Gizmos.DrawSphere(pZwVoXxoNCvaybTeOWoDMRDnkuQmogWz[cwMPIeeWzkSUpvuGCthVkmNOphYoQVAh], aKdTWFxAdIKOzjlGpJFJezONiglCgpWU.ParticleRadius);
                    }
                }
            }

            if (distanceConstraints)
            {
                float3 oXUBwYKHgysgfRMkbUDHeUmyfkImBCVE, OhOXFAOdhMKhVLANAHWjHfZvKfyjnpUd;
                SimulationMeshDescriptor nbfctyXmeZwbfzJrlyoLZvtsVUrZGXTh;
                for (int KgODtDeeqxIvChCjeXPgdsbMYDbtauVf = 0; KgODtDeeqxIvChCjeXPgdsbMYDbtauVf < VjilscmFIskxvxwaygVzGupfJbFFoTtf.Length; KgODtDeeqxIvChCjeXPgdsbMYDbtauVf++)
                {
                    if (VjilscmFIskxvxwaygVzGupfJbFFoTtf[KgODtDeeqxIvChCjeXPgdsbMYDbtauVf])
                    {
                        nbfctyXmeZwbfzJrlyoLZvtsVUrZGXTh = mftnELIFiUEbJenvWUHpqcKjNvsnAhRR[HnTASAkWgYTayJCNXfPpHIxFMsGaProe[KgODtDeeqxIvChCjeXPgdsbMYDbtauVf]];
                        oXUBwYKHgysgfRMkbUDHeUmyfkImBCVE = pZwVoXxoNCvaybTeOWoDMRDnkuQmogWz[kUyDhBvMtPPsklZZUZgDWgNrxaUNrHSY[KgODtDeeqxIvChCjeXPgdsbMYDbtauVf] + nbfctyXmeZwbfzJrlyoLZvtsVUrZGXTh.ParticleStartIndex];
                        OhOXFAOdhMKhVLANAHWjHfZvKfyjnpUd = pZwVoXxoNCvaybTeOWoDMRDnkuQmogWz[CUIfYiBYOxJLPRaZiZwJvMCZibOZREfj[KgODtDeeqxIvChCjeXPgdsbMYDbtauVf] + nbfctyXmeZwbfzJrlyoLZvtsVUrZGXTh.ParticleStartIndex];

                        UnityEngine.Random.InitState(HnTASAkWgYTayJCNXfPpHIxFMsGaProe[KgODtDeeqxIvChCjeXPgdsbMYDbtauVf]);
                        var rHPbHMYSvHQurCsSbEbMHtHDNtFCDlyG = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                        Gizmos.color = rHPbHMYSvHQurCsSbEbMHtHDNtFCDlyG;

                        if (distanceConstraintsDebugLabels)
                        {
                            using (new Handles.DrawingScope(rHPbHMYSvHQurCsSbEbMHtHDNtFCDlyG))
                            {
                                float BEmNEsQGMqgpJKDUbdaARIrXKEPwoCgC = math.distance(oXUBwYKHgysgfRMkbUDHeUmyfkImBCVE, OhOXFAOdhMKhVLANAHWjHfZvKfyjnpUd);
                                float zpMMlDnXNibkWhikKxoMPFEYNEuHhDSY = zBBeutAhVjKQWDbDzyucDFEfqtgSuTIt[KgODtDeeqxIvChCjeXPgdsbMYDbtauVf];
                                float3 kpjUrxekslSbDQiQcmtZaUuXHyNLXxGK = math.lerp(oXUBwYKHgysgfRMkbUDHeUmyfkImBCVE, OhOXFAOdhMKhVLANAHWjHfZvKfyjnpUd, 0.5f);
                                Handles.Label(kpjUrxekslSbDQiQcmtZaUuXHyNLXxGK, $"{BEmNEsQGMqgpJKDUbdaARIrXKEPwoCgC} / {zpMMlDnXNibkWhikKxoMPFEYNEuHhDSY}");
                            }
                        }

                        Gizmos.DrawLine(oXUBwYKHgysgfRMkbUDHeUmyfkImBCVE, OhOXFAOdhMKhVLANAHWjHfZvKfyjnpUd);
                    }
                }
            }

            if (collinearConstraints)
            {
                SimulationMeshDescriptor SQwCSmweEzqhDYipkrEBEiLfOUBLkglZ;
                float3 arCWnXhBjAULjOgfWoVXDRCiYQUOvROk, YtyDEojkFjLebTCTDNvsRMbSsVYHrIcH, GqEmYXkYFERtqtCtNfvmiZjAVmLvoOGE;
                UnityEngine.Random.InitState(0);
                for (int pZdwqUZqWYjRBHboJnpnxknDjPQEnqpX = 0; pZdwqUZqWYjRBHboJnpnxknDjPQEnqpX < ApNFbFiIbnkJLOLXtzJkjZDjYdPNNZKk.Length; pZdwqUZqWYjRBHboJnpnxknDjPQEnqpX++)
                {
                    if (ApNFbFiIbnkJLOLXtzJkjZDjYdPNNZKk[pZdwqUZqWYjRBHboJnpnxknDjPQEnqpX])
                    {
                        SQwCSmweEzqhDYipkrEBEiLfOUBLkglZ = mftnELIFiUEbJenvWUHpqcKjNvsnAhRR[IkmsDovsLFgmuotHIaXaDNJeFWKWVcmr[pZdwqUZqWYjRBHboJnpnxknDjPQEnqpX]];
                        arCWnXhBjAULjOgfWoVXDRCiYQUOvROk = pZwVoXxoNCvaybTeOWoDMRDnkuQmogWz[vQsyBNQZzexmHORbAkYbbvKUdQYsnhvn[pZdwqUZqWYjRBHboJnpnxknDjPQEnqpX] + SQwCSmweEzqhDYipkrEBEiLfOUBLkglZ.ParticleStartIndex];
                        YtyDEojkFjLebTCTDNvsRMbSsVYHrIcH = pZwVoXxoNCvaybTeOWoDMRDnkuQmogWz[QKTflfVMDLptKozPmsSoiKlaorMFBweg[pZdwqUZqWYjRBHboJnpnxknDjPQEnqpX] + SQwCSmweEzqhDYipkrEBEiLfOUBLkglZ.ParticleStartIndex];
                        GqEmYXkYFERtqtCtNfvmiZjAVmLvoOGE = pZwVoXxoNCvaybTeOWoDMRDnkuQmogWz[fquMCzRFSddpQYRkebIbmUpYOVWPtcUx[pZdwqUZqWYjRBHboJnpnxknDjPQEnqpX] + SQwCSmweEzqhDYipkrEBEiLfOUBLkglZ.ParticleStartIndex];

                        var tiBDEuRGDAueGixFpYPJpACgFpyFdsyD = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                        Gizmos.color = tiBDEuRGDAueGixFpYPJpACgFpyFdsyD;

                        Gizmos.DrawLine(arCWnXhBjAULjOgfWoVXDRCiYQUOvROk, YtyDEojkFjLebTCTDNvsRMbSsVYHrIcH);
                        tiBDEuRGDAueGixFpYPJpACgFpyFdsyD.a = 0.5f;
                        Gizmos.color = tiBDEuRGDAueGixFpYPJpACgFpyFdsyD;
                        Gizmos.DrawLine(YtyDEojkFjLebTCTDNvsRMbSsVYHrIcH, GqEmYXkYFERtqtCtNfvmiZjAVmLvoOGE);
                    }
                }
            }

            if (volumeConstraints)
            {
                float3 qVneCSNejlobPaflDsUFmPqVpVKoGLXd, hoSRxpINrcpzzZZyuMzPJOZwNuGnMMMr, ZdTfdAxMCXLySJzsTKtQhDXXhqOThouz, EEphsDPKkDSLJbqAwAGMeeiCFNdtVMXV;
                SimulationMeshDescriptor yRIhbtHFtWLaTYBAfpeElrydEgIfYaLX;
                for (int fmWqNVjFdQQHKXCHEMIgCBWVkwQgbCKE = 0; fmWqNVjFdQQHKXCHEMIgCBWVkwQgbCKE < WtEaNmHmLHSzRuBlgsfujvPBJmdNKNmU.Length; fmWqNVjFdQQHKXCHEMIgCBWVkwQgbCKE++)
                {
                    if (WtEaNmHmLHSzRuBlgsfujvPBJmdNKNmU[fmWqNVjFdQQHKXCHEMIgCBWVkwQgbCKE])
                    {
                        yRIhbtHFtWLaTYBAfpeElrydEgIfYaLX = mftnELIFiUEbJenvWUHpqcKjNvsnAhRR[jCGLcioGGyBQvAntcHjRpGeALQwWfSXh[fmWqNVjFdQQHKXCHEMIgCBWVkwQgbCKE]];
                        qVneCSNejlobPaflDsUFmPqVpVKoGLXd = pZwVoXxoNCvaybTeOWoDMRDnkuQmogWz[ztQNtFOmGvXaMlCbdTqhdmVUBqxactDQ[fmWqNVjFdQQHKXCHEMIgCBWVkwQgbCKE] + yRIhbtHFtWLaTYBAfpeElrydEgIfYaLX.ParticleStartIndex];
                        hoSRxpINrcpzzZZyuMzPJOZwNuGnMMMr = pZwVoXxoNCvaybTeOWoDMRDnkuQmogWz[NUVodoPvqibSpQluBtQofmNTpdpBFLNp[fmWqNVjFdQQHKXCHEMIgCBWVkwQgbCKE] + yRIhbtHFtWLaTYBAfpeElrydEgIfYaLX.ParticleStartIndex];
                        ZdTfdAxMCXLySJzsTKtQhDXXhqOThouz = pZwVoXxoNCvaybTeOWoDMRDnkuQmogWz[BCgFUyNGeTrNIGaGhvuegIieAQdZfrlm[fmWqNVjFdQQHKXCHEMIgCBWVkwQgbCKE] + yRIhbtHFtWLaTYBAfpeElrydEgIfYaLX.ParticleStartIndex];
                        EEphsDPKkDSLJbqAwAGMeeiCFNdtVMXV = pZwVoXxoNCvaybTeOWoDMRDnkuQmogWz[OCOHlMaUqLdrmZiYFqHZGDkYvntUuopB[fmWqNVjFdQQHKXCHEMIgCBWVkwQgbCKE] + yRIhbtHFtWLaTYBAfpeElrydEgIfYaLX.ParticleStartIndex];

                        UnityEngine.Random.InitState(HnTASAkWgYTayJCNXfPpHIxFMsGaProe[fmWqNVjFdQQHKXCHEMIgCBWVkwQgbCKE]);
                        var cAXgRwLpwCNfPQclDsqqKKulxLwTgTHa = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                        Gizmos.color = cAXgRwLpwCNfPQclDsqqKKulxLwTgTHa;

                        MAGES.Utilities.GizmoUtilities.DrawTetrahedron(qVneCSNejlobPaflDsUFmPqVpVKoGLXd, hoSRxpINrcpzzZZyuMzPJOZwNuGnMMMr, ZdTfdAxMCXLySJzsTKtQhDXXhqOThouz, EEphsDPKkDSLJbqAwAGMeeiCFNdtVMXV);
                    }
                }
            }
        }
    }
}
#endif
