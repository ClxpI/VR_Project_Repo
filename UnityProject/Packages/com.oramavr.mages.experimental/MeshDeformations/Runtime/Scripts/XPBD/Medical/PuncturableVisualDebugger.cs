namespace MAGES.MeshDeformations
{
    using Unity.Mathematics;
    using UnityEngine;

    /// <summary>
    /// Visualizes a <see cref="MeshDeformations.Puncturable"/> object.
    /// </summary>
    [RequireComponent(typeof(Puncturable))]
    public class PuncturableVisualDebugger : MonoBehaviour
    {
        private Puncturable ZXvcgayriJcDbQluZzdyeoGlXrXUPzqM;
        private PhysicsWorld LWfmeLLZFnrMxAtnYAgZsNMCMGhbiths;

        private int ApEKyoJVomsGNiVVoitgCuOflhGFVFjj = 0;
        private Puncturable.InsertionConstraint[] CTFqlxmiRYgZWQtTVnSYbthfrPXlPKJF = new Puncturable.InsertionConstraint[0];
        private float3[] shENXOFHNKasXMBqRSjGYmubjilMZIVD = new float3[0];

        private Puncturable Puncturable
        {
            get
            {
                if (ZXvcgayriJcDbQluZzdyeoGlXrXUPzqM == null)
                {
                    ZXvcgayriJcDbQluZzdyeoGlXrXUPzqM = GetComponent<Puncturable>();
                }

                return ZXvcgayriJcDbQluZzdyeoGlXrXUPzqM;
            }
        }

        private PhysicsWorld PhysicsWorld
        {
            get
            {
                if (LWfmeLLZFnrMxAtnYAgZsNMCMGhbiths == null)
                {
                    if (Puncturable != null)
                    {
                        LWfmeLLZFnrMxAtnYAgZsNMCMGhbiths = Puncturable.PhysicsWorld;

                        if (LWfmeLLZFnrMxAtnYAgZsNMCMGhbiths != null)
                        {
                            LWfmeLLZFnrMxAtnYAgZsNMCMGhbiths.PhysicsUpdate.AddListener(CeeAOydmfsuFdEaTUKHvoLlDRLWAUlnb);
                        }
                    }
                }

                return LWfmeLLZFnrMxAtnYAgZsNMCMGhbiths;
            }
        }

        private void IXoKzfWdDXVgKzvEFuTyuyqAxLrVkLsT()
        {
            if (LWfmeLLZFnrMxAtnYAgZsNMCMGhbiths == null)
            {
                PhysicsWorld vRpnZcQlTGLRsDMwrfDZkgLWZUBCocLT = PhysicsWorld;
            }
        }

        private void CeeAOydmfsuFdEaTUKHvoLlDRLWAUlnb(float timeStep)
        {
            if (!Puncturable.IsInitialized)
            {
                return;
            }

            
            if (CTFqlxmiRYgZWQtTVnSYbthfrPXlPKJF.Length < Puncturable.InsertionConstraints.Length)
            {
                CTFqlxmiRYgZWQtTVnSYbthfrPXlPKJF = new Puncturable.InsertionConstraint[Puncturable.InsertionConstraints.Length];
            }

            unsafe
            {
                ApEKyoJVomsGNiVVoitgCuOflhGFVFjj = *Puncturable.InsertionConstraintsCount.Counter;
            }

            for (int RSpocJEadzKxWuflhaqnUgXIStzVGWlX = 0; RSpocJEadzKxWuflhaqnUgXIStzVGWlX < ApEKyoJVomsGNiVVoitgCuOflhGFVFjj; RSpocJEadzKxWuflhaqnUgXIStzVGWlX++)
            {
                CTFqlxmiRYgZWQtTVnSYbthfrPXlPKJF[RSpocJEadzKxWuflhaqnUgXIStzVGWlX] = Puncturable.InsertionConstraints[RSpocJEadzKxWuflhaqnUgXIStzVGWlX];
            }

            
            if (shENXOFHNKasXMBqRSjGYmubjilMZIVD.Length < PhysicsWorld.ParticleData.Count)
            {
                shENXOFHNKasXMBqRSjGYmubjilMZIVD = new float3[PhysicsWorld.ParticleData.Count];
            }

            PhysicsWorld.ParticleData.Position.CopyTo(shENXOFHNKasXMBqRSjGYmubjilMZIVD);
        }

        private void ubDbkfVWPbdVFXRoeXOEtvmffZnCNNvh()
        {
            for (int UAtVrBVWLdhJMwIhlMRGRydtWoCgmYZY = 0; UAtVrBVWLdhJMwIhlMRGRydtWoCgmYZY < ApEKyoJVomsGNiVVoitgCuOflhGFVFjj; UAtVrBVWLdhJMwIhlMRGRydtWoCgmYZY++)
            {
                Puncturable.InsertionConstraint haMipGzqYBZANQeDiEIftcRZYHDEEdyf = CTFqlxmiRYgZWQtTVnSYbthfrPXlPKJF[UAtVrBVWLdhJMwIhlMRGRydtWoCgmYZY];

                float3 gCVUAXsqiNMlOAEtedHTEIyCKYeRbEME = shENXOFHNKasXMBqRSjGYmubjilMZIVD[haMipGzqYBZANQeDiEIftcRZYHDEEdyf.NeedleSegmentParticleA];
                float3 gnuEINnjazdIXoVJbUKQqCnfnQMGdkqF = shENXOFHNKasXMBqRSjGYmubjilMZIVD[haMipGzqYBZANQeDiEIftcRZYHDEEdyf.NeedleSegmentParticleB];

                float3 cbVOmAzfQaephQVkXVTqGnwDEdpXAKIj = shENXOFHNKasXMBqRSjGYmubjilMZIVD[haMipGzqYBZANQeDiEIftcRZYHDEEdyf.PuncturableParticleA];
                float3 ZxeqqbzfdrJFBpNETDQfDHXTIcwoCMlb = shENXOFHNKasXMBqRSjGYmubjilMZIVD[haMipGzqYBZANQeDiEIftcRZYHDEEdyf.PuncturableParticleB];
                float3 htYAxgbVWNAdKMVQgKUctvENJXPvNzRv = shENXOFHNKasXMBqRSjGYmubjilMZIVD[haMipGzqYBZANQeDiEIftcRZYHDEEdyf.PuncturableParticleC];

                Gizmos.color = Color.red;
                Gizmos.DrawLine(gCVUAXsqiNMlOAEtedHTEIyCKYeRbEME, gnuEINnjazdIXoVJbUKQqCnfnQMGdkqF);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(cbVOmAzfQaephQVkXVTqGnwDEdpXAKIj, ZxeqqbzfdrJFBpNETDQfDHXTIcwoCMlb);
                Gizmos.DrawLine(ZxeqqbzfdrJFBpNETDQfDHXTIcwoCMlb, htYAxgbVWNAdKMVQgKUctvENJXPvNzRv);
                Gizmos.DrawLine(htYAxgbVWNAdKMVQgKUctvENJXPvNzRv, cbVOmAzfQaephQVkXVTqGnwDEdpXAKIj);

                Gizmos.color = Color.blue;
                MAGES.Utilities.GizmoUtilities.DrawPoint(Utilities.Math.TriangleBarycentricInterpolation(cbVOmAzfQaephQVkXVTqGnwDEdpXAKIj, ZxeqqbzfdrJFBpNETDQfDHXTIcwoCMlb, htYAxgbVWNAdKMVQgKUctvENJXPvNzRv, haMipGzqYBZANQeDiEIftcRZYHDEEdyf.BarycentricCoordinates), 0.01F);
            }
        }
    }
}
