namespace MAGES.MeshDeformations.Synchronization
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;

    /// <summary>
    /// Command to be queued to remove a simulation mesh from the physics world in the next synchronization step.
    /// </summary>
    internal class RemoveSimulationMeshCommand : BaseCommand
    {
        private SimulationMesh BBnWjaUNNCsOeCfSGQvZCZCTVzreOvaf;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveSimulationMeshCommand"/> class.
        /// </summary>
        /// <param name="simulationMesh">The simulationMesh.</param>
        public RemoveSimulationMeshCommand(SimulationMesh simulationMesh)
        {
            this.BBnWjaUNNCsOeCfSGQvZCZCTVzreOvaf = simulationMesh;
        }

        /// <inheritdoc/>
        public override void Apply(PhysicsWorld physicsWorld)
        {
            if (!physicsWorld.SimulationMeshToDescriptorMap.ContainsKey(BBnWjaUNNCsOeCfSGQvZCZCTVzreOvaf))
            {
                return;
            }

            
            
            
            
            
            
            var hfICqQrwTktBRxtOpYaowXQURMljAAwR = physicsWorld.SimulationMeshToDescriptorMap[BBnWjaUNNCsOeCfSGQvZCZCTVzreOvaf];

            UnityEngine.Debug.Log("Removing descriptor: " + hfICqQrwTktBRxtOpYaowXQURMljAAwR);

            SimulationMeshDescriptor iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl = physicsWorld.SimulationMeshDescriptorData.SimulationMeshDescriptors[hfICqQrwTktBRxtOpYaowXQURMljAAwR];

            
            Parallel.For(hfICqQrwTktBRxtOpYaowXQURMljAAwR + 1, physicsWorld.SimulationMeshDescriptorData.SimulationMeshDescriptors.Length, (i) =>
            {
                SimulationMeshDescriptor BeGYyqqCtUPADKUldQaojBVdohdpcOOI = physicsWorld.SimulationMeshDescriptorData.SimulationMeshDescriptors[i];
                BeGYyqqCtUPADKUldQaojBVdohdpcOOI.ParticleStartIndex -= iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.ParticleCount;
                BeGYyqqCtUPADKUldQaojBVdohdpcOOI.DistanceConstraintStartIndex -= iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.DistanceConstraintCount;
                BeGYyqqCtUPADKUldQaojBVdohdpcOOI.VolumeConstraintStartIndex -= iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.VolumeConstraintCount;
                BeGYyqqCtUPADKUldQaojBVdohdpcOOI.ShapeMatchingConstraintStartIndex -= iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.ShapeMatchingConstraintCount;
                BeGYyqqCtUPADKUldQaojBVdohdpcOOI.CollinearConstraintStartIndex -= iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.CollinearConstraintCount;
                BeGYyqqCtUPADKUldQaojBVdohdpcOOI.PhysicsMaterialStartIndex -= iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.PhysicsMaterialCount;

                physicsWorld.SimulationMeshDescriptorData.SimulationMeshDescriptors[i] = BeGYyqqCtUPADKUldQaojBVdohdpcOOI;
            });

            
            physicsWorld.SimulationMeshDescriptorData.RemoveAt(hfICqQrwTktBRxtOpYaowXQURMljAAwR);

            
            physicsWorld.ParticleData.RemoveRange(iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.ParticleStartIndex, iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.TotalParticleSpaceCount);
            physicsWorld.SpatialHashGrid.SetItemsCount(physicsWorld.ParticleData.Count);
            physicsWorld.DistanceConstraintData.RemoveRange(iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.DistanceConstraintStartIndex, iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.TotalDistanceConstraintSpaceCount);
            physicsWorld.VolumeConstraintData.RemoveRange(iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.VolumeConstraintStartIndex, iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.TotalVolumeConstraintSpaceCount);
            physicsWorld.ShapeMatchingConstraintData.RemoveRange(iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.ShapeMatchingConstraintStartIndex, iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.TotalShapeMatchingConstraintSpaceCount);
            physicsWorld.CollinearConstraintData.RemoveRange(iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.CollinearConstraintStartIndex, iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.TotalCollinearConstraintSpaceCount);

            
            EJOamVbFYmsjSROkORKeaoUXJoOgNtHS(hfICqQrwTktBRxtOpYaowXQURMljAAwR, physicsWorld);

            
            physicsWorld.PhysicsMaterials.RemoveRange(iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.PhysicsMaterialStartIndex, iIcEgMdMFRMLhlDEHlYmmyaVJccjacGl.TotalPhysicsMaterialSpaceCount, Unity.Collections.Allocator.Persistent);

            
            var sDgfnMjpdflqFckYdDDcjDXAGCNTqFgh = new Dictionary<SimulationMesh, int>(physicsWorld.SimulationMeshToDescriptorMap.Count);
            var hvdRcgWgZeBoDvBbSoqXfDhotlTAvCbV = new Dictionary<int, SimulationMesh>(physicsWorld.DescriptorToSimulationMeshMap.Count);

            
            foreach (var kv in physicsWorld.SimulationMeshToDescriptorMap)
            {
                if (kv.Key == BBnWjaUNNCsOeCfSGQvZCZCTVzreOvaf)
                {
                    continue;
                }

                int fcIITaTpBEHDwJpInSttWVDXGmpBONFh = kv.Value;
                if (fcIITaTpBEHDwJpInSttWVDXGmpBONFh > hfICqQrwTktBRxtOpYaowXQURMljAAwR)
                {
                    fcIITaTpBEHDwJpInSttWVDXGmpBONFh--;
                }

                sDgfnMjpdflqFckYdDDcjDXAGCNTqFgh[kv.Key] = fcIITaTpBEHDwJpInSttWVDXGmpBONFh;
                hvdRcgWgZeBoDvBbSoqXfDhotlTAvCbV[fcIITaTpBEHDwJpInSttWVDXGmpBONFh] = kv.Key;
            }

            physicsWorld.SimulationMeshToDescriptorMap = sDgfnMjpdflqFckYdDDcjDXAGCNTqFgh;
            physicsWorld.DescriptorToSimulationMeshMap = hvdRcgWgZeBoDvBbSoqXfDhotlTAvCbV;

            UnityEngine.Debug.Log($"Removed simulation mesh from physics world [P, DC, VC, SMC]= [{physicsWorld.ParticleData.Count}, {physicsWorld.DistanceConstraintData.Count}, {physicsWorld.VolumeConstraintData.Count}, {physicsWorld.ShapeMatchingConstraintData.Count}]");

            
            physicsWorld.ReAssignDataSlicesToSimulationMeshes();

            BBnWjaUNNCsOeCfSGQvZCZCTVzreOvaf.OnRemovedFromPhysicsWorld(physicsWorld);
        }

        private void EJOamVbFYmsjSROkORKeaoUXJoOgNtHS(in int removedMeshDescriptorIndex, PhysicsWorld physicsWorld)
        {
            var vtgEzkjXDecaXYmUIJqUBoxOJLkqgFrc = new TKPciBTlcqvYlBretcszHyziSkIDqqxb
            {
                RemovedDescriptorIndex = removedMeshDescriptorIndex,
                IndicesArray = physicsWorld.ParticleData.SimulationMeshDescriptorIndex,
            };

            var gSnMBKHtKkisqPWGbFHSKOeSIgIwMfjU = new TKPciBTlcqvYlBretcszHyziSkIDqqxb
            {
                RemovedDescriptorIndex = removedMeshDescriptorIndex,
                IndicesArray = physicsWorld.DistanceConstraintData.SimulationMeshDescriptorIndex,
            };

            var cjEGyGGpgkrBezWVrewlcAdGOiePWkvZ = new TKPciBTlcqvYlBretcszHyziSkIDqqxb
            {
                RemovedDescriptorIndex = removedMeshDescriptorIndex,
                IndicesArray = physicsWorld.VolumeConstraintData.SimulationMeshDescriptorIndex,
            };

            var IFYujvfuDpnkbEhxIFMNNhLMRqHTDNLP = new TKPciBTlcqvYlBretcszHyziSkIDqqxb
            {
                RemovedDescriptorIndex = removedMeshDescriptorIndex,
                IndicesArray = physicsWorld.ShapeMatchingConstraintData.SimulationMeshDescriptorIndex,
            };

            var qxriZNuCSDrWHVwqToTbhXBfxHItdDWU = new TKPciBTlcqvYlBretcszHyziSkIDqqxb
            {
                RemovedDescriptorIndex = removedMeshDescriptorIndex,
                IndicesArray = physicsWorld.CollinearConstraintData.SimulationMeshDescriptorIndex,
            };

            var qhkWgcNyWDnyJFXmjnZdGwbkDNuFDzpB = vtgEzkjXDecaXYmUIJqUBoxOJLkqgFrc.Schedule(physicsWorld.ParticleData.Count, 64);
            var RfuSBHZrXmAqebpvveDWmjxQSwvWALJD = gSnMBKHtKkisqPWGbFHSKOeSIgIwMfjU.Schedule(physicsWorld.DistanceConstraintData.Count, 64);
            var VRsHyYbpmKSxCPZHEkrICaXdIePFKhLB = cjEGyGGpgkrBezWVrewlcAdGOiePWkvZ.Schedule(physicsWorld.VolumeConstraintData.Count, 64);
            var XGJiGDHBkLaABbjoDbQPLSAPZgDMkVXp = IFYujvfuDpnkbEhxIFMNNhLMRqHTDNLP.Schedule(physicsWorld.ShapeMatchingConstraintData.Count, 64);
            var xMSygSCINluvSsRxDnDEViqZgGwFxZMl = qxriZNuCSDrWHVwqToTbhXBfxHItdDWU.Schedule(physicsWorld.CollinearConstraintData.Count, 64);

            JobHandle.CombineDependencies(JobHandle.CombineDependencies(qhkWgcNyWDnyJFXmjnZdGwbkDNuFDzpB, RfuSBHZrXmAqebpvveDWmjxQSwvWALJD), JobHandle.CombineDependencies(VRsHyYbpmKSxCPZHEkrICaXdIePFKhLB, XGJiGDHBkLaABbjoDbQPLSAPZgDMkVXp), xMSygSCINluvSsRxDnDEViqZgGwFxZMl).Complete();
        }

        [BurstCompile]
        private struct TKPciBTlcqvYlBretcszHyziSkIDqqxb : IJobParallelFor
        {
            public int RemovedDescriptorIndex;
            public NativeArray<int> IndicesArray;

            public void Execute(int index)
            {
                if (IndicesArray[index] > RemovedDescriptorIndex)
                {
                    IndicesArray[index] = IndicesArray[index] - 1;
                }
            }
        }
    }
}
