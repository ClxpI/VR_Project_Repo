namespace MAGES.MeshDeformations.Editor
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MAGES.MeshDeformations.Tools;
    using Unity.Mathematics;
    using UnityEditor;
    using UnityEditor.EditorTools;
    using UnityEditor.Overlays;
    using UnityEngine;
    using UnityEngine.UIElements;
    using static MAGES.MeshDeformations.Tools.VolumeConstraintsDestructionDetector;

    /// <summary>
    /// Custom Inspector for the <see cref="VolumeConstraintsDestructionDetector"/> component.
    /// </summary>
    [CustomEditor(typeof(VolumeConstraintsDestructionDetector))]
    internal class VolumeConstraintsDestructionDetectorEditor : Editor
    {
        VisualElement GZrTGHQArOIetoQqWMFVPsMgDWqHveYJ;

        /// <inheritdoc/>
        public override VisualElement CreateInspectorGUI()
        {
            return base.CreateInspectorGUI();
        }
    }

    [EditorTool("Edit Volume Constraint Groups", typeof(VolumeConstraintsDestructionDetector))]
    internal class VolumeConstraintsDestructionDetectorSelectionTool : EditorTool
    {
        private const string rLcuHDnEUoDwilmlqayqQvdmyPkRXRwQ = "Packages/com.oramavr.mages.experimental/MeshDeformations/Editor/Icons/edit-distance-joint.png";
        private static float xeSDuOaoxALCZCdmSuecPWprZyytGimT = .1f;

        VolumeConstraintsDestructionDetectorOverlay LuLWIPMDrUhASCNXmtbBdkudmLSIqosd;
        private VolumeConstraintsDestructionDetector PFWcLAFasWhHEoEnaSTwegMIRUMkznVs;

        private float3[] RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK;
        private jZQeYdvMkmGWFBWlImvaPuLllydnegIY[] LuSfMADCUgmojpUkjcujwoiXzRZSwxys;

        private GUIContent mDnUmdpwBTMEKnFZcvoeUHIdtnCXqgPJ;

        private int XpjtNyiOhPneEJYyRadqiAiDTZmRmgIj = -1;

        /// <inheritdoc/>
        public override GUIContent toolbarIcon => mDnUmdpwBTMEKnFZcvoeUHIdtnCXqgPJ;

        /// <inheritdoc/>
        public override void OnActivated()
        {
            SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Entering Volume Constraint Selection Tool"), .5f);

            PFWcLAFasWhHEoEnaSTwegMIRUMkznVs = target as VolumeConstraintsDestructionDetector;
            LuLWIPMDrUhASCNXmtbBdkudmLSIqosd = new VolumeConstraintsDestructionDetectorOverlay(PFWcLAFasWhHEoEnaSTwegMIRUMkznVs, gEnvILsDmWcNkoHoovwrlwCIBXBzdbVM);
            SceneView.AddOverlayToActiveView(LuLWIPMDrUhASCNXmtbBdkudmLSIqosd);

            if (Application.isPlaying || PFWcLAFasWhHEoEnaSTwegMIRUMkznVs.Actor == null || PFWcLAFasWhHEoEnaSTwegMIRUMkznVs.Actor.SharedSimulationMesh == null)
            {
                ToolManager.RestorePreviousPersistentTool();
                return;
            }

            
            RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK = new float3[PFWcLAFasWhHEoEnaSTwegMIRUMkznVs.Actor.SharedSimulationMesh.Particles.Length];
            for (int jWQThdyhWqoGkJXiyeHOFkQAJLdfpCvj = 0; jWQThdyhWqoGkJXiyeHOFkQAJLdfpCvj < RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK.Length; jWQThdyhWqoGkJXiyeHOFkQAJLdfpCvj++)
            {
                RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK[jWQThdyhWqoGkJXiyeHOFkQAJLdfpCvj] = PFWcLAFasWhHEoEnaSTwegMIRUMkznVs.Actor.SharedSimulationMesh.Particles[jWQThdyhWqoGkJXiyeHOFkQAJLdfpCvj].Position;
            }

            LuSfMADCUgmojpUkjcujwoiXzRZSwxys = new jZQeYdvMkmGWFBWlImvaPuLllydnegIY[PFWcLAFasWhHEoEnaSTwegMIRUMkznVs.Actor.SharedSimulationMesh.VolumeConstraints.Length];
            for (int fdZGhODtYNJgvTtxGOyuqfOXIkASGbPu = 0; fdZGhODtYNJgvTtxGOyuqfOXIkASGbPu < LuSfMADCUgmojpUkjcujwoiXzRZSwxys.Length; fdZGhODtYNJgvTtxGOyuqfOXIkASGbPu++)
            {
                LuSfMADCUgmojpUkjcujwoiXzRZSwxys[fdZGhODtYNJgvTtxGOyuqfOXIkASGbPu] = new jZQeYdvMkmGWFBWlImvaPuLllydnegIY { Indices = new int4(PFWcLAFasWhHEoEnaSTwegMIRUMkznVs.Actor.SharedSimulationMesh.VolumeConstraints[fdZGhODtYNJgvTtxGOyuqfOXIkASGbPu].ParticleA, PFWcLAFasWhHEoEnaSTwegMIRUMkznVs.Actor.SharedSimulationMesh.VolumeConstraints[fdZGhODtYNJgvTtxGOyuqfOXIkASGbPu].ParticleB, PFWcLAFasWhHEoEnaSTwegMIRUMkznVs.Actor.SharedSimulationMesh.VolumeConstraints[fdZGhODtYNJgvTtxGOyuqfOXIkASGbPu].ParticleC, PFWcLAFasWhHEoEnaSTwegMIRUMkznVs.Actor.SharedSimulationMesh.VolumeConstraints[fdZGhODtYNJgvTtxGOyuqfOXIkASGbPu].ParticleD), Selected = false };
            }
        }

        /// <inheritdoc/>
        public override void OnWillBeDeactivated()
        {
            
            olLoHOlPRrOoAnWLpCOutsOLVhIewZBW();

            SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Exiting Volume Constraint Selection Tool"), .5f);

            SceneView.RemoveOverlayFromActiveView(LuLWIPMDrUhASCNXmtbBdkudmLSIqosd);
        }

        /// <inheritdoc/>
        public override void OnToolGUI(EditorWindow window)
        {
            PFWcLAFasWhHEoEnaSTwegMIRUMkznVs = target as VolumeConstraintsDestructionDetector;

            if (XpjtNyiOhPneEJYyRadqiAiDTZmRmgIj != -1)
            {
                pAtHLVACkfPxfltUolpcVnsMYiRTfucD(XpjtNyiOhPneEJYyRadqiAiDTZmRmgIj, LuLWIPMDrUhASCNXmtbBdkudmLSIqosd.HideSelectedVolumeConstraints);
            }
        }

        private void gEnvILsDmWcNkoHoovwrlwCIBXBzdbVM(int newGroupSelection)
        {
            
            olLoHOlPRrOoAnWLpCOutsOLVhIewZBW();

            XpjtNyiOhPneEJYyRadqiAiDTZmRmgIj = newGroupSelection;

            
            OhaejdeHcQbVoWjjWXczNHYTdXvSRpQl();
        }

        private void OhaejdeHcQbVoWjjWXczNHYTdXvSRpQl()
        {
            
            Parallel.For(0, LuSfMADCUgmojpUkjcujwoiXzRZSwxys.Length, (i) =>
            {
                LuSfMADCUgmojpUkjcujwoiXzRZSwxys[i].Selected = false;
            });

            foreach (ObservedVolumeConstraint vc in PFWcLAFasWhHEoEnaSTwegMIRUMkznVs.ObservedVolumeConstraintGroups[XpjtNyiOhPneEJYyRadqiAiDTZmRmgIj].ObservedVolumeConstraints)
            {
                LuSfMADCUgmojpUkjcujwoiXzRZSwxys[vc.VolumeConstraint].Selected = true;
            }
        }

        private void olLoHOlPRrOoAnWLpCOutsOLVhIewZBW()
        {
            if (XpjtNyiOhPneEJYyRadqiAiDTZmRmgIj < 0)
            {
                return;
            }

            Undo.RecordObject(PFWcLAFasWhHEoEnaSTwegMIRUMkznVs, "Volume Constraint Group Selection Changed");
            List<ObservedVolumeConstraint> rawMRHGzkpSVcBLOKjBxsbQhTfGoBamE = new List<ObservedVolumeConstraint>();
            for (int gsDzNEvQSMWTgXoRBdZCXWMMgzQxFnOG = 0; gsDzNEvQSMWTgXoRBdZCXWMMgzQxFnOG < LuSfMADCUgmojpUkjcujwoiXzRZSwxys.Length; gsDzNEvQSMWTgXoRBdZCXWMMgzQxFnOG++)
            {
                if (LuSfMADCUgmojpUkjcujwoiXzRZSwxys[gsDzNEvQSMWTgXoRBdZCXWMMgzQxFnOG].Selected)
                {
                    rawMRHGzkpSVcBLOKjBxsbQhTfGoBamE.Add(new ObservedVolumeConstraint { VolumeConstraint = gsDzNEvQSMWTgXoRBdZCXWMMgzQxFnOG });
                }
            }

            PFWcLAFasWhHEoEnaSTwegMIRUMkznVs.ObservedVolumeConstraintGroups[XpjtNyiOhPneEJYyRadqiAiDTZmRmgIj].ObservedVolumeConstraints = rawMRHGzkpSVcBLOKjBxsbQhTfGoBamE.ToArray();
            EditorUtility.SetDirty(PFWcLAFasWhHEoEnaSTwegMIRUMkznVs);
        }

        /// <summary>
        /// Shows the handles for editing a group.
        /// </summary>
        /// <param name="groupIndex">The group index.</param>
        /// <param name="inverseMode">Modes: Normal - Show selected tetrahedrons with red
        ///                                  Inverse - Show deselected tetrahedrons with green, hide selected.
        /// </param>
        private void pAtHLVACkfPxfltUolpcVnsMYiRTfucD(int groupIndex, bool inverseMode)
        {
            var BjSZDZKiNKcPrzJHnRFlNofWUxqceGQX = PFWcLAFasWhHEoEnaSTwegMIRUMkznVs.ObservedVolumeConstraintGroups[groupIndex];

            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                Ray WHEGxsdpPPTdlbxYbvJnSjfKJXninDmy = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                var rkFqTdDlUqlxdVUVGdJOZXGaqXMgMpxh = bJXKAYlXvaZjDeIZEVdlbTdmZOcMvBXL(WHEGxsdpPPTdlbxYbvJnSjfKJXninDmy, LuLWIPMDrUhASCNXmtbBdkudmLSIqosd.HideSelectedVolumeConstraints);

                if (rkFqTdDlUqlxdVUVGdJOZXGaqXMgMpxh >= 0)
                {
                    
                    if (Event.current.shift)
                    {
                        LuSfMADCUgmojpUkjcujwoiXzRZSwxys[rkFqTdDlUqlxdVUVGdJOZXGaqXMgMpxh].Selected = false;
                    }

                    
                    else
                    {
                        LuSfMADCUgmojpUkjcujwoiXzRZSwxys[rkFqTdDlUqlxdVUVGdJOZXGaqXMgMpxh].Selected = true;
                        UnityEngine.Debug.Log($"Selected Volume Constraint {rkFqTdDlUqlxdVUVGdJOZXGaqXMgMpxh}");
                    }

                }

                Event.current.Use();
            }

            
            if (!inverseMode)
            {
                foreach (jZQeYdvMkmGWFBWlImvaPuLllydnegIY tetInfo in LuSfMADCUgmojpUkjcujwoiXzRZSwxys)
                {
                    if (!tetInfo.Selected)
                    {
                        continue;
                    }

                    var bAcGEiFBbGCIeZqgWZSRyzfFfFKgjJbM = RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK[tetInfo.Indices.x];
                    var jNLltLeMNSMXxgyQDjuVuuuIPsKpnPMz = RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK[tetInfo.Indices.y];
                    var fBmZEwhpZLDhEucjkbeguDmzMZjdcdQC = RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK[tetInfo.Indices.z];
                    var papozjvluSOAGosNliyuzPtvrmGeEpPt = RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK[tetInfo.Indices.w];

                    UZcrBOOhriaNOpOavszygfrmjDsEkMvr(bAcGEiFBbGCIeZqgWZSRyzfFfFKgjJbM, jNLltLeMNSMXxgyQDjuVuuuIPsKpnPMz, fBmZEwhpZLDhEucjkbeguDmzMZjdcdQC, papozjvluSOAGosNliyuzPtvrmGeEpPt, Color.red, PFWcLAFasWhHEoEnaSTwegMIRUMkznVs.transform.localToWorldMatrix);
                }
            }

            
            else
            {
                foreach (jZQeYdvMkmGWFBWlImvaPuLllydnegIY tetInfo in LuSfMADCUgmojpUkjcujwoiXzRZSwxys)
                {
                    if (tetInfo.Selected)
                    {
                        continue;
                    }

                    var DoPmigXiNQQMGTUCbeUuzTygDNveVpjC = RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK[tetInfo.Indices.x];
                    var CvUtzOrMfwApHCenoeaTXLfsXmSPnfZl = RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK[tetInfo.Indices.y];
                    var FPyEsBXYjRbXxwRGTUIngzadbDOCaqbj = RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK[tetInfo.Indices.z];
                    var IpRcfNtDnhBJUdpwxWizfIGNQqLkOnxl = RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK[tetInfo.Indices.w];

                    UZcrBOOhriaNOpOavszygfrmjDsEkMvr(DoPmigXiNQQMGTUCbeUuzTygDNveVpjC, CvUtzOrMfwApHCenoeaTXLfsXmSPnfZl, FPyEsBXYjRbXxwRGTUIngzadbDOCaqbj, IpRcfNtDnhBJUdpwxWizfIGNQqLkOnxl, Color.green, PFWcLAFasWhHEoEnaSTwegMIRUMkznVs.transform.localToWorldMatrix);
                }
            }
        }

        private int bJXKAYlXvaZjDeIZEVdlbTdmZOcMvBXL(Ray r, bool ignoreSelected=false)
        {
            Matrix4x4 dGTtRCQBSHzseGuSSvcVYMGwTkuQNtyX = PFWcLAFasWhHEoEnaSTwegMIRUMkznVs.transform.localToWorldMatrix;
            ConcurrentBag<(int, float3)> jnWizJxuiYcNcYKcegWgkTRzEKXduXPp = new ConcurrentBag<(int, float3)>();
            Parallel.For(0, LuSfMADCUgmojpUkjcujwoiXzRZSwxys.Length, (i) =>
            {
                var QCEpRuMNIjZVWXRPZBGDYFRUXcvCeJXb = LuSfMADCUgmojpUkjcujwoiXzRZSwxys[i];

                if (ignoreSelected && QCEpRuMNIjZVWXRPZBGDYFRUXcvCeJXb.Selected)
                {
                    return;
                }

                var sjBvlitIRFXunNsxsFtpHbQdCUhklYjr = dGTtRCQBSHzseGuSSvcVYMGwTkuQNtyX.MultiplyPoint3x4(RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK[QCEpRuMNIjZVWXRPZBGDYFRUXcvCeJXb.Indices.x]);
                var wDfpLArEZumYwcRJDwQhPcfmHCFdDnYJ = dGTtRCQBSHzseGuSSvcVYMGwTkuQNtyX.MultiplyPoint3x4(RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK[QCEpRuMNIjZVWXRPZBGDYFRUXcvCeJXb.Indices.y]);
                var OdMkXYFgDSCnhLdlcWQBIkiUiroKMWVQ = dGTtRCQBSHzseGuSSvcVYMGwTkuQNtyX.MultiplyPoint3x4(RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK[QCEpRuMNIjZVWXRPZBGDYFRUXcvCeJXb.Indices.z]);
                var gOWEtSPkMBjMiQbLTzCqktARSxrmhZXE = dGTtRCQBSHzseGuSSvcVYMGwTkuQNtyX.MultiplyPoint3x4(RAYsuCufhmFVRDJgUtVkFbRbAmEopBLK[QCEpRuMNIjZVWXRPZBGDYFRUXcvCeJXb.Indices.w]);

                if (Utilities.Math.RayTetrahedronIntersection(r, sjBvlitIRFXunNsxsFtpHbQdCUhklYjr, wDfpLArEZumYwcRJDwQhPcfmHCFdDnYJ, OdMkXYFgDSCnhLdlcWQBIkiUiroKMWVQ, gOWEtSPkMBjMiQbLTzCqktARSxrmhZXE, out float3 intersectionPoint))
                {
                    jnWizJxuiYcNcYKcegWgkTRzEKXduXPp.Add((i, intersectionPoint));
                }
            });

            if (jnWizJxuiYcNcYKcegWgkTRzEKXduXPp.IsEmpty)
            {
                return -1;
            }

            
            var GefXHZlAXTdKBgslaZjjQbDzHwnuNcDP = jnWizJxuiYcNcYKcegWgkTRzEKXduXPp.ToList();
            GefXHZlAXTdKBgslaZjjQbDzHwnuNcDP.Sort((a, b) => math.distancesq(a.Item2, r.origin).CompareTo(math.distancesq(b.Item2, r.origin)));

            
            return GefXHZlAXTdKBgslaZjjQbDzHwnuNcDP[0].Item1;
        }

        private void hysTLGErZKggBmChLYFNNwRXhgnkUPuY()
        {
            
            
            mDnUmdpwBTMEKnFZcvoeUHIdtnCXqgPJ = new GUIContent("Edit Volume Constraint Groups");
        }

        private void fGoWmzWYJtEonkLoDBzwmieNmXyKZudN()
        {
            mDnUmdpwBTMEKnFZcvoeUHIdtnCXqgPJ = null;
        }

        private static void UZcrBOOhriaNOpOavszygfrmjDsEkMvr(float3 pos0, float3 pos1, float3 pos2, float3 pos3, Color color, Matrix4x4 mat)
        {
            using (new Handles.DrawingScope(color, mat))
            {
                Handles.DrawAAConvexPolygon(pos1, pos3, pos2);
                Handles.DrawAAConvexPolygon(pos0, pos2, pos3);
                Handles.DrawAAConvexPolygon(pos0, pos3, pos1);
                Handles.DrawAAConvexPolygon(pos0, pos1, pos2);
            }
        }

        private struct jZQeYdvMkmGWFBWlImvaPuLllydnegIY
        {
            public int4 Indices;
            public bool Selected;
        }
    }

    /// <summary>
    /// Overlay for showing <see cref="VolumeConstraintsDestructionDetectorSelectionTool"/> options.
    /// </summary>
    [Overlay(defaultDisplay = true)]
    public class VolumeConstraintsDestructionDetectorOverlay : Overlay
    {
        private VisualElement vPTioinhjIiegYuKLzxUZxyvRKHdtpeq;

        private VolumeConstraintsDestructionDetector zMPrWBxtgShsEIacbsGyDQKdzvbpTSmb;
        private OnGroupSelectionChanged tAvhTpoMDRtLBPScfezoXwfOHNlUEeRh;

        private Toggle OtpYmmXAMQImOitVqHOlTzrJUweJKKLs;

        public delegate void OnGroupSelectionChanged(int newGroupSelection);

        /// <summary>
        /// Gets or sets a value indicating whether the selected volume constraints should be hidden.
        /// </summary>
        public bool HideSelectedVolumeConstraints
        {
            get
            {
                return OtpYmmXAMQImOitVqHOlTzrJUweJKKLs?.value ?? false;
            }
        }

        public VolumeConstraintsDestructionDetectorOverlay(VolumeConstraintsDestructionDetector target, OnGroupSelectionChanged callback) : base()
        {
            this.zMPrWBxtgShsEIacbsGyDQKdzvbpTSmb = target;
            this.tAvhTpoMDRtLBPScfezoXwfOHNlUEeRh = callback;
        }

        public override VisualElement CreatePanelContent()
        {
            vPTioinhjIiegYuKLzxUZxyvRKHdtpeq = new VisualElement();

            var cASgCODgDSrBkknxyzDTpRcWMGMwJTjL = new Label("<b>Select a group to edit:</b>");
            vPTioinhjIiegYuKLzxUZxyvRKHdtpeq.Add(cASgCODgDSrBkknxyzDTpRcWMGMwJTjL);

            var JGbSUfQhbXazwXZLwvpkhqwNTkKZRmGj = new ListView();
            JGbSUfQhbXazwXZLwvpkhqwNTkKZRmGj.itemsSource = zMPrWBxtgShsEIacbsGyDQKdzvbpTSmb.ObservedVolumeConstraintGroups;
            JGbSUfQhbXazwXZLwvpkhqwNTkKZRmGj.selectionType = SelectionType.Single;
            JGbSUfQhbXazwXZLwvpkhqwNTkKZRmGj.showAddRemoveFooter = false;
            JGbSUfQhbXazwXZLwvpkhqwNTkKZRmGj.showFoldoutHeader = false;
            JGbSUfQhbXazwXZLwvpkhqwNTkKZRmGj.showBoundCollectionSize = false;
            JGbSUfQhbXazwXZLwvpkhqwNTkKZRmGj.virtualizationMethod = CollectionVirtualizationMethod.FixedHeight;
            JGbSUfQhbXazwXZLwvpkhqwNTkKZRmGj.fixedItemHeight = 20;

            JGbSUfQhbXazwXZLwvpkhqwNTkKZRmGj.makeItem += () =>
            {
                return new Label("Group Name");
            };
            JGbSUfQhbXazwXZLwvpkhqwNTkKZRmGj.bindItem += (e, i) =>
            {
                (e as Label).text = $"Group {i}";
            };
            JGbSUfQhbXazwXZLwvpkhqwNTkKZRmGj.selectionChanged += (sel) =>
            {
                int siyJCpGzbvcdLiutDgbRCAtnQuWVUMSS = JGbSUfQhbXazwXZLwvpkhqwNTkKZRmGj.selectedIndex;
                tAvhTpoMDRtLBPScfezoXwfOHNlUEeRh?.Invoke(siyJCpGzbvcdLiutDgbRCAtnQuWVUMSS);

                NeajsjvstmCHGOBpttPyOQhzJLbOJHVu(siyJCpGzbvcdLiutDgbRCAtnQuWVUMSS != -1);
            };
            vPTioinhjIiegYuKLzxUZxyvRKHdtpeq.Add(JGbSUfQhbXazwXZLwvpkhqwNTkKZRmGj);

            VisualElement sIeHVGZUseIhpOaQCZUEDIaIgPamdDmC = new VisualElement();
            sIeHVGZUseIhpOaQCZUEDIaIgPamdDmC.name = "group-options";

            var kkDlidjZfnLIjknFQSzchFqpfBsrkdSM = new Label("<b>Group Edit Options:</b>");
            kkDlidjZfnLIjknFQSzchFqpfBsrkdSM.style.paddingTop = 5;
            sIeHVGZUseIhpOaQCZUEDIaIgPamdDmC.Add(kkDlidjZfnLIjknFQSzchFqpfBsrkdSM);
            OtpYmmXAMQImOitVqHOlTzrJUweJKKLs = new Toggle();
            OtpYmmXAMQImOitVqHOlTzrJUweJKKLs.label = "Hide Selected Volume Constraints";
            sIeHVGZUseIhpOaQCZUEDIaIgPamdDmC.Add(OtpYmmXAMQImOitVqHOlTzrJUweJKKLs);

            vPTioinhjIiegYuKLzxUZxyvRKHdtpeq.Add(sIeHVGZUseIhpOaQCZUEDIaIgPamdDmC);

            vPTioinhjIiegYuKLzxUZxyvRKHdtpeq.style.height = 200;
            vPTioinhjIiegYuKLzxUZxyvRKHdtpeq.style.paddingLeft = 5;
            vPTioinhjIiegYuKLzxUZxyvRKHdtpeq.style.paddingTop = 5;
            vPTioinhjIiegYuKLzxUZxyvRKHdtpeq.style.paddingRight = 5;
            vPTioinhjIiegYuKLzxUZxyvRKHdtpeq.style.paddingBottom = 5;

            NeajsjvstmCHGOBpttPyOQhzJLbOJHVu(JGbSUfQhbXazwXZLwvpkhqwNTkKZRmGj.selectedIndex != -1);

            return vPTioinhjIiegYuKLzxUZxyvRKHdtpeq;
        }

        private void NeajsjvstmCHGOBpttPyOQhzJLbOJHVu(bool v)
        {
            if (v)
            {
                vPTioinhjIiegYuKLzxUZxyvRKHdtpeq.Q<VisualElement>("group-options").style.display = DisplayStyle.Flex;
            }
            else
            {
                vPTioinhjIiegYuKLzxUZxyvRKHdtpeq.Q<VisualElement>("group-options").style.display = DisplayStyle.None;
            }
        }
    }
}