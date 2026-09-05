namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;

    /// <summary>
    /// Represents the runtime data for shape matching constraints in the XPBD solver.
    /// </summary>
    [BurstCompile]
    internal struct RuntimeShapeMatchingConstraintData
    {
        /// <summary>
        /// Indicates whether the constraint data has been initialized.
        /// </summary>
        public bool IsCreated;

        /// <summary>
        /// The number of constraints.
        /// </summary>
        public int Count;

        /// <summary>
        /// Indicates whether each constraint is enabled.
        /// </summary>
        public NativeArray<bool> Enabled;

        /// <summary>
        /// Indicates whether each constraint is breakable.
        /// </summary>
        public NativeArray<bool> Breakable;

        /// <summary>
        /// The index of the physics material for each constraint.
        /// </summary>
        public NativeArray<int> PhysicsMaterialIndex;

        /// <summary>
        /// The index of the particle involved in each constraint.
        /// </summary>
        public NativeArray<int> Particle;

        /// <summary>
        /// The index of the simulation mesh descriptor for each constraint.
        /// </summary>
        public NativeArray<int> SimulationMeshDescriptorIndex;

        /// <summary>
        /// Initializes the constraint data arrays.
        /// </summary>
        /// <param name="size">The size of the arrays.</param>
        /// <param name="nativeArrayOptions">Options for array memory initialization.</param>
        public void Create(in int size, NativeArrayOptions nativeArrayOptions = NativeArrayOptions.ClearMemory)
        {
            Enabled = new NativeArray<bool>(size, Allocator.Persistent, nativeArrayOptions);
            Breakable = new NativeArray<bool>(size, Allocator.Persistent, nativeArrayOptions);

            PhysicsMaterialIndex = new NativeArray<int>(size, Allocator.Persistent, nativeArrayOptions);

            Particle = new NativeArray<int>(size, Allocator.Persistent, nativeArrayOptions);

            SimulationMeshDescriptorIndex = new NativeArray<int>(size, Allocator.Persistent, nativeArrayOptions);

            Count = size;
            IsCreated = true;
        }

        /// <summary>
        /// Resizes the data arrays.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        /// <returns>The index at which the new cells begin.</returns>
        public int Resize(in int newSize)
        {
            if (Unity.Burst.CompilerServices.Hint.Unlikely(Count == newSize))
            {
                return newSize;
            }

            if (Unity.Burst.CompilerServices.Hint.Unlikely(!IsCreated))
            {
                UnityEngine.Debug.LogError("DistanceConstraintData: Trying to resize data arrays, without being created first.");
                return -1;
            }

            
            Enabled.ResizeArray(newSize);
            Breakable.ResizeArray(newSize);

            PhysicsMaterialIndex.ResizeArray(newSize);

            Particle.ResizeArray(newSize);

            SimulationMeshDescriptorIndex.ResizeArray(newSize);

            var qSPlsTNgYvOivrLYmoTJqnFXQxebPpxD = Count;
            Count = newSize;
            return qSPlsTNgYvOivrLYmoTJqnFXQxebPpxD;
        }

        /// <summary>
        /// Inserts additional empty space into the data arrays.
        /// </summary>
        /// <param name="additionalSize">The size of the additional space to insert.</param>
        /// <param name="emptySpacePosition">The index at which the new empty space will be located.</param>
        /// <returns>The index at which the new cells begin.</returns>
        public int InsertAdditionalEmptySpace(in int additionalSize, in int emptySpacePosition)
        {
            var hOnNtxtfqUswufvWMJEXRCncnsDrqniV = Count + additionalSize;

            Enabled.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.ClearMemory);
            Breakable.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            PhysicsMaterialIndex.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            Particle.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            SimulationMeshDescriptorIndex.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            Count = hOnNtxtfqUswufvWMJEXRCncnsDrqniV;
            return emptySpacePosition;
        }

        /// <summary>
        /// Removes a range of data from the arrays.
        /// </summary>
        /// <param name="index">The starting index of the range to remove.</param>
        /// <param name="length">The length of the range to remove.</param>
        public void RemoveRange(in int index, in int length)
        {
            UnityEngine.Debug.Assert(index >= 0 && index + length <= Count, "RuntimeShapeMatchingConstraintData: Specified range is out of the bounds of the array.");

            Enabled.RemoveRange(index, length, Allocator.Persistent);
            Breakable.RemoveRange(index, length, Allocator.Persistent);

            PhysicsMaterialIndex.RemoveRange(index, length, Allocator.Persistent);

            Particle.RemoveRange(index, length, Allocator.Persistent);

            SimulationMeshDescriptorIndex.RemoveRange(index, length, Allocator.Persistent);

            Count -= length;
        }

        /// <summary>
        /// Disposes of the native arrays.
        /// </summary>
        public void Dispose()
        {
            IsCreated = false;
            Count = 0;

            if (Unity.Burst.CompilerServices.Hint.Likely(IsCreated))
            {
                Enabled.Dispose();
                Breakable.Dispose();

                PhysicsMaterialIndex.Dispose();

                Particle.Dispose();

                SimulationMeshDescriptorIndex.Dispose();
            }
        }
    }
}