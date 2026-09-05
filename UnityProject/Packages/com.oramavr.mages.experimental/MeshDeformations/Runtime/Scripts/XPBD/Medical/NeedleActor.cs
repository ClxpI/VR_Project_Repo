namespace MAGES.MeshDeformations
{
    using System.Collections.Generic;
    using Unity.Mathematics;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// A needle actor used along with the <see cref="Puncturable"/> component for sututing simulations.
    /// </summary>
    public class NeedleActor : RigidbodyActor
    {
        [SerializeField]
        private List<Transform> needlePoints = new List<Transform>();

        [SerializeField]
        private float mass = 0.1f;

        [SerializeField]
        private float damping = 0.01f;

        /// <summary>
        /// Gets or sets the list of points approximating the shape of the needle.
        /// </summary>
        public List<Transform> NeedlePoints
        {
            get
            {
                return needlePoints;
            }

            set
            {
                needlePoints = value;
                RebuildSimulationMesh();
            }
        }

        /// <summary>
        /// Gets or sets the total needle mass.
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
                RebuildSimulationMesh();
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
                value = math.max(0.0f, value);
                value = math.min(1.0f, value);

                if (damping == value)
                {
                    return;
                }

                damping = value;
                RebuildSimulationMesh();
            }
        }

        /// <summary>
        /// Rebuilds the simulation mesh from the given <see cref="NeedlePoints"/>.
        /// </summary>
        public void RebuildSimulationMesh()
        {
            if (NeedlePoints == null || NeedlePoints.Count < 2)
            {
                SharedSimulationMesh = null;
                return;
            }

            
            PhysicsMaterial[] pqaZboUevFkcNCJvSFatAWUaGOeqbjpt = new PhysicsMaterial[]
            {
                new PhysicsMaterial
                {
                    ParticleDamping = Damping,
                    DistanceConstraintInverseStiffness = 0f,
                    VolumeConstraintInverseStiffness = 0f,
                    ShapeMatchingConstraintStiffness = 1f,
                },
            };

            List<Particle> cXrAgUlmYsuFpeRFiRuHFnOzNvUVDxBS = new List<Particle>();
            var knjtkRsUCepUkbnLWaxEfLWFfzVrvqPg = SharedSimulationMesh?.Particles;
            var TUqzzTAegNSyJqXyKPpgJzgVDQiOFPej = Mass / NeedlePoints.Count;

            for (int xEuHZAqNtBuGchoRVXMtDPOMefFBbFpf = 0; xEuHZAqNtBuGchoRVXMtDPOMefFBbFpf < NeedlePoints.Count; xEuHZAqNtBuGchoRVXMtDPOMefFBbFpf++)
            {
                if (NeedlePoints[xEuHZAqNtBuGchoRVXMtDPOMefFBbFpf] == null)
                {
                    continue;
                }

                Particle xHoURwONpDDAfNooAcvfADiGAoqnObUs;

                
                if (knjtkRsUCepUkbnLWaxEfLWFfzVrvqPg != null && xEuHZAqNtBuGchoRVXMtDPOMefFBbFpf < knjtkRsUCepUkbnLWaxEfLWFfzVrvqPg.Length)
                {
                    xHoURwONpDDAfNooAcvfADiGAoqnObUs = knjtkRsUCepUkbnLWaxEfLWFfzVrvqPg[xEuHZAqNtBuGchoRVXMtDPOMefFBbFpf];
                }
                else
                {
                    xHoURwONpDDAfNooAcvfADiGAoqnObUs = new Particle
                    {
                        Enabled = true,
                    };
                }

                xHoURwONpDDAfNooAcvfADiGAoqnObUs.InverseMass = 1f / TUqzzTAegNSyJqXyKPpgJzgVDQiOFPej;
                xHoURwONpDDAfNooAcvfADiGAoqnObUs.Position = transform.InverseTransformPoint(NeedlePoints[xEuHZAqNtBuGchoRVXMtDPOMefFBbFpf].transform.position);
                xHoURwONpDDAfNooAcvfADiGAoqnObUs.PhysicsMaterialIndex = 0;

                cXrAgUlmYsuFpeRFiRuHFnOzNvUVDxBS.Add(xHoURwONpDDAfNooAcvfADiGAoqnObUs);
            }

            
            List<ShapeMatchingConstraint> fYbGCidzRQcZDmkBVDJVTmkhGvCcjQhB = new List<ShapeMatchingConstraint>();
            for (int nfMUXOJZxopjqvilhykqaltuNYRanNeY = 0; nfMUXOJZxopjqvilhykqaltuNYRanNeY < NeedlePoints.Count; nfMUXOJZxopjqvilhykqaltuNYRanNeY++)
            {
                if (NeedlePoints[nfMUXOJZxopjqvilhykqaltuNYRanNeY] == null)
                {
                    continue;
                }

                ShapeMatchingConstraint sJHSeByKDHQelcbjgNmFPHNqhPPYBBek = new ShapeMatchingConstraint
                {
                    Enabled = true,
                    Particle = nfMUXOJZxopjqvilhykqaltuNYRanNeY,
                    PhysicsMaterialIndex = 0,
                };

                fYbGCidzRQcZDmkBVDJVTmkhGvCcjQhB.Add(sJHSeByKDHQelcbjgNmFPHNqhPPYBBek);
            }

            var mcyBvinIaYPGeWNlzJAMwThoUvOQiPKF = SharedSimulationMesh;
            if (mcyBvinIaYPGeWNlzJAMwThoUvOQiPKF == null)
            {
                mcyBvinIaYPGeWNlzJAMwThoUvOQiPKF = ScriptableObject.CreateInstance<SimulationMesh>();
#if UNITY_EDITOR
                if (!EditorUtility.IsPersistent(mcyBvinIaYPGeWNlzJAMwThoUvOQiPKF))
                {
                    AssetDatabase.CreateAsset(mcyBvinIaYPGeWNlzJAMwThoUvOQiPKF, AssetDatabase.GenerateUniqueAssetPath($"Assets/{name}_SimulationMesh.asset"));
                }
#endif
            }
            else
            {
                SharedSimulationMesh = null;
            }

            mcyBvinIaYPGeWNlzJAMwThoUvOQiPKF.Clear();
            mcyBvinIaYPGeWNlzJAMwThoUvOQiPKF.PhysicsMaterials = pqaZboUevFkcNCJvSFatAWUaGOeqbjpt;
            mcyBvinIaYPGeWNlzJAMwThoUvOQiPKF.Particles = cXrAgUlmYsuFpeRFiRuHFnOzNvUVDxBS.ToArray();
            mcyBvinIaYPGeWNlzJAMwThoUvOQiPKF.ShapeMatchingConstraints = fYbGCidzRQcZDmkBVDJVTmkhGvCcjQhB.ToArray();

#if UNITY_EDITOR
            EditorUtility.SetDirty(mcyBvinIaYPGeWNlzJAMwThoUvOQiPKF);
#endif

            SharedSimulationMesh = mcyBvinIaYPGeWNlzJAMwThoUvOQiPKF;
        }

        private void FNZFzGInPrKwmkYuddNbNLjUaZkTTzpI()
        {
            if (!Application.isPlaying && SharedSimulationMesh != null)
            {
                Gizmos.color = Color.red;
                for (int qLsNXqQzoaQPZEwrqtWlkxcRagqUkcci = 0; qLsNXqQzoaQPZEwrqtWlkxcRagqUkcci < SharedSimulationMesh.Particles.Length - 1; qLsNXqQzoaQPZEwrqtWlkxcRagqUkcci++)
                {
                    var pPiTefSNoXjLeqIEnLklJGUgHXfrrdrs = SharedSimulationMesh.Particles[qLsNXqQzoaQPZEwrqtWlkxcRagqUkcci];
                    var VJDPlrorfeMPZsOneXwZbaZsjENVvVyO = SharedSimulationMesh.Particles[qLsNXqQzoaQPZEwrqtWlkxcRagqUkcci + 1];

                    Gizmos.DrawLine(transform.TransformPoint(pPiTefSNoXjLeqIEnLklJGUgHXfrrdrs.Position), transform.TransformPoint(VJDPlrorfeMPZsOneXwZbaZsjENVvVyO.Position));
                }
            }
        }
    }
}
