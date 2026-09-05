namespace MAGES.MeshDeformations
{
    using System.Collections.Generic;
    using MAGES.MeshDeformations.Utilities;
    using MAGES.Utilities;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Jobs;
    using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.Assertions;

    /// <summary>
    /// A solver for DistanceJoint constraints.
    /// </summary>
    internal class DistanceJointSolver : BaseConstraintSolver
    {
        private static Dictionary<PhysicsWorld, DistanceJointSolver> GpxdhtHwRHpjCHeRfckhdJJpSFJsJWjV = new Dictionary<PhysicsWorld, DistanceJointSolver>();

        private Queue<SsfyFqakBqlqLsHGMQHyiXWJeLMTmsLO> VNFKNESLYHWHqoepGZPPUqZqaQUmpsWF = new Queue<SsfyFqakBqlqLsHGMQHyiXWJeLMTmsLO>();
        private List<DistanceJoint> wPPlbTUEvBQHTqgIRYoJVTHlPfELGpUL = new List<DistanceJoint>();

        private NativeArray<bool> cFCYJtFlhlhvnsFZhevmisbijYpzZojg;

        private NativeArray<int> rDVfpaQLsdWrLdujtvAeamxIGYiMtcIQ;
        private NativeArray<int> TkotVPjzivWLTEDTBMWMsmYaeUSpcItv;

        private NativeArray<float> slinEGlLDePtELXFZaRuCGYolTKdFBMR;

        private NativeArray<float> IsYFIUOqEFpYEkyrCFhZidCxYtEsNAOO;

        private NativeArray<bool> dSCWaFbgCrFyRQlZvklireteapdscuMF;
        private NativeArray<float> UipWcoZdcxSMxTdVymmuZrWJjkHEniCG;

        private float3[] KJcepxOSRjFhLjRtxqRCNuvTsaReGiWj = new float3[0];
        private int[] krKVTtZHsRzqtjvQcjztBGLrpglvKWnx = new int[0];
        private int[] APOrwmGCGcnsCRSPuEcgFaadtHVQqFJx = new int[0];

        /// <summary>
        /// Gets or sets the solver substeps.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("The solver substeps.")]
        public int SubSteps { get; set; } = 2;

        /// <summary>
        /// Gets or sets the relaxation factor.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Relaxation factor.")]
        public float RelaxationFactor { get; set; } = 1;

        /// <summary>
        /// Gets or sets a value indicating whether to enable gizmos for debugging.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Enable gizmos for debugging.")]
        public bool DebugGizmos { get; set; }

        /// <summary>
        /// Gets or creates a DistanceJointSolver for the given PhysicsWorld.
        /// </summary>
        /// <param name="physicsWorld">The physics world with which this solver will be registered.</param>
        /// <returns>The created or existing DistanceJointSolver of the given <paramref name="physicsWorld"/>.</returns>
        public static DistanceJointSolver GetOrCreateSolverOfWorld(PhysicsWorld physicsWorld)
        {
            Assert.IsTrue(physicsWorld != null, "PhysicsWorld is null");

            if (GpxdhtHwRHpjCHeRfckhdJJpSFJsJWjV.TryGetValue(physicsWorld, out DistanceJointSolver existingSolver))
            {
                return existingSolver;
            }

            DistanceJointSolver NZQNgcCKaBVOfHokkwXuKPQDIfntOrNK = physicsWorld.gameObject.GetOrAddComponent<DistanceJointSolver>();
            NZQNgcCKaBVOfHokkwXuKPQDIfntOrNK.PhysicsWorld = physicsWorld;
            GpxdhtHwRHpjCHeRfckhdJJpSFJsJWjV[physicsWorld] = NZQNgcCKaBVOfHokkwXuKPQDIfntOrNK;

            return NZQNgcCKaBVOfHokkwXuKPQDIfntOrNK;
        }

        /// <inheritdoc/>
        public override void DeregisterSolver()
        {
            if (PhysicsWorld != null)
            {
                PhysicsWorld.PhysicsUpdate?.RemoveListener(iYHycvYkWcHPdXjUbpzuMkqfuWkbcXFt);
                GpxdhtHwRHpjCHeRfckhdJJpSFJsJWjV.Remove(PhysicsWorld);
            }

            base.DeregisterSolver();
        }

        /// <inheritdoc/>
        public override void RegisterSolver()
        {
            base.RegisterSolver();

            if (IsRegistered && PhysicsWorld != null)
            {
                GpxdhtHwRHpjCHeRfckhdJJpSFJsJWjV[PhysicsWorld] = this;
                PhysicsWorld.PhysicsUpdate.AddListener(iYHycvYkWcHPdXjUbpzuMkqfuWkbcXFt);
            }
        }

        /// <summary>
        /// Registers a joint to be solved with this solver.
        /// </summary>
        /// <param name="distanceJoint">The joint.</param>
        public void RegisterJoint(DistanceJoint distanceJoint)
        {
            VNFKNESLYHWHqoepGZPPUqZqaQUmpsWF.Enqueue(new vGWXPLxLSiJuSeKuxIkcDdRwUZHJEcwu(distanceJoint));
        }

        /// <summary>
        /// Deregisters a joint.
        /// </summary>
        /// <param name="distanceJoint">The joint.</param>
        public void DeregisterJoint(DistanceJoint distanceJoint)
        {
            VNFKNESLYHWHqoepGZPPUqZqaQUmpsWF.Enqueue(new XRheFmQxcVESwRWpnXSdDusEyUAagmcv(distanceJoint));
        }

        /// <inheritdoc/>
        public override JobHandle DispatchSolveJob(in float timeStep, JobHandle dependsOn)
        {
            float mPrjcvRoGlFmMWiZmxMKCyDGjpPevFEy = timeStep / SubSteps;

            var nSRFGmhpQQETAzsEzqPFbqCJBNhlXGii = new InitializeCorrectionsBufferJob
            {
                ParticlesCorrections = PhysicsWorld.ParticleData.PositionCorrections,
                ParticlesCorrectionsCount = PhysicsWorld.ParticleData.PositionCorrectionsCount,
            };

            var JkhJRzFKDTtHSOunBQShjzimkBhIwEHU = new SolveDistanceJointConstraintsJob
            {
                TimeStepSquared = mPrjcvRoGlFmMWiZmxMKCyDGjpPevFEy * mPrjcvRoGlFmMWiZmxMKCyDGjpPevFEy,

                ConstraintsEnabled = cFCYJtFlhlhvnsFZhevmisbijYpzZojg,

                ConstraintsParticleA = rDVfpaQLsdWrLdujtvAeamxIGYiMtcIQ,
                ConstraintsParticleB = TkotVPjzivWLTEDTBMWMsmYaeUSpcItv,

                ConstraintsRestDistance = slinEGlLDePtELXFZaRuCGYolTKdFBMR,

                ConstraintsCompliance = IsYFIUOqEFpYEkyrCFhZidCxYtEsNAOO,

                ConstraintsBreakable = dSCWaFbgCrFyRQlZvklireteapdscuMF,
                ConstraintsBreakForce = UipWcoZdcxSMxTdVymmuZrWJjkHEniCG,

                ParticlesEnabled = PhysicsWorld.ParticleData.Enabled,
                ParticlesInverseMass = PhysicsWorld.ParticleData.InverseMass,
                ParticlesPredictedPosition = PhysicsWorld.ParticleData.PredictedPosition,

                ParticleCorrections = PhysicsWorld.ParticleData.PositionCorrections,
                ParticleCorrectionsCount = PhysicsWorld.ParticleData.PositionCorrectionsCount,
                ParticleLocks = PhysicsWorld.ParticleData.ParticleLocks,
            };

            var nmymYOFdSogDgEuzYpdkpITgGUajHtfQ = new ApplyCorrectionsJob
            {
                ParticlesCorrections = PhysicsWorld.ParticleData.PositionCorrections,
                ParticlesCorrectionsCount = PhysicsWorld.ParticleData.PositionCorrectionsCount,
                ApplyTarget = PhysicsWorld.ParticleData.PredictedPosition,
                RelaxationFactor = 1f,
            };

            for (int jyRElPTmsXvKlBHtPyYcpuHcyxMiDjDX = 0; jyRElPTmsXvKlBHtPyYcpuHcyxMiDjDX < SubSteps; jyRElPTmsXvKlBHtPyYcpuHcyxMiDjDX++)
            {
                dependsOn = nSRFGmhpQQETAzsEzqPFbqCJBNhlXGii.Schedule(PhysicsWorld.ParticleData.Count, 64, dependsOn);
                dependsOn = JkhJRzFKDTtHSOunBQShjzimkBhIwEHU.Schedule(wPPlbTUEvBQHTqgIRYoJVTHlPfELGpUL.Count, 1, dependsOn);
                dependsOn = nmymYOFdSogDgEuzYpdkpITgGUajHtfQ.Schedule(PhysicsWorld.ParticleData.Count, 64, dependsOn);
            }

            return dependsOn;
        }

        private void iYHycvYkWcHPdXjUbpzuMkqfuWkbcXFt(float timeStep)
        {
            while (VNFKNESLYHWHqoepGZPPUqZqaQUmpsWF.TryDequeue(out var cmd))
            {
                cmd.Apply(this);
            }

            if (DebugGizmos)
            {
                
                if (krKVTtZHsRzqtjvQcjztBGLrpglvKWnx.Length != wPPlbTUEvBQHTqgIRYoJVTHlPfELGpUL.Count)
                {
                    krKVTtZHsRzqtjvQcjztBGLrpglvKWnx = new int[wPPlbTUEvBQHTqgIRYoJVTHlPfELGpUL.Count];
                    APOrwmGCGcnsCRSPuEcgFaadtHVQqFJx = new int[wPPlbTUEvBQHTqgIRYoJVTHlPfELGpUL.Count];
                }

                if (KJcepxOSRjFhLjRtxqRCNuvTsaReGiWj.Length != PhysicsWorld.ParticleData.Count)
                {
                    KJcepxOSRjFhLjRtxqRCNuvTsaReGiWj = new float3[PhysicsWorld.ParticleData.Count];
                }

                if (wPPlbTUEvBQHTqgIRYoJVTHlPfELGpUL.Count > 0)
                {
                    
                    rDVfpaQLsdWrLdujtvAeamxIGYiMtcIQ.CopyTo(krKVTtZHsRzqtjvQcjztBGLrpglvKWnx);
                    TkotVPjzivWLTEDTBMWMsmYaeUSpcItv.CopyTo(APOrwmGCGcnsCRSPuEcgFaadtHVQqFJx);
                    PhysicsWorld.ParticleData.Position.CopyTo(KJcepxOSRjFhLjRtxqRCNuvTsaReGiWj);
                }
            }
        }

        private void EflkWYhzOBekMGXmnmcTxiSnNjokNGQm()
        {
            for (int iMOXjdNlgbBFAPuvQBAVEeOKUnBCmsiJ = 0; iMOXjdNlgbBFAPuvQBAVEeOKUnBCmsiJ < wPPlbTUEvBQHTqgIRYoJVTHlPfELGpUL.Count; iMOXjdNlgbBFAPuvQBAVEeOKUnBCmsiJ++)
            {
                wPPlbTUEvBQHTqgIRYoJVTHlPfELGpUL[iMOXjdNlgbBFAPuvQBAVEeOKUnBCmsiJ].UpdateSlices(
                    cFCYJtFlhlhvnsFZhevmisbijYpzZojg.Slice(iMOXjdNlgbBFAPuvQBAVEeOKUnBCmsiJ, 1),
                    rDVfpaQLsdWrLdujtvAeamxIGYiMtcIQ.Slice(iMOXjdNlgbBFAPuvQBAVEeOKUnBCmsiJ, 1),
                    TkotVPjzivWLTEDTBMWMsmYaeUSpcItv.Slice(iMOXjdNlgbBFAPuvQBAVEeOKUnBCmsiJ, 1),
                    slinEGlLDePtELXFZaRuCGYolTKdFBMR.Slice(iMOXjdNlgbBFAPuvQBAVEeOKUnBCmsiJ, 1),
                    IsYFIUOqEFpYEkyrCFhZidCxYtEsNAOO.Slice(iMOXjdNlgbBFAPuvQBAVEeOKUnBCmsiJ, 1),
                    dSCWaFbgCrFyRQlZvklireteapdscuMF.Slice(iMOXjdNlgbBFAPuvQBAVEeOKUnBCmsiJ, 1),
                    UipWcoZdcxSMxTdVymmuZrWJjkHEniCG.Slice(iMOXjdNlgbBFAPuvQBAVEeOKUnBCmsiJ, 1));
            }
        }

        private void goVjPxMDCjmQvqkaggjKhRngWznGMoFl()
        {
            if (DebugGizmos)
            {
                for (int rLzOTpnKVvPGIhtlMxnLxfGhQKzYDjNK = 0; rLzOTpnKVvPGIhtlMxnLxfGhQKzYDjNK < krKVTtZHsRzqtjvQcjztBGLrpglvKWnx.Length; rLzOTpnKVvPGIhtlMxnLxfGhQKzYDjNK++)
                {
                    Gizmos.color = Color.red;
                    Vector3 fNnFJEmRfypRUsxcnJoioWkJWFpFBJxk = KJcepxOSRjFhLjRtxqRCNuvTsaReGiWj[krKVTtZHsRzqtjvQcjztBGLrpglvKWnx[rLzOTpnKVvPGIhtlMxnLxfGhQKzYDjNK]];
                    Vector3 wgbqLycmkwlFolwAJmOQjJDctkyaQAjM = KJcepxOSRjFhLjRtxqRCNuvTsaReGiWj[APOrwmGCGcnsCRSPuEcgFaadtHVQqFJx[rLzOTpnKVvPGIhtlMxnLxfGhQKzYDjNK]];
                    Gizmos.DrawLine(fNnFJEmRfypRUsxcnJoioWkJWFpFBJxk, wgbqLycmkwlFolwAJmOQjJDctkyaQAjM);
                }
            }
        }

        private abstract class SsfyFqakBqlqLsHGMQHyiXWJeLMTmsLO
        {
            public abstract void Apply(DistanceJointSolver solver);
        }

        private class vGWXPLxLSiJuSeKuxIkcDdRwUZHJEcwu : SsfyFqakBqlqLsHGMQHyiXWJeLMTmsLO
        {
            private DistanceJoint cqZlJXTagKaOYCkCOCSnxWuFywGNDDYC;

            public vGWXPLxLSiJuSeKuxIkcDdRwUZHJEcwu(DistanceJoint joint)
            {
                this.cqZlJXTagKaOYCkCOCSnxWuFywGNDDYC = joint;
            }

            public override void Apply(DistanceJointSolver solver)
            {
                if (cqZlJXTagKaOYCkCOCSnxWuFywGNDDYC == null)
                {
                    return;
                }

                solver.wPPlbTUEvBQHTqgIRYoJVTHlPfELGpUL.Add(cqZlJXTagKaOYCkCOCSnxWuFywGNDDYC);

                var shYkEIKKEIFmeiayruDBvWBKKrApnAvu = solver.wPPlbTUEvBQHTqgIRYoJVTHlPfELGpUL.Count;

                
                solver.cFCYJtFlhlhvnsFZhevmisbijYpzZojg.ResizeArray(shYkEIKKEIFmeiayruDBvWBKKrApnAvu, Allocator.Persistent, NativeArrayOptions.ClearMemory);

                solver.rDVfpaQLsdWrLdujtvAeamxIGYiMtcIQ.ResizeArray(shYkEIKKEIFmeiayruDBvWBKKrApnAvu, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
                solver.TkotVPjzivWLTEDTBMWMsmYaeUSpcItv.ResizeArray(shYkEIKKEIFmeiayruDBvWBKKrApnAvu, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

                solver.slinEGlLDePtELXFZaRuCGYolTKdFBMR.ResizeArray(shYkEIKKEIFmeiayruDBvWBKKrApnAvu, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
                solver.IsYFIUOqEFpYEkyrCFhZidCxYtEsNAOO.ResizeArray(shYkEIKKEIFmeiayruDBvWBKKrApnAvu, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

                solver.dSCWaFbgCrFyRQlZvklireteapdscuMF.ResizeArray(shYkEIKKEIFmeiayruDBvWBKKrApnAvu, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
                solver.UipWcoZdcxSMxTdVymmuZrWJjkHEniCG.ResizeArray(shYkEIKKEIFmeiayruDBvWBKKrApnAvu, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

                
                solver.EflkWYhzOBekMGXmnmcTxiSnNjokNGQm();

                
                cqZlJXTagKaOYCkCOCSnxWuFywGNDDYC?.OnJointRegistered();
            }
        }

        private class XRheFmQxcVESwRWpnXSdDusEyUAagmcv : SsfyFqakBqlqLsHGMQHyiXWJeLMTmsLO
        {
            private DistanceJoint GimedppbeGspPTDsDORKDpOTRaRUiTdy;

            private DistanceJointSolver NGSzqJLidaHbCWCItkDMTohoHPmWpYXz;

            public XRheFmQxcVESwRWpnXSdDusEyUAagmcv(DistanceJoint joint)
            {
                this.GimedppbeGspPTDsDORKDpOTRaRUiTdy = joint;
            }

            /// <inheritdoc/>
            public override void Apply(DistanceJointSolver solver)
            {
                this.NGSzqJLidaHbCWCItkDMTohoHPmWpYXz = solver;
                var QWQcjNjoSIRZAfOMbVpqbCbYkypWyYtc = solver.wPPlbTUEvBQHTqgIRYoJVTHlPfELGpUL.IndexOf(GimedppbeGspPTDsDORKDpOTRaRUiTdy);
                if (QWQcjNjoSIRZAfOMbVpqbCbYkypWyYtc >= 0)
                {
                    solver.wPPlbTUEvBQHTqgIRYoJVTHlPfELGpUL.RemoveAt(QWQcjNjoSIRZAfOMbVpqbCbYkypWyYtc);
                    fxJvjdtgkcFmpSnMgkPGKeXGBShcveTi(QWQcjNjoSIRZAfOMbVpqbCbYkypWyYtc);
                }
                else
                {
                    ceTPTLghtgIRaafXodFeYgxMpFUPKBXL();
                }
            }

            private void fxJvjdtgkcFmpSnMgkPGKeXGBShcveTi(int jointIndex)
            {
                
                NGSzqJLidaHbCWCItkDMTohoHPmWpYXz.cFCYJtFlhlhvnsFZhevmisbijYpzZojg.RemoveAt(jointIndex, Allocator.Persistent);

                NGSzqJLidaHbCWCItkDMTohoHPmWpYXz.rDVfpaQLsdWrLdujtvAeamxIGYiMtcIQ.RemoveAt(jointIndex, Allocator.Persistent);
                NGSzqJLidaHbCWCItkDMTohoHPmWpYXz.TkotVPjzivWLTEDTBMWMsmYaeUSpcItv.RemoveAt(jointIndex, Allocator.Persistent);

                NGSzqJLidaHbCWCItkDMTohoHPmWpYXz.IsYFIUOqEFpYEkyrCFhZidCxYtEsNAOO.RemoveAt(jointIndex, Allocator.Persistent);

                NGSzqJLidaHbCWCItkDMTohoHPmWpYXz.dSCWaFbgCrFyRQlZvklireteapdscuMF.RemoveAt(jointIndex, Allocator.Persistent);
                NGSzqJLidaHbCWCItkDMTohoHPmWpYXz.UipWcoZdcxSMxTdVymmuZrWJjkHEniCG.RemoveAt(jointIndex, Allocator.Persistent);

                
                NGSzqJLidaHbCWCItkDMTohoHPmWpYXz.EflkWYhzOBekMGXmnmcTxiSnNjokNGQm();
            }

            private void ceTPTLghtgIRaafXodFeYgxMpFUPKBXL()
            {
                for (int TiaAPoUORvnKiUZhfnByvqtimYMQQjWF = 0; TiaAPoUORvnKiUZhfnByvqtimYMQQjWF < NGSzqJLidaHbCWCItkDMTohoHPmWpYXz.wPPlbTUEvBQHTqgIRYoJVTHlPfELGpUL.Count; TiaAPoUORvnKiUZhfnByvqtimYMQQjWF++)
                {
                    if (NGSzqJLidaHbCWCItkDMTohoHPmWpYXz.wPPlbTUEvBQHTqgIRYoJVTHlPfELGpUL[TiaAPoUORvnKiUZhfnByvqtimYMQQjWF] == null)
                    {
                        NGSzqJLidaHbCWCItkDMTohoHPmWpYXz.wPPlbTUEvBQHTqgIRYoJVTHlPfELGpUL.RemoveAt(TiaAPoUORvnKiUZhfnByvqtimYMQQjWF);
                        fxJvjdtgkcFmpSnMgkPGKeXGBShcveTi(TiaAPoUORvnKiUZhfnByvqtimYMQQjWF);
                    }
                }
            }
        }
    }
}
