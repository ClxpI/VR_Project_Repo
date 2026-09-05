namespace MAGES.MeshDeformations
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A cutting tool for <see cref="RopeActor"/>.
    /// </summary>
    public class RopeCutter : MonoBehaviour
    {
        private List<Disc> DcXzfZyRkDYtbLKPVEJuCiuMrORImTWu = new List<Disc>();

        /// <summary>
        /// Gets or sets the actor to cut.
        /// </summary>
        [field: SerializeField]
        public RopeActor ActorToCut { get; set; }

        /// <summary>
        /// Gets or sets the cut radius.
        /// </summary>
        [field: SerializeField]
        public float CutRadius { get; set; } = 0.1f;

        /// <summary>
        /// Cuts the actor.
        /// </summary>
        [ContextMenu("CutRemove")]
        public void Cut()
        {
            if (!enabled)
            {
                return;
            }

            if (ActorToCut.SimulationMesh.IsPartOfPhysicsWorld)
            {
                DcXzfZyRkDYtbLKPVEJuCiuMrORImTWu.Add(new Disc(transform.position, transform.forward, CutRadius));
            }
        }

        private void oJqflgbZDfzjfVQBRBUCurxUadImuHrQ()
        {
            if (ActorToCut != null)
            {
                ActorToCut?.SimulationMesh.AddedToPhysicsWorld.AddListener(() =>
                {
                    IBhwUoODhOCnvVwALNAbySmHvPSBCqzd();
                });
                if (ActorToCut.SimulationMesh.IsPartOfPhysicsWorld)
                {
                    IBhwUoODhOCnvVwALNAbySmHvPSBCqzd();
                }
            }
        }

        private void BqNsXsrXLceYIlUaKJjgGRmIodBVjwRK()
        {
            if (ActorToCut != null)
            {
                ActorToCut?.SimulationMesh.AddedToPhysicsWorld.RemoveListener(() =>
                {
                    IBhwUoODhOCnvVwALNAbySmHvPSBCqzd();
                });
            }
        }

        private void IBhwUoODhOCnvVwALNAbySmHvPSBCqzd()
        {
            ActorToCut.SimulationMesh.PhysicsWorld.PhysicsUpdate.AddListener(BjueKZNLgNZynHsnMigkjFTGzupMawIu);
        }

        private void HwmufllYuOcKHRdhjhddQzIedXgUZzwX()
        {
            ActorToCut.SimulationMesh.PhysicsWorld.PhysicsUpdate.RemoveListener(BjueKZNLgNZynHsnMigkjFTGzupMawIu);
        }

        private void BjueKZNLgNZynHsnMigkjFTGzupMawIu(float timeStep)
        {
            foreach (var cut in DcXzfZyRkDYtbLKPVEJuCiuMrORImTWu)
            {
                ActorToCut.Cut(cut);
            }

            DcXzfZyRkDYtbLKPVEJuCiuMrORImTWu.Clear();
        }
    }
}
