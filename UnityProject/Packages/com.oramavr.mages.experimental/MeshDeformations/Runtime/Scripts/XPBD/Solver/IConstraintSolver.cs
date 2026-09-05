namespace MAGES.MeshDeformations
{
    using Unity.Jobs;

    /// <summary>
    /// Interface that must be implemented by all user defined constraint solver.
    /// </summary>
    internal interface IConstraintSolver
    {
        /// <summary>
        /// Gets the physics world in which this solver will be registered.
        /// </summary>
        public PhysicsWorld PhysicsWorld { get; }

        /// <summary>
        /// Gets the execution order of this solver. The lower the value, the earlier it will be executed along other user defined solvers.
        /// </summary>
        public int ExecutionOrder { get; }

        /// <summary>
        /// Registers the solver to the physics world.
        /// </summary>
        public void RegisterSolver();

        /// <summary>
        /// Deregisters the solver from the physics world.
        /// </summary>
        public void DeregisterSolver();

        /// <summary>
        /// Called by the physics world when it needs to dispatch this solver's jobs.
        /// </summary>
        /// <param name="timeStep">The physics update timestep.</param>
        /// <param name="dependsOn">A job dependency.</param>
        /// <returns>Returns the job handle of the dispatched job.</returns>
        JobHandle DispatchSolveJob(in float timeStep, JobHandle dependsOn);
    }
}
