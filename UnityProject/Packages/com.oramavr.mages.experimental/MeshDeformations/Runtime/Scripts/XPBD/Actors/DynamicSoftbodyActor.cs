namespace MAGES.MeshDeformations
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MAGES.MeshDeformations.Utilities;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Jobs;
    using Unity.Mathematics;
    using UnityEditor;
    using UnityEngine;
    using Debug = UnityEngine.Debug;

    /// <summary>
    /// A dynamic softbody actor.
    /// </summary>
    /// <remarks>The dynamic softbody, creates the render mesh dyanamically based on the current volume constraints of the simulation mesh.<br\>
    /// This allows us to cut the softbody based on forces and the render mesh to be automatically updated.
    /// </remarks>
    [RequireComponent(typeof(MeshFilter))]
    public class DynamicSoftbodyActor : BasePhysicsActor
    {
        private float vqYRARNEENgfEOlJmuZvNdDIbxYPusTs = 0.01f;
        private Mesh IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK;

        private float4x4 UkRjRxYGOtwGbecJiWnUmGphlFZZmzNg;
        private float4x4 BPTAdBSQMkeryqitdCrXorQfGNstLVJa;

        
        private UnsafeAtomicCounter32 vshbsmUQUkdIlduBcqgttBUdXjBaDJGO;
        private NativeArray<float3> gmvsOoSNlzvldXnFKbascWeOYlHGQDJj;
        private NativeArray<float2> gPqKLmKQVBdWfvxYAGmVdGewjlwfDqqX;
        [SerializeField]
        private float2[] particlesUVs;
        private NativeArray<float2> naqyjxEeJeBwvwAJGMkTUhtSXMNsokNV;

        
        [SerializeField]
        private int[] volumeConstraintsNeighborsSerialized = new int[0];
        private NativeArray<int> mNYomtnKnkQozVWHzuBEWMtzkgLkagUu;
        [SerializeField]
        private int[] volumeConstraintsToDistanceConstraintsMapSerialized = new int[0]; 
        private NativeArray<int> ailGOIZpwKQqqnvaaPIQRPlGUsnNhgxN;
        private NativeArray<bool> cyszmULnGziSnFsyxccoCTfbnmefpwhD;
        private NativeArray<int> ZnVKXkVsqGEZtclPEvDLfomCFgmdpyXt; 

        
        private NativeArray<int> QlFngOMddQelgUFvRiDVsuqnokGsjbbJ;
        private NativeArray<int> SRBTLwtxPhkZIeaGzKJVUurjiROqlotk;

        
        private NativeArray<int> WeieAgoBIRlepuzFFwFcGAmQNWbjBIkb; 
        private NativeArray<int> TzjttcWQKxXOBfvzCQMxFpxNKwEelrlb; 

        private kNfsIJiAeHdUsHBnINcLfwfDHElTSnIa gylEmSZdIEXLRCVwCFVmFRheZUaANiux;

        [SerializeField]
        private uint layer = 1;
        [SerializeField]
        private bool smoothShading = true;
        [SerializeField]
        private bool generateFacesInSubMeshBoundaries = false;

        private bool ZQwACbOLkFjDEUifuekasahCzexbGTVn = false;
        private bool nmAUFdYsRQrMVtMRVbBKJAAFZczlAZtr = false;

        /// <inheritdoc/>
        public override SimulationMesh SharedSimulationMesh
        {
            get
            {
                return sharedSimulationMesh;
            }

            set
            {
                if (sharedSimulationMesh == value)
                {
                    return;
                }

                if (MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj != null && MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld)
                {
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RemovedFromPhysicsWorld.AddListener(() =>
                    {
                        Destroy(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj);
                        MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj = null;
                        OJxTxcNNqJGpYsbmMgQncWNIZRbrZRsS();
                        EPpFXfFKinGVlMJXZvxnXTgmRtIaEVJa();
                        particlesUVs = new float2[SharedSimulationMesh.Particles.Length];
                        AddToPhysicsWorld();
                    });
                    sharedSimulationMesh = value;
                    RemoveFromPhysicsWorld();
                }
                else
                {
                    bool zZnkGVmpKlPgLfFFRlriPhaIRzrWVCHa = sharedSimulationMesh == null;
                    sharedSimulationMesh = value;

                    OJxTxcNNqJGpYsbmMgQncWNIZRbrZRsS();
                    EPpFXfFKinGVlMJXZvxnXTgmRtIaEVJa();
                    particlesUVs = new float2[SharedSimulationMesh.Particles.Length];

#if UNITY_EDITOR
                    
                    yApxMRTxBietZtaqZgPMTPKmIijKuwmZ();
#endif

                    if (zZnkGVmpKlPgLfFFRlriPhaIRzrWVCHa && enabled && Application.isPlaying)
                    {
                        AddToPhysicsWorld();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the layer used for collision filtering.
        /// </summary>
        public uint Layer
        {
            get
            {
                return layer;
            }

            set
            {
                if (layer == value)
                {
                    return;
                }

                layer = value;
                if (MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj != null && MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld)
                {
                    uint bOwzYcqLxoBfgmfmeJDsOYunAfwFTNNh = PhysicsWorld.CreatePhase(Layer, PhaseOptions.None);
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.PhysicsWorld.UpdatePhase(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj, bOwzYcqLxoBfgmfmeJDsOYunAfwFTNNh);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to update the kinematic particles position when this actor's transform is updated.
        /// </summary>
        [field: SerializeField]
        public bool UpdateKinematicParticles { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to generate smooth or flat shading for the render mesh.
        /// </summary>
        public bool SmoothShading
        {
            get
            {
                return smoothShading;
            }

            set
            {
                if (smoothShading == value)
                {
                    return;
                }

                smoothShading = value;

#if UNITY_EDITOR
                
                yApxMRTxBietZtaqZgPMTPKmIijKuwmZ();
#endif
                ZQwACbOLkFjDEUifuekasahCzexbGTVn = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to generate faces (triangles) in the boundaries of the physics material.
        /// </summary>
        public bool GenerateFacesInPhysicsMaterialBoundaries
        {
            get
            {
                return generateFacesInSubMeshBoundaries;
            }

            set
            {
                if (generateFacesInSubMeshBoundaries == value)
                {
                    return;
                }

                generateFacesInSubMeshBoundaries = value;

                OJxTxcNNqJGpYsbmMgQncWNIZRbrZRsS();
                EPpFXfFKinGVlMJXZvxnXTgmRtIaEVJa();

#if UNITY_EDITOR
                
                yApxMRTxBietZtaqZgPMTPKmIijKuwmZ();
#endif

                nmAUFdYsRQrMVtMRVbBKJAAFZczlAZtr = true;
                ZQwACbOLkFjDEUifuekasahCzexbGTVn = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether should remove residual distance constraints after a tear.
        /// </summary>
        [field: SerializeField]
        public bool RemoveResidualDistanceConstraints
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether after the simulation mesh has been modified, the volume constraints with limited number of neighbors should be removed.
        /// </summary>
        [field: SerializeField]
        public bool RemoveVolumeConstraintsWithLimitedNeighbors
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum number of neighbors before volume constraints removal.
        /// </summary>
        [field: SerializeField]
        public int MinNeighborsBeforeVolumeConstraintsRemoval
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public override void AddToPhysicsWorld()
        {
            if (SharedSimulationMesh == null)
            {
                UnityEngine.Debug.LogError("SharedSimulationMesh is not assigned. Softbody actor will not be simulated.", this);
                return;
            }

            if (PhysicsWorld == null)
            {
                UnityEngine.Debug.LogError("PhysicsWorld is not assigned. Softbody actor will not be simulated.", this);
                return;
            }

            
            UmsxBhiAByMTfiqkJZIhEzfAnIqTJTvv();

            UkRjRxYGOtwGbecJiWnUmGphlFZZmzNg = transform.worldToLocalMatrix;
            ZQwACbOLkFjDEUifuekasahCzexbGTVn = true;
            PhysicsWorld.AddSimulationMesh(SimulationMesh, transform.localToWorldMatrix, kwTnoUNwUPBElUHMWdoNecvWOCmSBsYT());

            base.AddToPhysicsWorld();
        }

        /// <inheritdoc/>
        public override void RemoveFromPhysicsWorld()
        {
            base.RemoveFromPhysicsWorld();

            
            PhysicsWorld?.RemoveSimulationMesh(SimulationMesh);

            
            fGEQJiKhwAwzIUjrbeELbWfGBLnbByfC();

            
            Destroy(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj);
            MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj = null;
        }

        /// <summary>
        /// Cut the mesh with a disc by splitting intersected tetrahedra. Only allowed to be called from the PhysicsUpdate Event.
        /// </summary>
        /// <param name="cuttingDisc">The disc based on which to cut.</param>
        public void CutSplit(Disc cuttingDisc)
        {
            if (!MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld)
            {
                return;
            }

            
            
            
            
            
            
            
            
            
            
            Dictionary<(int, int), float> bvwbUXtZYVeICCTWzWBOPFXJIiSRpdQY = CutUtilities.FindIntersections(cuttingDisc, MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj);

            if (bvwbUXtZYVeICCTWzWBOPFXJIiSRpdQY.Count == 0)
            {
                Debug.Log("No intersections");
                return;
            }

            
            Dictionary<(int, int, bool), int> tvfqoxCrAmTgzfwkqKiptfeoPrXaFTEK = CreateParticlesForIntersections(bvwbUXtZYVeICCTWzWBOPFXJIiSRpdQY);

            foreach (var k in tvfqoxCrAmTgzfwkqKiptfeoPrXaFTEK.Keys)
            {
                int axiGvnRfRitUWbfjBqCpTHqfuxZnXdUQ = tvfqoxCrAmTgzfwkqKiptfeoPrXaFTEK[(k.Item1, k.Item2, true)];

                Utilities.Debug.DrawPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[axiGvnRfRitUWbfjBqCpTHqfuxZnXdUQ], 10f, 0.01f);
            }

            RemoveIntersectedDistanceConstraints();

            RemoveIntersectedAndSplitVolumeConstraints();

            
            int WcjBzpoQCBNDMQPukryAPOWNwmYTigSC = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.Count * 4 * 3;
            gmvsOoSNlzvldXnFKbascWeOYlHGQDJj.ResizeArray(WcjBzpoQCBNDMQPukryAPOWNwmYTigSC, Allocator.Persistent);
            gPqKLmKQVBdWfvxYAGmVdGewjlwfDqqX.ResizeArray(WcjBzpoQCBNDMQPukryAPOWNwmYTigSC, Allocator.Persistent);
            WeieAgoBIRlepuzFFwFcGAmQNWbjBIkb.ResizeArray(WcjBzpoQCBNDMQPukryAPOWNwmYTigSC, Allocator.Persistent);

            
            for (int AfVCgdHqcYsJfVJcQZTWOgQOLahcYbwV = 0; AfVCgdHqcYsJfVJcQZTWOgQOLahcYbwV < SharedSimulationMesh.PhysicsMaterials.Length; AfVCgdHqcYsJfVJcQZTWOgQOLahcYbwV++)
            {
                int pjJuRpswJoCuRpxbdfoEBFfzrMOTwZgO = WcjBzpoQCBNDMQPukryAPOWNwmYTigSC;

                unsafe
                {
                    gylEmSZdIEXLRCVwCFVmFRheZUaANiux.Datas[AfVCgdHqcYsJfVJcQZTWOgQOLahcYbwV].Resize(pjJuRpswJoCuRpxbdfoEBFfzrMOTwZgO);
                }
            }

            if (SmoothShading)
            {
                int TlKIZvnkuTOcOdXdlCDAOqDUrviLsFCS = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Count;
                TzjttcWQKxXOBfvzCQMxFpxNKwEelrlb.ResizeArray(TlKIZvnkuTOcOdXdlCDAOqDUrviLsFCS, Allocator.Persistent);
                QlFngOMddQelgUFvRiDVsuqnokGsjbbJ.ResizeArray(TlKIZvnkuTOcOdXdlCDAOqDUrviLsFCS, Allocator.Persistent);
                SRBTLwtxPhkZIeaGzKJVUurjiROqlotk.ResizeArray(TlKIZvnkuTOcOdXdlCDAOqDUrviLsFCS, Allocator.Persistent);
            }

            
            ZQwACbOLkFjDEUifuekasahCzexbGTVn = true;

            bool IsEdgeIntersected(int p0, int p1)
            {
                return bvwbUXtZYVeICCTWzWBOPFXJIiSRpdQY.ContainsKey(p0 < p1 ? (p0, p1) : (p1, p0));
            }

            int GetParticleIndexForIntersection(int p0, int p1, bool side)
            {
                if (tvfqoxCrAmTgzfwkqKiptfeoPrXaFTEK.ContainsKey(p0 < p1 ? (p0, p1, side) : (p1, p0, side)))
                {
                    return tvfqoxCrAmTgzfwkqKiptfeoPrXaFTEK[p0 < p1 ? (p0, p1, side) : (p1, p0, side)];
                }

                Debug.LogError("Particle index not found for intersection");
                return 0;
            }

            Dictionary<(int, int, bool), int> CreateParticlesForIntersections(in Dictionary<(int, int), float> intersections)
            {
                Dictionary<(int, int, bool), int> lhQzUSDiIYaewhpzUngdPndTyjKiTEJI = new Dictionary<(int, int, bool), int>(intersections.Count * 2);

                bool[] dMEwRzMdIeAzTuwiDtYVTuSmUHAvDaoE = new bool[intersections.Count * 2];
                bool[] msxfRUseULNcEXAkYRoKlePFqpuWnkLz = new bool[intersections.Count * 2];
                int[] zGusAPxUSBzaPNBOxlWSoiGAnJOzOEBj = new int[intersections.Count * 2];
                float3[] RYMGuzDQrImMFcdgSatMcpylqgyRiGYh = new float3[intersections.Count * 2];
                float3[] WraXolDlfVvKxmjWbAmzvtFsHbXstXqh = new float3[intersections.Count * 2];
                float3[] wdSYvxVakRviNqJBjAxZfgqAbKmQEHej = new float3[intersections.Count * 2];
                float[] AHMvEBNKqLQNXedEQEfFdheHMprONiQk = new float[intersections.Count * 2];
                uint[] QNRjpcJbeqOFjqLghOKwdbbjHeuEeEYR = new uint[intersections.Count * 2];

                int nbGUacxHFwsdXyTKPCPQdYffyCTGceDH = 0;
                foreach (var key in intersections.Keys)
                {
                    var KCozLsbwwybSmuqsKXUJwfTebPFbjxcq = key.Item1;
                    var MRlvAFuRTGrUezsEAKEqZvSUHjMoBvyA = key.Item2;

                    var IBJUAumzEJehVhFWeMcGDRMSdAHvbtEO = intersections[key];

                    var GEbrSpDleWebPdbTHXbTtLMzRFhScmgR = math.lerp(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition[KCozLsbwwybSmuqsKXUJwfTebPFbjxcq], MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition[MRlvAFuRTGrUezsEAKEqZvSUHjMoBvyA], IBJUAumzEJehVhFWeMcGDRMSdAHvbtEO);
                    var ODpgEfrLRJhDLHyJKrfiEXASsnUvlyJJ = math.lerp(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[KCozLsbwwybSmuqsKXUJwfTebPFbjxcq], MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[MRlvAFuRTGrUezsEAKEqZvSUHjMoBvyA], IBJUAumzEJehVhFWeMcGDRMSdAHvbtEO);

                    
                    dMEwRzMdIeAzTuwiDtYVTuSmUHAvDaoE[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH] = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Enabled[KCozLsbwwybSmuqsKXUJwfTebPFbjxcq] && MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Enabled[MRlvAFuRTGrUezsEAKEqZvSUHjMoBvyA];
                    msxfRUseULNcEXAkYRoKlePFqpuWnkLz[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH] = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Breakable[KCozLsbwwybSmuqsKXUJwfTebPFbjxcq] && MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Enabled[MRlvAFuRTGrUezsEAKEqZvSUHjMoBvyA];
                    zGusAPxUSBzaPNBOxlWSoiGAnJOzOEBj[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH] = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.PhysicsMaterialIndex[KCozLsbwwybSmuqsKXUJwfTebPFbjxcq];
                    RYMGuzDQrImMFcdgSatMcpylqgyRiGYh[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH] = GEbrSpDleWebPdbTHXbTtLMzRFhScmgR;
                    WraXolDlfVvKxmjWbAmzvtFsHbXstXqh[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH] = ODpgEfrLRJhDLHyJKrfiEXASsnUvlyJJ;
                    wdSYvxVakRviNqJBjAxZfgqAbKmQEHej[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH] = math.lerp(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Velocity[KCozLsbwwybSmuqsKXUJwfTebPFbjxcq], MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Velocity[MRlvAFuRTGrUezsEAKEqZvSUHjMoBvyA], IBJUAumzEJehVhFWeMcGDRMSdAHvbtEO);

                    
                    AHMvEBNKqLQNXedEQEfFdheHMprONiQk[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH] = 1f;

                    QNRjpcJbeqOFjqLghOKwdbbjHeuEeEYR[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH] = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Phase[KCozLsbwwybSmuqsKXUJwfTebPFbjxcq];

                    
                    dMEwRzMdIeAzTuwiDtYVTuSmUHAvDaoE[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH + 1] = dMEwRzMdIeAzTuwiDtYVTuSmUHAvDaoE[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH];
                    msxfRUseULNcEXAkYRoKlePFqpuWnkLz[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH + 1] = dMEwRzMdIeAzTuwiDtYVTuSmUHAvDaoE[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH];
                    RYMGuzDQrImMFcdgSatMcpylqgyRiGYh[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH + 1] = RYMGuzDQrImMFcdgSatMcpylqgyRiGYh[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH];
                    WraXolDlfVvKxmjWbAmzvtFsHbXstXqh[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH + 1] = WraXolDlfVvKxmjWbAmzvtFsHbXstXqh[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH];
                    wdSYvxVakRviNqJBjAxZfgqAbKmQEHej[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH + 1] = wdSYvxVakRviNqJBjAxZfgqAbKmQEHej[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH];

                    
                    AHMvEBNKqLQNXedEQEfFdheHMprONiQk[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH + 1] = AHMvEBNKqLQNXedEQEfFdheHMprONiQk[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH];

                    QNRjpcJbeqOFjqLghOKwdbbjHeuEeEYR[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH + 1] = QNRjpcJbeqOFjqLghOKwdbbjHeuEeEYR[nbGUacxHFwsdXyTKPCPQdYffyCTGceDH];

                    lhQzUSDiIYaewhpzUngdPndTyjKiTEJI[(KCozLsbwwybSmuqsKXUJwfTebPFbjxcq, MRlvAFuRTGrUezsEAKEqZvSUHjMoBvyA, true)] = nbGUacxHFwsdXyTKPCPQdYffyCTGceDH;
                    lhQzUSDiIYaewhpzUngdPndTyjKiTEJI[(KCozLsbwwybSmuqsKXUJwfTebPFbjxcq, MRlvAFuRTGrUezsEAKEqZvSUHjMoBvyA, false)] = nbGUacxHFwsdXyTKPCPQdYffyCTGceDH + 1;

                    nbGUacxHFwsdXyTKPCPQdYffyCTGceDH += 2;
                }

                
                int LcDuYRWFcgGSewVrENkNnuMprQDPItTm = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.AddRuntimeParticles(dMEwRzMdIeAzTuwiDtYVTuSmUHAvDaoE, msxfRUseULNcEXAkYRoKlePFqpuWnkLz, zGusAPxUSBzaPNBOxlWSoiGAnJOzOEBj, RYMGuzDQrImMFcdgSatMcpylqgyRiGYh, WraXolDlfVvKxmjWbAmzvtFsHbXstXqh, wdSYvxVakRviNqJBjAxZfgqAbKmQEHej, AHMvEBNKqLQNXedEQEfFdheHMprONiQk, QNRjpcJbeqOFjqLghOKwdbbjHeuEeEYR);

                
                foreach (var key in intersections.Keys)
                {
                    lhQzUSDiIYaewhpzUngdPndTyjKiTEJI[(key.Item1, key.Item2, true)] += LcDuYRWFcgGSewVrENkNnuMprQDPItTm;
                    lhQzUSDiIYaewhpzUngdPndTyjKiTEJI[(key.Item1, key.Item2, false)] += LcDuYRWFcgGSewVrENkNnuMprQDPItTm;
                }

                return lhQzUSDiIYaewhpzUngdPndTyjKiTEJI;
            }

            void RemoveIntersectedDistanceConstraints()
            {
                
                for (int pwKfhiWumxHPTWHzMIiMFviDspRJalUk = 0; pwKfhiWumxHPTWHzMIiMFviDspRJalUk < MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeDistanceConstraintData.Count; pwKfhiWumxHPTWHzMIiMFviDspRJalUk++)
                {
                    int mbPLiGiGLtqPDrwHnUfBkHkOczLwwNHJ = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeDistanceConstraintData.ParticleA[pwKfhiWumxHPTWHzMIiMFviDspRJalUk];
                    int ewxuHstWPzlNoCXrwKLfsRHLPVcLGWGZ = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeDistanceConstraintData.ParticleB[pwKfhiWumxHPTWHzMIiMFviDspRJalUk];

                    
                    if (IsEdgeIntersected(mbPLiGiGLtqPDrwHnUfBkHkOczLwwNHJ, ewxuHstWPzlNoCXrwKLfsRHLPVcLGWGZ))
                    {
                        MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeDistanceConstraintData.Enabled[pwKfhiWumxHPTWHzMIiMFviDspRJalUk] = false;
                    }
                }
            }

            void RemoveIntersectedAndSplitVolumeConstraints()
            {
                List<int> FIBUpfaURUhyAmeVNOybYuroshvQdfKB = new List<int>(); 

                List<bool> jkrmjtfpOMwkUsOGcFBHXuNolFYnYzwr = new List<bool>();
                List<bool> jgiVKUJTAihYhcvMJabIIjwJErVpfqVy = new List<bool>();
                List<int> mQvioNJoLMjStPCZGiWmvIecxuBeJqYJ = new List<int>();
                List<int> rhIBZUFyNQwVCmGnXoVmFLBRIDKAzHNd = new List<int>();
                List<int> gkWuTpLZGlyOFGyRsKuyjcdEPQzpegyn = new List<int>();
                List<int> AxsnNfCOCQqpdcGhrzgBasOEqvWOgomJ = new List<int>();
                List<int> NEKlvvkOuLUIifuETmCjrsHeDpeAQMVU = new List<int>();
                List<float> vcXNpUFTwIoGFIFlcJEtHmxWXGgNjjCX = new List<float>();

                HashSet<(int, int)> pkanolhUdvBUVNmgOikOeJXSvDeujAdR = new HashSet<(int, int)>();
                List<bool> IgwgxlhJVwaFYoZMeUuFaalpNkfpupLz = new List<bool>();
                List<bool> lQRXcmLEELmKJMYlwPfbtUTlVbawrMoC = new List<bool>();
                List<int> PbhGOKDnPWClGWLVWxavDyhpDbecYLmM = new List<int>();
                List<int> bzDTjloiyzdscSnzKwWJqKJUliDYMfRC = new List<int>();
                List<int> GPSfDcIfLdNviJJVEmYuMsLgXhAaqbLM = new List<int>();
                List<float> ejtMeXvcJhbVYDiJuPgSHqqOARjelPIJ = new List<float>();

                HashSet<(int, int)> apkrvtbhuvOyhhITcfYTJVJfXSjkUrYp = new HashSet<(int, int)>();

                
                for (int tCqStDKtwhrWgZLGEcmoIAixspIVMqLD = 0; tCqStDKtwhrWgZLGEcmoIAixspIVMqLD < MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.Count; tCqStDKtwhrWgZLGEcmoIAixspIVMqLD++)
                {
                    var HppccEkxgkondFmqMHfkMBnewEColSMs = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleA[tCqStDKtwhrWgZLGEcmoIAixspIVMqLD];
                    var NsAmZBELNtoqlydVxiFjmppvOCFEnoHl = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleB[tCqStDKtwhrWgZLGEcmoIAixspIVMqLD];
                    var uISMMhkSIpKheJGJnBReJdCkjVWUEcKt = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleC[tCqStDKtwhrWgZLGEcmoIAixspIVMqLD];
                    var bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleD[tCqStDKtwhrWgZLGEcmoIAixspIVMqLD];

                    bool VeWQTLurjwVAmQQSVdLCcVVTLdauPhvt = IsEdgeIntersected(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl);
                    bool UnfVfQoLNWXKvbYdzKquZamYKTbMChhq = IsEdgeIntersected(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt);
                    bool lkLaHmiwfYGODMwjqnNpzDMzGEGdsDDC = IsEdgeIntersected(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt);
                    bool JGxfooHtQwNUcnvfcfghyQlphZaDaVzI = IsEdgeIntersected(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc);
                    bool JQjTHiWepJgcPbzLzOWpSjpGlvvoGqoQ = IsEdgeIntersected(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc);
                    bool GtgFwfDzmDrKXAJQPIweTqPqREnDLtrv = IsEdgeIntersected(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc);

                    
                    if (!(VeWQTLurjwVAmQQSVdLCcVVTLdauPhvt || UnfVfQoLNWXKvbYdzKquZamYKTbMChhq || lkLaHmiwfYGODMwjqnNpzDMzGEGdsDDC || JGxfooHtQwNUcnvfcfghyQlphZaDaVzI || JQjTHiWepJgcPbzLzOWpSjpGlvvoGqoQ || GtgFwfDzmDrKXAJQPIweTqPqREnDLtrv))
                    {
                        continue;
                    }

                    
                    
                    
                    
                    
                    bool FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.Enabled[tCqStDKtwhrWgZLGEcmoIAixspIVMqLD];
                    bool hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.Breakable[tCqStDKtwhrWgZLGEcmoIAixspIVMqLD];
                    int TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.PhysicsMaterialIndex[tCqStDKtwhrWgZLGEcmoIAixspIVMqLD];

                    
                    if (VeWQTLurjwVAmQQSVdLCcVVTLdauPhvt && UnfVfQoLNWXKvbYdzKquZamYKTbMChhq && JGxfooHtQwNUcnvfcfghyQlphZaDaVzI && GtgFwfDzmDrKXAJQPIweTqPqREnDLtrv)
                    {
                        bool gsQxuvXCFLTWOurECGSnRkFZsXZaVMKq = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[HppccEkxgkondFmqMHfkMBnewEColSMs]);

                        
                        {
                            var daaygzsboFaZQaCxSRgouxeKFFvGMPiy = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, gsQxuvXCFLTWOurECGSnRkFZsXZaVMKq);
                            var IjQhrstQOFsNUtwoYwDdPOgtjbDrqNEb = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, gsQxuvXCFLTWOurECGSnRkFZsXZaVMKq);
                            var dwKAxrKmdHtVmCWHqJBkRgSMlbPfHEtl = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, gsQxuvXCFLTWOurECGSnRkFZsXZaVMKq);
                            var lNeXaLUVyMiROjinXDBTiPbgufenzuoO = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, gsQxuvXCFLTWOurECGSnRkFZsXZaVMKq);

                            AddVolumeConstraintsAndDistanceConstraints(daaygzsboFaZQaCxSRgouxeKFFvGMPiy, dwKAxrKmdHtVmCWHqJBkRgSMlbPfHEtl, IjQhrstQOFsNUtwoYwDdPOgtjbDrqNEb, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(dwKAxrKmdHtVmCWHqJBkRgSMlbPfHEtl, lNeXaLUVyMiROjinXDBTiPbgufenzuoO, IjQhrstQOFsNUtwoYwDdPOgtjbDrqNEb, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(IjQhrstQOFsNUtwoYwDdPOgtjbDrqNEb, lNeXaLUVyMiROjinXDBTiPbgufenzuoO, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                        }

                        
                        {
                            var GMntMJNBlMGcQKBUKtIqSzbOOwKavvvo = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, !gsQxuvXCFLTWOurECGSnRkFZsXZaVMKq);
                            var alroqecQvIhXDFtBAicmSgPpqaTeVjpy = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !gsQxuvXCFLTWOurECGSnRkFZsXZaVMKq);
                            var PjDdgYbeZqpFdqvGJOZlfdOYjrMrZuRa = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !gsQxuvXCFLTWOurECGSnRkFZsXZaVMKq);
                            var raleSRukAXKzjFETvNnXRSfWVoiUmffv = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !gsQxuvXCFLTWOurECGSnRkFZsXZaVMKq);

                            AddVolumeConstraintsAndDistanceConstraints(PjDdgYbeZqpFdqvGJOZlfdOYjrMrZuRa, alroqecQvIhXDFtBAicmSgPpqaTeVjpy, raleSRukAXKzjFETvNnXRSfWVoiUmffv, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(GMntMJNBlMGcQKBUKtIqSzbOOwKavvvo, alroqecQvIhXDFtBAicmSgPpqaTeVjpy, PjDdgYbeZqpFdqvGJOZlfdOYjrMrZuRa, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(PjDdgYbeZqpFdqvGJOZlfdOYjrMrZuRa, raleSRukAXKzjFETvNnXRSfWVoiUmffv, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                        }

                        FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                    }
                    else if (UnfVfQoLNWXKvbYdzKquZamYKTbMChhq && lkLaHmiwfYGODMwjqnNpzDMzGEGdsDDC && JGxfooHtQwNUcnvfcfghyQlphZaDaVzI && JQjTHiWepJgcPbzLzOWpSjpGlvvoGqoQ)
                    {
                        bool lDLVJVdKprawxOjrGZGHbKKboGNIQSwp = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[HppccEkxgkondFmqMHfkMBnewEColSMs]);

                        
                        {
                            var OGzrzfELfOgZuVidFINLbQrOTlHDQdgn = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, lDLVJVdKprawxOjrGZGHbKKboGNIQSwp);
                            var OObKdvGBRYRPnhrRsQDpJWnxnfhZuUvv = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, lDLVJVdKprawxOjrGZGHbKKboGNIQSwp);
                            var jrUTiZGZGmDRuWQvmpUsJWTRMDLCQmMF = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, lDLVJVdKprawxOjrGZGHbKKboGNIQSwp);
                            var YVYrKcUqqadupqxOLUJYtsLgaUHHnafl = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, lDLVJVdKprawxOjrGZGHbKKboGNIQSwp);

                            AddVolumeConstraintsAndDistanceConstraints(OGzrzfELfOgZuVidFINLbQrOTlHDQdgn, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, YVYrKcUqqadupqxOLUJYtsLgaUHHnafl, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(OObKdvGBRYRPnhrRsQDpJWnxnfhZuUvv, OGzrzfELfOgZuVidFINLbQrOTlHDQdgn, YVYrKcUqqadupqxOLUJYtsLgaUHHnafl, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(OObKdvGBRYRPnhrRsQDpJWnxnfhZuUvv, YVYrKcUqqadupqxOLUJYtsLgaUHHnafl, jrUTiZGZGmDRuWQvmpUsJWTRMDLCQmMF, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                        }

                        
                        {
                            var rYALMBvBXrKceUOddDePXHueuCNulGRS = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !lDLVJVdKprawxOjrGZGHbKKboGNIQSwp);
                            var LKAtsiYIEZFPMbYZuJXfbgtFwCwWDhpp = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !lDLVJVdKprawxOjrGZGHbKKboGNIQSwp);
                            var SoUYdHVZPlvJmDPezGdnpPvRYAVSUJCU = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !lDLVJVdKprawxOjrGZGHbKKboGNIQSwp);
                            var wbrhNOntPLRhHhejsmLMWSNoQAmBZVNV = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !lDLVJVdKprawxOjrGZGHbKKboGNIQSwp);

                            AddVolumeConstraintsAndDistanceConstraints(SoUYdHVZPlvJmDPezGdnpPvRYAVSUJCU, wbrhNOntPLRhHhejsmLMWSNoQAmBZVNV, LKAtsiYIEZFPMbYZuJXfbgtFwCwWDhpp, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(LKAtsiYIEZFPMbYZuJXfbgtFwCwWDhpp, wbrhNOntPLRhHhejsmLMWSNoQAmBZVNV, rYALMBvBXrKceUOddDePXHueuCNulGRS, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(wbrhNOntPLRhHhejsmLMWSNoQAmBZVNV, SoUYdHVZPlvJmDPezGdnpPvRYAVSUJCU, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                        }

                        FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                    }
                    else if (VeWQTLurjwVAmQQSVdLCcVVTLdauPhvt && lkLaHmiwfYGODMwjqnNpzDMzGEGdsDDC && JQjTHiWepJgcPbzLzOWpSjpGlvvoGqoQ && GtgFwfDzmDrKXAJQPIweTqPqREnDLtrv)
                    {
                        bool EoqFPYOhYZtlySEQXksjWhLrYqakUbmW = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc]);

                        
                        {
                            var HjpxEjErWhqiVFiydfBgQebvvkbFIZDV = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, EoqFPYOhYZtlySEQXksjWhLrYqakUbmW);
                            var AvlpKsoYbaVSXfcqibpgxoohXTgzBVda = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, EoqFPYOhYZtlySEQXksjWhLrYqakUbmW);
                            var iteFnJXbgmKwgxDrzmXAdNDVxXGTZOmd = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, EoqFPYOhYZtlySEQXksjWhLrYqakUbmW);
                            var TJhpBWKOLihKuEzBPzrtpRyNKRnKNdFn = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, EoqFPYOhYZtlySEQXksjWhLrYqakUbmW);

                            AddVolumeConstraintsAndDistanceConstraints(AvlpKsoYbaVSXfcqibpgxoohXTgzBVda, HjpxEjErWhqiVFiydfBgQebvvkbFIZDV, iteFnJXbgmKwgxDrzmXAdNDVxXGTZOmd, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(AvlpKsoYbaVSXfcqibpgxoohXTgzBVda, iteFnJXbgmKwgxDrzmXAdNDVxXGTZOmd, TJhpBWKOLihKuEzBPzrtpRyNKRnKNdFn, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(TJhpBWKOLihKuEzBPzrtpRyNKRnKNdFn, iteFnJXbgmKwgxDrzmXAdNDVxXGTZOmd, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                        }

                        
                        {
                            var evAtGhXqKHmtKmluSimiSltqSFfhaVzA = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, !EoqFPYOhYZtlySEQXksjWhLrYqakUbmW);
                            var WcGGKflQuFKcHXnRNwwgRkGhZCcFgNzs = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !EoqFPYOhYZtlySEQXksjWhLrYqakUbmW);
                            var lCgFcqZqPmWmttyiELXeojZDILlGtOoZ = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !EoqFPYOhYZtlySEQXksjWhLrYqakUbmW);
                            var vmnIUCskPhkHZvLVTDHfCFJddmTydecA = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !EoqFPYOhYZtlySEQXksjWhLrYqakUbmW);

                            AddVolumeConstraintsAndDistanceConstraints(evAtGhXqKHmtKmluSimiSltqSFfhaVzA, WcGGKflQuFKcHXnRNwwgRkGhZCcFgNzs, lCgFcqZqPmWmttyiELXeojZDILlGtOoZ, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(WcGGKflQuFKcHXnRNwwgRkGhZCcFgNzs, vmnIUCskPhkHZvLVTDHfCFJddmTydecA, lCgFcqZqPmWmttyiELXeojZDILlGtOoZ, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, lCgFcqZqPmWmttyiELXeojZDILlGtOoZ, WcGGKflQuFKcHXnRNwwgRkGhZCcFgNzs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                        }

                        FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                    }

                    
                    else if (VeWQTLurjwVAmQQSVdLCcVVTLdauPhvt && lkLaHmiwfYGODMwjqnNpzDMzGEGdsDDC && JGxfooHtQwNUcnvfcfghyQlphZaDaVzI)
                    {
                        
                        
                        bool tRmaQklwXIwrVTDPqfgsEprzxcyIaRxs = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[HppccEkxgkondFmqMHfkMBnewEColSMs]);

                        
                        
                        {
                            var WDioiQexfkdYoqphHhtXwpsWHnCLveuw = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, tRmaQklwXIwrVTDPqfgsEprzxcyIaRxs);
                            var DQsdhtOhealCSfqCIeRoPcyAomlLqeQn = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, tRmaQklwXIwrVTDPqfgsEprzxcyIaRxs);
                            var YepSYaDYbGspVdjGnwEftsZmNrhJRjaE = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, tRmaQklwXIwrVTDPqfgsEprzxcyIaRxs);

                            AddVolumeConstraintsAndDistanceConstraints(WDioiQexfkdYoqphHhtXwpsWHnCLveuw, YepSYaDYbGspVdjGnwEftsZmNrhJRjaE, DQsdhtOhealCSfqCIeRoPcyAomlLqeQn, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                        }

                        
                        
                        
                        
                        {
                            var ZrNPOfaEFIiXUkFAANWDfKEosQSjIoso = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, !tRmaQklwXIwrVTDPqfgsEprzxcyIaRxs);
                            var cCUJzBuFDNbzJIRGcwePgwmjEDVAGUcG = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !tRmaQklwXIwrVTDPqfgsEprzxcyIaRxs);
                            var KIEuJUTpQBYSwDUFwznGONLPKgfykWAZ = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !tRmaQklwXIwrVTDPqfgsEprzxcyIaRxs);

                            AddVolumeConstraintsAndDistanceConstraints(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, KIEuJUTpQBYSwDUFwznGONLPKgfykWAZ, cCUJzBuFDNbzJIRGcwePgwmjEDVAGUcG, ZrNPOfaEFIiXUkFAANWDfKEosQSjIoso, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, KIEuJUTpQBYSwDUFwznGONLPKgfykWAZ, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, cCUJzBuFDNbzJIRGcwePgwmjEDVAGUcG, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, KIEuJUTpQBYSwDUFwznGONLPKgfykWAZ, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                        }

                        FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                    }
                    else if (VeWQTLurjwVAmQQSVdLCcVVTLdauPhvt && UnfVfQoLNWXKvbYdzKquZamYKTbMChhq && JQjTHiWepJgcPbzLzOWpSjpGlvvoGqoQ)
                    {
                        

                        
                        bool FgjmMwtmWlplmItqQKeTWBkZUeNoNGTC = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[NsAmZBELNtoqlydVxiFjmppvOCFEnoHl]);

                        
                        
                        {
                            var dQdYxwTiyZNGaJNUjTarUVTcnxBccrZM = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FgjmMwtmWlplmItqQKeTWBkZUeNoNGTC);
                            var mfpOsgCdUfYfdjBHbofSjWEoeARKvxpQ = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, FgjmMwtmWlplmItqQKeTWBkZUeNoNGTC);
                            var fQlnfejEeMHHuekqGyvKEWAYONJVbOky = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, FgjmMwtmWlplmItqQKeTWBkZUeNoNGTC);

                            AddVolumeConstraintsAndDistanceConstraints(dQdYxwTiyZNGaJNUjTarUVTcnxBccrZM, mfpOsgCdUfYfdjBHbofSjWEoeARKvxpQ, fQlnfejEeMHHuekqGyvKEWAYONJVbOky, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                        }

                        
                        
                        
                        
                        {
                            var gmBgJJaELwLPrDDRWIcuSsijlRDULlDt = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, !FgjmMwtmWlplmItqQKeTWBkZUeNoNGTC);
                            var uODvcKPiGpYUJCwbVEkePyULDXXYWXZP = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !FgjmMwtmWlplmItqQKeTWBkZUeNoNGTC);
                            var xfTMpfwzIkFFEkUtboWkAKDzTDmCAGiQ = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !FgjmMwtmWlplmItqQKeTWBkZUeNoNGTC);

                            AddVolumeConstraintsAndDistanceConstraints(bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, xfTMpfwzIkFFEkUtboWkAKDzTDmCAGiQ, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(xfTMpfwzIkFFEkUtboWkAKDzTDmCAGiQ, HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, uODvcKPiGpYUJCwbVEkePyULDXXYWXZP, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(xfTMpfwzIkFFEkUtboWkAKDzTDmCAGiQ, HppccEkxgkondFmqMHfkMBnewEColSMs, uODvcKPiGpYUJCwbVEkePyULDXXYWXZP, gmBgJJaELwLPrDDRWIcuSsijlRDULlDt, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                        }

                        FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                    }
                    else if (lkLaHmiwfYGODMwjqnNpzDMzGEGdsDDC && UnfVfQoLNWXKvbYdzKquZamYKTbMChhq && GtgFwfDzmDrKXAJQPIweTqPqREnDLtrv)
                    {
                        

                        
                        bool UfVHziSUIrtMxfrpOPbEkcFwrYfvCluI = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[uISMMhkSIpKheJGJnBReJdCkjVWUEcKt]);

                        
                        
                        {
                            var dtBCFKomIEwihBIcZLpXMxMjKTIHrTIC = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, UfVHziSUIrtMxfrpOPbEkcFwrYfvCluI);
                            var BBIcXUNiAvECFtihfuvmYnPFYtxyxnIY = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, UfVHziSUIrtMxfrpOPbEkcFwrYfvCluI);
                            var uTQVyaRTfDErjjLukKhGPDpodocAhTFO = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, UfVHziSUIrtMxfrpOPbEkcFwrYfvCluI);

                            AddVolumeConstraintsAndDistanceConstraints(uTQVyaRTfDErjjLukKhGPDpodocAhTFO, dtBCFKomIEwihBIcZLpXMxMjKTIHrTIC, BBIcXUNiAvECFtihfuvmYnPFYtxyxnIY, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                        }

                        
                        
                        
                        
                        {
                            var MsoWVvFJDHnUuHPKjxpTAJhOPoKVVoVh = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, !UfVHziSUIrtMxfrpOPbEkcFwrYfvCluI);
                            var RzcFIwSVYBbaDFBozFEcOBuXiyZcTpID = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !UfVHziSUIrtMxfrpOPbEkcFwrYfvCluI);
                            var IxHqOdeJqkgzVFAFVXFDemOfxmHPknFU = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !UfVHziSUIrtMxfrpOPbEkcFwrYfvCluI);

                            AddVolumeConstraintsAndDistanceConstraints(bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, HppccEkxgkondFmqMHfkMBnewEColSMs, IxHqOdeJqkgzVFAFVXFDemOfxmHPknFU, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(IxHqOdeJqkgzVFAFVXFDemOfxmHPknFU, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, HppccEkxgkondFmqMHfkMBnewEColSMs, MsoWVvFJDHnUuHPKjxpTAJhOPoKVVoVh, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(MsoWVvFJDHnUuHPKjxpTAJhOPoKVVoVh, HppccEkxgkondFmqMHfkMBnewEColSMs, IxHqOdeJqkgzVFAFVXFDemOfxmHPknFU, RzcFIwSVYBbaDFBozFEcOBuXiyZcTpID, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                        }

                        FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                    }
                    else if (JGxfooHtQwNUcnvfcfghyQlphZaDaVzI && JQjTHiWepJgcPbzLzOWpSjpGlvvoGqoQ && GtgFwfDzmDrKXAJQPIweTqPqREnDLtrv)
                    {
                        

                        
                        bool DmnCuxXumqZLZaaLYEtlQNkYVcCvGgqV = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc]);

                        
                        
                        {
                            var ZXmAiztOrWAhCTCPxpgUQNJTckxuAgpX = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, DmnCuxXumqZLZaaLYEtlQNkYVcCvGgqV);
                            var caurUDaDJTeOiHoMtvgBlSnpdUWfPqqX = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, DmnCuxXumqZLZaaLYEtlQNkYVcCvGgqV);
                            var VzeKJHjlLeFVPyxAZJRPtNuxcjLoETcO = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, DmnCuxXumqZLZaaLYEtlQNkYVcCvGgqV);

                            AddVolumeConstraintsAndDistanceConstraints(ZXmAiztOrWAhCTCPxpgUQNJTckxuAgpX, caurUDaDJTeOiHoMtvgBlSnpdUWfPqqX, VzeKJHjlLeFVPyxAZJRPtNuxcjLoETcO, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                        }

                        
                        
                        
                        
                        {
                            var doVDunqsUrtqnTVkUBhBOvRwrWmZMieJ = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !DmnCuxXumqZLZaaLYEtlQNkYVcCvGgqV);
                            var yUyvnwvYaEiHhXVooRvZoALgeQBLKXeT = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !DmnCuxXumqZLZaaLYEtlQNkYVcCvGgqV);
                            var uNTXQbzufokivryRSliWWxSPoEOfbKrZ = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !DmnCuxXumqZLZaaLYEtlQNkYVcCvGgqV);

                            AddVolumeConstraintsAndDistanceConstraints(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, doVDunqsUrtqnTVkUBhBOvRwrWmZMieJ, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(doVDunqsUrtqnTVkUBhBOvRwrWmZMieJ, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, uNTXQbzufokivryRSliWWxSPoEOfbKrZ, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            AddVolumeConstraintsAndDistanceConstraints(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, yUyvnwvYaEiHhXVooRvZoALgeQBLKXeT, uNTXQbzufokivryRSliWWxSPoEOfbKrZ, doVDunqsUrtqnTVkUBhBOvRwrWmZMieJ, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                        }

                        FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                    }

                    
                    else
                    {
                        
                        if (VeWQTLurjwVAmQQSVdLCcVVTLdauPhvt && UnfVfQoLNWXKvbYdzKquZamYKTbMChhq)
                        {
                            
                            bool tnttMSSiNHiVNoIhIYILfmveGKcrRqZv = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[NsAmZBELNtoqlydVxiFjmppvOCFEnoHl]);

                            
                            {
                                var uhxxhUuxkXUwqUknKUgRwRxHRXrtNXvU = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, tnttMSSiNHiVNoIhIYILfmveGKcrRqZv);
                                var RmraIQHeFXurDpdfmehqwbNRQhjlegYA = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, tnttMSSiNHiVNoIhIYILfmveGKcrRqZv);

                                AddVolumeConstraintsAndDistanceConstraints(uhxxhUuxkXUwqUknKUgRwRxHRXrtNXvU, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, RmraIQHeFXurDpdfmehqwbNRQhjlegYA, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var nypSBPBsOtWluEjuhuprUprVVZgXIeAH = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, !tnttMSSiNHiVNoIhIYILfmveGKcrRqZv);
                                var DrWXckhjpnDMTxlsvTIXqoeisNNwNXfI = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !tnttMSSiNHiVNoIhIYILfmveGKcrRqZv);

                                AddVolumeConstraintsAndDistanceConstraints(HppccEkxgkondFmqMHfkMBnewEColSMs, nypSBPBsOtWluEjuhuprUprVVZgXIeAH, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                                AddVolumeConstraintsAndDistanceConstraints(nypSBPBsOtWluEjuhuprUprVVZgXIeAH, DrWXckhjpnDMTxlsvTIXqoeisNNwNXfI, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (UnfVfQoLNWXKvbYdzKquZamYKTbMChhq && lkLaHmiwfYGODMwjqnNpzDMzGEGdsDDC)
                        {
                            
                            bool rDKmFrShqrNYCBRCpyenHBlnareYUfkT = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[uISMMhkSIpKheJGJnBReJdCkjVWUEcKt]);

                            
                            {
                                var FDBIUdoPVOYRepxdrgUTbIEbQUmNjMoe = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, rDKmFrShqrNYCBRCpyenHBlnareYUfkT);
                                var XTtZZSUEpQcmgIZjMTooiifXMoLTHPJU = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, rDKmFrShqrNYCBRCpyenHBlnareYUfkT);

                                AddVolumeConstraintsAndDistanceConstraints(XTtZZSUEpQcmgIZjMTooiifXMoLTHPJU, FDBIUdoPVOYRepxdrgUTbIEbQUmNjMoe, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var VaiBuoJfuDCaGBwzsFNIjkskgxRTvLHT = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !rDKmFrShqrNYCBRCpyenHBlnareYUfkT);
                                var mfdUKastiZPaaWPWnCSuvXwPMLZxoJFV = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !rDKmFrShqrNYCBRCpyenHBlnareYUfkT);

                                AddVolumeConstraintsAndDistanceConstraints(VaiBuoJfuDCaGBwzsFNIjkskgxRTvLHT, HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                                AddVolumeConstraintsAndDistanceConstraints(VaiBuoJfuDCaGBwzsFNIjkskgxRTvLHT, mfdUKastiZPaaWPWnCSuvXwPMLZxoJFV, HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (VeWQTLurjwVAmQQSVdLCcVVTLdauPhvt && lkLaHmiwfYGODMwjqnNpzDMzGEGdsDDC)
                        {
                            
                            bool QrWRJIjhqYktJtdPPfMXGuJqffsfsZzE = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[HppccEkxgkondFmqMHfkMBnewEColSMs]);

                            
                            {
                                var algITVazqwVsALmRbSOXQRXcInHcTSRv = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, QrWRJIjhqYktJtdPPfMXGuJqffsfsZzE);
                                var tISQGMHctdhTKbwrXmATajgzwQdpQRLk = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, QrWRJIjhqYktJtdPPfMXGuJqffsfsZzE);

                                AddVolumeConstraintsAndDistanceConstraints(tISQGMHctdhTKbwrXmATajgzwQdpQRLk, algITVazqwVsALmRbSOXQRXcInHcTSRv, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var ihRqmmKcbIPewzzvVutWynuKzNlmJPxb = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, !QrWRJIjhqYktJtdPPfMXGuJqffsfsZzE);
                                var iUYDtCBQFWJzJQBAPsgqKoAwiYMSDdGX = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !QrWRJIjhqYktJtdPPfMXGuJqffsfsZzE);

                                AddVolumeConstraintsAndDistanceConstraints(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, iUYDtCBQFWJzJQBAPsgqKoAwiYMSDdGX, ihRqmmKcbIPewzzvVutWynuKzNlmJPxb, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                                AddVolumeConstraintsAndDistanceConstraints(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, iUYDtCBQFWJzJQBAPsgqKoAwiYMSDdGX, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (VeWQTLurjwVAmQQSVdLCcVVTLdauPhvt && JGxfooHtQwNUcnvfcfghyQlphZaDaVzI)
                        {
                            
                            bool ZpFcMEnLmjCgDBTkMORbpDnptHDcZUlG = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[HppccEkxgkondFmqMHfkMBnewEColSMs]);

                            
                            {
                                var rMfqplrhRjJynbBFVqgAXjQGjmXBUIJA = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, ZpFcMEnLmjCgDBTkMORbpDnptHDcZUlG);
                                var LBEmSTiZWXOyGqStlPmFoRHqHzlAHZhd = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, ZpFcMEnLmjCgDBTkMORbpDnptHDcZUlG);

                                AddVolumeConstraintsAndDistanceConstraints(rMfqplrhRjJynbBFVqgAXjQGjmXBUIJA, LBEmSTiZWXOyGqStlPmFoRHqHzlAHZhd, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var bTheebtqrmGUPEgUHMYbTjgLdDsSswqc = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, !ZpFcMEnLmjCgDBTkMORbpDnptHDcZUlG);
                                var HLVfPcLQNXjMTgdHiQncecZhJvsikqOK = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !ZpFcMEnLmjCgDBTkMORbpDnptHDcZUlG);

                                AddVolumeConstraintsAndDistanceConstraints(bTheebtqrmGUPEgUHMYbTjgLdDsSswqc, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                                AddVolumeConstraintsAndDistanceConstraints(bTheebtqrmGUPEgUHMYbTjgLdDsSswqc, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, HLVfPcLQNXjMTgdHiQncecZhJvsikqOK, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (JGxfooHtQwNUcnvfcfghyQlphZaDaVzI && JQjTHiWepJgcPbzLzOWpSjpGlvvoGqoQ)
                        {
                            
                            bool VMlBoxJHErdfNpCPLRcnVvSuFNikPiks = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc]);

                            
                            {
                                var rkHonNqYjDafsYEWwAslQGfurncgqIix = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, VMlBoxJHErdfNpCPLRcnVvSuFNikPiks);
                                var vmWWzVjVRRCYBfWayQIqFsaokJtMrzbB = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, VMlBoxJHErdfNpCPLRcnVvSuFNikPiks);

                                AddVolumeConstraintsAndDistanceConstraints(rkHonNqYjDafsYEWwAslQGfurncgqIix, vmWWzVjVRRCYBfWayQIqFsaokJtMrzbB, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var mWZFobekqIjqLPQBSLSUzusteBefXHvm = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !VMlBoxJHErdfNpCPLRcnVvSuFNikPiks);
                                var uJTsbpSxeNdCgEGHSqxhXlChbBCHKUhV = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !VMlBoxJHErdfNpCPLRcnVvSuFNikPiks);

                                AddVolumeConstraintsAndDistanceConstraints(mWZFobekqIjqLPQBSLSUzusteBefXHvm, uJTsbpSxeNdCgEGHSqxhXlChbBCHKUhV, HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                                AddVolumeConstraintsAndDistanceConstraints(HppccEkxgkondFmqMHfkMBnewEColSMs, uJTsbpSxeNdCgEGHSqxhXlChbBCHKUhV, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (VeWQTLurjwVAmQQSVdLCcVVTLdauPhvt && JQjTHiWepJgcPbzLzOWpSjpGlvvoGqoQ)
                        {
                            
                            bool OOIZbOhFKyaNxfUuLlcfmSHbZGCqaFDJ = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc]);

                            
                            {
                                var EDOFqPbyUdOSSSukNAwOlivSCqkuqGiP = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, OOIZbOhFKyaNxfUuLlcfmSHbZGCqaFDJ);
                                var NwIhejFHSgFudXtMkTOLTXVGEtwMUqRx = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, OOIZbOhFKyaNxfUuLlcfmSHbZGCqaFDJ);

                                AddVolumeConstraintsAndDistanceConstraints(EDOFqPbyUdOSSSukNAwOlivSCqkuqGiP, NwIhejFHSgFudXtMkTOLTXVGEtwMUqRx, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var kimqvojaYXylMCrVbXhiMvvqbEFTogzG = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, !OOIZbOhFKyaNxfUuLlcfmSHbZGCqaFDJ);
                                var TgObnhgoNuYXExCJEekfFxnEuezXqOLa = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !OOIZbOhFKyaNxfUuLlcfmSHbZGCqaFDJ);

                                AddVolumeConstraintsAndDistanceConstraints(HppccEkxgkondFmqMHfkMBnewEColSMs, TgObnhgoNuYXExCJEekfFxnEuezXqOLa, kimqvojaYXylMCrVbXhiMvvqbEFTogzG, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                                AddVolumeConstraintsAndDistanceConstraints(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, TgObnhgoNuYXExCJEekfFxnEuezXqOLa, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (lkLaHmiwfYGODMwjqnNpzDMzGEGdsDDC && JGxfooHtQwNUcnvfcfghyQlphZaDaVzI)
                        {
                            
                            bool rnafYQgsibsYiDzrdkgCWJjbNEFXcKUo = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[HppccEkxgkondFmqMHfkMBnewEColSMs]);

                            
                            {
                                var MaqqlevWdIDxYUBhMXacTYMYMtYkbxZH = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, rnafYQgsibsYiDzrdkgCWJjbNEFXcKUo);
                                var yrmdEwzyZUmTqZBbLcMQtRoIsVCmPKYr = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, rnafYQgsibsYiDzrdkgCWJjbNEFXcKUo);

                                AddVolumeConstraintsAndDistanceConstraints(HppccEkxgkondFmqMHfkMBnewEColSMs, MaqqlevWdIDxYUBhMXacTYMYMtYkbxZH, yrmdEwzyZUmTqZBbLcMQtRoIsVCmPKYr, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var ceEskARMJgzXVdyojtjVkJICLlVvFzEC = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !rnafYQgsibsYiDzrdkgCWJjbNEFXcKUo);
                                var IANrvDAsbaSjPbdCRdvZHLHhHWLDzOND = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !rnafYQgsibsYiDzrdkgCWJjbNEFXcKUo);

                                AddVolumeConstraintsAndDistanceConstraints(ceEskARMJgzXVdyojtjVkJICLlVvFzEC, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                                AddVolumeConstraintsAndDistanceConstraints(IANrvDAsbaSjPbdCRdvZHLHhHWLDzOND, ceEskARMJgzXVdyojtjVkJICLlVvFzEC, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (JGxfooHtQwNUcnvfcfghyQlphZaDaVzI && GtgFwfDzmDrKXAJQPIweTqPqREnDLtrv)
                        {
                            
                            bool scrcNJseIFEERNkOlFDlDgOHDmeRbAJG = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc]);

                            
                            {
                                var KeeCWriCPcwZsihKAWPAmreRQzNQgTyA = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, scrcNJseIFEERNkOlFDlDgOHDmeRbAJG);
                                var bDggOPvLDkFKwYqEDOCcVqKRIvHgsEmt = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, scrcNJseIFEERNkOlFDlDgOHDmeRbAJG);

                                AddVolumeConstraintsAndDistanceConstraints(KeeCWriCPcwZsihKAWPAmreRQzNQgTyA, bDggOPvLDkFKwYqEDOCcVqKRIvHgsEmt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var eyFSqTdgeTwjFcIQItgtnPYllUjpzrCM = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !scrcNJseIFEERNkOlFDlDgOHDmeRbAJG);
                                var bpgkRcVNufrQPPDTMdhqHjcWTHUyqwPt = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !scrcNJseIFEERNkOlFDlDgOHDmeRbAJG);

                                AddVolumeConstraintsAndDistanceConstraints(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bpgkRcVNufrQPPDTMdhqHjcWTHUyqwPt, HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                                AddVolumeConstraintsAndDistanceConstraints(HppccEkxgkondFmqMHfkMBnewEColSMs, bpgkRcVNufrQPPDTMdhqHjcWTHUyqwPt, eyFSqTdgeTwjFcIQItgtnPYllUjpzrCM, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (lkLaHmiwfYGODMwjqnNpzDMzGEGdsDDC && GtgFwfDzmDrKXAJQPIweTqPqREnDLtrv)
                        {
                            
                            bool jCnOKKhtnfBAayhGrbcVIRCWtUZwtVzJ = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[uISMMhkSIpKheJGJnBReJdCkjVWUEcKt]);

                            
                            {
                                var iOjXWTbBuuafJNHvUZXYCakpnIXdKGMJ = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, jCnOKKhtnfBAayhGrbcVIRCWtUZwtVzJ);
                                var QqgCKPIbLiMOTfifieIfRTtKUkDYTWID = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, jCnOKKhtnfBAayhGrbcVIRCWtUZwtVzJ);

                                AddVolumeConstraintsAndDistanceConstraints(iOjXWTbBuuafJNHvUZXYCakpnIXdKGMJ, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, QqgCKPIbLiMOTfifieIfRTtKUkDYTWID, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var eyEaGoFIgVbvYZjAlUwzyrSCHooiqfdI = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !jCnOKKhtnfBAayhGrbcVIRCWtUZwtVzJ);
                                var zpMzUejbyWXmoklRKZJIkfYLPFZQrsKd = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !jCnOKKhtnfBAayhGrbcVIRCWtUZwtVzJ);

                                AddVolumeConstraintsAndDistanceConstraints(HppccEkxgkondFmqMHfkMBnewEColSMs, eyEaGoFIgVbvYZjAlUwzyrSCHooiqfdI, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                                AddVolumeConstraintsAndDistanceConstraints(eyEaGoFIgVbvYZjAlUwzyrSCHooiqfdI, zpMzUejbyWXmoklRKZJIkfYLPFZQrsKd, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (UnfVfQoLNWXKvbYdzKquZamYKTbMChhq && JQjTHiWepJgcPbzLzOWpSjpGlvvoGqoQ)
                        {
                            
                            bool hPxRiaIgtDoquunWqGdLOcDhYGDwYddu = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[NsAmZBELNtoqlydVxiFjmppvOCFEnoHl]);

                            
                            {
                                var PyJTtIhZLDvdCCOdaniGWbXmVeXpqfQF = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, hPxRiaIgtDoquunWqGdLOcDhYGDwYddu);
                                var coPLdamkluWwYqbRuwpsIbuqqfNnfvsn = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, hPxRiaIgtDoquunWqGdLOcDhYGDwYddu);

                                AddVolumeConstraintsAndDistanceConstraints(PyJTtIhZLDvdCCOdaniGWbXmVeXpqfQF, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, coPLdamkluWwYqbRuwpsIbuqqfNnfvsn, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var uNmhdgMgiVKDRLKdrnoIHAVDrmoQAmXr = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !hPxRiaIgtDoquunWqGdLOcDhYGDwYddu);
                                var ZjELJnWjQCyiyZCiWMvBEXaFuSIzCKUu = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !hPxRiaIgtDoquunWqGdLOcDhYGDwYddu);

                                AddVolumeConstraintsAndDistanceConstraints(uNmhdgMgiVKDRLKdrnoIHAVDrmoQAmXr, ZjELJnWjQCyiyZCiWMvBEXaFuSIzCKUu, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                                AddVolumeConstraintsAndDistanceConstraints(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, ZjELJnWjQCyiyZCiWMvBEXaFuSIzCKUu, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (JQjTHiWepJgcPbzLzOWpSjpGlvvoGqoQ && GtgFwfDzmDrKXAJQPIweTqPqREnDLtrv)
                        {
                            
                            bool TLopOjocKtkNvTUFqDDqsfZtBqGPMJdJ = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc]);

                            
                            {
                                var wEGPMVKgxdCpfjgoRiogiMbvvvADxpLi = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, TLopOjocKtkNvTUFqDDqsfZtBqGPMJdJ);
                                var MqowqhwqHLQOoyxjnfmxDtsSdlSltWSB = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, TLopOjocKtkNvTUFqDDqsfZtBqGPMJdJ);

                                AddVolumeConstraintsAndDistanceConstraints(MqowqhwqHLQOoyxjnfmxDtsSdlSltWSB, wEGPMVKgxdCpfjgoRiogiMbvvvADxpLi, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var dnmvtWdXVFbZsqKXCxerAcmUYZyirxPo = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !TLopOjocKtkNvTUFqDDqsfZtBqGPMJdJ);
                                var rhsxbKAnMozYbtjUMoLVgmwKBTNELQut = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !TLopOjocKtkNvTUFqDDqsfZtBqGPMJdJ);

                                AddVolumeConstraintsAndDistanceConstraints(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, dnmvtWdXVFbZsqKXCxerAcmUYZyirxPo, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                                AddVolumeConstraintsAndDistanceConstraints(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, dnmvtWdXVFbZsqKXCxerAcmUYZyirxPo, rhsxbKAnMozYbtjUMoLVgmwKBTNELQut, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (UnfVfQoLNWXKvbYdzKquZamYKTbMChhq && GtgFwfDzmDrKXAJQPIweTqPqREnDLtrv)
                        {
                            
                            bool kjiijPUhyNRCJYTWWWjJokbuqCvGIgLv = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[uISMMhkSIpKheJGJnBReJdCkjVWUEcKt]);

                            
                            {
                                var ARupiCyzbejPlFQwxTPwgRzGlBHJjDKC = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, kjiijPUhyNRCJYTWWWjJokbuqCvGIgLv);
                                var EHUtkOQliYGoBkKJliRarErAOpHjXiKS = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, kjiijPUhyNRCJYTWWWjJokbuqCvGIgLv);

                                AddVolumeConstraintsAndDistanceConstraints(ARupiCyzbejPlFQwxTPwgRzGlBHJjDKC, EHUtkOQliYGoBkKJliRarErAOpHjXiKS, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var RrNOWSQEDeejHwhGggadmfaCuQFTXsXb = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !kjiijPUhyNRCJYTWWWjJokbuqCvGIgLv);
                                var gKbrtYvmkdlYbezGuLXnhYfupHPdlwlM = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !kjiijPUhyNRCJYTWWWjJokbuqCvGIgLv);

                                AddVolumeConstraintsAndDistanceConstraints(RrNOWSQEDeejHwhGggadmfaCuQFTXsXb, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                                AddVolumeConstraintsAndDistanceConstraints(RrNOWSQEDeejHwhGggadmfaCuQFTXsXb, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, gKbrtYvmkdlYbezGuLXnhYfupHPdlwlM, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }

                        
                        else if (VeWQTLurjwVAmQQSVdLCcVVTLdauPhvt)
                        {
                            
                            bool qNqFyODFBWANDmsSKPVfWQjHxJMDSIdL = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[NsAmZBELNtoqlydVxiFjmppvOCFEnoHl]);

                            
                            {
                                var XDZfdUuTyFXPqzqbQKPUXFEJdNICxJAR = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, qNqFyODFBWANDmsSKPVfWQjHxJMDSIdL);

                                AddVolumeConstraintsAndDistanceConstraints(bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, XDZfdUuTyFXPqzqbQKPUXFEJdNICxJAR, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var iZSdaeoauPKbhlbjsjqOgKzGDeXaFNBp = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, !qNqFyODFBWANDmsSKPVfWQjHxJMDSIdL);

                                AddVolumeConstraintsAndDistanceConstraints(bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, iZSdaeoauPKbhlbjsjqOgKzGDeXaFNBp, HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (UnfVfQoLNWXKvbYdzKquZamYKTbMChhq)
                        {
                            
                            bool sFbCmlSNcLFRuimZsGyaxiRLgoyoFkGO = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[NsAmZBELNtoqlydVxiFjmppvOCFEnoHl]);

                            
                            {
                                var GMVBMFggISdFIvyvNAjPXKKOWhvbJesF = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, sFbCmlSNcLFRuimZsGyaxiRLgoyoFkGO);

                                AddVolumeConstraintsAndDistanceConstraints(bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, GMVBMFggISdFIvyvNAjPXKKOWhvbJesF, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var pevkCzCJdWFOBOTKwxfvGAYPJZCGFlNH = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !sFbCmlSNcLFRuimZsGyaxiRLgoyoFkGO);

                                AddVolumeConstraintsAndDistanceConstraints(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, pevkCzCJdWFOBOTKwxfvGAYPJZCGFlNH, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, HppccEkxgkondFmqMHfkMBnewEColSMs, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (lkLaHmiwfYGODMwjqnNpzDMzGEGdsDDC)
                        {
                            
                            bool EPNrIyKQJVUfNQTgGKfmzyDcdWMosSlj = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[uISMMhkSIpKheJGJnBReJdCkjVWUEcKt]);

                            
                            {
                                var lvKApKkYnVYOWYqIvzkpuVTlbmflzrPD = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, EPNrIyKQJVUfNQTgGKfmzyDcdWMosSlj);

                                AddVolumeConstraintsAndDistanceConstraints(lvKApKkYnVYOWYqIvzkpuVTlbmflzrPD, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var tooRXYiqukskKhLBmLGEXibfaPRoXlMh = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, !EPNrIyKQJVUfNQTgGKfmzyDcdWMosSlj);

                                AddVolumeConstraintsAndDistanceConstraints(HppccEkxgkondFmqMHfkMBnewEColSMs, tooRXYiqukskKhLBmLGEXibfaPRoXlMh, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (JGxfooHtQwNUcnvfcfghyQlphZaDaVzI)
                        {
                            
                            bool SWznzPyFhEDBrzTMoDAxBjJExrZrSyKJ = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc]);

                            
                            {
                                var KLHhbMpDShfQIcxaWhbZEgqkOaOsYDqf = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, SWznzPyFhEDBrzTMoDAxBjJExrZrSyKJ);

                                AddVolumeConstraintsAndDistanceConstraints(KLHhbMpDShfQIcxaWhbZEgqkOaOsYDqf, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var IFruJvooVrCmfjccoPUnpyHCGduZbgxa = GetParticleIndexForIntersection(HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !SWznzPyFhEDBrzTMoDAxBjJExrZrSyKJ);

                                AddVolumeConstraintsAndDistanceConstraints(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, IFruJvooVrCmfjccoPUnpyHCGduZbgxa, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (JQjTHiWepJgcPbzLzOWpSjpGlvvoGqoQ)
                        {
                            
                            bool uGsNcHhGplkzRfAqCGLTJGuzMKjhMtFA = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[NsAmZBELNtoqlydVxiFjmppvOCFEnoHl]);

                            
                            {
                                var uqYobOEPnIuPOFIwdedSoNnnCIYnRffG = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, uGsNcHhGplkzRfAqCGLTJGuzMKjhMtFA);

                                AddVolumeConstraintsAndDistanceConstraints(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, HppccEkxgkondFmqMHfkMBnewEColSMs, uqYobOEPnIuPOFIwdedSoNnnCIYnRffG, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var JccsSlUzOjSNhcfOyKhVFFsIVCMSMoHo = GetParticleIndexForIntersection(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !uGsNcHhGplkzRfAqCGLTJGuzMKjhMtFA);

                                AddVolumeConstraintsAndDistanceConstraints(JccsSlUzOjSNhcfOyKhVFFsIVCMSMoHo, HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }
                        else if (GtgFwfDzmDrKXAJQPIweTqPqREnDLtrv)
                        {
                            
                            bool nSnHaPBBWnbogwEZpYLZToiZgsVSgoLN = cuttingDisc.GetSideOfPoint(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position[uISMMhkSIpKheJGJnBReJdCkjVWUEcKt]);

                            
                            {
                                var FhpXbWoeqpeRZIVYUAeMyRxXKaUNnMTW = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, nSnHaPBBWnbogwEZpYLZToiZgsVSgoLN);

                                AddVolumeConstraintsAndDistanceConstraints(HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, FhpXbWoeqpeRZIVYUAeMyRxXKaUNnMTW, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            
                            {
                                var NBsdZREeRNAwPChFWzsegaHgjlrJgHhs = GetParticleIndexForIntersection(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, !nSnHaPBBWnbogwEZpYLZToiZgsVSgoLN);

                                AddVolumeConstraintsAndDistanceConstraints(HppccEkxgkondFmqMHfkMBnewEColSMs, NBsdZREeRNAwPChFWzsegaHgjlrJgHhs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, FKFafGlvOMIKTrIguQbvHsiQSjEMcJQm, hhWpAUEhJRdpgPeZVbrcQIjGKQHBYgDQ, TnfLhvHoFbxPqPpQhlfRWQRuvsepYEon);
                            }

                            FIBUpfaURUhyAmeVNOybYuroshvQdfKB.Add(tCqStDKtwhrWgZLGEcmoIAixspIVMqLD);
                        }

                        
                        if (VeWQTLurjwVAmQQSVdLCcVVTLdauPhvt)
                        {
                            apkrvtbhuvOyhhITcfYTJVJfXSjkUrYp.Add(HppccEkxgkondFmqMHfkMBnewEColSMs < NsAmZBELNtoqlydVxiFjmppvOCFEnoHl ? (HppccEkxgkondFmqMHfkMBnewEColSMs, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl) : (NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, HppccEkxgkondFmqMHfkMBnewEColSMs));
                        }

                        if (UnfVfQoLNWXKvbYdzKquZamYKTbMChhq)
                        {
                            apkrvtbhuvOyhhITcfYTJVJfXSjkUrYp.Add(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl < uISMMhkSIpKheJGJnBReJdCkjVWUEcKt ? (NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt) : (uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl));
                        }

                        if (lkLaHmiwfYGODMwjqnNpzDMzGEGdsDDC)
                        {
                            apkrvtbhuvOyhhITcfYTJVJfXSjkUrYp.Add(HppccEkxgkondFmqMHfkMBnewEColSMs < uISMMhkSIpKheJGJnBReJdCkjVWUEcKt ? (HppccEkxgkondFmqMHfkMBnewEColSMs, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt) : (uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, HppccEkxgkondFmqMHfkMBnewEColSMs));
                        }

                        if (JGxfooHtQwNUcnvfcfghyQlphZaDaVzI)
                        {
                            apkrvtbhuvOyhhITcfYTJVJfXSjkUrYp.Add(HppccEkxgkondFmqMHfkMBnewEColSMs < bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc ? (HppccEkxgkondFmqMHfkMBnewEColSMs, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc) : (bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, HppccEkxgkondFmqMHfkMBnewEColSMs));
                        }

                        if (JQjTHiWepJgcPbzLzOWpSjpGlvvoGqoQ)
                        {
                            apkrvtbhuvOyhhITcfYTJVJfXSjkUrYp.Add(NsAmZBELNtoqlydVxiFjmppvOCFEnoHl < bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc ? (NsAmZBELNtoqlydVxiFjmppvOCFEnoHl, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc) : (bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, NsAmZBELNtoqlydVxiFjmppvOCFEnoHl));
                        }

                        if (GtgFwfDzmDrKXAJQPIweTqPqREnDLtrv)
                        {
                            apkrvtbhuvOyhhITcfYTJVJfXSjkUrYp.Add(uISMMhkSIpKheJGJnBReJdCkjVWUEcKt < bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc ? (uISMMhkSIpKheJGJnBReJdCkjVWUEcKt, bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc) : (bHgdJMFfIfTxyZjdHvqKRZDJyhNNSncc, uISMMhkSIpKheJGJnBReJdCkjVWUEcKt));
                        }
                    }
                }

                Debug.Log("Generated " + jkrmjtfpOMwkUsOGcFBHXuNolFYnYzwr.Count + " new volume constraints");
                Debug.Log("Generated " + IgwgxlhJVwaFYoZMeUuFaalpNkfpupLz.Count + " new distance constraints");

                
                Dictionary<int, int> fpkwQuJNjHOMxcjeRBpZdtAZdzMCXujp = new Dictionary<int, int>(); 
                foreach (var boundaryIntersection in apkrvtbhuvOyhhITcfYTJVJfXSjkUrYp)
                {
                    
                    int HDEvwtImNOrmAbEXdgJJWBUzzqDTkLGJ = GetParticleIndexForIntersection(boundaryIntersection.Item1, boundaryIntersection.Item2, false);
                    int tmLhJvChUFSNrMhBQsuQZodfVQlOBUjP = GetParticleIndexForIntersection(boundaryIntersection.Item1, boundaryIntersection.Item2, true);

                    fpkwQuJNjHOMxcjeRBpZdtAZdzMCXujp.Add(HDEvwtImNOrmAbEXdgJJWBUzzqDTkLGJ, tmLhJvChUFSNrMhBQsuQZodfVQlOBUjP);

                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Enabled[HDEvwtImNOrmAbEXdgJJWBUzzqDTkLGJ] = false;
                }

                
                for (int CBhUmooctYCweVYzGutuYusFEQNWaHAx = 0; CBhUmooctYCweVYzGutuYusFEQNWaHAx < IgwgxlhJVwaFYoZMeUuFaalpNkfpupLz.Count; CBhUmooctYCweVYzGutuYusFEQNWaHAx++)
                {
                    bzDTjloiyzdscSnzKwWJqKJUliDYMfRC[CBhUmooctYCweVYzGutuYusFEQNWaHAx] = GetParticleReplacement(bzDTjloiyzdscSnzKwWJqKJUliDYMfRC[CBhUmooctYCweVYzGutuYusFEQNWaHAx]);

                    GPSfDcIfLdNviJJVEmYuMsLgXhAaqbLM[CBhUmooctYCweVYzGutuYusFEQNWaHAx] = GetParticleReplacement(GPSfDcIfLdNviJJVEmYuMsLgXhAaqbLM[CBhUmooctYCweVYzGutuYusFEQNWaHAx]);

                    
                    if (bzDTjloiyzdscSnzKwWJqKJUliDYMfRC[CBhUmooctYCweVYzGutuYusFEQNWaHAx] == GPSfDcIfLdNviJJVEmYuMsLgXhAaqbLM[CBhUmooctYCweVYzGutuYusFEQNWaHAx])
                    {
                        IgwgxlhJVwaFYoZMeUuFaalpNkfpupLz[CBhUmooctYCweVYzGutuYusFEQNWaHAx] = false;
                    }
                }

                
                for (int YMruJZvwAFhEhXDdFopQIvQJBnqaukYj = 0; YMruJZvwAFhEhXDdFopQIvQJBnqaukYj < jkrmjtfpOMwkUsOGcFBHXuNolFYnYzwr.Count; YMruJZvwAFhEhXDdFopQIvQJBnqaukYj++)
                {
                    rhIBZUFyNQwVCmGnXoVmFLBRIDKAzHNd[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] = GetParticleReplacement(rhIBZUFyNQwVCmGnXoVmFLBRIDKAzHNd[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj]);
                    gkWuTpLZGlyOFGyRsKuyjcdEPQzpegyn[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] = GetParticleReplacement(gkWuTpLZGlyOFGyRsKuyjcdEPQzpegyn[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj]);
                    AxsnNfCOCQqpdcGhrzgBasOEqvWOgomJ[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] = GetParticleReplacement(AxsnNfCOCQqpdcGhrzgBasOEqvWOgomJ[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj]);
                    NEKlvvkOuLUIifuETmCjrsHeDpeAQMVU[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] = GetParticleReplacement(NEKlvvkOuLUIifuETmCjrsHeDpeAQMVU[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj]);

                    
                    if (rhIBZUFyNQwVCmGnXoVmFLBRIDKAzHNd[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] == gkWuTpLZGlyOFGyRsKuyjcdEPQzpegyn[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] ||
                        rhIBZUFyNQwVCmGnXoVmFLBRIDKAzHNd[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] == AxsnNfCOCQqpdcGhrzgBasOEqvWOgomJ[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] ||
                        rhIBZUFyNQwVCmGnXoVmFLBRIDKAzHNd[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] == NEKlvvkOuLUIifuETmCjrsHeDpeAQMVU[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] ||
                        gkWuTpLZGlyOFGyRsKuyjcdEPQzpegyn[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] == AxsnNfCOCQqpdcGhrzgBasOEqvWOgomJ[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] ||
                        gkWuTpLZGlyOFGyRsKuyjcdEPQzpegyn[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] == NEKlvvkOuLUIifuETmCjrsHeDpeAQMVU[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] ||
                        AxsnNfCOCQqpdcGhrzgBasOEqvWOgomJ[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] == NEKlvvkOuLUIifuETmCjrsHeDpeAQMVU[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj])
                    {
                        jkrmjtfpOMwkUsOGcFBHXuNolFYnYzwr[YMruJZvwAFhEhXDdFopQIvQJBnqaukYj] = false;
                    }
                }

                fpkwQuJNjHOMxcjeRBpZdtAZdzMCXujp.Clear();

                
                var YAqKsBaITRIbQIzTzIZnTyqdRMkYgStE = 0;
                for (int OfgEsfIThqseetRpdOuXNeWNTFTPKKfB = IgwgxlhJVwaFYoZMeUuFaalpNkfpupLz.Count - 1; OfgEsfIThqseetRpdOuXNeWNTFTPKKfB >= 0; OfgEsfIThqseetRpdOuXNeWNTFTPKKfB--)
                {
                    if (!IgwgxlhJVwaFYoZMeUuFaalpNkfpupLz[OfgEsfIThqseetRpdOuXNeWNTFTPKKfB])
                    {
                        IgwgxlhJVwaFYoZMeUuFaalpNkfpupLz.RemoveAt(OfgEsfIThqseetRpdOuXNeWNTFTPKKfB);
                        lQRXcmLEELmKJMYlwPfbtUTlVbawrMoC.RemoveAt(OfgEsfIThqseetRpdOuXNeWNTFTPKKfB);
                        PbhGOKDnPWClGWLVWxavDyhpDbecYLmM.RemoveAt(OfgEsfIThqseetRpdOuXNeWNTFTPKKfB);
                        bzDTjloiyzdscSnzKwWJqKJUliDYMfRC.RemoveAt(OfgEsfIThqseetRpdOuXNeWNTFTPKKfB);
                        GPSfDcIfLdNviJJVEmYuMsLgXhAaqbLM.RemoveAt(OfgEsfIThqseetRpdOuXNeWNTFTPKKfB);
                        ejtMeXvcJhbVYDiJuPgSHqqOARjelPIJ.RemoveAt(OfgEsfIThqseetRpdOuXNeWNTFTPKKfB);
                        YAqKsBaITRIbQIzTzIZnTyqdRMkYgStE++;
                    }
                }

                YAqKsBaITRIbQIzTzIZnTyqdRMkYgStE = 0;
                for (int nfMIDuDbjYBliLVXuXGncIoQhUwbPEoj = jkrmjtfpOMwkUsOGcFBHXuNolFYnYzwr.Count - 1; nfMIDuDbjYBliLVXuXGncIoQhUwbPEoj >= 0; nfMIDuDbjYBliLVXuXGncIoQhUwbPEoj--)
                {
                    if (!jkrmjtfpOMwkUsOGcFBHXuNolFYnYzwr[nfMIDuDbjYBliLVXuXGncIoQhUwbPEoj])
                    {
                        jkrmjtfpOMwkUsOGcFBHXuNolFYnYzwr.RemoveAt(nfMIDuDbjYBliLVXuXGncIoQhUwbPEoj);
                        jgiVKUJTAihYhcvMJabIIjwJErVpfqVy.RemoveAt(nfMIDuDbjYBliLVXuXGncIoQhUwbPEoj);
                        mQvioNJoLMjStPCZGiWmvIecxuBeJqYJ.RemoveAt(nfMIDuDbjYBliLVXuXGncIoQhUwbPEoj);
                        rhIBZUFyNQwVCmGnXoVmFLBRIDKAzHNd.RemoveAt(nfMIDuDbjYBliLVXuXGncIoQhUwbPEoj);
                        gkWuTpLZGlyOFGyRsKuyjcdEPQzpegyn.RemoveAt(nfMIDuDbjYBliLVXuXGncIoQhUwbPEoj);
                        AxsnNfCOCQqpdcGhrzgBasOEqvWOgomJ.RemoveAt(nfMIDuDbjYBliLVXuXGncIoQhUwbPEoj);
                        NEKlvvkOuLUIifuETmCjrsHeDpeAQMVU.RemoveAt(nfMIDuDbjYBliLVXuXGncIoQhUwbPEoj);
                        vcXNpUFTwIoGFIFlcJEtHmxWXGgNjjCX.RemoveAt(nfMIDuDbjYBliLVXuXGncIoQhUwbPEoj);
                        YAqKsBaITRIbQIzTzIZnTyqdRMkYgStE++;
                    }
                }

                
                MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.AddRuntimeDistanceConstraints(
                    IgwgxlhJVwaFYoZMeUuFaalpNkfpupLz.ToArray(),
                    lQRXcmLEELmKJMYlwPfbtUTlVbawrMoC.ToArray(),
                    PbhGOKDnPWClGWLVWxavDyhpDbecYLmM.ToArray(),
                    bzDTjloiyzdscSnzKwWJqKJUliDYMfRC.ToArray(),
                    GPSfDcIfLdNviJJVEmYuMsLgXhAaqbLM.ToArray(),
                    ejtMeXvcJhbVYDiJuPgSHqqOARjelPIJ.ToArray());

                
                MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.AddRuntimeVolumeConstraints(
                    jkrmjtfpOMwkUsOGcFBHXuNolFYnYzwr.ToArray(),
                    jgiVKUJTAihYhcvMJabIIjwJErVpfqVy.ToArray(),
                    mQvioNJoLMjStPCZGiWmvIecxuBeJqYJ.ToArray(),
                    rhIBZUFyNQwVCmGnXoVmFLBRIDKAzHNd.ToArray(),
                    gkWuTpLZGlyOFGyRsKuyjcdEPQzpegyn.ToArray(),
                    AxsnNfCOCQqpdcGhrzgBasOEqvWOgomJ.ToArray(),
                    NEKlvvkOuLUIifuETmCjrsHeDpeAQMVU.ToArray(),
                    vcXNpUFTwIoGFIFlcJEtHmxWXGgNjjCX.ToArray());

                
                mNYomtnKnkQozVWHzuBEWMtzkgLkagUu.ResizeArray(mNYomtnKnkQozVWHzuBEWMtzkgLkagUu.Length + (jkrmjtfpOMwkUsOGcFBHXuNolFYnYzwr.Count * 4), Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

                
                for (int NqwLGBkALcvGKVJCefUdPHQCeFCYcVHv = mNYomtnKnkQozVWHzuBEWMtzkgLkagUu.Length - (jkrmjtfpOMwkUsOGcFBHXuNolFYnYzwr.Count * 4); NqwLGBkALcvGKVJCefUdPHQCeFCYcVHv < mNYomtnKnkQozVWHzuBEWMtzkgLkagUu.Length; NqwLGBkALcvGKVJCefUdPHQCeFCYcVHv++)
                {
                    mNYomtnKnkQozVWHzuBEWMtzkgLkagUu[NqwLGBkALcvGKVJCefUdPHQCeFCYcVHv] = -1;
                }

                
                foreach (var volumeConstraintIndex in FIBUpfaURUhyAmeVNOybYuroshvQdfKB)
                {
                    MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.Enabled[volumeConstraintIndex] = false;
                }

                int GetParticleReplacement(int particleIndex)
                {
                    int kpJpMeWtGZLmVLiAwOkJtgfhJXhuVrVW = particleIndex;
                    int vcQprBHFzbPTXnbMLjZTMIyXWsvyzouN = kpJpMeWtGZLmVLiAwOkJtgfhJXhuVrVW;
                    HashSet<int> YMYeVJXgCtCIOdyRniasHJyAgJYZfbWe = new HashSet<int>
                    {
                        particleIndex,
                    };

                    while (fpkwQuJNjHOMxcjeRBpZdtAZdzMCXujp.TryGetValue(kpJpMeWtGZLmVLiAwOkJtgfhJXhuVrVW, out vcQprBHFzbPTXnbMLjZTMIyXWsvyzouN))
                    {
                        
                        if (YMYeVJXgCtCIOdyRniasHJyAgJYZfbWe.Contains(vcQprBHFzbPTXnbMLjZTMIyXWsvyzouN))
                        {
                            return kpJpMeWtGZLmVLiAwOkJtgfhJXhuVrVW;
                        }
                        else
                        {
                            kpJpMeWtGZLmVLiAwOkJtgfhJXhuVrVW = vcQprBHFzbPTXnbMLjZTMIyXWsvyzouN;
                            YMYeVJXgCtCIOdyRniasHJyAgJYZfbWe.Add(kpJpMeWtGZLmVLiAwOkJtgfhJXhuVrVW);
                        }
                    }

                    return kpJpMeWtGZLmVLiAwOkJtgfhJXhuVrVW;
                }

                void AddVolumeConstraintsAndDistanceConstraints(int p0, int p1, int p2, int p3, bool enabled, bool breakable, int physicsMaterialIndex)
                {
                    
                    if (!ValidateDistance(p0, p1) ||
                        !ValidateDistance(p1, p2) ||
                        !ValidateDistance(p2, p0) ||

                        !ValidateDistance(p3, p0) ||
                        !ValidateDistance(p3, p1) ||
                        !ValidateDistance(p3, p2))
                    {
                        return;
                    }

                    
                    jkrmjtfpOMwkUsOGcFBHXuNolFYnYzwr.Add(enabled);
                    jgiVKUJTAihYhcvMJabIIjwJErVpfqVy.Add(breakable);
                    rhIBZUFyNQwVCmGnXoVmFLBRIDKAzHNd.Add(p0);
                    gkWuTpLZGlyOFGyRsKuyjcdEPQzpegyn.Add(p1);
                    AxsnNfCOCQqpdcGhrzgBasOEqvWOgomJ.Add(p2);
                    NEKlvvkOuLUIifuETmCjrsHeDpeAQMVU.Add(p3);

                    var VVDpIFbwMajXeeWjFqBvlGDZbVIYQFNf = Math.TetrahedronVolume(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition[p0], MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition[p1], MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition[p2], MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition[p3]);
                    vcXNpUFTwIoGFIFlcJEtHmxWXGgNjjCX.Add(VVDpIFbwMajXeeWjFqBvlGDZbVIYQFNf);
                    mQvioNJoLMjStPCZGiWmvIecxuBeJqYJ.Add(physicsMaterialIndex);

                    
                    AddDistanceConstraintIfNotAlreadyAdded(p0, p1, enabled, breakable, physicsMaterialIndex);
                    AddDistanceConstraintIfNotAlreadyAdded(p1, p2, enabled, breakable, physicsMaterialIndex);
                    AddDistanceConstraintIfNotAlreadyAdded(p2, p0, enabled, breakable, physicsMaterialIndex);

                    AddDistanceConstraintIfNotAlreadyAdded(p3, p0, enabled, breakable, physicsMaterialIndex);
                    AddDistanceConstraintIfNotAlreadyAdded(p3, p1, enabled, breakable, physicsMaterialIndex);
                    AddDistanceConstraintIfNotAlreadyAdded(p3, p2, enabled, breakable, physicsMaterialIndex);
                }

                bool ValidateDistance(int p0, int p1)
                {
                    if (math.distancesq(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition[p0], MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition[p1]) < vqYRARNEENgfEOlJmuZvNdDIbxYPusTs * vqYRARNEENgfEOlJmuZvNdDIbxYPusTs)
                    {
                        return false;
                    }

                    return true;
                }

                bool AddDistanceConstraintIfNotAlreadyAdded(int p0, int p1, bool enabled, bool breakable, int physicsMaterialIndex)
                {
                    
                    if (pkanolhUdvBUVNmgOikOeJXSvDeujAdR.Contains(p0 < p1 ? (p0, p1) : (p1, p0)))
                    {
                        return true;
                    }

                    float ZTXqPNQLHDFCambACKyjbmKKZSxKsNaL = math.distance(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition[p0], MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition[p1]);

                    IgwgxlhJVwaFYoZMeUuFaalpNkfpupLz.Add(enabled);
                    lQRXcmLEELmKJMYlwPfbtUTlVbawrMoC.Add(breakable);
                    bzDTjloiyzdscSnzKwWJqKJUliDYMfRC.Add(p0);
                    GPSfDcIfLdNviJJVEmYuMsLgXhAaqbLM.Add(p1);

                    ejtMeXvcJhbVYDiJuPgSHqqOARjelPIJ.Add(ZTXqPNQLHDFCambACKyjbmKKZSxKsNaL);
                    PbhGOKDnPWClGWLVWxavDyhpDbecYLmM.Add(physicsMaterialIndex);

                    pkanolhUdvBUVNmgOikOeJXSvDeujAdR.Add(p0 < p1 ? (p0, p1) : (p1, p0));

                    return true;
                }
            }

            Debug.Log("SMESH INFO: " + MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Count + " " + MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeDistanceConstraintData.Count + " " + MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.Count);
        }

        /// <summary>
        /// Cut the mesh with a disc by removing intersected tetrahedra. Only allowed to be called from the PhysicsUpdate Event.
        /// </summary>
        /// <param name="cuttingDisc">The disc based on which to cut.</param>
        public void CutRemove(Disc cuttingDisc)
        {
            if (!MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld)
            {
                return;
            }

            
            
            
            Dictionary<(int, int), float> OVZYKFeGfeDBzPKNfVxeFPNapCqNmipn = CutUtilities.FindIntersections(cuttingDisc, MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj);

            if (OVZYKFeGfeDBzPKNfVxeFPNapCqNmipn.Count == 0)
            {
                return;
            }

            Parallel.For(0, SimulationMesh.RuntimeVolumeConstraintData.Count, (i) =>
            {
                var IsFkkDHywNbdgWIBTUeVnWRAYbkMECzv = SimulationMesh.RuntimeVolumeConstraintData.ParticleA[i];
                var wMgCSjOKPTdqcCWPAlfJfFBTeoKsFREW = SimulationMesh.RuntimeVolumeConstraintData.ParticleB[i];
                var qmmbnEQtTYEeLSsMyyVSOgbeutTnCNgK = SimulationMesh.RuntimeVolumeConstraintData.ParticleC[i];
                var ggnFwylqdZUSMBeDuEcGXhzgrtFUdDrG = SimulationMesh.RuntimeVolumeConstraintData.ParticleD[i];

                if (IsEdgeIntersected(IsFkkDHywNbdgWIBTUeVnWRAYbkMECzv, wMgCSjOKPTdqcCWPAlfJfFBTeoKsFREW) ||
                    IsEdgeIntersected(wMgCSjOKPTdqcCWPAlfJfFBTeoKsFREW, qmmbnEQtTYEeLSsMyyVSOgbeutTnCNgK) ||
                    IsEdgeIntersected(qmmbnEQtTYEeLSsMyyVSOgbeutTnCNgK, IsFkkDHywNbdgWIBTUeVnWRAYbkMECzv) ||
                    IsEdgeIntersected(IsFkkDHywNbdgWIBTUeVnWRAYbkMECzv, ggnFwylqdZUSMBeDuEcGXhzgrtFUdDrG) ||
                    IsEdgeIntersected(wMgCSjOKPTdqcCWPAlfJfFBTeoKsFREW, ggnFwylqdZUSMBeDuEcGXhzgrtFUdDrG) ||
                    IsEdgeIntersected(qmmbnEQtTYEeLSsMyyVSOgbeutTnCNgK, ggnFwylqdZUSMBeDuEcGXhzgrtFUdDrG))
                {
                    SimulationMesh.RuntimeVolumeConstraintData.Enabled[i] = false;
                    SimulationMesh.VolumeConstraintsEnabledStateHasChanged = true;
                }
            });

            bool IsEdgeIntersected(int p0, int p1)
            {
                return OVZYKFeGfeDBzPKNfVxeFPNapCqNmipn.ContainsKey(p0 < p1 ? (p0, p1) : (p1, p0));
            }
        }

        /// <summary>
        /// Uses the <paramref name="mesh"/> for generating UV Maps.
        /// </summary>
        /// <param name="mesh">The mesh to use UV maps from.</param>
        public void GenerateUVMapsFromMesh(Mesh mesh)
        {
            particlesUVs = new float2[SharedSimulationMesh.Particles.Length];
            var uAlUYDLXXnaDqMFZByZQfoIjIvFTvDFJ = mesh.vertices;
            var WPUbqbzwBpkcvxCMnvgqinViBEATTfcc = mesh.uv;

            if (mesh.uv == null || mesh.uv.Length < uAlUYDLXXnaDqMFZByZQfoIjIvFTvDFJ.Length)
            {
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("Error", $"The selected model has invalid UV mapping: {mesh.name}.", "Ok");
#endif
                return;
            }

            
            Parallel.For(0, SharedSimulationMesh.Particles.Length, (i) =>
            {
                float AzcqstlSSHSaEFVHvuvFXcIqDZNelYOd = float.MaxValue;
                var cMjDuZSagAdQdoHmZPsmuFenuNsWDEdw = SharedSimulationMesh.Particles[i].Position;

                Parallel.For(0, uAlUYDLXXnaDqMFZByZQfoIjIvFTvDFJ.Length, (j) =>
                {
                    var eyLOoPxgwZclHYnPEzEDKXLYCqrLcIgM = uAlUYDLXXnaDqMFZByZQfoIjIvFTvDFJ[j];

                    var WmSnwdGDCDyFZvBLvOHeVMIDoktLAjYW = math.distance(cMjDuZSagAdQdoHmZPsmuFenuNsWDEdw, eyLOoPxgwZclHYnPEzEDKXLYCqrLcIgM);

                    if (WmSnwdGDCDyFZvBLvOHeVMIDoktLAjYW < AzcqstlSSHSaEFVHvuvFXcIqDZNelYOd)
                    {
                        AzcqstlSSHSaEFVHvuvFXcIqDZNelYOd = WmSnwdGDCDyFZvBLvOHeVMIDoktLAjYW;
                        particlesUVs[i] = WPUbqbzwBpkcvxCMnvgqinViBEATTfcc[j];
                    }
                });
            });

            if (naqyjxEeJeBwvwAJGMkTUhtSXMNsokNV.IsCreated)
            {
                naqyjxEeJeBwvwAJGMkTUhtSXMNsokNV.CopyFrom(particlesUVs);
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            EditorUtility.DisplayDialog("UV Maps Generated", $"UV Maps have been successfully imported from the mesh {mesh.name}.", "Ok");

            
            yApxMRTxBietZtaqZgPMTPKmIijKuwmZ();
#endif
        }

        /// <inheritdoc/>
        protected override void PhysicsUpdate(float timeStep)
        {
            if (MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj == null || !MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld)
            {
                return;
            }

            if (UpdateKinematicParticles && transform.hasChanged)
            {
                JIOiucOEYRpIQeYzaWBwYzUTlHeVrWdH().Complete();
                transform.hasChanged = false;
            }
        }

        /// <inheritdoc/>
        protected override void LatePhysicsUpdate(float timeStep)
        {
            if (MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj == null || !MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.IsPartOfPhysicsWorld)
            {
                return;
            }

            
            if (RemoveVolumeConstraintsWithLimitedNeighbors && MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.VolumeConstraintsEnabledStateHasChanged)
            {
                eSIuasJrVPPUxfxbHZEFtqhZbJHwYJtR().Complete();
            }

            
            if (RemoveResidualDistanceConstraints && MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.VolumeConstraintsEnabledStateHasChanged)
            {
                hjKigGtdCVjFkaJysCnaRBAmgECLLyDe().Complete();
            }

            
            ZQwACbOLkFjDEUifuekasahCzexbGTVn = ZQwACbOLkFjDEUifuekasahCzexbGTVn || MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.VolumeConstraintsEnabledStateHasChanged;

            
            if (nmAUFdYsRQrMVtMRVbBKJAAFZczlAZtr && mNYomtnKnkQozVWHzuBEWMtzkgLkagUu.IsCreated)
            {
                if (mNYomtnKnkQozVWHzuBEWMtzkgLkagUu.Length == volumeConstraintsNeighborsSerialized.Length)
                {
                    mNYomtnKnkQozVWHzuBEWMtzkgLkagUu.CopyFrom(volumeConstraintsNeighborsSerialized);
                }
                else
                {
                    mNYomtnKnkQozVWHzuBEWMtzkgLkagUu.Dispose();
                    mNYomtnKnkQozVWHzuBEWMtzkgLkagUu = new NativeArray<int>(volumeConstraintsNeighborsSerialized, Allocator.Persistent);
                }

                nmAUFdYsRQrMVtMRVbBKJAAFZczlAZtr = false;
            }

            
            if (ZQwACbOLkFjDEUifuekasahCzexbGTVn)
            {
                cleIZvXgoQMKmHPWbJTpWvCbjEpZTCPI().Complete();

                zmraicaIfMMnuYkpgOnnsuSqDgPPsZjN();

                ZQwACbOLkFjDEUifuekasahCzexbGTVn = false;
            }

            
            else
            {
                BmNCleZamIMoZerXOIlHbbrocFJpcint().Complete();

                sAwWBTaedoVgXcTfMHCWMFUcFgPqxbsc();
            }
        }

        private uint kwTnoUNwUPBElUHMWdoNecvWOCmSBsYT()
        {
            var GQKiUaeqQuTVmtNThaYCIcFrglduDTPI = PhaseOptions.None;
            return PhysicsWorld.CreatePhase(Layer, GQKiUaeqQuTVmtNThaYCIcFrglduDTPI);
        }

        private void OJxTxcNNqJGpYsbmMgQncWNIZRbrZRsS()
        {
            if (SharedSimulationMesh == null)
            {
                volumeConstraintsNeighborsSerialized = new int[0];
                return;
            }

            var DgrpcCMtbHlCGyOfcexVfPZcmCPFdppI = SharedSimulationMesh.VolumeConstraints;
            int[] LRbWktQQfVAUtbBNAzARhDdlLvTrjPFP = new int[DgrpcCMtbHlCGyOfcexVfPZcmCPFdppI.Length * 4];

            for (int DfrzMNIRuKeFmddGKHZfJDRuIhFxpwYR = 0; DfrzMNIRuKeFmddGKHZfJDRuIhFxpwYR < DgrpcCMtbHlCGyOfcexVfPZcmCPFdppI.Length; DfrzMNIRuKeFmddGKHZfJDRuIhFxpwYR++)
            {
                LRbWktQQfVAUtbBNAzARhDdlLvTrjPFP[DfrzMNIRuKeFmddGKHZfJDRuIhFxpwYR * 4] = DgrpcCMtbHlCGyOfcexVfPZcmCPFdppI[DfrzMNIRuKeFmddGKHZfJDRuIhFxpwYR].ParticleA;
                LRbWktQQfVAUtbBNAzARhDdlLvTrjPFP[(DfrzMNIRuKeFmddGKHZfJDRuIhFxpwYR * 4) + 1] = DgrpcCMtbHlCGyOfcexVfPZcmCPFdppI[DfrzMNIRuKeFmddGKHZfJDRuIhFxpwYR].ParticleB;
                LRbWktQQfVAUtbBNAzARhDdlLvTrjPFP[(DfrzMNIRuKeFmddGKHZfJDRuIhFxpwYR * 4) + 2] = DgrpcCMtbHlCGyOfcexVfPZcmCPFdppI[DfrzMNIRuKeFmddGKHZfJDRuIhFxpwYR].ParticleC;
                LRbWktQQfVAUtbBNAzARhDdlLvTrjPFP[(DfrzMNIRuKeFmddGKHZfJDRuIhFxpwYR * 4) + 3] = DgrpcCMtbHlCGyOfcexVfPZcmCPFdppI[DfrzMNIRuKeFmddGKHZfJDRuIhFxpwYR].ParticleD;
            }

            volumeConstraintsNeighborsSerialized = MeshUtilities.FindTetrahedronNeighbors(LRbWktQQfVAUtbBNAzARhDdlLvTrjPFP);

            if (GenerateFacesInPhysicsMaterialBoundaries)
            {
                ZqCEQXiYrGULdwjZVWXJvoBUlqRtMsyC();
            }
        }

        private void EPpFXfFKinGVlMJXZvxnXTgmRtIaEVJa()
        {
            if (SharedSimulationMesh == null)
            {
                volumeConstraintsToDistanceConstraintsMapSerialized = new int[0];
                return;
            }

            var GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe = SharedSimulationMesh.VolumeConstraints;
            var avoUHPfqgKroWAuQOPOnVKgfXhNlghCo = SharedSimulationMesh.DistanceConstraints;

#if UNITY_EDITOR
            var WQAXqcBQycpGnjqYjKBDXDAemvAacYsQ = 0;
            var qqutdbFsgvGdDUPxnuUlKfQBUgXMFKEP = avoUHPfqgKroWAuQOPOnVKgfXhNlghCo.Length + GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe.Length;
            UnityEditor.EditorUtility.DisplayProgressBar("Loading", "[Allocating Memory] Calculating Volume Constraint to Distance Constraint mapping.", (float)WQAXqcBQycpGnjqYjKBDXDAemvAacYsQ / qqutdbFsgvGdDUPxnuUlKfQBUgXMFKEP);
#endif

            volumeConstraintsToDistanceConstraintsMapSerialized = null;
            System.GC.Collect();
            volumeConstraintsToDistanceConstraintsMapSerialized = new int[GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe.Length * 6];

            var BBQtGWloUXxbYBmlFcDrugDmTUwtrWeD = new Dictionary<(int, int), int>();

            for (int YaRetnENAnJxusfvWwgMEHYBmLpSTJhg = 0; YaRetnENAnJxusfvWwgMEHYBmLpSTJhg < avoUHPfqgKroWAuQOPOnVKgfXhNlghCo.Length; YaRetnENAnJxusfvWwgMEHYBmLpSTJhg++)
            {
                AddDistanceConstraintToIndexDict(avoUHPfqgKroWAuQOPOnVKgfXhNlghCo[YaRetnENAnJxusfvWwgMEHYBmLpSTJhg].ParticleA, avoUHPfqgKroWAuQOPOnVKgfXhNlghCo[YaRetnENAnJxusfvWwgMEHYBmLpSTJhg].ParticleB, YaRetnENAnJxusfvWwgMEHYBmLpSTJhg);

#if UNITY_EDITOR
                WQAXqcBQycpGnjqYjKBDXDAemvAacYsQ++;
                UnityEditor.EditorUtility.DisplayProgressBar("Loading", "Calculating Volume Constraint to Distance Constraint mapping.", (float)WQAXqcBQycpGnjqYjKBDXDAemvAacYsQ / qqutdbFsgvGdDUPxnuUlKfQBUgXMFKEP);
#endif
            }

            for (int NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK = 0; NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK < GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe.Length; NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK++)
            {
                int IrzfRnTkwesIDfUOTrjaiBvlnUkgOegu = NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK * 6;

                volumeConstraintsToDistanceConstraintsMapSerialized[IrzfRnTkwesIDfUOTrjaiBvlnUkgOegu] = GetDistanceConstraintIndexFromDict(GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe[NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK].ParticleA, GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe[NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK].ParticleB);
                volumeConstraintsToDistanceConstraintsMapSerialized[IrzfRnTkwesIDfUOTrjaiBvlnUkgOegu + 1] = GetDistanceConstraintIndexFromDict(GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe[NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK].ParticleB, GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe[NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK].ParticleC);
                volumeConstraintsToDistanceConstraintsMapSerialized[IrzfRnTkwesIDfUOTrjaiBvlnUkgOegu + 2] = GetDistanceConstraintIndexFromDict(GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe[NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK].ParticleC, GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe[NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK].ParticleA);
                volumeConstraintsToDistanceConstraintsMapSerialized[IrzfRnTkwesIDfUOTrjaiBvlnUkgOegu + 3] = GetDistanceConstraintIndexFromDict(GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe[NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK].ParticleA, GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe[NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK].ParticleD);
                volumeConstraintsToDistanceConstraintsMapSerialized[IrzfRnTkwesIDfUOTrjaiBvlnUkgOegu + 4] = GetDistanceConstraintIndexFromDict(GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe[NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK].ParticleB, GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe[NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK].ParticleD);
                volumeConstraintsToDistanceConstraintsMapSerialized[IrzfRnTkwesIDfUOTrjaiBvlnUkgOegu + 5] = GetDistanceConstraintIndexFromDict(GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe[NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK].ParticleC, GBINRPkSoFNmgvAdLVeWJSciVHAVnqwe[NMqwFTAtKDbqXiBplJLNTkmWZCqlQUDK].ParticleD);

#if UNITY_EDITOR
                WQAXqcBQycpGnjqYjKBDXDAemvAacYsQ++;
                UnityEditor.EditorUtility.DisplayProgressBar("Loading", "Calculating Volume Constraint to Distance Constraint mapping.", (float)WQAXqcBQycpGnjqYjKBDXDAemvAacYsQ / qqutdbFsgvGdDUPxnuUlKfQBUgXMFKEP);
#endif
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.ClearProgressBar();
#endif

            void AddDistanceConstraintToIndexDict(int particleAIndex, int particleBIndex, int index)
            {
                BBQtGWloUXxbYBmlFcDrugDmTUwtrWeD[particleAIndex < particleBIndex ? (particleAIndex, particleBIndex) : (particleBIndex, particleAIndex)] = index;
            }

            int GetDistanceConstraintIndexFromDict(int particleAIndex, int particleBIndex)
            {
                var qbKxMYFLIOIHBOwXEBqSbSkJbDfULeXV = particleAIndex < particleBIndex ? (particleAIndex, particleBIndex) : (particleBIndex, particleAIndex);
                if (!BBQtGWloUXxbYBmlFcDrugDmTUwtrWeD.ContainsKey(qbKxMYFLIOIHBOwXEBqSbSkJbDfULeXV))
                {
                    return -1;
                }

                return BBQtGWloUXxbYBmlFcDrugDmTUwtrWeD[qbKxMYFLIOIHBOwXEBqSbSkJbDfULeXV];
            }
        }

        private void ZqCEQXiYrGULdwjZVWXJvoBUlqRtMsyC()
        {
            var gceIvDSLhZGMvLInLjsQeNnPgieWlbGo = SharedSimulationMesh.VolumeConstraints;
            var UCeuidhWFUgkvzkIQOXUyiIxaSUAgbhO = new NativeArray<int>(volumeConstraintsNeighborsSerialized.Length, Allocator.TempJob);
            UCeuidhWFUgkvzkIQOXUyiIxaSUAgbhO.CopyFrom(volumeConstraintsNeighborsSerialized);
            var UNFhLkRQZSCqgNVuhcZdEVqFseleuqsO = new NativeArray<int>(gceIvDSLhZGMvLInLjsQeNnPgieWlbGo.Length, Allocator.TempJob);
            for (int VVhrYEjqjklFlDBgitmYBZCvwAkWCTZx = 0; VVhrYEjqjklFlDBgitmYBZCvwAkWCTZx < gceIvDSLhZGMvLInLjsQeNnPgieWlbGo.Length; VVhrYEjqjklFlDBgitmYBZCvwAkWCTZx++)
            {
                UNFhLkRQZSCqgNVuhcZdEVqFseleuqsO[VVhrYEjqjklFlDBgitmYBZCvwAkWCTZx] = gceIvDSLhZGMvLInLjsQeNnPgieWlbGo[VVhrYEjqjklFlDBgitmYBZCvwAkWCTZx].PhysicsMaterialIndex;
            }

            var UpqBeqyaMskHKkkjoonxJXaScYvXOlfN = new BwNSNQtubLjxpfsKVPPvzgDGZoNQwaPw
            {
                TetrahedralMeshNeighbors = UCeuidhWFUgkvzkIQOXUyiIxaSUAgbhO,
                VolumeConstraintsPhysicsMaterialIndex = UNFhLkRQZSCqgNVuhcZdEVqFseleuqsO,
            };

            UpqBeqyaMskHKkkjoonxJXaScYvXOlfN.Schedule(gceIvDSLhZGMvLInLjsQeNnPgieWlbGo.Length, 64).Complete();

            UCeuidhWFUgkvzkIQOXUyiIxaSUAgbhO.CopyTo(volumeConstraintsNeighborsSerialized);

            UCeuidhWFUgkvzkIQOXUyiIxaSUAgbhO.Dispose();
            UNFhLkRQZSCqgNVuhcZdEVqFseleuqsO.Dispose();
        }

        private void wFObStqphDMqEwDCJKlgcFHvAaARiIBM()
        {
            if (IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK == null)
            {
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK = new Mesh();
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.name = gameObject.name + "_SoftbodyMesh";
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.MarkDynamic();
                GetComponent<MeshFilter>().mesh = IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK;
            }
        }

        private void zmraicaIfMMnuYkpgOnnsuSqDgPPsZjN()
        {
            unsafe
            {
                wFObStqphDMqEwDCJKlgcFHvAaARiIBM();
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.Clear();
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.SetVertices(gmvsOoSNlzvldXnFKbascWeOYlHGQDJj, 0, *vshbsmUQUkdIlduBcqgttBUdXjBaDJGO.Counter);
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.SetUVs(0, gPqKLmKQVBdWfvxYAGmVdGewjlwfDqqX, 0, *vshbsmUQUkdIlduBcqgttBUdXjBaDJGO.Counter);
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.subMeshCount = gylEmSZdIEXLRCVwCFVmFRheZUaANiux.DataCount;

                for (int qcLFBBQhilavSQwkQygGswpdfdISQvuA = 0; qcLFBBQhilavSQwkQygGswpdfdISQvuA < gylEmSZdIEXLRCVwCFVmFRheZUaANiux.DataCount; qcLFBBQhilavSQwkQygGswpdfdISQvuA++)
                {
                    kVpvotvXxBdwCpONVPtDFVRBZFdnGOht TdomKOmRboSsaPGvXYAPaBxFcxAdAXml = gylEmSZdIEXLRCVwCFVmFRheZUaANiux.Datas[qcLFBBQhilavSQwkQygGswpdfdISQvuA];
                    IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.SetIndices(TdomKOmRboSsaPGvXYAPaBxFcxAdAXml.Indices, 0, *TdomKOmRboSsaPGvXYAPaBxFcxAdAXml.IndexCount.Counter, MeshTopology.Triangles, qcLFBBQhilavSQwkQygGswpdfdISQvuA);
                }

                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.RecalculateNormals();
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.RecalculateBounds();
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.RecalculateTangents();
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.MarkModified();
            }
        }

        private void sAwWBTaedoVgXcTfMHCWMFUcFgPqxbsc()
        {
            unsafe
            {
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.SetVertices(gmvsOoSNlzvldXnFKbascWeOYlHGQDJj, 0, *vshbsmUQUkdIlduBcqgttBUdXjBaDJGO.Counter);
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.RecalculateNormals();
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.RecalculateBounds();
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.RecalculateTangents();
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.MarkModified();
            }
        }

        private void UmsxBhiAByMTfiqkJZIhEzfAnIqTJTvv()
        {
            if (SimulationMesh == null)
            {
                return;
            }

            mNYomtnKnkQozVWHzuBEWMtzkgLkagUu = new NativeArray<int>(volumeConstraintsNeighborsSerialized, Allocator.Persistent);
            ailGOIZpwKQqqnvaaPIQRPlGUsnNhgxN = new NativeArray<int>(volumeConstraintsToDistanceConstraintsMapSerialized, Allocator.Persistent);
            cyszmULnGziSnFsyxccoCTfbnmefpwhD = new NativeArray<bool>(SimulationMesh.DistanceConstraints.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            unsafe
            {
                int* eIWaWlFrqbTgPrznySNDDlKkKuCjiDAA = (int*)UnsafeUtility.Malloc(sizeof(int), 4, Allocator.Persistent);
                vshbsmUQUkdIlduBcqgttBUdXjBaDJGO = new UnsafeAtomicCounter32(eIWaWlFrqbTgPrznySNDDlKkKuCjiDAA);
            }

            int ClqdzHcWIBUAwLPHvDDSkqWxoUWaDKUO = SharedSimulationMesh.VolumeConstraints.Length * 4 * 3;
            gmvsOoSNlzvldXnFKbascWeOYlHGQDJj = new NativeArray<float3>(ClqdzHcWIBUAwLPHvDDSkqWxoUWaDKUO, Allocator.Persistent);
            gPqKLmKQVBdWfvxYAGmVdGewjlwfDqqX = new NativeArray<float2>(ClqdzHcWIBUAwLPHvDDSkqWxoUWaDKUO, Allocator.Persistent);
            WeieAgoBIRlepuzFFwFcGAmQNWbjBIkb = new NativeArray<int>(ClqdzHcWIBUAwLPHvDDSkqWxoUWaDKUO, Allocator.Persistent);
            naqyjxEeJeBwvwAJGMkTUhtSXMNsokNV = new NativeArray<float2>(particlesUVs, Allocator.Persistent);

            
            var oLvPQemsWIJMgMNjSIsjzugvbWFHdBhw = SharedSimulationMesh.PhysicsMaterials.Length;
            gylEmSZdIEXLRCVwCFVmFRheZUaANiux = new kNfsIJiAeHdUsHBnINcLfwfDHElTSnIa(oLvPQemsWIJMgMNjSIsjzugvbWFHdBhw);
            for (int RSFLtqBqJAUifYINUJQavBLiRKkXHiqC = 0; RSFLtqBqJAUifYINUJQavBLiRKkXHiqC < oLvPQemsWIJMgMNjSIsjzugvbWFHdBhw; RSFLtqBqJAUifYINUJQavBLiRKkXHiqC++)
            {
                int axKaUgEWIStUcxYvRkUVxMkFNlGREbmk = SharedSimulationMesh.VolumeConstraints.Length * 4; 

                unsafe
                {
                    gylEmSZdIEXLRCVwCFVmFRheZUaANiux.Datas[RSFLtqBqJAUifYINUJQavBLiRKkXHiqC] = new kVpvotvXxBdwCpONVPtDFVRBZFdnGOht(axKaUgEWIStUcxYvRkUVxMkFNlGREbmk);
                }
            }

            var BNPlbrQvPHzhNjiBoMLWtcpsNhESHjJV = SharedSimulationMesh.Particles.Length;
            TzjttcWQKxXOBfvzCQMxFpxNKwEelrlb = new NativeArray<int>(BNPlbrQvPHzhNjiBoMLWtcpsNhESHjJV, Allocator.Persistent);
            QlFngOMddQelgUFvRiDVsuqnokGsjbbJ = new NativeArray<int>(BNPlbrQvPHzhNjiBoMLWtcpsNhESHjJV, Allocator.Persistent);
            SRBTLwtxPhkZIeaGzKJVUurjiROqlotk = new NativeArray<int>(BNPlbrQvPHzhNjiBoMLWtcpsNhESHjJV, Allocator.Persistent);
        }

        private void fGEQJiKhwAwzIUjrbeELbWfGBLnbByfC()
        {
            unsafe
            {
                if (vshbsmUQUkdIlduBcqgttBUdXjBaDJGO.Counter != null)
                {
                    UnsafeUtility.Free(vshbsmUQUkdIlduBcqgttBUdXjBaDJGO.Counter, Allocator.Persistent);
                }
            }

            if (mNYomtnKnkQozVWHzuBEWMtzkgLkagUu.IsCreated)
            {
                mNYomtnKnkQozVWHzuBEWMtzkgLkagUu.Dispose();
            }

            if (ailGOIZpwKQqqnvaaPIQRPlGUsnNhgxN.IsCreated)
            {
                ailGOIZpwKQqqnvaaPIQRPlGUsnNhgxN.Dispose();
            }

            if (cyszmULnGziSnFsyxccoCTfbnmefpwhD.IsCreated)
            {
                cyszmULnGziSnFsyxccoCTfbnmefpwhD.Dispose();
            }

            if (gmvsOoSNlzvldXnFKbascWeOYlHGQDJj.IsCreated)
            {
                gmvsOoSNlzvldXnFKbascWeOYlHGQDJj.Dispose();
            }

            if (gPqKLmKQVBdWfvxYAGmVdGewjlwfDqqX.IsCreated)
            {
                gPqKLmKQVBdWfvxYAGmVdGewjlwfDqqX.Dispose();
            }

            if (WeieAgoBIRlepuzFFwFcGAmQNWbjBIkb.IsCreated)
            {
                WeieAgoBIRlepuzFFwFcGAmQNWbjBIkb.Dispose();
            }

            if (TzjttcWQKxXOBfvzCQMxFpxNKwEelrlb.IsCreated)
            {
                TzjttcWQKxXOBfvzCQMxFpxNKwEelrlb.Dispose();
            }

            if (naqyjxEeJeBwvwAJGMkTUhtSXMNsokNV.IsCreated)
            {
                naqyjxEeJeBwvwAJGMkTUhtSXMNsokNV.Dispose();
            }

            if (QlFngOMddQelgUFvRiDVsuqnokGsjbbJ.IsCreated)
            {
                QlFngOMddQelgUFvRiDVsuqnokGsjbbJ.Dispose();
            }

            if (SRBTLwtxPhkZIeaGzKJVUurjiROqlotk.IsCreated)
            {
                SRBTLwtxPhkZIeaGzKJVUurjiROqlotk.Dispose();
            }

            if (gylEmSZdIEXLRCVwCFVmFRheZUaANiux != null)
            {
                for (int mnBVRSOduogJijAeevZqUUGfgqKQuMOV = 0; mnBVRSOduogJijAeevZqUUGfgqKQuMOV < gylEmSZdIEXLRCVwCFVmFRheZUaANiux.DataCount; mnBVRSOduogJijAeevZqUUGfgqKQuMOV++)
                {
                    gylEmSZdIEXLRCVwCFVmFRheZUaANiux.GetDataAt(mnBVRSOduogJijAeevZqUUGfgqKQuMOV).Dispose();
                }

                gylEmSZdIEXLRCVwCFVmFRheZUaANiux.Dispose();
                gylEmSZdIEXLRCVwCFVmFRheZUaANiux = null;
            }
        }

        private JobHandle hjKigGtdCVjFkaJysCnaRBAmgECLLyDe(JobHandle dependsOn = default)
        {
            var sFUupazNALcmjMyevhQRVeoklYbNQsFo = new InitializeBufferJob<bool>
            {
                Buffer = cyszmULnGziSnFsyxccoCTfbnmefpwhD,
                Value = true,
            };

            var xCxovcjBANWiaXkJWwUynRfmnNBaPOAU = new SDHqFwtlTLLPwPfGhNNHQgosjAutSHYJ
            {
                VolumeConstraintsEnabled = SimulationMesh.RuntimeVolumeConstraintData.Enabled,
                VolumeConstraintsToDistanceConstraintsIndex = ailGOIZpwKQqqnvaaPIQRPlGUsnNhgxN,
                ResidualDistanceConstraints = cyszmULnGziSnFsyxccoCTfbnmefpwhD,
            };

            var QuvjNhgBUgPoDdDjSKaGdZaIIuvnlNfh = new LxhLSXEheDAdxPVZzNYtHYiQCKhCRRhB
            {
                DistanceConstraintsEnabled = SimulationMesh.RuntimeDistanceConstraintData.Enabled,
                DistanceConstraintsEnabledStateHasChanged = SimulationMesh.PhysicsWorld.SimulationMeshDescriptorData.DistanceConstraintsEnabledStateHasChanged.Slice(SimulationMesh.SimulationMeshDescriptorIndex, 1),
                ResidualDistanceConstraints = cyszmULnGziSnFsyxccoCTfbnmefpwhD,
            };

            dependsOn = sFUupazNALcmjMyevhQRVeoklYbNQsFo.Schedule(SimulationMesh.RuntimeDistanceConstraintData.Count, 64, dependsOn);
            dependsOn = xCxovcjBANWiaXkJWwUynRfmnNBaPOAU.Schedule(SimulationMesh.RuntimeVolumeConstraintData.Count, 64, dependsOn);
            dependsOn = QuvjNhgBUgPoDdDjSKaGdZaIIuvnlNfh.Schedule(SimulationMesh.DistanceConstraints.Length, 64, dependsOn);

            return dependsOn;
        }

        private JobHandle eSIuasJrVPPUxfxbHZEFtqhZbJHwYJtR(JobHandle dependsOn = default)
        {
            var mUZvJBbdnNXbyEgIGogEzeNOnzkCaoKO = new soQquvlZKtSufDGLYySIiSFbEnbjrRLC
            {
                MinNeighborCount = MinNeighborsBeforeVolumeConstraintsRemoval,
                VolumeConstraintsEnabled = SimulationMesh.RuntimeVolumeConstraintData.Enabled,
                VolumeConstraintsNeighbors = mNYomtnKnkQozVWHzuBEWMtzkgLkagUu,
                VolumeConstraintsEnabledStateHasChanged = SimulationMesh.PhysicsWorld.SimulationMeshDescriptorData.VolumeConstraintsEnabledStateHasChanged.Slice(SimulationMesh.SimulationMeshDescriptorIndex, 1),
            };

            return mUZvJBbdnNXbyEgIGogEzeNOnzkCaoKO.Schedule(SimulationMesh.RuntimeVolumeConstraintData.Count, 64, dependsOn);
        }

        private JobHandle cleIZvXgoQMKmHPWbJTpWvCbjEpZTCPI(JobHandle dependsOn = default)
        {
            JobHandle DispatchRecreateFlatMeshJob(JobHandle dependsOn = default)
            {
                unsafe
                {
                    var YRmmPexvIqmYfrubiLiZnfGrsrLIfajQ = new gjEBrvkrhmgzYTzfqsSGOztFyPFCHOgr
                    {
                        VertexCount = vshbsmUQUkdIlduBcqgttBUdXjBaDJGO,
                        SubMeshIndexDataArray = gylEmSZdIEXLRCVwCFVmFRheZUaANiux.Datas,
                        SubMeshIndexDataArrayLength = gylEmSZdIEXLRCVwCFVmFRheZUaANiux.DataCount,
                    };

                    var nusjPrTbKszAWwqBPLcLdEogufTIGzMu = new BakiPyhfGLoQRsNyCovenLNJyvxSIthg
                    {
                        WorldToLocalMatrix = transform.worldToLocalMatrix,

                        
                        VolumeConstraintsNeighbors = mNYomtnKnkQozVWHzuBEWMtzkgLkagUu,

                        
                        ParticlesPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position,

                        
                        ParticlesUVs = naqyjxEeJeBwvwAJGMkTUhtSXMNsokNV,

                        
                        VolumeConstraintsEnabled = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.Enabled,
                        VolumeConstraintsParticleA = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleA,
                        VolumeConstraintsParticleB = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleB,
                        VolumeConstraintsParticleC = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleC,
                        VolumeConstraintsParticleD = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleD,
                        VolumeConstraintsSubMeshIndex = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.PhysicsMaterialIndex,

                        
                        VertexCount = vshbsmUQUkdIlduBcqgttBUdXjBaDJGO,
                        Vertices = gmvsOoSNlzvldXnFKbascWeOYlHGQDJj,
                        UVs = gPqKLmKQVBdWfvxYAGmVdGewjlwfDqqX,
                        BindInfo = WeieAgoBIRlepuzFFwFcGAmQNWbjBIkb,

                        SubMeshIndexDataArray = gylEmSZdIEXLRCVwCFVmFRheZUaANiux.Datas,
                        SubMeshIndexDataArrayLength = gylEmSZdIEXLRCVwCFVmFRheZUaANiux.DataCount,
                    };

                    dependsOn = YRmmPexvIqmYfrubiLiZnfGrsrLIfajQ.Schedule(dependsOn);
                    dependsOn = nusjPrTbKszAWwqBPLcLdEogufTIGzMu.Schedule(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.Count, 64, dependsOn);

                    return dependsOn;
                }
            }

            JobHandle DispatchRecreateSmoothMeshJob(JobHandle dependsOn = default)
            {
                unsafe
                {
                    var wEdSJCoSQyMTBzovDxgNDIMYULjZPFFP = new gjEBrvkrhmgzYTzfqsSGOztFyPFCHOgr
                    {
                        VertexCount = vshbsmUQUkdIlduBcqgttBUdXjBaDJGO,
                        SubMeshIndexDataArray = gylEmSZdIEXLRCVwCFVmFRheZUaANiux.Datas,
                        SubMeshIndexDataArrayLength = gylEmSZdIEXLRCVwCFVmFRheZUaANiux.DataCount,
                    };

                    dependsOn = wEdSJCoSQyMTBzovDxgNDIMYULjZPFFP.Schedule(dependsOn);

                    var qsNyAhDQarpFzwBjKuBDJOErROuMnyyD = new ODoqVMMbxYpeSCvumTSirDBLrjlMvJlX
                    {
                        ParticleToVertexMap = TzjttcWQKxXOBfvzCQMxFpxNKwEelrlb,
                    };

                    dependsOn = qsNyAhDQarpFzwBjKuBDJOErROuMnyyD.Schedule(TzjttcWQKxXOBfvzCQMxFpxNKwEelrlb.Length, 64, dependsOn);

                    var jFJfYsdZRIFJslcZtCaoRahTGiFBpzBw = new TYlPRKsIijeEaHaIzUmYeUBwFQeeuAfJ
                    {
                        WorldToLocalMatrix = transform.worldToLocalMatrix,

                        

                        
                        ParticlesPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position,
                        ParticlesUVs = naqyjxEeJeBwvwAJGMkTUhtSXMNsokNV,

                        
                        VolumeConstraintsEnabled = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.Enabled,
                        VolumeConstraintsParticleA = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleA,
                        VolumeConstraintsParticleB = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleB,
                        VolumeConstraintsParticleC = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleC,
                        VolumeConstraintsParticleD = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.ParticleD,
                        VolumeConstraintsPhysicsMaterialIndex = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.PhysicsMaterialIndex,

                        
                        VertexCount = vshbsmUQUkdIlduBcqgttBUdXjBaDJGO,
                        Vertices = gmvsOoSNlzvldXnFKbascWeOYlHGQDJj,
                        UVs = gPqKLmKQVBdWfvxYAGmVdGewjlwfDqqX,
                        BindInfo = WeieAgoBIRlepuzFFwFcGAmQNWbjBIkb,
                        VolumeConstraintsNeighbors = mNYomtnKnkQozVWHzuBEWMtzkgLkagUu,

                        SubMeshIndexDataArray = gylEmSZdIEXLRCVwCFVmFRheZUaANiux.Datas,
                        SubMeshIndexDataArrayLength = gylEmSZdIEXLRCVwCFVmFRheZUaANiux.DataCount,

                        
                        ParticleToVertexMap = TzjttcWQKxXOBfvzCQMxFpxNKwEelrlb,
                        VertexLocks = SRBTLwtxPhkZIeaGzKJVUurjiROqlotk,
                    };

                    return jFJfYsdZRIFJslcZtCaoRahTGiFBpzBw.Schedule(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeVolumeConstraintData.Count, 64, dependsOn);
                }
            }

            if (SmoothShading)
            {
                return DispatchRecreateSmoothMeshJob(dependsOn);
            }
            else
            {
                return DispatchRecreateFlatMeshJob(dependsOn);
            }
        }

        private JobHandle BmNCleZamIMoZerXOIlHbbrocFJpcint(JobHandle dependsOn = default)
        {
            var sRtHedXSyQyVSKnwtrZnPyJtLDSOrSwZ = new zUMipoJVZMiXeMPLJFnpaVweFVagqump
            {
                WorldToLocalMatrix = transform.worldToLocalMatrix,
                VertexBindings = WeieAgoBIRlepuzFFwFcGAmQNWbjBIkb,
                Vertices = gmvsOoSNlzvldXnFKbascWeOYlHGQDJj,
                ParticlesPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position,
            };

            unsafe
            {
                return sRtHedXSyQyVSKnwtrZnPyJtLDSOrSwZ.Schedule(*vshbsmUQUkdIlduBcqgttBUdXjBaDJGO.Counter, 64, dependsOn);
            }
        }

        private JobHandle JIOiucOEYRpIQeYzaWBwYzUTlHeVrWdH(JobHandle dependsOn = default)
        {
            var OBxXnvKAgVraFJDwLDKoGtKeyYlhAeGg = new nOfUTIIRRQqsZjKPltWRueYzjbOjuJry
            {
                InitialWorldToLocalMatrix = UkRjRxYGOtwGbecJiWnUmGphlFZZmzNg,
                LocalToWorldMatrix = transform.localToWorldMatrix,

                ParticlesInitialPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InitialPosition,
                ParticlesPosition = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Position,
                ParticlesInverseMass = MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.InverseMass,
            };

            return OBxXnvKAgVraFJDwLDKoGtKeyYlhAeGg.Schedule(MUstXBTdvUqjBlUBBQOdhRkcMiqhfOsj.RuntimeParticleData.Count, 64, dependsOn);
        }

        private void yApxMRTxBietZtaqZgPMTPKmIijKuwmZ()
        {
            if (SharedSimulationMesh == null)
            {
                wFObStqphDMqEwDCJKlgcFHvAaARiIBM();
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.Clear();
                return;
            }

            unsafe
            {
                
                var OjPYytoYHlicQMSAyRiQrHqfLaXGHSyo = SharedSimulationMesh.Particles;
                var XRrVtwMcnMBDsaQVvSxZshhFWzFZCTyH = OjPYytoYHlicQMSAyRiQrHqfLaXGHSyo.Length;
                NativeArray<float3> WSeNPORYvDogdhHFowHdAAlvgvOsixnH = new NativeArray<float3>(XRrVtwMcnMBDsaQVvSxZshhFWzFZCTyH, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                NativeArray<float2> rNzAouVuobRcTvvhYFaTunIGIFNrjkrH = new NativeArray<float2>(particlesUVs, Allocator.TempJob);

                var DjHJTVZVeTZOxIASmqlbTqfkMJGKPFWb = SharedSimulationMesh.VolumeConstraints;
                var lhVcwdeMFOsOciGfEacCCtHLNtATCLGB = DjHJTVZVeTZOxIASmqlbTqfkMJGKPFWb.Length;
                NativeArray<bool> pvjJblVZcYeKDbqPbHvBwrzIJptWJTbH = new NativeArray<bool>(lhVcwdeMFOsOciGfEacCCtHLNtATCLGB, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                NativeArray<int> EDUqmwJByJHaDtEVuBmUsyKFdXcfieMQ = new NativeArray<int>(lhVcwdeMFOsOciGfEacCCtHLNtATCLGB, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                NativeArray<int> UrImtSFGEuBYJStpANejQrzjaHijXHxH = new NativeArray<int>(lhVcwdeMFOsOciGfEacCCtHLNtATCLGB, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                NativeArray<int> qTkskZTbFEDLhILaNPuukluGDJZJFMQW = new NativeArray<int>(lhVcwdeMFOsOciGfEacCCtHLNtATCLGB, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                NativeArray<int> KPTFvGCQHLINIRmTxZozFvfSdpqxsWhb = new NativeArray<int>(lhVcwdeMFOsOciGfEacCCtHLNtATCLGB, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                NativeArray<int> UtunoCWkYuSbtYFfaHqsfgKirSgZFuYp = new NativeArray<int>(lhVcwdeMFOsOciGfEacCCtHLNtATCLGB, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

                var xhzRnjYLKlWWqMaAkFMDXGnXZXyCRVxl = transform.localToWorldMatrix;

                
                Parallel.For(0, XRrVtwMcnMBDsaQVvSxZshhFWzFZCTyH, i =>
                {
                    WSeNPORYvDogdhHFowHdAAlvgvOsixnH[i] = xhzRnjYLKlWWqMaAkFMDXGnXZXyCRVxl.MultiplyPoint3x4(OjPYytoYHlicQMSAyRiQrHqfLaXGHSyo[i].Position);
                });

                Parallel.For(0, lhVcwdeMFOsOciGfEacCCtHLNtATCLGB, i =>
                {
                    pvjJblVZcYeKDbqPbHvBwrzIJptWJTbH[i] = DjHJTVZVeTZOxIASmqlbTqfkMJGKPFWb[i].Enabled;
                    EDUqmwJByJHaDtEVuBmUsyKFdXcfieMQ[i] = DjHJTVZVeTZOxIASmqlbTqfkMJGKPFWb[i].ParticleA;
                    UrImtSFGEuBYJStpANejQrzjaHijXHxH[i] = DjHJTVZVeTZOxIASmqlbTqfkMJGKPFWb[i].ParticleB;
                    qTkskZTbFEDLhILaNPuukluGDJZJFMQW[i] = DjHJTVZVeTZOxIASmqlbTqfkMJGKPFWb[i].ParticleC;
                    KPTFvGCQHLINIRmTxZozFvfSdpqxsWhb[i] = DjHJTVZVeTZOxIASmqlbTqfkMJGKPFWb[i].ParticleD;
                    UtunoCWkYuSbtYFfaHqsfgKirSgZFuYp[i] = DjHJTVZVeTZOxIASmqlbTqfkMJGKPFWb[i].PhysicsMaterialIndex;
                });

                
                NativeArray<int> YloHjXCOPrTYBXFiUJZLdZeyXtoxeEgw = new NativeArray<int>(volumeConstraintsNeighborsSerialized, Allocator.TempJob);

                
                int nQLoeWvuuZceaulwHJubUxVVRwaMXUIc = DjHJTVZVeTZOxIASmqlbTqfkMJGKPFWb.Length * 4 * 4 * 3; 
                int* rDiaXhKYpHHaJoayZgyzahvJmRMghOjK = (int*)UnsafeUtility.Malloc(sizeof(int), 4, Allocator.Persistent);
                var GtdPlWeRTHLTJDKKyeRgifjExreBPmmV = new UnsafeAtomicCounter32(rDiaXhKYpHHaJoayZgyzahvJmRMghOjK);
                NativeArray<float3> KsWcPQTNvRutSVNnImWWJWoGqgockKzh = new NativeArray<float3>(nQLoeWvuuZceaulwHJubUxVVRwaMXUIc, Allocator.TempJob);
                NativeArray<float2> NLsAsxPlkqZNCFqWkTPBdcLGZpOsfjeR = new NativeArray<float2>(nQLoeWvuuZceaulwHJubUxVVRwaMXUIc, Allocator.TempJob);
                NativeArray<float3> DOELbcMWSsdzELrAVHlQQecKrgkDzkTF = new NativeArray<float3>(nQLoeWvuuZceaulwHJubUxVVRwaMXUIc, Allocator.TempJob);
                NativeArray<int> CxHkmdYhnpaEDvAgIPbPDNHZJvJltbgf = new NativeArray<int>(nQLoeWvuuZceaulwHJubUxVVRwaMXUIc, Allocator.Persistent);

                
                var oCOgpRmZsFerKfrCtGPoAqgPmkmqrSvH = SharedSimulationMesh.PhysicsMaterials.Length;
                var LCzprxhUoALMDZNRjZPjYtMdNknhXWMC = new kNfsIJiAeHdUsHBnINcLfwfDHElTSnIa(oCOgpRmZsFerKfrCtGPoAqgPmkmqrSvH);
                for (int FJukcmARkLZshveTwGryoUvsUDEmcRvi = 0; FJukcmARkLZshveTwGryoUvsUDEmcRvi < oCOgpRmZsFerKfrCtGPoAqgPmkmqrSvH; FJukcmARkLZshveTwGryoUvsUDEmcRvi++)
                {
                    int GHfgyVTAtPPenvscbvLHPuKKoOHHlvyn = SharedSimulationMesh.VolumeConstraints.Length * 4; 

                    unsafe
                    {
                        LCzprxhUoALMDZNRjZPjYtMdNknhXWMC.Datas[FJukcmARkLZshveTwGryoUvsUDEmcRvi] = new kVpvotvXxBdwCpONVPtDFVRBZFdnGOht(GHfgyVTAtPPenvscbvLHPuKKoOHHlvyn);
                    }
                }

                
                var yIxtAEUEHvggkarsriomwjXsgPiPseES = new NativeArray<int>(XRrVtwMcnMBDsaQVvSxZshhFWzFZCTyH, Allocator.TempJob);
                var fkuQJbFKYgcqoqXOkrKhEIXAYMYWAoaq = new NativeArray<int>(XRrVtwMcnMBDsaQVvSxZshhFWzFZCTyH, Allocator.TempJob);
                var XiZEpObTIDeEyDVNDpkaTCqZqRsfwxPz = new NativeArray<int>(XRrVtwMcnMBDsaQVvSxZshhFWzFZCTyH, Allocator.TempJob);

                
                if (SmoothShading)
                {
                    DispatchRecreateSmoothMeshJob().Complete();
                }
                else
                {
                    DispatchRecreateFlatMeshJob().Complete();
                }

                
                wFObStqphDMqEwDCJKlgcFHvAaARiIBM();
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.Clear();
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.SetVertices(KsWcPQTNvRutSVNnImWWJWoGqgockKzh, 0, *GtdPlWeRTHLTJDKKyeRgifjExreBPmmV.Counter);
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.SetUVs(0, NLsAsxPlkqZNCFqWkTPBdcLGZpOsfjeR, 0, *GtdPlWeRTHLTJDKKyeRgifjExreBPmmV.Counter);

                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.subMeshCount = LCzprxhUoALMDZNRjZPjYtMdNknhXWMC.DataCount;

                for (int NGxhBdruhDHDNXkbvnYQIVLtIhDtygvz = 0; NGxhBdruhDHDNXkbvnYQIVLtIhDtygvz < LCzprxhUoALMDZNRjZPjYtMdNknhXWMC.DataCount; NGxhBdruhDHDNXkbvnYQIVLtIhDtygvz++)
                {
                    kVpvotvXxBdwCpONVPtDFVRBZFdnGOht DlItHKNLDHAFjEwpQEKlgifYXOgCnjTX = LCzprxhUoALMDZNRjZPjYtMdNknhXWMC.Datas[NGxhBdruhDHDNXkbvnYQIVLtIhDtygvz];
                    IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.SetIndices(DlItHKNLDHAFjEwpQEKlgifYXOgCnjTX.Indices, 0, *DlItHKNLDHAFjEwpQEKlgifYXOgCnjTX.IndexCount.Counter, MeshTopology.Triangles, NGxhBdruhDHDNXkbvnYQIVLtIhDtygvz);
                }

                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.RecalculateNormals();
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.RecalculateBounds();
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.RecalculateTangents();
                IVRJdgvFgtNjgLIlyrGsLhSBMlZTTKiK.MarkModified();

                
                WSeNPORYvDogdhHFowHdAAlvgvOsixnH.Dispose();
                rNzAouVuobRcTvvhYFaTunIGIFNrjkrH.Dispose();
                pvjJblVZcYeKDbqPbHvBwrzIJptWJTbH.Dispose();
                EDUqmwJByJHaDtEVuBmUsyKFdXcfieMQ.Dispose();
                UrImtSFGEuBYJStpANejQrzjaHijXHxH.Dispose();
                qTkskZTbFEDLhILaNPuukluGDJZJFMQW.Dispose();
                KPTFvGCQHLINIRmTxZozFvfSdpqxsWhb.Dispose();
                UtunoCWkYuSbtYFfaHqsfgKirSgZFuYp.Dispose();
                YloHjXCOPrTYBXFiUJZLdZeyXtoxeEgw.Dispose();

                if (GtdPlWeRTHLTJDKKyeRgifjExreBPmmV.Counter != null)
                {
                    UnsafeUtility.Free(GtdPlWeRTHLTJDKKyeRgifjExreBPmmV.Counter, Allocator.Persistent);
                }

                KsWcPQTNvRutSVNnImWWJWoGqgockKzh.Dispose();
                DOELbcMWSsdzELrAVHlQQecKrgkDzkTF.Dispose();
                NLsAsxPlkqZNCFqWkTPBdcLGZpOsfjeR.Dispose();
                CxHkmdYhnpaEDvAgIPbPDNHZJvJltbgf.Dispose();
                yIxtAEUEHvggkarsriomwjXsgPiPseES.Dispose();
                fkuQJbFKYgcqoqXOkrKhEIXAYMYWAoaq.Dispose();
                XiZEpObTIDeEyDVNDpkaTCqZqRsfwxPz.Dispose();

                for (int WEkqinJoEqMrcaYgmZHyYIEIdNECZiPZ = 0; WEkqinJoEqMrcaYgmZHyYIEIdNECZiPZ < LCzprxhUoALMDZNRjZPjYtMdNknhXWMC.DataCount; WEkqinJoEqMrcaYgmZHyYIEIdNECZiPZ++)
                {
                    LCzprxhUoALMDZNRjZPjYtMdNknhXWMC.GetDataAt(WEkqinJoEqMrcaYgmZHyYIEIdNECZiPZ).Dispose();
                }

                LCzprxhUoALMDZNRjZPjYtMdNknhXWMC.Dispose();

                JobHandle DispatchRecreateFlatMeshJob(JobHandle dependsOn = default)
                {
                    unsafe
                    {
                        var tmxodpBTyBhnUyfJGqOoScTWfcIUXdPQ = new gjEBrvkrhmgzYTzfqsSGOztFyPFCHOgr
                        {
                            VertexCount = GtdPlWeRTHLTJDKKyeRgifjExreBPmmV,
                            SubMeshIndexDataArray = LCzprxhUoALMDZNRjZPjYtMdNknhXWMC.Datas,
                            SubMeshIndexDataArrayLength = LCzprxhUoALMDZNRjZPjYtMdNknhXWMC.DataCount,
                        };

                        var HCnmjAVJGOLfEQoUFrlMtXlZZBjvyJXB = new BakiPyhfGLoQRsNyCovenLNJyvxSIthg
                        {
                            WorldToLocalMatrix = transform.worldToLocalMatrix,

                            
                            VolumeConstraintsNeighbors = YloHjXCOPrTYBXFiUJZLdZeyXtoxeEgw,

                            
                            ParticlesPosition = WSeNPORYvDogdhHFowHdAAlvgvOsixnH,
                            ParticlesUVs = rNzAouVuobRcTvvhYFaTunIGIFNrjkrH,

                            
                            VolumeConstraintsEnabled = pvjJblVZcYeKDbqPbHvBwrzIJptWJTbH,
                            VolumeConstraintsParticleA = EDUqmwJByJHaDtEVuBmUsyKFdXcfieMQ,
                            VolumeConstraintsParticleB = UrImtSFGEuBYJStpANejQrzjaHijXHxH,
                            VolumeConstraintsParticleC = qTkskZTbFEDLhILaNPuukluGDJZJFMQW,
                            VolumeConstraintsParticleD = KPTFvGCQHLINIRmTxZozFvfSdpqxsWhb,
                            VolumeConstraintsSubMeshIndex = UtunoCWkYuSbtYFfaHqsfgKirSgZFuYp,

                            
                            VertexCount = GtdPlWeRTHLTJDKKyeRgifjExreBPmmV,
                            Vertices = KsWcPQTNvRutSVNnImWWJWoGqgockKzh,
                            UVs = NLsAsxPlkqZNCFqWkTPBdcLGZpOsfjeR,
                            BindInfo = CxHkmdYhnpaEDvAgIPbPDNHZJvJltbgf,

                            SubMeshIndexDataArray = LCzprxhUoALMDZNRjZPjYtMdNknhXWMC.Datas,
                            SubMeshIndexDataArrayLength = LCzprxhUoALMDZNRjZPjYtMdNknhXWMC.DataCount,
                        };

                        dependsOn = tmxodpBTyBhnUyfJGqOoScTWfcIUXdPQ.Schedule(dependsOn);
                        dependsOn = HCnmjAVJGOLfEQoUFrlMtXlZZBjvyJXB.Schedule(lhVcwdeMFOsOciGfEacCCtHLNtATCLGB, 64, dependsOn);

                        return dependsOn;
                    }
                }

                JobHandle DispatchRecreateSmoothMeshJob(JobHandle dependsOn = default)
                {
                    unsafe
                    {
                        var ZTjfZQVSRsDPSDTVwFOZVpyGyPJQDbZD = new gjEBrvkrhmgzYTzfqsSGOztFyPFCHOgr
                        {
                            VertexCount = GtdPlWeRTHLTJDKKyeRgifjExreBPmmV,
                            SubMeshIndexDataArray = LCzprxhUoALMDZNRjZPjYtMdNknhXWMC.Datas,
                            SubMeshIndexDataArrayLength = LCzprxhUoALMDZNRjZPjYtMdNknhXWMC.DataCount,
                        };

                        dependsOn = ZTjfZQVSRsDPSDTVwFOZVpyGyPJQDbZD.Schedule(dependsOn);

                        var zlvOzKaRpSobskfsaDSnQzoPjJQJPjSq = new ODoqVMMbxYpeSCvumTSirDBLrjlMvJlX
                        {
                            ParticleToVertexMap = yIxtAEUEHvggkarsriomwjXsgPiPseES,
                        };

                        dependsOn = zlvOzKaRpSobskfsaDSnQzoPjJQJPjSq.Schedule(yIxtAEUEHvggkarsriomwjXsgPiPseES.Length, 64, dependsOn);

                        var oTWcDYaLhlqwfLNiKDKnsGFLBKTMksSR = new TYlPRKsIijeEaHaIzUmYeUBwFQeeuAfJ
                        {
                            WorldToLocalMatrix = transform.worldToLocalMatrix,

                            
                            VolumeConstraintsNeighbors = YloHjXCOPrTYBXFiUJZLdZeyXtoxeEgw,

                            
                            ParticlesPosition = WSeNPORYvDogdhHFowHdAAlvgvOsixnH,
                            ParticlesUVs = rNzAouVuobRcTvvhYFaTunIGIFNrjkrH,

                            
                            VolumeConstraintsEnabled = pvjJblVZcYeKDbqPbHvBwrzIJptWJTbH,
                            VolumeConstraintsParticleA = EDUqmwJByJHaDtEVuBmUsyKFdXcfieMQ,
                            VolumeConstraintsParticleB = UrImtSFGEuBYJStpANejQrzjaHijXHxH,
                            VolumeConstraintsParticleC = qTkskZTbFEDLhILaNPuukluGDJZJFMQW,
                            VolumeConstraintsParticleD = KPTFvGCQHLINIRmTxZozFvfSdpqxsWhb,
                            VolumeConstraintsPhysicsMaterialIndex = UtunoCWkYuSbtYFfaHqsfgKirSgZFuYp,

                            
                            VertexCount = GtdPlWeRTHLTJDKKyeRgifjExreBPmmV,
                            Vertices = KsWcPQTNvRutSVNnImWWJWoGqgockKzh,
                            UVs = NLsAsxPlkqZNCFqWkTPBdcLGZpOsfjeR,
                            BindInfo = CxHkmdYhnpaEDvAgIPbPDNHZJvJltbgf,

                            SubMeshIndexDataArray = LCzprxhUoALMDZNRjZPjYtMdNknhXWMC.Datas,
                            SubMeshIndexDataArrayLength = LCzprxhUoALMDZNRjZPjYtMdNknhXWMC.DataCount,

                            
                            ParticleToVertexMap = yIxtAEUEHvggkarsriomwjXsgPiPseES,
                            VertexLocks = XiZEpObTIDeEyDVNDpkaTCqZqRsfwxPz,
                        };

                        return oTWcDYaLhlqwfLNiKDKnsGFLBKTMksSR.Schedule(DjHJTVZVeTZOxIASmqlbTqfkMJGKPFWb.Length, 64, dependsOn);
                    }
                }
            }
        }

        [BurstCompile]
        private struct soQquvlZKtSufDGLYySIiSFbEnbjrRLC : IJobParallelFor
        {
            [ReadOnly]
            public int MinNeighborCount;

            [NativeDisableParallelForRestriction]
            public NativeSlice<bool> VolumeConstraintsEnabled;

            [ReadOnly]
            public NativeArray<int> VolumeConstraintsNeighbors;

            [NativeDisableParallelForRestriction]
            [WriteOnly]
            public NativeSlice<bool> VolumeConstraintsEnabledStateHasChanged;

            public void Execute(int volumeConstraintIndex)
            {
                int FiAWJcqmhrzslcRaiSSWovSWUzHxhkVx = 0;
                int XFPtPWchqKfkaiqcgFtHcuNWxbmSGHUD;
                for (int yetCzmpexruhgYOOvDZThepNaueAHoBC = 0; yetCzmpexruhgYOOvDZThepNaueAHoBC < 4; yetCzmpexruhgYOOvDZThepNaueAHoBC++)
                {
                    XFPtPWchqKfkaiqcgFtHcuNWxbmSGHUD = VolumeConstraintsNeighbors[(volumeConstraintIndex * 4) + yetCzmpexruhgYOOvDZThepNaueAHoBC];
                    if (XFPtPWchqKfkaiqcgFtHcuNWxbmSGHUD == -1 || VolumeConstraintsEnabled[XFPtPWchqKfkaiqcgFtHcuNWxbmSGHUD] == false)
                    {
                        continue;
                    }

                    FiAWJcqmhrzslcRaiSSWovSWUzHxhkVx++;
                }

                if (FiAWJcqmhrzslcRaiSSWovSWUzHxhkVx < MinNeighborCount)
                {
                    VolumeConstraintsEnabled[volumeConstraintIndex] = false;
                    VolumeConstraintsEnabledStateHasChanged[0] = true;
                }
            }
        }

        [BurstCompile]
        private struct SDHqFwtlTLLPwPfGhNNHQgosjAutSHYJ : IJobParallelFor
        {
            [ReadOnly]
            public NativeSlice<bool> VolumeConstraintsEnabled;

            [ReadOnly]
            public NativeArray<int> VolumeConstraintsToDistanceConstraintsIndex;

            [WriteOnly]
            [NativeDisableParallelForRestriction]
            public NativeArray<bool> ResidualDistanceConstraints;

            public void Execute(int volumeConstraintIndex)
            {
                if (!VolumeConstraintsEnabled[volumeConstraintIndex])
                {
                    return;
                }

                for (int YNjxtDBvvnniWYQKeCdlMPVgPXAlgHBD = 0; YNjxtDBvvnniWYQKeCdlMPVgPXAlgHBD < 6; YNjxtDBvvnniWYQKeCdlMPVgPXAlgHBD++)
                {
                    ResidualDistanceConstraints[VolumeConstraintsToDistanceConstraintsIndex[(volumeConstraintIndex * 6) + YNjxtDBvvnniWYQKeCdlMPVgPXAlgHBD]] = false;
                }
            }
        }

        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private struct LxhLSXEheDAdxPVZzNYtHYiQCKhCRRhB : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<bool> ResidualDistanceConstraints;

            [WriteOnly]
            public NativeSlice<bool> DistanceConstraintsEnabled;

            [WriteOnly]
            [NativeDisableParallelForRestriction]
            public NativeSlice<bool> DistanceConstraintsEnabledStateHasChanged;

            public void Execute(int distanceConstraintIndex)
            {
                if (ResidualDistanceConstraints[distanceConstraintIndex])
                {
                    DistanceConstraintsEnabled[distanceConstraintIndex] = false;
                    DistanceConstraintsEnabledStateHasChanged[0] = true;
                }
            }
        }

        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private struct BwNSNQtubLjxpfsKVPPvzgDGZoNQwaPw : IJobParallelFor
        {
            
            [NativeDisableParallelForRestriction]
            public NativeArray<int> TetrahedralMeshNeighbors;

            [ReadOnly]
            public NativeArray<int> VolumeConstraintsPhysicsMaterialIndex;

            public void Execute(int volumeConstraintIndex)
            {
                
                int ujmgvrvbTtVVRcOsggZiShdhcEXuPviT = VolumeConstraintsPhysicsMaterialIndex[volumeConstraintIndex];

                
                for (int xGCpeWAoMcjPPlJCbmspKOiTRMlJulLA = 0; xGCpeWAoMcjPPlJCbmspKOiTRMlJulLA < 4; xGCpeWAoMcjPPlJCbmspKOiTRMlJulLA++)
                {
                    int CABFIWMsVHfFsZfbtPLZZRoZFGeUSblR = TetrahedralMeshNeighbors[(volumeConstraintIndex * 4) + xGCpeWAoMcjPPlJCbmspKOiTRMlJulLA];

                    if (CABFIWMsVHfFsZfbtPLZZRoZFGeUSblR != -1)
                    {
                        if (ujmgvrvbTtVVRcOsggZiShdhcEXuPviT != VolumeConstraintsPhysicsMaterialIndex[CABFIWMsVHfFsZfbtPLZZRoZFGeUSblR])
                        {
                            TetrahedralMeshNeighbors[(volumeConstraintIndex * 4) + xGCpeWAoMcjPPlJCbmspKOiTRMlJulLA] = -1;
                        }
                    }
                }
            }

            private bool rrXHSDpBNSAxoIWRnhYlhoeNXkJindpg(in TetrahedralSubMeshDescriptor smd, in int tetIndex)
            {
                return smd.IndexStart / 4 <= tetIndex && (smd.IndexStart + smd.IndexCount) / 4 > tetIndex;
            }
        }

        [BurstCompile]
        private unsafe struct BakiPyhfGLoQRsNyCovenLNJyvxSIthg : IJobParallelFor
        {
            [ReadOnly]
            public float4x4 WorldToLocalMatrix;

            
            [ReadOnly]
            public NativeArray<int> VolumeConstraintsNeighbors;

            
            [ReadOnly]
            public NativeSlice<float3> ParticlesPosition;

            
            [ReadOnly]
            public NativeArray<float2> ParticlesUVs;

            
            [ReadOnly]
            public NativeSlice<bool> VolumeConstraintsEnabled;
            [ReadOnly]
            public NativeSlice<int> VolumeConstraintsParticleA;
            [ReadOnly]
            public NativeSlice<int> VolumeConstraintsParticleB;
            [ReadOnly]
            public NativeSlice<int> VolumeConstraintsParticleC;
            [ReadOnly]
            public NativeSlice<int> VolumeConstraintsParticleD;
            [ReadOnly]
            public NativeSlice<int> VolumeConstraintsSubMeshIndex;

            
            [NativeDisableUnsafePtrRestriction]
            public UnsafeAtomicCounter32 VertexCount;

            [NativeDisableParallelForRestriction]
            [WriteOnly]
            public NativeArray<float3> Vertices;

            [WriteOnly]
            [NativeDisableParallelForRestriction]
            public NativeArray<float2> UVs;

            [NativeDisableParallelForRestriction]
            [WriteOnly]
            public NativeArray<int> BindInfo;

            [NativeDisableUnsafePtrRestriction]
            public kVpvotvXxBdwCpONVPtDFVRBZFdnGOht* SubMeshIndexDataArray;
            public int SubMeshIndexDataArrayLength;

            public void Execute(int volumeConstraintIndex)
            {
                if (!VolumeConstraintsEnabled[volumeConstraintIndex])
                {
                    return;
                }

                var FmQuXLMuJoNInFLWWehZCEVVUSGnlvGh = volumeConstraintIndex * 4;

                var LOptmNtBSFgyuOlmMYlbxNGpLWmnsKoB = VolumeConstraintsParticleA[volumeConstraintIndex];
                var NBSmiThsffdHvYuoPqrdlmoJjMGMcJOQ = VolumeConstraintsParticleB[volumeConstraintIndex];
                var baRwQrLXewfHenWmogRQcIEzFLUlKSGm = VolumeConstraintsParticleC[volumeConstraintIndex];
                var KJBrsTzKRImVCaxJqEsQsLAZkpkemeIx = VolumeConstraintsParticleD[volumeConstraintIndex];

                var MSXcOdpRIWOBnUSBXAgSnwUHqmTnjORq = ParticlesPosition[LOptmNtBSFgyuOlmMYlbxNGpLWmnsKoB];
                var DtPzXtKrVckExLfGLJtMrpyFEoUerEqF = ParticlesPosition[NBSmiThsffdHvYuoPqrdlmoJjMGMcJOQ];
                var zaRDmvFsSXCySBreageodLCiLiZyozLA = ParticlesPosition[baRwQrLXewfHenWmogRQcIEzFLUlKSGm];
                var ODiIeARiNlRaOyyVeLMVWlSjFcispNVD = ParticlesPosition[KJBrsTzKRImVCaxJqEsQsLAZkpkemeIx];

                var GtCSvwSvMZQWCFLHBYoUbzvjbauMymzQ = VolumeConstraintsNeighbors[FmQuXLMuJoNInFLWWehZCEVVUSGnlvGh + 0];
                var dZfAPdtqZnLqrIESUzRDwBxiOVcaMxBT = VolumeConstraintsNeighbors[FmQuXLMuJoNInFLWWehZCEVVUSGnlvGh + 1];
                var jfLcDqsDkbijigGYaRTXPCvhGIUSlBVS = VolumeConstraintsNeighbors[FmQuXLMuJoNInFLWWehZCEVVUSGnlvGh + 2];
                var JPjaEgnUvLHdFbuhRHIeJLuKnXGLjmWG = VolumeConstraintsNeighbors[FmQuXLMuJoNInFLWWehZCEVVUSGnlvGh + 3];

                int rrHVrvRLTaCWjcJTYOWIZJiuSdwPQtBm = VolumeConstraintsSubMeshIndex[volumeConstraintIndex];

                
                if (GtCSvwSvMZQWCFLHBYoUbzvjbauMymzQ < 0 || !VolumeConstraintsEnabled[GtCSvwSvMZQWCFLHBYoUbzvjbauMymzQ])
                {
                    var CjyyZveKaVVFpbvQQGRMmAGJqnWBFudW = VYAqjgtrqQysPtQSvFxbdWbnkZSyhFrC(DtPzXtKrVckExLfGLJtMrpyFEoUerEqF, ODiIeARiNlRaOyyVeLMVWlSjFcispNVD, zaRDmvFsSXCySBreageodLCiLiZyozLA, rrHVrvRLTaCWjcJTYOWIZJiuSdwPQtBm);
                    BindInfo[CjyyZveKaVVFpbvQQGRMmAGJqnWBFudW.Item1] = NBSmiThsffdHvYuoPqrdlmoJjMGMcJOQ;
                    BindInfo[CjyyZveKaVVFpbvQQGRMmAGJqnWBFudW.Item1 + 1] = KJBrsTzKRImVCaxJqEsQsLAZkpkemeIx;
                    BindInfo[CjyyZveKaVVFpbvQQGRMmAGJqnWBFudW.Item1 + 2] = baRwQrLXewfHenWmogRQcIEzFLUlKSGm;
                    UVs[CjyyZveKaVVFpbvQQGRMmAGJqnWBFudW.Item1] = ParticlesUVs[NBSmiThsffdHvYuoPqrdlmoJjMGMcJOQ];
                    UVs[CjyyZveKaVVFpbvQQGRMmAGJqnWBFudW.Item1 + 1] = ParticlesUVs[KJBrsTzKRImVCaxJqEsQsLAZkpkemeIx];
                    UVs[CjyyZveKaVVFpbvQQGRMmAGJqnWBFudW.Item1 + 2] = ParticlesUVs[baRwQrLXewfHenWmogRQcIEzFLUlKSGm];
                }

                
                if (dZfAPdtqZnLqrIESUzRDwBxiOVcaMxBT < 0 || !VolumeConstraintsEnabled[dZfAPdtqZnLqrIESUzRDwBxiOVcaMxBT])
                {
                    var euJuoYNlSztkMAPVDqkBUaQssaHsDDux = VYAqjgtrqQysPtQSvFxbdWbnkZSyhFrC(MSXcOdpRIWOBnUSBXAgSnwUHqmTnjORq, zaRDmvFsSXCySBreageodLCiLiZyozLA, ODiIeARiNlRaOyyVeLMVWlSjFcispNVD, rrHVrvRLTaCWjcJTYOWIZJiuSdwPQtBm);
                    BindInfo[euJuoYNlSztkMAPVDqkBUaQssaHsDDux.Item1] = LOptmNtBSFgyuOlmMYlbxNGpLWmnsKoB;
                    BindInfo[euJuoYNlSztkMAPVDqkBUaQssaHsDDux.Item1 + 1] = baRwQrLXewfHenWmogRQcIEzFLUlKSGm;
                    BindInfo[euJuoYNlSztkMAPVDqkBUaQssaHsDDux.Item1 + 2] = KJBrsTzKRImVCaxJqEsQsLAZkpkemeIx;
                    UVs[euJuoYNlSztkMAPVDqkBUaQssaHsDDux.Item1] = ParticlesUVs[LOptmNtBSFgyuOlmMYlbxNGpLWmnsKoB];
                    UVs[euJuoYNlSztkMAPVDqkBUaQssaHsDDux.Item1 + 1] = ParticlesUVs[baRwQrLXewfHenWmogRQcIEzFLUlKSGm];
                    UVs[euJuoYNlSztkMAPVDqkBUaQssaHsDDux.Item1 + 2] = ParticlesUVs[KJBrsTzKRImVCaxJqEsQsLAZkpkemeIx];
                }

                
                if (jfLcDqsDkbijigGYaRTXPCvhGIUSlBVS < 0 || !VolumeConstraintsEnabled[jfLcDqsDkbijigGYaRTXPCvhGIUSlBVS])
                {
                    var SLPsHklyXMydqwgjbjXYHaAjYwlAadwX = VYAqjgtrqQysPtQSvFxbdWbnkZSyhFrC(MSXcOdpRIWOBnUSBXAgSnwUHqmTnjORq, ODiIeARiNlRaOyyVeLMVWlSjFcispNVD, DtPzXtKrVckExLfGLJtMrpyFEoUerEqF, rrHVrvRLTaCWjcJTYOWIZJiuSdwPQtBm);
                    BindInfo[SLPsHklyXMydqwgjbjXYHaAjYwlAadwX.Item1] = LOptmNtBSFgyuOlmMYlbxNGpLWmnsKoB;
                    BindInfo[SLPsHklyXMydqwgjbjXYHaAjYwlAadwX.Item1 + 1] = KJBrsTzKRImVCaxJqEsQsLAZkpkemeIx;
                    BindInfo[SLPsHklyXMydqwgjbjXYHaAjYwlAadwX.Item1 + 2] = NBSmiThsffdHvYuoPqrdlmoJjMGMcJOQ;

                    UVs[SLPsHklyXMydqwgjbjXYHaAjYwlAadwX.Item1] = ParticlesUVs[LOptmNtBSFgyuOlmMYlbxNGpLWmnsKoB];
                    UVs[SLPsHklyXMydqwgjbjXYHaAjYwlAadwX.Item1 + 1] = ParticlesUVs[KJBrsTzKRImVCaxJqEsQsLAZkpkemeIx];
                    UVs[SLPsHklyXMydqwgjbjXYHaAjYwlAadwX.Item1 + 2] = ParticlesUVs[NBSmiThsffdHvYuoPqrdlmoJjMGMcJOQ];
                }

                
                if (JPjaEgnUvLHdFbuhRHIeJLuKnXGLjmWG < 0 || !VolumeConstraintsEnabled[JPjaEgnUvLHdFbuhRHIeJLuKnXGLjmWG])
                {
                    var KkDFMBoDsgxdRjXUuAeFtKzvWvhcNOxv = VYAqjgtrqQysPtQSvFxbdWbnkZSyhFrC(MSXcOdpRIWOBnUSBXAgSnwUHqmTnjORq, DtPzXtKrVckExLfGLJtMrpyFEoUerEqF, zaRDmvFsSXCySBreageodLCiLiZyozLA, rrHVrvRLTaCWjcJTYOWIZJiuSdwPQtBm);
                    BindInfo[KkDFMBoDsgxdRjXUuAeFtKzvWvhcNOxv.Item1] = LOptmNtBSFgyuOlmMYlbxNGpLWmnsKoB;
                    BindInfo[KkDFMBoDsgxdRjXUuAeFtKzvWvhcNOxv.Item1 + 1] = NBSmiThsffdHvYuoPqrdlmoJjMGMcJOQ;
                    BindInfo[KkDFMBoDsgxdRjXUuAeFtKzvWvhcNOxv.Item1 + 2] = baRwQrLXewfHenWmogRQcIEzFLUlKSGm;

                    UVs[KkDFMBoDsgxdRjXUuAeFtKzvWvhcNOxv.Item1] = ParticlesUVs[LOptmNtBSFgyuOlmMYlbxNGpLWmnsKoB];
                    UVs[KkDFMBoDsgxdRjXUuAeFtKzvWvhcNOxv.Item1 + 1] = ParticlesUVs[NBSmiThsffdHvYuoPqrdlmoJjMGMcJOQ];
                    UVs[KkDFMBoDsgxdRjXUuAeFtKzvWvhcNOxv.Item1 + 2] = ParticlesUVs[baRwQrLXewfHenWmogRQcIEzFLUlKSGm];
                }
            }

            private (int VertexStartIndex, int IndexStartIndex) VYAqjgtrqQysPtQSvFxbdWbnkZSyhFrC(in float3 pos1, in float3 pos2, in float3 pos3, in int subMeshIndex)
            {
                var PaUvdkMXFcXkHwJQVtSxxHisKhtbsogM = VertexCount.Add(3);

                Vertices[PaUvdkMXFcXkHwJQVtSxxHisKhtbsogM] = math.mul(WorldToLocalMatrix, new float4(pos1, 1.0f)).xyz;
                Vertices[PaUvdkMXFcXkHwJQVtSxxHisKhtbsogM + 1] = math.mul(WorldToLocalMatrix, new float4(pos2, 1.0f)).xyz;
                Vertices[PaUvdkMXFcXkHwJQVtSxxHisKhtbsogM + 2] = math.mul(WorldToLocalMatrix, new float4(pos3, 1.0f)).xyz;

                var YQTGQhbjPpfqYNxkJGZYdPYumvXXecax = SubMeshIndexDataArray[subMeshIndex];
                var HQMgiaOaYqXYdEqFYGKoqvYLxdOIvlKt = YQTGQhbjPpfqYNxkJGZYdPYumvXXecax.IndexCount.Add(3);
                YQTGQhbjPpfqYNxkJGZYdPYumvXXecax.Indices[HQMgiaOaYqXYdEqFYGKoqvYLxdOIvlKt] = PaUvdkMXFcXkHwJQVtSxxHisKhtbsogM;
                YQTGQhbjPpfqYNxkJGZYdPYumvXXecax.Indices[HQMgiaOaYqXYdEqFYGKoqvYLxdOIvlKt + 1] = PaUvdkMXFcXkHwJQVtSxxHisKhtbsogM + 1;
                YQTGQhbjPpfqYNxkJGZYdPYumvXXecax.Indices[HQMgiaOaYqXYdEqFYGKoqvYLxdOIvlKt + 2] = PaUvdkMXFcXkHwJQVtSxxHisKhtbsogM + 2;

                return (PaUvdkMXFcXkHwJQVtSxxHisKhtbsogM, HQMgiaOaYqXYdEqFYGKoqvYLxdOIvlKt);
            }
        }

        [BurstCompile]
        private unsafe struct gjEBrvkrhmgzYTzfqsSGOztFyPFCHOgr : IJob
        {
            [NativeDisableUnsafePtrRestriction]
            public UnsafeAtomicCounter32 VertexCount;

            [NativeDisableUnsafePtrRestriction]
            public kVpvotvXxBdwCpONVPtDFVRBZFdnGOht* SubMeshIndexDataArray;
            public int SubMeshIndexDataArrayLength;

            public void Execute()
            {
                unsafe
                {
                    VertexCount.Reset();

                    for (int ITKCbSaBawkJoyrDyNeiUAhaNlJVVmjm = 0; ITKCbSaBawkJoyrDyNeiUAhaNlJVVmjm < SubMeshIndexDataArrayLength; ITKCbSaBawkJoyrDyNeiUAhaNlJVVmjm++)
                    {
                        SubMeshIndexDataArray[ITKCbSaBawkJoyrDyNeiUAhaNlJVVmjm].IndexCount.Reset();
                    }
                }
            }
        }

        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private unsafe struct TYlPRKsIijeEaHaIzUmYeUBwFQeeuAfJ : IJobParallelFor
        {
            [ReadOnly]
            public float4x4 WorldToLocalMatrix;

            [NativeDisableParallelForRestriction]
            public NativeArray<int> VertexLocks;

            [NativeDisableUnsafePtrRestriction]
            public UnsafeAtomicCounter32 VertexCount;

            [WriteOnly]
            [NativeDisableParallelForRestriction]
            public NativeArray<float3> Vertices;

            [WriteOnly]
            [NativeDisableParallelForRestriction]
            public NativeArray<float2> UVs;

            [WriteOnly]
            [NativeDisableParallelForRestriction]
            public NativeArray<int> BindInfo;

            [NativeDisableUnsafePtrRestriction]
            public kVpvotvXxBdwCpONVPtDFVRBZFdnGOht* SubMeshIndexDataArray;

            public int SubMeshIndexDataArrayLength;

            [ReadOnly]
            public NativeArray<int> VolumeConstraintsNeighbors;

            [ReadOnly]
            public NativeSlice<float3> ParticlesPosition;

            [ReadOnly]
            public NativeArray<float2> ParticlesUVs;

            
            [ReadOnly]
            public NativeSlice<bool> VolumeConstraintsEnabled;
            [ReadOnly]
            public NativeSlice<int> VolumeConstraintsParticleA;
            [ReadOnly]
            public NativeSlice<int> VolumeConstraintsParticleB;
            [ReadOnly]
            public NativeSlice<int> VolumeConstraintsParticleC;
            [ReadOnly]
            public NativeSlice<int> VolumeConstraintsParticleD;
            [ReadOnly]
            public NativeSlice<int> VolumeConstraintsPhysicsMaterialIndex;

            [NativeDisableParallelForRestriction]
            public NativeArray<int> ParticleToVertexMap;

            public void Execute(int volumeConstraintIndex)
            {
                if (!VolumeConstraintsEnabled[volumeConstraintIndex])
                {
                    return;
                }

                var XCoBewLCWTIPhnSKfeuGVKsyVOBeLBAg = volumeConstraintIndex * 4;

                
                var FSvftbIghyzPeGMhPssrlnnKroXCHaPt = VolumeConstraintsParticleA[volumeConstraintIndex];
                var wivLUcjmbVrIFoWXsaTMuVgIJYgBYWiw = VolumeConstraintsParticleB[volumeConstraintIndex];
                var xeiKaKCHkEEhmJYrpzWTSbdfCPXNQiMb = VolumeConstraintsParticleC[volumeConstraintIndex];
                var dRlREwZpkWZwHYHvliLOAQmnmUUHBckX = VolumeConstraintsParticleD[volumeConstraintIndex];

                var TueaMHMvDwYXbLrXzOJlRPUNwspGxdYZ = VolumeConstraintsNeighbors[XCoBewLCWTIPhnSKfeuGVKsyVOBeLBAg + 0];
                var DPUISncFlQyJcDBmvQBkMIndrlYvTHkA = VolumeConstraintsNeighbors[XCoBewLCWTIPhnSKfeuGVKsyVOBeLBAg + 1];
                var bySsxhRLPkAaBBGqXmWeauDRrpRaBsQE = VolumeConstraintsNeighbors[XCoBewLCWTIPhnSKfeuGVKsyVOBeLBAg + 2];
                var igZcqBciLZiYdsKmkfTVuKFckLWBVTNV = VolumeConstraintsNeighbors[XCoBewLCWTIPhnSKfeuGVKsyVOBeLBAg + 3];

                var pIyDFmHztKJLSkoPmVCtdsmYbXVvtMxz = VolumeConstraintsPhysicsMaterialIndex[volumeConstraintIndex];

                
                if (TueaMHMvDwYXbLrXzOJlRPUNwspGxdYZ < 0 || !VolumeConstraintsEnabled[TueaMHMvDwYXbLrXzOJlRPUNwspGxdYZ])
                {
                    nBXiILdcporrsTggsbdjdWrvHLLYwekS(wivLUcjmbVrIFoWXsaTMuVgIJYgBYWiw, dRlREwZpkWZwHYHvliLOAQmnmUUHBckX, xeiKaKCHkEEhmJYrpzWTSbdfCPXNQiMb, pIyDFmHztKJLSkoPmVCtdsmYbXVvtMxz);
                }

                
                if (DPUISncFlQyJcDBmvQBkMIndrlYvTHkA < 0 || !VolumeConstraintsEnabled[DPUISncFlQyJcDBmvQBkMIndrlYvTHkA])
                {
                    nBXiILdcporrsTggsbdjdWrvHLLYwekS(FSvftbIghyzPeGMhPssrlnnKroXCHaPt, xeiKaKCHkEEhmJYrpzWTSbdfCPXNQiMb, dRlREwZpkWZwHYHvliLOAQmnmUUHBckX, pIyDFmHztKJLSkoPmVCtdsmYbXVvtMxz);
                }

                
                if (bySsxhRLPkAaBBGqXmWeauDRrpRaBsQE < 0 || !VolumeConstraintsEnabled[bySsxhRLPkAaBBGqXmWeauDRrpRaBsQE])
                {
                    nBXiILdcporrsTggsbdjdWrvHLLYwekS(FSvftbIghyzPeGMhPssrlnnKroXCHaPt, dRlREwZpkWZwHYHvliLOAQmnmUUHBckX, wivLUcjmbVrIFoWXsaTMuVgIJYgBYWiw, pIyDFmHztKJLSkoPmVCtdsmYbXVvtMxz);
                }

                
                if (igZcqBciLZiYdsKmkfTVuKFckLWBVTNV < 0 || !VolumeConstraintsEnabled[igZcqBciLZiYdsKmkfTVuKFckLWBVTNV])
                {
                    nBXiILdcporrsTggsbdjdWrvHLLYwekS(FSvftbIghyzPeGMhPssrlnnKroXCHaPt, wivLUcjmbVrIFoWXsaTMuVgIJYgBYWiw, xeiKaKCHkEEhmJYrpzWTSbdfCPXNQiMb, pIyDFmHztKJLSkoPmVCtdsmYbXVvtMxz);
                }
            }

            private void nBXiILdcporrsTggsbdjdWrvHLLYwekS(in int particle1, in int particle2, in int particle3, in int subMeshIndex)
            {
                
                int DmGdkUagrifkvuwTWpjyPOTRvTvDQhbG = ikbaaTCxvQTeZAFJdkqtUYGendGAimgr(particle1);
                int BXdttoBWrXKSOwGabUrLPpcElMCNnkYu = ikbaaTCxvQTeZAFJdkqtUYGendGAimgr(particle2);
                int sqaVYyUTzAmuliztJUJpEuZrgQFNpaFa = ikbaaTCxvQTeZAFJdkqtUYGendGAimgr(particle3);

                var fxSokzTeDDbzhRWGljBdQnaJWFepaKUS = Math.TriangleNormal(ParticlesPosition[particle1], ParticlesPosition[particle2], ParticlesPosition[particle3]);

                
                var VFlWBliCUwahazuLREPbLYBCYshGAcIN = SubMeshIndexDataArray[subMeshIndex];
                var zxOJqqGiWymSuUCJTBBDRzycZAdsGBQW = VFlWBliCUwahazuLREPbLYBCYshGAcIN.IndexCount.Add(3);
                VFlWBliCUwahazuLREPbLYBCYshGAcIN.Indices[zxOJqqGiWymSuUCJTBBDRzycZAdsGBQW] = DmGdkUagrifkvuwTWpjyPOTRvTvDQhbG;
                VFlWBliCUwahazuLREPbLYBCYshGAcIN.Indices[zxOJqqGiWymSuUCJTBBDRzycZAdsGBQW + 1] = BXdttoBWrXKSOwGabUrLPpcElMCNnkYu;
                VFlWBliCUwahazuLREPbLYBCYshGAcIN.Indices[zxOJqqGiWymSuUCJTBBDRzycZAdsGBQW + 2] = sqaVYyUTzAmuliztJUJpEuZrgQFNpaFa;
            }

            private int ikbaaTCxvQTeZAFJdkqtUYGendGAimgr(int particle)
            {
                
                if (ParticleToVertexMap.InterlockedCompareExchange(particle, -2, -1) == -1)
                {
                    int oXHmgWCgjblUSTafpkPsTCZAwafBAnvg = VertexCount.Add(1);
                    Vertices[oXHmgWCgjblUSTafpkPsTCZAwafBAnvg] = math.mul(WorldToLocalMatrix, new float4(ParticlesPosition[particle], 1.0f)).xyz;
                    UVs[oXHmgWCgjblUSTafpkPsTCZAwafBAnvg] = ParticlesUVs[particle];

                    ParticleToVertexMap[particle] = oXHmgWCgjblUSTafpkPsTCZAwafBAnvg;

                    
                    BindInfo[oXHmgWCgjblUSTafpkPsTCZAwafBAnvg] = particle;
                }

                
                while (ParticleToVertexMap.InterlockedCompareExchange(particle, -2, -2) < 0)
                {
                }

                return ParticleToVertexMap[particle];
            }
        }

        [BurstCompile]
        private struct ODoqVMMbxYpeSCvumTSirDBLrjlMvJlX : IJobParallelFor
        {
            public NativeArray<int> ParticleToVertexMap;

            public void Execute(int particleIndex)
            {
                ParticleToVertexMap[particleIndex] = -1;
            }
        }

        [BurstCompile]
        private struct zpujZcKmdLmmZTrSyeMBIQjXXLWJAZRY : IJobParallelFor
        {
            public NativeArray<float3> Normals;
            public NativeArray<int> NormalAffectCount;

            public void Execute(int index)
            {
                var OQTheuaxoAkJtevoqBYSmFsJSReuRAZs = Normals[index];
                var OsYZtUwatUCmPNlBVztUzJncIskMQRZH = NormalAffectCount[index];

                if (OsYZtUwatUCmPNlBVztUzJncIskMQRZH > 0)
                {
                    Normals[index] = math.normalize(OQTheuaxoAkJtevoqBYSmFsJSReuRAZs / OsYZtUwatUCmPNlBVztUzJncIskMQRZH);
                }

                NormalAffectCount[index] = 0;
            }
        }

        [BurstCompile]
        private struct zUMipoJVZMiXeMPLJFnpaVweFVagqump : IJobParallelFor
        {
            [ReadOnly]
            public float4x4 WorldToLocalMatrix;

            [ReadOnly]
            public NativeArray<int> VertexBindings;
            [WriteOnly]
            public NativeArray<float3> Vertices;

            [ReadOnly]
            public NativeSlice<float3> ParticlesPosition;

            public void Execute(int vertexIndex)
            {
                Vertices[vertexIndex] = math.mul(WorldToLocalMatrix, new float4(ParticlesPosition[VertexBindings[vertexIndex]], 1.0f)).xyz;
            }
        }

        [BurstCompile]
        private struct nOfUTIIRRQqsZjKPltWRueYzjbOjuJry : IJobParallelFor
        {
            [ReadOnly]
            public float4x4 InitialWorldToLocalMatrix;

            [ReadOnly]
            public float4x4 LocalToWorldMatrix;

            [ReadOnly]
            public NativeSlice<float3> ParticlesInitialPosition;

            [ReadOnly]
            public NativeSlice<float> ParticlesInverseMass;

            [WriteOnly]
            public NativeSlice<float3> ParticlesPosition;

            public void Execute(int particleIndex)
            {
                if (ParticlesInverseMass[particleIndex] > math.EPSILON)
                {
                    return;
                }

                float3 olVucrZZdlcNgCuwelZkVqlRTKTiWTDP = ParticlesInitialPosition[particleIndex];

                olVucrZZdlcNgCuwelZkVqlRTKTiWTDP = math.mul(InitialWorldToLocalMatrix, new float4(olVucrZZdlcNgCuwelZkVqlRTKTiWTDP, 1.0f)).xyz;
                olVucrZZdlcNgCuwelZkVqlRTKTiWTDP = math.mul(LocalToWorldMatrix, new float4(olVucrZZdlcNgCuwelZkVqlRTKTiWTDP, 1.0f)).xyz;

                ParticlesPosition[particleIndex] = olVucrZZdlcNgCuwelZkVqlRTKTiWTDP;
            }
        }

        [BurstCompile]
        private struct kVpvotvXxBdwCpONVPtDFVRBZFdnGOht
        {
            public UnsafeAtomicCounter32 IndexCount;
            public NativeArray<int> Indices;

            public kVpvotvXxBdwCpONVPtDFVRBZFdnGOht(int indexCount)
            {
                unsafe
                {
                    int* QPLtfdsQINFwMTqCqFrcvZtjAehlkCIu = (int*)UnsafeUtility.Malloc(sizeof(int), 4, Allocator.Persistent);
                    IndexCount = new UnsafeAtomicCounter32(QPLtfdsQINFwMTqCqFrcvZtjAehlkCIu);
                    Indices = new NativeArray<int>(indexCount, Allocator.Persistent);
                }
            }

            public void Resize(int newSize)
            {
                Indices.ResizeArray(newSize);
            }

            public void Dispose()
            {
                unsafe
                {
                    if (IndexCount.Counter != null)
                    {
                        UnsafeUtility.Free(IndexCount.Counter, Allocator.Persistent);
                    }
                }

                Indices.Dispose();
            }
        }

        private unsafe class kNfsIJiAeHdUsHBnINcLfwfDHElTSnIa
        {
#pragma warning disable SA1401 // Fields should be private
            public int DataCount;
            public kVpvotvXxBdwCpONVPtDFVRBZFdnGOht* Datas;
#pragma warning restore SA1401 // Fields should be private

            public kNfsIJiAeHdUsHBnINcLfwfDHElTSnIa(int subMeshCount)
            {
                unsafe
                {
                    Datas = (kVpvotvXxBdwCpONVPtDFVRBZFdnGOht*)UnsafeUtility.Malloc(sizeof(kVpvotvXxBdwCpONVPtDFVRBZFdnGOht) * subMeshCount, 4, Allocator.Persistent);
                    DataCount = subMeshCount;
                }
            }

            public kVpvotvXxBdwCpONVPtDFVRBZFdnGOht GetDataAt(int index)
            {
                UnityEngine.Debug.Assert(index >= 0 && index < DataCount);

                return Datas[index];
            }

            public void Dispose()
            {
                unsafe
                {
                    UnsafeUtility.Free(Datas, Allocator.Persistent);
                    DataCount = 0;
                }
            }
        }
    }
}
