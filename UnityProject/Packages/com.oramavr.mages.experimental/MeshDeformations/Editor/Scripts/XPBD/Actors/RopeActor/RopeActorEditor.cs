namespace MAGES.MeshDeformations.Editor
{
    using MAGES.Editor.Utilities;
    using UnityEditor;
    using UnityEditor.EditorTools;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.UIElements;

    /// <summary>
    /// Custom Inspector for <see cref="RigidbodyActor"/>.
    /// </summary>
    [CustomEditor(typeof(RopeActor))]
    internal class RopeActorEditor : Editor
    {
        private static readonly string CnnTSPBXERDghuMDCHXKDctPgusIiBMS = "Packages/com.oramavr.mages.experimental/MeshDeformations/Editor/UI/Actors/RopeActor/RopeActorEditor.uxml";

        private VisualElement tpbNXZdozcyoYyzStFYLXmnTDTIOhZeb;

        /// <inheritdoc/>
        public override VisualElement CreateInspectorGUI()
        {
            var mACSOTdNdTxqtnUMPoXdRIECpmMoJOei = target as RopeActor;
            var ywLcRNgoQkXuPYHcJKLTiDNzePSmJKXT = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(CnnTSPBXERDghuMDCHXKDctPgusIiBMS);

            if (ywLcRNgoQkXuPYHcJKLTiDNzePSmJKXT == null)
            {
                Debug.LogError($"Failed to load UXML file at path: {CnnTSPBXERDghuMDCHXKDctPgusIiBMS}");
                return base.CreateInspectorGUI();
            }

            tpbNXZdozcyoYyzStFYLXmnTDTIOhZeb = ywLcRNgoQkXuPYHcJKLTiDNzePSmJKXT.Instantiate();

            
            tpbNXZdozcyoYyzStFYLXmnTDTIOhZeb.Q<Button>("edit-particles").clicked += () =>
            {
                if (ToolManager.activeToolType == typeof(BasePhysicsActorEditorTool))
                {
                    RBmzLAJnNGmdFoKilIRUagupToKzaPZc();
                }
                else
                {
                    if (Application.isPlaying)
                    {
                        SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Particle Editing tool unavailable in play mode"), .5f);
                        return;
                    }
                    else
                    {
                        vZgGGWCFJcBbVfvRjOeAFWOfhdcJZDnz();
                    }
                }
            };

            
            var whtXPKItNrissHsMJXmmjbhRTcXUEvfT = tpbNXZdozcyoYyzStFYLXmnTDTIOhZeb.Q<ObjectField>("physics-world");
            whtXPKItNrissHsMJXmmjbhRTcXUEvfT.objectType = typeof(PhysicsWorld);
            whtXPKItNrissHsMJXmmjbhRTcXUEvfT.value = mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.PhysicsWorld;
            whtXPKItNrissHsMJXmmjbhRTcXUEvfT.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei, "Changed Property in RigidbodyActor");
                mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.PhysicsWorld = evt.newValue as PhysicsWorld;
                EditorUtility.SetDirty(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei);
            });

            var bgtFBWWAHpIVanOeCeENYOkkGnogvArk = tpbNXZdozcyoYyzStFYLXmnTDTIOhZeb.Q<FloatField>("length");
            bgtFBWWAHpIVanOeCeENYOkkGnogvArk.value = mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.Length;
            bgtFBWWAHpIVanOeCeENYOkkGnogvArk.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei, "Changed Property in RigidbodyActor");
                mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.Length = evt.newValue;
                EditorUtility.SetDirty(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei);

                bgtFBWWAHpIVanOeCeENYOkkGnogvArk.SetValueWithoutNotify(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.Length);
            });

            var BfqUrWiUiHJdPctBQunVjNMAWbmNTjGf = tpbNXZdozcyoYyzStFYLXmnTDTIOhZeb.Q<FloatField>("bending-compliance");
            BfqUrWiUiHJdPctBQunVjNMAWbmNTjGf.value = mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.BendingCompliance;
            BfqUrWiUiHJdPctBQunVjNMAWbmNTjGf.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei, "Changed Property in RigidbodyActor");
                mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.BendingCompliance = evt.newValue;
                EditorUtility.SetDirty(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei);

                BfqUrWiUiHJdPctBQunVjNMAWbmNTjGf.SetValueWithoutNotify(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.BendingCompliance);
            });

            var GLqkHuGvAlkdvZOlHrcEXnOtFHNYXJwj = tpbNXZdozcyoYyzStFYLXmnTDTIOhZeb.Q<Toggle>("breakable");
            GLqkHuGvAlkdvZOlHrcEXnOtFHNYXJwj.value = mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.Breakable;
            GLqkHuGvAlkdvZOlHrcEXnOtFHNYXJwj.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei, "Changed Property in RigidbodyActor");
                mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.Breakable = evt.newValue;
                EditorUtility.SetDirty(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei);
            });

            var fSOxYCLhrukucRtYllayoaZOtwLKPovf = tpbNXZdozcyoYyzStFYLXmnTDTIOhZeb.Q<FloatField>("break-force");
            fSOxYCLhrukucRtYllayoaZOtwLKPovf.value = mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.BreakForce;
            fSOxYCLhrukucRtYllayoaZOtwLKPovf.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei, "Changed Property in RigidbodyActor");
                mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.BreakForce = evt.newValue;
                EditorUtility.SetDirty(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei);

                fSOxYCLhrukucRtYllayoaZOtwLKPovf.SetValueWithoutNotify(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.BreakForce);
            });

            var xurQBrNVaOwLYybmLsHMWARRRuUFGIKI = tpbNXZdozcyoYyzStFYLXmnTDTIOhZeb.Q<Toggle>("self-collision");
            xurQBrNVaOwLYybmLsHMWARRRuUFGIKI.value = mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.SelfCollision;
            xurQBrNVaOwLYybmLsHMWARRRuUFGIKI.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei, "Changed Property in PhysicsActor");
                mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.SelfCollision = evt.newValue;
                EditorUtility.SetDirty(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei);
            });

            var nWOMUHcCJupSBoFhXyDHNaEkmkCrtweq = tpbNXZdozcyoYyzStFYLXmnTDTIOhZeb.Q<UnsignedIntegerField>("layer");
            nWOMUHcCJupSBoFhXyDHNaEkmkCrtweq.value = mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.Layer;
            nWOMUHcCJupSBoFhXyDHNaEkmkCrtweq.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei, "Changed Property in RigidbodyActor");
                mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.Layer = evt.newValue;
                EditorUtility.SetDirty(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei);
            });

            var PuPRvuCwMhNjfGlZVEnSmEuNEAoNrNSi = tpbNXZdozcyoYyzStFYLXmnTDTIOhZeb.Q<Toggle>("update-kinematic-particles");
            PuPRvuCwMhNjfGlZVEnSmEuNEAoNrNSi.value = mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.UpdateKinematicParticles;
            PuPRvuCwMhNjfGlZVEnSmEuNEAoNrNSi.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei, "Changed Property in RigidbodyActor");
                mACSOTdNdTxqtnUMPoXdRIECpmMoJOei.UpdateKinematicParticles = evt.newValue;
                EditorUtility.SetDirty(mACSOTdNdTxqtnUMPoXdRIECpmMoJOei);
            });

            var JPLowvzCyOfJbbgtkCVcQrpNoVMoEhhd = tpbNXZdozcyoYyzStFYLXmnTDTIOhZeb.Q<PropertyField>("on-break");
            JPLowvzCyOfJbbgtkCVcQrpNoVMoEhhd.BindProperty(serializedObject.FindFieldProperty("OnBreak"));

            return tpbNXZdozcyoYyzStFYLXmnTDTIOhZeb;
        }

        private void RBmzLAJnNGmdFoKilIRUagupToKzaPZc()
        {
            if (ToolManager.activeToolType == typeof(BasePhysicsActorEditorTool))
            {
                ToolManager.RestorePreviousPersistentTool();
            }
        }

        private void vZgGGWCFJcBbVfvRjOeAFWOfhdcJZDnz()
        {
            if (ToolManager.activeToolType != typeof(BasePhysicsActorEditorTool))
            {
                ToolManager.SetActiveTool<BasePhysicsActorEditorTool>();
            }
        }

        private void xkRPkUppjnFwdXMwRRTEwTwZJKILySKn()
        {
            RBmzLAJnNGmdFoKilIRUagupToKzaPZc();
        }
    }
}
