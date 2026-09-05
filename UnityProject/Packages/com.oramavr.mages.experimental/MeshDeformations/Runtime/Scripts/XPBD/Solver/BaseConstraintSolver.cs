namespace MAGES.MeshDeformations
{
    using Unity.Jobs;
    using UnityEngine;

    /// <summary>
    /// A base implementation for a user defined constraint solver.
    /// </summary>
    public abstract class BaseConstraintSolver : MonoBehaviour, IConstraintSolver
    {
        /// <summary>
        /// The physics world in which this solver is registered.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        private PhysicsWorld physicsWorld;

        /// <summary>
        /// Gets or sets the physics world in which this solver will be registered.
        /// </summary>
        public virtual PhysicsWorld PhysicsWorld
        {
            get
            {
                return physicsWorld;
            }

            set
            {
                DeregisterSolver();
                physicsWorld = value;
                RegisterSolver();
            }
        }

        /// <summary>
        /// Gets or sets the execution order of this solver. The lower the value, the earlier it will be executed along other user defined solvers.
        /// </summary>
        public virtual int ExecutionOrder { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether this solver is registered with a physics world.
        /// </summary>
        protected bool IsRegistered { get; set; }

        /// <inheritdoc cref="IConstraintSolver.RegisterSolver"/>
        public virtual void RegisterSolver()
        {
            
            if (IsRegistered && PhysicsWorld != null)
            {
                return;
            }

            if (PhysicsWorld == null)
            {
                return;
            }

            PhysicsWorld?.RegisterConstraintSolver(this);
            IsRegistered = true;
        }

        /// <inheritdoc cref="IConstraintSolver.DeregisterSolver"/>
        public virtual void DeregisterSolver()
        {
            if (PhysicsWorld == null)
            {
                return;
            }

            PhysicsWorld?.DeregisterConstraintSolver(this);
            IsRegistered = false;
        }

        /// <inheritdoc cref="IConstraintSolver.DispatchSolveJob"/>
        public abstract JobHandle DispatchSolveJob(in float timeStep, JobHandle dependsOn);

        /// <summary>
        /// Unity's OnEnable. See <see cref="MonoBehaviour"/> for more info.
        /// </summary>
        protected virtual void OnEnable()
        {
            RegisterSolver();
        }

        /// <summary>
        /// Unity's OnDisable. See <see cref="MonoBehaviour"/> for more info.
        /// </summary>
        protected virtual void OnDisable()
        {
            DeregisterSolver();
        }

        /// <summary>
        /// Unity's Reset. See <see cref="MonoBehaviour"/> for more info.
        /// </summary>
        protected virtual void Reset()
        {
            if (PhysicsWorld == null)
            {
                PhysicsWorld = FindFirstObjectByType<PhysicsWorld>();
            }
        }
    }
}
