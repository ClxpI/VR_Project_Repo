namespace MAGES.MeshDeformations
{
    using Unity.Collections;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Serialization;

    /// <summary>
    /// A joint trying to keep two particles a certain distance apart.
    /// </summary>
    public class DistanceJoint : MonoBehaviour
    {
        private PhysicsWorld CYWLmQEqxNHWgyjMolMlluAqlTsIPixI;
        private DistanceJointSolver QpSrttbRAiBRSbeTjsyAUlymjGFVUXXj;

        [SerializeField]
        [HideInInspector]
        private SimulationMesh simulationMesh;

        [SerializeField]
        [HideInInspector]
        private SimulationMesh connectedSimulationMesh;

        private bool hIREmmTEzNdzsOUgzrsACpVxbSCuEwnp = false;

        
        private NativeSlice<bool> LBtMOTSrhdlUhVOSZvnywiykDSGavUJi;
        private NativeSlice<int> mEjXOobCHnApTragZgZuNqWAbtGweZgV;
        private NativeSlice<int> bThFisItkNbwgZcJPLAzwXsWumNGyYnP;
        private NativeSlice<float> nngKxjUSsbrRaGxsZZaBtzXBAwfXwZiX;
        private NativeSlice<float> OzWJyuwbDGytRARyNAgARzVbQIggmEyK;
        private NativeSlice<bool> POCUwYdiqiTDSEOyKlCrleUzcYgqyfQQ;
        private NativeSlice<float> HemDeRKoejULLyABuUZOOHzDPcCaeXvP;

        /// <summary>
        /// Gets or sets the actor this joint is connected to.
        /// </summary>
        [field: SerializeField]
        public BasePhysicsActor Actor { get; set; }

        /// <summary>
        /// Gets or sets the simulation mesh this joint is connected to.
        /// </summary>
        public SimulationMesh SimulationMesh
        {
            get
            {
                if (Actor != null && Actor.SimulationMesh != simulationMesh)
                {
                    if (simulationMesh != null)
                    {
                        DMZEAcMhshPqpyUeYdGAafKrcZGpCXig();
                    }

                    simulationMesh = Actor.SimulationMesh;

                    if (QpSrttbRAiBRSbeTjsyAUlymjGFVUXXj == null)
                    {
                        EcdjqDGbfgvzmcYvLSbAuxoZNDZZQgYa();
                    }
                }

                return simulationMesh;
            }

            set
            {
                DMZEAcMhshPqpyUeYdGAafKrcZGpCXig();
                simulationMesh = value;
                if (QpSrttbRAiBRSbeTjsyAUlymjGFVUXXj == null)
                {
                    EcdjqDGbfgvzmcYvLSbAuxoZNDZZQgYa();
                }
            }
        }

        /// <summary>
        /// Gets or sets the index of the particle in the <see cref="Actor"/> this joint is connected to.
        /// </summary>
        [field: SerializeField]
        [field: FormerlySerializedAs("<ActorParticleIndex>k__BackingField")]
        public int SimulationMeshParticleIndex { get; set; }

        /// <summary>
        /// Gets or sets the other actor this joint is connected to.
        /// </summary>
        [field: SerializeField]
        public BasePhysicsActor ConnectedActor { get; set; }

        /// <summary>
        /// Gets or sets the other simulation mesh this joint is connected to.
        /// </summary>
        public SimulationMesh ConnectedSimulationMesh
        {
            get
            {
                if (ConnectedActor != null && ConnectedActor.SimulationMesh != connectedSimulationMesh)
                {
                    if (connectedSimulationMesh != null)
                    {
                        DMZEAcMhshPqpyUeYdGAafKrcZGpCXig();
                    }

                    connectedSimulationMesh = ConnectedActor.SimulationMesh;

                    if (QpSrttbRAiBRSbeTjsyAUlymjGFVUXXj == null)
                    {
                        EcdjqDGbfgvzmcYvLSbAuxoZNDZZQgYa();
                    }
                }

                return connectedSimulationMesh;
            }

            set
            {
                DMZEAcMhshPqpyUeYdGAafKrcZGpCXig();
                connectedSimulationMesh = value;
                if (QpSrttbRAiBRSbeTjsyAUlymjGFVUXXj == null)
                {
                    EcdjqDGbfgvzmcYvLSbAuxoZNDZZQgYa();
                }
            }
        }

        /// <summary>
        /// Gets or sets the index of the particle in the <see cref="ConnectedActor"/> this joint is connected to.
        /// </summary>
        [field: SerializeField]
        [field: FormerlySerializedAs("<ConnectedActorParticleIndex>k__BackingField")]
        public int ConnectedSimulationMeshParticleIndex { get; set; }

        /// <summary>
        /// Gets or sets the distane the solver will try to achieve between the two particles.
        /// </summary>
        [field: SerializeField]
        public float TargetDistance { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the constraint's compliance.
        /// </summary>
        [field: SerializeField]
        public float Compliance { get; set; } = 0f;

        /// <summary>
        /// Gets or sets a value indicating whether this joint is breakable.
        /// </summary>
        [field: SerializeField]
        public bool Breakable { get; set; } = false;

        /// <summary>
        /// Gets or sets the force at which this joint will break, if it is breakable.
        /// </summary>
        [field: SerializeField]
        public float BreakForce { get; set; } = 1000f;

        /// <summary>
        /// Updates the local slice references.
        /// </summary>
        /// <param name="enabled">Enabled.</param>
        /// <param name="particleA">Particle A.</param>
        /// <param name="particleB">Particle B.</param>
        /// <param name="restLength">RestLength.</param>
        /// <param name="compliance">Compliance.</param>
        /// <param name="breakable">Breakable.</param>
        /// <param name="breakForce">BreakForce.</param>
        internal void UpdateSlices(NativeSlice<bool> enabled, NativeSlice<int> particleA, NativeSlice<int> particleB, NativeSlice<float> restLength, NativeSlice<float> compliance, NativeSlice<bool> breakable, NativeSlice<float> breakForce)
        {
            LBtMOTSrhdlUhVOSZvnywiykDSGavUJi = enabled;
            mEjXOobCHnApTragZgZuNqWAbtGweZgV = particleA;
            bThFisItkNbwgZcJPLAzwXsWumNGyYnP = particleB;
            nngKxjUSsbrRaGxsZZaBtzXBAwfXwZiX = restLength;
            OzWJyuwbDGytRARyNAgARzVbQIggmEyK = compliance;
            POCUwYdiqiTDSEOyKlCrleUzcYgqyfQQ = breakable;
            HemDeRKoejULLyABuUZOOHzDPcCaeXvP = breakForce;
        }

        /// <summary>
        /// Called by the joint solver, when this is finally added to it.
        /// </summary>
        internal void OnJointRegistered()
        {
            
            bVnwUlMepwtYgfxazdUdnxWJerMWCMGr();
        }

        private void pZgkaQroluusQYSQLaAJykmhqLiUsVhY()
        {
            EbkvDJnSvYZTumrmIMvMCVSbigNIxKvB();
            if (QpSrttbRAiBRSbeTjsyAUlymjGFVUXXj == null)
            {
                EcdjqDGbfgvzmcYvLSbAuxoZNDZZQgYa();
            }
        }

        private void VdYBtTOeJVExAQQbDEVOEZGmlHJDkCPI()
        {
            XeVDEaWALoHBHHenLVYWzwPMKkOYqHHU();
            DMZEAcMhshPqpyUeYdGAafKrcZGpCXig();
        }

        private void cRuSnvhfsGMTAqWxHnXYnRCUXPrVMWxF()
        {
            Actor = GetComponent<BasePhysicsActor>();
        }

        private void zfhIvzzOlcTsITNkVeumeDHedarjUEPP(float timeStep)
        {
            if (!enabled)
            {
                return;
            }

            
            if (LBtMOTSrhdlUhVOSZvnywiykDSGavUJi[0] == false)
            {
                gameObject.SendMessage("OnDistanceJointBreak", this, SendMessageOptions.DontRequireReceiver);
                DestroyImmediate(this);
            }
            else if (hIREmmTEzNdzsOUgzrsACpVxbSCuEwnp)
            {
                bVnwUlMepwtYgfxazdUdnxWJerMWCMGr();
                hIREmmTEzNdzsOUgzrsACpVxbSCuEwnp = false;
            }
        }

        private void EbkvDJnSvYZTumrmIMvMCVSbigNIxKvB()
        {
            
            if (SimulationMesh != null)
            {
                SimulationMesh.AddedToPhysicsWorld.AddListener(qrEEHhRmmxnFLJnQycNYmaZQYuBPUuWO);
                SimulationMesh.RemovedFromPhysicsWorld.AddListener(RqOmFcjKOCzjFqxqFfvmQXfYVOqfrVgV);
            }

            if (ConnectedSimulationMesh != null)
            {
                ConnectedSimulationMesh.AddedToPhysicsWorld.AddListener(qrEEHhRmmxnFLJnQycNYmaZQYuBPUuWO);
                ConnectedSimulationMesh.RemovedFromPhysicsWorld.AddListener(RqOmFcjKOCzjFqxqFfvmQXfYVOqfrVgV);
            }
        }

        private void XeVDEaWALoHBHHenLVYWzwPMKkOYqHHU()
        {
            
            if (SimulationMesh != null)
            {
                SimulationMesh.AddedToPhysicsWorld.RemoveListener(qrEEHhRmmxnFLJnQycNYmaZQYuBPUuWO);
                SimulationMesh.RemovedFromPhysicsWorld.RemoveListener(RqOmFcjKOCzjFqxqFfvmQXfYVOqfrVgV);
            }

            if (ConnectedSimulationMesh != null)
            {
                ConnectedSimulationMesh.AddedToPhysicsWorld.RemoveListener(qrEEHhRmmxnFLJnQycNYmaZQYuBPUuWO);
                ConnectedSimulationMesh.RemovedFromPhysicsWorld.RemoveListener(RqOmFcjKOCzjFqxqFfvmQXfYVOqfrVgV);
            }
        }

        private void qrEEHhRmmxnFLJnQycNYmaZQYuBPUuWO()
        {
            if (QpSrttbRAiBRSbeTjsyAUlymjGFVUXXj == null)
            {
                EcdjqDGbfgvzmcYvLSbAuxoZNDZZQgYa();
            }
        }

        private void RqOmFcjKOCzjFqxqFfvmQXfYVOqfrVgV()
        {
            DMZEAcMhshPqpyUeYdGAafKrcZGpCXig();
        }

        private bool KUQwDfyGdVtGuwGoJUQGPYrPufOBufiW()
        {
            if (SimulationMesh == null || ConnectedSimulationMesh == null)
            {
                return false;
            }

            if (!SimulationMesh.IsPartOfPhysicsWorld || !ConnectedSimulationMesh.IsPartOfPhysicsWorld)
            {
                return false;
            }

            if (SimulationMesh.PhysicsWorld != ConnectedSimulationMesh.PhysicsWorld)
            {
                return false;
            }

            return true;
        }

        private void EcdjqDGbfgvzmcYvLSbAuxoZNDZZQgYa()
        {
            if (!KUQwDfyGdVtGuwGoJUQGPYrPufOBufiW())
            {
                return;
            }

            if (QpSrttbRAiBRSbeTjsyAUlymjGFVUXXj != null)
            {
                return;
            }

            CYWLmQEqxNHWgyjMolMlluAqlTsIPixI = SimulationMesh.PhysicsWorld;

            if (CYWLmQEqxNHWgyjMolMlluAqlTsIPixI == null)
            {
                return;
            }

            QpSrttbRAiBRSbeTjsyAUlymjGFVUXXj = DistanceJointSolver.GetOrCreateSolverOfWorld(CYWLmQEqxNHWgyjMolMlluAqlTsIPixI);
            QpSrttbRAiBRSbeTjsyAUlymjGFVUXXj.RegisterJoint(this);

            CYWLmQEqxNHWgyjMolMlluAqlTsIPixI.PhysicsUpdate?.AddListener(zfhIvzzOlcTsITNkVeumeDHedarjUEPP);
        }

        private void DMZEAcMhshPqpyUeYdGAafKrcZGpCXig()
        {
            CYWLmQEqxNHWgyjMolMlluAqlTsIPixI?.PhysicsUpdate?.RemoveListener(zfhIvzzOlcTsITNkVeumeDHedarjUEPP);

            if (QpSrttbRAiBRSbeTjsyAUlymjGFVUXXj != null)
            {
                QpSrttbRAiBRSbeTjsyAUlymjGFVUXXj.DeregisterJoint(this);

                CYWLmQEqxNHWgyjMolMlluAqlTsIPixI = null;
            }

            QpSrttbRAiBRSbeTjsyAUlymjGFVUXXj = null;
        }

        private void qqxkcbRQMbxkfSFKbUavpeAyHtmjzdhi()
        {
            if (QpSrttbRAiBRSbeTjsyAUlymjGFVUXXj != null)
            {
                hIREmmTEzNdzsOUgzrsACpVxbSCuEwnp = true;
            }
        }

        private void bVnwUlMepwtYgfxazdUdnxWJerMWCMGr()
        {
            LBtMOTSrhdlUhVOSZvnywiykDSGavUJi[0] = true;
            mEjXOobCHnApTragZgZuNqWAbtGweZgV[0] = CYWLmQEqxNHWgyjMolMlluAqlTsIPixI.SimulationMeshDescriptorData.SimulationMeshDescriptors[SimulationMesh.SimulationMeshDescriptorIndex].ParticleStartIndex + SimulationMeshParticleIndex;
            bThFisItkNbwgZcJPLAzwXsWumNGyYnP[0] = CYWLmQEqxNHWgyjMolMlluAqlTsIPixI.SimulationMeshDescriptorData.SimulationMeshDescriptors[ConnectedSimulationMesh.SimulationMeshDescriptorIndex].ParticleStartIndex + ConnectedSimulationMeshParticleIndex;
            OzWJyuwbDGytRARyNAgARzVbQIggmEyK[0] = Compliance;
            nngKxjUSsbrRaGxsZZaBtzXBAwfXwZiX[0] = TargetDistance;
            POCUwYdiqiTDSEOyKlCrleUzcYgqyfQQ[0] = Breakable;
            HemDeRKoejULLyABuUZOOHzDPcCaeXvP[0] = BreakForce;
        }
    }
}
