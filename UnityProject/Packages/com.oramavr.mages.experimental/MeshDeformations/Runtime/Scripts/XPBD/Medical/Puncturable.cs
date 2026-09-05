namespace MAGES.MeshDeformations
{
    using System.Collections.Generic;
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Jobs;
    using Unity.Mathematics;
    using UnityEngine;
    using Debug = UnityEngine.Debug;

    /// <summary>
    /// A Puncturable object that can be punctured by a <see cref="MeshDeformations.NeedleActor"/>.
    /// </summary>
    /// <seealso cref="MeshDeformations.NeedleActor"/>
    [RequireComponent(typeof(BasePhysicsActor))]
    public class Puncturable : BaseConstraintSolver
    {
#pragma warning disable SA1401 // Fields should be private
        /// <summary>
        /// The currently active insertion constraints.
        /// </summary>
        public NativeArray<InsertionConstraint> InsertionConstraints;

        /// <summary>
        /// The currently active insertion constraints count.
        /// </summary>
        public UnsafeAtomicCounter32 InsertionConstraintsCount;
#pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// The maximum number of insertion constraints that can be simulated simultaneously.
        /// </summary>
        private const int QHjROKAXtkkPnlAzYdocSEBTHbGGHAqW = 64;

        private bool fuSNWsIeLoYVkFZrexAqDUFNLYseTReu = false;

        private BasePhysicsActor DUotrVvPSUPPbKEbzaStFNRXrGzOdRNk;
        private SimulationMesh PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi;

        private SimulationMesh ZKeQfSfBjHARUZASFQvSqOHjAatcVUbX;

        private NativeArray<int> ljQAwDDutrtzrsyumSjFFHaPfPoZPwXp;

        private NativeList<AuDXCbxNyIRGioGVOAPnKkouTEAljrCn> rtnhUGsPGSBhwhfPAXcMXAaAnrUmVXBD;
        private NativeList<AuDXCbxNyIRGioGVOAPnKkouTEAljrCn> MGbvKuVGorxsSyONRzCAEcVyAmSyYlbd;

        /// <summary>
        /// The previous positions of the particles.
        /// </summary>
        private NativeArray<float3> YosSBOCDhWYEuSNiZsLtIIqhRrUahfIw;

        private uint hSMfgutnudItXORUCjHlRZQdTZekwVoE = 0;

        /// <summary>
        /// Gets or sets the needle actor that will be used to puncture the puncturable.
        /// </summary>
        [field: SerializeField]
        public NeedleActor NeedleActor { get; set; }

        /// <summary>
        /// Gets or sets the substeps used for simulating Insertion Constraints.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("The number of substeps to use when solving the insertion constraints.")]
        public int InsertionConstraintsSubSteps { get; set; } = 4;

        /// <summary>
        /// Gets or sets the relaxation factor used for solving the insertion constraints.
        /// </summary>
        [field: SerializeField]
        [field: Range(0.0f, 2.0f)]
        public float InsertionConstraintsRelaxationFactor { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets the static friction of the needle - puncturable interaction.
        /// </summary>
        [field: SerializeField]
        public float StaticFriction { get; set; } = 0.8f;

        /// <summary>
        /// Gets or sets the kinetic friction of the needle - puncturable interaction.
        /// </summary>
        [field: SerializeField]
        public float KineticFriction { get; set; } = 0.7f;

        /// <summary>
        /// Gets or sets the friction relaxation factor.
        /// </summary>
        [field: SerializeField]
        [field: Range(0.0f, 2.0f)]
        public float FrictionRelaxationFactor { get; set; } = 1.0f;

        /// <summary>
        /// Gets a value indicating whether the puncturable is initialized.
        /// </summary>
        public bool IsInitialized => fuSNWsIeLoYVkFZrexAqDUFNLYseTReu;

        /// <summary>
        /// Gets a value indicating whether this puncturable is punctured by a needle.
        /// </summary>
        public unsafe bool IsPunctured => *InsertionConstraintsCount.Counter > 0;

        /// <inheritdoc/>
        public override JobHandle DispatchSolveJob(in float timeStep, JobHandle dependsOn)
        {
            if (!IsInitialized || DUotrVvPSUPPbKEbzaStFNRXrGzOdRNk == null || NeedleActor == null)
            {
                return dependsOn;
            }

            var UNMjTGVSAfYuHmVLNQLqbfqqxwvbVleN = new rehTCtuAFLjHyMEmyIjACUDySXlBeGzG
            {
                NeedleSimulationMeshDescriptor = PhysicsWorld.SimulationMeshDescriptorData.SimulationMeshDescriptors[ZKeQfSfBjHARUZASFQvSqOHjAatcVUbX.SimulationMeshDescriptorIndex],
                PuncturableSimulationMeshDescriptor = PhysicsWorld.SimulationMeshDescriptorData.SimulationMeshDescriptors[PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.SimulationMeshDescriptorIndex],

                NeedleOriginalPhase = hSMfgutnudItXORUCjHlRZQdTZekwVoE,

                PunctureData = MGbvKuVGorxsSyONRzCAEcVyAmSyYlbd,
                PreviousPunctureData = rtnhUGsPGSBhwhfPAXcMXAaAnrUmVXBD,

                PuncturableTriangles = ljQAwDDutrtzrsyumSjFFHaPfPoZPwXp,

                ParticlesEnabled = PhysicsWorld.ParticleData.Enabled,
                ParticlesPosition = PhysicsWorld.ParticleData.Position,
                ParticlesPredictedPosition = PhysicsWorld.ParticleData.PredictedPosition,
                ParticlesPhase = PhysicsWorld.ParticleData.Phase,

                InsertionConstraints = InsertionConstraints,
                InsertionConstraintsCount = InsertionConstraintsCount,
            };

            var mmqtnoJgtViTHgmIyYQIgANxbErRYUMM = new InitializeCorrectionsBufferJob
            {
                ParticlesCorrections = PhysicsWorld.ParticleData.PositionCorrections,
                ParticlesCorrectionsCount = PhysicsWorld.ParticleData.PositionCorrectionsCount,
            };

            var cNJyTyeSeYQnhmJiJMGZdGtTNwRxKlcM = new XQgsFXetamirDwYISuNksasLFrqWDOuk
            {
                InsertionConstraints = InsertionConstraints,

                ParticlesEnabled = PhysicsWorld.ParticleData.Enabled,
                ParticlesPredictedPosition = PhysicsWorld.ParticleData.PredictedPosition,
                ParticlesInverseMass = PhysicsWorld.ParticleData.InverseMass,

                ParticlesCorrections = PhysicsWorld.ParticleData.PositionCorrections,
                ParticlesCorrectionsCount = PhysicsWorld.ParticleData.PositionCorrectionsCount,
                ParticlesLocks = PhysicsWorld.ParticleData.ParticleLocks,
            };

            var onXgIUsbOCONBijAugyScJpZmXVNtXrh = new ApplyCorrectionsJob
            {
                RelaxationFactor = InsertionConstraintsRelaxationFactor,

                ApplyTarget = PhysicsWorld.ParticleData.PredictedPosition,

                ParticlesCorrections = PhysicsWorld.ParticleData.PositionCorrections,
                ParticlesCorrectionsCount = PhysicsWorld.ParticleData.PositionCorrectionsCount,
            };

            var BZBIsgMhggzgEgzFsGetWQmyHOQczhfd = new CopyBufferJob<float3>
            {
                Source = PhysicsWorld.ParticleData.PredictedPosition,
                Destination = YosSBOCDhWYEuSNiZsLtIIqhRrUahfIw,
            };

            var aaJyADOagnZevjOosGOiEsTPwxMcyJUE = new NdciQmrUelIjbluNTtiIdmPZDrTUeQwG
            {
                StaticFriction = StaticFriction,
                KineticFriction = KineticFriction,

                InsertionConstraints = InsertionConstraints,

                ParticlesEnabled = PhysicsWorld.ParticleData.Enabled,
                ParticlesPreviousPosition = PhysicsWorld.ParticleData.Position,
                ParticlesPredictedPosition = PhysicsWorld.ParticleData.PredictedPosition,
                ParticlesInverseMass = PhysicsWorld.ParticleData.InverseMass,

                ParticlesCorrections = PhysicsWorld.ParticleData.PositionCorrections,
                ParticlesCorrectionsCount = PhysicsWorld.ParticleData.PositionCorrectionsCount,
                ParticlesLocks = PhysicsWorld.ParticleData.ParticleLocks,
            };

            var qravolEYqWdZGVLnhOBkcbHomZOLFNld = new ApplyCorrectionsJob
            {
                RelaxationFactor = FrictionRelaxationFactor,

                ApplyTarget = PhysicsWorld.ParticleData.PredictedPosition,

                ParticlesCorrections = PhysicsWorld.ParticleData.PositionCorrections,
                ParticlesCorrectionsCount = PhysicsWorld.ParticleData.PositionCorrectionsCount,
            };

            dependsOn = UNMjTGVSAfYuHmVLNQLqbfqqxwvbVleN.Schedule(dependsOn);

            
            for (int SnLJoJGGHqRefEPOffHpzDHhwUVTqcwd = 0; SnLJoJGGHqRefEPOffHpzDHhwUVTqcwd < InsertionConstraintsSubSteps; SnLJoJGGHqRefEPOffHpzDHhwUVTqcwd++)
            {
                
                dependsOn = BZBIsgMhggzgEgzFsGetWQmyHOQczhfd.Schedule(PhysicsWorld.ParticleData.Count, 64, dependsOn);

                
                dependsOn = mmqtnoJgtViTHgmIyYQIgANxbErRYUMM.Schedule(PhysicsWorld.ParticleData.Count, 64, dependsOn);

                
                unsafe
                {
                    dependsOn = cNJyTyeSeYQnhmJiJMGZdGtTNwRxKlcM.Schedule(InsertionConstraintsCount.Counter, 64, dependsOn);
                }

                
                dependsOn = onXgIUsbOCONBijAugyScJpZmXVNtXrh.Schedule(PhysicsWorld.ParticleData.Count, 64, dependsOn);
            }

            
            dependsOn = mmqtnoJgtViTHgmIyYQIgANxbErRYUMM.Schedule(PhysicsWorld.ParticleData.Count, 64, dependsOn);

            
            unsafe
            {
                dependsOn = aaJyADOagnZevjOosGOiEsTPwxMcyJUE.Schedule(InsertionConstraintsCount.Counter, 64, dependsOn);
            }

            
            dependsOn = qravolEYqWdZGVLnhOBkcbHomZOLFNld.Schedule(PhysicsWorld.ParticleData.Count, 64, dependsOn);

            return dependsOn;
        }

        /// <inheritdoc/>
        public override void RegisterSolver()
        {
            base.RegisterSolver();

            if (IsRegistered)
            {
                RXFskaFDQPqfNYooRRczvXAbYdvLOwvw();
                PhysicsWorld.PhysicsUpdate?.AddListener(SkZsxSXMdkctVKZtHFnpiXLicHREsDjJ);
            }
        }

        /// <inheritdoc/>
        public override void DeregisterSolver()
        {
            PhysicsWorld?.PhysicsUpdate?.RemoveListener(SkZsxSXMdkctVKZtHFnpiXLicHREsDjJ);

            base.DeregisterSolver();
        }

        /// <inheritdoc/>
        protected override void OnEnable()
        {
            PhysicsWorld = null;

            DUotrVvPSUPPbKEbzaStFNRXrGzOdRNk = GetComponent<BasePhysicsActor>();

            if (DUotrVvPSUPPbKEbzaStFNRXrGzOdRNk.SimulationMesh)
            {
                PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi = DUotrVvPSUPPbKEbzaStFNRXrGzOdRNk.SimulationMesh;

                PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.AddedToPhysicsWorld.AddListener(pCagvhAVfaNOpyjnYXPAJrfzioclvKQD);

                if (PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.IsPartOfPhysicsWorld)
                {
                    pCagvhAVfaNOpyjnYXPAJrfzioclvKQD();
                }

                PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.RemovedFromPhysicsWorld.AddListener(LNsHzigFaQPSvKKSRmXIopHFawzUsXbM);
            }

            if (NeedleActor.SimulationMesh)
            {

                ZKeQfSfBjHARUZASFQvSqOHjAatcVUbX = NeedleActor.SimulationMesh;

                ZKeQfSfBjHARUZASFQvSqOHjAatcVUbX.AddedToPhysicsWorld.AddListener(sKcbLXtwQliubntWUdmPqrktMaSLhDga);

                if (ZKeQfSfBjHARUZASFQvSqOHjAatcVUbX.IsPartOfPhysicsWorld)
                {
                    sKcbLXtwQliubntWUdmPqrktMaSLhDga();
                }

                ZKeQfSfBjHARUZASFQvSqOHjAatcVUbX.RemovedFromPhysicsWorld.AddListener(XBxAXyApgehFIbCedkZLWvEsAAuiJJJR);
            }
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            if (PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi)
            {
                PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.AddedToPhysicsWorld.RemoveListener(pCagvhAVfaNOpyjnYXPAJrfzioclvKQD);
                PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.RemovedFromPhysicsWorld.RemoveListener(LNsHzigFaQPSvKKSRmXIopHFawzUsXbM);
            }

            if (ZKeQfSfBjHARUZASFQvSqOHjAatcVUbX)
            {
                ZKeQfSfBjHARUZASFQvSqOHjAatcVUbX.AddedToPhysicsWorld.RemoveListener(sKcbLXtwQliubntWUdmPqrktMaSLhDga);
                ZKeQfSfBjHARUZASFQvSqOHjAatcVUbX.RemovedFromPhysicsWorld.RemoveListener(XBxAXyApgehFIbCedkZLWvEsAAuiJJJR);
            }
        }

        private void SkZsxSXMdkctVKZtHFnpiXLicHREsDjJ(float timeStep)
        {
            RXFskaFDQPqfNYooRRczvXAbYdvLOwvw();
        }

        private void RXFskaFDQPqfNYooRRczvXAbYdvLOwvw()
        {
            if (YosSBOCDhWYEuSNiZsLtIIqhRrUahfIw.Length < PhysicsWorld.ParticleData.Count)
            {
                YosSBOCDhWYEuSNiZsLtIIqhRrUahfIw.Dispose();
                YosSBOCDhWYEuSNiZsLtIIqhRrUahfIw = new NativeArray<float3>(PhysicsWorld.ParticleData.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            }
        }

        private void pCagvhAVfaNOpyjnYXPAJrfzioclvKQD()
        {
            if (PhysicsWorld == null)
            {
                PhysicsWorld = PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.PhysicsWorld;
            }
            else
            {
                if (PhysicsWorld != PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.PhysicsWorld)
                {
                    UnityEngine.Debug.Log("[Puncturable]: Both Puncturable Simulation Mesh and Needle Simulation Mesh must belong to the same Physics World.");
                    return;
                }
                else
                {
                    UbgGJPqONrdfTQhbBWkuoMaJlmryfqKn();
                }
            }
        }

        private void sKcbLXtwQliubntWUdmPqrktMaSLhDga()
        {
            if (PhysicsWorld == null)
            {
                PhysicsWorld = ZKeQfSfBjHARUZASFQvSqOHjAatcVUbX.PhysicsWorld;
            }
            else
            {
                if (PhysicsWorld != ZKeQfSfBjHARUZASFQvSqOHjAatcVUbX.PhysicsWorld)
                {
                    UnityEngine.Debug.Log("[Puncturable]: Both Puncturable Simulation Mesh and Needle Simulation Mesh must belong to the same Physics World.");
                    return;
                }
                else
                {
                    UbgGJPqONrdfTQhbBWkuoMaJlmryfqKn();
                }
            }
        }

        private void LNsHzigFaQPSvKKSRmXIopHFawzUsXbM()
        {
            MPBWGdYwTBpyECUeEJupRXalEGxKSXzh();
        }

        private void XBxAXyApgehFIbCedkZLWvEsAAuiJJJR()
        {
            MPBWGdYwTBpyECUeEJupRXalEGxKSXzh();
        }

        private void UbgGJPqONrdfTQhbBWkuoMaJlmryfqKn()
        {
            RqJtlObCCGhktcwBqQWUUkrnlfJPECub();
        }

        private void RqJtlObCCGhktcwBqQWUUkrnlfJPECub()
        {
            
            hSMfgutnudItXORUCjHlRZQdTZekwVoE = ZKeQfSfBjHARUZASFQvSqOHjAatcVUbX.Phase;

            InsertionConstraints = new NativeArray<InsertionConstraint>(QHjROKAXtkkPnlAzYdocSEBTHbGGHAqW, Allocator.Persistent);
            unsafe
            {
                int* mfWAHkUvkGbInZRIiPVGCWJMbECUjooB = (int*)UnsafeUtility.Malloc(sizeof(int), 4, Allocator.Persistent);
                InsertionConstraintsCount = new UnsafeAtomicCounter32(mfWAHkUvkGbInZRIiPVGCWJMbECUjooB);
                InsertionConstraintsCount.Reset();
            }

            rtnhUGsPGSBhwhfPAXcMXAaAnrUmVXBD = new NativeList<AuDXCbxNyIRGioGVOAPnKkouTEAljrCn>(QHjROKAXtkkPnlAzYdocSEBTHbGGHAqW, Allocator.Persistent);
            MGbvKuVGorxsSyONRzCAEcVyAmSyYlbd = new NativeList<AuDXCbxNyIRGioGVOAPnKkouTEAljrCn>(QHjROKAXtkkPnlAzYdocSEBTHbGGHAqW, Allocator.Persistent);

            wZERciNnlDSilkrsijDWOeSFAbInnkZL();

            RegisterSolver();

            fuSNWsIeLoYVkFZrexAqDUFNLYseTReu = true;
        }

        private void MPBWGdYwTBpyECUeEJupRXalEGxKSXzh()
        {
            fuSNWsIeLoYVkFZrexAqDUFNLYseTReu = false;

            DeregisterSolver();

            InsertionConstraints.Dispose();
            unsafe
            {
                UnsafeUtility.Free(InsertionConstraintsCount.Counter, Allocator.Persistent);
            }

            MGbvKuVGorxsSyONRzCAEcVyAmSyYlbd.Dispose();
            rtnhUGsPGSBhwhfPAXcMXAaAnrUmVXBD.Dispose();

            ljQAwDDutrtzrsyumSjFFHaPfPoZPwXp.Dispose();

            YosSBOCDhWYEuSNiZsLtIIqhRrUahfIw.Dispose();
        }

        private void wZERciNnlDSilkrsijDWOeSFAbInnkZL()
        {
            HashSet<(int, int, int)> NjYwzJNlqnIxYYQzDvNNSoZUNJnEaeXE = new HashSet<(int, int, int)>();

            for (int blSDRzEywcAxeVMSpfLlyUBghFQjqWdQ = 0; blSDRzEywcAxeVMSpfLlyUBghFQjqWdQ < PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.RuntimeVolumeConstraintData.Count; blSDRzEywcAxeVMSpfLlyUBghFQjqWdQ++)
            {
                AddTriangle(PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.RuntimeVolumeConstraintData.ParticleA[blSDRzEywcAxeVMSpfLlyUBghFQjqWdQ], PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.RuntimeVolumeConstraintData.ParticleB[blSDRzEywcAxeVMSpfLlyUBghFQjqWdQ], PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.RuntimeVolumeConstraintData.ParticleC[blSDRzEywcAxeVMSpfLlyUBghFQjqWdQ]);
                AddTriangle(PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.RuntimeVolumeConstraintData.ParticleA[blSDRzEywcAxeVMSpfLlyUBghFQjqWdQ], PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.RuntimeVolumeConstraintData.ParticleD[blSDRzEywcAxeVMSpfLlyUBghFQjqWdQ], PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.RuntimeVolumeConstraintData.ParticleB[blSDRzEywcAxeVMSpfLlyUBghFQjqWdQ]);
                AddTriangle(PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.RuntimeVolumeConstraintData.ParticleB[blSDRzEywcAxeVMSpfLlyUBghFQjqWdQ], PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.RuntimeVolumeConstraintData.ParticleD[blSDRzEywcAxeVMSpfLlyUBghFQjqWdQ], PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.RuntimeVolumeConstraintData.ParticleC[blSDRzEywcAxeVMSpfLlyUBghFQjqWdQ]);
                AddTriangle(PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.RuntimeVolumeConstraintData.ParticleA[blSDRzEywcAxeVMSpfLlyUBghFQjqWdQ], PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.RuntimeVolumeConstraintData.ParticleC[blSDRzEywcAxeVMSpfLlyUBghFQjqWdQ], PqBgOGnCBkAuYHgtusPpRZKRuCGyCghi.RuntimeVolumeConstraintData.ParticleD[blSDRzEywcAxeVMSpfLlyUBghFQjqWdQ]);
            }

            void AddTriangle(int particleAIndex, int particleBIndex, int particleCIndex)
            {
                
                var wFsiZxCgCwlzAhfYHOEuZJMpbVzgyNbJ = new int[] { particleAIndex, particleBIndex, particleCIndex };
                System.Array.Sort(wFsiZxCgCwlzAhfYHOEuZJMpbVzgyNbJ);

                (int, int, int) LswmINymOcbOGNQypVCjRxqHcWaSceli = (wFsiZxCgCwlzAhfYHOEuZJMpbVzgyNbJ[0], wFsiZxCgCwlzAhfYHOEuZJMpbVzgyNbJ[1], wFsiZxCgCwlzAhfYHOEuZJMpbVzgyNbJ[2]);

                if (!NjYwzJNlqnIxYYQzDvNNSoZUNJnEaeXE.Contains(LswmINymOcbOGNQypVCjRxqHcWaSceli))
                {
                    NjYwzJNlqnIxYYQzDvNNSoZUNJnEaeXE.Add(LswmINymOcbOGNQypVCjRxqHcWaSceli);
                }
            }

            ljQAwDDutrtzrsyumSjFFHaPfPoZPwXp = new NativeArray<int>(NjYwzJNlqnIxYYQzDvNNSoZUNJnEaeXE.Count * 3, Allocator.Persistent);

            var pOMJuBqsjgIvHlJvqkZOgBrsBqUibcLi = 0;
            foreach (var triangle in NjYwzJNlqnIxYYQzDvNNSoZUNJnEaeXE)
            {
                ljQAwDDutrtzrsyumSjFFHaPfPoZPwXp[pOMJuBqsjgIvHlJvqkZOgBrsBqUibcLi++] = triangle.Item1;
                ljQAwDDutrtzrsyumSjFFHaPfPoZPwXp[pOMJuBqsjgIvHlJvqkZOgBrsBqUibcLi++] = triangle.Item2;
                ljQAwDDutrtzrsyumSjFFHaPfPoZPwXp[pOMJuBqsjgIvHlJvqkZOgBrsBqUibcLi++] = triangle.Item3;
            }
        }

        /// <summary>
        /// An insertion constraint used for suturing simulations.
        /// </summary>
        [BurstCompile]
        public struct InsertionConstraint
        {
            /// <summary>
            /// Whether the constraint is enabled.
            /// </summary>
            public bool Enabled;

            /// <summary>
            /// The puncturable triangle index.
            /// </summary>
            public int TriangleIndex;

            /// <summary>
            /// Puncture point's barycentric coordinates relative to the punctured triangle.
            /// </summary>
            public float3 BarycentricCoordinates;

            /// <summary>
            /// Needle segment particle A index.
            /// </summary>
            public int NeedleSegmentParticleA;

            /// <summary>
            /// Needle segment particle B index.
            /// </summary>
            public int NeedleSegmentParticleB;

            /// <summary>
            /// Punctured triangle Particle A index.
            /// </summary>
            public int PuncturableParticleA;

            /// <summary>
            /// Punctured triangle Particle B index.
            /// </summary>
            public int PuncturableParticleB;

            /// <summary>
            /// Punctured triangle Particle C index.
            /// </summary>
            public int PuncturableParticleC;
        }

        [BurstCompile]
        private struct rehTCtuAFLjHyMEmyIjACUDySXlBeGzG : IJob
        {
            

            public SimulationMeshDescriptor NeedleSimulationMeshDescriptor;
            public SimulationMeshDescriptor PuncturableSimulationMeshDescriptor;

            [ReadOnly]
            public uint NeedleOriginalPhase;

            
            public NativeList<AuDXCbxNyIRGioGVOAPnKkouTEAljrCn> PreviousPunctureData;
            public NativeList<AuDXCbxNyIRGioGVOAPnKkouTEAljrCn> PunctureData;

            
            [ReadOnly]
            public NativeArray<int> PuncturableTriangles;

            [ReadOnly]
            public NativeArray<float3> ParticlesPosition;
            [ReadOnly]
            public NativeArray<float3> ParticlesPredictedPosition;
            [ReadOnly]
            public NativeArray<bool> ParticlesEnabled;
            [WriteOnly]
            public NativeArray<uint> ParticlesPhase;

            
            public NativeArray<InsertionConstraint> InsertionConstraints;
            [NativeDisableUnsafePtrRestriction]
            public UnsafeAtomicCounter32 InsertionConstraintsCount;

            public void Execute()
            {
                FCcYhbphpToYEuMdhptSYiKDZWgqnbAD();
                InsertionConstraintsCount.Reset();

                
                for (int VBhPiZnVKFRWDirphvgPMcUdsZIJJpuz = 0; VBhPiZnVKFRWDirphvgPMcUdsZIJJpuz < NeedleSimulationMeshDescriptor.ParticleCount - 1; VBhPiZnVKFRWDirphvgPMcUdsZIJJpuz++)
                {
                    CNcrBBVSXiEgAhPrIjXEswVxLYyibRCU(VBhPiZnVKFRWDirphvgPMcUdsZIJJpuz, NeedleSimulationMeshDescriptor.ParticleCount - 1);
                }

                
                wVSeBsrTvknbrYxHubjZVmrkPbRODJIK();

                gXQQegIHDueLXCjECCQlGlgDrcgTYSMz();

                PreviousPunctureData.CopyFrom(PunctureData);
                PunctureData.Clear();
            }

            private void CNcrBBVSXiEgAhPrIjXEswVxLYyibRCU(int needleSegmentIndex, int needleSegmentCount)
            {
                var keONHTGybNRcZbxzPXCTbTlvXZGThYfP = needleSegmentIndex;
                var zBkvoIevBgmThVnHrODMdmqztifhTEuO = needleSegmentIndex + 1;

                var dZafsNZmoMKibdwcRACKCLniSSlRUdcO = ParticlesPredictedPosition[NeedleSimulationMeshDescriptor.ParticleStartIndex + keONHTGybNRcZbxzPXCTbTlvXZGThYfP];
                var KuaBXRaYnEoYgdtMZscckitIIxPBXPui = ParticlesPredictedPosition[NeedleSimulationMeshDescriptor.ParticleStartIndex + zBkvoIevBgmThVnHrODMdmqztifhTEuO];

                
                if (needleSegmentIndex == 0)
                {
                    var hPWzYCYlzekNhlVXXanIkoLFgTtZTEjO = dZafsNZmoMKibdwcRACKCLniSSlRUdcO;
                    var abZtrZJDNhiHjYzkdoXRkgSCMlktqegj = KuaBXRaYnEoYgdtMZscckitIIxPBXPui;

                    
                    

                    
                    for (int VPGRweZxvTvntsvUACjsHIwTmHBFBUwJ = 0; VPGRweZxvTvntsvUACjsHIwTmHBFBUwJ < PuncturableTriangles.Length / 3; VPGRweZxvTvntsvUACjsHIwTmHBFBUwJ++)
                    {
                        if (WIPMUdiKQOpEZnKasxJbtGaBXOTtOOYU(hPWzYCYlzekNhlVXXanIkoLFgTtZTEjO, abZtrZJDNhiHjYzkdoXRkgSCMlktqegj, VPGRweZxvTvntsvUACjsHIwTmHBFBUwJ, out float3 barCoords))
                        {
                            if (!GiYOSryYRxIJhFyFLpkMQtaPYEEtOeMg(0, VPGRweZxvTvntsvUACjsHIwTmHBFBUwJ, out _))
                            {
                                
                                PunctureData.Add(new AuDXCbxNyIRGioGVOAPnKkouTEAljrCn
                                {
                                    SegmentIndex = 0,
                                    SegmentParticleASide = FreHQjlMBIanRtAgbGaGmmjZbQgVCKyW(dZafsNZmoMKibdwcRACKCLniSSlRUdcO, VPGRweZxvTvntsvUACjsHIwTmHBFBUwJ),
                                    TriangleIndex = VPGRweZxvTvntsvUACjsHIwTmHBFBUwJ,
                                    BarycentricCoordinates = barCoords,
                                });
                            }
                        }
                    }
                }

                
                for (int aeDZOkYSuvNbLkrkdBTdncTcUumICiBE = 0; aeDZOkYSuvNbLkrkdBTdncTcUumICiBE < PreviousPunctureData.Length; aeDZOkYSuvNbLkrkdBTdncTcUumICiBE++)
                {
                    var YBUFaTQUfBMiRqZlEdnAhKZNOotnvxgF = PreviousPunctureData[aeDZOkYSuvNbLkrkdBTdncTcUumICiBE];

                    var kOXvdzNKNsXrxXegkskmWyXLkOealkym = dZafsNZmoMKibdwcRACKCLniSSlRUdcO;
                    var JBRgpGRPqFUTafQTsvTjpXsBGzWygUNc = KuaBXRaYnEoYgdtMZscckitIIxPBXPui;

                    if (YBUFaTQUfBMiRqZlEdnAhKZNOotnvxgF.SegmentIndex == needleSegmentIndex)
                    {
                        
                        if (KsKNWljsVTRFRdfKMsGVfMjPgyBZcjMB(kOXvdzNKNsXrxXegkskmWyXLkOealkym, JBRgpGRPqFUTafQTsvTjpXsBGzWygUNc, YBUFaTQUfBMiRqZlEdnAhKZNOotnvxgF.TriangleIndex))
                        {
                            

                            
                            if (FreHQjlMBIanRtAgbGaGmmjZbQgVCKyW(kOXvdzNKNsXrxXegkskmWyXLkOealkym, YBUFaTQUfBMiRqZlEdnAhKZNOotnvxgF.TriangleIndex) == YBUFaTQUfBMiRqZlEdnAhKZNOotnvxgF.SegmentParticleASide)
                            {
                                
                                if (needleSegmentIndex < needleSegmentCount - 1)
                                {
                                    YBUFaTQUfBMiRqZlEdnAhKZNOotnvxgF.SegmentIndex = needleSegmentIndex + 1;
                                    PunctureData.Add(YBUFaTQUfBMiRqZlEdnAhKZNOotnvxgF);
                                }
                            }

                            
                            else
                            {
                                
                                if (needleSegmentIndex > 0)
                                {
                                    YBUFaTQUfBMiRqZlEdnAhKZNOotnvxgF.SegmentIndex = needleSegmentIndex - 1;
                                    PunctureData.Add(YBUFaTQUfBMiRqZlEdnAhKZNOotnvxgF);
                                }
                            }
                        }

                        
                        else
                        {
                            PunctureData.Add(YBUFaTQUfBMiRqZlEdnAhKZNOotnvxgF);
                        }
                    }
                }
            }

            private bool FreHQjlMBIanRtAgbGaGmmjZbQgVCKyW(in float3 pos, in int puncturableTriangleIndex)
            {
                var MPaqwsTKIffOnEUPHChWhaWZwDBhfCSi = PuncturableTriangles[puncturableTriangleIndex * 3] + PuncturableSimulationMeshDescriptor.ParticleStartIndex;
                var wIVJGpsUDqMSQTGPLjmWMGOXCvWuPsWH = PuncturableTriangles[(puncturableTriangleIndex * 3) + 1] + PuncturableSimulationMeshDescriptor.ParticleStartIndex;
                var TCeEgAEvrcfToshRMmcrDtIcNrbuVHOt = PuncturableTriangles[(puncturableTriangleIndex * 3) + 2] + PuncturableSimulationMeshDescriptor.ParticleStartIndex;

                var jFwHWFhMTBxEysMuKRjGiKaSfxjvaPjm = ParticlesPredictedPosition[MPaqwsTKIffOnEUPHChWhaWZwDBhfCSi];
                var RYodwUmutTVkprogBhbYvdniBRmXVLhP = ParticlesPredictedPosition[wIVJGpsUDqMSQTGPLjmWMGOXCvWuPsWH];
                var HBfuhKlVwiRXaOpbUFlIUQpeKDkXdNrI = ParticlesPredictedPosition[TCeEgAEvrcfToshRMmcrDtIcNrbuVHOt];

                return Math.GetSideOfTriangle(pos, jFwHWFhMTBxEysMuKRjGiKaSfxjvaPjm, RYodwUmutTVkprogBhbYvdniBRmXVLhP, HBfuhKlVwiRXaOpbUFlIUQpeKDkXdNrI);
            }

            private bool KsKNWljsVTRFRdfKMsGVfMjPgyBZcjMB(in float3 p0, in float3 p1, in int puncturableTriangleIndex)
            {
                var FxYyiHaWwPFDajlqThXYXCqYmGSnavnP = PuncturableTriangles[puncturableTriangleIndex * 3] + PuncturableSimulationMeshDescriptor.ParticleStartIndex;
                var NlYCeWjAYKcfdZhErxgyrNscZOJinumK = PuncturableTriangles[(puncturableTriangleIndex * 3) + 1] + PuncturableSimulationMeshDescriptor.ParticleStartIndex;
                var uXTISmmVhqFdXqDLdDSrXoVRLxvALczK = PuncturableTriangles[(puncturableTriangleIndex * 3) + 2] + PuncturableSimulationMeshDescriptor.ParticleStartIndex;

                var uCCDfvYCRMdgOeSrhAoGYNRUoCKCwfNH = ParticlesPredictedPosition[FxYyiHaWwPFDajlqThXYXCqYmGSnavnP];
                var qdXsIicgVaGjPteMZdwFdROtkwheKiuV = ParticlesPredictedPosition[NlYCeWjAYKcfdZhErxgyrNscZOJinumK];
                var QOEOcwpCdGduCpSjhIBnpCYoJzRkdskB = ParticlesPredictedPosition[uXTISmmVhqFdXqDLdDSrXoVRLxvALczK];

                return Math.GetSideOfTriangle(p0, uCCDfvYCRMdgOeSrhAoGYNRUoCKCwfNH, qdXsIicgVaGjPteMZdwFdROtkwheKiuV, QOEOcwpCdGduCpSjhIBnpCYoJzRkdskB) == Math.GetSideOfTriangle(p1, uCCDfvYCRMdgOeSrhAoGYNRUoCKCwfNH, qdXsIicgVaGjPteMZdwFdROtkwheKiuV, QOEOcwpCdGduCpSjhIBnpCYoJzRkdskB);
            }

            private bool WIPMUdiKQOpEZnKasxJbtGaBXOTtOOYU(in float3 linePointA, in float3 linePointB, in int puncturableTriangleIndex, out float3 barycentricCoordinates)
            {
                var yqpsbmCOvMhQsRwwOBrufJpcTurgLpGA = PuncturableTriangles[puncturableTriangleIndex * 3] + PuncturableSimulationMeshDescriptor.ParticleStartIndex;
                var WyVIgYlPZirXNJmWakZNWISRYvQFwCam = PuncturableTriangles[(puncturableTriangleIndex * 3) + 1] + PuncturableSimulationMeshDescriptor.ParticleStartIndex;
                var eJzraRaTrnLTeXTnADfCphkMIbgzdeKg = PuncturableTriangles[(puncturableTriangleIndex * 3) + 2] + PuncturableSimulationMeshDescriptor.ParticleStartIndex;

                var VQCSlsLzGmoNPegNNjetpkSMkOZdKPrD = ParticlesPredictedPosition[yqpsbmCOvMhQsRwwOBrufJpcTurgLpGA];
                var ktsKqBxfXuRDKVnMOVaWulvratqekRJT = ParticlesPredictedPosition[WyVIgYlPZirXNJmWakZNWISRYvQFwCam];
                var XRfCiGUvgqajkTCfoSIqYtDIhDNCpxgj = ParticlesPredictedPosition[eJzraRaTrnLTeXTnADfCphkMIbgzdeKg];

                return Math.LineSegmentTriangleIntersection(VQCSlsLzGmoNPegNNjetpkSMkOZdKPrD, ktsKqBxfXuRDKVnMOVaWulvratqekRJT, XRfCiGUvgqajkTCfoSIqYtDIhDNCpxgj, linePointA, linePointB, out barycentricCoordinates);
            }

            private bool GiYOSryYRxIJhFyFLpkMQtaPYEEtOeMg(int segmentIndex, int triIndex, out int punctureDataIndex)
            {
                for (int rdAzFPMfcbchbHOFUBNpOTZInBvlPcNe = 0; rdAzFPMfcbchbHOFUBNpOTZInBvlPcNe < PreviousPunctureData.Length; rdAzFPMfcbchbHOFUBNpOTZInBvlPcNe++)
                {
                    if (PreviousPunctureData[rdAzFPMfcbchbHOFUBNpOTZInBvlPcNe].SegmentIndex == segmentIndex && PreviousPunctureData[rdAzFPMfcbchbHOFUBNpOTZInBvlPcNe].TriangleIndex == triIndex)
                    {
                        punctureDataIndex = rdAzFPMfcbchbHOFUBNpOTZInBvlPcNe;
                        return true;
                    }
                }

                punctureDataIndex = -1;
                return false;
            }

            private void wVSeBsrTvknbrYxHubjZVmrkPbRODJIK()
            {
                for (int aAwtEoUUwBMgBCJMSNtgOXKjiCzIHxxA = 0; aAwtEoUUwBMgBCJMSNtgOXKjiCzIHxxA < math.min(PunctureData.Length, QHjROKAXtkkPnlAzYdocSEBTHbGGHAqW); aAwtEoUUwBMgBCJMSNtgOXKjiCzIHxxA++)
                {
                    var SSQnCZPCACVzrAttKibezaYEYMBEAoIE = PunctureData[aAwtEoUUwBMgBCJMSNtgOXKjiCzIHxxA];

                    InsertionConstraint XmDfVmcmpBXmkNGvqAKsSPyCvSpOoiKy = new InsertionConstraint
                    {
                        Enabled = true,
                        TriangleIndex = SSQnCZPCACVzrAttKibezaYEYMBEAoIE.TriangleIndex,
                        NeedleSegmentParticleA = SSQnCZPCACVzrAttKibezaYEYMBEAoIE.SegmentIndex + NeedleSimulationMeshDescriptor.ParticleStartIndex,
                        NeedleSegmentParticleB = SSQnCZPCACVzrAttKibezaYEYMBEAoIE.SegmentIndex + 1 + NeedleSimulationMeshDescriptor.ParticleStartIndex,

                        PuncturableParticleA = PuncturableTriangles[SSQnCZPCACVzrAttKibezaYEYMBEAoIE.TriangleIndex * 3] + PuncturableSimulationMeshDescriptor.ParticleStartIndex,
                        PuncturableParticleB = PuncturableTriangles[(SSQnCZPCACVzrAttKibezaYEYMBEAoIE.TriangleIndex * 3) + 1] + PuncturableSimulationMeshDescriptor.ParticleStartIndex,
                        PuncturableParticleC = PuncturableTriangles[(SSQnCZPCACVzrAttKibezaYEYMBEAoIE.TriangleIndex * 3) + 2] + PuncturableSimulationMeshDescriptor.ParticleStartIndex,

                        BarycentricCoordinates = SSQnCZPCACVzrAttKibezaYEYMBEAoIE.BarycentricCoordinates,
                    };

                    InsertionConstraints[InsertionConstraintsCount.Add(1)] = XmDfVmcmpBXmkNGvqAKsSPyCvSpOoiKy;
                }
            }

            private void FCcYhbphpToYEuMdhptSYiKDZWgqnbAD()
            {
                ParticlesPhase[NeedleSimulationMeshDescriptor.ParticleStartIndex] = 0;
                for (int GAKxlluqdeMEnuWqDAHQNkyujPDurieS = 1; GAKxlluqdeMEnuWqDAHQNkyujPDurieS < NeedleSimulationMeshDescriptor.ParticleCount; GAKxlluqdeMEnuWqDAHQNkyujPDurieS++)
                {
                    ParticlesPhase[GAKxlluqdeMEnuWqDAHQNkyujPDurieS + NeedleSimulationMeshDescriptor.ParticleStartIndex] = NeedleOriginalPhase;
                }
            }

            private void gXQQegIHDueLXCjECCQlGlgDrcgTYSMz()
            {
                unsafe
                {
                    for (int okyQtRISKViGnlwCAyoTJFqnUqdvxkXO = 0; okyQtRISKViGnlwCAyoTJFqnUqdvxkXO < *InsertionConstraintsCount.Counter; okyQtRISKViGnlwCAyoTJFqnUqdvxkXO++)
                    {
                        ParticlesPhase[InsertionConstraints[okyQtRISKViGnlwCAyoTJFqnUqdvxkXO].NeedleSegmentParticleA] = 0;
                        ParticlesPhase[InsertionConstraints[okyQtRISKViGnlwCAyoTJFqnUqdvxkXO].NeedleSegmentParticleB] = 0;
                    }
                }
            }
        }

        [BurstCompile]
        private struct XQgsFXetamirDwYISuNksasLFrqWDOuk : IJobParallelForDefer
        {
            public NativeArray<InsertionConstraint> InsertionConstraints;

            [ReadOnly]
            public NativeArray<bool> ParticlesEnabled;
            [ReadOnly]
            public NativeArray<float3> ParticlesPredictedPosition;
            [ReadOnly]
            public NativeArray<float> ParticlesInverseMass;

            
            [NativeDisableParallelForRestriction]
            public NativeArray<float3> ParticlesCorrections;
            [NativeDisableParallelForRestriction]
            public NativeArray<int> ParticlesCorrectionsCount;
            [NativeDisableParallelForRestriction]
            public NativeArray<int> ParticlesLocks;

            
            
            
            
            
            

            

            
            
            

            
            

            

            
            
            
            
            
            

            
            

            

            
            
            
            

            
            

            

            
            
            
            

            
            
            

            
            
            
            

            
            

            
            
            

            
            

            
            
            
            

            

            
            
            
            
            
            
            

            
            
            
            
            

            public void Execute(int insertionConstraintIndex)
            {
                if (Unity.Burst.CompilerServices.Hint.Unlikely(!InsertionConstraints[insertionConstraintIndex].Enabled))
                {
                    return;
                }

                InsertionConstraint KsDtDSkVYqnQpAmejbAxjrNBGRDKsjZW = InsertionConstraints[insertionConstraintIndex];

                int qcPEmDJchdPWkbhfTrHlwoQnVWCZDGHQ = InsertionConstraints[insertionConstraintIndex].PuncturableParticleA;
                int OkyChGaKpZlFSMOcbqeoMzWmeCyMdHij = InsertionConstraints[insertionConstraintIndex].PuncturableParticleB;
                int SfxsPiORhcbMQZdNhgHLquTEGaHDRVWh = InsertionConstraints[insertionConstraintIndex].PuncturableParticleC;

                int gvIrgfGQNQdCTbXpgthkoiXQTBiCTrMJ = InsertionConstraints[insertionConstraintIndex].NeedleSegmentParticleA;
                int yTYLtwZQJopeNXWJdbpKtjYTrWkFdAiv = InsertionConstraints[insertionConstraintIndex].NeedleSegmentParticleB;

                float3 USahNEvFMCmgMlpEzyhEcCHrwodkffGJ = InsertionConstraints[insertionConstraintIndex].BarycentricCoordinates;

                if (Unity.Burst.CompilerServices.Hint.Unlikely(!ParticlesEnabled[qcPEmDJchdPWkbhfTrHlwoQnVWCZDGHQ] || !ParticlesEnabled[OkyChGaKpZlFSMOcbqeoMzWmeCyMdHij] || !ParticlesEnabled[SfxsPiORhcbMQZdNhgHLquTEGaHDRVWh] || !ParticlesEnabled[gvIrgfGQNQdCTbXpgthkoiXQTBiCTrMJ] || !ParticlesEnabled[yTYLtwZQJopeNXWJdbpKtjYTrWkFdAiv]))
                {
                    KsDtDSkVYqnQpAmejbAxjrNBGRDKsjZW.Enabled = false;
                    InsertionConstraints[insertionConstraintIndex] = KsDtDSkVYqnQpAmejbAxjrNBGRDKsjZW;
                    return;
                }

                
                var sBEAGUKMFURKIePuboeXTAnoNpQsWFJU = math.cross(ParticlesPredictedPosition[OkyChGaKpZlFSMOcbqeoMzWmeCyMdHij] - ParticlesPredictedPosition[qcPEmDJchdPWkbhfTrHlwoQnVWCZDGHQ], ParticlesPredictedPosition[SfxsPiORhcbMQZdNhgHLquTEGaHDRVWh] - ParticlesPredictedPosition[qcPEmDJchdPWkbhfTrHlwoQnVWCZDGHQ]);
                var ncdvAcyfRncMqHMPbdBippNdAFMZdJgD = math.dot(sBEAGUKMFURKIePuboeXTAnoNpQsWFJU, ParticlesPredictedPosition[qcPEmDJchdPWkbhfTrHlwoQnVWCZDGHQ] - ParticlesPredictedPosition[gvIrgfGQNQdCTbXpgthkoiXQTBiCTrMJ]) / math.dot(sBEAGUKMFURKIePuboeXTAnoNpQsWFJU, ParticlesPredictedPosition[yTYLtwZQJopeNXWJdbpKtjYTrWkFdAiv] - ParticlesPredictedPosition[gvIrgfGQNQdCTbXpgthkoiXQTBiCTrMJ]);

                if (ncdvAcyfRncMqHMPbdBippNdAFMZdJgD < math.EPSILON || ncdvAcyfRncMqHMPbdBippNdAFMZdJgD > 1f - math.EPSILON)
                {
                    return;
                }

                
                var cCgrDFRjRmlRMQhELurDLvvnbVZFpRDT = ParticlesPredictedPosition[gvIrgfGQNQdCTbXpgthkoiXQTBiCTrMJ] + (ncdvAcyfRncMqHMPbdBippNdAFMZdJgD * (ParticlesPredictedPosition[yTYLtwZQJopeNXWJdbpKtjYTrWkFdAiv] - ParticlesPredictedPosition[gvIrgfGQNQdCTbXpgthkoiXQTBiCTrMJ]));
                var SVkjrDpNkSENaViMRhdfDmjzlXBjuMtd = Utilities.Math.TriangleBarycentricInterpolation(ParticlesPredictedPosition[qcPEmDJchdPWkbhfTrHlwoQnVWCZDGHQ], ParticlesPredictedPosition[OkyChGaKpZlFSMOcbqeoMzWmeCyMdHij], ParticlesPredictedPosition[SfxsPiORhcbMQZdNhgHLquTEGaHDRVWh], USahNEvFMCmgMlpEzyhEcCHrwodkffGJ);

                var hqTOMweIaTjsYCERBLGhMaqBmMREplcM = SVkjrDpNkSENaViMRhdfDmjzlXBjuMtd - cCgrDFRjRmlRMQhELurDLvvnbVZFpRDT;

                if (math.length(hqTOMweIaTjsYCERBLGhMaqBmMREplcM) < 0.0001f)
                {
                    return;
                }

                var HEOIBkKXAzrCJmfBXGHYKXBfnwwBKPMS = ParticlesInverseMass[qcPEmDJchdPWkbhfTrHlwoQnVWCZDGHQ] + ParticlesInverseMass[OkyChGaKpZlFSMOcbqeoMzWmeCyMdHij] + ParticlesInverseMass[SfxsPiORhcbMQZdNhgHLquTEGaHDRVWh] + ParticlesInverseMass[gvIrgfGQNQdCTbXpgthkoiXQTBiCTrMJ] + ParticlesInverseMass[yTYLtwZQJopeNXWJdbpKtjYTrWkFdAiv];

                ParticlesCorrections.InterlockedAddFloat3(ParticlesLocks, qcPEmDJchdPWkbhfTrHlwoQnVWCZDGHQ, -hqTOMweIaTjsYCERBLGhMaqBmMREplcM * (ParticlesInverseMass[qcPEmDJchdPWkbhfTrHlwoQnVWCZDGHQ] / HEOIBkKXAzrCJmfBXGHYKXBfnwwBKPMS));
                ParticlesCorrectionsCount.InterlockedAdd(qcPEmDJchdPWkbhfTrHlwoQnVWCZDGHQ, 1);
                ParticlesCorrections.InterlockedAddFloat3(ParticlesLocks, OkyChGaKpZlFSMOcbqeoMzWmeCyMdHij, -hqTOMweIaTjsYCERBLGhMaqBmMREplcM * (ParticlesInverseMass[OkyChGaKpZlFSMOcbqeoMzWmeCyMdHij] / HEOIBkKXAzrCJmfBXGHYKXBfnwwBKPMS));
                ParticlesCorrectionsCount.InterlockedAdd(OkyChGaKpZlFSMOcbqeoMzWmeCyMdHij, 1);
                ParticlesCorrections.InterlockedAddFloat3(ParticlesLocks, SfxsPiORhcbMQZdNhgHLquTEGaHDRVWh, -hqTOMweIaTjsYCERBLGhMaqBmMREplcM * (ParticlesInverseMass[SfxsPiORhcbMQZdNhgHLquTEGaHDRVWh] / HEOIBkKXAzrCJmfBXGHYKXBfnwwBKPMS));
                ParticlesCorrectionsCount.InterlockedAdd(SfxsPiORhcbMQZdNhgHLquTEGaHDRVWh, 1);

                ParticlesCorrections.InterlockedAddFloat3(ParticlesLocks, gvIrgfGQNQdCTbXpgthkoiXQTBiCTrMJ, hqTOMweIaTjsYCERBLGhMaqBmMREplcM * (ParticlesInverseMass[gvIrgfGQNQdCTbXpgthkoiXQTBiCTrMJ] / HEOIBkKXAzrCJmfBXGHYKXBfnwwBKPMS));
                ParticlesCorrectionsCount.InterlockedAdd(gvIrgfGQNQdCTbXpgthkoiXQTBiCTrMJ, 1);
                ParticlesCorrections.InterlockedAddFloat3(ParticlesLocks, yTYLtwZQJopeNXWJdbpKtjYTrWkFdAiv, hqTOMweIaTjsYCERBLGhMaqBmMREplcM * (ParticlesInverseMass[yTYLtwZQJopeNXWJdbpKtjYTrWkFdAiv] / HEOIBkKXAzrCJmfBXGHYKXBfnwwBKPMS));
                ParticlesCorrectionsCount.InterlockedAdd(yTYLtwZQJopeNXWJdbpKtjYTrWkFdAiv, 1);
            }
        }

        
        [BurstCompile]
        private struct NdciQmrUelIjbluNTtiIdmPZDrTUeQwG : IJobParallelForDefer
        {
            [ReadOnly]
            public float KineticFriction;
            [ReadOnly]
            public float StaticFriction;

            [ReadOnly]
            public NativeArray<InsertionConstraint> InsertionConstraints;

            [ReadOnly]
            public NativeArray<bool> ParticlesEnabled;
            [ReadOnly]
            public NativeArray<float3> ParticlesPreviousPosition;
            [ReadOnly]
            public NativeArray<float3> ParticlesPredictedPosition;
            [ReadOnly]
            public NativeArray<float> ParticlesInverseMass;

            
            [NativeDisableParallelForRestriction]
            public NativeArray<float3> ParticlesCorrections;
            [NativeDisableParallelForRestriction]
            public NativeArray<int> ParticlesCorrectionsCount;
            [NativeDisableParallelForRestriction]
            public NativeArray<int> ParticlesLocks;

            public void Execute(int insertionConstraintIndex)
            {
                if (Unity.Burst.CompilerServices.Hint.Unlikely(!InsertionConstraints[insertionConstraintIndex].Enabled))
                {
                    return;
                }

                InsertionConstraint ACxYICfFpuoHwYithlxLcEdLiywvspwh = InsertionConstraints[insertionConstraintIndex];

                int TqzawSUWJhWIBxyBrHyOTQCCPePntkhl = ACxYICfFpuoHwYithlxLcEdLiywvspwh.PuncturableParticleA;
                int VrdAdMbWkrpzeHBZGfeuobqEvWGmuSYP = ACxYICfFpuoHwYithlxLcEdLiywvspwh.PuncturableParticleB;
                int SRZaHlOuKMfllICtvhGFwUtJfIMXrHkK = ACxYICfFpuoHwYithlxLcEdLiywvspwh.PuncturableParticleC;

                int kvVZPkFovwkffSKDhftQnAtADGerfLlb = ACxYICfFpuoHwYithlxLcEdLiywvspwh.NeedleSegmentParticleA;
                int USEjXCErXoZUdpihGMOaoTbiGldPhnaW = ACxYICfFpuoHwYithlxLcEdLiywvspwh.NeedleSegmentParticleB;

                var WCsBNzAnRqIZkKXQVHUzOjACZevDdLbO = ParticlesInverseMass[TqzawSUWJhWIBxyBrHyOTQCCPePntkhl];
                WCsBNzAnRqIZkKXQVHUzOjACZevDdLbO += ParticlesInverseMass[VrdAdMbWkrpzeHBZGfeuobqEvWGmuSYP];
                WCsBNzAnRqIZkKXQVHUzOjACZevDdLbO += ParticlesInverseMass[SRZaHlOuKMfllICtvhGFwUtJfIMXrHkK];

                WCsBNzAnRqIZkKXQVHUzOjACZevDdLbO += ParticlesInverseMass[kvVZPkFovwkffSKDhftQnAtADGerfLlb];
                WCsBNzAnRqIZkKXQVHUzOjACZevDdLbO += ParticlesInverseMass[USEjXCErXoZUdpihGMOaoTbiGldPhnaW];

                if (WCsBNzAnRqIZkKXQVHUzOjACZevDdLbO == 0)
                {
                    return;
                }

                
                var pTpTKRjajYIynQNNCjkbkMhezQNYeZNk = ParticlesPreviousPosition[USEjXCErXoZUdpihGMOaoTbiGldPhnaW] - ParticlesPreviousPosition[kvVZPkFovwkffSKDhftQnAtADGerfLlb];

#pragma warning disable SA1305 // Field names should not use Hungarian notation
                var GmvuWoDvcgKuaYLRAIavOInDQPvyFyCe = ParticlesPredictedPosition[TqzawSUWJhWIBxyBrHyOTQCCPePntkhl] - ParticlesPreviousPosition[TqzawSUWJhWIBxyBrHyOTQCCPePntkhl];
                var YZfJavAHvMbDnsAYfGIaWAPnuIkpDjbS = ParticlesPredictedPosition[VrdAdMbWkrpzeHBZGfeuobqEvWGmuSYP] - ParticlesPreviousPosition[VrdAdMbWkrpzeHBZGfeuobqEvWGmuSYP];
                var JkTrkKytLVJEgKJSaEsIlkdELgylDvKn = ParticlesPredictedPosition[SRZaHlOuKMfllICtvhGFwUtJfIMXrHkK] - ParticlesPreviousPosition[SRZaHlOuKMfllICtvhGFwUtJfIMXrHkK];

                
                var UCJUDaZnwXWGeqzodxSKgJjGFCydktrY = GmvuWoDvcgKuaYLRAIavOInDQPvyFyCe;
                if (math.lengthsq(YZfJavAHvMbDnsAYfGIaWAPnuIkpDjbS) > math.lengthsq(UCJUDaZnwXWGeqzodxSKgJjGFCydktrY))
                {
                    UCJUDaZnwXWGeqzodxSKgJjGFCydktrY = YZfJavAHvMbDnsAYfGIaWAPnuIkpDjbS;
                }

                if (math.lengthsq(JkTrkKytLVJEgKJSaEsIlkdELgylDvKn) > math.lengthsq(UCJUDaZnwXWGeqzodxSKgJjGFCydktrY))
                {
                    UCJUDaZnwXWGeqzodxSKgJjGFCydktrY = JkTrkKytLVJEgKJSaEsIlkdELgylDvKn;
                }

                var vTEsBgMtzQhAntVEmBIDcAHSRnsEPRjB = math.length(GmvuWoDvcgKuaYLRAIavOInDQPvyFyCe);
                var cNAEKGFUyTAEnktHdinNPSrPGIQDhhXo = math.length(YZfJavAHvMbDnsAYfGIaWAPnuIkpDjbS);
                var EwPLIbqFwiHFuFkhZcuVoYROHkKPIYgQ = math.length(JkTrkKytLVJEgKJSaEsIlkdELgylDvKn);

                var oXgXvYhOWuZheHaeaBIgggaQhajORSow = ParticlesPredictedPosition[kvVZPkFovwkffSKDhftQnAtADGerfLlb] - ParticlesPreviousPosition[kvVZPkFovwkffSKDhftQnAtADGerfLlb];
                var mzSTyIpLCGikhGoRNpDjgOFfIAKgbEor = ParticlesPredictedPosition[USEjXCErXoZUdpihGMOaoTbiGldPhnaW] - ParticlesPreviousPosition[USEjXCErXoZUdpihGMOaoTbiGldPhnaW];
                var tYKJdeKaAThkKFBCXJPdjQkeINrRAjmj = (oXgXvYhOWuZheHaeaBIgggaQhajORSow + mzSTyIpLCGikhGoRNpDjgOFfIAKgbEor) / 2.0f;
                var IevOqjGGVKoigBvgRclrKEClEHvwzLGu = math.length(tYKJdeKaAThkKFBCXJPdjQkeINrRAjmj);
#pragma warning restore SA1305 // Field names should not use Hungarian notation

                var cONnHRxuBOZOsLmwxNMLUSSvjbMaQqGf = UCJUDaZnwXWGeqzodxSKgJjGFCydktrY - tYKJdeKaAThkKFBCXJPdjQkeINrRAjmj;

                var SbvyhQQpHDTCtbtVdWFztumfTzCcxeeB = GmvuWoDvcgKuaYLRAIavOInDQPvyFyCe - tYKJdeKaAThkKFBCXJPdjQkeINrRAjmj;
                var YOSGmtBOOwunHXSIPnJMWZbBQTQEQVfG = YZfJavAHvMbDnsAYfGIaWAPnuIkpDjbS - tYKJdeKaAThkKFBCXJPdjQkeINrRAjmj;
                var OhmblnJldKyYGQDjjKVqdCZxVkNAclFv = JkTrkKytLVJEgKJSaEsIlkdELgylDvKn - tYKJdeKaAThkKFBCXJPdjQkeINrRAjmj;

                var BHwrRRGskHXEtOWTqwOOGxecuEFQAWeX = math.projectsafe(SbvyhQQpHDTCtbtVdWFztumfTzCcxeeB, pTpTKRjajYIynQNNCjkbkMhezQNYeZNk, float3.zero);
                var ufuEyFizXkymGWTmNDSLBkQSCPoXYUxo = math.projectsafe(YOSGmtBOOwunHXSIPnJMWZbBQTQEQVfG, pTpTKRjajYIynQNNCjkbkMhezQNYeZNk, float3.zero);
                var sBXQnHIXGewkzkCObKOxGKrNBaIrvaDK = math.projectsafe(OhmblnJldKyYGQDjjKVqdCZxVkNAclFv, pTpTKRjajYIynQNNCjkbkMhezQNYeZNk, float3.zero);

                var sfPAzjqgagqSNyzabDbDaPYktggnccEC = math.projectsafe(cONnHRxuBOZOsLmwxNMLUSSvjbMaQqGf, pTpTKRjajYIynQNNCjkbkMhezQNYeZNk, float3.zero);
                var fUVksebyxHclxpDcWOKedVUfEDuOfJKR = zkIahFUThvbUoAQRbAZIIcAXTWSEIpyV(sfPAzjqgagqSNyzabDbDaPYktggnccEC, IevOqjGGVKoigBvgRclrKEClEHvwzLGu);

                
                ParticlesCorrections.InterlockedAddFloat3(ParticlesLocks, TqzawSUWJhWIBxyBrHyOTQCCPePntkhl, -((ParticlesInverseMass[TqzawSUWJhWIBxyBrHyOTQCCPePntkhl] + ParticlesInverseMass[VrdAdMbWkrpzeHBZGfeuobqEvWGmuSYP] + ParticlesInverseMass[SRZaHlOuKMfllICtvhGFwUtJfIMXrHkK]) / WCsBNzAnRqIZkKXQVHUzOjACZevDdLbO) * zkIahFUThvbUoAQRbAZIIcAXTWSEIpyV(BHwrRRGskHXEtOWTqwOOGxecuEFQAWeX, vTEsBgMtzQhAntVEmBIDcAHSRnsEPRjB));
                ParticlesCorrectionsCount.InterlockedAdd(TqzawSUWJhWIBxyBrHyOTQCCPePntkhl, 1);
                ParticlesCorrections.InterlockedAddFloat3(ParticlesLocks, VrdAdMbWkrpzeHBZGfeuobqEvWGmuSYP, -((ParticlesInverseMass[TqzawSUWJhWIBxyBrHyOTQCCPePntkhl] + ParticlesInverseMass[VrdAdMbWkrpzeHBZGfeuobqEvWGmuSYP] + ParticlesInverseMass[SRZaHlOuKMfllICtvhGFwUtJfIMXrHkK]) / WCsBNzAnRqIZkKXQVHUzOjACZevDdLbO) * zkIahFUThvbUoAQRbAZIIcAXTWSEIpyV(ufuEyFizXkymGWTmNDSLBkQSCPoXYUxo, cNAEKGFUyTAEnktHdinNPSrPGIQDhhXo));
                ParticlesCorrectionsCount.InterlockedAdd(VrdAdMbWkrpzeHBZGfeuobqEvWGmuSYP, 1);
                ParticlesCorrections.InterlockedAddFloat3(ParticlesLocks, SRZaHlOuKMfllICtvhGFwUtJfIMXrHkK, -((ParticlesInverseMass[TqzawSUWJhWIBxyBrHyOTQCCPePntkhl] + ParticlesInverseMass[VrdAdMbWkrpzeHBZGfeuobqEvWGmuSYP] + ParticlesInverseMass[SRZaHlOuKMfllICtvhGFwUtJfIMXrHkK]) / WCsBNzAnRqIZkKXQVHUzOjACZevDdLbO) * zkIahFUThvbUoAQRbAZIIcAXTWSEIpyV(sBXQnHIXGewkzkCObKOxGKrNBaIrvaDK, EwPLIbqFwiHFuFkhZcuVoYROHkKPIYgQ));
                ParticlesCorrectionsCount.InterlockedAdd(SRZaHlOuKMfllICtvhGFwUtJfIMXrHkK, 1);

                ParticlesCorrections.InterlockedAddFloat3(ParticlesLocks, kvVZPkFovwkffSKDhftQnAtADGerfLlb, ((ParticlesInverseMass[kvVZPkFovwkffSKDhftQnAtADGerfLlb] + ParticlesInverseMass[USEjXCErXoZUdpihGMOaoTbiGldPhnaW]) / WCsBNzAnRqIZkKXQVHUzOjACZevDdLbO) * fUVksebyxHclxpDcWOKedVUfEDuOfJKR);
                ParticlesCorrectionsCount.InterlockedAdd(kvVZPkFovwkffSKDhftQnAtADGerfLlb, 1);
                ParticlesCorrections.InterlockedAddFloat3(ParticlesLocks, USEjXCErXoZUdpihGMOaoTbiGldPhnaW, ((ParticlesInverseMass[kvVZPkFovwkffSKDhftQnAtADGerfLlb] + ParticlesInverseMass[USEjXCErXoZUdpihGMOaoTbiGldPhnaW]) / WCsBNzAnRqIZkKXQVHUzOjACZevDdLbO) * fUVksebyxHclxpDcWOKedVUfEDuOfJKR);
                ParticlesCorrectionsCount.InterlockedAdd(USEjXCErXoZUdpihGMOaoTbiGldPhnaW, 1);
            }

            private float3 zkIahFUThvbUoAQRbAZIIcAXTWSEIpyV(in float3 projectedVector, in float d)
            {
                var QHiafbNQMamYGBLnciThcARGWQjHmkTn = math.length(projectedVector);
                if (QHiafbNQMamYGBLnciThcARGWQjHmkTn < StaticFriction * d)
                {
                    return projectedVector;
                }
                else
                {
                    return projectedVector * math.min((KineticFriction * d) / QHiafbNQMamYGBLnciThcARGWQjHmkTn, 1f);
                }
            }
        }

        private struct AuDXCbxNyIRGioGVOAPnKkouTEAljrCn
        {
            public int SegmentIndex;

            public bool SegmentParticleASide; 
            public int TriangleIndex;
            public float3 BarycentricCoordinates;
        }
    }
}
