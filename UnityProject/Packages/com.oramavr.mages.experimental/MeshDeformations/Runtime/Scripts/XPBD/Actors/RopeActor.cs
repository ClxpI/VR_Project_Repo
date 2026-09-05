namespace MAGES.MeshDeformations
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// A rope actor.
    /// </summary>
    public class RopeActor : BasePhysicsActor
    {
        [SerializeField]
        private float length = 0.5f;

        [SerializeField]
        private float mass = 1.0f;

        [SerializeField]
        private float damping = 0.01f;

        [SerializeField]
        private bool breakable = false;

        [SerializeField]
        private float breakForce = 1000.0f;

        [SerializeField]
        private bool selfCollision = true;

        [SerializeField]
        private uint layer = 1;

        [SerializeField]
        private float bendingCompliance = 0.1f;

        private float4x4 McmcxzSFrAWtAtwivYPrxCBbMUSeZtiv;
        private Queue<Disc> jjuFGrondxChjvHgZARFfXkafsDWrHEN = new Queue<Disc>();

        /// <summary>
        /// Gets or sets the length of the rope.
        /// </summary>
        public float Length
        {
            get
            {
                return length;
            }

            set
            {
                value = math.max(0, value);
                if (length == value)
                {
                    return;
                }

                length = value;
                cvWaAGgfRWxwifoQLfodKEYGdvDXiXaa();
            }
        }

        /// <summary>
        /// Gets or sets the total rope mass.
        /// </summary>
        public float Mass
        {
            get
            {
                return mass;
            }

            set
            {
                value = math.max(1e-07f, value);
                value = math.min(1e+09f, value);
                if (mass == value)
                {
                    return;
                }

                mass = value;
                cvWaAGgfRWxwifoQLfodKEYGdvDXiXaa();
            }
        }

        /// <summary>
        /// Gets or sets the damping applied to each particle. A value in the range [0, 1].
        /// </summary>
        public float Damping
        {
            get
            {
                return damping;
            }

            set
            {
                value = math.clamp(value, 0, 1.0f);

                if (damping == value)
                {
                    return;
                }

                damping = value;
                cvWaAGgfRWxwifoQLfodKEYGdvDXiXaa();
            }
        }

        /// <summary>
        /// Gets or setsh the bending compliance of the rope. A value in the range [0, Infinity].
        /// </summary>
        public float BendingCompliance
        {
            get
            {
                return bendingCompliance;
            }

            set
            {
                value = math.max(0, value);

                if (bendingCompliance == value)
                {
                    return;
                }

                bendingCompliance = value;

                if (SharedSimulationMesh != null && SharedSimulationMesh.PhysicsMaterials.Length > 0)
                {
                    PhysicsMaterial qCgevBduYLDqAnBvGMZHfqffVxoHQVks = SharedSimulationMesh.PhysicsMaterials[0];
                    qCgevBduYLDqAnBvGMZHfqffVxoHQVks.CollinearConstraintInverseStiffness = bendingCompliance;
                    SharedSimulationMesh.PhysicsMaterials[0] = qCgevBduYLDqAnBvGMZHfqffVxoHQVks;
#if UNITY_EDITOR
                    EditorUtility.SetDirty(SharedSimulationMesh);
#endif
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the rope is breakable.
        /// </summary>
        public bool Breakable
        {
            get
            {
                return breakable;
            }

            set
            {
                breakable = value;
                cvWaAGgfRWxwifoQLfodKEYGdvDXiXaa();
            }
        }

        /// <summary>
        /// Gets or sets the break force of the rope. Only valid if <see cref="Breakable"/> is true.
        /// </summary>
        public float BreakForce
        {
            get
            {
                return breakForce;
            }

            set
            {
                breakForce = math.max(0, value);
                cvWaAGgfRWxwifoQLfodKEYGdvDXiXaa();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether self collision is enabled.
        /// </summary>
        public bool SelfCollision
        {
            get
            {
                return selfCollision;
            }

            set
            {
                if (selfCollision == value)
                {
                    return;
                }

                selfCollision = value;
                if (MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj != null && MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld)
                {
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.PhysicsWorld.UpdatePhase(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj, pwsnlLqebdNFoFBkbatAOuSUeqEXtsLH());
                }
            }
        }

        /// <summary>
        /// Gets or sets the layer used for collision filtering.
        /// </summary>
        public uint Layer
        {
            get
            {
                return layer;
            }

            set
            {
                if (layer == value)
                {
                    return;
                }

                layer = value;
                if (MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj != null && MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld)
                {
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.PhysicsWorld.UpdatePhase(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj, pwsnlLqebdNFoFBkbatAOuSUeqEXtsLH());
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to update the kinematic particles position when this actor's transform is updated.
        /// </summary>
        [field: SerializeField]
        public bool UpdateKinematicParticles { get; set; } = true;

        /// <summary>
        /// Gets or sets the event called when the rope breaks due to exceeding the break force.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Called when the rope breaks due to exceeding the break force.")]
        public UnityEvent OnBreak { get; set; } = new UnityEvent();

        /// <inheritdoc/>
        public override void AddToPhysicsWorld()
        {
            if (SharedSimulationMesh == null)
            {
                UnityEngine.Debug.LogError("SimulationMeshSource is not assigned. Softbody actor will not be simulated.", this);
                return;
            }

            if (PhysicsWorld == null)
            {
                UnityEngine.Debug.LogError("PhysicsWorld is not assigned. Softbody actor will not be simulated.", this);
                return;
            }

            McmcxzSFrAWtAtwivYPrxCBbMUSeZtiv = transform.worldToLocalMatrix;
            PhysicsWorld.AddSimulationMesh(SimulationMesh, transform.localToWorldMatrix, pwsnlLqebdNFoFBkbatAOuSUeqEXtsLH());

            base.AddToPhysicsWorld();
        }

        /// <inheritdoc/>
        public override void RemoveFromPhysicsWorld()
        {
            base.RemoveFromPhysicsWorld();

            
            PhysicsWorld?.RemoveSimulationMesh(SimulationMesh);

            
            Destroy(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj);
            MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj = null;
        }

        /// <summary>
        /// Cuts the rope at the specified position.
        /// </summary>
        /// <param name="cuttingDisc">The disc, based on which to perform the cut.</param>
        public void Cut(Disc cuttingDisc)
        {
            jjuFGrondxChjvHgZARFfXkafsDWrHEN.Enqueue(cuttingDisc);
        }

        /// <inheritdoc/>
        protected override void PhysicsUpdate(float timeStep)
        {
            if (MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj == null || !MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld)
            {
                return;
            }

            
            
            if (SimulationMesh.DistanceConstraintsEnabledStateHasChanged)
            {
                
                Parallel.For(0, SimulationMesh.RuntimeCollinearConstraintData.Count, (i) =>
                {
                    bool WrAgJHTErjjONuclCkunyYoozHwyUEjd = SimulationMesh.RuntimeCollinearConstraintData.Enabled[i];

                    if (WrAgJHTErjjONuclCkunyYoozHwyUEjd)
                    {
                        bool CKQMGlrJDLRKefVIdGaoNRECHdmlqPTI = SimulationMesh.RuntimeDistanceConstraintData.Enabled[i];
                        bool pUzyOKscWxSBCCMJDmyifMQJWTuEDUMp = SimulationMesh.RuntimeDistanceConstraintData.Enabled[i + 1];

                        if (!CKQMGlrJDLRKefVIdGaoNRECHdmlqPTI || !pUzyOKscWxSBCCMJDmyifMQJWTuEDUMp)
                        {
                            SimulationMesh.RuntimeCollinearConstraintData.Enabled[i] = false;
                            SimulationMesh.CollinearConstraintsEnabledStateHasChanged = true;
                        }
                    }
                });

                OnBreak?.Invoke();
            }

            
            while (jjuFGrondxChjvHgZARFfXkafsDWrHEN.Count > 0)
            {
                Disc hvrpUEQukGTvklNYdSRgAHBqVgXiRlQc = jjuFGrondxChjvHgZARFfXkafsDWrHEN.Dequeue();
                Parallel.For(0, SimulationMesh.RuntimeDistanceConstraintData.Count, (i) =>
                {
                    var quTRdvcPYJgmZCqBAZSxYZjUZTjDWbeW = SimulationMesh.RuntimeParticleData.Position[SimulationMesh.RuntimeDistanceConstraintData.ParticleA[i]];
                    var nbLMoEPsvKzfdyqtQmSWkIXJtVDYeoVE = SimulationMesh.RuntimeParticleData.Position[SimulationMesh.RuntimeDistanceConstraintData.ParticleB[i]];

                    if (hvrpUEQukGTvklNYdSRgAHBqVgXiRlQc.LineSegmentIntersection(quTRdvcPYJgmZCqBAZSxYZjUZTjDWbeW, nbLMoEPsvKzfdyqtQmSWkIXJtVDYeoVE, out _))
                    {
                        SimulationMesh.RuntimeDistanceConstraintData.Enabled[i] = false;
                        SimulationMesh.RuntimeCollinearConstraintData.Enabled[i] = false;
                        if (i > 0)
                        {
                            SimulationMesh.RuntimeCollinearConstraintData.Enabled[i - 1] = false;
                        }

                        SimulationMesh.DistanceConstraintsEnabledStateHasChanged = true;
                        SimulationMesh.CollinearConstraintsEnabledStateHasChanged = true;
                    }
                });
            }

            if (UpdateKinematicParticles && transform.hasChanged)
            {
                AJcsVSmZJMvGLVATulmPCkqpDKzPRABo().Complete();
                transform.hasChanged = false;
            }
        }

        /// <inheritdoc/>
        protected override void Reset()
        {
            base.Reset();

            cvWaAGgfRWxwifoQLfodKEYGdvDXiXaa();
        }

        private uint pwsnlLqebdNFoFBkbatAOuSUeqEXtsLH()
        {
            var rtuEWaLndzOCOFbLPbONXVULEJLLRakI = PhaseOptions.None;
            if (SelfCollision)
            {
                rtuEWaLndzOCOFbLPbONXVULEJLLRakI |= PhaseOptions.SelfCollisionEnabled;
            }

            return PhysicsWorld.CreatePhase(Layer, rtuEWaLndzOCOFbLPbONXVULEJLLRakI);
        }

        private void cvWaAGgfRWxwifoQLfodKEYGdvDXiXaa()
        {
            
            PhysicsMaterial[] rluGfysLSMDrkeuaYmCIWVaCownnsoqH = new PhysicsMaterial[]
            {
                new PhysicsMaterial
                {
                    ParticleDamping = Damping,
                    DistanceConstraintInverseStiffness = 0f,
                    DistanceConstraintTearForce = BreakForce,
                    VolumeConstraintInverseStiffness = 0f,
                    ShapeMatchingConstraintStiffness = 1f,
                },
            };

            float kMMUBYUIzlNDYaqxqfcTWnRLbCtBdIsp = PhysicsWorld ? (PhysicsWorld.ParticleRadius * 2.0f) + 0.001f : 0.1f;
            int oaXhYQAKsFaJZMrfyeOIygSRkrOwzoZk = (int)math.ceil(Length / kMMUBYUIzlNDYaqxqfcTWnRLbCtBdIsp);
            var rUtcKskhuvoeDqeTDJOLIkQnzVmgShNI = Mass / oaXhYQAKsFaJZMrfyeOIygSRkrOwzoZk;

            List<Particle> NJSuxPHTTEbEyBnxmEUkEKQpBoAnKMGQ = new List<Particle>(oaXhYQAKsFaJZMrfyeOIygSRkrOwzoZk);
            List<DistanceConstraint> ycVOLSAhnZXpPGercZkzCNLuEMDrMbrd = new List<DistanceConstraint>(oaXhYQAKsFaJZMrfyeOIygSRkrOwzoZk - 1);
            List<CollinearConstraint> TqQAJpFiXJUYesElDFTYMIqFnNQipDgr = new List<CollinearConstraint>(oaXhYQAKsFaJZMrfyeOIygSRkrOwzoZk - 2);

            var sghZCaAbsJBNCOsuCKcWirdGdCbVsHwg = Vector3.zero;
            for (int RJhaIBKPyAYZPwzgOiGAepZLfvtSJMrz = 0; RJhaIBKPyAYZPwzgOiGAepZLfvtSJMrz < oaXhYQAKsFaJZMrfyeOIygSRkrOwzoZk; RJhaIBKPyAYZPwzgOiGAepZLfvtSJMrz++)
            {
                Particle UpfhhgCCnjclHfQuFUrpGdYRnvsfyEKh;
                if (SharedSimulationMesh != null && RJhaIBKPyAYZPwzgOiGAepZLfvtSJMrz < SharedSimulationMesh.Particles.Length)
                {
                    UpfhhgCCnjclHfQuFUrpGdYRnvsfyEKh = SharedSimulationMesh.Particles[RJhaIBKPyAYZPwzgOiGAepZLfvtSJMrz];
                    UpfhhgCCnjclHfQuFUrpGdYRnvsfyEKh.Position = sghZCaAbsJBNCOsuCKcWirdGdCbVsHwg;
                    UpfhhgCCnjclHfQuFUrpGdYRnvsfyEKh.InverseMass = 1f / rUtcKskhuvoeDqeTDJOLIkQnzVmgShNI;
                }
                else
                {
                    UpfhhgCCnjclHfQuFUrpGdYRnvsfyEKh = new Particle
                    {
                        Enabled = true,
                        Breakable = false,
                        Kinematic = false,
                        Position = sghZCaAbsJBNCOsuCKcWirdGdCbVsHwg,
                        Velocity = 0f,
                        InverseMass = 1f / rUtcKskhuvoeDqeTDJOLIkQnzVmgShNI,
                        PhysicsMaterialIndex = 0,
                    };
                }

                NJSuxPHTTEbEyBnxmEUkEKQpBoAnKMGQ.Add(UpfhhgCCnjclHfQuFUrpGdYRnvsfyEKh);
                sghZCaAbsJBNCOsuCKcWirdGdCbVsHwg += kMMUBYUIzlNDYaqxqfcTWnRLbCtBdIsp * Vector3.forward;
            }

            for (int rcddYVamjhlTaKbOAHxTkGuiHnCaauyS = 0; rcddYVamjhlTaKbOAHxTkGuiHnCaauyS < oaXhYQAKsFaJZMrfyeOIygSRkrOwzoZk - 1; rcddYVamjhlTaKbOAHxTkGuiHnCaauyS++)
            {
                var okHGHmFvSxsughgnIiEmaZBDTqRGfzYF = new DistanceConstraint
                {
                    Enabled = true,
                    Breakable = Breakable,
                    ParticleA = rcddYVamjhlTaKbOAHxTkGuiHnCaauyS,
                    ParticleB = rcddYVamjhlTaKbOAHxTkGuiHnCaauyS + 1,
                    PhysicsMaterialIndex = 0,
                };

                ycVOLSAhnZXpPGercZkzCNLuEMDrMbrd.Add(okHGHmFvSxsughgnIiEmaZBDTqRGfzYF);
            }

            for (int qIRgtopvQvcjwLBgQClNWlswDtpwsooi = 0; qIRgtopvQvcjwLBgQClNWlswDtpwsooi < oaXhYQAKsFaJZMrfyeOIygSRkrOwzoZk - 2; qIRgtopvQvcjwLBgQClNWlswDtpwsooi++)
            {
                CollinearConstraint PIzXmQkdGfyNNKxeOnPgRjnseSeFbnCG = new CollinearConstraint
                {
                    Enabled = true,
                    Breakable = false,
                    ParticleA = qIRgtopvQvcjwLBgQClNWlswDtpwsooi,
                    ParticleB = qIRgtopvQvcjwLBgQClNWlswDtpwsooi + 1,
                    ParticleC = qIRgtopvQvcjwLBgQClNWlswDtpwsooi + 2,
                    PhysicsMaterialIndex = 0,
                };

                TqQAJpFiXJUYesElDFTYMIqFnNQipDgr.Add(PIzXmQkdGfyNNKxeOnPgRjnseSeFbnCG);
            }

            var VaOvROZNAKtIZXQTVIaqfZXulCvQULBv = SharedSimulationMesh;
            if (VaOvROZNAKtIZXQTVIaqfZXulCvQULBv == null)
            {
                VaOvROZNAKtIZXQTVIaqfZXulCvQULBv = ScriptableObject.CreateInstance<SimulationMesh>();
#if UNITY_EDITOR
                if (!EditorUtility.IsPersistent(VaOvROZNAKtIZXQTVIaqfZXulCvQULBv))
                {
                    AssetDatabase.CreateAsset(VaOvROZNAKtIZXQTVIaqfZXulCvQULBv, AssetDatabase.GenerateUniqueAssetPath($"Assets/{name}_SimulationMesh.asset"));
                }
#endif
            }
            else
            {
                SharedSimulationMesh = null;
            }

            VaOvROZNAKtIZXQTVIaqfZXulCvQULBv.Clear();
            VaOvROZNAKtIZXQTVIaqfZXulCvQULBv.PhysicsMaterials = rluGfysLSMDrkeuaYmCIWVaCownnsoqH;
            VaOvROZNAKtIZXQTVIaqfZXulCvQULBv.Particles = NJSuxPHTTEbEyBnxmEUkEKQpBoAnKMGQ.ToArray();
            VaOvROZNAKtIZXQTVIaqfZXulCvQULBv.DistanceConstraints = ycVOLSAhnZXpPGercZkzCNLuEMDrMbrd.ToArray();
            VaOvROZNAKtIZXQTVIaqfZXulCvQULBv.CollinearConstraints = TqQAJpFiXJUYesElDFTYMIqFnNQipDgr.ToArray();

#if UNITY_EDITOR
            EditorUtility.SetDirty(VaOvROZNAKtIZXQTVIaqfZXulCvQULBv);
#endif

            SharedSimulationMesh = VaOvROZNAKtIZXQTVIaqfZXulCvQULBv;
        }

        private void ktAXPyIDTZtjwlYKmBGBnLpKnmCqyPpY()
        {
            if (SharedSimulationMesh == null || Application.isPlaying)
            {
                return;
            }

            var RhhnlcnNeKJGMDCRDPcYlwKJBdSrDFBr = Gizmos.color;
            var SjFnXFpAyUnyteYByEOKoVgPbkLevNtt = Gizmos.matrix;
            Gizmos.color = Color.green;
            Gizmos.matrix = transform.localToWorldMatrix;

            for (int LSIsqVlPlyjuaGsppDpUHSoNXpjjvljq = 0; LSIsqVlPlyjuaGsppDpUHSoNXpjjvljq < SharedSimulationMesh.Particles.Length - 1; LSIsqVlPlyjuaGsppDpUHSoNXpjjvljq++)
            {
                Gizmos.DrawLine(SharedSimulationMesh.Particles[LSIsqVlPlyjuaGsppDpUHSoNXpjjvljq].Position, SharedSimulationMesh.Particles[LSIsqVlPlyjuaGsppDpUHSoNXpjjvljq + 1].Position);
            }

            Gizmos.color = RhhnlcnNeKJGMDCRDPcYlwKJBdSrDFBr;
            Gizmos.matrix = SjFnXFpAyUnyteYByEOKoVgPbkLevNtt;
        }

        private JobHandle AJcsVSmZJMvGLVATulmPCkqpDKzPRABo(JobHandle dependsOn = default)
        {
            var oHNJNTfRrHrztIbEZeWvrSwKGJsjOEpD = new nUgCUrwrTwIwlVIUTPZuTxJYrTpJlpMI
            {
                InitialWorldToLocalMatrix = McmcxzSFrAWtAtwivYPrxCBbMUSeZtiv,
                LocalToWorldMatrix = transform.localToWorldMatrix,

                ParticlesInitialPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition,
                ParticlesPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position,
                ParticlesInverseMass = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InverseMass,
            };

            return oHNJNTfRrHrztIbEZeWvrSwKGJsjOEpD.Schedule(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Count, 64, dependsOn);
        }

        [BurstCompile]
        private struct nUgCUrwrTwIwlVIUTPZuTxJYrTpJlpMI : IJobParallelFor
        {
            [ReadOnly]
            public float4x4 InitialWorldToLocalMatrix;

            [ReadOnly]
            public float4x4 LocalToWorldMatrix;

            [ReadOnly]
            public NativeSlice<float3> ParticlesInitialPosition;

            [ReadOnly]
            public NativeSlice<float> ParticlesInverseMass;

            [WriteOnly]
            public NativeSlice<float3> ParticlesPosition;

            public void Execute(int particleIndex)
            {
                if (ParticlesInverseMass[particleIndex] > math.EPSILON)
                {
                    return;
                }

                float3 glcxeIJAWbHFINUCzVlHVhAKOdGomESt = ParticlesInitialPosition[particleIndex];

                glcxeIJAWbHFINUCzVlHVhAKOdGomESt = math.mul(InitialWorldToLocalMatrix, new float4(glcxeIJAWbHFINUCzVlHVhAKOdGomESt, 1.0f)).xyz;
                glcxeIJAWbHFINUCzVlHVhAKOdGomESt = math.mul(LocalToWorldMatrix, new float4(glcxeIJAWbHFINUCzVlHVhAKOdGomESt, 1.0f)).xyz;

                ParticlesPosition[particleIndex] = glcxeIJAWbHFINUCzVlHVhAKOdGomESt;
            }
        }
    }
}
