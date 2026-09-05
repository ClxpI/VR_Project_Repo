namespace MAGES.MeshDeformations
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MAGES.MeshDeformations.Tools;
    using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Detector to detect when a particle group is colliding with the floor.
    /// </summary>
    public class FloorCollisionDetector : BaseDetector
    {
        /// <summary>
        /// Gets or sets the groups of particles to check for collision with the floor.
        /// </summary>
        [field: SerializeField]
        public List<ObservedParticleGroup> ObservedGroups { get; set; } = new List<ObservedParticleGroup>();

        /// <inheritdoc/>
        protected override void DetectorUpdate()
        {
            float JcTbHgimGVhAiylHzzThbIFwFlzmtkwM = Actor.PhysicsWorld.transform.position.y + Actor.PhysicsWorld.FloorHeight;
            float cvssnIacTFzogDSevfTIlMAElYINOVUX = Actor.PhysicsWorld.ParticleRadius;

            foreach (ObservedParticleGroup group in ObservedGroups)
            {
                bool fPouEbSOhAoWkSPBEAmyjevanwVRYbgz = false;

                Parallel.For(0, group.ObservedParticles.Count, (i) =>
                {
                    int rkgDRzSJXPzVczMgTEdinfluMBkTrQug = group.ObservedParticles[i];
                    if (Actor.SimulationMesh.RuntimeParticleData.Position[rkgDRzSJXPzVczMgTEdinfluMBkTrQug].y <= JcTbHgimGVhAiylHzzThbIFwFlzmtkwM + cvssnIacTFzogDSevfTIlMAElYINOVUX + math.EPSILON)
                    {
                        fPouEbSOhAoWkSPBEAmyjevanwVRYbgz = true;
                    }
                });

                if (fPouEbSOhAoWkSPBEAmyjevanwVRYbgz && !group.IsColliding)
                {
                    group.IsColliding = true;
                    group.OnCollisionEnter.Invoke();
                }
                else if (!fPouEbSOhAoWkSPBEAmyjevanwVRYbgz && group.IsColliding)
                {
                    group.IsColliding = false;
                    group.OnCollisionExit.Invoke();
                }
            }
        }

        private void XBkCsjBhDhhSrcquluXMBefuMsiUfchZ()
        {
            if (Actor.SharedSimulationMesh == null)
            {
                return;
            }

            
            ObservedGroups.Clear();
            ObservedParticleGroup SOsPMKlugDZjzrwXEVZqEIDatrShNYof = new ObservedParticleGroup();
            for (int NZDTBXSBJINTgyYcIBbkrWZCMzTvstmj = 0; NZDTBXSBJINTgyYcIBbkrWZCMzTvstmj < Actor.SharedSimulationMesh.Particles.Length; NZDTBXSBJINTgyYcIBbkrWZCMzTvstmj++)
            {
                SOsPMKlugDZjzrwXEVZqEIDatrShNYof.ObservedParticles.Add(NZDTBXSBJINTgyYcIBbkrWZCMzTvstmj);
            }

            ObservedGroups.Add(SOsPMKlugDZjzrwXEVZqEIDatrShNYof);
        }

        /// <summary>
        /// A group of particles to check for collision with floor.
        /// </summary>
        [System.Serializable]
        public class ObservedParticleGroup
        {
            /// <summary>
            /// Gets or sets the list of observed particles.
            /// </summary>
            [field: SerializeField]
            public List<int> ObservedParticles { get; set; } = new List<int>();

            /// <summary>
            /// Gets a value indicating whether this group is colliding with the floor.
            /// </summary>
            [field: SerializeField]
            [field: HideInInspector]
            public bool IsColliding { get; internal set; } = false;

            /// <summary>
            /// Gets or sets the event called when the observed particles collide with the floor.
            /// </summary>
            [field: SerializeField]
            public UnityEvent OnCollisionEnter { get; set; } = new UnityEvent();

            /// <summary>
            /// Gets or sets the event called when the object observed particles stop colliding with the floor.
            /// </summary>
            [field: SerializeField]
            public UnityEvent OnCollisionExit { get; set; } = new UnityEvent();
        }
    }
}
