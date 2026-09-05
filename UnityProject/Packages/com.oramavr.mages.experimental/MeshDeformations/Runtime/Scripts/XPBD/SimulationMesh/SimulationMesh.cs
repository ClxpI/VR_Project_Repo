namespace MAGES.MeshDeformations
{
    using System.Runtime.CompilerServices;
    using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// A simulation mesh, used with the XPBD solver.
    /// </summary>
    /// <seealso cref="PhysicsWorld"/>
    [CreateAssetMenu(menuName = "MAGES/Simulation Mesh")]
    public class SimulationMesh : ScriptableObject
    {
        
#pragma warning disable SA1401 // Fields should be private
        /// <summary>
        /// Runtime Particle Data Slice.
        /// </summary>
        public RuntimeParticleDataSlice RuntimeParticleData;

        /// <summary>
        /// Runtime Distance Constraint Data Slice.
        /// </summary>
        public RuntimeDistanceConstraintDataSlice RuntimeDistanceConstraintData;

        /// <summary>
        /// Runtime Volume Constraint Data Slice.
        /// </summary>
        public RuntimeVolumeConstraintDataSlice RuntimeVolumeConstraintData;

        /// <summary>
        /// Runtime Shape Matching Constraint Data Slice.
        /// </summary>
        public RuntimeShapeMatchingConstraintDataSlice RuntimeShapeMatchingConstraintData;

        /// <summary>
        /// Runtime Collinear Constraint Data Slice.
        /// </summary>
        public RuntimeCollinearConstraintDataSlice RuntimeCollinearConstraintData;

        /// <summary>
        /// Runtime Simulation Mesh Descriptor Index given by the physics world.
        /// </summary>
#pragma warning restore SA1401 // Fields should be private

        private PhysicsWorld zsZKYlNSCaNOkMufBTORwDGbXITHYFpa;      
        private uint cCbdpNpzjjZEpVPQGdtPgTbecYGpbiVY = 0;                 

        [SerializeField]
        private Particle[] particles = new Particle[0];

        [SerializeField]
        private DistanceConstraint[] distanceConstraints = new DistanceConstraint[0];

        [SerializeField]
        private VolumeConstraint[] volumeConstraints = new VolumeConstraint[0];

        [SerializeField]
        private ShapeMatchingConstraint[] shapeMatchingConstraints = new ShapeMatchingConstraint[0];

        [SerializeField]
        private CollinearConstraint[] collinearConstraints = new CollinearConstraint[0];

        [SerializeField]
        private PhysicsMaterial[] physicsMaterials = new PhysicsMaterial[1]
        {
            new PhysicsMaterial
            {
                ParticleDamping = 0f,
                DistanceConstraintInverseStiffness = 0f,
                VolumeConstraintInverseStiffness = 0f,
            },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulationMesh"/> class.
        /// </summary>
        public SimulationMesh()
        {
            zsZKYlNSCaNOkMufBTORwDGbXITHYFpa = null;
            particles = new Particle[0];
            distanceConstraints = new DistanceConstraint[0];
            volumeConstraints = new VolumeConstraint[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulationMesh"/> class.
        /// </summary>
        /// <param name="particles">The particles of the simulation mesh.</param>
        /// <param name="distanceConstraints">The distance constraints of the simulation mesh.</param>
        /// <param name="volumeConstraints">The volume constraints of the simulation mesh.</param>
        /// <param name="shapeMatchingConstraints">The shape matching constraints of the simulation mesh.</param>
        /// <param name="collinearConstraints">The collinear constraints of the simulation mesh.</param>
        public SimulationMesh(Particle[] particles, DistanceConstraint[] distanceConstraints, VolumeConstraint[] volumeConstraints, ShapeMatchingConstraint[] shapeMatchingConstraints, CollinearConstraint[] collinearConstraints)
        {
            zsZKYlNSCaNOkMufBTORwDGbXITHYFpa = null;
            this.particles = particles;
            this.distanceConstraints = distanceConstraints;
            this.volumeConstraints = volumeConstraints;
            this.shapeMatchingConstraints = shapeMatchingConstraints;
            this.collinearConstraints = collinearConstraints;
        }

        /// <summary>
        /// Gets the event that is invoked when the simulation mesh is added to a physics world.
        /// </summary>
        public UnityEvent AddedToPhysicsWorld { get; private set; } = new UnityEvent();

        /// <summary>
        /// Gets the event that is invoked when the simulation mesh is removed from a physics world.
        /// </summary>
        public UnityEvent RemovedFromPhysicsWorld { get; private set; } = new UnityEvent();

        /// <summary>
        /// Gets a value indicating whether this simulation mesh is part of a physics world.
        /// </summary>
        public bool IsPartOfPhysicsWorld => zsZKYlNSCaNOkMufBTORwDGbXITHYFpa != null;

        /// <summary>
        /// Gets the simulation mesh descriptor index.
        /// </summary>
        public int SimulationMeshDescriptorIndex
        {
            get
            {
                if (!IsPartOfPhysicsWorld)
                {
                    return -1;
                }

                return zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.SimulationMeshToDescriptorMap[this];
            }
        }

        /// <summary>
        /// Gets the physics world that the simulation mesh is part of or <see langword="null"/> if not part of any physics world.
        /// </summary>
        public PhysicsWorld PhysicsWorld
        {
            get
            {
                return zsZKYlNSCaNOkMufBTORwDGbXITHYFpa;
            }
        }

        /// <summary>
        /// Gets or sets the particles of this simulation mesh.
        /// </summary>
        /// <remarks>Getting the particles involves memory allocation and is typically a slow operation.</remarks>
        public Particle[] Particles
        {
            get
            {
                if (IsPartOfPhysicsWorld)
                {
                    return particles;
                }
                else
                {
                    return particles;
                }
            }

            set
            {
                if (IsPartOfPhysicsWorld)
                {
                    Debug.LogWarning("Updating the particles while SimulationMesh is part of physics world is not supported.");
                    return;
                }

                particles = value;
            }
        }

        /// <summary>
        /// Gets or sets the volume constraints of this simulation mesh.
        /// </summary>
        /// <remarks>Getting the constraints involves memory allocation and is typically a slow operation.</remarks>
        public VolumeConstraint[] VolumeConstraints
        {
            get
            {
                if (IsPartOfPhysicsWorld)
                {
                    return volumeConstraints;
                }
                else
                {
                    return volumeConstraints;
                }
            }

            set
            {
                if (IsPartOfPhysicsWorld)
                {
                    Debug.LogWarning("Updating the volume constraints while SimulationMesh is part of physics world is not supported.");
                    return;
                }

                if (!niHMjLPvnsSHojNlpdhsXEhLRzJrWtJp(value))
                {
                    Debug.LogError("Some volume constraints reference particles that are out of particle array bounds.");
                    return;
                }

                volumeConstraints = value;

                
            }
        }

        /// <summary>
        /// Gets or sets the distance constraints of the simulation mesh.
        /// </summary>
        /// <remarks>Getting the constraints involves memory allocation and is typically a slow operation.</remarks>
        public DistanceConstraint[] DistanceConstraints
        {
            get
            {
                if (IsPartOfPhysicsWorld)
                {
                    var qmQOIHrDhwjCMJRDlgXGJzZHMDWLPkYA = new DistanceConstraint[RuntimeDistanceConstraintData.Count];

                    
                    return distanceConstraints;
                }
                else
                {
                    return distanceConstraints;
                }
            }

            set
            {
                if (IsPartOfPhysicsWorld)
                {
                    Debug.LogWarning("Updating the distance constraints while SimulationMesh is part of physics world is not supported.");
                    return;
                }

                if (!CZTYOOqvSCimcccSnkNVaViDqJljCEho(value))
                {
                    Debug.LogError("Some distance constraints reference particles that are out of particle array bounds.");
                    return;
                }

                distanceConstraints = value;

                
            }
        }

        /// <summary>
        /// Gets or sets the shape matching constraints of the simulation mesh.
        /// </summary>
        public ShapeMatchingConstraint[] ShapeMatchingConstraints
        {
            get
            {
                if (IsPartOfPhysicsWorld)
                {
                    
                    return shapeMatchingConstraints;
                }
                else
                {
                    return shapeMatchingConstraints;
                }
            }

            set
            {
                if (IsPartOfPhysicsWorld)
                {
                    Debug.LogWarning("Updating the shape matching constraints while SimulationMesh is part of physics world is not supported.");
                    return;
                }

                if (!wZSACyWNXpNmOMIbrRPKdcQDQmCmZnrl(value))
                {
                    Debug.LogError("Some shape matching constraints reference particles that are out of particle array bounds.");
                    return;
                }

                shapeMatchingConstraints = value;

                
            }
        }

        /// <summary>
        /// Gets or sets the collinear constraints of the simulation mesh.
        /// </summary>
        public CollinearConstraint[] CollinearConstraints
        {
            get
            {
                if (IsPartOfPhysicsWorld)
                {
                    
                    return collinearConstraints;
                }
                else
                {
                    return collinearConstraints;
                }
            }

            set
            {
                if (IsPartOfPhysicsWorld)
                {
                    Debug.LogWarning("Updating the collinear constraints while SimulationMesh is part of physics world is not supported.");
                    return;
                }

                if (!MvyoBfwUwDzlUiJzFHilzuniWnAUUglr(value))
                {
                    Debug.LogError("Some collinear constraints reference particles that are out of particle array bounds.");
                    return;
                }

                collinearConstraints = value;
            }
        }

        /// <summary>
        /// Gets or sets the physics materials.
        /// </summary>
        public PhysicsMaterial[] PhysicsMaterials
        {
            get
            {
                return physicsMaterials;
            }

            set
            {
                physicsMaterials = value;
            }
        }

        /// <summary>
        /// Gets the phase assigned to the particles by the physics world, -1 if not part of any physics world.
        /// </summary>
        public uint Phase
        {
            get => cCbdpNpzjjZEpVPQGdtPgTbecYGpbiVY;
        }

        /// <summary>
        /// Gets or sets a value indicating whether there was a change in some of the <see cref="Particle.Enabled"/> state in the last frame.
        /// </summary>
        public bool ParticlesEnabledStateHasChanged
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return IsPartOfPhysicsWorld && zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.SimulationMeshDescriptorData.ParticlesEnabledStateHasChanged[SimulationMeshDescriptorIndex];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (IsPartOfPhysicsWorld)
                {
                    zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.SimulationMeshDescriptorData.ParticlesEnabledStateHasChanged[SimulationMeshDescriptorIndex] = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether that there was a change in some of the <see cref="DistanceConstraint.Enabled"/> state in the last frame.
        /// </summary>
        public bool DistanceConstraintsEnabledStateHasChanged
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return IsPartOfPhysicsWorld && zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.SimulationMeshDescriptorData.DistanceConstraintsEnabledStateHasChanged[SimulationMeshDescriptorIndex];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (IsPartOfPhysicsWorld)
                {
                    zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.SimulationMeshDescriptorData.DistanceConstraintsEnabledStateHasChanged[SimulationMeshDescriptorIndex] = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether that there was a change in some of the <see cref="VolumeConstraint.Enabled"/> state in the last frame.
        /// </summary>
        public bool VolumeConstraintsEnabledStateHasChanged
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return IsPartOfPhysicsWorld && zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.SimulationMeshDescriptorData.VolumeConstraintsEnabledStateHasChanged[SimulationMeshDescriptorIndex];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (IsPartOfPhysicsWorld)
                {
                    zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.SimulationMeshDescriptorData.VolumeConstraintsEnabledStateHasChanged[SimulationMeshDescriptorIndex] = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether that there was a change in some of the <see cref="ShapeMatchingConstraint.Enabled"/> state in the last frame.
        /// </summary>
        public bool ShapeMatchingConstraintsEnabledStateHasChanged
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return IsPartOfPhysicsWorld && zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.SimulationMeshDescriptorData.ShapeMatchingConstraintsEnabledStateHasChanged[SimulationMeshDescriptorIndex];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (IsPartOfPhysicsWorld)
                {
                    zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.SimulationMeshDescriptorData.ShapeMatchingConstraintsEnabledStateHasChanged[SimulationMeshDescriptorIndex] = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether that there was a change in some of the <see cref="ShapeMatchingConstraint.Enabled"/> state in the last frame.
        /// </summary>
        public bool CollinearConstraintsEnabledStateHasChanged
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return IsPartOfPhysicsWorld && zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.SimulationMeshDescriptorData.CollinearConstraintsEnabledStateHasChanged[SimulationMeshDescriptorIndex];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (IsPartOfPhysicsWorld)
                {
                    zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.SimulationMeshDescriptorData.CollinearConstraintsEnabledStateHasChanged[SimulationMeshDescriptorIndex] = value;
                }
            }
        }

        /// <summary>
        /// Clears the simulation mesh of all data.
        /// </summary>
        public void Clear()
        {
            zsZKYlNSCaNOkMufBTORwDGbXITHYFpa = null;

            AddedToPhysicsWorld.RemoveAllListeners();
            RemovedFromPhysicsWorld.RemoveAllListeners();

            particles = new Particle[0];
            distanceConstraints = new DistanceConstraint[0];
            volumeConstraints = new VolumeConstraint[0];
            shapeMatchingConstraints = new ShapeMatchingConstraint[0];

            physicsMaterials = new PhysicsMaterial[0];
        }

        /// <summary>
        /// Adds particles to the simulation mesh. Only available at runtime and when the simulation mesh is added to a physics world.
        /// </summary>
        /// <param name="particlesEnabled">The enabled state of the particles.</param>
        /// <param name="particlesBreakable">The breakable state of the particles.</param>
        /// <param name="particlesPhysicsMaterialIndex">The physics material index of the particles.</param>
        /// <param name="particlesInitialPosition">The initial position of the particles.</param>
        /// <param name="particlesPosition">The position of the particles.</param>
        /// <param name="particlesVelocity">The velocity of the particles.</param>
        /// <param name="particlesInverseMass">The inverse mass of the particles.</param>
        /// <param name="particlesPhase">The phase of the particles.</param>
        /// <returns>The index at which the new added particles start in the particles buffer.</returns>
        internal int AddRuntimeParticles(
            in bool[] particlesEnabled,
            in bool[] particlesBreakable,
            in int[] particlesPhysicsMaterialIndex,
            in float3[] particlesInitialPosition,
            in float3[] particlesPosition,
            in float3[] particlesVelocity,
            in float[] particlesInverseMass,
            in uint[] particlesPhase)
        {
            UnityEngine.Debug.Assert(IsPartOfPhysicsWorld);

            return zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.AddRuntimeParticles(this, particlesEnabled, particlesBreakable, particlesPhysicsMaterialIndex, particlesInitialPosition, particlesPosition, particlesVelocity, particlesInverseMass, particlesPhase);
        }

        /// <summary>
        /// Adds distance constraints to the simulation mesh. Only available at runtime and when the simulation mesh is added to a physics world.
        /// </summary>
        /// <param name="distanceConstraintsEnabled">The enabled state of the distance constraints.</param>
        /// <param name="distanceConstraintsBreakable">The breakable state of the distance constraints.</param>
        /// <param name="distanceConstraintsPhysicsMaterialIndex">The physics material index of the distance constraints.</param>
        /// <param name="distanceConstraintsParticleA">The first particle index of the distance constraints.</param>
        /// <param name="distanceConstraintsParticleB">The second particle index of the distance constraints.</param>
        /// <param name="distanceConstraintsRestDistance">The rest distance of the distance constraints.</param>
        /// <returns>The index at which the new added distance constraints start in the distance constraints buffer.</returns>
        internal int AddRuntimeDistanceConstraints(
            in bool[] distanceConstraintsEnabled,
            in bool[] distanceConstraintsBreakable,
            in int[] distanceConstraintsPhysicsMaterialIndex,
            in int[] distanceConstraintsParticleA,
            in int[] distanceConstraintsParticleB,
            in float[] distanceConstraintsRestDistance)
        {
            UnityEngine.Debug.Assert(IsPartOfPhysicsWorld);

            return zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.AddRuntimeDistanceConstraints(this, distanceConstraintsEnabled, distanceConstraintsBreakable, distanceConstraintsPhysicsMaterialIndex, distanceConstraintsParticleA, distanceConstraintsParticleB, distanceConstraintsRestDistance);
        }

        /// <summary>
        /// Adds volume constraints to the simulation mesh. Only available at runtime and when the simulation mesh is added to a physics world.
        /// </summary>
        /// <param name="volumeConstraintsEnabled">The enabled state of the volume constraints.</param>
        /// <param name="volumeConstraintsBreakable">The breakable state of the volume constraints.</param>
        /// <param name="volumeConstraintsPhysicsMaterialIndex">The physics material index of the volume constraints.</param>
        /// <param name="volumeConstraintsParticleA">The first particle index of the volume constraints.</param>
        /// <param name="volumeConstraintsParticleB">The second particle index of the volume constraints.</param>
        /// <param name="volumeConstraintsParticleC">The third particle index of the volume constraints.</param>
        /// <param name="volumeConstraintsParticleD">The fourth particle index of the volume constraints.</param>
        /// <param name="volumeConstraintsRestVolume">The rest volume of the volume constraints.</param>
        /// <returns>The index at which the new added volume constraints start in the volume constraints buffer.</returns>
        internal int AddRuntimeVolumeConstraints(
            in bool[] volumeConstraintsEnabled,
            in bool[] volumeConstraintsBreakable,
            in int[] volumeConstraintsPhysicsMaterialIndex,
            in int[] volumeConstraintsParticleA,
            in int[] volumeConstraintsParticleB,
            in int[] volumeConstraintsParticleC,
            in int[] volumeConstraintsParticleD,
            in float[] volumeConstraintsRestVolume)
        {
            UnityEngine.Debug.Assert(IsPartOfPhysicsWorld);

            return zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.AddRuntimeVolumeConstraints(this, volumeConstraintsEnabled, volumeConstraintsBreakable, volumeConstraintsPhysicsMaterialIndex, volumeConstraintsParticleA, volumeConstraintsParticleB, volumeConstraintsParticleC, volumeConstraintsParticleD, volumeConstraintsRestVolume);
        }

        /// <summary>
        /// Gets the center of mass position of the simulation mesh. Only updated for simulation meshes that are part of a physics world and contain at least one shape matching constraint.
        /// </summary>
        /// <returns>The center of mass position.</returns>
        internal float3 GetCenterOfMassPosition()
        {
            if (!IsPartOfPhysicsWorld)
            {
                return float3.zero;
            }

            return zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.SimulationMeshDescriptorData.CenterOfMassPosition[SimulationMeshDescriptorIndex];
        }

        /// <summary>
        /// Gets the center of mass rotation of the simulation mesh. Only updated for simulation meshes that are part of a physics world and contain at least one shape matching constraint.
        /// </summary>
        /// <returns>The center of mass rotation.</returns>
        internal quaternion GetCenterOfMassRotation()
        {
            if (!IsPartOfPhysicsWorld)
            {
                return quaternion.identity;
            }

            return zsZKYlNSCaNOkMufBTORwDGbXITHYFpa.SimulationMeshDescriptorData.CenterOfMassRotation[SimulationMeshDescriptorIndex];
        }

        /// <summary>
        /// Called by the physics world when the simulation mesh is added to it.
        /// </summary>
        /// <param name="physicsWorld">The physics world in which this simulation mesh was added.</param>
        /// <param name="simulationMeshDescriptorIndex">The simulation mesh descriptor index given by the physics world.</param>
        /// <param name="phase">The particle phase give to the particles by the physics world.</param>
        internal void OnAddedToPhysicsWorld(PhysicsWorld physicsWorld, int simulationMeshDescriptorIndex, uint phase)
        {
            this.zsZKYlNSCaNOkMufBTORwDGbXITHYFpa = physicsWorld;
            this.cCbdpNpzjjZEpVPQGdtPgTbecYGpbiVY = phase;
            AddedToPhysicsWorld?.Invoke();
        }

        /// <summary>
        /// Called by the physics world when the simulation mehs is removed from it.
        /// </summary>
        /// <param name="physicsWorld">The physics world from which it was rmoved.</param>
        internal void OnRemovedFromPhysicsWorld(PhysicsWorld physicsWorld)
        {
            this.zsZKYlNSCaNOkMufBTORwDGbXITHYFpa = null;
            cCbdpNpzjjZEpVPQGdtPgTbecYGpbiVY = 0;
            RemovedFromPhysicsWorld?.Invoke();
        }

        private bool niHMjLPvnsSHojNlpdhsXEhLRzJrWtJp(in VolumeConstraint[] volumeConstraints)
        {
            for (int zLcgLjtyAayQcoyeRAkpqNwxHyqalEnv = 0; zLcgLjtyAayQcoyeRAkpqNwxHyqalEnv < volumeConstraints.Length; zLcgLjtyAayQcoyeRAkpqNwxHyqalEnv++)
            {
                VolumeConstraint hvmYqJVxUrmLBzTufAjOaBcLmdCxRHeg = volumeConstraints[zLcgLjtyAayQcoyeRAkpqNwxHyqalEnv];
                if (!iClrLoBsjQHMPNWEIZDOLdEMXTauZpzI(hvmYqJVxUrmLBzTufAjOaBcLmdCxRHeg.ParticleA) || !iClrLoBsjQHMPNWEIZDOLdEMXTauZpzI(hvmYqJVxUrmLBzTufAjOaBcLmdCxRHeg.ParticleB) || !iClrLoBsjQHMPNWEIZDOLdEMXTauZpzI(hvmYqJVxUrmLBzTufAjOaBcLmdCxRHeg.ParticleC) || !iClrLoBsjQHMPNWEIZDOLdEMXTauZpzI(hvmYqJVxUrmLBzTufAjOaBcLmdCxRHeg.ParticleD))
                {
                    return false;
                }
            }

            return true;
        }

        private bool CZTYOOqvSCimcccSnkNVaViDqJljCEho(in DistanceConstraint[] distanceConstraints)
        {
            for (int VAZFpjsZuwltzJPsRspfuqJKJuXUuhbM = 0; VAZFpjsZuwltzJPsRspfuqJKJuXUuhbM < distanceConstraints.Length; VAZFpjsZuwltzJPsRspfuqJKJuXUuhbM++)
            {
                DistanceConstraint AkFzqpUVwDnpJTLEcazjkOAhKuRNdsTW = distanceConstraints[VAZFpjsZuwltzJPsRspfuqJKJuXUuhbM];
                if (!iClrLoBsjQHMPNWEIZDOLdEMXTauZpzI(AkFzqpUVwDnpJTLEcazjkOAhKuRNdsTW.ParticleA) || !iClrLoBsjQHMPNWEIZDOLdEMXTauZpzI(AkFzqpUVwDnpJTLEcazjkOAhKuRNdsTW.ParticleB))
                {
                    return false;
                }
            }

            return true;
        }

        private bool wZSACyWNXpNmOMIbrRPKdcQDQmCmZnrl(ShapeMatchingConstraint[] shapeMatchingConstraints)
        {
            for (int fKGIGQaLNqfBtdmoUCpwnbUwePCisykK = 0; fKGIGQaLNqfBtdmoUCpwnbUwePCisykK < shapeMatchingConstraints.Length; fKGIGQaLNqfBtdmoUCpwnbUwePCisykK++)
            {
                ShapeMatchingConstraint uQSKpphttmBuwDaeeThrOHrgXGIyQgOV = shapeMatchingConstraints[fKGIGQaLNqfBtdmoUCpwnbUwePCisykK];
                if (!iClrLoBsjQHMPNWEIZDOLdEMXTauZpzI(uQSKpphttmBuwDaeeThrOHrgXGIyQgOV.Particle))
                {
                    return false;
                }
            }

            return true;
        }

        private bool MvyoBfwUwDzlUiJzFHilzuniWnAUUglr(CollinearConstraint[] collinearConstraints)
        {
            for (int CyoWPDEGchHPIKGsnuEhbArmgvlFApWL = 0; CyoWPDEGchHPIKGsnuEhbArmgvlFApWL < shapeMatchingConstraints.Length; CyoWPDEGchHPIKGsnuEhbArmgvlFApWL++)
            {
                CollinearConstraint JdUEdZJEiEfZbnQQQeHHPRguuAFjlPDt = collinearConstraints[CyoWPDEGchHPIKGsnuEhbArmgvlFApWL];
                if (!iClrLoBsjQHMPNWEIZDOLdEMXTauZpzI(JdUEdZJEiEfZbnQQQeHHPRguuAFjlPDt.ParticleA) || !iClrLoBsjQHMPNWEIZDOLdEMXTauZpzI(JdUEdZJEiEfZbnQQQeHHPRguuAFjlPDt.ParticleB) || !iClrLoBsjQHMPNWEIZDOLdEMXTauZpzI(JdUEdZJEiEfZbnQQQeHHPRguuAFjlPDt.ParticleC))
                {
                    return false;
                }
            }

            return true;
        }

        private bool iClrLoBsjQHMPNWEIZDOLdEMXTauZpzI(int particleIndex)
        {
            return particleIndex >= 0 && particleIndex < particles.Length;
        }
    }
}
