namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Mathematics;
    using Debug = UnityEngine.Debug;

    /// <summary>
    /// Represents the runtime data for particles in the XPBD solver.
    /// </summary>
    [BurstCompile]
    internal struct RuntimeParticleData
    {
        /// <summary>
        /// Indicates whether the particle data has been initialized.
        /// </summary>
        public bool IsCreated;

        /// <summary>
        /// The number of particles.
        /// </summary>
        public int Count;

        /// <summary>
        /// Indicates whether each particle is enabled.
        /// </summary>
        public NativeArray<bool> Enabled;

        /// <summary>
        /// Indicates whether each particle is breakable.
        /// </summary>
        public NativeArray<bool> Breakable;

        /// <summary>
        /// The index of the physics material for each particle.
        /// </summary>
        public NativeArray<int> PhysicsMaterialIndex;

        /// <summary>
        /// The initial position of each particle.
        /// </summary>
        public NativeArray<float3> InitialPosition;

        /// <summary>
        /// The current position of each particle.
        /// </summary>
        public NativeArray<float3> Position;

        /// <summary>
        /// The predicted position of each particle for the next simulation step.
        /// </summary>
        public NativeArray<float3> PredictedPosition;

        /// <summary>
        /// The temporary position buffer used for friction solving.
        /// </summary>
        public NativeArray<float3> TemporaryPositionBuffer;

        /// <summary>
        /// The local rest position of each particle relative to the simulation mesh's center of mass.
        /// </summary>
        public NativeArray<float3> LocalRestPosition;

        /// <summary>
        /// The velocity of each particle.
        /// </summary>
        public NativeArray<float3> Velocity;

        /// <summary>
        /// The inverse mass of each particle.
        /// </summary>
        public NativeArray<float> InverseMass;

        /// <summary>
        /// The adjusted inverse mass of each particle for faster convergence.
        /// </summary>
        public NativeArray<float> AdjustedInverseMass;

        /// <summary>
        /// The phase (group) of each particle for collision handling.
        /// </summary>
        public NativeArray<uint> Phase;

        /// <summary>
        /// The index of the simulation mesh descriptor for each particle.
        /// </summary>
        public NativeArray<int> SimulationMeshDescriptorIndex;

        /// <summary>
        /// The position corrections to be applied to each particle.
        /// </summary>
        public NativeArray<float3> PositionCorrections;

        /// <summary>
        /// The count of position corrections for each particle.
        /// </summary>
        public NativeArray<int> PositionCorrectionsCount;

        /// <summary>
        /// The locks array used for atomic operations on particles.
        /// </summary>
        public NativeArray<int> ParticleLocks;

        /// <summary>
        /// Initializes the particle data arrays.
        /// </summary>
        /// <param name="size">The size of the arrays.</param>
        /// <param name="nativeArrayOptions">Options for array memory initialization.</param>
        public void Create(in int size, NativeArrayOptions nativeArrayOptions = NativeArrayOptions.ClearMemory)
        {
            Enabled = new NativeArray<bool>(size, Allocator.Persistent, nativeArrayOptions);
            Breakable = new NativeArray<bool>(size, Allocator.Persistent, nativeArrayOptions);

            PhysicsMaterialIndex = new NativeArray<int>(size, Allocator.Persistent, nativeArrayOptions);

            InitialPosition = new NativeArray<float3>(size, Allocator.Persistent, nativeArrayOptions);
            PredictedPosition = new NativeArray<float3>(size, Allocator.Persistent, nativeArrayOptions);
            TemporaryPositionBuffer = new NativeArray<float3>(size, Allocator.Persistent, nativeArrayOptions);
            Position = new NativeArray<float3>(size, Allocator.Persistent, nativeArrayOptions);
            LocalRestPosition = new NativeArray<float3>(size, Allocator.Persistent, nativeArrayOptions);
            Velocity = new NativeArray<float3>(size, Allocator.Persistent, nativeArrayOptions);

            InverseMass = new NativeArray<float>(size, Allocator.Persistent, nativeArrayOptions);
            AdjustedInverseMass = new NativeArray<float>(size, Allocator.Persistent, nativeArrayOptions);
            Phase = new NativeArray<uint>(size, Allocator.Persistent, nativeArrayOptions);

            SimulationMeshDescriptorIndex = new NativeArray<int>(size, Allocator.Persistent, nativeArrayOptions);

            ParticleLocks = new NativeArray<int>(size, Allocator.Persistent, nativeArrayOptions);
            PositionCorrections = new NativeArray<float3>(size, Allocator.Persistent, nativeArrayOptions);
            PositionCorrectionsCount = new NativeArray<int>(size, Allocator.Persistent, nativeArrayOptions);

            Count = size;
            IsCreated = true;
        }

        /// <summary>
        /// Resizes the data arrays.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        /// <returns>The old size before resizing.</returns>
        public int Resize(in int newSize)
        {
            if (Unity.Burst.CompilerServices.Hint.Unlikely(Count == newSize))
            {
                return newSize;
            }

            if (Unity.Burst.CompilerServices.Hint.Unlikely(!IsCreated))
            {
                Debug.LogError("ParticleData: Trying to resize data arrays, without being created first.");
                return -1;
            }

            
            Enabled.ResizeArray(newSize);
            Breakable.ResizeArray(newSize);

            PhysicsMaterialIndex.ResizeArray(newSize);

            InitialPosition.ResizeArray(newSize);
            PredictedPosition.ResizeArray(newSize);
            Position.ResizeArray(newSize);
            TemporaryPositionBuffer.ResizeArray(newSize);
            LocalRestPosition.ResizeArray(newSize);
            Velocity.ResizeArray(newSize);

            InverseMass.ResizeArray(newSize);
            AdjustedInverseMass.ResizeArray(newSize);
            Phase.ResizeArray(newSize);

            SimulationMeshDescriptorIndex.ResizeArray(newSize);

            
            ParticleLocks.ResizeArray(newSize);
            PositionCorrections.ResizeArray(newSize);
            PositionCorrectionsCount.ResizeArray(newSize);

            var mBGmbCGkNGdGSTLhBJGTYpOiYfFdbMZx = Count;
            Count = newSize;

            return mBGmbCGkNGdGSTLhBJGTYpOiYfFdbMZx;
        }

        /// <summary>
        /// Inserts additional empty space into the data arrays.
        /// </summary>
        /// <param name="additionalSize">The size of the additional space to insert.</param>
        /// <param name="emptySpacePosition">The index at which the new empty space will be located.</param>
        /// <returns>The index at which the new cells begin.</returns>
        public int InsertAdditionalEmptySpace(in int additionalSize, in int emptySpacePosition)
        {
            var nshnmUGkNylcUkZHywOupsPAHgELlaHM = Count + additionalSize;

            Enabled.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.ClearMemory);
            Breakable.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            PhysicsMaterialIndex.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            InitialPosition.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            PredictedPosition.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            TemporaryPositionBuffer.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            Position.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            LocalRestPosition.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            Velocity.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            InverseMass.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            AdjustedInverseMass.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            Phase.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            SimulationMeshDescriptorIndex.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            ParticleLocks.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            PositionCorrections.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            PositionCorrectionsCount.InsertSpace(additionalSize, emptySpacePosition, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            Count = nshnmUGkNylcUkZHywOupsPAHgELlaHM;
            return emptySpacePosition;
        }

        /// <summary>
        /// Removes a range of data from the arrays.
        /// </summary>
        /// <param name="index">The starting index of the range to remove.</param>
        /// <param name="length">The length of the range to remove.</param>
        public void RemoveRange(in int index, in int length)
        {
            UnityEngine.Debug.Assert(index >= 0 && index + length <= Count, "RuntimeParticleData: Specified range is out of the bounds of the array.");

            Enabled.RemoveRange(index, length, Allocator.Persistent);
            Breakable.RemoveRange(index, length, Allocator.Persistent);

            PhysicsMaterialIndex.RemoveRange(index, length, Allocator.Persistent);

            InitialPosition.RemoveRange(index, length, Allocator.Persistent);
            PredictedPosition.RemoveRange(index, length, Allocator.Persistent);
            Position.RemoveRange(index, length, Allocator.Persistent);
            TemporaryPositionBuffer.RemoveRange(index, length, Allocator.Persistent);
            LocalRestPosition.RemoveRange(index, length, Allocator.Persistent);
            Velocity.RemoveRange(index, length, Allocator.Persistent);

            InverseMass.RemoveRange(index, length, Allocator.Persistent);
            AdjustedInverseMass.RemoveRange(index, length, Allocator.Persistent);
            Phase.RemoveRange(index, length, Allocator.Persistent);

            SimulationMeshDescriptorIndex.RemoveRange(index, length, Allocator.Persistent);

            ParticleLocks.RemoveRange(index, length, Allocator.Persistent);
            PositionCorrections.RemoveRange(index, length, Allocator.Persistent);
            PositionCorrectionsCount.RemoveRange(index, length, Allocator.Persistent);

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

                InitialPosition.Dispose();
                PredictedPosition.Dispose();
                TemporaryPositionBuffer.Dispose();
                Position.Dispose();
                LocalRestPosition.Dispose();
                Velocity.Dispose();

                InverseMass.Dispose();
                AdjustedInverseMass.Dispose();
                Phase.Dispose();

                SimulationMeshDescriptorIndex.Dispose();

                ParticleLocks.Dispose();
                PositionCorrections.Dispose();
                PositionCorrectionsCount.Dispose();
            }
        }
    }
}
