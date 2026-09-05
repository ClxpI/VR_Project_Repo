namespace MAGES.MeshDeformations.Utilities
{
    using System.Threading;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Mathematics;

    /// <summary>
    /// Extension methods for Unity's NativeCollections.
    /// </summary>
    public static class NativeCollectionsExtensions
    {
        /// <summary>
        /// Atomic add for float3 in a NativeArray.
        /// </summary>
        /// <param name="arr">The array.</param>
        /// <param name="locks">The locks array.</param>
        /// <param name="index">The index of the position to add to.</param>
        /// <param name="val">The value to add to that position.</param>
        /// <returns>The new value that was stored at <paramref name="index"/> by this operation.</returns>
        public static float3 InterlockedAddFloat3(this NativeArray<float3> arr, NativeArray<int> locks, int index, float3 val)
        {
            unsafe
            {
                int* dgObAIFDbjaTODxqyBOtWFQiendDyYpL = (int*)locks.GetUnsafePtr();

                
                while (true)
                {
                    if (Interlocked.CompareExchange(ref dgObAIFDbjaTODxqyBOtWFQiendDyYpL[index], 1, 0) == 0)
                    {
                        break;
                    }
                }

                
                var tPwwKoVZAvbvPhIsGhrdAyOefsQwARAw = arr[index] += val;

                
                dgObAIFDbjaTODxqyBOtWFQiendDyYpL[index] = 0;

                return tPwwKoVZAvbvPhIsGhrdAyOefsQwARAw;
            }
        }

        /// <summary>
        /// Atomic add for float in a NativeArray.
        /// </summary>
        /// <param name="arr">The array.</param>
        /// <param name="locks">The locks array.</param>
        /// <param name="index">The index in which to add to.</param>
        /// <param name="val">The value to add.</param>
        /// <returns>The new value that was stored at <paramref name="index"/> by this operation.</returns>
        public static float InterlockedAddFloat(this NativeArray<float> arr, NativeArray<int> locks, int index, float val)
        {
            unsafe
            {
                int* kOjlNLuUyoFMGIcjOOjMleAnxaGiPFiL = (int*)locks.GetUnsafePtr();

                
                while (true)
                {
                    if (Interlocked.CompareExchange(ref kOjlNLuUyoFMGIcjOOjMleAnxaGiPFiL[index], 1, 0) == 0)
                    {
                        break;
                    }
                }

                
                var YclrdSMQZqfyuKiinsSuQstRvpNHiSot = arr[index] += val;

                
                kOjlNLuUyoFMGIcjOOjMleAnxaGiPFiL[index] = 0;

                return YclrdSMQZqfyuKiinsSuQstRvpNHiSot;
            }
        }

        /// <summary>
        /// Atomically adds a value to an integer in a NativeArray.
        /// </summary>
        /// <param name="arr">The array.</param>
        /// <param name="index">The index of the array in which to add to.</param>
        /// <param name="val">The value to add.</param>
        /// <returns>The new value that was stored at <paramref name="index"/> by this operation.</returns>
        public static int InterlockedAdd(this NativeArray<int> arr, int index, int val)
        {
            unsafe
            {
                int* BDcdbfIgiGEFkHiyJbZiouQlgAJoTUPJ = (int*)arr.GetUnsafePtr();

                return Interlocked.Add(ref BDcdbfIgiGEFkHiyJbZiouQlgAJoTUPJ[index], val);
            }
        }

        /// <summary>
        /// Atomically adds a value to an integer in a NativeReference.
        /// </summary>
        /// <param name="nativeRef">The native ref.</param>
        /// <param name="val">The value to add.</param>
        /// <returns>The new value that was stored at <paramref name="nativeRef"/> by this operation.</returns>
        public static int InterlockedAdd(this NativeReference<int> nativeRef, int val)
        {
            unsafe
            {
                int* xuCDftaiIpdJBMthLMxGdYTSoJrwhjQJ = nativeRef.GetUnsafePtr();

                return Interlocked.Add(ref *xuCDftaiIpdJBMthLMxGdYTSoJrwhjQJ, val);
            }
        }

        /// <summary>
        /// Compares two values for equality and, if they are equal, replaces the first value.
        /// </summary>
        /// <param name="arr">The array.</param>
        /// <param name="index">The index of the value whose value is compared with the value of <paramref name="comparand"/> and possibly replaced by <paramref name="value"/>.</param>
        /// <param name="value">The value that replaces the destination value if the comparison results in equality.</param>
        /// <param name="comparand">The value that is compared to the value of the array at <paramref name="index"/>.</param>
        /// <returns>The original value of the array at <paramref name="index"/>.</returns>
        public static int InterlockedCompareExchange(this NativeArray<int> arr, int index, int value, int comparand)
        {
            unsafe
            {
                int* EFsEiNTMansbEstxSerXlsDkqrvJBxvb = (int*)arr.GetUnsafePtr();

                return Interlocked.CompareExchange(ref EFsEiNTMansbEstxSerXlsDkqrvJBxvb[index], value, comparand);
            }
        }

        /// <summary>
        /// Resizes the array.
        /// </summary>
        /// <typeparam name="T">The type of the NativeArray.</typeparam>
        /// <param name="array">The array to resize.</param>
        /// <param name="capacity">The new size of the array.</param>
        /// <param name="allocator">The memory allocator.</param>
        /// <param name="options">The memory options.</param>
        public static void ResizeArray<T>(this ref NativeArray<T> array, int capacity, Allocator allocator = Allocator.Persistent, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
            where T : struct
        {
            if (array.Length == capacity)
            {
                return;
            }

            var PNYwKAGsbQPuCbHhXhHuystNWpqLbNtl = new NativeArray<T>(capacity, allocator, options);
            if (array.IsCreated)
            {
                NativeArray<T>.Copy(array, PNYwKAGsbQPuCbHhXhHuystNWpqLbNtl, math.min(array.Length, capacity));
                array.Dispose();
            }

            array = PNYwKAGsbQPuCbHhXhHuystNWpqLbNtl;
        }

        /// <summary>
        /// Inserts space into the array.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array.</typeparam>
        /// <param name="array">The array to modify.</param>
        /// <param name="additionalSize">The size of the space to insert.</param>
        /// <param name="insertionIndex">The index at which the empty space will begin.</param>
        /// <param name="allocator">The memory allocator.</param>
        /// <param name="newSpaceOptions">The memory options for the new space.</param>
        public static void InsertSpace<T>(this ref NativeArray<T> array, int additionalSize, int insertionIndex, Allocator allocator, NativeArrayOptions newSpaceOptions = NativeArrayOptions.ClearMemory)
            where T : struct
        {
            var PdNHnEKskNeeGlOyMBbbCYWammGBLYha = new NativeArray<T>(array.Length + additionalSize, allocator, NativeArrayOptions.UninitializedMemory);

            if (array.IsCreated)
            {
                if (insertionIndex != 0)
                {
                    NativeArray<T>.Copy(array, 0, PdNHnEKskNeeGlOyMBbbCYWammGBLYha, 0, insertionIndex); 
                }

                if (insertionIndex < array.Length)
                {
                    NativeArray<T>.Copy(array, insertionIndex, PdNHnEKskNeeGlOyMBbbCYWammGBLYha, insertionIndex + additionalSize, array.Length - insertionIndex); 
                }

                array.Dispose();
            }

            if (newSpaceOptions == NativeArrayOptions.ClearMemory)
            {
                for (int aeJBKabTYkNuGNBecghIfVIraGdWIDeq = insertionIndex; aeJBKabTYkNuGNBecghIfVIraGdWIDeq < insertionIndex + additionalSize; aeJBKabTYkNuGNBecghIfVIraGdWIDeq++)
                {
                    PdNHnEKskNeeGlOyMBbbCYWammGBLYha[aeJBKabTYkNuGNBecghIfVIraGdWIDeq] = default;
                }
            }

            array = PdNHnEKskNeeGlOyMBbbCYWammGBLYha;
        }

        /// <summary>
        /// Removes a range of elements from the array.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array.</typeparam>
        /// <param name="array">The array to modify.</param>
        /// <param name="index">The index of the first element to remove.</param>
        /// <param name="length">The length of the range.</param>
        /// <param name="allocator">The memory allocator.</param>
        public static void RemoveRange<T>(this ref NativeArray<T> array, int index, int length, Allocator allocator)
            where T : struct
        {
            UnityEngine.Debug.Assert(index >= 0 && index + length <= array.Length, "Specified range is out of the bounds of the array.");

            if (!array.IsCreated || length == 0)
            {
                return;
            }

            var XcYfSZHEeRuxjRyxVneHEsQAfOsJNDvW = new NativeArray<T>(array.Length - length, allocator, NativeArrayOptions.UninitializedMemory);
            if (array.Length > 1)
            {
                if (index > 0)
                {
                    NativeArray<T>.Copy(array, 0, XcYfSZHEeRuxjRyxVneHEsQAfOsJNDvW, 0, index);
                }

                if (index + length < array.Length)
                {
                    NativeArray<T>.Copy(array, index + length, XcYfSZHEeRuxjRyxVneHEsQAfOsJNDvW, index, array.Length - index - length);
                }

                array.Dispose();
            }

            array = XcYfSZHEeRuxjRyxVneHEsQAfOsJNDvW;
        }

        /// <summary>
        /// Remove item at position.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array.</typeparam>
        /// <param name="array">The array to modify.</param>
        /// <param name="index">The index of the element to remove.</param>
        /// <param name="allocator">The memory allocator.</param>
        public static void RemoveAt<T>(this ref NativeArray<T> array, int index, Allocator allocator)
            where T : struct
        {
            UnityEngine.Debug.Assert(index >= 0 && index < array.Length, "Index out of bounds of the array.");

            if (!array.IsCreated || array.Length == 0)
            {
                return;
            }

            var qkuvPNPTyEGjHcOfBuhhhXfFOASlNMHc = new NativeArray<T>(array.Length - 1, allocator, NativeArrayOptions.UninitializedMemory);

            if (index > 0)
            {
                
                NativeArray<T>.Copy(array, 0, qkuvPNPTyEGjHcOfBuhhhXfFOASlNMHc, 0, index);
            }

            if (index < array.Length - 1)
            {
                
                NativeArray<T>.Copy(array, index + 1, qkuvPNPTyEGjHcOfBuhhhXfFOASlNMHc, index, array.Length - index - 1);
            }

            
            array.Dispose();

            
            array = qkuvPNPTyEGjHcOfBuhhhXfFOASlNMHc;
        }
    }
}
