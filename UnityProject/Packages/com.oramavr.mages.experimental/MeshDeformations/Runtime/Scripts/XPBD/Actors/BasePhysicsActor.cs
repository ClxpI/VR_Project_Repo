namespace MAGES.MeshDeformations
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// A base class for all physics actors.
    /// </summary>
    public abstract class BasePhysicsActor : MonoBehaviour
    {
#pragma warning disable SA1401 // Fields should be private
        /// <summary>
        /// The physics world that the simulation is or will become part of.
        /// </summary>
        [SerializeField]
        protected PhysicsWorld physicsWorld;

        /// <summary>
        /// The shared simulation mesh.
        /// </summary>
        [SerializeField]
        protected SimulationMesh sharedSimulationMesh;

        /// <summary>
        /// An instance of the shared simulation mesh, used for simulating this specific actor.
        /// </summary>
        protected SimulationMesh MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj; 
#pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// Gets or sets the PhysicsWorld that the simulation is or will become part of.
        /// </summary>
        public virtual PhysicsWorld PhysicsWorld
        {
            get
            {
                return physicsWorld;
            }

            set
            {
                if (MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj != null && MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld)
                {
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RemovedFromPhysicsWorld.AddListener(() =>
                    {
                        physicsWorld = value;
                        AddToPhysicsWorld();
                    });
                    RemoveFromPhysicsWorld();
                }
                else
                {
                    if (physicsWorld == null && enabled && Application.isPlaying)
                    {
                        physicsWorld = value;
                        AddToPhysicsWorld();
                        return;
                    }

                    physicsWorld = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the shared simulation mesh.
        /// </summary>
        /// <remarks>
        /// The actual runtime simulation <see cref="SimulationMesh"/> is created from this shared mesh as an instance of it.
        /// </remarks>
        public virtual SimulationMesh SharedSimulationMesh
        {
            get
            {
                return sharedSimulationMesh;
            }

            set
            {
                if (MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj != null && MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld)
                {
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RemovedFromPhysicsWorld.AddListener(() =>
                    {
                        Destroy(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj);
                        MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj = null;
                        AddToPhysicsWorld();
                    });
                    sharedSimulationMesh = value;
                    RemoveFromPhysicsWorld();
                }
                else
                {
                    if (sharedSimulationMesh == null && enabled && Application.isPlaying)
                    {
                        sharedSimulationMesh = value;
                        AddToPhysicsWorld();
                        return;
                    }

                    sharedSimulationMesh = value;
                }
            }
        }

        /// <summary>
        /// Gets the runtime simulation mesh.
        /// </summary>
        public virtual SimulationMesh SimulationMesh
        {
            get
            {
                if (MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj == null && SharedSimulationMesh != null)
                {
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj = Instantiate(SharedSimulationMesh);
                }

                return MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the simulation mesh is part of the physics world.
        /// </summary>
        public bool IsPartOfPhysicsWorld
        {
            get
            {
                return MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj != null && MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld;
            }
        }

        /// <summary>
        /// Gets or sets the callback invoked when this actor is added to the physics world.
        /// </summary>
        [field: SerializeField]
        public UnityEvent<PhysicsWorld> AddedToPhysicsWorld { get; set; } = new UnityEvent<PhysicsWorld>();

        /// <summary>
        /// Gets or sets the callback invoked when this actor is removed from the physics world.
        /// </summary>
        [field: SerializeField]
        public UnityEvent<PhysicsWorld> RemovedFromPhysicsWorld { get; set; } = new UnityEvent<PhysicsWorld>();

        /// <summary>
        /// Override this function to add the simulation mesh to the physics world.
        /// </summary>
        public virtual void AddToPhysicsWorld()
        {
            if (PhysicsWorld == null)
            {
                return;
            }

            SimulationMesh.AddedToPhysicsWorld?.AddListener(OnAddedToPhysicsWorld);
            SimulationMesh.RemovedFromPhysicsWorld?.AddListener(OnRemovedFromPhysicsWorld);
        }

        /// <summary>
        /// Override this function to remove the simulation mesh from the physics world.
        /// </summary>
        public virtual void RemoveFromPhysicsWorld()
        {
            if (PhysicsWorld == null)
            {
                return;
            }

            PhysicsWorld.PhysicsUpdate.RemoveListener(PhysicsUpdate);
            PhysicsWorld.LatePhysicsUpdate.RemoveListener(LatePhysicsUpdate);
        }

        /// <summary>
        /// Called when the actor is actually added to the physics world.
        /// </summary>
        protected virtual void OnAddedToPhysicsWorld()
        {
            SimulationMesh.AddedToPhysicsWorld?.RemoveListener(OnAddedToPhysicsWorld);

            PhysicsWorld.PhysicsUpdate?.AddListener(PhysicsUpdate);
            PhysicsWorld.LatePhysicsUpdate?.AddListener(LatePhysicsUpdate);

            AddedToPhysicsWorld?.Invoke(PhysicsWorld);
        }

        /// <summary>
        /// Called when the simulation mesh is actually removed from the physics world.
        /// </summary>
        protected virtual void OnRemovedFromPhysicsWorld()
        {
            SimulationMesh.RemovedFromPhysicsWorld?.RemoveListener(OnRemovedFromPhysicsWorld);
            RemovedFromPhysicsWorld?.Invoke(PhysicsWorld);
        }

        /// <summary>
        /// Called every physics update. Override this to add custom physics logic.
        /// </summary>
        /// <param name="timeStep">The physics time step.</param>
        protected virtual void PhysicsUpdate(float timeStep)
        {
        }

        /// <summary>
        /// Called immediately after the physics update. Override this to add custom logic.
        /// </summary>
        /// <param name="timeStep">The physics time step.</param>
        protected virtual void LatePhysicsUpdate(float timeStep)
        {
        }

        /// <summary>
        /// Unity's OnEnable method. See <see cref="MonoBehaviour"/> for more.
        /// </summary>
        protected virtual void OnEnable()
        {
            AddToPhysicsWorld();
        }

        /// <summary>
        /// Unity's OnDisable method. See <see cref="MonoBehaviour"/> for more.
        /// </summary>
        protected virtual void OnDisable()
        {
            RemoveFromPhysicsWorld();
        }

        /// <summary>
        /// Unity's Reset method. See <see cref="MonoBehaviour"/> for more.
        /// </summary>
        protected virtual void Reset()
        {
            PhysicsWorld = FindFirstObjectByType<PhysicsWorld>();
        }
    }
}
