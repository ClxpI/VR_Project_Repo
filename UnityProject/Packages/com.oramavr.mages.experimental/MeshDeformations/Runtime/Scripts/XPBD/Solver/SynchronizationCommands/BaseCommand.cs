namespace MAGES.MeshDeformations.Synchronization
{
    /// <summary>
    /// A base command used for synchronization. All commands must inherit from this class.
    /// </summary>
    internal abstract class BaseCommand
    {
        /// <summary>
        /// Apply this command.
        /// </summary>
        /// <param name="physicsWorld">The physics world in which to apply it to.</param>
        public abstract void Apply(PhysicsWorld physicsWorld);
    }
}
