namespace MAGES.MeshDeformations.Tools
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Detects and invokes a callback when a puncture in the observed triangles occurs.
    /// </summary>
    [RequireComponent(typeof(Puncturable))]
    public class PunctureDetector : MonoBehaviour
    {
        private ObservedTriangle OYkiggBjSqVguENAbMnayPoAioJrxeHO;
        private Puncturable oiTJkBHYrcIBKXulUyQXYgFIoKkrxXjb;
        private bool isNwfPNKaWJHlENNhpHuooyTAicXnxSj = false;
        private bool fxtoKVfOjMpIBtBjDlVMxfnVOrgnfsIk = false;

        /// <summary>
        /// Gets or sets a list of triangles to check for punctures.
        /// </summary>
        [field: SerializeField]
        public List<ObservedTriangle> ObservedTriangles { get; set; } = new List<ObservedTriangle>();

        /// <summary>
        /// Gets or sets the event called once, when the puncturable is first punctured. This is evaluated after evaluating all observed triangles.
        /// </summary>
        [field: SerializeField]
        public UnityEvent OnPunctured { get; set; } = new UnityEvent();

        /// <summary>
        /// Gets or sets the event called once, when the puncturable is unpunctured. This is evaluated after evaluating all observed triangles.
        /// </summary>
        [field: SerializeField]
        public UnityEvent OnUnpunctured { get; set; } = new UnityEvent();

        /// <summary>
        /// Gets the puncturable component.
        /// </summary>
        public Puncturable Puncturable
        {
            get
            {
                if (oiTJkBHYrcIBKXulUyQXYgFIoKkrxXjb == null)
                {
                    oiTJkBHYrcIBKXulUyQXYgFIoKkrxXjb = GetComponent<Puncturable>();
                }

                return oiTJkBHYrcIBKXulUyQXYgFIoKkrxXjb;
            }
        }

        private void YutUCMTVohZUPSJTukgPYozVfmBVYHca()
        {
            Puncturable.PhysicsWorld?.DetectorsUpdate.AddListener(YBowBgVbUfbihwWROJeQmXBSWVmzFpIj);
        }

        private void ZuNrnlLDYVVRamvqbAwhaWYPUOSMlGQr()
        {
            Puncturable.PhysicsWorld?.DetectorsUpdate.RemoveListener(YBowBgVbUfbihwWROJeQmXBSWVmzFpIj);
        }

        private void YBowBgVbUfbihwWROJeQmXBSWVmzFpIj()
        {
            
            foreach (var ot in ObservedTriangles)
            {
                ot.IsPunctured = false;
            }

            
            unsafe
            {
                int TRrxyJVGixjLrtkqaTtSAWbnqSKmQNfp = *Puncturable.InsertionConstraintsCount.Counter;

                for (int tYkMLiXgneNbDNrJKjgkhLgWLSNsviQq = 0; tYkMLiXgneNbDNrJKjgkhLgWLSNsviQq < TRrxyJVGixjLrtkqaTtSAWbnqSKmQNfp; tYkMLiXgneNbDNrJKjgkhLgWLSNsviQq++)
                {
                    foreach (var ot in ObservedTriangles)
                    {
                        if (ot.PuncturableTriangleIndex != Puncturable.InsertionConstraints[tYkMLiXgneNbDNrJKjgkhLgWLSNsviQq].TriangleIndex)
                        {
                            continue;
                        }

                        ot.IsPunctured = true;
                        if (ot.WasPunctured)
                        {
                            continue;
                        }

                        
                        ot.WasPunctured = true;
                        ot.OnPunctured?.Invoke();
                    }
                }

                isNwfPNKaWJHlENNhpHuooyTAicXnxSj = TRrxyJVGixjLrtkqaTtSAWbnqSKmQNfp > 0;

                if (!fxtoKVfOjMpIBtBjDlVMxfnVOrgnfsIk && isNwfPNKaWJHlENNhpHuooyTAicXnxSj)
                {
                    OnPunctured?.Invoke();
                }
                else if (fxtoKVfOjMpIBtBjDlVMxfnVOrgnfsIk && !isNwfPNKaWJHlENNhpHuooyTAicXnxSj)
                {
                    OnUnpunctured?.Invoke();
                }

                fxtoKVfOjMpIBtBjDlVMxfnVOrgnfsIk = isNwfPNKaWJHlENNhpHuooyTAicXnxSj;
            }

            
            foreach (var ot in ObservedTriangles)
            {
                if (ot.WasPunctured && !ot.IsPunctured)
                {
                    ot.WasPunctured = false;
                    ot.OnUnpunctured?.Invoke();
                }
            }
        }

        /// <summary>
        /// An observed triangle that can be punctured.
        /// </summary>
        [System.Serializable]
        public class ObservedTriangle
        {
            /// <summary>
            /// The triangle index.
            /// </summary>
#pragma warning disable SA1401 // Fields should be private
            public int PuncturableTriangleIndex = 0;

            /// <summary>
            /// Whether the triangle is punctured.
            /// </summary>
            [HideInInspector]
            public bool IsPunctured = false;

            /// <summary>
            /// Whether it was punctured in the last frame.
            /// </summary>
            [HideInInspector]
            public bool WasPunctured = false;

            /// <summary>
            /// Event invoked when the triangle is punctured.
            /// </summary>
            public UnityEvent OnPunctured = new UnityEvent();

            /// <summary>
            /// Event invoked when the triangle is unpunctured.
            /// </summary>
            public UnityEvent OnUnpunctured = new UnityEvent();
#pragma warning restore SA1401 // Fields should be private
        }
    }
}
