namespace MAGES.MeshDeformations.Tools
{
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using static UnityEngine.PlayerLoop.PreUpdate;

    /// <summary>
    /// A base class to be used for all detectors, used for detecting changes in <see cref="SimulationMesh"/>.
    /// </summary>
    [RequireComponent(typeof(BasePhysicsActor))]
    public abstract class BaseDetector : MonoBehaviour
    {
        private BasePhysicsActor nEQwerVgGkmEMWMbnHstvgLHrDUFVmXv;
        private PhysicsWorld VHstYKLxiNkuRDOgPChMwRXBKTSKmlmG;

        /// <summary>
        /// Gets the <see cref="BasePhysicsActor"/> component attached to the same GameObject.
        /// </summary>
        public BasePhysicsActor Actor
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (nEQwerVgGkmEMWMbnHstvgLHrDUFVmXv == null)
                {
                    nEQwerVgGkmEMWMbnHstvgLHrDUFVmXv = GetComponent<BasePhysicsActor>();
                }

                return nEQwerVgGkmEMWMbnHstvgLHrDUFVmXv;
            }
        }

        /// <summary>
        /// Gets the PhysicsWorld this detector is used for.
        /// </summary>
        protected PhysicsWorld PhysicsWorld
        {
            get
            {
                return VHstYKLxiNkuRDOgPChMwRXBKTSKmlmG;
            }
        }

        /// <summary>
        /// Called when the simulation mesh is actually added to the physics world.
        /// </summary>
        protected virtual void OnSimulationMeshAddedToPhysicsWorld()
        {
            VHstYKLxiNkuRDOgPChMwRXBKTSKmlmG = Actor.SimulationMesh.PhysicsWorld;
            PhysicsWorld.PhysicsUpdate.AddListener(PhysicsUpdate);
            PhysicsWorld.LatePhysicsUpdate.AddListener(LatePhysicsUpdate);
            PhysicsWorld.DetectorsUpdate.AddListener(DetectorUpdate);
        }

        /// <summary>
        /// Called when the simulation mesh is actually removed from the physics world.
        /// </summary>
        protected virtual void OnSimulationMeshRemovedFromPhysicsWorld()
        {
            PhysicsWorld.PhysicsUpdate.RemoveListener(PhysicsUpdate);
            PhysicsWorld.LatePhysicsUpdate.RemoveListener(LatePhysicsUpdate);
            PhysicsWorld.DetectorsUpdate.RemoveListener(DetectorUpdate);
            VHstYKLxiNkuRDOgPChMwRXBKTSKmlmG = null;
        }

        /// <summary>
        /// Called when the physics world is updated.
        /// </summary>
        /// <param name="deltaTime">The time since last physics update.</param>
        protected virtual void PhysicsUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// Called immediately after <see cref="PhysicsUpdate(float)"/>.
        /// </summary>
        /// <param name="deltaTime">The time since last physics update.</param>
        protected virtual void LatePhysicsUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// Use this for detecting changes in SimulationMesh.
        /// Called immediately after <see cref="LatePhysicsUpdate(float)"/>.
        /// </summary>
        /// <remarks>
        /// SimulationMesh modifications should not be done here, but rather in <see cref="PhysicsUpdate(float)"/> or <see cref="LatePhysicsUpdate(float)"/>.
        /// </remarks>
        protected virtual void DetectorUpdate()
        {
        }

        private void XHkOMrZJVhmclZVadcPRNTQifAPACtwx()
        {
            if (Actor.SimulationMesh)
            {
                var tnvvuWZVdtPMwsFsAzPgErHeVhcuSYoU = Actor.SimulationMesh;

                tnvvuWZVdtPMwsFsAzPgErHeVhcuSYoU.AddedToPhysicsWorld.AddListener(OnSimulationMeshAddedToPhysicsWorld);
                tnvvuWZVdtPMwsFsAzPgErHeVhcuSYoU.RemovedFromPhysicsWorld.AddListener(OnSimulationMeshRemovedFromPhysicsWorld);

                if (tnvvuWZVdtPMwsFsAzPgErHeVhcuSYoU.IsPartOfPhysicsWorld)
                {
                    OnSimulationMeshAddedToPhysicsWorld();
                }
            }
        }

        private void cHjkjoUtAdhWpKGBngKaUPakIqNdrajU()
        {
            if (Actor.SimulationMesh.IsPartOfPhysicsWorld)
            {
                Actor.SimulationMesh.PhysicsWorld.PhysicsUpdate.RemoveListener(PhysicsUpdate);
                Actor.SimulationMesh.PhysicsWorld.LatePhysicsUpdate.RemoveListener(LatePhysicsUpdate);
            }
        }

    }
}