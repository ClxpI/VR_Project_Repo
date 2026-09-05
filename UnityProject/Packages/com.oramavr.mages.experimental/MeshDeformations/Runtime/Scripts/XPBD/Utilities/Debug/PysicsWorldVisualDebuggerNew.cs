#if UNITY_EDITOR
namespace MAGES.MeshDeformations.Utilities
{
    using System.Threading.Tasks;
    using Unity.Collections;
    using UnityEngine;

    /// <summary>
    /// A visual debugger for a <see cref="PhysicsWorld"/>.
    /// </summary>
    [RequireComponent(typeof(PhysicsWorld))]
    public class PysicsWorldVisualDebuggerNew : MonoBehaviour
    {
        private PhysicsWorld DukDyjwocJJHcySULhWnPWjFGWrnPxAB;

        private Material YpdsfXEtSIBAIlaFJRzpNCDZkOKaKYyh;

        private int GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr = 0;
        private ComputeBuffer QAiKQoFcpHmmkbMdXIlPGXJDeoxzArRz;
        private NativeArray<int> HkgTxuBnmSvrbhRgxviAtTXEVugttPIM;
        private ComputeBuffer OJJaFHdtmArvByfvKRmTcHwCULIJfmkP;
        private ComputeBuffer DhdVoErQCKDQQDnEZPakGAPxXZhbDBJP;
        private ComputeBuffer HgrCHuAJEmGJZnApsBJXhmDtvzWGkZRl;

        private ComputeBuffer CsIobkAteDrSWDolqSYLwJFkpUlHqipR;
        private Bounds TfiZzvsIEUPwkmYCBohGCfjPeXouBbvO = new Bounds(Vector3.zero, Vector3.one * 10000); 

        [SerializeField]
        private Shader shader;

        [SerializeField]
        private Mesh particleMesh;

        private void NUPlbAKjHXAjQlSEJJkyNxqRAXcELsCY()
        {
            UVkRRDpVHEdoTCUYJVhGmsVFJZYiaRDF();
        }

        private void oMeTgFjYJTssEsvjHPqKLLJfSfCicRhJ()
        {
            DukDyjwocJJHcySULhWnPWjFGWrnPxAB.LatePhysicsUpdate.RemoveListener(ARhzUnMlWESTFZgDXiJIwgZCtmMJReEm);
            IuuktGNNnTDFbZWZFgjOfUBhfNUCEHNa();
        }

        private void vpmwQwkvwwQZSasLvMNpONnzJudJVoDK()
        {
            if (GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr > 0)
            {
                Graphics.DrawMeshInstancedIndirect(particleMesh, 0, YpdsfXEtSIBAIlaFJRzpNCDZkOKaKYyh, TfiZzvsIEUPwkmYCBohGCfjPeXouBbvO, CsIobkAteDrSWDolqSYLwJFkpUlHqipR);
            }
        }

        private void UVkRRDpVHEdoTCUYJVhGmsVFJZYiaRDF()
        {
            DukDyjwocJJHcySULhWnPWjFGWrnPxAB = GetComponent<PhysicsWorld>();

            if (DukDyjwocJJHcySULhWnPWjFGWrnPxAB == null)
            {
                UnityEngine.Debug.LogError("PhysicsWorld component not found.");
                enabled = false;
                return;
            }

            YpdsfXEtSIBAIlaFJRzpNCDZkOKaKYyh = new Material(shader);

            urAovhexZHwYnMUcnKbsdXupXbAuqmJm();

            Matrix4x4 tbDnnLUatgkkwJGPZDDdiQJBWdzdLwXR = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
            YpdsfXEtSIBAIlaFJRzpNCDZkOKaKYyh.SetMatrix("localToWorld", tbDnnLUatgkkwJGPZDDdiQJBWdzdLwXR);

            DukDyjwocJJHcySULhWnPWjFGWrnPxAB.LatePhysicsUpdate.AddListener(ARhzUnMlWESTFZgDXiJIwgZCtmMJReEm);
        }

        private void ARhzUnMlWESTFZgDXiJIwgZCtmMJReEm(float timeStep)
        {
            AgsLRcgzPWBSZtgYYrSxXvVguSBoaROT();
        }

        private void urAovhexZHwYnMUcnKbsdXupXbAuqmJm()
        {
            GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr = DukDyjwocJJHcySULhWnPWjFGWrnPxAB.ParticleData.Count;
            if (GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr > 0)
            {
                QAiKQoFcpHmmkbMdXIlPGXJDeoxzArRz = new ComputeBuffer(GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr, sizeof(float) * 3);
                HkgTxuBnmSvrbhRgxviAtTXEVugttPIM = new NativeArray<int>(GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr, Allocator.Persistent);
                OJJaFHdtmArvByfvKRmTcHwCULIJfmkP = new ComputeBuffer(GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr, sizeof(int));
                DhdVoErQCKDQQDnEZPakGAPxXZhbDBJP = new ComputeBuffer(GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr, sizeof(uint));
                HgrCHuAJEmGJZnApsBJXhmDtvzWGkZRl = new ComputeBuffer(GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr, sizeof(float));
                CsIobkAteDrSWDolqSYLwJFkpUlHqipR = dJhbaBlBiFPXOmCbQbItsvHmoRtOMpnI(particleMesh, GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr);
            }
        }

        private void AgsLRcgzPWBSZtgYYrSxXvVguSBoaROT()
        {
            GltbqWRtFSDetgESEvvlQXoWaKbiGhIC();

            if (GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr > 0)
            {
                
                QAiKQoFcpHmmkbMdXIlPGXJDeoxzArRz.SetData(DukDyjwocJJHcySULhWnPWjFGWrnPxAB.ParticleData.Position);

                Parallel.For(0, GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr, i =>
                {
                    HkgTxuBnmSvrbhRgxviAtTXEVugttPIM[i] = DukDyjwocJJHcySULhWnPWjFGWrnPxAB.ParticleData.Enabled[i] ? 1 : 0;
                });

                OJJaFHdtmArvByfvKRmTcHwCULIJfmkP.SetData(HkgTxuBnmSvrbhRgxviAtTXEVugttPIM);
                DhdVoErQCKDQQDnEZPakGAPxXZhbDBJP.SetData(DukDyjwocJJHcySULhWnPWjFGWrnPxAB.ParticleData.Phase);
                HgrCHuAJEmGJZnApsBJXhmDtvzWGkZRl.SetData(DukDyjwocJJHcySULhWnPWjFGWrnPxAB.ParticleData.InverseMass);

                
                YpdsfXEtSIBAIlaFJRzpNCDZkOKaKYyh.SetFloat("scale", DukDyjwocJJHcySULhWnPWjFGWrnPxAB.ParticleRadius * 2.0f);
            }
        }

        private void IuuktGNNnTDFbZWZFgjOfUBhfNUCEHNa()
        {
            QAiKQoFcpHmmkbMdXIlPGXJDeoxzArRz.Release();
            OJJaFHdtmArvByfvKRmTcHwCULIJfmkP.Release();
            DhdVoErQCKDQQDnEZPakGAPxXZhbDBJP.Release();
            HgrCHuAJEmGJZnApsBJXhmDtvzWGkZRl.Release();

            CsIobkAteDrSWDolqSYLwJFkpUlHqipR.Release();
        }

        private void GltbqWRtFSDetgESEvvlQXoWaKbiGhIC()
        {
            if (GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr != DukDyjwocJJHcySULhWnPWjFGWrnPxAB.ParticleData.Count)
            {
                if (GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr > 0)
                {
                    IuuktGNNnTDFbZWZFgjOfUBhfNUCEHNa();
                }

                GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr = DukDyjwocJJHcySULhWnPWjFGWrnPxAB.ParticleData.Count;

                if (GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr > 0)
                {
                    QAiKQoFcpHmmkbMdXIlPGXJDeoxzArRz = new ComputeBuffer(GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr, sizeof(float) * 3);
                    YpdsfXEtSIBAIlaFJRzpNCDZkOKaKYyh.SetBuffer("ParticlesPosition", QAiKQoFcpHmmkbMdXIlPGXJDeoxzArRz);

                    HkgTxuBnmSvrbhRgxviAtTXEVugttPIM.ResizeArray(GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
                    OJJaFHdtmArvByfvKRmTcHwCULIJfmkP = new ComputeBuffer(GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr, sizeof(int));
                    YpdsfXEtSIBAIlaFJRzpNCDZkOKaKYyh.SetBuffer("ParticlesEnabled", OJJaFHdtmArvByfvKRmTcHwCULIJfmkP);

                    DhdVoErQCKDQQDnEZPakGAPxXZhbDBJP = new ComputeBuffer(GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr, sizeof(uint));
                    YpdsfXEtSIBAIlaFJRzpNCDZkOKaKYyh.SetBuffer("ParticlesPhase", DhdVoErQCKDQQDnEZPakGAPxXZhbDBJP);

                    HgrCHuAJEmGJZnApsBJXhmDtvzWGkZRl = new ComputeBuffer(GTULUkxdYLOwCEKnCeYOrNEoTWJxjIRr, sizeof(float));
                    YpdsfXEtSIBAIlaFJRzpNCDZkOKaKYyh.SetBuffer("ParticlesInverseMass", HgrCHuAJEmGJZnApsBJXhmDtvzWGkZRl);

                    CsIobkAteDrSWDolqSYLwJFkpUlHqipR = dJhbaBlBiFPXOmCbQbItsvHmoRtOMpnI(particleMesh, QAiKQoFcpHmmkbMdXIlPGXJDeoxzArRz.count);
                }
            }
        }

        
        private ComputeBuffer dJhbaBlBiFPXOmCbQbItsvHmoRtOMpnI(Mesh mesh, int numInstances)
        {
            const int pNoSNCGbrwHEQjDakRnnfEvdRDpGzjMi = 0;
            uint[] HUzFpOIzaBaGeKkbrKObwAyngbTWvprM = new uint[5];
            HUzFpOIzaBaGeKkbrKObwAyngbTWvprM[0] = (uint)mesh.GetIndexCount(pNoSNCGbrwHEQjDakRnnfEvdRDpGzjMi);
            HUzFpOIzaBaGeKkbrKObwAyngbTWvprM[1] = (uint)numInstances;
            HUzFpOIzaBaGeKkbrKObwAyngbTWvprM[2] = (uint)mesh.GetIndexStart(pNoSNCGbrwHEQjDakRnnfEvdRDpGzjMi);
            HUzFpOIzaBaGeKkbrKObwAyngbTWvprM[3] = (uint)mesh.GetBaseVertex(pNoSNCGbrwHEQjDakRnnfEvdRDpGzjMi);
            HUzFpOIzaBaGeKkbrKObwAyngbTWvprM[4] = 0; 

            ComputeBuffer QWPdHLXNbwnfMZWKajTOUPQYgsBhgFPN = new ComputeBuffer(1, 5 * sizeof(uint), ComputeBufferType.IndirectArguments);
            QWPdHLXNbwnfMZWKajTOUPQYgsBhgFPN.SetData(HUzFpOIzaBaGeKkbrKObwAyngbTWvprM);
            return QWPdHLXNbwnfMZWKajTOUPQYgsBhgFPN;
        }
    }
}
#endif