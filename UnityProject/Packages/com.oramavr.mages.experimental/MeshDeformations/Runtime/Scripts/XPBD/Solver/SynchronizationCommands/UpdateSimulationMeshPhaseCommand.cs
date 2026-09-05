
namespace MAGES.MeshDeformations.Synchronization
{
    using System.Diagnostics;
    using Unity.Jobs;

    internal class UpdateSimulationMeshPhaseCommand : BaseCommand
    {
        private SimulationMesh DcfACpqAEXABhLPyWeTrGsdfqPOeOTiH;
        private uint weWWoMlJRupvRyIrLcSNYNfYOgHeoqrI;

        public UpdateSimulationMeshPhaseCommand(SimulationMesh simulationMesh, uint newPhase)
        {
            this.DcfACpqAEXABhLPyWeTrGsdfqPOeOTiH = simulationMesh;
            this.weWWoMlJRupvRyIrLcSNYNfYOgHeoqrI = newPhase;
        }

        public override void Apply(PhysicsWorld physicsWorld)
        {
            if (!physicsWorld.SimulationMeshToDescriptorMap.ContainsKey(DcfACpqAEXABhLPyWeTrGsdfqPOeOTiH))
            {
                return;
            }

            var rLpkFfjCvmpAVsYgNNsUMYNLWWjAXavj = physicsWorld.SimulationMeshToDescriptorMap[DcfACpqAEXABhLPyWeTrGsdfqPOeOTiH];

            SimulationMeshDescriptor sZzoUKOYkEFIuIpQLtOgBbmyCFEWXvSc = physicsWorld.SimulationMeshDescriptorData.SimulationMeshDescriptors[rLpkFfjCvmpAVsYgNNsUMYNLWWjAXavj];

            var htaOrXFHwLIsDgiYkYWjotDLIuIBaPLq = new UpdateParticlesPhaseJob
            {
                StartOffset = sZzoUKOYkEFIuIpQLtOgBbmyCFEWXvSc.ParticleStartIndex,
                NewPhase = weWWoMlJRupvRyIrLcSNYNfYOgHeoqrI,

                ParticlesPhase = physicsWorld.ParticleData.Phase
            };

            htaOrXFHwLIsDgiYkYWjotDLIuIBaPLq.Schedule(sZzoUKOYkEFIuIpQLtOgBbmyCFEWXvSc.ParticleCount, 64).Complete();
        }
    }
}
