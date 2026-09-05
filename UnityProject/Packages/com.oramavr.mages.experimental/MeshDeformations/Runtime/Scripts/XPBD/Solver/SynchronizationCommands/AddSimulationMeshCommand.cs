namespace MAGES.MeshDeformations.Synchronization
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Command to be queued for adding a simulation mesh to the physics world in the next synchronization step.
    /// </summary>
    internal class AddSimulationMeshCommand : BaseCommand
    {
        private SimulationMesh iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR;
        private float4x4 GiMXkcoswCyzweDQypPTAxGOOidScabx;
        private uint sKsuITiwiZVCvDErTpdnCEECoFJJytRK;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddSimulationMeshCommand"/> class.
        /// </summary>
        /// <param name="simulationMesh">The simulation mesh to spawn.</param>
        /// <param name="spawnTRS">The trs spawn matrix.</param>
        /// <param name="phase">The collision phase.</param>
        public AddSimulationMeshCommand(SimulationMesh simulationMesh, float4x4 spawnTRS, uint phase)
        {
            this.iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR = simulationMesh;
            this.GiMXkcoswCyzweDQypPTAxGOOidScabx = spawnTRS;
            this.sKsuITiwiZVCvDErTpdnCEECoFJJytRK = phase;
        }

        /// <inheritdoc/>
        public override void Apply(PhysicsWorld physicsWorld)
        {
            if (physicsWorld.SimulationMeshToDescriptorMap.ContainsKey(iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR))
            {
                return;
            }

            if (iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.IsPartOfPhysicsWorld)
            {
                UnityEngine.Debug.LogError("You are trying to add a simulation mesh that is already part of a physics world. This is not allowed.");
                return;
            }

            var LIWwXRHXwdvZFhVxkUmabqQLXiGUFZgv = physicsWorld.ParticleData.Resize(physicsWorld.ParticleData.Count + iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.Particles.Length);
            var XgVawokuFZlWsxCUZBGilzPmZUlYBkKQ = physicsWorld.DistanceConstraintData.Resize(physicsWorld.DistanceConstraintData.Count + iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.DistanceConstraints.Length);
            var vlhfopiljWZobbokWCFMvzEVPHyWcDXL = physicsWorld.VolumeConstraintData.Resize(physicsWorld.VolumeConstraintData.Count + iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.VolumeConstraints.Length);
            var MxCEImuTSamHgbbAzRVYfIvuMcHgddTl = physicsWorld.CollinearConstraintData.Resize(physicsWorld.CollinearConstraintData.Count + iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.CollinearConstraints.Length);
            var XfxyReKHkxoETBvcpWjkFijYLOfrYzpY = physicsWorld.ShapeMatchingConstraintData.Resize(physicsWorld.ShapeMatchingConstraintData.Count + iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.ShapeMatchingConstraints.Length);
            int GmZhEMoYGnDjHSASnZcBozSupwIJQVqz = physicsWorld.SimulationMeshDescriptorData.Resize(physicsWorld.SimulationMeshDescriptorData.Count + 1);

            var DlAUvVhphiveZjpEnuJkupeyQyiuENyH = physicsWorld.PhysicsMaterials.Length;
            physicsWorld.PhysicsMaterials.ResizeArray(physicsWorld.PhysicsMaterials.Length + iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.PhysicsMaterials.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            
            var RNHquCCLgtHgiaNoNnFTYXNTXQSbXnOZ = new SimulationMeshDescriptor
            {
                ParticleStartIndex = LIWwXRHXwdvZFhVxkUmabqQLXiGUFZgv,
                ParticleCount = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.Particles.Length,
                TotalParticleSpaceCount = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.Particles.Length,

                DistanceConstraintStartIndex = XgVawokuFZlWsxCUZBGilzPmZUlYBkKQ,
                DistanceConstraintCount = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.DistanceConstraints.Length,
                TotalDistanceConstraintSpaceCount = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.DistanceConstraints.Length,

                VolumeConstraintStartIndex = vlhfopiljWZobbokWCFMvzEVPHyWcDXL,
                VolumeConstraintCount = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.VolumeConstraints.Length,
                TotalVolumeConstraintSpaceCount = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.VolumeConstraints.Length,

                CollinearConstraintStartIndex = MxCEImuTSamHgbbAzRVYfIvuMcHgddTl,
                CollinearConstraintCount = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.CollinearConstraints.Length,
                TotalCollinearConstraintSpaceCount = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.CollinearConstraints.Length,

                ShapeMatchingConstraintStartIndex = XfxyReKHkxoETBvcpWjkFijYLOfrYzpY,
                ShapeMatchingConstraintCount = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.ShapeMatchingConstraints.Length,
                TotalShapeMatchingConstraintSpaceCount = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.ShapeMatchingConstraints.Length,

                PhysicsMaterialStartIndex = DlAUvVhphiveZjpEnuJkupeyQyiuENyH,
                PhysicsMaterialCount = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.PhysicsMaterials.Length,
                TotalPhysicsMaterialSpaceCount = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.PhysicsMaterials.Length,
            };

            physicsWorld.SimulationMeshDescriptorData.SimulationMeshDescriptors[GmZhEMoYGnDjHSASnZcBozSupwIJQVqz] = RNHquCCLgtHgiaNoNnFTYXNTXQSbXnOZ;

            
            for (int LMIVJqbViKujDJGAXTGMXODzLGgvywBH = 0; LMIVJqbViKujDJGAXTGMXODzLGgvywBH < iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.PhysicsMaterials.Length; LMIVJqbViKujDJGAXTGMXODzLGgvywBH++)
            {
                physicsWorld.PhysicsMaterials[DlAUvVhphiveZjpEnuJkupeyQyiuENyH + LMIVJqbViKujDJGAXTGMXODzLGgvywBH] = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.PhysicsMaterials[LMIVJqbViKujDJGAXTGMXODzLGgvywBH];
            }

            physicsWorld.SimulationMeshDescriptorData.TotalMass[GmZhEMoYGnDjHSASnZcBozSupwIJQVqz] = 0;
            physicsWorld.SimulationMeshDescriptorData.CenterOfMassPosition[GmZhEMoYGnDjHSASnZcBozSupwIJQVqz] = float3.zero;
            physicsWorld.SimulationMeshDescriptorData.CenterOfMassRotation[GmZhEMoYGnDjHSASnZcBozSupwIJQVqz] = quaternion.identity;
            physicsWorld.SimulationMeshDescriptorData.CovarianceMatrix[GmZhEMoYGnDjHSASnZcBozSupwIJQVqz] = float3x3.zero;
            physicsWorld.SimulationMeshDescriptorData.ContainsParticlesWithInfiniteMass[GmZhEMoYGnDjHSASnZcBozSupwIJQVqz] = false;

            
            for (int bCoQNfSAmANpmWkJRqKBoPhAZyDkieEl = 0; bCoQNfSAmANpmWkJRqKBoPhAZyDkieEl < iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.Particles.Length; bCoQNfSAmANpmWkJRqKBoPhAZyDkieEl++)
            {
                Particle hsnSzQnwTxiZiXsImGouWetgzzEnrwdQ = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.Particles[bCoQNfSAmANpmWkJRqKBoPhAZyDkieEl];

                var sSXaQzSIStksZVavWoAUYLvGjGNtCMdO = bCoQNfSAmANpmWkJRqKBoPhAZyDkieEl + LIWwXRHXwdvZFhVxkUmabqQLXiGUFZgv;

                physicsWorld.ParticleData.Enabled[sSXaQzSIStksZVavWoAUYLvGjGNtCMdO] = true;
                physicsWorld.ParticleData.Breakable[sSXaQzSIStksZVavWoAUYLvGjGNtCMdO] = hsnSzQnwTxiZiXsImGouWetgzzEnrwdQ.Breakable;

                physicsWorld.ParticleData.PhysicsMaterialIndex[sSXaQzSIStksZVavWoAUYLvGjGNtCMdO] = hsnSzQnwTxiZiXsImGouWetgzzEnrwdQ.PhysicsMaterialIndex;

                var nSEHLWbSSEOPlpyuNFAVzqwWPRFwHmIY = math.mul(GiMXkcoswCyzweDQypPTAxGOOidScabx, new float4(hsnSzQnwTxiZiXsImGouWetgzzEnrwdQ.Position, 1.0f)).xyz;
                physicsWorld.ParticleData.InitialPosition[sSXaQzSIStksZVavWoAUYLvGjGNtCMdO] = nSEHLWbSSEOPlpyuNFAVzqwWPRFwHmIY;
                physicsWorld.ParticleData.Position[sSXaQzSIStksZVavWoAUYLvGjGNtCMdO] = nSEHLWbSSEOPlpyuNFAVzqwWPRFwHmIY;
                physicsWorld.ParticleData.PredictedPosition[sSXaQzSIStksZVavWoAUYLvGjGNtCMdO] = nSEHLWbSSEOPlpyuNFAVzqwWPRFwHmIY;
                physicsWorld.ParticleData.Velocity[sSXaQzSIStksZVavWoAUYLvGjGNtCMdO] = hsnSzQnwTxiZiXsImGouWetgzzEnrwdQ.Velocity;

                physicsWorld.ParticleData.InverseMass[sSXaQzSIStksZVavWoAUYLvGjGNtCMdO] = hsnSzQnwTxiZiXsImGouWetgzzEnrwdQ.Kinematic ? 0 : hsnSzQnwTxiZiXsImGouWetgzzEnrwdQ.InverseMass;
                physicsWorld.ParticleData.AdjustedInverseMass[sSXaQzSIStksZVavWoAUYLvGjGNtCMdO] = physicsWorld.ParticleData.InverseMass[sSXaQzSIStksZVavWoAUYLvGjGNtCMdO];
                physicsWorld.ParticleData.Phase[sSXaQzSIStksZVavWoAUYLvGjGNtCMdO] = sKsuITiwiZVCvDErTpdnCEECoFJJytRK;

                physicsWorld.ParticleData.ParticleLocks[sSXaQzSIStksZVavWoAUYLvGjGNtCMdO] = 0;
                physicsWorld.ParticleData.PositionCorrectionsCount[sSXaQzSIStksZVavWoAUYLvGjGNtCMdO] = 0;
                physicsWorld.ParticleData.PositionCorrections[sSXaQzSIStksZVavWoAUYLvGjGNtCMdO] = float3.zero;

                
                physicsWorld.ParticleData.SimulationMeshDescriptorIndex[sSXaQzSIStksZVavWoAUYLvGjGNtCMdO] = GmZhEMoYGnDjHSASnZcBozSupwIJQVqz;

                if (hsnSzQnwTxiZiXsImGouWetgzzEnrwdQ.Kinematic)
                {
                    physicsWorld.SimulationMeshDescriptorData.ContainsParticlesWithInfiniteMass[GmZhEMoYGnDjHSASnZcBozSupwIJQVqz] = true;
                }
            }

            unsafe
            {
                var rtmPowoXeZQFZKuFEsbNvRPmQXMblLup = new CalculateParticlesContributionToCoMsJob
                {
                    ParticlesEnabled = physicsWorld.ParticleData.Enabled,
                    ParticlesPredictedPosition = physicsWorld.ParticleData.PredictedPosition,
                    ParticlesInverseMass = physicsWorld.ParticleData.InverseMass,
                    ParticlesSimulationMeshDescriptorIndex = physicsWorld.ParticleData.SimulationMeshDescriptorIndex,

                    SimulationMeshLocks = physicsWorld.SimulationMeshDescriptorData.Locks,
                    SimulationMeshContainsParticlesWithInfiniteMass = physicsWorld.SimulationMeshDescriptorData.ContainsParticlesWithInfiniteMass,
                    SimulationMeshTotalMass = physicsWorld.SimulationMeshDescriptorData.TotalMass,
                    SimulationMeshCoM = physicsWorld.SimulationMeshDescriptorData.CenterOfMassPosition,
                };

                var DZmOklQItdUxivsYmxpuRJfogGyVADfV = new AverageCoMContributionsJob
                {
                    SimulationMeshTotalMass = physicsWorld.SimulationMeshDescriptorData.TotalMass,
                    SimulationMeshCoM = physicsWorld.SimulationMeshDescriptorData.CenterOfMassPosition,
                };

                
                JobHandle tRALMtMBakHVHrYsvjeCOXNAhvEKsyXr = rtmPowoXeZQFZKuFEsbNvRPmQXMblLup.Schedule(physicsWorld.ParticleData.Count, 64);
                tRALMtMBakHVHrYsvjeCOXNAhvEKsyXr = DZmOklQItdUxivsYmxpuRJfogGyVADfV.Schedule(physicsWorld.SimulationMeshDescriptorData.Count, 64, tRALMtMBakHVHrYsvjeCOXNAhvEKsyXr);
                tRALMtMBakHVHrYsvjeCOXNAhvEKsyXr.Complete();

                
                physicsWorld.SimulationMeshDescriptorData.CovarianceMatrix[GmZhEMoYGnDjHSASnZcBozSupwIJQVqz] = float3x3.zero;
            }

            float3 eBmQJhkjSmyEXwPZQhnAFgXQgVvrmyed = physicsWorld.SimulationMeshDescriptorData.CenterOfMassPosition[GmZhEMoYGnDjHSASnZcBozSupwIJQVqz];

            
            for (int arpstEdMIpJhupBWhWQPbuiVaEPtiqOX = 0; arpstEdMIpJhupBWhWQPbuiVaEPtiqOX < iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.Particles.Length; arpstEdMIpJhupBWhWQPbuiVaEPtiqOX++)
            {
                var bCOVUnNQQnXHzIaQdMstqwLhlJsPdvjx = arpstEdMIpJhupBWhWQPbuiVaEPtiqOX + LIWwXRHXwdvZFhVxkUmabqQLXiGUFZgv;

                physicsWorld.ParticleData.LocalRestPosition[bCOVUnNQQnXHzIaQdMstqwLhlJsPdvjx] = physicsWorld.ParticleData.InitialPosition[bCOVUnNQQnXHzIaQdMstqwLhlJsPdvjx] - eBmQJhkjSmyEXwPZQhnAFgXQgVvrmyed;
            }

            
            physicsWorld.SpatialHashGrid.SetItemsCount(physicsWorld.ParticleData.Count);

            
            for (int HxKbHmVRoSvBktzMXUDiOlFtfLdBHTaa = 0; HxKbHmVRoSvBktzMXUDiOlFtfLdBHTaa < iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.DistanceConstraints.Length; HxKbHmVRoSvBktzMXUDiOlFtfLdBHTaa++)
            {
                DistanceConstraint fJmBiFnTLFvYnDdkkkBmLEMZbXdHbUEi = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.DistanceConstraints[HxKbHmVRoSvBktzMXUDiOlFtfLdBHTaa];

                var immbTirzFNezgbYAhtznyZLEsYvAtzCe = HxKbHmVRoSvBktzMXUDiOlFtfLdBHTaa + XgVawokuFZlWsxCUZBGilzPmZUlYBkKQ;

                physicsWorld.DistanceConstraintData.Enabled[immbTirzFNezgbYAhtznyZLEsYvAtzCe] = true;
                physicsWorld.DistanceConstraintData.Breakable[immbTirzFNezgbYAhtznyZLEsYvAtzCe] = fJmBiFnTLFvYnDdkkkBmLEMZbXdHbUEi.Breakable;

                physicsWorld.DistanceConstraintData.PhysicsMaterialIndex[immbTirzFNezgbYAhtznyZLEsYvAtzCe] = fJmBiFnTLFvYnDdkkkBmLEMZbXdHbUEi.PhysicsMaterialIndex;

                physicsWorld.DistanceConstraintData.ParticleA[immbTirzFNezgbYAhtznyZLEsYvAtzCe] = fJmBiFnTLFvYnDdkkkBmLEMZbXdHbUEi.ParticleA;
                physicsWorld.DistanceConstraintData.ParticleB[immbTirzFNezgbYAhtznyZLEsYvAtzCe] = fJmBiFnTLFvYnDdkkkBmLEMZbXdHbUEi.ParticleB;

                var kOIMzHRXxYXmLPajOPejDfdopDLreGls = math.distance(physicsWorld.ParticleData.InitialPosition[fJmBiFnTLFvYnDdkkkBmLEMZbXdHbUEi.ParticleA + LIWwXRHXwdvZFhVxkUmabqQLXiGUFZgv], physicsWorld.ParticleData.InitialPosition[fJmBiFnTLFvYnDdkkkBmLEMZbXdHbUEi.ParticleB + LIWwXRHXwdvZFhVxkUmabqQLXiGUFZgv]);
                physicsWorld.DistanceConstraintData.RestDistance[immbTirzFNezgbYAhtznyZLEsYvAtzCe] = kOIMzHRXxYXmLPajOPejDfdopDLreGls;

                physicsWorld.DistanceConstraintData.SimulationMeshDescriptorIndex[immbTirzFNezgbYAhtznyZLEsYvAtzCe] = GmZhEMoYGnDjHSASnZcBozSupwIJQVqz;
            }

            
            for (int TAPwGZRJsllGetPVFgffXdImMMhdHiJW = 0; TAPwGZRJsllGetPVFgffXdImMMhdHiJW < iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.VolumeConstraints.Length; TAPwGZRJsllGetPVFgffXdImMMhdHiJW++)
            {
                VolumeConstraint OmzZYoiuQFZAcJbghkoMUlbyfhFPARSm = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.VolumeConstraints[TAPwGZRJsllGetPVFgffXdImMMhdHiJW];

                var efYHezDJobmjjcziKkkLzGZFTmJiHrDf = TAPwGZRJsllGetPVFgffXdImMMhdHiJW + vlhfopiljWZobbokWCFMvzEVPHyWcDXL;

                physicsWorld.VolumeConstraintData.Enabled[efYHezDJobmjjcziKkkLzGZFTmJiHrDf] = true;
                physicsWorld.VolumeConstraintData.Breakable[efYHezDJobmjjcziKkkLzGZFTmJiHrDf] = OmzZYoiuQFZAcJbghkoMUlbyfhFPARSm.Breakable;

                physicsWorld.VolumeConstraintData.PhysicsMaterialIndex[efYHezDJobmjjcziKkkLzGZFTmJiHrDf] = OmzZYoiuQFZAcJbghkoMUlbyfhFPARSm.PhysicsMaterialIndex;

                physicsWorld.VolumeConstraintData.ParticleA[efYHezDJobmjjcziKkkLzGZFTmJiHrDf] = OmzZYoiuQFZAcJbghkoMUlbyfhFPARSm.ParticleA;
                physicsWorld.VolumeConstraintData.ParticleB[efYHezDJobmjjcziKkkLzGZFTmJiHrDf] = OmzZYoiuQFZAcJbghkoMUlbyfhFPARSm.ParticleB;
                physicsWorld.VolumeConstraintData.ParticleC[efYHezDJobmjjcziKkkLzGZFTmJiHrDf] = OmzZYoiuQFZAcJbghkoMUlbyfhFPARSm.ParticleC;
                physicsWorld.VolumeConstraintData.ParticleD[efYHezDJobmjjcziKkkLzGZFTmJiHrDf] = OmzZYoiuQFZAcJbghkoMUlbyfhFPARSm.ParticleD;

                var hZqzSYPCptclYwteMaCVCYNIOpzbAcvi = Utilities.Math.TetrahedronVolume(
                                       physicsWorld.ParticleData.InitialPosition[OmzZYoiuQFZAcJbghkoMUlbyfhFPARSm.ParticleA + LIWwXRHXwdvZFhVxkUmabqQLXiGUFZgv],
                                       physicsWorld.ParticleData.InitialPosition[OmzZYoiuQFZAcJbghkoMUlbyfhFPARSm.ParticleB + LIWwXRHXwdvZFhVxkUmabqQLXiGUFZgv],
                                       physicsWorld.ParticleData.InitialPosition[OmzZYoiuQFZAcJbghkoMUlbyfhFPARSm.ParticleC + LIWwXRHXwdvZFhVxkUmabqQLXiGUFZgv],
                                       physicsWorld.ParticleData.InitialPosition[OmzZYoiuQFZAcJbghkoMUlbyfhFPARSm.ParticleD + LIWwXRHXwdvZFhVxkUmabqQLXiGUFZgv]);

                physicsWorld.VolumeConstraintData.RestVolume[efYHezDJobmjjcziKkkLzGZFTmJiHrDf] = hZqzSYPCptclYwteMaCVCYNIOpzbAcvi;

                physicsWorld.VolumeConstraintData.SimulationMeshDescriptorIndex[efYHezDJobmjjcziKkkLzGZFTmJiHrDf] = GmZhEMoYGnDjHSASnZcBozSupwIJQVqz;
            }

            
            for (int itOslkKbTidXTIpSeQVrcSwZaxsrbeeP = 0; itOslkKbTidXTIpSeQVrcSwZaxsrbeeP < iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.CollinearConstraints.Length; itOslkKbTidXTIpSeQVrcSwZaxsrbeeP++)
            {
                CollinearConstraint JVVrPlvldMsotIzbLKBNMpRmTbEcZkgb = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.CollinearConstraints[itOslkKbTidXTIpSeQVrcSwZaxsrbeeP];

                var yPqrtmuaLevyYlXPTyWTAuFJxiyvmRCu = itOslkKbTidXTIpSeQVrcSwZaxsrbeeP + MxCEImuTSamHgbbAzRVYfIvuMcHgddTl;

                physicsWorld.CollinearConstraintData.Enabled[yPqrtmuaLevyYlXPTyWTAuFJxiyvmRCu] = true;
                physicsWorld.CollinearConstraintData.Breakable[yPqrtmuaLevyYlXPTyWTAuFJxiyvmRCu] = JVVrPlvldMsotIzbLKBNMpRmTbEcZkgb.Breakable;

                physicsWorld.CollinearConstraintData.PhysicsMaterialIndex[yPqrtmuaLevyYlXPTyWTAuFJxiyvmRCu] = JVVrPlvldMsotIzbLKBNMpRmTbEcZkgb.PhysicsMaterialIndex;

                physicsWorld.CollinearConstraintData.ParticleA[yPqrtmuaLevyYlXPTyWTAuFJxiyvmRCu] = JVVrPlvldMsotIzbLKBNMpRmTbEcZkgb.ParticleA;
                physicsWorld.CollinearConstraintData.ParticleB[yPqrtmuaLevyYlXPTyWTAuFJxiyvmRCu] = JVVrPlvldMsotIzbLKBNMpRmTbEcZkgb.ParticleB;
                physicsWorld.CollinearConstraintData.ParticleC[yPqrtmuaLevyYlXPTyWTAuFJxiyvmRCu] = JVVrPlvldMsotIzbLKBNMpRmTbEcZkgb.ParticleC;

                physicsWorld.CollinearConstraintData.SimulationMeshDescriptorIndex[yPqrtmuaLevyYlXPTyWTAuFJxiyvmRCu] = GmZhEMoYGnDjHSASnZcBozSupwIJQVqz;
            }

            
            for (int SGgtQNNIibqPEPJOeMzqKqeDcUGgNXUU = 0; SGgtQNNIibqPEPJOeMzqKqeDcUGgNXUU < iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.ShapeMatchingConstraints.Length; SGgtQNNIibqPEPJOeMzqKqeDcUGgNXUU++)
            {
                ShapeMatchingConstraint ArfDLxImyoETereIQboFsNTwdRRyivfM = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.ShapeMatchingConstraints[SGgtQNNIibqPEPJOeMzqKqeDcUGgNXUU];

                var FHlopelYBvBabEOFToCBagVjLCOJcvBl = SGgtQNNIibqPEPJOeMzqKqeDcUGgNXUU + XfxyReKHkxoETBvcpWjkFijYLOfrYzpY;

                physicsWorld.ShapeMatchingConstraintData.Enabled[FHlopelYBvBabEOFToCBagVjLCOJcvBl] = true;
                physicsWorld.ShapeMatchingConstraintData.Breakable[FHlopelYBvBabEOFToCBagVjLCOJcvBl] = ArfDLxImyoETereIQboFsNTwdRRyivfM.Breakable;
                physicsWorld.ShapeMatchingConstraintData.Particle[FHlopelYBvBabEOFToCBagVjLCOJcvBl] = ArfDLxImyoETereIQboFsNTwdRRyivfM.Particle;
                physicsWorld.ShapeMatchingConstraintData.PhysicsMaterialIndex[FHlopelYBvBabEOFToCBagVjLCOJcvBl] = ArfDLxImyoETereIQboFsNTwdRRyivfM.PhysicsMaterialIndex;
                physicsWorld.ShapeMatchingConstraintData.SimulationMeshDescriptorIndex[FHlopelYBvBabEOFToCBagVjLCOJcvBl] = GmZhEMoYGnDjHSASnZcBozSupwIJQVqz;
            }

            physicsWorld.SimulationMeshToDescriptorMap[iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR] = GmZhEMoYGnDjHSASnZcBozSupwIJQVqz;
            physicsWorld.DescriptorToSimulationMeshMap[GmZhEMoYGnDjHSASnZcBozSupwIJQVqz] = iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR;

            physicsWorld.ReAssignDataSlicesToSimulationMeshes();

            iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.OnAddedToPhysicsWorld(physicsWorld, GmZhEMoYGnDjHSASnZcBozSupwIJQVqz, sKsuITiwiZVCvDErTpdnCEECoFJJytRK);

            UnityEngine.Debug.Log($"Added simulation mesh {iuzgkcolIBvjMpOAaKaKeDsLCSlXWHCR.name} to physics world [P, DC, VC, SMC]= [{physicsWorld.ParticleData.Count}, {physicsWorld.DistanceConstraintData.Count}, {physicsWorld.VolumeConstraintData.Count}, {physicsWorld.ShapeMatchingConstraintData.Count}]");
        }
    }
}
