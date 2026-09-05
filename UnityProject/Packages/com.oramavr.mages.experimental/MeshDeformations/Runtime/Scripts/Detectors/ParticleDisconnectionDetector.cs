namespace MAGES.MeshDeformations.Tools
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Detects disconnection between particles in a simulation mesh.
    /// </summary>
    /// <remarks>
    /// Two particles are connected if there is a distance constraint between them or between some other particles that are connected to them.
    /// </remarks>
    [RequireComponent(typeof(BasePhysicsActor))]
    public class ParticleDisconnectionDetector : BaseDetector
    {
        private int[] jjfYKClSOexGDbTkHFkwNzKEVysdkuMc = new int[0];

        /// <summary>
        /// Gets or sets the particle pairs to be observed for disconnection.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Observed pairs for checking for disconnection.")]
        public List<ObservedParticlePair> ObservedParticlePairs { get; set; } = new List<ObservedParticlePair>();

        /// <summary>
        /// Gets the UnityEvent invoked when a pair of the observed ones diconnects.<br/>
        ///     arg1: The index of the first particle in the pair.<br/>
        ///     arg2: The index of the second particle in the pair.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Event invoked when a pair of the observed ones diconnects.")]
        public UnityEvent<int, int> DisconnectionEvent { get; private set; } = new UnityEvent<int, int>();

        /// <inheritdoc/>
        protected override void DetectorUpdate()
        {
            if (Actor.SimulationMesh.DistanceConstraintsEnabledStateHasChanged)
            {
                for (int tsgrefDjAZMAmjmDjWBbqEFoSPvqnGnK = 0; tsgrefDjAZMAmjmDjWBbqEFoSPvqnGnK < ObservedParticlePairs.Count; tsgrefDjAZMAmjmDjWBbqEFoSPvqnGnK++)
                {
                    var EscZMkPzRXWZqCtOVOrmnhBiFEclnYDv = ObservedParticlePairs[tsgrefDjAZMAmjmDjWBbqEFoSPvqnGnK];

                    if (EscZMkPzRXWZqCtOVOrmnhBiFEclnYDv.Invoked)
                    {
                        continue;
                    }

                    if (!nQUBUpCbhtGlrkZVHdVnMkBLvNtzJTsq(EscZMkPzRXWZqCtOVOrmnhBiFEclnYDv.ParticleA, EscZMkPzRXWZqCtOVOrmnhBiFEclnYDv.ParticleB, EscZMkPzRXWZqCtOVOrmnhBiFEclnYDv.ThroughPhysicsMaterial))
                    {
                        EscZMkPzRXWZqCtOVOrmnhBiFEclnYDv.Invoked = true;
                        ObservedParticlePairs[tsgrefDjAZMAmjmDjWBbqEFoSPvqnGnK] = EscZMkPzRXWZqCtOVOrmnhBiFEclnYDv;
                        DisconnectionEvent?.Invoke(EscZMkPzRXWZqCtOVOrmnhBiFEclnYDv.ParticleA, EscZMkPzRXWZqCtOVOrmnhBiFEclnYDv.ParticleB);
                    }
                }
            }
        }

        private bool nQUBUpCbhtGlrkZVHdVnMkBLvNtzJTsq(int particleA, int particleB, int throughPhysicsMaterial)
        {
            var HHNulTVbAZsbreiicTsbOurTtSXpWZpc = Actor.SimulationMesh.RuntimeParticleData.Count;
            if (jjfYKClSOexGDbTkHFkwNzKEVysdkuMc.Length < HHNulTVbAZsbreiicTsbOurTtSXpWZpc)
            {
                jjfYKClSOexGDbTkHFkwNzKEVysdkuMc = new int[HHNulTVbAZsbreiicTsbOurTtSXpWZpc];
            }

            
            for (int kimQFyhEfjYxkcKksbocTyWsogkgqntX = 0; kimQFyhEfjYxkcKksbocTyWsogkgqntX < jjfYKClSOexGDbTkHFkwNzKEVysdkuMc.Length; kimQFyhEfjYxkcKksbocTyWsogkgqntX++)
            {
                jjfYKClSOexGDbTkHFkwNzKEVysdkuMc[kimQFyhEfjYxkcKksbocTyWsogkgqntX] = -1;
            }

            jjfYKClSOexGDbTkHFkwNzKEVysdkuMc[particleA] = 0;
            jjfYKClSOexGDbTkHFkwNzKEVysdkuMc[particleB] = 1;

            
            while (true)
            {
                bool ChVZiBoYFsIDthbpTwgHjRKruSONxEoW = false;
                bool mQxxZwqDYeVnkDQwfauaboeZVbPNLRkn = false;

                var CJWMNTKdHDrKgRsVFZzzqxjtrqQgZVZp = Parallel.For(0, Actor.SimulationMesh.RuntimeDistanceConstraintData.Count, (i, loopState) =>
                {
                    if (!Actor.SimulationMesh.RuntimeDistanceConstraintData.Enabled[i])
                    {
                        return;
                    }

                    if (throughPhysicsMaterial >= 0 && Actor.SimulationMesh.RuntimeDistanceConstraintData.PhysicsMaterialIndex[i] != throughPhysicsMaterial)
                    {
                        return;
                    }

                    int GrJzhDkOJJLcruDmjbaHQjfpAQiEKhkt = Actor.SimulationMesh.RuntimeDistanceConstraintData.ParticleA[i];
                    int eNApfrZUHIRkKbxGNglgoGqhkOscPGpR = Actor.SimulationMesh.RuntimeDistanceConstraintData.ParticleB[i];

                    int dZRwsCwzEZIZiTABvFVMjEEXgnNXTlJL = jjfYKClSOexGDbTkHFkwNzKEVysdkuMc[GrJzhDkOJJLcruDmjbaHQjfpAQiEKhkt];
                    int YBsHLvqyrnWqxfOngcQVtzKnmKXaSzUQ = jjfYKClSOexGDbTkHFkwNzKEVysdkuMc[eNApfrZUHIRkKbxGNglgoGqhkOscPGpR];

                    
                    if (dZRwsCwzEZIZiTABvFVMjEEXgnNXTlJL != -1 && YBsHLvqyrnWqxfOngcQVtzKnmKXaSzUQ == -1)
                    {
                        jjfYKClSOexGDbTkHFkwNzKEVysdkuMc[eNApfrZUHIRkKbxGNglgoGqhkOscPGpR] = dZRwsCwzEZIZiTABvFVMjEEXgnNXTlJL;
                        ChVZiBoYFsIDthbpTwgHjRKruSONxEoW = true;
                    }
                    else if (YBsHLvqyrnWqxfOngcQVtzKnmKXaSzUQ != -1 && dZRwsCwzEZIZiTABvFVMjEEXgnNXTlJL == -1)
                    {
                        jjfYKClSOexGDbTkHFkwNzKEVysdkuMc[GrJzhDkOJJLcruDmjbaHQjfpAQiEKhkt] = YBsHLvqyrnWqxfOngcQVtzKnmKXaSzUQ;
                        ChVZiBoYFsIDthbpTwgHjRKruSONxEoW = true;
                    }
                    else if (dZRwsCwzEZIZiTABvFVMjEEXgnNXTlJL != -1 && YBsHLvqyrnWqxfOngcQVtzKnmKXaSzUQ != -1 && dZRwsCwzEZIZiTABvFVMjEEXgnNXTlJL != YBsHLvqyrnWqxfOngcQVtzKnmKXaSzUQ)
                    {
                        mQxxZwqDYeVnkDQwfauaboeZVbPNLRkn = true;
                        loopState.Stop();
                    }
                });

                if (mQxxZwqDYeVnkDQwfauaboeZVbPNLRkn)
                {
                    return true;
                }

                if (!ChVZiBoYFsIDthbpTwgHjRKruSONxEoW)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// A pair of particles observed for disconnection.
        /// </summary>
        [System.Serializable]
        public struct ObservedParticlePair
        {
            /// <summary>
            /// The particleA.
            /// </summary>
            public int ParticleA;

            /// <summary>
            /// The particleB.
            /// </summary>
            public int ParticleB;

            /// <summary>
            /// The PhysicsMaterial's Index to check for connection through.<br/>
            /// If -1, connection through all physics materials will be considered.
            /// </summary>
            public int ThroughPhysicsMaterial;

            /// <summary>
            /// Whether the DisconnectionEvent has been invoked for this pair.
            /// </summary>
            [HideInInspector]
            [System.NonSerialized]
            internal bool Invoked;
        }
    }
}
