namespace MAGES.MeshDeformations
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using MAGES.MeshDeformations.Synchronization;
    using MAGES.MeshDeformations.Utilities;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Jobs;
    using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.Events;
    using Debug = UnityEngine.Debug;

    /// <summary>
    /// A physics world that simulates particles and constraints.
    /// </summary>
    public class PhysicsWorld : MonoBehaviour
    {
#pragma warning disable SA1401 // Fields should be private
        /// <summary>
        /// The runtime particle data.
        /// </summary>
        internal RuntimeParticleData ParticleData;

        /// <summary>
        /// The runtime distance constraint data.
        /// </summary>
        internal RuntimeDistanceConstraintData DistanceConstraintData;

        /// <summary>
        /// The runtime volume constraint data.
        /// </summary>
        internal RuntimeVolumeConstraintData VolumeConstraintData;

        /// <summary>
        /// The runtime shape matching constraint data.
        /// </summary>
        internal RuntimeShapeMatchingConstraintData ShapeMatchingConstraintData;

        /// <summary>
        /// The runtime collinear constraint data.
        /// </summary>
        internal RuntimeCollinearConstraintData CollinearConstraintData;

        /// <summary>
        /// The runtime collision constraint data.
        /// </summary>
        internal RuntimeSimulationMeshDescriptorData SimulationMeshDescriptorData;

        /// <summary>
        /// The runtime physics materials.
        /// </summary>
        internal NativeArray<PhysicsMaterial> PhysicsMaterials = new NativeArray<PhysicsMaterial>(0, Allocator.Persistent, NativeArrayOptions.ClearMemory);

        /// <summary>
        /// The contact pairs count.
        /// </summary>
#pragma warning disable SA1202 // Elements should be ordered by access
        public NativeReference<int> ContactPairsCount;

        /// <summary>
        /// The contact pairs.
        /// </summary>
        public NativeArray<ContactPair> ContactPairs;
#pragma warning restore SA1202 // Elements should be ordered by access

        /// <summary>
        /// The spatial hash grid used for contact detection.
        /// </summary>
        internal SpatialHashGrid SpatialHashGrid;

        /// <summary>
        /// A map from simulation mesh to its descriptor.
        /// </summary>
        internal Dictionary<SimulationMesh, int> SimulationMeshToDescriptorMap = new Dictionary<SimulationMesh, int>();

        /// <summary>
        /// A map from a descriptor to its simulation mesh.
        /// </summary>
        internal Dictionary<int, SimulationMesh> DescriptorToSimulationMeshMap = new Dictionary<int, SimulationMesh>();
#pragma warning restore SA1401 // Fields should be private

        private JUZnLKejmTkSdgYeXeeIjqZqdhSxtoBD aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf;
        private bool kUpSPIkKSjSJaVYamQPQNyytrRTDGYLp = false;

        private Queue<BaseCommand> NpRgNYqXOQnxSLUeACnXuoJOSxlSElhe = new Queue<BaseCommand>();

        private List<IConstraintSolver> vZHHMDAlQnZtdxCQQDCgQiLYmOlZXfOU = new List<IConstraintSolver>();

        /// <summary>
        /// Gets or sets a value indicating whether to simulate physics or not.
        /// </summary>
        [field: Tooltip("If true, the physics world will simulate physics. If false, the physics world will not simulate physics.")]
        [field: SerializeField]
        public bool Simulate { get; set; } = true;

        /// <summary>
        /// Gets or sets the physics time step.
        /// </summary>
        [field: Tooltip("The time step of the physics simulation.")]
        [field: SerializeField]
        public float TimeStep { get; set; } = 0.01f;

        /// <summary>
        /// Gets or sets the world's gravity.
        /// </summary>
        [field: Tooltip("The world's gravity.")]
        [field: SerializeField]
        public float3 Gravity { get; set; } = new float3(0, -9.81f, 0);

        /// <summary>
        /// Gets or sets the max velocity of particles.
        /// </summary>
        /// <remarks>
        /// Fast moving particles can pass through objects if time step is too large.
        /// This value can be used to limit their max velocity and mitigate this issue.
        /// </remarks>
        [field: Tooltip("Max velocity of particles.")]
        [field: SerializeField]
        public float MaxVelocity { get; set; } = 10f;

        /// <summary>
        /// Gets or sets the sleep velocity threshold.
        /// Particles with a speed lower than this will appear static.
        /// </summary>
        /// <remarks>
        /// This value can be tuned to make particles appear static when they are moving slowly or eliminate jittering.
        /// </remarks>
        [field: Tooltip("The speed threshold in order for particles to sleep. If a particle's speed is lower than this it will appear static.")]
        [field: SerializeField]
        public float SleepVelocityThreshold { get; set; } = 0.01f;

        /// <summary>
        /// Gets or sets the particle radius, used for collisions.
        /// </summary>
        [field: Tooltip("The radius of the particles. This is used for collision detection.")]
        [field: SerializeField]
        public float ParticleRadius { get; set; } = 0.1f;

        /// <summary>
        /// Gets or sets the solver iterations. Each time step, the solver will solve constraints for this many iterations.
        /// </summary>
        [field: Tooltip("The number of iterations to run the physics simulation.")]
        [field: SerializeField]
        public int SolverIterations { get; set; } = 2;

        /// <summary>
        /// Gets or sets the floor height.
        /// </summary>
        [field: Tooltip("The height of the floor plane.")]
        [field: SerializeField]
        public float FloorHeight { get; set; } = 0f;

        /// <summary>
        /// Gets the UnityEvent invoked after a physics update has been completed.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Physics Update. Like Unity's fixed update. Any code that modifies the physics world should be done here.")]
        public UnityEvent<float> PhysicsUpdate { get; private set; } = new UnityEvent<float>();

        /// <summary>
        /// Gets the UnityEvent invoked after a physics update has been completed.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Late Physics Update. Called immediately after Physics Update.")]
        public UnityEvent<float> LatePhysicsUpdate { get; private set; } = new UnityEvent<float>();

        /// <summary>
        /// Gets the update event for all detectors. Called immediately after LatePhysicsUpdate. During this step no further modifications to the mesh should be done.
        /// </summary>
        public UnityEvent DetectorsUpdate { get; private set; } = new UnityEvent();

        /// <summary>
        /// Gets or sets a value indicating whether to solve collision constraints or not.
        /// </summary>
        [field: Space(5)]
        [field: Header("Collisions")]
        [field: Tooltip("If true, the physics world will solve collision constraints between particles.")]
        [field: SerializeField]
        public bool SolveCollisionConstraints { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to apply friction between colliding particles.
        /// </summary>
        [field: Tooltip("Apply friction between colliding particles?")]
        [field: SerializeField]
        public bool ContactFriction { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating the maximum number of collisions in a simulation step.
        /// </summary>
        [field: Tooltip("The maximum number of collision pairs.")]
        [field: SerializeField]
        public int MaxCollisionPairs { get; set; } = 10000;

        /// <summary>
        /// Gets or sets the sub steps for collision constraints.
        /// </summary>
        [field: Tooltip("The number of iterations for solving collision constraints.")]
        [field: SerializeField]
        public int CollisionConstraintSolverSubSteps { get; set; } = 1;

        /// <summary>
        /// Gets or sets the relaxation factor for collision constraints.
        /// </summary>
        [field: Range(0.0f, 2.0f)]
        [field: SerializeField]
        public float CollisionConstraintsRelaxationFactor { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets a value indicating whether to solve distance constraints or not.
        /// </summary>
        [field: Space(5)]
        [field: Header("Distance Constraints")]
        [field: Tooltip("If true, the physics world will solve distance constraints.")]
        [field: SerializeField]
        public bool SolveDistanceConstraints { get; set; } = true;

        /// <summary>
        /// Gets or sets the sub steps for distance constraints.
        /// </summary>
        [field: Tooltip("The number of iterations for solving distance constraints.")]
        [field: SerializeField]
        public int DistanceConstraintSolverSubSteps { get; set; } = 4;

        /// <summary>
        /// Gets or sets the relaxation factor for distance constraints.
        /// </summary>
        [field: Range(0.0f, 2.0f)]
        [field: SerializeField]
        public float DistanceConstraintsRelaxationFactor { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets a value indicating whether to solve volume constraints or not.
        /// </summary>
        [field: Space(5)]
        [field: Header("Volume Constraints")]
        [field: Tooltip("If true, the physics world will solve volume constraints.")]
        [field: SerializeField]
        public bool SolveVolumeConstraints { get; set; } = true;

        /// <summary>
        /// Gets or sets the sub steps for volume constraints.
        /// </summary>
        [field: Tooltip("The number of iterations for solving volume constraints.")]
        [field: SerializeField]
        public int VolumeConstraintSolverSubSteps { get; set; } = 4;

        /// <summary>
        /// Gets or sets the relaxation factor for volume constraints.
        /// </summary>
        [field: Range(0.0f, 2.0f)]
        [field: SerializeField]
        public float VolumeConstraintsRelaxationFactor { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets a value indicating whether to solve bending/collinear constraints or not.
        /// </summary>
        [field: Space(5)]
        [field: Header("Collinear Constraints")]
        [field: SerializeField]
        public bool SolveCollinearConstraints { get; set; } = true;

        /// <summary>
        /// Gets or sets the sub steps for bending/collinear constraints.
        /// </summary>
        [field: SerializeField]
        public int CollinearConstraintSolverSubSteps { get; set; } = 4;

        /// <summary>
        /// Gets or sets the relaxation factor for bending/collinear constraints.
        /// </summary>
        [field: Range(0.0f, 2.0f)]
        [field: SerializeField]
        public float CollinearConstraintsRelaxationFactor { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets a value indicating whether to solve shape matching constraints or not.
        /// </summary>
        [field: Space(5)]
        [field: Header("Shape Matching Constraints")]
        [field: Tooltip("If true, the physics world will solve shape matching constraints.")]
        [field: SerializeField]
        public bool SolveShapeMatchingConstraints { get; set; } = true;

        /// <summary>
        /// Gets or sets the sub steps for shape matching constraints.
        /// </summary>
        [field: Tooltip("The number of iterations for solving shape matching constraints.")]
        [field: SerializeField]
        public int ShapeMatchingConstraintSolverSubSteps { get; set; } = 1;

        /// <summary>
        /// Gets or sets the relaxation factor for shape matching constraints.
        /// </summary>
        [field: Range(0.0f, 2.0f)]
        [field: SerializeField]
        public float ShapeMatchingConstraintsRelaxationFactor { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets a value indicating whether to solve any extra registered constraint or not.
        /// </summary>
        /// <remarks>
        /// Extra constraints are user-defined constraints that are solved after the built-in constraints and implement the <see cref="IConstraintSolver"/> interface.
        /// </remarks>
        [field: Space(5)]
        [field: Header("Extra Constraints")]
        [field: Tooltip("If true, the physics world will solve all extra constraints registered to it, including joint constraints.")]
        [field: SerializeField]
        public bool SolveExtraConstraints { get; set; } = true;

        /// <summary>
        /// Creates a new phase for the physics world.
        /// </summary>
        /// <param name="layer">The collision layer.</param>
        /// <param name="options">The phase options.</param>
        /// <returns>The created phase.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint CreatePhase(uint layer, PhaseOptions options)
        {
            
            layer = layer << 2;
            layer = layer >> 2;

            
            layer |= (uint)options;

            return layer;
        }

        /// <summary>
        /// Extracts the layer from the given phase.
        /// </summary>
        /// <param name="phase">The phase.</param>
        /// <returns>The layer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint GetLayerFromPhase(uint phase)
        {
            
            return (phase << 2) >> 2;
        }

        /// <summary>
        /// Adds a simulation mesh to the physics world.
        /// </summary>
        /// <param name="simulationMesh">The simulation mesh to add.</param>
        /// <param name="trsMatrix">The TRS matrix for the simulation mesh.</param>
        /// <param name="phase">The partilces' phase.</param>
        /// <remarks>This operation is slow and should not be used in a per frame basis.</remarks>
        public void AddSimulationMesh(SimulationMesh simulationMesh, float4x4 trsMatrix, uint phase)
        {
            NpRgNYqXOQnxSLUeACnXuoJOSxlSElhe.Enqueue(new AddSimulationMeshCommand(simulationMesh, trsMatrix, phase));
        }

        /// <summary>
        /// Removes a simulation mesh from the physics world.
        /// </summary>
        /// <param name="simulationMesh">The simulation mesh to remove.</param>
        /// <remarks>This operation is slow and should not be used in a per frame basis.</remarks>
        public void RemoveSimulationMesh(SimulationMesh simulationMesh)
        {
            NpRgNYqXOQnxSLUeACnXuoJOSxlSElhe.Enqueue(new RemoveSimulationMeshCommand(simulationMesh));
        }

        /// <summary>
        /// Gets the simulation mesh for the given simulation mesh descriptor index.
        /// </summary>
        /// <param name="simulationMeshDescriptorIndex">The simulation mesh descriptor index.</param>
        /// <returns>The simulation mesh.</returns>
        public SimulationMesh GetSimulationMesh(int simulationMeshDescriptorIndex)
        {
            DescriptorToSimulationMeshMap.TryGetValue(simulationMeshDescriptorIndex, out SimulationMesh simMesh);
            return simMesh;
        }

        /// <summary>
        /// Gets the simulation mesh descriptor index of the <paramref name="simulationMesh"/>.
        /// </summary>
        /// <param name="simulationMesh">The simulation mesh to get the descriptor index for.</param>
        /// <returns>The descriptor index or -1 if the <paramref name="simulationMesh"/> is not part of the physics world.</returns>
        public int GetSimulationMeshDescriptorIndex(SimulationMesh simulationMesh)
        {
            if (SimulationMeshToDescriptorMap.TryGetValue(simulationMesh, out var descIndex))
            {
                return descIndex;
            }

            return -1;
        }

        /// <summary>
        /// Updates the phase of the particles on the given simulation mesh.
        /// </summary>
        /// <param name="simulationMesh">The simulation mesh.</param>
        /// <param name="newPhase">The new phase to change particles to.</param>
        public void UpdatePhase(SimulationMesh simulationMesh, uint newPhase)
        {
            NpRgNYqXOQnxSLUeACnXuoJOSxlSElhe.Enqueue(new UpdateSimulationMeshPhaseCommand(simulationMesh, newPhase));
        }

        /// <summary>
        /// Moves forward the physics simulation by the given <paramref name="timeStep"/>.
        /// </summary>
        /// <param name="timeStep">The time step.</param>
        public void Step(float timeStep)
        {
            Debug.Assert(timeStep > 0.000001f, "Time step should be greater than 0.000001.");

            
            if (aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Scheduled && aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Handle.IsCompleted)
            {
                aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Handle.Complete();
                aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Scheduled = false;
                jtqkLUcZBepZjdoksIhuZocEZREaWdyy();
            }

            aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Handle = MXoISoZeEVOBMEiSkJtvEcnWqTtwqFUI(timeStep);
            aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Handle.Complete();
        }

        /// <summary>
        /// Spatially queries for particles in the physics world.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="maxDistance">The max distance from this position.</param>
        /// <param name="queryResults">Referece to the array in which to store the results. The array should be sufficient size to store them.</param>
        /// <returns>The number of the results.</returns>
        public int QueryParticles(float3 position, float maxDistance, ref NativeArray<int> queryResults)
        {
            if (kUpSPIkKSjSJaVYamQPQNyytrRTDGYLp)
            {
                Debug.LogError("You can only query particles in the physics update loop", this);
                return 0;
            }

            unsafe
            {
                if (SpatialHashGrid != null)
                {
                    SpatialHashGrid.Query(position, maxDistance);
                    queryResults = SpatialHashGrid.QueryResults;

                    return *SpatialHashGrid.QueryResultsCount.Counter;
                }

                return 0;
            }
        }

        /// <summary>
        /// Registers a custom constraint solver.
        /// </summary>
        /// <param name="solver">The solver to register.</param>
        internal void RegisterConstraintSolver(IConstraintSolver solver)
        {
            vZHHMDAlQnZtdxCQQDCgQiLYmOlZXfOU.Add(solver);

            vZHHMDAlQnZtdxCQQDCgQiLYmOlZXfOU.Sort((a, b) => a.ExecutionOrder.CompareTo(b.ExecutionOrder));
        }

        /// <summary>
        /// Deregisters a custom constraint solver.
        /// </summary>
        /// <param name="solver">The solver to remove.</param>
        /// <seealso cref="RegisterConstraintSolver(IConstraintSolver)"/>
        internal void DeregisterConstraintSolver(IConstraintSolver solver)
        {
            vZHHMDAlQnZtdxCQQDCgQiLYmOlZXfOU.Remove(solver);
        }

        /// <summary>
        /// Re assign the data slices to the simulation meshes.
        /// </summary>
        internal void ReAssignDataSlicesToSimulationMeshes()
        {
            for (int MpfVwlvbYsIjWReUmoObIrtNmuihyKcG = 0; MpfVwlvbYsIjWReUmoObIrtNmuihyKcG < SimulationMeshDescriptorData.Count; MpfVwlvbYsIjWReUmoObIrtNmuihyKcG++)
            {
                SimulationMeshDescriptor oStsMobcJcwBNlbURaphYbngNWIsrqMn = SimulationMeshDescriptorData.SimulationMeshDescriptors[MpfVwlvbYsIjWReUmoObIrtNmuihyKcG];

                SimulationMesh zwgZxdlEcTyauufRsahxEdftMFgefpEP = DescriptorToSimulationMeshMap[MpfVwlvbYsIjWReUmoObIrtNmuihyKcG];

                zwgZxdlEcTyauufRsahxEdftMFgefpEP.RuntimeParticleData = ParticleData.Slice(oStsMobcJcwBNlbURaphYbngNWIsrqMn.ParticleStartIndex, oStsMobcJcwBNlbURaphYbngNWIsrqMn.ParticleCount);
                zwgZxdlEcTyauufRsahxEdftMFgefpEP.RuntimeDistanceConstraintData = DistanceConstraintData.Slice(oStsMobcJcwBNlbURaphYbngNWIsrqMn.DistanceConstraintStartIndex, oStsMobcJcwBNlbURaphYbngNWIsrqMn.DistanceConstraintCount);
                zwgZxdlEcTyauufRsahxEdftMFgefpEP.RuntimeVolumeConstraintData = VolumeConstraintData.Slice(oStsMobcJcwBNlbURaphYbngNWIsrqMn.VolumeConstraintStartIndex, oStsMobcJcwBNlbURaphYbngNWIsrqMn.VolumeConstraintCount);
                zwgZxdlEcTyauufRsahxEdftMFgefpEP.RuntimeShapeMatchingConstraintData = ShapeMatchingConstraintData.Slice(oStsMobcJcwBNlbURaphYbngNWIsrqMn.ShapeMatchingConstraintStartIndex, oStsMobcJcwBNlbURaphYbngNWIsrqMn.ShapeMatchingConstraintCount);
                zwgZxdlEcTyauufRsahxEdftMFgefpEP.RuntimeCollinearConstraintData = CollinearConstraintData.Slice(oStsMobcJcwBNlbURaphYbngNWIsrqMn.CollinearConstraintStartIndex, oStsMobcJcwBNlbURaphYbngNWIsrqMn.CollinearConstraintCount);
            }
        }

        /// <summary>
        /// Reassigns the particle data slices to the simulation meshes.
        /// </summary>
        internal void ReAssignParticleDataSlicesToSimulationMeshes()
        {
            for (int cppAqspiXacROKcAVlzevYHVJZpqZcLY = 0; cppAqspiXacROKcAVlzevYHVJZpqZcLY < SimulationMeshDescriptorData.Count; cppAqspiXacROKcAVlzevYHVJZpqZcLY++)
            {
                SimulationMeshDescriptor YhkkVBSvhjAMnYVOIDAGlQlNtwBWnqWb = SimulationMeshDescriptorData.SimulationMeshDescriptors[cppAqspiXacROKcAVlzevYHVJZpqZcLY];

                SimulationMesh MbnOFRBAZJUEpCZRObuEuNxduIofCcgd = DescriptorToSimulationMeshMap[cppAqspiXacROKcAVlzevYHVJZpqZcLY];

                MbnOFRBAZJUEpCZRObuEuNxduIofCcgd.RuntimeParticleData = ParticleData.Slice(YhkkVBSvhjAMnYVOIDAGlQlNtwBWnqWb.ParticleStartIndex, YhkkVBSvhjAMnYVOIDAGlQlNtwBWnqWb.ParticleCount);
            }
        }

        /// <summary>
        /// Reassigns the distance constraint data slices to the simulation meshes.
        /// </summary>
        internal void ReAssignDistanceConstraintDataSlicesToSimulationMeshes()
        {
            for (int sugRSgDRwuvHYtBIwSsPDwZlQHYxVMqa = 0; sugRSgDRwuvHYtBIwSsPDwZlQHYxVMqa < SimulationMeshDescriptorData.Count; sugRSgDRwuvHYtBIwSsPDwZlQHYxVMqa++)
            {
                SimulationMeshDescriptor vrHvKJvfgQJLfWhtgCuDzuwPDXopOgrX = SimulationMeshDescriptorData.SimulationMeshDescriptors[sugRSgDRwuvHYtBIwSsPDwZlQHYxVMqa];

                SimulationMesh cyNApAjLMnQNRSPbThaxNNZunSxAigSV = DescriptorToSimulationMeshMap[sugRSgDRwuvHYtBIwSsPDwZlQHYxVMqa];

                cyNApAjLMnQNRSPbThaxNNZunSxAigSV.RuntimeDistanceConstraintData = DistanceConstraintData.Slice(vrHvKJvfgQJLfWhtgCuDzuwPDXopOgrX.DistanceConstraintStartIndex, vrHvKJvfgQJLfWhtgCuDzuwPDXopOgrX.DistanceConstraintCount);
            }
        }

        /// <summary>
        /// Reassigns the volume constraint data slices to the simulation meshes.
        /// </summary>
        internal void ReAssignVolumeConstraintDataSlicesToSimulationMeshes()
        {
            for (int iAIOVovRdrVobgZYsaMOemmRYePMLiyq = 0; iAIOVovRdrVobgZYsaMOemmRYePMLiyq < SimulationMeshDescriptorData.Count; iAIOVovRdrVobgZYsaMOemmRYePMLiyq++)
            {
                SimulationMeshDescriptor xajHpYOHMfFWLOMCTIxVoIdJeCTWVFfe = SimulationMeshDescriptorData.SimulationMeshDescriptors[iAIOVovRdrVobgZYsaMOemmRYePMLiyq];

                SimulationMesh DAREJJondbKTtmImXDVzvGYYzNcnOmzB = DescriptorToSimulationMeshMap[iAIOVovRdrVobgZYsaMOemmRYePMLiyq];

                DAREJJondbKTtmImXDVzvGYYzNcnOmzB.RuntimeVolumeConstraintData = VolumeConstraintData.Slice(xajHpYOHMfFWLOMCTIxVoIdJeCTWVFfe.VolumeConstraintStartIndex, xajHpYOHMfFWLOMCTIxVoIdJeCTWVFfe.VolumeConstraintCount);
            }
        }

        /// <summary>
        /// Reassigns the shape matching constraint data slices to the simulation meshes.
        /// </summary>
        internal void ReAssignCollinearConstraintDataSlicesToSimulationMeshes()
        {
            for (int BBjhKEPIGedjvhaZOCtnZxiVTemhffme = 0; BBjhKEPIGedjvhaZOCtnZxiVTemhffme < SimulationMeshDescriptorData.Count; BBjhKEPIGedjvhaZOCtnZxiVTemhffme++)
            {
                SimulationMeshDescriptor vUyjdapDVoERvkKsrYUTsFtCivvOgNJc = SimulationMeshDescriptorData.SimulationMeshDescriptors[BBjhKEPIGedjvhaZOCtnZxiVTemhffme];

                SimulationMesh WOXorHImFvaIanrPCSfnRtgcQWQIAoKI = DescriptorToSimulationMeshMap[BBjhKEPIGedjvhaZOCtnZxiVTemhffme];

                WOXorHImFvaIanrPCSfnRtgcQWQIAoKI.RuntimeCollinearConstraintData = CollinearConstraintData.Slice(vUyjdapDVoERvkKsrYUTsFtCivvOgNJc.CollinearConstraintStartIndex, vUyjdapDVoERvkKsrYUTsFtCivvOgNJc.CollinearConstraintCount);
            }
        }

        /// <summary>
        /// Reassigns the shape matching constraint data slices to the simulation meshes.
        /// </summary>
        internal void ReAssignShapeMatchingConstraintDataSlicesToSimulationMeshes()
        {
            for (int RJBqYqmbjSZytsskyihWcSCRXZQSXqGO = 0; RJBqYqmbjSZytsskyihWcSCRXZQSXqGO < SimulationMeshDescriptorData.Count; RJBqYqmbjSZytsskyihWcSCRXZQSXqGO++)
            {
                SimulationMeshDescriptor doTEpEUZIpHjiNDuMzJFIkqAmWWKvfAd = SimulationMeshDescriptorData.SimulationMeshDescriptors[RJBqYqmbjSZytsskyihWcSCRXZQSXqGO];

                SimulationMesh BZOBHMwxoLlRCFPLYowtSGHNhjEzqLUx = DescriptorToSimulationMeshMap[RJBqYqmbjSZytsskyihWcSCRXZQSXqGO];

                BZOBHMwxoLlRCFPLYowtSGHNhjEzqLUx.RuntimeShapeMatchingConstraintData = ShapeMatchingConstraintData.Slice(doTEpEUZIpHjiNDuMzJFIkqAmWWKvfAd.VolumeConstraintStartIndex, doTEpEUZIpHjiNDuMzJFIkqAmWWKvfAd.VolumeConstraintCount);
            }
        }

        

        /// <summary>
        /// Resizes the particle data for this simulation mesh and adds the given particles.
        /// </summary>
        /// <param name="simulationMesh">The simulation mesh to which these particles belong to.</param>
        /// <param name="particlesEnabled">The enabled state of the particles.</param>
        /// <param name="particlesBreakable">The breakable flag of the particles.</param>
        /// <param name="particlesPhysicsMaterialIndex">The material index for each particle.</param>
        /// <param name="particlesInitialPosition">The initial positions for each particle.</param>
        /// <param name="particlesPositions">The positions for each particle.</param>
        /// <param name="particlesVelocity">The velocity for each particle.</param>
        /// <param name="particlesInverseMass">The inverse mass for each particle.</param>
        /// <param name="particlesPhase">The phase for each particle.</param>
        /// <returns>The index at which the new added particles start in the particle data slice.</returns>
        internal int AddRuntimeParticles(
            in SimulationMesh simulationMesh,
            in bool[] particlesEnabled,
            in bool[] particlesBreakable,
            in int[] particlesPhysicsMaterialIndex,
            in float3[] particlesInitialPosition,
            in float3[] particlesPositions,
            in float3[] particlesVelocity,
            in float[] particlesInverseMass,
            in uint[] particlesPhase)
        {
            UnityEngine.Debug.Assert(particlesEnabled.Length == particlesBreakable.Length &&
                                     particlesEnabled.Length == particlesInitialPosition.Length &&
                                     particlesEnabled.Length == particlesInitialPosition.Length &&
                                     particlesEnabled.Length == particlesPositions.Length &&
                                     particlesEnabled.Length == particlesVelocity.Length &&
                                     particlesEnabled.Length == particlesInverseMass.Length &&
                                     particlesEnabled.Length == particlesPhase.Length);

            
            if (!SimulationMeshToDescriptorMap.TryGetValue(simulationMesh, out var smdIndex))
            {
                return 0;
            }

            int qPivROsRyhuStZIpBMGfNuhJFsiQHIxc = particlesEnabled.Length;

            SimulationMeshDescriptor jrDCTTFNiDQFzaZRsOxzNTTtiiVCADvR = SimulationMeshDescriptorData.SimulationMeshDescriptors[smdIndex];

            
            if (jrDCTTFNiDQFzaZRsOxzNTTtiiVCADvR.ParticleCount + qPivROsRyhuStZIpBMGfNuhJFsiQHIxc > jrDCTTFNiDQFzaZRsOxzNTTtiiVCADvR.TotalParticleSpaceCount)
            {
                
                int YkXALarODQlJdiGQhJOaaqUJXaFAMjEK = math.max(jrDCTTFNiDQFzaZRsOxzNTTtiiVCADvR.TotalParticleSpaceCount, 1);
                while (YkXALarODQlJdiGQhJOaaqUJXaFAMjEK < qPivROsRyhuStZIpBMGfNuhJFsiQHIxc)
                {
                    YkXALarODQlJdiGQhJOaaqUJXaFAMjEK *= 2;
                }

                
                ParticleData.InsertAdditionalEmptySpace(YkXALarODQlJdiGQhJOaaqUJXaFAMjEK, jrDCTTFNiDQFzaZRsOxzNTTtiiVCADvR.ParticleStartIndex + jrDCTTFNiDQFzaZRsOxzNTTtiiVCADvR.ParticleCount);

                jrDCTTFNiDQFzaZRsOxzNTTtiiVCADvR.TotalParticleSpaceCount += YkXALarODQlJdiGQhJOaaqUJXaFAMjEK;

                
                for (int coOqXZiyWgXeDUHMeyJZgFovojIGrbXN = smdIndex; coOqXZiyWgXeDUHMeyJZgFovojIGrbXN < SimulationMeshDescriptorData.Count; coOqXZiyWgXeDUHMeyJZgFovojIGrbXN++)
                {
                    if (coOqXZiyWgXeDUHMeyJZgFovojIGrbXN == smdIndex)
                    {
                        SimulationMeshDescriptorData.SimulationMeshDescriptors[coOqXZiyWgXeDUHMeyJZgFovojIGrbXN] = jrDCTTFNiDQFzaZRsOxzNTTtiiVCADvR;
                        continue;
                    }
                    else
                    {
                        var pKSZGlCQXmDHIqRVvCStAQxqunJAlgVw = SimulationMeshDescriptorData.SimulationMeshDescriptors[coOqXZiyWgXeDUHMeyJZgFovojIGrbXN];
                        pKSZGlCQXmDHIqRVvCStAQxqunJAlgVw.ParticleStartIndex += YkXALarODQlJdiGQhJOaaqUJXaFAMjEK;
                        SimulationMeshDescriptorData.SimulationMeshDescriptors[coOqXZiyWgXeDUHMeyJZgFovojIGrbXN] = pKSZGlCQXmDHIqRVvCStAQxqunJAlgVw;
                    }
                }
            }

            
            var cTVRUiywRmFyIPAbGhfCCDDDUXaeXmFF = jrDCTTFNiDQFzaZRsOxzNTTtiiVCADvR.ParticleStartIndex + jrDCTTFNiDQFzaZRsOxzNTTtiiVCADvR.ParticleCount;
            var CwLmKZbzraXARhCqIZLcTuvmpxookDzi = cTVRUiywRmFyIPAbGhfCCDDDUXaeXmFF;
            for (int CKLeukosNwjVulnpevIqqsVdiRaByIlP = 0; CKLeukosNwjVulnpevIqqsVdiRaByIlP < qPivROsRyhuStZIpBMGfNuhJFsiQHIxc; CKLeukosNwjVulnpevIqqsVdiRaByIlP++)
            {
                ParticleData.Enabled[CwLmKZbzraXARhCqIZLcTuvmpxookDzi] = particlesEnabled[CKLeukosNwjVulnpevIqqsVdiRaByIlP];
                ParticleData.Breakable[CwLmKZbzraXARhCqIZLcTuvmpxookDzi] = particlesBreakable[CKLeukosNwjVulnpevIqqsVdiRaByIlP];

                ParticleData.PhysicsMaterialIndex[CwLmKZbzraXARhCqIZLcTuvmpxookDzi] = particlesPhysicsMaterialIndex[CKLeukosNwjVulnpevIqqsVdiRaByIlP];

                ParticleData.InitialPosition[CwLmKZbzraXARhCqIZLcTuvmpxookDzi] = particlesInitialPosition[CKLeukosNwjVulnpevIqqsVdiRaByIlP];
                ParticleData.Position[CwLmKZbzraXARhCqIZLcTuvmpxookDzi] = particlesPositions[CKLeukosNwjVulnpevIqqsVdiRaByIlP];

                
                ParticleData.LocalRestPosition[CwLmKZbzraXARhCqIZLcTuvmpxookDzi] = particlesPositions[CKLeukosNwjVulnpevIqqsVdiRaByIlP] - SimulationMeshDescriptorData.CenterOfMassPosition[smdIndex];
                ParticleData.PredictedPosition[CwLmKZbzraXARhCqIZLcTuvmpxookDzi] = particlesPositions[CKLeukosNwjVulnpevIqqsVdiRaByIlP];

                ParticleData.Velocity[CwLmKZbzraXARhCqIZLcTuvmpxookDzi] = particlesVelocity[CKLeukosNwjVulnpevIqqsVdiRaByIlP];
                ParticleData.InverseMass[CwLmKZbzraXARhCqIZLcTuvmpxookDzi] = particlesInverseMass[CKLeukosNwjVulnpevIqqsVdiRaByIlP];
                ParticleData.Phase[CwLmKZbzraXARhCqIZLcTuvmpxookDzi] = particlesPhase[CKLeukosNwjVulnpevIqqsVdiRaByIlP];

                ParticleData.SimulationMeshDescriptorIndex[CwLmKZbzraXARhCqIZLcTuvmpxookDzi] = smdIndex;

                ParticleData.ParticleLocks[CwLmKZbzraXARhCqIZLcTuvmpxookDzi] = 0;
                ParticleData.PositionCorrections[CwLmKZbzraXARhCqIZLcTuvmpxookDzi] = float3.zero;
                ParticleData.PositionCorrectionsCount[CwLmKZbzraXARhCqIZLcTuvmpxookDzi] = 0;

                CwLmKZbzraXARhCqIZLcTuvmpxookDzi++;
            }

            jrDCTTFNiDQFzaZRsOxzNTTtiiVCADvR.ParticleCount += qPivROsRyhuStZIpBMGfNuhJFsiQHIxc;

            SimulationMeshDescriptorData.SimulationMeshDescriptors[smdIndex] = jrDCTTFNiDQFzaZRsOxzNTTtiiVCADvR;

            
            if (SpatialHashGrid != null)
            {
                SpatialHashGrid.SetItemsCount(ParticleData.Count);
            }

            
            ReAssignParticleDataSlicesToSimulationMeshes();

            Debug.Log($"Added particles to physics world: [P, DC, VC, SMC]= [{ParticleData.Count}, {DistanceConstraintData.Count}, {VolumeConstraintData.Count}, {CollinearConstraintData.Count}, {ShapeMatchingConstraintData.Count}]");

            return cTVRUiywRmFyIPAbGhfCCDDDUXaeXmFF - jrDCTTFNiDQFzaZRsOxzNTTtiiVCADvR.ParticleStartIndex;
        }

        /// <summary>
        /// Resizes the distance constraints data for this simulation mesh and adds the given distance constraints.
        /// </summary>
        /// <param name="simulationMesh">The simulation mesh to which these distance constraints belong to.</param>
        /// <param name="distanceConstraintsEnabled">The enabled state of the distance constraints.</param>
        /// <param name="distanceConstraintsBreakable">The breakable flag of the distance constraints.</param>
        /// <param name="distanceConstraintsPhysicsMaterialIndex">The physics material index for the distance constraints.</param>
        /// <param name="distanceConstraintsParticleA">The ParticleA for the distance constraints.</param>
        /// <param name="distanceConstraintsParticleB">The ParticleB for the distance constraints.</param>
        /// <param name="distanceConstraintsRestDistance">The distance of the constraints at rest configuration.</param>
        /// <returns>The index at which the new added distance constraints start in the distance constraint data slice.</returns>
        internal int AddRuntimeDistanceConstraints(
            in SimulationMesh simulationMesh,
            in bool[] distanceConstraintsEnabled,
            in bool[] distanceConstraintsBreakable,
            in int[] distanceConstraintsPhysicsMaterialIndex,
            in int[] distanceConstraintsParticleA,
            in int[] distanceConstraintsParticleB,
            in float[] distanceConstraintsRestDistance)
        {
            UnityEngine.Debug.Assert(distanceConstraintsEnabled.Length == distanceConstraintsBreakable.Length &&
                                     distanceConstraintsEnabled.Length == distanceConstraintsParticleA.Length &&
                                     distanceConstraintsEnabled.Length == distanceConstraintsParticleB.Length &&
                                     distanceConstraintsEnabled.Length == distanceConstraintsRestDistance.Length &&
                                     distanceConstraintsEnabled.Length == distanceConstraintsPhysicsMaterialIndex.Length);

            
            if (!SimulationMeshToDescriptorMap.TryGetValue(simulationMesh, out var smdIndex))
            {
                return 0;
            }

            int WWRirHxzjrYFFsKLlCaQWmwTSudwGGiR = distanceConstraintsEnabled.Length;

            SimulationMeshDescriptor pRukCyrKKkUjEHlSlgOKvvGhCoeqWQZC = SimulationMeshDescriptorData.SimulationMeshDescriptors[smdIndex];

            
            if (pRukCyrKKkUjEHlSlgOKvvGhCoeqWQZC.DistanceConstraintCount + WWRirHxzjrYFFsKLlCaQWmwTSudwGGiR > pRukCyrKKkUjEHlSlgOKvvGhCoeqWQZC.TotalDistanceConstraintSpaceCount)
            {
                
                int agfUkHynJTaGnHDVXFXVwwYppMXOTTbV = math.max(pRukCyrKKkUjEHlSlgOKvvGhCoeqWQZC.TotalDistanceConstraintSpaceCount, 1);
                while (agfUkHynJTaGnHDVXFXVwwYppMXOTTbV < WWRirHxzjrYFFsKLlCaQWmwTSudwGGiR)
                {
                    agfUkHynJTaGnHDVXFXVwwYppMXOTTbV *= 2;
                }

                
                DistanceConstraintData.InsertAdditionalEmptySpace(agfUkHynJTaGnHDVXFXVwwYppMXOTTbV, pRukCyrKKkUjEHlSlgOKvvGhCoeqWQZC.DistanceConstraintStartIndex + pRukCyrKKkUjEHlSlgOKvvGhCoeqWQZC.DistanceConstraintCount);

                pRukCyrKKkUjEHlSlgOKvvGhCoeqWQZC.TotalDistanceConstraintSpaceCount += agfUkHynJTaGnHDVXFXVwwYppMXOTTbV;

                
                for (int zPCVOVInrMBMkYRXgAcNlwybKFirUekN = smdIndex; zPCVOVInrMBMkYRXgAcNlwybKFirUekN < SimulationMeshDescriptorData.Count; zPCVOVInrMBMkYRXgAcNlwybKFirUekN++)
                {
                    if (zPCVOVInrMBMkYRXgAcNlwybKFirUekN == smdIndex)
                    {
                        SimulationMeshDescriptorData.SimulationMeshDescriptors[zPCVOVInrMBMkYRXgAcNlwybKFirUekN] = pRukCyrKKkUjEHlSlgOKvvGhCoeqWQZC;
                        continue;
                    }
                    else
                    {
                        var pMJGGCHKiBudouBRLSAscWvjnPpbqUEU = SimulationMeshDescriptorData.SimulationMeshDescriptors[zPCVOVInrMBMkYRXgAcNlwybKFirUekN];
                        pMJGGCHKiBudouBRLSAscWvjnPpbqUEU.DistanceConstraintStartIndex += agfUkHynJTaGnHDVXFXVwwYppMXOTTbV;
                        SimulationMeshDescriptorData.SimulationMeshDescriptors[zPCVOVInrMBMkYRXgAcNlwybKFirUekN] = pMJGGCHKiBudouBRLSAscWvjnPpbqUEU;
                    }
                }
            }

            
            var jySoOWfHDfEEQUvGptRQjgZbCuVBoTPY = pRukCyrKKkUjEHlSlgOKvvGhCoeqWQZC.DistanceConstraintStartIndex + pRukCyrKKkUjEHlSlgOKvvGhCoeqWQZC.DistanceConstraintCount;
            var LHezOyRyvwrCLPlbpvnrFiNKaUzhTyCY = jySoOWfHDfEEQUvGptRQjgZbCuVBoTPY;
            for (int TeJoSQHolBnZZNjtQLRcCCjuCCJDWaCs = 0; TeJoSQHolBnZZNjtQLRcCCjuCCJDWaCs < WWRirHxzjrYFFsKLlCaQWmwTSudwGGiR; TeJoSQHolBnZZNjtQLRcCCjuCCJDWaCs++)
            {
                DistanceConstraintData.Enabled[LHezOyRyvwrCLPlbpvnrFiNKaUzhTyCY] = distanceConstraintsEnabled[TeJoSQHolBnZZNjtQLRcCCjuCCJDWaCs];
                DistanceConstraintData.Breakable[LHezOyRyvwrCLPlbpvnrFiNKaUzhTyCY] = distanceConstraintsBreakable[TeJoSQHolBnZZNjtQLRcCCjuCCJDWaCs];

                DistanceConstraintData.PhysicsMaterialIndex[LHezOyRyvwrCLPlbpvnrFiNKaUzhTyCY] = distanceConstraintsPhysicsMaterialIndex[TeJoSQHolBnZZNjtQLRcCCjuCCJDWaCs];

                DistanceConstraintData.ParticleA[LHezOyRyvwrCLPlbpvnrFiNKaUzhTyCY] = distanceConstraintsParticleA[TeJoSQHolBnZZNjtQLRcCCjuCCJDWaCs];
                DistanceConstraintData.ParticleB[LHezOyRyvwrCLPlbpvnrFiNKaUzhTyCY] = distanceConstraintsParticleB[TeJoSQHolBnZZNjtQLRcCCjuCCJDWaCs];

                DistanceConstraintData.RestDistance[LHezOyRyvwrCLPlbpvnrFiNKaUzhTyCY] = distanceConstraintsRestDistance[TeJoSQHolBnZZNjtQLRcCCjuCCJDWaCs];

                DistanceConstraintData.SimulationMeshDescriptorIndex[LHezOyRyvwrCLPlbpvnrFiNKaUzhTyCY] = smdIndex;

                LHezOyRyvwrCLPlbpvnrFiNKaUzhTyCY++;
            }

            pRukCyrKKkUjEHlSlgOKvvGhCoeqWQZC.DistanceConstraintCount += WWRirHxzjrYFFsKLlCaQWmwTSudwGGiR;
            SimulationMeshDescriptorData.SimulationMeshDescriptors[smdIndex] = pRukCyrKKkUjEHlSlgOKvvGhCoeqWQZC;

            
            ReAssignDistanceConstraintDataSlicesToSimulationMeshes();

            Debug.Log($"Added distance constraints to physics world: [P, DC, VC, SMC]= [{ParticleData.Count}, {DistanceConstraintData.Count}, {VolumeConstraintData.Count}, {CollinearConstraintData.Count}, {ShapeMatchingConstraintData.Count}]");

            return jySoOWfHDfEEQUvGptRQjgZbCuVBoTPY - pRukCyrKKkUjEHlSlgOKvvGhCoeqWQZC.DistanceConstraintStartIndex;
        }

        /// <summary>
        /// Resizes the volume constraints data for this simulation mesh and adds the given volume constraints.
        /// </summary>
        /// <param name="simulationMesh">The simulation mesh to which these volume constraints belongs to.</param>
        /// <param name="volumeConstraintsEnabled">The enabled state of the volume constraints.</param>
        /// <param name="volumeConstraintsBreakable">The breakable flag of the volume cosntraints.</param>
        /// <param name="volumeConstraintsPhysicsMaterialIndex">The physics material index for the volume constraints.</param>
        /// <param name="volumeConstraintsParticleA">The ParticleA for the volume constraints.</param>
        /// <param name="volumeConstraintsParticleB">The ParticleB for the volume constraints.</param>
        /// <param name="volumeConstraintsParticleC">The ParticleC for the volume constraints.</param>
        /// <param name="volumeConstraintsParticleD">The ParticleD for the volume constraints.</param>
        /// <param name="volumeConstraintsRestVolume">The volume of the constraints at rest configuration.</param>
        /// <returns>The index at which the new added volume constraints start in the volume constraint data slice.</returns>
        internal int AddRuntimeVolumeConstraints(
            in SimulationMesh simulationMesh,
            in bool[] volumeConstraintsEnabled,
            in bool[] volumeConstraintsBreakable,
            in int[] volumeConstraintsPhysicsMaterialIndex,
            in int[] volumeConstraintsParticleA,
            in int[] volumeConstraintsParticleB,
            in int[] volumeConstraintsParticleC,
            in int[] volumeConstraintsParticleD,
            in float[] volumeConstraintsRestVolume)
        {
            UnityEngine.Debug.Assert(volumeConstraintsEnabled.Length == volumeConstraintsBreakable.Length &&
                                     volumeConstraintsEnabled.Length == volumeConstraintsParticleA.Length &&
                                     volumeConstraintsEnabled.Length == volumeConstraintsParticleB.Length &&
                                     volumeConstraintsEnabled.Length == volumeConstraintsParticleC.Length &&
                                     volumeConstraintsEnabled.Length == volumeConstraintsParticleD.Length &&
                                     volumeConstraintsEnabled.Length == volumeConstraintsRestVolume.Length &&
                                     volumeConstraintsEnabled.Length == volumeConstraintsPhysicsMaterialIndex.Length);

            
            if (!SimulationMeshToDescriptorMap.TryGetValue(simulationMesh, out var smdIndex))
            {
                return 0;
            }

            int QnShZnkbVMkEGkIPeMbxuUYvUUrMCtTx = volumeConstraintsEnabled.Length;

            SimulationMeshDescriptor ORttllMCEbHPNUPxaEOzeFrOejfqsAXM = SimulationMeshDescriptorData.SimulationMeshDescriptors[smdIndex];

            
            if (ORttllMCEbHPNUPxaEOzeFrOejfqsAXM.VolumeConstraintCount + QnShZnkbVMkEGkIPeMbxuUYvUUrMCtTx > ORttllMCEbHPNUPxaEOzeFrOejfqsAXM.TotalVolumeConstraintSpaceCount)
            {
                
                int rGRxwVZIbhTPcBYbRkqFlxIygJvljfHc = math.max(ORttllMCEbHPNUPxaEOzeFrOejfqsAXM.TotalVolumeConstraintSpaceCount, 1);
                while (rGRxwVZIbhTPcBYbRkqFlxIygJvljfHc < QnShZnkbVMkEGkIPeMbxuUYvUUrMCtTx)
                {
                    rGRxwVZIbhTPcBYbRkqFlxIygJvljfHc *= 2;
                }

                
                VolumeConstraintData.InsertAdditionalEmptySpace(rGRxwVZIbhTPcBYbRkqFlxIygJvljfHc, ORttllMCEbHPNUPxaEOzeFrOejfqsAXM.VolumeConstraintStartIndex + ORttllMCEbHPNUPxaEOzeFrOejfqsAXM.VolumeConstraintCount);

                ORttllMCEbHPNUPxaEOzeFrOejfqsAXM.TotalVolumeConstraintSpaceCount += rGRxwVZIbhTPcBYbRkqFlxIygJvljfHc;

                
                for (int gKlfgnKZHyUXJvWLqvPbhifCctvsnujy = smdIndex; gKlfgnKZHyUXJvWLqvPbhifCctvsnujy < SimulationMeshDescriptorData.Count; gKlfgnKZHyUXJvWLqvPbhifCctvsnujy++)
                {
                    if (gKlfgnKZHyUXJvWLqvPbhifCctvsnujy == smdIndex)
                    {
                        SimulationMeshDescriptorData.SimulationMeshDescriptors[gKlfgnKZHyUXJvWLqvPbhifCctvsnujy] = ORttllMCEbHPNUPxaEOzeFrOejfqsAXM;
                        continue;
                    }
                    else
                    {
                        var QZrZBamYOIXxZSQbZOAZPpwkzgLuILHR = SimulationMeshDescriptorData.SimulationMeshDescriptors[gKlfgnKZHyUXJvWLqvPbhifCctvsnujy];
                        QZrZBamYOIXxZSQbZOAZPpwkzgLuILHR.VolumeConstraintStartIndex += rGRxwVZIbhTPcBYbRkqFlxIygJvljfHc;
                        SimulationMeshDescriptorData.SimulationMeshDescriptors[gKlfgnKZHyUXJvWLqvPbhifCctvsnujy] = QZrZBamYOIXxZSQbZOAZPpwkzgLuILHR;
                    }
                }
            }

            
            var tZvtrKjuJOpjmmCxDfEGIQWDrnSHBMve = ORttllMCEbHPNUPxaEOzeFrOejfqsAXM.VolumeConstraintStartIndex + ORttllMCEbHPNUPxaEOzeFrOejfqsAXM.VolumeConstraintCount;
            var NamRUPZxeliiFcPDSYHEzgYFjKRJlgqO = tZvtrKjuJOpjmmCxDfEGIQWDrnSHBMve;
            for (int iVFJOrlxcexAqDAFUPROcvKytEDvmVMJ = 0; iVFJOrlxcexAqDAFUPROcvKytEDvmVMJ < QnShZnkbVMkEGkIPeMbxuUYvUUrMCtTx; iVFJOrlxcexAqDAFUPROcvKytEDvmVMJ++)
            {
                VolumeConstraintData.Enabled[NamRUPZxeliiFcPDSYHEzgYFjKRJlgqO] = volumeConstraintsEnabled[iVFJOrlxcexAqDAFUPROcvKytEDvmVMJ];
                VolumeConstraintData.Breakable[NamRUPZxeliiFcPDSYHEzgYFjKRJlgqO] = volumeConstraintsBreakable[iVFJOrlxcexAqDAFUPROcvKytEDvmVMJ];

                VolumeConstraintData.PhysicsMaterialIndex[NamRUPZxeliiFcPDSYHEzgYFjKRJlgqO] = volumeConstraintsPhysicsMaterialIndex[iVFJOrlxcexAqDAFUPROcvKytEDvmVMJ];

                VolumeConstraintData.ParticleA[NamRUPZxeliiFcPDSYHEzgYFjKRJlgqO] = volumeConstraintsParticleA[iVFJOrlxcexAqDAFUPROcvKytEDvmVMJ];
                VolumeConstraintData.ParticleB[NamRUPZxeliiFcPDSYHEzgYFjKRJlgqO] = volumeConstraintsParticleB[iVFJOrlxcexAqDAFUPROcvKytEDvmVMJ];
                VolumeConstraintData.ParticleC[NamRUPZxeliiFcPDSYHEzgYFjKRJlgqO] = volumeConstraintsParticleC[iVFJOrlxcexAqDAFUPROcvKytEDvmVMJ];
                VolumeConstraintData.ParticleD[NamRUPZxeliiFcPDSYHEzgYFjKRJlgqO] = volumeConstraintsParticleD[iVFJOrlxcexAqDAFUPROcvKytEDvmVMJ];

                VolumeConstraintData.RestVolume[NamRUPZxeliiFcPDSYHEzgYFjKRJlgqO] = volumeConstraintsRestVolume[iVFJOrlxcexAqDAFUPROcvKytEDvmVMJ];

                VolumeConstraintData.SimulationMeshDescriptorIndex[NamRUPZxeliiFcPDSYHEzgYFjKRJlgqO] = smdIndex;

                NamRUPZxeliiFcPDSYHEzgYFjKRJlgqO++;
            }

            ORttllMCEbHPNUPxaEOzeFrOejfqsAXM.VolumeConstraintCount += QnShZnkbVMkEGkIPeMbxuUYvUUrMCtTx;
            SimulationMeshDescriptorData.SimulationMeshDescriptors[smdIndex] = ORttllMCEbHPNUPxaEOzeFrOejfqsAXM;

            
            ReAssignVolumeConstraintDataSlicesToSimulationMeshes();

            Debug.Log($"Added volume constraints to physics world: [P, DC, VC, SMC]= [{ParticleData.Count}, {DistanceConstraintData.Count}, {VolumeConstraintData.Count}, {ShapeMatchingConstraintData.Count}]");

            return tZvtrKjuJOpjmmCxDfEGIQWDrnSHBMve - ORttllMCEbHPNUPxaEOzeFrOejfqsAXM.VolumeConstraintStartIndex;
        }

        private void vJIqTRFSiUmFdYuLGDkwJXGPAmLxkWEO()
        {
            ParticleData.Create(0);
            DistanceConstraintData.Create(0);
            VolumeConstraintData.Create(0);
            CollinearConstraintData.Create(0);
            ShapeMatchingConstraintData.Create(0);
            SimulationMeshDescriptorData.Create(0);

            
            SpatialHashGrid = new SpatialHashGrid(2 * ParticleRadius, 0);
            ContactPairsCount = new NativeReference<int>(Allocator.Persistent);
            ContactPairs = new NativeArray<ContactPair>(MaxCollisionPairs, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        }

        private void EPyaVJKKeWrlqjQzTCAsBaGCOIoaQvRe()
        {
            aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Handle.Complete();
            ParticleData.Dispose();
            DistanceConstraintData.Dispose();
            VolumeConstraintData.Dispose();
            CollinearConstraintData.Dispose();
            ShapeMatchingConstraintData.Dispose();
            SimulationMeshDescriptorData.Dispose();

            ContactPairsCount.Dispose();
            ContactPairs.Dispose();
        }

        private void YqsaYuUDCeVBDlTHbDMfaqAynIkiypcv()
        {
            var eNErGftnorwfBFsiqmoDqQIIClQcSZyR = new Vector3[4]
            {
                new Vector3(-1f + transform.position.x, FloorHeight + transform.position.y, -1f + transform.position.z),
                new Vector3(1f + transform.position.x, FloorHeight + transform.position.y, -1f + transform.position.z),
                new Vector3(1f + transform.position.x, FloorHeight + transform.position.y, 1f + transform.position.z),
                new Vector3(-1f + transform.position.x, FloorHeight + transform.position.y, 1f + transform.position.z),
            };
            Gizmos.DrawLineStrip(eNErGftnorwfBFsiqmoDqQIIClQcSZyR, true);
        }

        private void KTxAZbNjFvqlOFTGnWOQvDkjrSVnwjhp()
        {
            aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf = new JUZnLKejmTkSdgYeXeeIjqZqdhSxtoBD
            {
                PreviousTimeStepTime = Time.timeAsDouble,
                Scheduled = false,
                Handle = default,
            };
        }

        private void wZGSbEorfpZXeIrNItYDNOQkvpzhieeS()
        {
            if (Simulate)
            {
                
                if (aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Scheduled && aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Handle.IsCompleted)
                {
                    aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Handle.Complete();
                    aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Scheduled = false;
                    jtqkLUcZBepZjdoksIhuZocEZREaWdyy();
                }

                
                if (Time.timeAsDouble - aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.PreviousTimeStepTime >= TimeStep)
                {
                    
                    if (aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Scheduled)
                    {
                        aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Handle.Complete();
                        aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Scheduled = false;
                        jtqkLUcZBepZjdoksIhuZocEZREaWdyy();
                    }

                    float vLBidweSQHmHMAdhmPHBamcssrCHEIls = (float)(Time.timeAsDouble - aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.PreviousTimeStepTime);
                    aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.PreviousTimeStepTime = Time.timeAsDouble;
                    aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Scheduled = true;
                    abbHTMsHxKlPFWEgmVxWYTZjsUYTfoEN();
                    aOGtXueYQgCJBRnbTVsjLVAePoKaqFzf.Handle = MXoISoZeEVOBMEiSkJtvEcnWqTtwqFUI(vLBidweSQHmHMAdhmPHBamcssrCHEIls);
                }
            }
        }

        private JobHandle MXoISoZeEVOBMEiSkJtvEcnWqTtwqFUI(float timeStep)
        {
            float CbuoPLtDoLFxeKKsiKHYwwSboMCtStKu = timeStep / SolverIterations;

            float PMMSfFGDfxZKvUIgyRgakQjOXbwStMxC = CbuoPLtDoLFxeKKsiKHYwwSboMCtStKu / CollisionConstraintSolverSubSteps;
            float JyleaSZFVRWURhVcaoBgWDXbXodJnCop = CbuoPLtDoLFxeKKsiKHYwwSboMCtStKu / DistanceConstraintSolverSubSteps;
            float EUrsEtJhbbtlMmgdmnLiHbRBRUeunNqD = CbuoPLtDoLFxeKKsiKHYwwSboMCtStKu / VolumeConstraintSolverSubSteps;
            float ntyPglfrTRULJsmiuUKxmalRtATDbOjz = CbuoPLtDoLFxeKKsiKHYwwSboMCtStKu / CollinearConstraintSolverSubSteps;
            float UfoFNCRMtPvFEjQiERaZWciJNtGUouAf = CbuoPLtDoLFxeKKsiKHYwwSboMCtStKu / ShapeMatchingConstraintSolverSubSteps;

            JobHandle XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = default;

            
            InitializeBufferJob<bool> vSxdecshYnTNBFOCcCpcyYkGCVquHYDP = new InitializeBufferJob<bool>
            {
                Buffer = SimulationMeshDescriptorData.ParticlesEnabledStateHasChanged,
                Value = false,
            };
            InitializeBufferJob<bool> MvGiQryGawLliZyWkjqYBarKuCABeiBR = new InitializeBufferJob<bool>
            {
                Buffer = SimulationMeshDescriptorData.DistanceConstraintsEnabledStateHasChanged,
                Value = false,
            };
            InitializeBufferJob<bool> sYacmRyJvXYZenQhHlPwxaRMdzckZVCG = new InitializeBufferJob<bool>
            {
                Buffer = SimulationMeshDescriptorData.VolumeConstraintsEnabledStateHasChanged,
                Value = false,
            };
            InitializeBufferJob<bool> IhuABfJlRDidSCohkaauORHaOEYFsfpE = new InitializeBufferJob<bool>
            {
                Buffer = SimulationMeshDescriptorData.ShapeMatchingConstraintsEnabledStateHasChanged,
                Value = false,
            };
            InitializeBufferJob<bool> LmSbDlVSXWjDMwnwaKkNSieAWrCiNgNv = new InitializeBufferJob<bool>
            {
                Buffer = SimulationMeshDescriptorData.CollinearConstraintsEnabledStateHasChanged,
                Value = false,
            };
            XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = vSxdecshYnTNBFOCcCpcyYkGCVquHYDP.Schedule(SimulationMeshDescriptorData.Count, 64, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
            XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = MvGiQryGawLliZyWkjqYBarKuCABeiBR.Schedule(SimulationMeshDescriptorData.Count, 64, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
            XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = sYacmRyJvXYZenQhHlPwxaRMdzckZVCG.Schedule(SimulationMeshDescriptorData.Count, 64, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
            XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = IhuABfJlRDidSCohkaauORHaOEYFsfpE.Schedule(SimulationMeshDescriptorData.Count, 64, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
            XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = LmSbDlVSXWjDMwnwaKkNSieAWrCiNgNv.Schedule(SimulationMeshDescriptorData.Count, 64, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);

            
            XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = SpatialHashGrid.RecalculateGrid(ParticleData.PredictedPosition, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
            XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = TfiTfsPeqXeEqKDtRvPZYSZZZFOhWRLR(XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
            XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = hMLkyHxJkISbDqICElooRzPDfRTsbTNN(XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);

            XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = leivJwMZzLTkycKiKiJTBURnDOLgbfak(timeStep, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);

            for (int pPNSyYPDCEPVvwOGYeYiKAexpjdDSaAr = 0; pPNSyYPDCEPVvwOGYeYiKAexpjdDSaAr < SolverIterations; pPNSyYPDCEPVvwOGYeYiKAexpjdDSaAr++)
            {
                
                if (ContactFriction)
                {
                    XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = sMOWkDwTnKdcIZsbLIMbGsOMhFKvmxdw(XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
                }

                
                if (SolveCollisionConstraints && ParticleData.Count > 0)
                {
                    
                    for (int XrTtUCbVDKwmakyLboYZOZmvMqUgbBLs = 0; XrTtUCbVDKwmakyLboYZOZmvMqUgbBLs < CollisionConstraintSolverSubSteps; XrTtUCbVDKwmakyLboYZOZmvMqUgbBLs++)
                    {
                        XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = SOuLdjzZcgGoOabDZmBcQeWQMwORjmRH(XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);

                        XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = EDymCDOxaIJUKNyFQxjfYSNDgFLRHYTo(PMMSfFGDfxZKvUIgyRgakQjOXbwStMxC, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);

                        XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = oGnirSUMygvXCLrFgHHHVowrEjNUstbv(CollisionConstraintsRelaxationFactor, ParticleData.PredictedPosition, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
                    }
                }

                
                if (ContactFriction)
                {
                    XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = SOuLdjzZcgGoOabDZmBcQeWQMwORjmRH(XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
                    XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = PStqRhgadbPWOVulLBBGRMPmgXEGluOK(XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
                    XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = oGnirSUMygvXCLrFgHHHVowrEjNUstbv(1.0f, ParticleData.PredictedPosition, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
                }

                
                if (SolveCollinearConstraints && CollinearConstraintData.Count > 0)
                {
                    for (int tkVbvhAOPLnLETIvFqXruuluFHcoNSXG = 0; tkVbvhAOPLnLETIvFqXruuluFHcoNSXG < CollinearConstraintSolverSubSteps; tkVbvhAOPLnLETIvFqXruuluFHcoNSXG++)
                    {
                        XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = SOuLdjzZcgGoOabDZmBcQeWQMwORjmRH(XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);

                        XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = uqPmnziZDFzVyaruxpzeIdsnkBxIJLfI(ntyPglfrTRULJsmiuUKxmalRtATDbOjz, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);

                        XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = oGnirSUMygvXCLrFgHHHVowrEjNUstbv(CollinearConstraintsRelaxationFactor, ParticleData.PredictedPosition, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
                    }
                }

                
                if (SolveDistanceConstraints && DistanceConstraintData.Count > 0)
                {
                    for (int YykWsImrrIecsWjiIuqRAvfANguTzZPs = 0; YykWsImrrIecsWjiIuqRAvfANguTzZPs < DistanceConstraintSolverSubSteps; YykWsImrrIecsWjiIuqRAvfANguTzZPs++)
                    {
                        XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = SOuLdjzZcgGoOabDZmBcQeWQMwORjmRH(XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);

                        XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = fRkmCUwpjONSXOwizfUKrwSHoxiRAxDC(JyleaSZFVRWURhVcaoBgWDXbXodJnCop, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);

                        XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = oGnirSUMygvXCLrFgHHHVowrEjNUstbv(DistanceConstraintsRelaxationFactor, ParticleData.PredictedPosition, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
                    }
                }

                
                if (SolveVolumeConstraints && VolumeConstraintData.Count > 0)
                {
                    for (int fjXUSPhmtBCvNVyJttFYoSCEpcjtTXcz = 0; fjXUSPhmtBCvNVyJttFYoSCEpcjtTXcz < VolumeConstraintSolverSubSteps; fjXUSPhmtBCvNVyJttFYoSCEpcjtTXcz++)
                    {
                        XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = SOuLdjzZcgGoOabDZmBcQeWQMwORjmRH(XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);

                        XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = VYiBmRHQYMgcnNYyCONwVVksrqemqOWQ(EUrsEtJhbbtlMmgdmnLiHbRBRUeunNqD, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);

                        XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = oGnirSUMygvXCLrFgHHHVowrEjNUstbv(VolumeConstraintsRelaxationFactor, ParticleData.PredictedPosition, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
                    }
                }

                
                if (SolveShapeMatchingConstraints && ShapeMatchingConstraintData.Count > 0)
                {
                    for (int USpRJMmlvRrCxTeGwTtzQbmbEpUTYMke = 0; USpRJMmlvRrCxTeGwTtzQbmbEpUTYMke < VolumeConstraintSolverSubSteps; USpRJMmlvRrCxTeGwTtzQbmbEpUTYMke++)
                    {
                        XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = SOuLdjzZcgGoOabDZmBcQeWQMwORjmRH(XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);

                        XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = eBzoFHoUNLJRPUgYcvPyUWjqZGioQRkK(UfoFNCRMtPvFEjQiERaZWciJNtGUouAf, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);

                        XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = oGnirSUMygvXCLrFgHHHVowrEjNUstbv(ShapeMatchingConstraintsRelaxationFactor, ParticleData.PredictedPosition, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
                    }
                }

                
                if (SolveExtraConstraints && vZHHMDAlQnZtdxCQQDCgQiLYmOlZXfOU.Count > 0)
                {
                    XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = RyZaIDslSqAqTXMIHUAexGxHuVxNrPbC(CbuoPLtDoLFxeKKsiKHYwwSboMCtStKu, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);
                }
            }

            XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA = gLRNYcaFJrwxfdTDGWGOKMnbJkNmUWho(timeStep, XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA);

            return XAKDUFzrzvHLNtHgCFCyuYYYTVJpHZVA;
        }

        private void abbHTMsHxKlPFWEgmVxWYTZjsUYTfoEN()
        {
            LMLeGZXPwSrvMCZOMFqTnBsdKjZmvckD();
            kUpSPIkKSjSJaVYamQPQNyytrRTDGYLp = true;
        }

        private void jtqkLUcZBepZjdoksIhuZocEZREaWdyy()
        {
            kUpSPIkKSjSJaVYamQPQNyytrRTDGYLp = false;
            LMLeGZXPwSrvMCZOMFqTnBsdKjZmvckD();
            PhysicsUpdate?.Invoke(TimeStep);
            LatePhysicsUpdate?.Invoke(TimeStep);
            DetectorsUpdate?.Invoke();
        }

        private void LMLeGZXPwSrvMCZOMFqTnBsdKjZmvckD()
        {
            while (NpRgNYqXOQnxSLUeACnXuoJOSxlSElhe.Count > 0)
            {
                NpRgNYqXOQnxSLUeACnXuoJOSxlSElhe.Dequeue().Apply(this);
            }
        }

        
        private JobHandle leivJwMZzLTkycKiKiJTBURnDOLgbfak(in float timeStep, JobHandle dependsOn = default)
        {
            
            ResetContainsParticlesWithInfiniteMassFlagJob AnUxAifMrwBZolYEMnQJCVvROrnFsAJo = new ResetContainsParticlesWithInfiniteMassFlagJob
            {
                ContainsParticlesWithInfiniteMass = SimulationMeshDescriptorData.ContainsParticlesWithInfiniteMass,
            };

            
            PreSolveJob AmNfZkrQXYusZNUQPdrDHHnsXTVkxKiX = new PreSolveJob
            {
                TimeStep = timeStep,
                Gravity = Gravity,

                FloorHeight = transform.position.y + FloorHeight,
                ParticleRadius = ParticleRadius,

                MaxVelocitySquared = MaxVelocity * MaxVelocity,

                ParticlesEnabled = ParticleData.Enabled,
                ParticlesSimulationMeshDescriptorIndex = ParticleData.SimulationMeshDescriptorIndex,
                ParticlesPhysicsMaterialIndex = ParticleData.PhysicsMaterialIndex,
                ParticlesOriginalInverseMass = ParticleData.InverseMass,
                ParticlesAdjustedInverseMass = ParticleData.AdjustedInverseMass,
                ParticlesPosition = ParticleData.Position,
                ParticlesPredictedPosition = ParticleData.PredictedPosition,
                ParticlesVelocity = ParticleData.Velocity,
                ContainsParticlesWithInfiniteMass = SimulationMeshDescriptorData.ContainsParticlesWithInfiniteMass,

                SimulationMeshDescriptors = SimulationMeshDescriptorData.SimulationMeshDescriptors,
                PhysicsMaterials = PhysicsMaterials,
            };

            dependsOn = AnUxAifMrwBZolYEMnQJCVvROrnFsAJo.Schedule(SimulationMeshDescriptorData.Count, 64, dependsOn);
            return AmNfZkrQXYusZNUQPdrDHHnsXTVkxKiX.Schedule(ParticleData.Count, 64, dependsOn);
        }

        private JobHandle TfiTfsPeqXeEqKDtRvPZYSZZZFOhWRLR(JobHandle dependsOn)
        {
            var yxoZYcOjuRBkOtKeIOtKyDYIbzYIUYPm = new InitializeCounterJob
            {
                Counter = ContactPairsCount,
            };

            return yxoZYcOjuRBkOtKeIOtKyDYIbzYIUYPm.Schedule(dependsOn);
        }

        private JobHandle hMLkyHxJkISbDqICElooRzPDfRTsbTNN(JobHandle dependsOn)
        {
            return SpatialHashGrid.CollectContactPairs(ParticleRadius, ref ParticleData.PredictedPosition, ref ContactPairs, ref ContactPairsCount, dependsOn);
        }

        private JobHandle sMOWkDwTnKdcIZsbLIMbGsOMhFKvmxdw(JobHandle dependsOn)
        {
            return new CopyBufferJob<float3>
            {
                Source = ParticleData.PredictedPosition,
                Destination = ParticleData.TemporaryPositionBuffer,
            }.Schedule(ParticleData.Count, 64, dependsOn);
        }

        private JobHandle EDymCDOxaIJUKNyFQxjfYSNDgFLRHYTo(in float timeStep, JobHandle dependsOn)
        {
            unsafe
            {
                SolveCollisionConstraintsJob mHuDYFKWZUlYcTEQvRrivhBHkCDLTsVY = new SolveCollisionConstraintsJob
                {
                    ParticleRadius = ParticleRadius,
                    TimeStepSquared = timeStep * timeStep,

                    ParticlesEnabled = ParticleData.Enabled,
                    ParticlesBreakable = ParticleData.Breakable,
                    ParticlesPredictedPosition = ParticleData.PredictedPosition,
                    ParticlesInverseMass = ParticleData.AdjustedInverseMass,
                    ParticlesPhase = ParticleData.Phase,
                    ParticlesPhysicsMaterialIndex = ParticleData.PhysicsMaterialIndex,
                    ParticlesSimulationMeshDescriptorIndex = ParticleData.SimulationMeshDescriptorIndex,

                    ContactPairs = ContactPairs,

                    PhysicsMaterials = PhysicsMaterials,
                    SimulationMeshDescriptors = SimulationMeshDescriptorData.SimulationMeshDescriptors,

                    ParticlesEnabledStateHasChanged = SimulationMeshDescriptorData.ParticlesEnabledStateHasChanged,

                    ParticleCorrections = ParticleData.PositionCorrections,
                    ParticleCorrectionsCount = ParticleData.PositionCorrectionsCount,
                    ParticleLocks = ParticleData.ParticleLocks,
                };

                return mHuDYFKWZUlYcTEQvRrivhBHkCDLTsVY.Schedule(ContactPairsCount.GetUnsafePtr(), 64, dependsOn);
            }
        }

        private JobHandle PStqRhgadbPWOVulLBBGRMPmgXEGluOK(JobHandle dependsOn)
        {
            unsafe
            {
                ApplyContactFrictionJob hTQzhcgWjpsMDimcEXfFIPSRehvJjEQI = new ApplyContactFrictionJob
                {
                    ParticleRadius = ParticleRadius,

                    ParticlesEnabled = ParticleData.Enabled,
                    ParticlesPredictedPositionBeforeCollisions = ParticleData.TemporaryPositionBuffer,
                    ParticlesPredictedPositionAfterCollisions = ParticleData.PredictedPosition,
                    ParticlesPhase = ParticleData.Phase,
                    ParticlesInverseMass = ParticleData.InverseMass,
                    ParticlesPhysicsMaterialIndex = ParticleData.PhysicsMaterialIndex,
                    ParticlesSimulationMeshDescriptorIndex = ParticleData.SimulationMeshDescriptorIndex,

                    ParticleCorrections = ParticleData.PositionCorrections,
                    ParticleCorrectionsCount = ParticleData.PositionCorrectionsCount,
                    ParticleLocks = ParticleData.ParticleLocks,

                    ContactPairs = ContactPairs,

                    PhysicsMaterials = PhysicsMaterials,
                    SimulationMeshDescriptors = SimulationMeshDescriptorData.SimulationMeshDescriptors,
                };

                return hTQzhcgWjpsMDimcEXfFIPSRehvJjEQI.Schedule(ContactPairsCount.GetUnsafePtr(), 64, dependsOn);
            }
        }

        private JobHandle fRkmCUwpjONSXOwizfUKrwSHoxiRAxDC(in float timeStep, JobHandle dependsOn = default)
        {
            SolveDistanceConstraintsJob gYIlpmvQhjBFodMxDgZLbknUznadjysL = new SolveDistanceConstraintsJob
            {
                TimeStepSquared = timeStep * timeStep,

                
                ParticlesEnabled = ParticleData.Enabled,
                ParticlesPredictedPosition = ParticleData.PredictedPosition,
                ParticlesInverseMass = ParticleData.InverseMass,

                
                DistanceConstraintsEnabled = DistanceConstraintData.Enabled,
                DistanceConstraintsBreakable = DistanceConstraintData.Breakable,
                DistanceConstraintsPhysicsMaterialIndex = DistanceConstraintData.PhysicsMaterialIndex,
                DistanceConstraintsParticleA = DistanceConstraintData.ParticleA,
                DistanceConstraintsParticleB = DistanceConstraintData.ParticleB,
                DistanceConstraintsRestDistance = DistanceConstraintData.RestDistance,
                DistanceConstraintsSimulationMeshDescriptorIndex = DistanceConstraintData.SimulationMeshDescriptorIndex,
                DistanceConstraintsEnabledStateHasChanged = SimulationMeshDescriptorData.DistanceConstraintsEnabledStateHasChanged,

                
                SimulationMeshDescriptors = SimulationMeshDescriptorData.SimulationMeshDescriptors,
                PhysicsMaterials = PhysicsMaterials,

                
                ParticleCorrections = ParticleData.PositionCorrections,
                ParticleCorrectionsCount = ParticleData.PositionCorrectionsCount,
                ParticleLocks = ParticleData.ParticleLocks,
            };

            return gYIlpmvQhjBFodMxDgZLbknUznadjysL.Schedule(DistanceConstraintData.Count, 64, dependsOn);
        }

        private JobHandle VYiBmRHQYMgcnNYyCONwVVksrqemqOWQ(in float timeStep, JobHandle dependsOn = default)
        {
            SolveVolumeConstraintsJob StUYBNmBuyXbfmYXJDbnuCYJqxefzsYp = new SolveVolumeConstraintsJob
            {
                TimeStepSquared = timeStep * timeStep,

                
                ParticlesEnabled = ParticleData.Enabled,
                ParticlesPredictedPosition = ParticleData.PredictedPosition,
                ParticlesInverseMass = ParticleData.InverseMass,

                
                VolumeConstraintsEnabled = VolumeConstraintData.Enabled,
                VolumeConstraintsBreakable = VolumeConstraintData.Breakable,
                VolumeConstraintsPhysicsMaterialIndex = VolumeConstraintData.PhysicsMaterialIndex,
                VolumeConstraintsParticleA = VolumeConstraintData.ParticleA,
                VolumeConstraintsParticleB = VolumeConstraintData.ParticleB,
                VolumeConstraintsParticleC = VolumeConstraintData.ParticleC,
                VolumeConstraintsParticleD = VolumeConstraintData.ParticleD,
                VolumeConstraintsRestVolume = VolumeConstraintData.RestVolume,
                VolumeConstraintsSimulationMeshDescriptorIndex = VolumeConstraintData.SimulationMeshDescriptorIndex,

                
                SimulationMeshDescriptors = SimulationMeshDescriptorData.SimulationMeshDescriptors,
                VolumeConstraintsEnabledStateHasChanged = SimulationMeshDescriptorData.VolumeConstraintsEnabledStateHasChanged,
                PhysicsMaterials = PhysicsMaterials,

                
                ParticleCorrections = ParticleData.PositionCorrections,
                ParticleCorrectionsCount = ParticleData.PositionCorrectionsCount,
                ParticleLocks = ParticleData.ParticleLocks,
            };

            return StUYBNmBuyXbfmYXJDbnuCYJqxefzsYp.Schedule(VolumeConstraintData.Count, 64, dependsOn);
        }

        private JobHandle uqPmnziZDFzVyaruxpzeIdsnkBxIJLfI(in float timeStep, JobHandle dependsOn = default)
        {
            SolveCollinearConstraintsJob DptzWjpgUdqXdJViDPubgmbzPvCIAEOY = new SolveCollinearConstraintsJob
            {
                TimeStepSquared = timeStep * timeStep,

                
                ParticlesEnabled = ParticleData.Enabled,
                ParticlesPredictedPosition = ParticleData.PredictedPosition,
                ParticlesInverseMass = ParticleData.InverseMass,

                
                CollinearConstraintsEnabled = CollinearConstraintData.Enabled,
                CollinearConstraintsBreakable = CollinearConstraintData.Breakable,
                CollinearConstraintsPhysicsMaterialIndex = CollinearConstraintData.PhysicsMaterialIndex,
                CollinearConstraintsParticleA = CollinearConstraintData.ParticleA,
                CollinearConstraintsParticleB = CollinearConstraintData.ParticleB,
                CollinearConstraintsParticleC = CollinearConstraintData.ParticleC,
                CollinearConstraintsSimulationMeshDescriptorIndex = CollinearConstraintData.SimulationMeshDescriptorIndex,

                

                
                SimulationMeshDescriptors = SimulationMeshDescriptorData.SimulationMeshDescriptors,
                PhysicsMaterials = PhysicsMaterials,

                
                ParticleCorrections = ParticleData.PositionCorrections,
                ParticleCorrectionsCount = ParticleData.PositionCorrectionsCount,
                ParticleLocks = ParticleData.ParticleLocks,
            };

            return DptzWjpgUdqXdJViDPubgmbzPvCIAEOY.Schedule(CollinearConstraintData.Count, 64, dependsOn);
        }

        private JobHandle eBzoFHoUNLJRPUgYcvPyUWjqZGioQRkK(in float timeStep, JobHandle dependsOn = default)
        {
            unsafe
            {
                var prwhfCOWcujmRtjPEOrXeFGHQQgniArr = new ResetCoMsJob
                {
                    SimulationMeshTotalMass = SimulationMeshDescriptorData.TotalMass,
                    SimulationMeshCoM = SimulationMeshDescriptorData.CenterOfMassPosition,
                    SimulationMeshCovarianceMatrix = SimulationMeshDescriptorData.CovarianceMatrix,
                };

                var CCHwhVKPFCcPxkxtouCRJtDUQvVuwXAd = new CalculateParticlesContributionToCoMsJob
                {
                    ParticlesEnabled = ParticleData.Enabled,
                    ParticlesPredictedPosition = ParticleData.PredictedPosition,
                    ParticlesInverseMass = ParticleData.InverseMass,
                    ParticlesSimulationMeshDescriptorIndex = ParticleData.SimulationMeshDescriptorIndex,

                    SimulationMeshLocks = SimulationMeshDescriptorData.Locks,
                    SimulationMeshContainsParticlesWithInfiniteMass = SimulationMeshDescriptorData.ContainsParticlesWithInfiniteMass,
                    SimulationMeshTotalMass = SimulationMeshDescriptorData.TotalMass,
                    SimulationMeshCoM = SimulationMeshDescriptorData.CenterOfMassPosition,
                };

                var YeldSgrAfFuKGVnqNknJuklmWbeHWTDf = new AverageCoMContributionsJob
                {
                    SimulationMeshTotalMass = SimulationMeshDescriptorData.TotalMass,
                    SimulationMeshCoM = SimulationMeshDescriptorData.CenterOfMassPosition,
                };

                var xmGDxcpRDZRXWqakwFRZAyYJJBlxCKkM = new CalculateCovarianceMatricesJob
                {
                    ParticlesEnabled = ParticleData.Enabled,
                    ParticlesPosition = ParticleData.PredictedPosition,
                    ParticlesLocalRestPosition = ParticleData.LocalRestPosition,
                    ParticlesSimulationMeshDescriptorIndex = ParticleData.SimulationMeshDescriptorIndex,

                    SimulationMeshCoM = SimulationMeshDescriptorData.CenterOfMassPosition,
                    SimulationMeshCovarianceMatrix = SimulationMeshDescriptorData.CovarianceMatrix,
                };

                var LnoWgycvmvgmZlQblsGGEFOnlgaVjhcY = new CalculateCoMRotationsJob
                {
                    SimulationMeshCovarianceMatrix = SimulationMeshDescriptorData.CovarianceMatrix,
                    SimulationMeshCoMRotation = SimulationMeshDescriptorData.CenterOfMassRotation,
                };

                var MfOWqidRyvsoeDwUhFQFvHxrUyNySXEy = new SolveShapeMatchingConstraintsJob
                {
                    ParticlesEnabled = ParticleData.Enabled,
                    ParticlesPredictedPosition = ParticleData.PredictedPosition,
                    ParticlesLocalRestPosition = ParticleData.LocalRestPosition,
                    ParticlesInverseMass = ParticleData.InverseMass,

                    ShapeMatchingConstraintsEnabled = ShapeMatchingConstraintData.Enabled,
                    ShapeMatchingConstraintsBreakable = ShapeMatchingConstraintData.Breakable,
                    ShapeMatchingConstraintsParticle = ShapeMatchingConstraintData.Particle,
                    ShapeMatchingConstraintsPhysicsMaterialIndex = ShapeMatchingConstraintData.PhysicsMaterialIndex,
                    ShapeMatchingConstraintsSimulationMeshDescriptorIndex = ShapeMatchingConstraintData.SimulationMeshDescriptorIndex,
                    ShapeMatchingConstraintsEnabledStateHasChanged = SimulationMeshDescriptorData.ShapeMatchingConstraintsEnabledStateHasChanged,

                    SimulationMeshDescriptors = SimulationMeshDescriptorData.SimulationMeshDescriptors,
                    PhysicsMaterials = PhysicsMaterials,

                    CoMPosition = SimulationMeshDescriptorData.CenterOfMassPosition,
                    CoMRotation = SimulationMeshDescriptorData.CenterOfMassRotation,

                    ParticleCorrections = ParticleData.PositionCorrections,
                    ParticleCorrectionsCount = ParticleData.PositionCorrectionsCount,
                    ParticleLocks = ParticleData.ParticleLocks,
                };

                
                dependsOn = prwhfCOWcujmRtjPEOrXeFGHQQgniArr.Schedule(SimulationMeshDescriptorData.Count, 64, dependsOn);
                dependsOn = CCHwhVKPFCcPxkxtouCRJtDUQvVuwXAd.Schedule(ParticleData.Count, 64, dependsOn);
                dependsOn = YeldSgrAfFuKGVnqNknJuklmWbeHWTDf.Schedule(SimulationMeshDescriptorData.Count, 64, dependsOn);
                dependsOn = xmGDxcpRDZRXWqakwFRZAyYJJBlxCKkM.Schedule(dependsOn);
                dependsOn = LnoWgycvmvgmZlQblsGGEFOnlgaVjhcY.Schedule(SimulationMeshDescriptorData.Count, 64, dependsOn);
                dependsOn = MfOWqidRyvsoeDwUhFQFvHxrUyNySXEy.Schedule(ShapeMatchingConstraintData.Count, 64, dependsOn);

                return dependsOn;
            }
        }

        private JobHandle SOuLdjzZcgGoOabDZmBcQeWQMwORjmRH(JobHandle dependsOn = default)
        {
            InitializeCorrectionsBufferJob nqCaByhEONSCsPHvZnPQOJNVYwZvlBJt = new InitializeCorrectionsBufferJob
            {
                ParticlesCorrections = ParticleData.PositionCorrections,
                ParticlesCorrectionsCount = ParticleData.PositionCorrectionsCount,
            };

            return nqCaByhEONSCsPHvZnPQOJNVYwZvlBJt.Schedule(ParticleData.PositionCorrections.Length, 64, dependsOn);
        }

        private JobHandle oGnirSUMygvXCLrFgHHHVowrEjNUstbv(float relaxationFactor, NativeArray<float3> targetArray, JobHandle dependsOn = default)
        {
            ApplyCorrectionsJob WtgEgjWOgIPidZbhIlQkiSRpQINdOsnM = new ApplyCorrectionsJob
            {
                RelaxationFactor = relaxationFactor,

                ParticlesCorrections = ParticleData.PositionCorrections,
                ParticlesCorrectionsCount = ParticleData.PositionCorrectionsCount,

                ApplyTarget = targetArray,
            };

            return WtgEgjWOgIPidZbhIlQkiSRpQINdOsnM.Schedule(ParticleData.Count, 64, dependsOn);
        }

        private JobHandle RyZaIDslSqAqTXMIHUAexGxHuVxNrPbC(in float iterationTimeStep, JobHandle dependsOn = default)
        {
            for (int HuuKnhwXxuwLOFwWvuFxDDnaqTzHzARR = 0; HuuKnhwXxuwLOFwWvuFxDDnaqTzHzARR < vZHHMDAlQnZtdxCQQDCgQiLYmOlZXfOU.Count; HuuKnhwXxuwLOFwWvuFxDDnaqTzHzARR++)
            {
                IConstraintSolver ZRccohLHZTtvmJgCAjXqQrRchTKtTMsL = vZHHMDAlQnZtdxCQQDCgQiLYmOlZXfOU[HuuKnhwXxuwLOFwWvuFxDDnaqTzHzARR];
                dependsOn = ZRccohLHZTtvmJgCAjXqQrRchTKtTMsL.DispatchSolveJob(iterationTimeStep, dependsOn);
            }

            return dependsOn;
        }

        private JobHandle gLRNYcaFJrwxfdTDGWGOKMnbJkNmUWho(in float timeStep, JobHandle dependsOn = default)
        {
            PostSolveJob AgQLwBcblZDTaPdXpYFFnLfQyTmtIDLa = new PostSolveJob
            {
                TimeStep = timeStep,

                SleepVelocityThresholdSquared = SleepVelocityThreshold * SleepVelocityThreshold,

                ParticlesPredictedPosition = ParticleData.PredictedPosition,
                ParticlesPosition = ParticleData.Position,
                ParticlesVelocity = ParticleData.Velocity,
            };

            return AgQLwBcblZDTaPdXpYFFnLfQyTmtIDLa.Schedule(ParticleData.Count, 64, dependsOn);
        }

        private struct JUZnLKejmTkSdgYeXeeIjqZqdhSxtoBD
        {
            public bool Scheduled;
            public double PreviousTimeStepTime;
            public double RunningTime;
            public JobHandle Handle;
        }
    }
}
