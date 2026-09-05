namespace MAGES.MeshDeformations
{
    using System.Collections.Generic;
    using MAGES.Utilities.Collections;
    using Unity.Collections;
    using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// A simple particle grabbing tool
    /// </summary>
    public class AdvancedParticleGrabber : PhysicsActor
    {
        private bool DOBzvNOVmPUurlRjbrhvcMuCOIZvPeNj = false;
        private bool kSJdAIBxDPpuUypiqvgEResqyeoKfhjb = false;

        private List<int> cynLNIJRDamSQYDFszuByWdlxQefLhPD = new List<int>();
        private List<DistanceJoint> LhNBNZzezSDnLRLtuLmIHYPujpFOFmDV = new List<DistanceJoint>();

        /// <summary>
        /// Gets or sets the distance at which the grabber will grab a particle
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("The distance at which the grabber will grab a particle.")]
        public float GrabRadius { get; set; } = 0.1f;

        /// <summary>
        /// Gets or sets the type of grab the grabber will perform.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("The type of grab the grabber will perform.")]
        public GrabType GrabType { get; set; } = GrabType.Closest;

        /// <summary>
        /// Gets or sets a value indicating whether the grabbed particle should be moved to the center of the grabber or keep the initial offset.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Whether the grabbed particle should be moved to the center of the grabber or keep the initial offset.")]
        public bool MoveToCenter { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the grabber should grab particles that are kinematic (with infinite mass).
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Whether the grabber should grab particles that are kinematic (with infinite mass).")]
        public bool GrabKinematicParticles { get; set; } = false;

        /// <summary>
        /// Gets or sets the layer at which the grabber will grab particles. If set to -1, the grabber will grab all particles.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("The layer at which the grabber will grab particles. If set to -1, the grabber will grab all particles.")]
        public int GrabLayer { get; set; } = -1;

        /// <summary>
        /// Gets or sets the event called when a grab is requested.
        /// </summary>
        [field: SerializeField]
        public UnityEvent OnGrab { get; set; } = new UnityEvent();

        /// <summary>
        /// Gets or sets the event called when a release is requested.
        /// </summary>
        [field: SerializeField]
        public UnityEvent OnRelease { get; set; } = new UnityEvent();

        /// <summary>
        /// Gets or sets the observed particles that will be grabbed.
        /// </summary>
        [field: SerializeField]
        [field:Tooltip("The observed particles. If any of these is grabbed a callback will be fired.")]
        public List<ObservedParticleGroup> ObservedParticles { get; set; } = new List<ObservedParticleGroup>();

        /// <summary>
        /// Grab the closest particle to the grabber.
        /// The grabbing will be done in the next physics update.
        /// </summary>
        [ContextMenu("Grab")]
        public void Grab()
        {
            kSJdAIBxDPpuUypiqvgEResqyeoKfhjb = false;
            DOBzvNOVmPUurlRjbrhvcMuCOIZvPeNj = true;
        }

        /// <summary>
        /// Released the grabbed particles if any.
        /// </summary>
        [ContextMenu("Release")]
        public void Release()
        {
            DOBzvNOVmPUurlRjbrhvcMuCOIZvPeNj = false;
            kSJdAIBxDPpuUypiqvgEResqyeoKfhjb = true;
        }

        /// <inheritdoc/>
        protected override void PhysicsUpdate(float timeStep)
        {
            base.PhysicsUpdate(timeStep);

            if (kSJdAIBxDPpuUypiqvgEResqyeoKfhjb)
            {
                for (int bsrjAiKlRcKvSKlhNaKAZbXHmFKZNdPZ = LhNBNZzezSDnLRLtuLmIHYPujpFOFmDV.Count - 1; bsrjAiKlRcKvSKlhNaKAZbXHmFKZNdPZ >= 0; bsrjAiKlRcKvSKlhNaKAZbXHmFKZNdPZ--)
                {
                    Destroy(LhNBNZzezSDnLRLtuLmIHYPujpFOFmDV[bsrjAiKlRcKvSKlhNaKAZbXHmFKZNdPZ]);
                }

                LhNBNZzezSDnLRLtuLmIHYPujpFOFmDV.Clear();

                
                for (int WMGrfXimhcwJhZUkoZzhFYUkCVcUErMh = 0; WMGrfXimhcwJhZUkoZzhFYUkCVcUErMh < ObservedParticles.Count; WMGrfXimhcwJhZUkoZzhFYUkCVcUErMh++)
                {
                    ObservedParticleGroup jikTxMJzoNHkzXkOyDFbvFDQfjOaqfAE = ObservedParticles[WMGrfXimhcwJhZUkoZzhFYUkCVcUErMh];
                    if (jikTxMJzoNHkzXkOyDFbvFDQfjOaqfAE.Fired)
                    {
                        jikTxMJzoNHkzXkOyDFbvFDQfjOaqfAE.OnReleased?.Invoke();
                    }

                    jikTxMJzoNHkzXkOyDFbvFDQfjOaqfAE.Fired = false;
                    ObservedParticles[WMGrfXimhcwJhZUkoZzhFYUkCVcUErMh] = jikTxMJzoNHkzXkOyDFbvFDQfjOaqfAE;
                }

                kSJdAIBxDPpuUypiqvgEResqyeoKfhjb = false;
                OnRelease?.Invoke();
            }

            if (DOBzvNOVmPUurlRjbrhvcMuCOIZvPeNj)
            {
                cynLNIJRDamSQYDFszuByWdlxQefLhPD.Clear();

                NativeArray<int> MvVQAVEKhnEYlBpdmxEZBaqZnqKSPqkE = new();

                int pdeiHpREjAOddoxPNdbuTTeONgRGYWgX = PhysicsWorld.QueryParticles(transform.position, GrabRadius, ref MvVQAVEKhnEYlBpdmxEZBaqZnqKSPqkE);

                for (int dMtetqgjQnRiPcphrvzjJFGpBeNnihjc = 0; dMtetqgjQnRiPcphrvzjJFGpBeNnihjc < pdeiHpREjAOddoxPNdbuTTeONgRGYWgX; dMtetqgjQnRiPcphrvzjJFGpBeNnihjc++)
                {
                    int FAuCCCWbrTHiXGgmRuXipRHkCijuvQeL = MvVQAVEKhnEYlBpdmxEZBaqZnqKSPqkE[dMtetqgjQnRiPcphrvzjJFGpBeNnihjc];

                    
                    if (PhysicsWorld.ParticleData.Enabled[FAuCCCWbrTHiXGgmRuXipRHkCijuvQeL] &&
                        math.distancesq(PhysicsWorld.ParticleData.Position[FAuCCCWbrTHiXGgmRuXipRHkCijuvQeL], transform.position) < GrabRadius * GrabRadius)
                    {
                        
                        if (GrabLayer > 0)
                        {
                            uint xZskYEXKplPSIxhPMpKBOTbiLKUuZeLV = PhysicsWorld.GetLayerFromPhase(PhysicsWorld.ParticleData.Phase[FAuCCCWbrTHiXGgmRuXipRHkCijuvQeL]);
                            if (xZskYEXKplPSIxhPMpKBOTbiLKUuZeLV != 0 && xZskYEXKplPSIxhPMpKBOTbiLKUuZeLV != (int)GrabLayer)
                            {
                                continue;
                            }
                        }

                        
                        if (!GrabKinematicParticles && PhysicsWorld.ParticleData.InverseMass[FAuCCCWbrTHiXGgmRuXipRHkCijuvQeL] == 0)
                        {
                            continue;
                        }

                        cynLNIJRDamSQYDFszuByWdlxQefLhPD.Add(FAuCCCWbrTHiXGgmRuXipRHkCijuvQeL);
                    }
                }

                
                if (GrabType == GrabType.Closest && cynLNIJRDamSQYDFszuByWdlxQefLhPD.Count > 1)
                {
                    var qJGnbjBVUVKTEoQkmOsjWYItMOeBwqUM = -1;
                    var eBxzJAknlRPFivoABuTkcyNQUqRMqomh = float.MaxValue;
                    for (int OUQzNtAFNYAtshUovpeIJRmhBVwaNXMn = 0; OUQzNtAFNYAtshUovpeIJRmhBVwaNXMn < cynLNIJRDamSQYDFszuByWdlxQefLhPD.Count; OUQzNtAFNYAtshUovpeIJRmhBVwaNXMn++)
                    {
                        var lBIaCGfdwYFMfAoAAeBIDgUZNzAIWWfm = math.lengthsq((float3)transform.position - PhysicsWorld.ParticleData.Position[cynLNIJRDamSQYDFszuByWdlxQefLhPD[OUQzNtAFNYAtshUovpeIJRmhBVwaNXMn]]);
                        if (lBIaCGfdwYFMfAoAAeBIDgUZNzAIWWfm < eBxzJAknlRPFivoABuTkcyNQUqRMqomh)
                        {
                            eBxzJAknlRPFivoABuTkcyNQUqRMqomh = lBIaCGfdwYFMfAoAAeBIDgUZNzAIWWfm;
                            qJGnbjBVUVKTEoQkmOsjWYItMOeBwqUM = OUQzNtAFNYAtshUovpeIJRmhBVwaNXMn;
                        }
                    }

                    var AUJtyFKVsEcLsjSfdFtqsKhgkJcBBjbL = cynLNIJRDamSQYDFszuByWdlxQefLhPD[qJGnbjBVUVKTEoQkmOsjWYItMOeBwqUM];
                    cynLNIJRDamSQYDFszuByWdlxQefLhPD.Clear();
                    cynLNIJRDamSQYDFszuByWdlxQefLhPD.Add(AUJtyFKVsEcLsjSfdFtqsKhgkJcBBjbL);
                }

                
                for (int GvyeamgZMRuVMKeIMokFaRhuLEEnuOjs = 0; GvyeamgZMRuVMKeIMokFaRhuLEEnuOjs < cynLNIJRDamSQYDFszuByWdlxQefLhPD.Count; GvyeamgZMRuVMKeIMokFaRhuLEEnuOjs++)
                {
                    
                    var ILJvJbEjExkhZhSJVsOxXLAUxfcPqLbr = PhysicsWorld.ParticleData.SimulationMeshDescriptorIndex[cynLNIJRDamSQYDFszuByWdlxQefLhPD[GvyeamgZMRuVMKeIMokFaRhuLEEnuOjs]];
                    var vRvbcEMtrJryLOqkGCnVvfUySBWrSLHO = PhysicsWorld.GetSimulationMesh(ILJvJbEjExkhZhSJVsOxXLAUxfcPqLbr);

                    if (vRvbcEMtrJryLOqkGCnVvfUySBWrSLHO != null)
                    {

                        
                        
                        var yxwVgLpGFHnSgStNEhsXlYbpRGfvxOMZ = PhysicsWorld.ParticleData.Position[cynLNIJRDamSQYDFszuByWdlxQefLhPD[GvyeamgZMRuVMKeIMokFaRhuLEEnuOjs]];

                        
                        
                        
                        
                        

                        
                        
                        
                        
                        
                        
                        

                        for (int dMGqvIsVUhpcbQOQVZaGWyVxUXSkdAal = 0; dMGqvIsVUhpcbQOQVZaGWyVxUXSkdAal < 6; dMGqvIsVUhpcbQOQVZaGWyVxUXSkdAal++)
                        {
                            var wWmPFPGGRFgOKBNZNLLUBdPNxRXVoJTH = gameObject.AddComponent<DistanceJoint>();
                            wWmPFPGGRFgOKBNZNLLUBdPNxRXVoJTH.SimulationMeshParticleIndex = dMGqvIsVUhpcbQOQVZaGWyVxUXSkdAal;
                            var ZcKZQTmtupIwlGcZipUTKFtFhhBOOXsM = PhysicsWorld.SimulationMeshDescriptorData.SimulationMeshDescriptors[ILJvJbEjExkhZhSJVsOxXLAUxfcPqLbr].ParticleStartIndex;
                            wWmPFPGGRFgOKBNZNLLUBdPNxRXVoJTH.ConnectedSimulationMeshParticleIndex = cynLNIJRDamSQYDFszuByWdlxQefLhPD[GvyeamgZMRuVMKeIMokFaRhuLEEnuOjs] - ZcKZQTmtupIwlGcZipUTKFtFhhBOOXsM;

                            wWmPFPGGRFgOKBNZNLLUBdPNxRXVoJTH.Actor = this;
                            wWmPFPGGRFgOKBNZNLLUBdPNxRXVoJTH.ConnectedSimulationMesh = vRvbcEMtrJryLOqkGCnVvfUySBWrSLHO;

                            wWmPFPGGRFgOKBNZNLLUBdPNxRXVoJTH.TargetDistance = math.distance(SimulationMesh.RuntimeParticleData.Position[dMGqvIsVUhpcbQOQVZaGWyVxUXSkdAal], yxwVgLpGFHnSgStNEhsXlYbpRGfvxOMZ);

                            LhNBNZzezSDnLRLtuLmIHYPujpFOFmDV.Add(wWmPFPGGRFgOKBNZNLLUBdPNxRXVoJTH);
                        }
                    }
                }

                
                for (int JdZvopziVRsDTZeccTjsMqyiGIbSCHYs = 0; JdZvopziVRsDTZeccTjsMqyiGIbSCHYs < ObservedParticles.Count; JdZvopziVRsDTZeccTjsMqyiGIbSCHYs++)
                {
                    ObservedParticleGroup TSJFEBLxCNqsIJqgUyHgwYlaZrsudnSl = ObservedParticles[JdZvopziVRsDTZeccTjsMqyiGIbSCHYs];
                    if (TSJFEBLxCNqsIJqgUyHgwYlaZrsudnSl.Fired || TSJFEBLxCNqsIJqgUyHgwYlaZrsudnSl.Actor == null || TSJFEBLxCNqsIJqgUyHgwYlaZrsudnSl.Actor.PhysicsWorld == null || TSJFEBLxCNqsIJqgUyHgwYlaZrsudnSl.Actor.PhysicsWorld != PhysicsWorld || !TSJFEBLxCNqsIJqgUyHgwYlaZrsudnSl.Actor.IsPartOfPhysicsWorld)
                    {
                        continue;
                    }

                    var HihdzsEJUStIvegNCfcBtYrFmwqXsxCX = TSJFEBLxCNqsIJqgUyHgwYlaZrsudnSl.Actor.SimulationMesh.SimulationMeshDescriptorIndex;
                    var UfJuOvjcJEKGgSzSqOnaVPPYJBGHdYNQ = TSJFEBLxCNqsIJqgUyHgwYlaZrsudnSl.Actor.PhysicsWorld.SimulationMeshDescriptorData.SimulationMeshDescriptors[HihdzsEJUStIvegNCfcBtYrFmwqXsxCX];

                    for (int ngFtYhmhavBjrDBulUxfoaVBlHiMzHBR = 0; ngFtYhmhavBjrDBulUxfoaVBlHiMzHBR < cynLNIJRDamSQYDFszuByWdlxQefLhPD.Count; ngFtYhmhavBjrDBulUxfoaVBlHiMzHBR++)
                    {
                        if (TSJFEBLxCNqsIJqgUyHgwYlaZrsudnSl.Particles.Contains(cynLNIJRDamSQYDFszuByWdlxQefLhPD[ngFtYhmhavBjrDBulUxfoaVBlHiMzHBR] - UfJuOvjcJEKGgSzSqOnaVPPYJBGHdYNQ.ParticleStartIndex))
                        {
                            TSJFEBLxCNqsIJqgUyHgwYlaZrsudnSl.OnGrabbed?.Invoke();
                            TSJFEBLxCNqsIJqgUyHgwYlaZrsudnSl.Fired = true;
                            ObservedParticles[JdZvopziVRsDTZeccTjsMqyiGIbSCHYs] = TSJFEBLxCNqsIJqgUyHgwYlaZrsudnSl;
                            continue;
                        }
                    }
                }

                DOBzvNOVmPUurlRjbrhvcMuCOIZvPeNj = false;
                OnGrab?.Invoke();
            }
        }

        private void xKvKIwngCuWAvbdysIlicAZtbWgDrvVW()
        {
            Gizmos.DrawWireSphere(transform.position, GrabRadius);
        }

        /// <summary>
        /// A group of observed particles.
        /// </summary>
        [System.Serializable]
        public struct ObservedParticleGroup
        {
            /// <summary>
            /// The actor these particles belong to.
            /// </summary>
            public BasePhysicsActor Actor;

            /// <summary>
            /// The particle indices.
            /// </summary>
            [HideInInspector]
            public SerializableHashSet<int> Particles;

            /// <summary>
            /// The event to be called when a particle of the group is grabbed.
            /// </summary>
            public UnityEvent OnGrabbed;

            /// <summary>
            /// The event to be called when a particle of the group is released.
            /// </summary>
            public UnityEvent OnReleased;

            /// <summary>
            /// Whether the event has alredy been fired.
            /// </summary>
            [HideInInspector]
            public bool Fired;
        }

    }
}
