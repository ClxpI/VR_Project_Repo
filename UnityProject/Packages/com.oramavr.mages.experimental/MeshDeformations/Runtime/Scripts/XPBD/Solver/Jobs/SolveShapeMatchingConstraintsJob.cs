namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// Solves shape matching constraints.
    /// </summary>
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    internal struct SolveShapeMatchingConstraintsJob : IJobParallelFor
    {
        /// <summary>
        /// The simulation time step squared.
        /// </summary>
        [ReadOnly]
        public float TimeStepSquared;

        

        /// <summary>
        /// The particles enabled state.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<bool> ParticlesEnabled;

        /// <summary>
        /// The particles predicted position.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<float3> ParticlesPredictedPosition;

        /// <summary>
        /// The local position based on the center of mass of each particle in the rest configuration.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<float3> ParticlesLocalRestPosition;

        /// <summary>
        /// The inverse mass of the particles.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<float> ParticlesInverseMass;

        

        /// <summary>
        /// The shape matching constraints enabled state.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<bool> ShapeMatchingConstraintsEnabled;

        /// <summary>
        /// The shape matching constraints breakable state.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<bool> ShapeMatchingConstraintsBreakable;

        /// <summary>
        /// The shape matching constraints particle indices.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<int> ShapeMatchingConstraintsParticle;

        /// <summary>
        /// The shape matching constraints physics material index.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<int> ShapeMatchingConstraintsPhysicsMaterialIndex;

        /// <summary>
        /// THe shape matching constraints simulation mesh descriptor index.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<int> ShapeMatchingConstraintsSimulationMeshDescriptorIndex;

        /// <summary>
        /// The flags indicating if the enabled state of the shape matching constraints has changed.
        /// </summary>
        [NativeDisableContainerSafetyRestriction]
        [WriteOnly]
        public NativeArray<bool> ShapeMatchingConstraintsEnabledStateHasChanged;

        /// <summary>
        /// The simulation mesh descriptors.
        /// </summary>
        
        [ReadOnly]
        public NativeArray<SimulationMeshDescriptor> SimulationMeshDescriptors;

        /// <summary>
        /// The physics materials.
        /// </summary>
        [ReadOnly]
        public NativeArray<PhysicsMaterial> PhysicsMaterials;

        

        /// <summary>
        /// The center of mass position of each simulation mesh.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<float3> CoMPosition;

        /// <summary>
        /// The center of mass rotation of each simulation mesh.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<quaternion> CoMRotation;

        

        /// <summary>
        /// The particle corrections.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<float3> ParticleCorrections;

        /// <summary>
        /// The particle corrections count.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<int> ParticleCorrectionsCount;

        /// <summary>
        /// The particle locks for interlocked operations.
        /// </summary>
        [NativeDisableParallelForRestriction]
        public NativeArray<int> ParticleLocks;

        /// <inheritdoc cref="IJobParallelFor.Execute(int)"/>
        public void Execute(int shapeMatchingConstraintIndex)
        {
            if (Unity.Burst.CompilerServices.Hint.Unlikely(!ShapeMatchingConstraintsEnabled[shapeMatchingConstraintIndex]))
            {
                return;
            }

            int cnHurCazoOIWpfHQCxAoQTPHndjBgZVa = ShapeMatchingConstraintsSimulationMeshDescriptorIndex[shapeMatchingConstraintIndex];

            SimulationMeshDescriptor vCNUBUYYZMQaHJnNQjcFDZOvWMUCfdqo = SimulationMeshDescriptors[cnHurCazoOIWpfHQCxAoQTPHndjBgZVa];

            int TKuMxNKILvddpyoMcfriSQCeZdpQPDXT = ShapeMatchingConstraintsParticle[shapeMatchingConstraintIndex] + vCNUBUYYZMQaHJnNQjcFDZOvWMUCfdqo.ParticleStartIndex;

            if (Unity.Burst.CompilerServices.Hint.Unlikely(!ParticlesEnabled[TKuMxNKILvddpyoMcfriSQCeZdpQPDXT]))
            {
                ShapeMatchingConstraintsEnabled[shapeMatchingConstraintIndex] = false;
                ShapeMatchingConstraintsEnabledStateHasChanged[cnHurCazoOIWpfHQCxAoQTPHndjBgZVa] = true;
                return;
            }

            if (Unity.Burst.CompilerServices.Hint.Unlikely(ParticlesInverseMass[TKuMxNKILvddpyoMcfriSQCeZdpQPDXT] < math.EPSILON))
            {
                return;
            }

            float3 IeXExujUbJhsZUiPxhrnMnIHZgtlFrFa = (math.rotate(CoMRotation[cnHurCazoOIWpfHQCxAoQTPHndjBgZVa], ParticlesLocalRestPosition[TKuMxNKILvddpyoMcfriSQCeZdpQPDXT]) + CoMPosition[cnHurCazoOIWpfHQCxAoQTPHndjBgZVa]) - ParticlesPredictedPosition[TKuMxNKILvddpyoMcfriSQCeZdpQPDXT];

            IeXExujUbJhsZUiPxhrnMnIHZgtlFrFa *= PhysicsMaterials[ShapeMatchingConstraintsPhysicsMaterialIndex[shapeMatchingConstraintIndex] + vCNUBUYYZMQaHJnNQjcFDZOvWMUCfdqo.PhysicsMaterialStartIndex].ShapeMatchingConstraintStiffness;

            
            
            
            ParticleCorrections.InterlockedAddFloat3(ParticleLocks, TKuMxNKILvddpyoMcfriSQCeZdpQPDXT, IeXExujUbJhsZUiPxhrnMnIHZgtlFrFa);
            ParticleCorrectionsCount.InterlockedAdd(TKuMxNKILvddpyoMcfriSQCeZdpQPDXT, 1);
        }
    }
}
