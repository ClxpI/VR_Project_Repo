namespace MAGES.MeshDeformations
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A cutting tool for <see cref="DynamicSoftbodyActor"/>.
    /// </summary>
    public class Cutter : MonoBehaviour
    {
        private List<Disc> rhWfjdVvfyyqJmPvYQUZXWHHeKOpCuTl = new List<Disc>();
        private List<Disc> bafRnIENNmVLmYHYTyYgGWAkNlpfraxM = new List<Disc>();

        /// <summary>
        /// Gets or sets the actor to cut.
        /// </summary>
        [field: SerializeField]
        public DynamicSoftbodyActor ActorToCut { get; set; }

        /// <summary>
        /// Gets or sets the cut radius.
        /// </summary>
        [field: SerializeField]
        public float CutRadius { get; set; } = 0.1f;

        /// <summary>
        /// Cuts the actor.
        /// </summary>
        [ContextMenu("CutSplit")]
        public void CutSplit()
        {
            if (!enabled)
            {
                return;
            }

            if (ActorToCut.SimulationMesh.IsPartOfPhysicsWorld)
            {
                rhWfjdVvfyyqJmPvYQUZXWHHeKOpCuTl.Add(new Disc(transform.position, transform.forward, CutRadius));
            }
        }

        /// <summary>
        /// Cuts the actor.
        /// </summary>
        [ContextMenu("CutRemove")]
        public void CutRemove()
        {
            if (!enabled)
            {
                return;
            }

            if (ActorToCut.SimulationMesh.IsPartOfPhysicsWorld)
            {
                bafRnIENNmVLmYHYTyYgGWAkNlpfraxM.Add(new Disc(transform.position, transform.forward, CutRadius));
            }
        }

        private void vStMACcWLBFmTVaWpeXecFGwAGVqoFlU()
        {
            if (ActorToCut != null)
            {
                ActorToCut?.SimulationMesh.AddedToPhysicsWorld.AddListener(() =>
                {
                    UQvWdoIBArtREURvsytLqyvbCwQowhIE();
                });
                if (ActorToCut.SimulationMesh.IsPartOfPhysicsWorld)
                {
                    UQvWdoIBArtREURvsytLqyvbCwQowhIE();
                }
            }
        }

        private void lwPZMlerLRxDSrWhmWSsJLpVwuwqWtEh()
        {
            if (ActorToCut != null)
            {
                ActorToCut?.SimulationMesh.AddedToPhysicsWorld.RemoveListener(() =>
                {
                    UQvWdoIBArtREURvsytLqyvbCwQowhIE();
                });
            }
        }

        private void UQvWdoIBArtREURvsytLqyvbCwQowhIE()
        {
            ActorToCut.SimulationMesh.PhysicsWorld.PhysicsUpdate.AddListener(FryZDTFjlOHnPbaEHRzEyjsIaleDqoYs);
        }

        private void XMvmlRPOqfwfsqZgrvilTfuvcGslumLX()
        {
            ActorToCut.SimulationMesh.PhysicsWorld.PhysicsUpdate.RemoveListener(FryZDTFjlOHnPbaEHRzEyjsIaleDqoYs);
        }

        private void FryZDTFjlOHnPbaEHRzEyjsIaleDqoYs(float timeStep)
        {
            
            foreach (var cut in rhWfjdVvfyyqJmPvYQUZXWHHeKOpCuTl)
            {
                ActorToCut.CutSplit(cut);
            }

            foreach (var cut in bafRnIENNmVLmYHYTyYgGWAkNlpfraxM)
            {
                ActorToCut.CutRemove(cut);
            }

            rhWfjdVvfyyqJmPvYQUZXWHHeKOpCuTl.Clear();
            bafRnIENNmVLmYHYTyYgGWAkNlpfraxM.Clear();
        }
    }
}
