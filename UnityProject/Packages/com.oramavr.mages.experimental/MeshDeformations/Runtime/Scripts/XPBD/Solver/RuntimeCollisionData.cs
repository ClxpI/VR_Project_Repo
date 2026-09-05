namespace MAGES.MeshDeformations
{
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Debug = UnityEngine.Debug;

    /// <summary>
    /// Runtime collision data.
    /// </summary>
    [BurstCompile]
    internal struct RuntimeCollisionData
    {
        /// <summary>
        /// The collision count.
        /// </summary>
        public NativeReference<int> Count;

        /// <summary>
        /// The actual collisions.
        /// </summary>
        public NativeArray<ContactPair> Collisions;

        private int kihAKmkLAcIpkNbkTDJEtjkKcxQmafYZ;
        private Allocator ozgZVDbOEhaAOGadtuXRvLpQhSZjYXzP;

        /// <summary>
        /// Gets a value indicating whether this data is created.
        /// </summary>
        public bool IsCreated { get; private set; }

        /// <summary>
        /// Gets or sets the capacity of the data.
        /// </summary>
        public int Capacity
        {
            get => Collisions.Length;
            set
            {
                Debug.Assert(value > kihAKmkLAcIpkNbkTDJEtjkKcxQmafYZ, "Can not lower capacity. Only increase of capacity is allowed");

                Collisions.ResizeArray(Capacity, ozgZVDbOEhaAOGadtuXRvLpQhSZjYXzP, NativeArrayOptions.UninitializedMemory);
                kihAKmkLAcIpkNbkTDJEtjkKcxQmafYZ = value;
            }
        }

        /// <summary>
        /// Create the runtime collision data buffers.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="allocator">The allocator.</param>
        /// <param name="nativeArrayOptions">The array options.</param>
        public void Create(in int capacity, Allocator allocator, NativeArrayOptions nativeArrayOptions = NativeArrayOptions.ClearMemory)
        {
            this.ozgZVDbOEhaAOGadtuXRvLpQhSZjYXzP = allocator;
            Count = new NativeReference<int>(allocator, nativeArrayOptions);
            Count.Value = 0;
            Collisions = new NativeArray<ContactPair>(capacity, allocator, nativeArrayOptions);
        }

        /// <summary>
        /// Disposes the allocated memory.
        /// </summary>
        public void Dispose()
        {
            IsCreated = false;
            kihAKmkLAcIpkNbkTDJEtjkKcxQmafYZ = 0;
            Count.Dispose();
            Collisions.Dispose();
        }
    }
}