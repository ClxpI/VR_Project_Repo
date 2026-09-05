namespace MAGES.MeshDeformations
{
    using Unity.Burst;
    using UnityEngine;

    /// <summary>
    /// A physics material for the simulation mesh.
    /// </summary>
    [BurstCompile]
    [System.Serializable]
    public struct PhysicsMaterial
    {
        /// <summary>
        /// The damping of the particles.
        /// </summary>
        [Header("Particle Properties")]
        public float ParticleDamping;

        /// <summary>
        /// The inverse stiffness (compliance) of distance constraints.
        /// </summary>
        [Header("Distance Constraint Properties")]
        public float DistanceConstraintInverseStiffness;

        /// <summary>
        /// The maximum distance a distance constraint can stretch.
        /// </summary>
        public float DistanceConstraintTearForce;

        /// <summary>
        /// The inverse stiffness (compliance) of volume constraints.
        /// </summary>
        [Header("Volume Constraint Properties")]
        public float VolumeConstraintInverseStiffness;

        /// <summary>
        /// The inverse stiffness (compliance) of collinear constraints.
        /// </summary>
        [Header("Collinear Constraint Properties")]
        public float CollinearConstraintInverseStiffness;

        /// <summary>
        /// Shape matching constraint stiffness. Range [0, 1].
        /// </summary>
        [Header("Shape Matching Constraint Properties. Range [0, 1].")]
        public float ShapeMatchingConstraintStiffness;

        /// <summary>
        /// The force excerted to a particle in order to break.
        /// </summary>
        [Header("Generic")]
        [Tooltip("A particle can break if force from a cut flagged particle exceeds this value.")]
        public float CutForceThreshold;

        /// <summary>
        /// Static friction coefficient. Range [0, 1].
        /// </summary>
        [Tooltip("The coefficient of static friction. Range [0, Infinity].")]
        public float StaticFriction;

        /// <summary>
        /// Kinetic friction coefficient. Range [0, 1].
        /// </summary>
        [Tooltip("The coefficient of kinetic friction. Range [0, Infinity].")]
        public float KineticFriction;
    }
}
