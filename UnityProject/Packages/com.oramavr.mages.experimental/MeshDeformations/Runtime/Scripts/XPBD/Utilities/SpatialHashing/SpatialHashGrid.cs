namespace MAGES.MeshDeformations
{
    using System;
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Jobs;
    using Unity.Mathematics;

    /// <summary>
    /// A spatial hash grid for fast neighbor search.
    /// </summary>
    internal class SpatialHashGrid
    {
#pragma warning disable SA1401 // Fields should be private
        /// <summary>
        /// The lookup indices array.
        /// </summary>
        public NativeArray<int> SpatialLookUpItemIndices = new();

        /// <summary>
        /// The cell keys.
        /// </summary>
        public NativeArray<int> SpatialLookUpCellKeys = new();

        /// <summary>
        /// The index at which each cell starts.
        /// </summary>
        public NativeArray<int> StartIndices = new();

        /// <summary>
        /// The query results counter.
        /// </summary>
        public UnsafeAtomicCounter32 QueryResultsCount;

        /// <summary>
        /// The query results.
        /// </summary>
        public NativeArray<int> QueryResults = new();
#pragma warning restore SA1401 // Fields should be private

        private static readonly int3[] oWBBqJZKvCnSaNPhUGLCmKQEGbbcpfsa = new int3[27]
        {
            new int3(-1, -1, -1),
            new int3(-1, -1, 0),
            new int3(-1, -1, 1),
            new int3(-1, 0, -1),
            new int3(-1, 0, 0),
            new int3(-1, 0, 1),
            new int3(-1, 1, -1),
            new int3(-1, 1, 0),
            new int3(-1, 1, 1),
            new int3(0, -1, -1),
            new int3(0, -1, 0),
            new int3(0, -1, 1),
            new int3(0, 0, -1),
            new int3(0, 0, 0),
            new int3(0, 0, 1),
            new int3(0, 1, -1),
            new int3(0, 1, 0),
            new int3(0, 1, 1),
            new int3(1, -1, -1),
            new int3(1, -1, 0),
            new int3(1, -1, 1),
            new int3(1, 0, -1),
            new int3(1, 0, 0),
            new int3(1, 0, 1),
            new int3(1, 1, -1),
            new int3(1, 1, 0),
            new int3(1, 1, 1),
        };

        private static readonly int ViRusFfWWnKPlXvneeLhzmGZFLbTixwj = 92837111;
        private static readonly int GvMWWcmtPsNQiQWsUuzTdoThZVAQRaVb = 689287499;
        private static readonly int xFihpibrdlTYZdhwrjUUyrdqYaXtydGD = 283923481;

        private float CYfTqSqrsXUlRAwotWHASrlaTJDYBZxl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialHashGrid"/> class.
        /// </summary>
        /// <param name="cellSize">the cell size.</param>
        /// <param name="itemCount">The items count.</param>
        public SpatialHashGrid(float cellSize, int itemCount)
        {
            this.CYfTqSqrsXUlRAwotWHASrlaTJDYBZxl = cellSize;

            unsafe
            {
                int* swtIyRJUFlwZJdfLFoxYYVsaEsPkweOX = (int*)UnsafeUtility.Malloc(sizeof(int), 4, Allocator.Persistent);
                QueryResultsCount = new UnsafeAtomicCounter32(swtIyRJUFlwZJdfLFoxYYVsaEsPkweOX);
            }

            SetItemsCount(itemCount);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="SpatialHashGrid"/> class.
        /// </summary>
        ~SpatialHashGrid()
        {
            SpatialLookUpItemIndices.Dispose();
            SpatialLookUpCellKeys.Dispose();
            StartIndices.Dispose();

            unsafe
            {
                if (QueryResultsCount.Counter != null)
                {
                    UnsafeUtility.Free(QueryResultsCount.Counter, Allocator.Persistent);
                }
            }

            QueryResults.Dispose();
        }

        /// <summary>
        /// Gets or sets the cell size.
        /// </summary>
        public float CellSize
        {
            get => CYfTqSqrsXUlRAwotWHASrlaTJDYBZxl;
            set => CYfTqSqrsXUlRAwotWHASrlaTJDYBZxl = value;
        }

        /// <summary>
        /// Sets the items count.
        /// </summary>
        /// <param name="itemCount">The new number of items.</param>
        public void SetItemsCount(int itemCount)
        {
            SpatialLookUpItemIndices.Dispose();
            SpatialLookUpCellKeys.Dispose();
            StartIndices.Dispose();
            SpatialLookUpItemIndices = new NativeArray<int>(itemCount, Allocator.Persistent);
            SpatialLookUpCellKeys = new NativeArray<int>(itemCount, Allocator.Persistent);
            StartIndices = new NativeArray<int>(itemCount, Allocator.Persistent);

            QueryResultsCount.Reset();
            QueryResults.ResizeArray(itemCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        }

        /// <summary>
        /// Recalculates the grid.
        /// </summary>
        /// <param name="particlePositions">The particle positions.</param>
        /// <param name="dependsOn">A dependency job.</param>
        /// <returns>The job handle.</returns>
        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        public JobHandle RecalculateGrid(NativeArray<float3> particlePositions, JobHandle dependsOn = default)
        {
            UnityEngine.Debug.Assert(particlePositions.Length == SpatialLookUpItemIndices.Length);

            JobHandle GiWqXIoknlMtjHlOqBjuirZzQHfMWUKW = dependsOn;

            var niYTncQXkWADDqYCtrXNDkYZroOXvhyd = new NwdprJNfXtbfNkzzHmPCGbBgtCZFrfer
            {
                CellSize = CYfTqSqrsXUlRAwotWHASrlaTJDYBZxl,
                ParticlesPosition = particlePositions,
                SpatialLookUpCellKeys = SpatialLookUpCellKeys,
                SpatialLookUpItemIndices = SpatialLookUpItemIndices,
                StartIndices = StartIndices,
            };

            GiWqXIoknlMtjHlOqBjuirZzQHfMWUKW = niYTncQXkWADDqYCtrXNDkYZroOXvhyd.Schedule(particlePositions.Length, 64, GiWqXIoknlMtjHlOqBjuirZzQHfMWUKW);

            GiWqXIoknlMtjHlOqBjuirZzQHfMWUKW = LCMvisBomlAJzTFHCSSCTNOsItbeJiDx(SpatialLookUpItemIndices, SpatialLookUpCellKeys, GiWqXIoknlMtjHlOqBjuirZzQHfMWUKW);

            RbhtdxjFuEpjlbNnQuFxvcCcIpXYZVnj GewPGemfEztJeiZKTOSrcugElDbkESFn = new RbhtdxjFuEpjlbNnQuFxvcCcIpXYZVnj
            {
                SpatialLookUpItemIndices = SpatialLookUpItemIndices,
                SpatialLookUpCellKeys = SpatialLookUpCellKeys,
                StartIndices = StartIndices,
            };

            GiWqXIoknlMtjHlOqBjuirZzQHfMWUKW = GewPGemfEztJeiZKTOSrcugElDbkESFn.Schedule(particlePositions.Length, 64, GiWqXIoknlMtjHlOqBjuirZzQHfMWUKW);

            return GiWqXIoknlMtjHlOqBjuirZzQHfMWUKW;
        }

        /// <summary>
        /// Collects the contact pairs.
        /// </summary>
        /// <param name="particleRadius">The radius of the particles.</param>
        /// <param name="particlesPredictedPosition">The particles predicted positions.</param>
        /// <param name="contactPairs">The contact pairs generated.</param>
        /// <param name="contactPairsCount">The number of contact pairs generated.</param>
        /// <param name="dependsOn">A job dependency.</param>
        /// <returns>The job handle.</returns>
        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        public JobHandle CollectContactPairs(in float particleRadius, ref NativeArray<float3> particlesPredictedPosition, ref NativeArray<ContactPair> contactPairs, ref NativeReference<int> contactPairsCount, JobHandle dependsOn = default)
        {
            var ydtyZrWxbcmRbJWFCVfXUIiSliMcSZaW = new NWRHWNkWeuEfyyqaXXpygKCKbPyXMsjP
            {
                ParticleRadius = particleRadius,
                GridCellSize = CYfTqSqrsXUlRAwotWHASrlaTJDYBZxl,
                ParticlesPredictedPosition = particlesPredictedPosition,
                SpatialLookUpItemIndices = SpatialLookUpItemIndices,
                SpatialLookUpCellKeys = SpatialLookUpCellKeys,
                StartIndices = StartIndices,
                ContactPairs = contactPairs,
                ContactPairsCount = contactPairsCount,
            };

            return ydtyZrWxbcmRbJWFCVfXUIiSliMcSZaW.Schedule(particlesPredictedPosition.Length, 64, dependsOn);
        }

        /// <summary>
        /// Queries the grid.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="maxDistance">The distance from the position.</param>
        public void Query(in float3 position, in float maxDistance)
        {
            int3 drxrjWPQjqvJiSIfKtNAJvsEsCVRxUYq = poRBhbJWXilLsFKspjMSyonkEBPkrmyn(position - maxDistance, CYfTqSqrsXUlRAwotWHASrlaTJDYBZxl);
            int3 QSahenAUNVtUBGHyHmnPsgNpysPWPsIu = poRBhbJWXilLsFKspjMSyonkEBPkrmyn(position + maxDistance, CYfTqSqrsXUlRAwotWHASrlaTJDYBZxl);

            QueryResultsCount.Reset();

            for (int LfQnkZbeuaDyFuIrhFfiFBZTVJFfndLm = drxrjWPQjqvJiSIfKtNAJvsEsCVRxUYq.x; LfQnkZbeuaDyFuIrhFfiFBZTVJFfndLm <= QSahenAUNVtUBGHyHmnPsgNpysPWPsIu.x; LfQnkZbeuaDyFuIrhFfiFBZTVJFfndLm++)
            {
                for (int vmbWMIQPFSxPOiFaXRLYGcdMHmJCSZcc = drxrjWPQjqvJiSIfKtNAJvsEsCVRxUYq.y; vmbWMIQPFSxPOiFaXRLYGcdMHmJCSZcc <= QSahenAUNVtUBGHyHmnPsgNpysPWPsIu.y; vmbWMIQPFSxPOiFaXRLYGcdMHmJCSZcc++)
                {
                    for (int aHpQBeMTArbqLcrNdmIWBTByZLGFzUXy = drxrjWPQjqvJiSIfKtNAJvsEsCVRxUYq.z; aHpQBeMTArbqLcrNdmIWBTByZLGFzUXy <= QSahenAUNVtUBGHyHmnPsgNpysPWPsIu.z; aHpQBeMTArbqLcrNdmIWBTByZLGFzUXy++)
                    {
                        int3 QIBeMXsiprTRKodBARATsMpndQozhqbw = new int3(LfQnkZbeuaDyFuIrhFfiFBZTVJFfndLm, vmbWMIQPFSxPOiFaXRLYGcdMHmJCSZcc, aHpQBeMTArbqLcrNdmIWBTByZLGFzUXy);
                        int aBMLpWnHBgXDqDSuOiexgwiBBVdeScmb;

                        int WcPWNkaXPyJKKlEKtPqNTGYUibsheaJK = nVdmojPflRyZXvZhkkTMuYCybmCBsSDj(SbrfOgHZRzveNUMrRmvqrfnvStlTOFnE(QIBeMXsiprTRKodBARATsMpndQozhqbw), SpatialLookUpItemIndices.Length);

                        if (WcPWNkaXPyJKKlEKtPqNTGYUibsheaJK < 0 || WcPWNkaXPyJKKlEKtPqNTGYUibsheaJK >= StartIndices.Length)
                        {
                            continue;
                        }

                        int HPGqzaKGmalDhKNxDBrFoTNYiADPOdJv = StartIndices[WcPWNkaXPyJKKlEKtPqNTGYUibsheaJK];

                        for (int JANuSusmsKvkWKHcnJFFLdxTAyLxzyDi = HPGqzaKGmalDhKNxDBrFoTNYiADPOdJv; JANuSusmsKvkWKHcnJFFLdxTAyLxzyDi < SpatialLookUpItemIndices.Length; JANuSusmsKvkWKHcnJFFLdxTAyLxzyDi++)
                        {
                            aBMLpWnHBgXDqDSuOiexgwiBBVdeScmb = SpatialLookUpCellKeys[JANuSusmsKvkWKHcnJFFLdxTAyLxzyDi];

                            
                            if (aBMLpWnHBgXDqDSuOiexgwiBBVdeScmb != WcPWNkaXPyJKKlEKtPqNTGYUibsheaJK)
                            {
                                break;
                            }

                            var SDZogLydbloFKVsboYPTqcUbJJEmRBvr = SpatialLookUpItemIndices[JANuSusmsKvkWKHcnJFFLdxTAyLxzyDi];

                            unsafe
                            {
                                int WTmaZkpQzqeVcWadRIKTGEikuatmyZey = QueryResults.IndexOf(SDZogLydbloFKVsboYPTqcUbJJEmRBvr);
                                if (WTmaZkpQzqeVcWadRIKTGEikuatmyZey < 0 || WTmaZkpQzqeVcWadRIKTGEikuatmyZey >= *QueryResultsCount.Counter) 
                                {
                                    QueryResults[*QueryResultsCount.Counter] = SDZogLydbloFKVsboYPTqcUbJJEmRBvr;
                                    *QueryResultsCount.Counter = *QueryResultsCount.Counter + 1;
                                }
                            }
                        }
                    }
                }
            }
        }

        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private static uint SbrfOgHZRzveNUMrRmvqrfnvStlTOFnE(int3 cellCoords)
        {
            int GZDydjjxNsHIIiZjPNjZvEvPthIOMlAj = cellCoords.x * ViRusFfWWnKPlXvneeLhzmGZFLbTixwj;
            int ssHuqmejLkHcZNKbdoaTbpupkwLkDHpi = cellCoords.y * GvMWWcmtPsNQiQWsUuzTdoThZVAQRaVb;
            int rUJNWvFSkdYdHmIPcNSdDFbWvyONBSus = cellCoords.z * xFihpibrdlTYZdhwrjUUyrdqYaXtydGD;

            return (uint)math.abs(GZDydjjxNsHIIiZjPNjZvEvPthIOMlAj ^ ssHuqmejLkHcZNKbdoaTbpupkwLkDHpi ^ rUJNWvFSkdYdHmIPcNSdDFbWvyONBSus);
        }

        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private static int3 poRBhbJWXilLsFKspjMSyonkEBPkrmyn(in float3 itemPosition, in float cellSize)
        {
            return (int3)math.floor(itemPosition / cellSize);
        }

        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private static int nVdmojPflRyZXvZhkkTMuYCybmCBsSDj(uint hashVal, in int hashTableSize)
        {
            return (int)((uint)hashVal % (uint)hashTableSize);
        }

        
        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private static JobHandle LCMvisBomlAJzTFHCSSCTNOsItbeJiDx(NativeArray<int> spatialLookUpItemIndices, NativeArray<int> spatialLookUpCellKeys, JobHandle dependsOn = default)
        {
            int xDQdlOvLMIMCiPUltUwfDgLvGWYIYPBl = spatialLookUpItemIndices.Length;

            int KAvFJZvUZpDbSKXnWHSvYRrvPpNOXwhl = math.ceilpow2(xDQdlOvLMIMCiPUltUwfDgLvGWYIYPBl) / 2;
            int kPerXCkKnhlNZWsNtmnifsQJgPuccGKx = (int)math.log2(KAvFJZvUZpDbSKXnWHSvYRrvPpNOXwhl * 2);

            JobHandle KBkgjJkNQAyiqSkHJjwlkmTVoGCLIZTT = dependsOn;
            int XratuFKLtBKsmXmYfqjGtgMgAKgYHLjV;
            int xzXgPQohzKsPwVChbaRxEgWBJXokKSFm;
            for (int WklkDYvhMouHTSDkFRDimAjLLvMVqOIG = 0; WklkDYvhMouHTSDkFRDimAjLLvMVqOIG < kPerXCkKnhlNZWsNtmnifsQJgPuccGKx; WklkDYvhMouHTSDkFRDimAjLLvMVqOIG++)
            {
                for (int CEXvQtvTdqciRyZrRPOceChFJksIhtMr = 0; CEXvQtvTdqciRyZrRPOceChFJksIhtMr < WklkDYvhMouHTSDkFRDimAjLLvMVqOIG + 1; CEXvQtvTdqciRyZrRPOceChFJksIhtMr++)
                {
                    XratuFKLtBKsmXmYfqjGtgMgAKgYHLjV = 1 << (WklkDYvhMouHTSDkFRDimAjLLvMVqOIG - CEXvQtvTdqciRyZrRPOceChFJksIhtMr);
                    xzXgPQohzKsPwVChbaRxEgWBJXokKSFm = (2 * XratuFKLtBKsmXmYfqjGtgMgAKgYHLjV) - 1;

                    var WXTUKdEAUZgwkYrsyQMIhbdRNouiupTF = new lyOWRvJjyFUJtSvHXPKFSnclnMattfjj
                    {
                        GroupWidth = (uint)XratuFKLtBKsmXmYfqjGtgMgAKgYHLjV,
                        GroupHeight = (uint)xzXgPQohzKsPwVChbaRxEgWBJXokKSFm,
                        StepIndex = (uint)CEXvQtvTdqciRyZrRPOceChFJksIhtMr,
                        SpatialLookUpItemIndices = spatialLookUpItemIndices,
                        SpatialLookUpCellKeys = spatialLookUpCellKeys,
                    };

                    KBkgjJkNQAyiqSkHJjwlkmTVoGCLIZTT = WXTUKdEAUZgwkYrsyQMIhbdRNouiupTF.Schedule(KAvFJZvUZpDbSKXnWHSvYRrvPpNOXwhl, 64, KBkgjJkNQAyiqSkHJjwlkmTVoGCLIZTT);
                }
            }

            return KBkgjJkNQAyiqSkHJjwlkmTVoGCLIZTT;
        }

        [BurstCompile]
        private struct NwdprJNfXtbfNkzzHmPCGbBgtCZFrfer : IJobParallelFor
        {
            [ReadOnly]
            public float CellSize;

            [ReadOnly]
            public NativeArray<float3> ParticlesPosition;
            public NativeArray<int> SpatialLookUpItemIndices;
            public NativeArray<int> SpatialLookUpCellKeys;
            [WriteOnly]
            public NativeArray<int> StartIndices;

            public void Execute(int i)
            {
                var JkVPKPdiBpWnbnUrhOBWNWsVPkYgzhQc = ParticlesPosition[i];

                int3 dxVahEHlBHwjgtQZaRmKLIQcimVJqrid = poRBhbJWXilLsFKspjMSyonkEBPkrmyn(JkVPKPdiBpWnbnUrhOBWNWsVPkYgzhQc, CellSize);
                int POTEpgrQKVjzBFGLYnMohZHkhuPBMeYS = nVdmojPflRyZXvZhkkTMuYCybmCBsSDj(SbrfOgHZRzveNUMrRmvqrfnvStlTOFnE(dxVahEHlBHwjgtQZaRmKLIQcimVJqrid), SpatialLookUpItemIndices.Length);

                SpatialLookUpItemIndices[i] = i;
                SpatialLookUpCellKeys[i] = POTEpgrQKVjzBFGLYnMohZHkhuPBMeYS;

                StartIndices[i] = int.MaxValue;
            }
        }

        [BurstCompile]
        private struct lyOWRvJjyFUJtSvHXPKFSnclnMattfjj : IJobParallelFor
        {
            [ReadOnly]
            public uint GroupWidth;
            [ReadOnly]
            public uint GroupHeight;
            [ReadOnly]
            public uint StepIndex;

            [NativeDisableParallelForRestriction]
            public NativeArray<int> SpatialLookUpItemIndices;
            [NativeDisableParallelForRestriction]
            public NativeArray<int> SpatialLookUpCellKeys;

            public void Execute(int id)
            {
                uint sQGJKPxDyTPQsMjSABwqcXszpBuJybuL = (uint)id;
                uint XWBzqHPrlSdqOxKMsBmBoDJuPtvqvyYu = sQGJKPxDyTPQsMjSABwqcXszpBuJybuL & (GroupWidth - 1);

                uint SBsgQVcmAXxXpCaQFqzDWYiBsMuBPZfm = XWBzqHPrlSdqOxKMsBmBoDJuPtvqvyYu + ((GroupHeight + 1) * (sQGJKPxDyTPQsMjSABwqcXszpBuJybuL / GroupWidth));
                uint WZVehnKvonguXrNkCjUdHBtRtabfsHpI = SBsgQVcmAXxXpCaQFqzDWYiBsMuBPZfm + (StepIndex == 0 ? GroupHeight - (2 * XWBzqHPrlSdqOxKMsBmBoDJuPtvqvyYu) : (GroupHeight + 1) / 2);

                
                if (WZVehnKvonguXrNkCjUdHBtRtabfsHpI >= SpatialLookUpItemIndices.Length)
                {
                    return;
                }

                int uvbeiLJhENhojmbSdXrHNzbtlIsVpPSj = (int)SBsgQVcmAXxXpCaQFqzDWYiBsMuBPZfm;
                int qpxNgwSWvpHwppFHzbawWSzOmauPPKpE = (int)WZVehnKvonguXrNkCjUdHBtRtabfsHpI;

                int LnRJxCUQgDgXMUnOlgBxAHnCRAnZQGFy = SpatialLookUpCellKeys[(int)SBsgQVcmAXxXpCaQFqzDWYiBsMuBPZfm];
                int PXMrfbLrjbZyToweXPSfOXMztxmWEOIz = SpatialLookUpCellKeys[(int)WZVehnKvonguXrNkCjUdHBtRtabfsHpI];
                int CNSgoHJtJxDOZkqGbBhiyWAGMIfFEEVO = SpatialLookUpItemIndices[(int)SBsgQVcmAXxXpCaQFqzDWYiBsMuBPZfm];
                int TjofBIerlnZducGykKcfJITrDtrkUwpy = SpatialLookUpItemIndices[(int)WZVehnKvonguXrNkCjUdHBtRtabfsHpI];

                
                if (LnRJxCUQgDgXMUnOlgBxAHnCRAnZQGFy > PXMrfbLrjbZyToweXPSfOXMztxmWEOIz)
                {
                    SpatialLookUpCellKeys[uvbeiLJhENhojmbSdXrHNzbtlIsVpPSj] = PXMrfbLrjbZyToweXPSfOXMztxmWEOIz;
                    SpatialLookUpCellKeys[qpxNgwSWvpHwppFHzbawWSzOmauPPKpE] = LnRJxCUQgDgXMUnOlgBxAHnCRAnZQGFy;

                    SpatialLookUpItemIndices[uvbeiLJhENhojmbSdXrHNzbtlIsVpPSj] = TjofBIerlnZducGykKcfJITrDtrkUwpy;
                    SpatialLookUpItemIndices[qpxNgwSWvpHwppFHzbawWSzOmauPPKpE] = CNSgoHJtJxDOZkqGbBhiyWAGMIfFEEVO;
                }
            }
        }

        [BurstCompile]
        private struct RbhtdxjFuEpjlbNnQuFxvcCcIpXYZVnj : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<int> SpatialLookUpItemIndices;
            [ReadOnly]
            public NativeArray<int> SpatialLookUpCellKeys;

            [WriteOnly]
            [NativeDisableParallelForRestriction]
            public NativeArray<int> StartIndices;

            public void Execute(int i)
            {
                var PmIsBRpVNFpvcIhvWXUmDTooCUtMSNqs = SpatialLookUpCellKeys[i];
                var TrFthiEAUSEQRWVIiNkAyPailvKGfroN = i == 0 ? int.MaxValue : SpatialLookUpCellKeys[i - 1];

                if (PmIsBRpVNFpvcIhvWXUmDTooCUtMSNqs != TrFthiEAUSEQRWVIiNkAyPailvKGfroN)
                {
                    StartIndices[PmIsBRpVNFpvcIhvWXUmDTooCUtMSNqs] = i;
                }
            }
        }

        [BurstCompile]
        private struct NWRHWNkWeuEfyyqaXXpygKCKbPyXMsjP : IJobParallelFor
        {
            [ReadOnly]
            public float ParticleRadius;
            [ReadOnly]
            public float GridCellSize;

            [ReadOnly]
            public NativeArray<float3> ParticlesPredictedPosition;

            [ReadOnly]
            public NativeArray<int> SpatialLookUpItemIndices;
            [ReadOnly]
            public NativeArray<int> SpatialLookUpCellKeys;
            [ReadOnly]
            public NativeArray<int> StartIndices;

            [NativeDisableUnsafePtrRestriction]
            [NativeDisableContainerSafetyRestriction]
            public NativeReference<int> ContactPairsCount;

            [NativeDisableParallelForRestriction]
            [WriteOnly]
            public NativeArray<ContactPair> ContactPairs;

            public void Execute(int particleAIndex)
            {
                if (SpatialLookUpItemIndices.Length == 0)
                {
                    return;
                }

                var fGrLAMUuaaDBETByGpuzEGxpeOsUbHMI = ParticlesPredictedPosition[particleAIndex];

                int3 fQEKndFKGKsykcAlaFezsycFnxJzpQUV = poRBhbJWXilLsFKspjMSyonkEBPkrmyn(fGrLAMUuaaDBETByGpuzEGxpeOsUbHMI, GridCellSize);
                int HaIvWJRnVbQKDhSFmOAKAgMNDNNTxPlq;

                
                foreach (int3 offset in oWBBqJZKvCnSaNPhUGLCmKQEGbbcpfsa)
                {
                    int3 vdWySyYEYVpHdGAsJmLcXiiyUZCkjQWM = fQEKndFKGKsykcAlaFezsycFnxJzpQUV + offset;
                    int DhHTLGvaBpBfpzTQlSzcQxjJCjPxcAav = nVdmojPflRyZXvZhkkTMuYCybmCBsSDj(SbrfOgHZRzveNUMrRmvqrfnvStlTOFnE(vdWySyYEYVpHdGAsJmLcXiiyUZCkjQWM), SpatialLookUpItemIndices.Length);
                    int qTsBCPdZAWtpSsFQOksIPpKIuAgglQrf = StartIndices[DhHTLGvaBpBfpzTQlSzcQxjJCjPxcAav];

                    for (int MwJoNocQkiOhfpwSMomdycFYkXvvWcxj = qTsBCPdZAWtpSsFQOksIPpKIuAgglQrf; MwJoNocQkiOhfpwSMomdycFYkXvvWcxj < SpatialLookUpItemIndices.Length; MwJoNocQkiOhfpwSMomdycFYkXvvWcxj++)
                    {
                        HaIvWJRnVbQKDhSFmOAKAgMNDNNTxPlq = SpatialLookUpCellKeys[MwJoNocQkiOhfpwSMomdycFYkXvvWcxj];

                        
                        if (HaIvWJRnVbQKDhSFmOAKAgMNDNNTxPlq != DhHTLGvaBpBfpzTQlSzcQxjJCjPxcAav)
                        {
                            break;
                        }

                        var jfuhVvNXwSNiFOszrgXHqpABehDXCHPM = SpatialLookUpItemIndices[MwJoNocQkiOhfpwSMomdycFYkXvvWcxj];

                        if (Unity.Burst.CompilerServices.Hint.Unlikely(particleAIndex == jfuhVvNXwSNiFOszrgXHqpABehDXCHPM))
                        {
                            continue;
                        }

                        var beIhkaVJIYkHXGjQIbnqRQqgKwsfZRTl = ParticlesPredictedPosition[jfuhVvNXwSNiFOszrgXHqpABehDXCHPM];

                        float3 NhtivqFGhqycislLbogTQJWVllkxwfDX = beIhkaVJIYkHXGjQIbnqRQqgKwsfZRTl - fGrLAMUuaaDBETByGpuzEGxpeOsUbHMI;
                        float YFYhJcdVQjLpZOeZYLUmFDVvxaVkVkKB = math.lengthsq(NhtivqFGhqycislLbogTQJWVllkxwfDX);
                        if (YFYhJcdVQjLpZOeZYLUmFDVvxaVkVkKB < math.pow(2 * ParticleRadius, 2))
                        {
                            int eaTgLXmwDLALIXUyvhQRuYgpIrwIMRlG = math.min(ContactPairsCount.InterlockedAdd(1) - 1, ContactPairs.Length - 1);

                            
                            ContactPair ZuzMOCSEJAeAJcfAXFCSJSavSqAWcyGh = new ContactPair
                            {
                                Particle = particleAIndex,
                                OtherParticle = jfuhVvNXwSNiFOszrgXHqpABehDXCHPM,
                                Separation = math.sqrt(YFYhJcdVQjLpZOeZYLUmFDVvxaVkVkKB),
                                ContactNormal = math.normalize(NhtivqFGhqycislLbogTQJWVllkxwfDX),
                            };

                            ContactPairs[eaTgLXmwDLALIXUyvhQRuYgpIrwIMRlG] = ZuzMOCSEJAeAJcfAXFCSJSavSqAWcyGh;
                        }
                    }
                }
            }
        }
    }
}
