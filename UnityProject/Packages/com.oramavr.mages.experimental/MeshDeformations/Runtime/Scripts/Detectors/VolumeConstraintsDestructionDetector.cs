namespace MAGES.MeshDeformations.Tools
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Detects when a volume constraint is destroyed and invokes an event.
    /// </summary>
    [RequireComponent(typeof(BasePhysicsActor))]
    public class VolumeConstraintsDestructionDetector : BaseDetector
    {
        /// <summary>
        /// Gets or sets the list of observed volume constraint groups.
        /// </summary>
        [field: SerializeField]
        public List<ObservedVolumeConstraintGroup> ObservedVolumeConstraintGroups { get; set; } = new List<ObservedVolumeConstraintGroup>();

        /// <inheritdoc/>
        protected override void DetectorUpdate()
        {
            if (Actor.SimulationMesh.VolumeConstraintsEnabledStateHasChanged)
            {
                FhemVKXjcEzyaUkidtRfOtFrdYLTqvkk();
            }
        }

        private void FhemVKXjcEzyaUkidtRfOtFrdYLTqvkk()
        {
            for (int trVJCikBkAJUbmJwaadnqnbGXYGQBwGZ = 0; trVJCikBkAJUbmJwaadnqnbGXYGQBwGZ < ObservedVolumeConstraintGroups.Count; trVJCikBkAJUbmJwaadnqnbGXYGQBwGZ++)
            {
                bool RhegWdvpWDPLkPdpkTEcPRveBMcyelWx = false;
                var RBqdBjGMWtxGqlRrtrtIAswEybdhvdMh = ObservedVolumeConstraintGroups[trVJCikBkAJUbmJwaadnqnbGXYGQBwGZ];

                Parallel.For(0, RBqdBjGMWtxGqlRrtrtIAswEybdhvdMh.ObservedVolumeConstraints.Length, (i) =>
                {
                    if (RBqdBjGMWtxGqlRrtrtIAswEybdhvdMh.ObservedVolumeConstraints[i].Invoked)
                    {
                        return;
                    }

                    int LoKIZWGgWCcBHBmkdhafedspvtwupehp = RBqdBjGMWtxGqlRrtrtIAswEybdhvdMh.ObservedVolumeConstraints[i].VolumeConstraint;

                    if (!Actor.SimulationMesh.RuntimeVolumeConstraintData.Enabled[LoKIZWGgWCcBHBmkdhafedspvtwupehp])
                    {
                        RhegWdvpWDPLkPdpkTEcPRveBMcyelWx = true;

                        ObservedVolumeConstraint EPwxcDCwXxYuqZQkttkzoxgfKlCiZMFe = RBqdBjGMWtxGqlRrtrtIAswEybdhvdMh.ObservedVolumeConstraints[i];
                        EPwxcDCwXxYuqZQkttkzoxgfKlCiZMFe.Invoked = true;
                        RBqdBjGMWtxGqlRrtrtIAswEybdhvdMh.ObservedVolumeConstraints[i] = EPwxcDCwXxYuqZQkttkzoxgfKlCiZMFe;
                    }
                });

                if (RhegWdvpWDPLkPdpkTEcPRveBMcyelWx)
                {
                    RBqdBjGMWtxGqlRrtrtIAswEybdhvdMh.VolumeConstraintsDestroyed?.Invoke();
                }
            }
        }

        /// <summary>
        /// An observed volume constraint.
        /// </summary>
        [System.Serializable]
        public struct ObservedVolumeConstraint
        {
            /// <summary>
            /// The volume constraint index.
            /// </summary>
            public int VolumeConstraint;

            /// <summary>
            /// Whether the event has been already invoked.
            /// </summary>
            [HideInInspector]
            [System.NonSerialized]
            internal bool Invoked;
        }

        /// <summary>
        /// A group of observed volume constraints.
        /// </summary>
        [System.Serializable]
        public class ObservedVolumeConstraintGroup
        {
            /// <summary>
            /// Gets or sets an array of the observed volume constraints.
            /// </summary>
            [field: SerializeField]
            public ObservedVolumeConstraint[] ObservedVolumeConstraints { get; set; } = new ObservedVolumeConstraint[0];

            /// <summary>
            /// Gets or sets the event invoked when a volume constraint in the group is destroyed.
            /// </summary>
            [field: SerializeField]
            public UnityEvent VolumeConstraintsDestroyed { get; set; } = new UnityEvent();
        }
    }
}
