namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Debug = UnityEngine.Debug;

    /// <summary>
    /// Runtime Data
    /// </summary>
    [BurstCompile]
    internal struct RuntimeVolumeConstraintData
    {
        /// <summary>
        /// Whether the data is created.
        /// </summary>
        public bool IsCreated;

        /// <summary>
        /// The data count.
        /// </summary>
        public int Count;

        /// <summary>
        /// Enabled data.
        /// </summary>
        public NativeArray<bool> Enabled;

        /// <summary>
        /// Breakable data.
        /// </summary>
        public NativeArray<bool> Breakable;

        /// <summary>
        /// Physics Material Index data.
        /// </summary>
        public NativeArray<int> PhysicsMaterialIndex;

        /// <summary>
        /// Particle A.
        /// </summary>
        public NativeArray<int> ParticleA;

        /// <summary>
        /// Particle B.
        /// </summary>
        public NativeArray<int> ParticleB;

        /// <summary>
        /// Particle C.
        /// </summary>
        public NativeArray<int> ParticleC;

        /// <summary>
        /// Particle D.
        /// </summary>
        public NativeArray<int> ParticleD;

        /// <summary>
        /// The rest volume for each constraint.
        /// </summary>
        public NativeArray<float> RestVolume;

        /// <summary>
        /// The index of the simulation mesh descriptor for each constraint.
        /// </summary>
        public NativeArray<int> SimulationMeshDescriptorIndex;

        /// <summary>
        /// Initializes the data arrays.
        /// </summary>
        /// <param name="size">The size of the arrays.</param>
        /// <param name="nativeArrayOptions">Options for array memory initialization.</param>
        public void Create(in int size, NativeArrayOptions nativeArrayOptions = NativeArrayOptions.ClearMemory)
        {
            Enabled = new NativeArray<bool>(size, Allocator.Persistent, nativeArrayOptions);
            Breakable = new NativeArray<bool>(size, Allocator.Persistent, nativeArrayOptions);

            PhysicsMaterialIndex = new NativeArray<int>(size, Allocator.Persistent, nativeArrayOptions);

            ParticleA = new NativeArray<int>(size, Allocator.Persistent, nativeArrayOptions);
            ParticleB = new NativeArray<int>(size, Allocator.Persistent, nativeArrayOptions);
            ParticleC = new NativeArray<int>(size, Allocator.Persistent, nativeArrayOptions);
            ParticleD = new NativeArray<int>(size, Allocator.Persistent, nativeArrayOptions);

            RestVolume = new NativeArray<float>(size, Allocator.Persistent, nativeArrayOptions);

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
                Debug.LogError("DistanceConstraintData: Trying to resize data arrays, without being created first.");
                return -1;
            }

            
            Enabled.ResizeArray(newSize);
            Breakable.ResizeArray(newSize);

            PhysicsMaterialIndex.ResizeArray(newSize);

            ParticleA.ResizeArray(newSize);
            ParticleB.ResizeArray(newSize);
            ParticleC.ResizeArray(newSize);
            ParticleD.ResizeArray(newSize);

            RestVolume.ResizeArray(newSize);

            SimulationMeshDescriptorIndex.ResizeArray(newSize);

            var arwRlASGtopweQewUKKoHfIxyJHOUpqg = Count;
            Count = newSize;
            return arwRlASGtopweQewUKKoHfIxyJHOUpqg;
        }

        /// <summary>
        /// Inserts additional empty space into the data arrays.
        /// </summary>
        /// <param name="additionalSize">The size of the additional space to insert.</param>
        /// <param name="emptySpacePosition">The index at which the new empty space will be located.</param>
        /// <returns>The index at which the new cells begin.</returns>
        public int InsertAdditionalEmptySpace(in int additionalSize, in int emptySpacePosition)
        {
            var UeEauJSaXzrWRCekNZyqGAMazFQzzOcH = Count + additionalSize;

            Enabled.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.ClearMemory);
            Breakable.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            PhysicsMaterialIndex.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            ParticleA.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            ParticleB.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            ParticleC.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            ParticleD.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            RestVolume.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            SimulationMeshDescriptorIndex.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            Count = UeEauJSaXzrWRCekNZyqGAMazFQzzOcH;
            return emptySpacePosition;
        }

        /// <summary>
        /// Removes a range of data from the arrays.
        /// </summary>
        /// <param name="index">The starting index of the range to remove.</param>
        /// <param name="length">The length of the range to remove.</param>
        public void RemoveRange(in int index, in int length)
        {
            UnityEngine.Debug.Assert(index >= 0 && index + length <= Count, "RuntimeVolumeConstraintData: Specified range is out of the bounds of the array.");

            Enabled.RemoveRange(index, length, Allocator.Persistent);
            Breakable.RemoveRange(index, length, Allocator.Persistent);

            PhysicsMaterialIndex.RemoveRange(index, length, Allocator.Persistent);

            ParticleA.RemoveRange(index, length, Allocator.Persistent);
            ParticleB.RemoveRange(index, length, Allocator.Persistent);
            ParticleC.RemoveRange(index, length, Allocator.Persistent);
            ParticleD.RemoveRange(index, length, Allocator.Persistent);

            RestVolume.RemoveRange(index, length, Allocator.Persistent);

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

                ParticleA.Dispose();
                ParticleB.Dispose();
                ParticleC.Dispose();
                ParticleD.Dispose();

                RestVolume.Dispose();

                SimulationMeshDescriptorIndex.Dispose();
            }
        }
    }
}
