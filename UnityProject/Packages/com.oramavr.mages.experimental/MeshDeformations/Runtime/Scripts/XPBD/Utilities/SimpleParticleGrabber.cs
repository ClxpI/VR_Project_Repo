namespace MAGES.MeshDeformations
{
    using System.Collections.Generic;
    using Unity.Collections;
    using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.Events;
    using static MAGES.MeshDeformations.AdvancedParticleGrabber;

    /// <summary>
    /// A simple particle grabbing tool
    /// </summary>
    public class SimpleParticleGrabber : MonoBehaviour
    {
        private bool RZapEKkoQJlPntnaGbXhTdiiDPucntoI = false;
        private bool rNRBRLdeWBuAQscUmmnFHtZUXcEmheiI = false;

        private List<int> oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ = new List<int>();
        private List<zPMsYkUHbHybJqGidaEbNlxNEhxhdrUL> PuSVmgxTeapfQeqPKAQBDVObXyldXYVf = new List<zPMsYkUHbHybJqGidaEbNlxNEhxhdrUL>();

        /// <summary>
        /// Gets or sets the physics world in which this grabber tool will operate.
        /// </summary>
        [field: SerializeField]
        public PhysicsWorld PhysicsWorld { get; set; }

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
        [field: Tooltip("The observed particles. If any of these is grabbed a callback will be fired.")]
        public List<ObservedParticleGroup> ObservedParticles { get; set; } = new List<ObservedParticleGroup>();

        /// <summary>
        /// Grab the closest particle to the grabber.
        /// The grabbing will be done in the next physics update.
        /// </summary>
        [ContextMenu("Grab")]
        public void Grab()
        {
            rNRBRLdeWBuAQscUmmnFHtZUXcEmheiI = true; 
            RZapEKkoQJlPntnaGbXhTdiiDPucntoI = true;
        }

        /// <summary>
        /// Released the grabbed particles if any.
        /// </summary>
        [ContextMenu("Release")]
        public void Release()
        {
            RZapEKkoQJlPntnaGbXhTdiiDPucntoI = false;
            rNRBRLdeWBuAQscUmmnFHtZUXcEmheiI = true;
        }

        private void KUtZjcHMBabrHllrKhBFGhAKKWxTCpBp(float timeStep)
        {
            if (rNRBRLdeWBuAQscUmmnFHtZUXcEmheiI)
            {
                for (int TjSnkZnQceXvJqxQbZfJeDsGILleVXnV = 0; TjSnkZnQceXvJqxQbZfJeDsGILleVXnV < oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ.Count; TjSnkZnQceXvJqxQbZfJeDsGILleVXnV++)
                {
                    PhysicsWorld.ParticleData.InverseMass[oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ[TjSnkZnQceXvJqxQbZfJeDsGILleVXnV]] = PuSVmgxTeapfQeqPKAQBDVObXyldXYVf[TjSnkZnQceXvJqxQbZfJeDsGILleVXnV].OriginalInverseMass;
                }

                oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ.Clear();
                PuSVmgxTeapfQeqPKAQBDVObXyldXYVf.Clear();

                
                for (int tBmwsLTCPmZhITebcQmXQVxwWxpiUGwP = 0; tBmwsLTCPmZhITebcQmXQVxwWxpiUGwP < ObservedParticles.Count; tBmwsLTCPmZhITebcQmXQVxwWxpiUGwP++)
                {
                    ObservedParticleGroup NdjvGsTfYixbgEVzkjfylDdNIgoKPvmV = ObservedParticles[tBmwsLTCPmZhITebcQmXQVxwWxpiUGwP];
                    if (NdjvGsTfYixbgEVzkjfylDdNIgoKPvmV.Fired)
                    {
                        NdjvGsTfYixbgEVzkjfylDdNIgoKPvmV.OnReleased?.Invoke();
                    }

                    NdjvGsTfYixbgEVzkjfylDdNIgoKPvmV.Fired = false;
                    ObservedParticles[tBmwsLTCPmZhITebcQmXQVxwWxpiUGwP] = NdjvGsTfYixbgEVzkjfylDdNIgoKPvmV;
                }

                rNRBRLdeWBuAQscUmmnFHtZUXcEmheiI = false;
                OnRelease?.Invoke();
            }

            if (RZapEKkoQJlPntnaGbXhTdiiDPucntoI)
            {
                oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ.Clear();
                PuSVmgxTeapfQeqPKAQBDVObXyldXYVf.Clear();

                NativeArray<int> KzpNeTLUalCechgncwYMGtlVjliUMkEi = new ();

                int soQXLfrhcyuUyHJPUiYjRfiFHuaDSuvz = PhysicsWorld.QueryParticles(transform.position, GrabRadius, ref KzpNeTLUalCechgncwYMGtlVjliUMkEi);

                for (int yfKrwwioQzQBpdfyZrXWKoustHQdarzh = 0; yfKrwwioQzQBpdfyZrXWKoustHQdarzh < soQXLfrhcyuUyHJPUiYjRfiFHuaDSuvz; yfKrwwioQzQBpdfyZrXWKoustHQdarzh++)
                {
                    int BkcpGIpLMgNNLSUEvtBdsrxjjoUbiDXF = KzpNeTLUalCechgncwYMGtlVjliUMkEi[yfKrwwioQzQBpdfyZrXWKoustHQdarzh];

                    
                    if (PhysicsWorld.ParticleData.Enabled[BkcpGIpLMgNNLSUEvtBdsrxjjoUbiDXF] &&
                        math.distancesq(PhysicsWorld.ParticleData.Position[BkcpGIpLMgNNLSUEvtBdsrxjjoUbiDXF], transform.position) < GrabRadius * GrabRadius)
                    {
                        
                        if (GrabLayer > 0)
                        {
                            uint NfWQLpbIkEEoKQWXpeMuCHhOwDtpQJmw = PhysicsWorld.GetLayerFromPhase(PhysicsWorld.ParticleData.Phase[BkcpGIpLMgNNLSUEvtBdsrxjjoUbiDXF]);
                            if (NfWQLpbIkEEoKQWXpeMuCHhOwDtpQJmw != 0 && NfWQLpbIkEEoKQWXpeMuCHhOwDtpQJmw != (int)GrabLayer)
                            {
                                continue;
                            }
                        }

                        
                        if (!GrabKinematicParticles && PhysicsWorld.ParticleData.InverseMass[BkcpGIpLMgNNLSUEvtBdsrxjjoUbiDXF] == 0)
                        {
                            continue;
                        }

                        oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ.Add(BkcpGIpLMgNNLSUEvtBdsrxjjoUbiDXF);
                        PuSVmgxTeapfQeqPKAQBDVObXyldXYVf.Add(new zPMsYkUHbHybJqGidaEbNlxNEhxhdrUL
                        {
                            PositionOffset = transform.InverseTransformPoint(PhysicsWorld.ParticleData.Position[BkcpGIpLMgNNLSUEvtBdsrxjjoUbiDXF]),
                            OriginalInverseMass = PhysicsWorld.ParticleData.InverseMass[BkcpGIpLMgNNLSUEvtBdsrxjjoUbiDXF],
                        });
                    }
                }

                
                if (GrabType == GrabType.Closest && oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ.Count > 1)
                {
                    var KpifeGpErHRwsHaFEckTLLCjMDAdRwbk = -1;
                    var qZUNsRbEWJGCrKZlcuuWvlXgSdOVVMzt = float.MaxValue;
                    for (int dghlMNjKfEyroSpLHiWSKGmpGbxBaHrw = 0; dghlMNjKfEyroSpLHiWSKGmpGbxBaHrw < oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ.Count; dghlMNjKfEyroSpLHiWSKGmpGbxBaHrw++)
                    {
                        var ONkawtBRIMisiLeGdPwgrhAbpFfaIIbJ = math.lengthsq(PuSVmgxTeapfQeqPKAQBDVObXyldXYVf[dghlMNjKfEyroSpLHiWSKGmpGbxBaHrw].PositionOffset);
                        if (ONkawtBRIMisiLeGdPwgrhAbpFfaIIbJ < qZUNsRbEWJGCrKZlcuuWvlXgSdOVVMzt)
                        {
                            qZUNsRbEWJGCrKZlcuuWvlXgSdOVVMzt = ONkawtBRIMisiLeGdPwgrhAbpFfaIIbJ;
                            KpifeGpErHRwsHaFEckTLLCjMDAdRwbk = dghlMNjKfEyroSpLHiWSKGmpGbxBaHrw;
                        }
                    }

                    var FCHsfnxltMhNBzlOZLCNNFYbImOmCrwE = PuSVmgxTeapfQeqPKAQBDVObXyldXYVf[KpifeGpErHRwsHaFEckTLLCjMDAdRwbk];
                    var YHXsvhFItBBrDkVQZZkdFQCMFMGWyOJe = oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ[KpifeGpErHRwsHaFEckTLLCjMDAdRwbk];
                    PuSVmgxTeapfQeqPKAQBDVObXyldXYVf.Clear();
                    oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ.Clear();
                    oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ.Add(YHXsvhFItBBrDkVQZZkdFQCMFMGWyOJe);
                    PuSVmgxTeapfQeqPKAQBDVObXyldXYVf.Add(FCHsfnxltMhNBzlOZLCNNFYbImOmCrwE);
                }

                
                for (int QgFtLeDsSQUHitqBCMlMkZIkGaNbAeOZ = 0; QgFtLeDsSQUHitqBCMlMkZIkGaNbAeOZ < oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ.Count; QgFtLeDsSQUHitqBCMlMkZIkGaNbAeOZ++)
                {
                    PhysicsWorld.ParticleData.InverseMass[oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ[QgFtLeDsSQUHitqBCMlMkZIkGaNbAeOZ]] = 0;
                }

                RZapEKkoQJlPntnaGbXhTdiiDPucntoI = false;

                
                for (int zHcIQoInUWATKUMqEbPHsdnxxzyBKnKy = 0; zHcIQoInUWATKUMqEbPHsdnxxzyBKnKy < ObservedParticles.Count; zHcIQoInUWATKUMqEbPHsdnxxzyBKnKy++)
                {
                    ObservedParticleGroup SgvfovTuRfnqwpHoZLukTxpqHIjPmqWF = ObservedParticles[zHcIQoInUWATKUMqEbPHsdnxxzyBKnKy];
                    if (SgvfovTuRfnqwpHoZLukTxpqHIjPmqWF.Fired || SgvfovTuRfnqwpHoZLukTxpqHIjPmqWF.Actor == null || SgvfovTuRfnqwpHoZLukTxpqHIjPmqWF.Actor.PhysicsWorld == null || SgvfovTuRfnqwpHoZLukTxpqHIjPmqWF.Actor.PhysicsWorld != PhysicsWorld || !SgvfovTuRfnqwpHoZLukTxpqHIjPmqWF.Actor.IsPartOfPhysicsWorld)
                    {
                        continue;
                    }

                    var nQttcEBuBISIwgNxazapvrQzkvqOJMjS = SgvfovTuRfnqwpHoZLukTxpqHIjPmqWF.Actor.SimulationMesh.SimulationMeshDescriptorIndex;
                    var klHnOLvKAKCebiblUPqxVrvxUpekuDML = SgvfovTuRfnqwpHoZLukTxpqHIjPmqWF.Actor.PhysicsWorld.SimulationMeshDescriptorData.SimulationMeshDescriptors[nQttcEBuBISIwgNxazapvrQzkvqOJMjS];

                    for (int KrMhyBjdpMDzXjMjwLERyTbCMXWkPXZk = 0; KrMhyBjdpMDzXjMjwLERyTbCMXWkPXZk < oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ.Count; KrMhyBjdpMDzXjMjwLERyTbCMXWkPXZk++)
                    {
                        if (SgvfovTuRfnqwpHoZLukTxpqHIjPmqWF.Particles.Contains(oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ[KrMhyBjdpMDzXjMjwLERyTbCMXWkPXZk] - klHnOLvKAKCebiblUPqxVrvxUpekuDML.ParticleStartIndex))
                        {
                            SgvfovTuRfnqwpHoZLukTxpqHIjPmqWF.OnGrabbed?.Invoke();
                            SgvfovTuRfnqwpHoZLukTxpqHIjPmqWF.Fired = true;
                            ObservedParticles[zHcIQoInUWATKUMqEbPHsdnxxzyBKnKy] = SgvfovTuRfnqwpHoZLukTxpqHIjPmqWF;
                            continue;
                        }
                    }
                }
                OnGrab?.Invoke();
            }

            
            for (int rRURdUNHkiVQvkkZkuGnPvgTjJAwgUbb = 0; rRURdUNHkiVQvkkZkuGnPvgTjJAwgUbb < oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ.Count; rRURdUNHkiVQvkkZkuGnPvgTjJAwgUbb++)
            {
                var gqgzPvFvKNvVJwlOtuCRbqrLTsdHVrbq = oSnearJPbQKOCqWwkmmtyEVwSYPXysIZ[rRURdUNHkiVQvkkZkuGnPvgTjJAwgUbb];
                var AKjxHuRpGucuLaSPuumXHvWuJzOjLlpU = PuSVmgxTeapfQeqPKAQBDVObXyldXYVf[rRURdUNHkiVQvkkZkuGnPvgTjJAwgUbb];

                var FJYbmkOfTevsjsuzXLFfHkbOFrhmmNBs = MoveToCenter ? transform.position : transform.TransformPoint(AKjxHuRpGucuLaSPuumXHvWuJzOjLlpU.PositionOffset);
                PhysicsWorld.ParticleData.Position[gqgzPvFvKNvVJwlOtuCRbqrLTsdHVrbq] = FJYbmkOfTevsjsuzXLFfHkbOFrhmmNBs;
            }
        }

        private void BoEewXgdEBGJQwELhPjYFXiqsjwBLQtS()
        {
            PhysicsWorld?.PhysicsUpdate.AddListener(KUtZjcHMBabrHllrKhBFGhAKKWxTCpBp);
        }

        private void sDFNBYMWUsJdytdQimVwfLSixHpcIwRi()
        {
            PhysicsWorld?.PhysicsUpdate.RemoveListener(KUtZjcHMBabrHllrKhBFGhAKKWxTCpBp);
        }

        private void bTsyZgRfasKQQrtHmLeYwsEIsQvfsAQd()
        {
            Gizmos.DrawWireSphere(transform.position, GrabRadius);
        }

        private void hjiEqJfLvxrEcDxJiFrllACpDFeuPAiX()
        {
            PhysicsWorld = FindFirstObjectByType<PhysicsWorld>();
        }

        private struct zPMsYkUHbHybJqGidaEbNlxNEhxhdrUL
        {
            public float3 PositionOffset;
            public float OriginalInverseMass;
        }
    }
}
