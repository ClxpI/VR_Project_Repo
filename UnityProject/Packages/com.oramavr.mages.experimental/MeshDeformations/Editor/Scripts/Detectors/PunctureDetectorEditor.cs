namespace MAGES.MeshDeformations.Editor
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MAGES.Editor.Utilities;
    using MAGES.MeshDeformations.Tools;
    using Unity.Mathematics;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    /// <summary>
    /// A custom inspector for <see cref="PunctureDetector"/>.
    /// </summary>
    [CustomEditor(typeof(PunctureDetector))]
    internal class PunctureDetectorEditor : UnityEditor.Editor
    {
        private ListView nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK;
        private List<int> MeizpygCQKqbbfiwCHVERbLCtbkpUTed = new List<int>();

        private int DyLpoSUNeAOKVWPsPyWNmeFYQblxZzOF = -1;

        /// <inheritdoc/>
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement qZOitDrcQfGDZnyFjeLAnFWhSMeESRbZ = new VisualElement();

            PunctureDetector aNttYQEulDBmUtMLGJAmQNrMQkcWirnN = target as PunctureDetector;

            nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK = new ListView();
            nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK.name = "observed-triangles";
            nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK.BindProperty(serializedObject.FindFieldProperty("ObservedTriangles"));
            nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK.showFoldoutHeader = true;
            nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK.headerTitle = "Observed Triangles";
            nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK.showBoundCollectionSize = true;
            nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK.showAddRemoveFooter = true;
            nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK.selectionType = SelectionType.Single;

            nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK.makeItem = () =>
            {
                var NZUacGkjDxmbdwIFDAqAoTqPZetdKItm = new VisualElement();
                NZUacGkjDxmbdwIFDAqAoTqPZetdKItm.style.paddingBottom = 2;
                NZUacGkjDxmbdwIFDAqAoTqPZetdKItm.style.paddingLeft = 2;
                NZUacGkjDxmbdwIFDAqAoTqPZetdKItm.style.paddingRight = 2;
                NZUacGkjDxmbdwIFDAqAoTqPZetdKItm.style.paddingTop = 2;

                NZUacGkjDxmbdwIFDAqAoTqPZetdKItm.style.flexGrow = 1;
                NZUacGkjDxmbdwIFDAqAoTqPZetdKItm.style.flexShrink = 1;

                var MfthIRjkAMNnMdTUXFuucUAjHXIMmBzg = new Label();
                MfthIRjkAMNnMdTUXFuucUAjHXIMmBzg.name = "tri-index";
                MfthIRjkAMNnMdTUXFuucUAjHXIMmBzg.style.marginBottom = 5;

                var DcQuTCxvuOKVlPKvhkoQPPSSMSmJDWzu = new PropertyField();
                DcQuTCxvuOKVlPKvhkoQPPSSMSmJDWzu.name = "on-puncture-event";
                DcQuTCxvuOKVlPKvhkoQPPSSMSmJDWzu.style.marginBottom = 5;

                var PLmETSIGIkeUIriZKITYbibJOvnibDpo = new PropertyField();
                PLmETSIGIkeUIriZKITYbibJOvnibDpo.name = "on-unpuncture-event";
                PLmETSIGIkeUIriZKITYbibJOvnibDpo.style.marginBottom = 5;

                var vUGMzyXQcCRonEnuKqwxIQxAuGaxcuWx = new Button();
                vUGMzyXQcCRonEnuKqwxIQxAuGaxcuWx.name = "edit-triangle";
                vUGMzyXQcCRonEnuKqwxIQxAuGaxcuWx.text = "Edit Triangle";
                vUGMzyXQcCRonEnuKqwxIQxAuGaxcuWx.style.marginBottom = 5;

                NZUacGkjDxmbdwIFDAqAoTqPZetdKItm.Add(MfthIRjkAMNnMdTUXFuucUAjHXIMmBzg);
                NZUacGkjDxmbdwIFDAqAoTqPZetdKItm.Add(DcQuTCxvuOKVlPKvhkoQPPSSMSmJDWzu);
                NZUacGkjDxmbdwIFDAqAoTqPZetdKItm.Add(PLmETSIGIkeUIriZKITYbibJOvnibDpo);
                NZUacGkjDxmbdwIFDAqAoTqPZetdKItm.Add(vUGMzyXQcCRonEnuKqwxIQxAuGaxcuWx);

                return NZUacGkjDxmbdwIFDAqAoTqPZetdKItm;
            };

            nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK.bindItem = (element, index) =>
            {
                var EUQVWtNXTUzjspBltbmwFAxmZdDUglMv = element.Q<Label>("tri-index");
                EUQVWtNXTUzjspBltbmwFAxmZdDUglMv.text = $"<b>Triangle: {aNttYQEulDBmUtMLGJAmQNrMQkcWirnN.ObservedTriangles[index].PuncturableTriangleIndex}</b>";

                var LOXhfeLMUgvhqEQQatoBcwYwMVdKlvwk = element.Q<PropertyField>("on-puncture-event");
                LOXhfeLMUgvhqEQQatoBcwYwMVdKlvwk.BindProperty(serializedObject.FindFieldProperty("ObservedTriangles").GetArrayElementAtIndex(index).FindPropertyRelative("OnPunctured"));

                var HSXcTgHHlRKIWLJktfwRwxDFQbTXYiiE = element.Q<PropertyField>("on-unpuncture-event");
                HSXcTgHHlRKIWLJktfwRwxDFQbTXYiiE.BindProperty(serializedObject.FindFieldProperty("ObservedTriangles").GetArrayElementAtIndex(index).FindPropertyRelative("OnUnpunctured"));

                var IrKemQSqRfmWBUfNXrEOFHWujSBKTFAh = element.Q<Button>("edit-triangle");
                IrKemQSqRfmWBUfNXrEOFHWujSBKTFAh.clicked += () =>
                {
                    SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Click on Triangle to select it."), 2f);
                    DyLpoSUNeAOKVWPsPyWNmeFYQblxZzOF = index;
                    SceneView.RepaintAll();
                };
            };

            nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK.unbindItem = (element, index) =>
            {
                var XBYfpLfIclxXVRaRInEletNkkFLVMnfs = element.Q<Label>("tri-index");
                XBYfpLfIclxXVRaRInEletNkkFLVMnfs.text = string.Empty;

                var ATrmKSCyFUaKicumohQbsCXazsKmimga = element.Q<PropertyField>("on-puncture-event");
                ATrmKSCyFUaKicumohQbsCXazsKmimga.Unbind();

                var LIEjeWIftgLAVuDmhTFfPUMJTcMVTtcE = element.Q<PropertyField>("on-unpuncture-event");
                LIEjeWIftgLAVuDmhTFfPUMJTcMVTtcE.Unbind();
            };

            qZOitDrcQfGDZnyFjeLAnFWhSMeESRbZ.Add(nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK);

            var QHdlFplnpjdBVmFZMigpyhQhncSHptrT = new PropertyField();
            QHdlFplnpjdBVmFZMigpyhQhncSHptrT.BindProperty(serializedObject.FindFieldProperty("OnPunctured"));
            qZOitDrcQfGDZnyFjeLAnFWhSMeESRbZ.Add(QHdlFplnpjdBVmFZMigpyhQhncSHptrT);

            var yQJOFbpzDslerSTuCmbPEseVbzHFmKAN = new PropertyField();
            yQJOFbpzDslerSTuCmbPEseVbzHFmKAN.BindProperty(serializedObject.FindFieldProperty("OnUnpunctured"));
            qZOitDrcQfGDZnyFjeLAnFWhSMeESRbZ.Add(yQJOFbpzDslerSTuCmbPEseVbzHFmKAN);

            return qZOitDrcQfGDZnyFjeLAnFWhSMeESRbZ;
        }

        private void tkodIynuFwdHqwBFxvCumQhtTAJFPeqw()
        {
            PunctureDetector iQmTpxgZUfAowcSEaAJxnpOQYJgkgPPu = target as PunctureDetector;

            if (nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK == null)
            {
                return;
            }

            if (DyLpoSUNeAOKVWPsPyWNmeFYQblxZzOF != -1)
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    var fysXOCktyDrboduBESBZvLjnfqWBoQMX = YMCvuGwHNURHoVhlksDCHUmOgiKWNuMk(Event.current.mousePosition);

                    if (fysXOCktyDrboduBESBZvLjnfqWBoQMX != -1)
                    {
                        serializedObject.FindFieldProperty("ObservedTriangles").GetArrayElementAtIndex(DyLpoSUNeAOKVWPsPyWNmeFYQblxZzOF).FindPropertyRelative("PuncturableTriangleIndex").intValue = fysXOCktyDrboduBESBZvLjnfqWBoQMX;
                        serializedObject.ApplyModifiedProperties();
                        nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK.RefreshItem(DyLpoSUNeAOKVWPsPyWNmeFYQblxZzOF);
                        nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK.selectedIndex = DyLpoSUNeAOKVWPsPyWNmeFYQblxZzOF;
                        SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Exiting Selection Mode."), .5f);
                    }

                    Event.current.Use();
                    DyLpoSUNeAOKVWPsPyWNmeFYQblxZzOF = -1;
                }
                else
                {
                    DrawTriangle(iQmTpxgZUfAowcSEaAJxnpOQYJgkgPPu.ObservedTriangles[DyLpoSUNeAOKVWPsPyWNmeFYQblxZzOF].PuncturableTriangleIndex);
                }
            }
            else
            {
                if (nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK.selectedIndex != -1 && iQmTpxgZUfAowcSEaAJxnpOQYJgkgPPu.ObservedTriangles.Count > 0)
                {
                    DrawTriangle(iQmTpxgZUfAowcSEaAJxnpOQYJgkgPPu.ObservedTriangles[nHAnYBBiRjRzFguDlPbTtFeLgRvthzpK.selectedIndex].PuncturableTriangleIndex);
                }
            }

            void DrawTriangle(int triIndex)
            {
                
                var PoHGGHbTQZRpvuolggjrOanRZSFlcQlQ = iQmTpxgZUfAowcSEaAJxnpOQYJgkgPPu.gameObject.GetComponent<BasePhysicsActor>().SimulationMesh;
                using (new Handles.DrawingScope(color: Color.red, iQmTpxgZUfAowcSEaAJxnpOQYJgkgPPu.gameObject.transform.localToWorldMatrix))
                {
                    Handles.DrawAAConvexPolygon(PoHGGHbTQZRpvuolggjrOanRZSFlcQlQ.Particles[MeizpygCQKqbbfiwCHVERbLCtbkpUTed[triIndex * 3]].Position, PoHGGHbTQZRpvuolggjrOanRZSFlcQlQ.Particles[MeizpygCQKqbbfiwCHVERbLCtbkpUTed[(triIndex * 3) + 1]].Position, PoHGGHbTQZRpvuolggjrOanRZSFlcQlQ.Particles[MeizpygCQKqbbfiwCHVERbLCtbkpUTed[(triIndex * 3) + 2]].Position);
                }
            }
        }

        private int YMCvuGwHNURHoVhlksDCHUmOgiKWNuMk(Vector2 screenPos)
        {
            PunctureDetector qkmqDLKvqOJTirgioezKkHTGunhkEhFo = target as PunctureDetector;
            var WqPrBcsVvCORzAjPqvOfoGzAHhocrUxt = qkmqDLKvqOJTirgioezKkHTGunhkEhFo.gameObject.GetComponent<BasePhysicsActor>().SimulationMesh;
            Matrix4x4 ImtRToSQjwYIhDghNTWnXwbhbdbSNADG = qkmqDLKvqOJTirgioezKkHTGunhkEhFo.gameObject.transform.localToWorldMatrix;

            ConcurrentBag<(int, float)> lDIoSAPreqXfeeeCkwBLiSACnVnqCDVj = new ConcurrentBag<(int, float)>();
            Ray HklrfKGQGXcgQKTfeSlvKPXWpKRfhvqO = HandleUtility.GUIPointToWorldRay(screenPos);

            Parallel.For(0, MeizpygCQKqbbfiwCHVERbLCtbkpUTed.Count / 3, (i) =>
            {
                var KeSTSwwHRbjnINtyWIBOajtSCEpSrTgZ = ImtRToSQjwYIhDghNTWnXwbhbdbSNADG.MultiplyPoint3x4(WqPrBcsVvCORzAjPqvOfoGzAHhocrUxt.Particles[MeizpygCQKqbbfiwCHVERbLCtbkpUTed[i * 3]].Position);
                var tHdWkmPROnkXZFqtYVWnbYykmFQAafAV = ImtRToSQjwYIhDghNTWnXwbhbdbSNADG.MultiplyPoint3x4(WqPrBcsVvCORzAjPqvOfoGzAHhocrUxt.Particles[MeizpygCQKqbbfiwCHVERbLCtbkpUTed[(i * 3) + 1]].Position);
                var rHBJmLkbCgaoRQkiLnuAeeFCTydXeVBF = ImtRToSQjwYIhDghNTWnXwbhbdbSNADG.MultiplyPoint3x4(WqPrBcsVvCORzAjPqvOfoGzAHhocrUxt.Particles[MeizpygCQKqbbfiwCHVERbLCtbkpUTed[(i * 3) + 2]].Position);

                if (Utilities.Math.RayTriangleIntersection(HklrfKGQGXcgQKTfeSlvKPXWpKRfhvqO, KeSTSwwHRbjnINtyWIBOajtSCEpSrTgZ, tHdWkmPROnkXZFqtYVWnbYykmFQAafAV, rHBJmLkbCgaoRQkiLnuAeeFCTydXeVBF, out var intersectionPoint))
                {
                    lDIoSAPreqXfeeeCkwBLiSACnVnqCDVj.Add((i, math.distancesq(HklrfKGQGXcgQKTfeSlvKPXWpKRfhvqO.origin, intersectionPoint)));
                }
            });

            if (lDIoSAPreqXfeeeCkwBLiSACnVnqCDVj.Count > 0)
            {
                var gVBsteVOuXrWiObRVrarOyNMoeTrfxBT = lDIoSAPreqXfeeeCkwBLiSACnVnqCDVj.OrderBy(x => x.Item2).First();
                return gVBsteVOuXrWiObRVrarOyNMoeTrfxBT.Item1;
            }

            return -1;
        }

        private void RgrDyFkrUkimFGXfIrcvSGYdIhvdVBHQ()
        {
            PGzXWJEfjhxEhltfqSsfCoMgJhPWqwiX();
        }

        private void ZhJYbnibTpLeEFPAiFkuMCgmehLUhITP()
        {
            MeizpygCQKqbbfiwCHVERbLCtbkpUTed.Clear();
        }

        private void PGzXWJEfjhxEhltfqSsfCoMgJhPWqwiX()
        {
            PunctureDetector AkfJrRfsOMJBYYlwNhkoMqwWgVoEuIGj = target as PunctureDetector;

            var KJiDPndiHpOhJJGqJikVbVQOgyfoVqCX = AkfJrRfsOMJBYYlwNhkoMqwWgVoEuIGj.gameObject.GetComponent<BasePhysicsActor>().SimulationMesh;

            HashSet<(int, int, int)> NZDJMdlarDofSliaTtCgaxgrRCPUAEVv = new HashSet<(int, int, int)>();

            for (int bbjaIiZtEBmgSWVfEmVJobjwjJNcvoTd = 0; bbjaIiZtEBmgSWVfEmVJobjwjJNcvoTd < KJiDPndiHpOhJJGqJikVbVQOgyfoVqCX.VolumeConstraints.Length; bbjaIiZtEBmgSWVfEmVJobjwjJNcvoTd++)
            {
                AddTriangle(KJiDPndiHpOhJJGqJikVbVQOgyfoVqCX.VolumeConstraints[bbjaIiZtEBmgSWVfEmVJobjwjJNcvoTd].ParticleA, KJiDPndiHpOhJJGqJikVbVQOgyfoVqCX.VolumeConstraints[bbjaIiZtEBmgSWVfEmVJobjwjJNcvoTd].ParticleB, KJiDPndiHpOhJJGqJikVbVQOgyfoVqCX.VolumeConstraints[bbjaIiZtEBmgSWVfEmVJobjwjJNcvoTd].ParticleC);
                AddTriangle(KJiDPndiHpOhJJGqJikVbVQOgyfoVqCX.VolumeConstraints[bbjaIiZtEBmgSWVfEmVJobjwjJNcvoTd].ParticleA, KJiDPndiHpOhJJGqJikVbVQOgyfoVqCX.VolumeConstraints[bbjaIiZtEBmgSWVfEmVJobjwjJNcvoTd].ParticleD, KJiDPndiHpOhJJGqJikVbVQOgyfoVqCX.VolumeConstraints[bbjaIiZtEBmgSWVfEmVJobjwjJNcvoTd].ParticleB);
                AddTriangle(KJiDPndiHpOhJJGqJikVbVQOgyfoVqCX.VolumeConstraints[bbjaIiZtEBmgSWVfEmVJobjwjJNcvoTd].ParticleB, KJiDPndiHpOhJJGqJikVbVQOgyfoVqCX.VolumeConstraints[bbjaIiZtEBmgSWVfEmVJobjwjJNcvoTd].ParticleD, KJiDPndiHpOhJJGqJikVbVQOgyfoVqCX.VolumeConstraints[bbjaIiZtEBmgSWVfEmVJobjwjJNcvoTd].ParticleC);
                AddTriangle(KJiDPndiHpOhJJGqJikVbVQOgyfoVqCX.VolumeConstraints[bbjaIiZtEBmgSWVfEmVJobjwjJNcvoTd].ParticleA, KJiDPndiHpOhJJGqJikVbVQOgyfoVqCX.VolumeConstraints[bbjaIiZtEBmgSWVfEmVJobjwjJNcvoTd].ParticleC, KJiDPndiHpOhJJGqJikVbVQOgyfoVqCX.VolumeConstraints[bbjaIiZtEBmgSWVfEmVJobjwjJNcvoTd].ParticleD);
            }

            void AddTriangle(int particleAIndex, int particleBIndex, int particleCIndex)
            {
                
                var VuoskcHkVYRJYTxuPqNqrWyTHWFdRLPB = new int[] { particleAIndex, particleBIndex, particleCIndex };
                System.Array.Sort(VuoskcHkVYRJYTxuPqNqrWyTHWFdRLPB);

                (int, int, int) iSAkEiFoTVQlMiDADRxbwINWOgBkvngT = (VuoskcHkVYRJYTxuPqNqrWyTHWFdRLPB[0], VuoskcHkVYRJYTxuPqNqrWyTHWFdRLPB[1], VuoskcHkVYRJYTxuPqNqrWyTHWFdRLPB[2]);

                if (!NZDJMdlarDofSliaTtCgaxgrRCPUAEVv.Contains(iSAkEiFoTVQlMiDADRxbwINWOgBkvngT))
                {
                    NZDJMdlarDofSliaTtCgaxgrRCPUAEVv.Add(iSAkEiFoTVQlMiDADRxbwINWOgBkvngT);
                }
            }

            MeizpygCQKqbbfiwCHVERbLCtbkpUTed.Clear();
            MeizpygCQKqbbfiwCHVERbLCtbkpUTed.Capacity = Mathf.Max(MeizpygCQKqbbfiwCHVERbLCtbkpUTed.Capacity, NZDJMdlarDofSliaTtCgaxgrRCPUAEVv.Count * 3);

            foreach (var triangle in NZDJMdlarDofSliaTtCgaxgrRCPUAEVv)
            {
                MeizpygCQKqbbfiwCHVERbLCtbkpUTed.Add(triangle.Item1);
                MeizpygCQKqbbfiwCHVERbLCtbkpUTed.Add(triangle.Item2);
                MeizpygCQKqbbfiwCHVERbLCtbkpUTed.Add(triangle.Item3);
            }
        }
    }
}
