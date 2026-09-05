namespace MAGES.MeshDeformations.Editor
{
    using MAGES.Editor;
    using System.Collections.Generic;
    using System.IO;
    using Unity.Mathematics;
    using UnityEditor;
    using UnityEditor.Search;
    using UnityEngine;
    using UnityEngine.UIElements;

    /// <summary>
    /// A converter window for converting <see cref="TetrahedralMesh"/> to <see cref="SimulationMesh"/>.
    /// </summary>
    internal class SimulationMeshConverterWindow : EditorWindow
    {
        private VisualElement aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk;
        private TetrahedralMesh aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ;

        private rQUjczMGmAntdJzNogxmTCillUFaMlSN[] aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb = new rQUjczMGmAntdJzNogxmTCillUFaMlSN[0];

        /// <summary>
        /// Shows the window.
        /// </summary>
        [MenuItem("MAGES/Mesh Deformations/Simulation Mesh Converter Tool", priority = MAGESMenuItemPriorities.MeshDeformations + 1)]
        public static void ShowWindow()
        {
            SimulationMeshConverterWindow WuvPcFRLKNSmpzEnGzPzxknVohEakIwM = GetWindow<SimulationMeshConverterWindow>();
            WuvPcFRLKNSmpzEnGzPzxknVohEakIwM.titleContent = new GUIContent("Simulation Mesh Converter Tool");
            WuvPcFRLKNSmpzEnGzPzxknVohEakIwM.minSize = new Vector2(400, 400);
        }

        private void UsoVfYZffzGFvGKnRacVesWoZVsKguiK()
        {
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk = new ScrollView();
            rootVisualElement.Add(aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk);
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.style.paddingLeft = 10;
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.style.paddingRight = 10;
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.style.paddingTop = 10;
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.style.paddingBottom = 10;

            Label ocVLCOvZoJDbXRNHuzAFxbbdgsGkJYSN = new Label("Simulation Mesh Converter Tool");
            ocVLCOvZoJDbXRNHuzAFxbbdgsGkJYSN.style.fontSize = 20;
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Add(ocVLCOvZoJDbXRNHuzAFxbbdgsGkJYSN);

            ObjectField uptkAErcTFKUftmoweSFLSWwQhKlzYdR = new ObjectField();
            uptkAErcTFKUftmoweSFLSWwQhKlzYdR.style.marginTop = 10;
            uptkAErcTFKUftmoweSFLSWwQhKlzYdR.name = "source-tet-mesh";
            uptkAErcTFKUftmoweSFLSWwQhKlzYdR.label = "Source Tetrahedral Mesh";
            uptkAErcTFKUftmoweSFLSWwQhKlzYdR.objectType = typeof(TetrahedralMesh);
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Add(uptkAErcTFKUftmoweSFLSWwQhKlzYdR);

            uptkAErcTFKUftmoweSFLSWwQhKlzYdR.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ = evt.newValue as TetrahedralMesh;
                nAVGcCFxMgMuSethdTCYjWhVNJVQcSfA(aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ != null);
                KHBqFTZoYchRubwIxCvSVKXuIVQEFhbB(aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Q<VisualElement>("sub-mesh-options"));
            });

            Label ofouerseljrFZnQimRPqbHBWUZyLGLLX = new Label("Particles");
            ofouerseljrFZnQimRPqbHBWUZyLGLLX.style.marginTop = 10;
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Add(ofouerseljrFZnQimRPqbHBWUZyLGLLX);
            FloatField ePCqMbtVXimdRnkAonCmljuLcUesFHrG = new FloatField();
            ePCqMbtVXimdRnkAonCmljuLcUesFHrG.name = "object-mass";
            ePCqMbtVXimdRnkAonCmljuLcUesFHrG.label = "Mass";
            ePCqMbtVXimdRnkAonCmljuLcUesFHrG.value = 1.0f;
            ePCqMbtVXimdRnkAonCmljuLcUesFHrG.RegisterValueChangedCallback((evt) =>
            {
                cKoKFfWiRbEIvQnuIzxosMAaadXLdEtY();
            });

            FloatField PiFSQcvdfVXDignlzXKAwNzzLgWyOnaT = new FloatField();
            PiFSQcvdfVXDignlzXKAwNzzLgWyOnaT.name = "particle-mass";
            PiFSQcvdfVXDignlzXKAwNzzLgWyOnaT.label = "Particle Mass";
            PiFSQcvdfVXDignlzXKAwNzzLgWyOnaT.value = 0f;
            PiFSQcvdfVXDignlzXKAwNzzLgWyOnaT.RegisterValueChangedCallback((evt) =>
            {
                FxEnRsJlpcLGFXyndnRxJFmyhwUCsEEX();
            });

            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Add(ePCqMbtVXimdRnkAonCmljuLcUesFHrG);
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Add(PiFSQcvdfVXDignlzXKAwNzzLgWyOnaT);

            Label OPPWMIPXWZINEeMuvMKElMcABbEvtfAG = new Label("Distance Constraints");
            OPPWMIPXWZINEeMuvMKElMcABbEvtfAG.style.marginTop = 10;
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Add(OPPWMIPXWZINEeMuvMKElMcABbEvtfAG);
            EnumField aScZotEtHYqMeooPoBBRZVJplbVDCXXI = new EnumField();
            aScZotEtHYqMeooPoBBRZVJplbVDCXXI.name = "dc-generation-options";
            aScZotEtHYqMeooPoBBRZVJplbVDCXXI.label = "Generate:";
            aScZotEtHYqMeooPoBBRZVJplbVDCXXI.Init(DistanceConstraintsGenerationOptions.OnePerEdge);
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Add(aScZotEtHYqMeooPoBBRZVJplbVDCXXI);

            Label hYADZDUNcfhSWhqXMLttWRVYKjFgiVKR = new Label("Volume Constraints");
            hYADZDUNcfhSWhqXMLttWRVYKjFgiVKR.style.marginTop = 10;
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Add(hYADZDUNcfhSWhqXMLttWRVYKjFgiVKR);
            EnumField KusMGWaaAPrZkOJWLfGgzNcwUcyZSSbD = new EnumField();
            KusMGWaaAPrZkOJWLfGgzNcwUcyZSSbD.name = "vc-generation-options";
            KusMGWaaAPrZkOJWLfGgzNcwUcyZSSbD.label = "Generate:";
            KusMGWaaAPrZkOJWLfGgzNcwUcyZSSbD.Init(VolumeConstraintsGenerationOptions.OnePerTetrahedron);
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Add(KusMGWaaAPrZkOJWLfGgzNcwUcyZSSbD);

            Label ApnlQDNWoVNVMfmYKDEATgVpVWxeOGMN = new Label("Shape Matching Constraints");
            ApnlQDNWoVNVMfmYKDEATgVpVWxeOGMN.style.marginTop = 10;
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Add(ApnlQDNWoVNVMfmYKDEATgVpVWxeOGMN);
            EnumField mGpWPWNqXJPivwnBmumKieSMKwTHiRvq = new EnumField();
            mGpWPWNqXJPivwnBmumKieSMKwTHiRvq.name = "smc-generation-options";
            mGpWPWNqXJPivwnBmumKieSMKwTHiRvq.label = "Generate:";
            mGpWPWNqXJPivwnBmumKieSMKwTHiRvq.Init(ShapeMatchingConstraintsGenerationOptions.DontGenerate);
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Add(mGpWPWNqXJPivwnBmumKieSMKwTHiRvq);

            VisualElement qQNsBosVnYdflcwLbvWmotLcKlcSjfta = new VisualElement();
            qQNsBosVnYdflcwLbvWmotLcKlcSjfta.name = "sub-mesh-options";
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Add(qQNsBosVnYdflcwLbvWmotLcKlcSjfta);

            Button TEcShXlHDKackbEKJuliLlXozqDkrYpS = new Button();
            TEcShXlHDKackbEKJuliLlXozqDkrYpS.style.marginTop = 20;
            TEcShXlHDKackbEKJuliLlXozqDkrYpS.name = "create-sm";
            TEcShXlHDKackbEKJuliLlXozqDkrYpS.text = "Create Simulation Mesh";
            TEcShXlHDKackbEKJuliLlXozqDkrYpS.RegisterCallback<ClickEvent>((evt) =>
            {
                XALlhjvnphCcvyYwCaxZMhRYgxhYzuCZ();
            });
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Add(TEcShXlHDKackbEKJuliLlXozqDkrYpS);

            nAVGcCFxMgMuSethdTCYjWhVNJVQcSfA(aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ != null);
            KHBqFTZoYchRubwIxCvSVKXuIVQEFhbB(aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Q<VisualElement>("sub-mesh-options"));
        }

        private void nAVGcCFxMgMuSethdTCYjWhVNJVQcSfA(bool enabled)
        {
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Q<Button>("create-sm").SetEnabled(enabled);
            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Q<FloatField>("particle-mass").SetEnabled(enabled);
            if (enabled)
            {
                cKoKFfWiRbEIvQnuIzxosMAaadXLdEtY();
            }
        }

        private void cKoKFfWiRbEIvQnuIzxosMAaadXLdEtY()
        {
            if (aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ == null)
            {
                return;
            }

            if (aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Vertices.Length > 0)
            {
                aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Q<FloatField>("particle-mass").value = aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Q<FloatField>("object-mass").value / aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Vertices.Length;
            }
            else
            {
                aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Q<FloatField>("particle-mass").value = 0f;
            }
        }

        private void FxEnRsJlpcLGFXyndnRxJFmyhwUCsEEX()
        {
            if (aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ == null)
            {
                return;
            }

            aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Q<FloatField>("object-mass").value = aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Q<FloatField>("particle-mass").value * aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Vertices.Length;
        }

        private void KHBqFTZoYchRubwIxCvSVKXuIVQEFhbB(VisualElement root)
        {
            root?.Clear();
            aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb = new rQUjczMGmAntdJzNogxmTCillUFaMlSN[0];

            if (aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ == null)
            {
                return;
            }

            var EvzjgdMyzQsMyDVuTFpezAMqVwLgcKtJ = new Label("Per Sub Mesh Options");
            EvzjgdMyzQsMyDVuTFpezAMqVwLgcKtJ.style.marginTop = 10;
            root.Add(EvzjgdMyzQsMyDVuTFpezAMqVwLgcKtJ);

            aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb = new rQUjczMGmAntdJzNogxmTCillUFaMlSN[aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.SubMeshCount];

            for (int asFRApuiajorndWVZOlmsHMfzyXWdNRM = 0; asFRApuiajorndWVZOlmsHMfzyXWdNRM < aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb.Length; asFRApuiajorndWVZOlmsHMfzyXWdNRM++)
            {
                var VRNgMUJphyFUPbqZLtIuQySJuXSlTFNm = new Label($"Sub Mesh {asFRApuiajorndWVZOlmsHMfzyXWdNRM}");
                VRNgMUJphyFUPbqZLtIuQySJuXSlTFNm.style.marginTop = 5;
                VRNgMUJphyFUPbqZLtIuQySJuXSlTFNm.style.marginLeft = 5;
                root.Add(VRNgMUJphyFUPbqZLtIuQySJuXSlTFNm);

                VisualElement cxrgWwSJvnwaTnfXWstKvMKYeDxsQBPi = new VisualElement();
                cxrgWwSJvnwaTnfXWstKvMKYeDxsQBPi.style.marginLeft = 10;

                int ltPRMkimAPAOzwajCMggivoMYGRrxrIw = asFRApuiajorndWVZOlmsHMfzyXWdNRM;

                Toggle yrySAFDABKclNQQIJEKgXMpsgayYhlqv = new Toggle();
                yrySAFDABKclNQQIJEKgXMpsgayYhlqv.name = $"tearable-{asFRApuiajorndWVZOlmsHMfzyXWdNRM}";
                yrySAFDABKclNQQIJEKgXMpsgayYhlqv.label = "Tearable";
                yrySAFDABKclNQQIJEKgXMpsgayYhlqv.value = false;
                yrySAFDABKclNQQIJEKgXMpsgayYhlqv.RegisterValueChangedCallback((evt) =>
                {
                    aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[ltPRMkimAPAOzwajCMggivoMYGRrxrIw].Tearable = evt.newValue;
                });
                aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[asFRApuiajorndWVZOlmsHMfzyXWdNRM].Tearable = yrySAFDABKclNQQIJEKgXMpsgayYhlqv.value;
                cxrgWwSJvnwaTnfXWstKvMKYeDxsQBPi.Add(yrySAFDABKclNQQIJEKgXMpsgayYhlqv);

                FloatField rPZBBOVuvjMqeuOrcmQksQYkiRHCAkKb = new FloatField();
                rPZBBOVuvjMqeuOrcmQksQYkiRHCAkKb.name = $"particle-damping-{asFRApuiajorndWVZOlmsHMfzyXWdNRM}";
                rPZBBOVuvjMqeuOrcmQksQYkiRHCAkKb.label = "Particle Damping";
                rPZBBOVuvjMqeuOrcmQksQYkiRHCAkKb.value = 0.01f;
                rPZBBOVuvjMqeuOrcmQksQYkiRHCAkKb.RegisterCallback<ChangeEvent<float>>((evt) =>
                {
                    aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[ltPRMkimAPAOzwajCMggivoMYGRrxrIw].ParticleDamping = math.clamp(evt.newValue, 0f, 1.0f);
                });
                rPZBBOVuvjMqeuOrcmQksQYkiRHCAkKb.RegisterCallback<FocusOutEvent>((evt) =>
                {
                    rPZBBOVuvjMqeuOrcmQksQYkiRHCAkKb.SetValueWithoutNotify(aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[ltPRMkimAPAOzwajCMggivoMYGRrxrIw].ParticleDamping);
                });
                aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[asFRApuiajorndWVZOlmsHMfzyXWdNRM].ParticleDamping = rPZBBOVuvjMqeuOrcmQksQYkiRHCAkKb.value;
                cxrgWwSJvnwaTnfXWstKvMKYeDxsQBPi.Add(rPZBBOVuvjMqeuOrcmQksQYkiRHCAkKb);

                FloatField SDNBvjfTOMGzcLWijsgjrrfQZRQQbFAd = new FloatField();
                SDNBvjfTOMGzcLWijsgjrrfQZRQQbFAd.name = $"dc-stiffness-{asFRApuiajorndWVZOlmsHMfzyXWdNRM}";
                SDNBvjfTOMGzcLWijsgjrrfQZRQQbFAd.label = "Distance Constraints Stiffness";
                SDNBvjfTOMGzcLWijsgjrrfQZRQQbFAd.value = 10000f;
                SDNBvjfTOMGzcLWijsgjrrfQZRQQbFAd.RegisterCallback<ChangeEvent<float>>((evt) =>
                {
                    aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[ltPRMkimAPAOzwajCMggivoMYGRrxrIw].DistanceConstraintsInverseStiffness = 1.0f / Mathf.Max(evt.newValue, 0.0000001f);
                });
                aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[asFRApuiajorndWVZOlmsHMfzyXWdNRM].DistanceConstraintsInverseStiffness = 1.0f / SDNBvjfTOMGzcLWijsgjrrfQZRQQbFAd.value;
                cxrgWwSJvnwaTnfXWstKvMKYeDxsQBPi.Add(SDNBvjfTOMGzcLWijsgjrrfQZRQQbFAd);

                FloatField OGDkrRkUJLyMYIkKFfDACgucfbfXkkrI = new FloatField();
                OGDkrRkUJLyMYIkKFfDACgucfbfXkkrI.name = $"vc-stiffness-{asFRApuiajorndWVZOlmsHMfzyXWdNRM}";
                OGDkrRkUJLyMYIkKFfDACgucfbfXkkrI.label = "Volume Constraints Stiffness";
                OGDkrRkUJLyMYIkKFfDACgucfbfXkkrI.value = 10000f;
                OGDkrRkUJLyMYIkKFfDACgucfbfXkkrI.RegisterCallback<ChangeEvent<float>>((evt) =>
                {
                    aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[ltPRMkimAPAOzwajCMggivoMYGRrxrIw].VolumeConstraintsInverseStiffness = 1.0f / Mathf.Max(evt.newValue, 0.0000001f);
                });
                aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[asFRApuiajorndWVZOlmsHMfzyXWdNRM].VolumeConstraintsInverseStiffness = 1.0f / OGDkrRkUJLyMYIkKFfDACgucfbfXkkrI.value;
                cxrgWwSJvnwaTnfXWstKvMKYeDxsQBPi.Add(OGDkrRkUJLyMYIkKFfDACgucfbfXkkrI);

                FloatField FFWxxmbdMJISGsFrdPGllQcuPGVtpCcd = new FloatField();
                FFWxxmbdMJISGsFrdPGllQcuPGVtpCcd.name = $"smc-stiffness-{asFRApuiajorndWVZOlmsHMfzyXWdNRM}";
                FFWxxmbdMJISGsFrdPGllQcuPGVtpCcd.label = "Shape Matching Constraints Stiffness";
                FFWxxmbdMJISGsFrdPGllQcuPGVtpCcd.tooltip = "Stiffness of shape matching constraints. Range [0, 1].";
                FFWxxmbdMJISGsFrdPGllQcuPGVtpCcd.value = 1f;
                FFWxxmbdMJISGsFrdPGllQcuPGVtpCcd.RegisterCallback<ChangeEvent<float>>((evt) =>
                {
                    aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[ltPRMkimAPAOzwajCMggivoMYGRrxrIw].ShapeMatchingConstraintsStiffness = math.clamp(evt.newValue, 0, 1.0f);
                });
                aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[asFRApuiajorndWVZOlmsHMfzyXWdNRM].ShapeMatchingConstraintsStiffness = FFWxxmbdMJISGsFrdPGllQcuPGVtpCcd.value;
                cxrgWwSJvnwaTnfXWstKvMKYeDxsQBPi.Add(FFWxxmbdMJISGsFrdPGllQcuPGVtpCcd);

                FloatField ppSGZtfcJHZXXmuXZOAEPtkucAxJuoZd = new FloatField();
                ppSGZtfcJHZXXmuXZOAEPtkucAxJuoZd.name = $"static-friction-{asFRApuiajorndWVZOlmsHMfzyXWdNRM}";
                ppSGZtfcJHZXXmuXZOAEPtkucAxJuoZd.label = "Static Friction";
                ppSGZtfcJHZXXmuXZOAEPtkucAxJuoZd.value = 0.6f;
                ppSGZtfcJHZXXmuXZOAEPtkucAxJuoZd.RegisterCallback<ChangeEvent<float>>((evt) =>
                {
                    aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[ltPRMkimAPAOzwajCMggivoMYGRrxrIw].StaticFriction = evt.newValue;
                });
                aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[asFRApuiajorndWVZOlmsHMfzyXWdNRM].StaticFriction = ppSGZtfcJHZXXmuXZOAEPtkucAxJuoZd.value;
                cxrgWwSJvnwaTnfXWstKvMKYeDxsQBPi.Add(ppSGZtfcJHZXXmuXZOAEPtkucAxJuoZd);

                FloatField zZlJKkdUsgmVZCUWRPVXLzrnNhPfZSRK = new FloatField();
                zZlJKkdUsgmVZCUWRPVXLzrnNhPfZSRK.name = $"kinetic-friction-{asFRApuiajorndWVZOlmsHMfzyXWdNRM}";
                zZlJKkdUsgmVZCUWRPVXLzrnNhPfZSRK.label = "kinetic Friction";
                zZlJKkdUsgmVZCUWRPVXLzrnNhPfZSRK.value = 0.6f;
                zZlJKkdUsgmVZCUWRPVXLzrnNhPfZSRK.RegisterCallback<ChangeEvent<float>>((evt) =>
                {
                    aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[ltPRMkimAPAOzwajCMggivoMYGRrxrIw].KineticFriction = evt.newValue;
                });
                aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[asFRApuiajorndWVZOlmsHMfzyXWdNRM].KineticFriction = zZlJKkdUsgmVZCUWRPVXLzrnNhPfZSRK.value;
                cxrgWwSJvnwaTnfXWstKvMKYeDxsQBPi.Add(zZlJKkdUsgmVZCUWRPVXLzrnNhPfZSRK);

                root.Add(cxrgWwSJvnwaTnfXWstKvMKYeDxsQBPi);
            }
        }

        private void XALlhjvnphCcvyYwCaxZMhRYgxhYzuCZ()
        {
            var UzZXXFuzWNxNlsNSQQJyPvPrGklaxyAH = ScriptableObject.CreateInstance<SimulationMesh>();

            PhysicsMaterial[] ngZpoHzfuqAxdNGlZsoRMBMaHavkonsY = new PhysicsMaterial[aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.SubMeshCount];
            List<Particle> GkclfpDgFBPwnBRNPfGVLlnJkJqaAAZe = new List<Particle>();
            List<DistanceConstraint> OMBZROpazGyIJbmcLaMOeIiHOBpuApEF = new List<DistanceConstraint>();
            List<VolumeConstraint> OKRtoBlIFEmmOqzQwiqbZyUDZlBWJiCc = new List<VolumeConstraint>();
            List<ShapeMatchingConstraint> JoIbKooAuUvKMVJSrSNsMEDMNIbFOXsG = new List<ShapeMatchingConstraint>();

            for (int OXFsYYtAElMvNDwjXCiCYqunyvBJGNoi = 0; OXFsYYtAElMvNDwjXCiCYqunyvBJGNoi < aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.SubMeshCount; OXFsYYtAElMvNDwjXCiCYqunyvBJGNoi++)
            {
                ngZpoHzfuqAxdNGlZsoRMBMaHavkonsY[OXFsYYtAElMvNDwjXCiCYqunyvBJGNoi] = new PhysicsMaterial
                {
                    ParticleDamping = aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[OXFsYYtAElMvNDwjXCiCYqunyvBJGNoi].ParticleDamping,
                    DistanceConstraintInverseStiffness = aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[OXFsYYtAElMvNDwjXCiCYqunyvBJGNoi].DistanceConstraintsInverseStiffness,
                    VolumeConstraintInverseStiffness = aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[OXFsYYtAElMvNDwjXCiCYqunyvBJGNoi].VolumeConstraintsInverseStiffness,
                    ShapeMatchingConstraintStiffness = aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[OXFsYYtAElMvNDwjXCiCYqunyvBJGNoi].ShapeMatchingConstraintsStiffness,
                    StaticFriction = aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[OXFsYYtAElMvNDwjXCiCYqunyvBJGNoi].StaticFriction,
                    KineticFriction = aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[OXFsYYtAElMvNDwjXCiCYqunyvBJGNoi].KineticFriction,
                };
            }

            
            for (int sClLVPmcYnLuTWeoQIfWBMjTZQXscoIu = 0; sClLVPmcYnLuTWeoQIfWBMjTZQXscoIu < aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Vertices.Length; sClLVPmcYnLuTWeoQIfWBMjTZQXscoIu++)
            {
                Particle vGfSqSyJarmDDXCAyEedsAFMRLQZIbKq = new Particle
                {
                    PhysicsMaterialIndex = -1,
                    Enabled = true,
                    InverseMass = 1.0f / Mathf.Max(aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Q<FloatField>("particle-mass").value, 0.0000001f),
                    Position = aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Vertices[sClLVPmcYnLuTWeoQIfWBMjTZQXscoIu],
                    Velocity = float3.zero,
                };

                GkclfpDgFBPwnBRNPfGVLlnJkJqaAAZe.Add(vGfSqSyJarmDDXCAyEedsAFMRLQZIbKq);
            }

            
            for (int yKyntEpkRcSXXekcAMphjHWYrMBiUccE = 0; yKyntEpkRcSXXekcAMphjHWYrMBiUccE < aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra.Length; yKyntEpkRcSXXekcAMphjHWYrMBiUccE += 4)
            {
                
                int rvueOvEhnAqnNvWgBugGeaICXNCzVhrr = -1;
                for (int djULDYxeTJoNBZicUNrNuZlyRZctNmMu = 0; djULDYxeTJoNBZicUNrNuZlyRZctNmMu < aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.SubMeshCount; djULDYxeTJoNBZicUNrNuZlyRZctNmMu++)
                {
                    TetrahedralSubMeshDescriptor irQYgUAZSdhMLLaWreGDSGcAEhJwYJwT = aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.GetSubMesh(djULDYxeTJoNBZicUNrNuZlyRZctNmMu);

                    if (irQYgUAZSdhMLLaWreGDSGcAEhJwYJwT.IndexStart <= yKyntEpkRcSXXekcAMphjHWYrMBiUccE && yKyntEpkRcSXXekcAMphjHWYrMBiUccE < irQYgUAZSdhMLLaWreGDSGcAEhJwYJwT.IndexStart + irQYgUAZSdhMLLaWreGDSGcAEhJwYJwT.IndexCount)
                    {
                        rvueOvEhnAqnNvWgBugGeaICXNCzVhrr = djULDYxeTJoNBZicUNrNuZlyRZctNmMu;
                        break;
                    }
                }

                Particle lJjMHuvaDTxlhSBNQdoTLnXVvcrmyXON = GkclfpDgFBPwnBRNPfGVLlnJkJqaAAZe[aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra[yKyntEpkRcSXXekcAMphjHWYrMBiUccE]];
                Particle nuXqlGgZmcePrJUGviYTXKMXxYANHZss = GkclfpDgFBPwnBRNPfGVLlnJkJqaAAZe[aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra[yKyntEpkRcSXXekcAMphjHWYrMBiUccE + 1]];
                Particle YEvfiFwEFbRGARXCsilLxjCPtFhXssba = GkclfpDgFBPwnBRNPfGVLlnJkJqaAAZe[aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra[yKyntEpkRcSXXekcAMphjHWYrMBiUccE + 2]];
                Particle lgvAnFsomMnMuIfyQHpcDzfsoLocNdKa = GkclfpDgFBPwnBRNPfGVLlnJkJqaAAZe[aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra[yKyntEpkRcSXXekcAMphjHWYrMBiUccE + 3]];

                lJjMHuvaDTxlhSBNQdoTLnXVvcrmyXON.PhysicsMaterialIndex = lJjMHuvaDTxlhSBNQdoTLnXVvcrmyXON.PhysicsMaterialIndex >= 0 ? math.min(lJjMHuvaDTxlhSBNQdoTLnXVvcrmyXON.PhysicsMaterialIndex, rvueOvEhnAqnNvWgBugGeaICXNCzVhrr) : rvueOvEhnAqnNvWgBugGeaICXNCzVhrr;
                nuXqlGgZmcePrJUGviYTXKMXxYANHZss.PhysicsMaterialIndex = nuXqlGgZmcePrJUGviYTXKMXxYANHZss.PhysicsMaterialIndex >= 0 ? math.min(nuXqlGgZmcePrJUGviYTXKMXxYANHZss.PhysicsMaterialIndex, rvueOvEhnAqnNvWgBugGeaICXNCzVhrr) : rvueOvEhnAqnNvWgBugGeaICXNCzVhrr;
                YEvfiFwEFbRGARXCsilLxjCPtFhXssba.PhysicsMaterialIndex = YEvfiFwEFbRGARXCsilLxjCPtFhXssba.PhysicsMaterialIndex >= 0 ? math.min(YEvfiFwEFbRGARXCsilLxjCPtFhXssba.PhysicsMaterialIndex, rvueOvEhnAqnNvWgBugGeaICXNCzVhrr) : rvueOvEhnAqnNvWgBugGeaICXNCzVhrr;
                lgvAnFsomMnMuIfyQHpcDzfsoLocNdKa.PhysicsMaterialIndex = lgvAnFsomMnMuIfyQHpcDzfsoLocNdKa.PhysicsMaterialIndex >= 0 ? math.min(lgvAnFsomMnMuIfyQHpcDzfsoLocNdKa.PhysicsMaterialIndex, rvueOvEhnAqnNvWgBugGeaICXNCzVhrr) : rvueOvEhnAqnNvWgBugGeaICXNCzVhrr;
                lJjMHuvaDTxlhSBNQdoTLnXVvcrmyXON.Breakable = aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[lJjMHuvaDTxlhSBNQdoTLnXVvcrmyXON.PhysicsMaterialIndex].Tearable;
                nuXqlGgZmcePrJUGviYTXKMXxYANHZss.Breakable = aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[lJjMHuvaDTxlhSBNQdoTLnXVvcrmyXON.PhysicsMaterialIndex].Tearable;
                YEvfiFwEFbRGARXCsilLxjCPtFhXssba.Breakable = aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[lJjMHuvaDTxlhSBNQdoTLnXVvcrmyXON.PhysicsMaterialIndex].Tearable;
                lgvAnFsomMnMuIfyQHpcDzfsoLocNdKa.Breakable = aYYyaisvyKmkOfhkgUWEihJZGLYIHxQb[lJjMHuvaDTxlhSBNQdoTLnXVvcrmyXON.PhysicsMaterialIndex].Tearable;

                GkclfpDgFBPwnBRNPfGVLlnJkJqaAAZe[aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra[yKyntEpkRcSXXekcAMphjHWYrMBiUccE]] = lJjMHuvaDTxlhSBNQdoTLnXVvcrmyXON;
                GkclfpDgFBPwnBRNPfGVLlnJkJqaAAZe[aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra[yKyntEpkRcSXXekcAMphjHWYrMBiUccE + 1]] = nuXqlGgZmcePrJUGviYTXKMXxYANHZss;
                GkclfpDgFBPwnBRNPfGVLlnJkJqaAAZe[aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra[yKyntEpkRcSXXekcAMphjHWYrMBiUccE + 2]] = YEvfiFwEFbRGARXCsilLxjCPtFhXssba;
                GkclfpDgFBPwnBRNPfGVLlnJkJqaAAZe[aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra[yKyntEpkRcSXXekcAMphjHWYrMBiUccE + 3]] = lgvAnFsomMnMuIfyQHpcDzfsoLocNdKa;
            }

            
            for (int oucBbyzolJKdjGSexkJttlfDuLpQWzJs = 0; oucBbyzolJKdjGSexkJttlfDuLpQWzJs < aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Vertices.Length; oucBbyzolJKdjGSexkJttlfDuLpQWzJs++)
            {
                Particle krZEZCEvDaDcaaDguIIiYxfbLIlBSidj = GkclfpDgFBPwnBRNPfGVLlnJkJqaAAZe[oucBbyzolJKdjGSexkJttlfDuLpQWzJs];
                if (krZEZCEvDaDcaaDguIIiYxfbLIlBSidj.PhysicsMaterialIndex < 0)
                {
                    krZEZCEvDaDcaaDguIIiYxfbLIlBSidj.PhysicsMaterialIndex = 0;
                }

                GkclfpDgFBPwnBRNPfGVLlnJkJqaAAZe[oucBbyzolJKdjGSexkJttlfDuLpQWzJs] = krZEZCEvDaDcaaDguIIiYxfbLIlBSidj;
            }

            
            switch (aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Q<EnumField>("dc-generation-options").value)
            {
                case DistanceConstraintsGenerationOptions.OnePerEdge:

                    Dictionary<(int, int), int> jQPPQGGYtfFUXMWrdGxsAWxXVTjQheIO = new Dictionary<(int, int), int>();
                    for (int tvYXbrJPIuFeudrWwAROlVsOVadVmGrY = 0; tvYXbrJPIuFeudrWwAROlVsOVadVmGrY < aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra.Length; tvYXbrJPIuFeudrWwAROlVsOVadVmGrY += 4)
                    {
                        for (int SLpWDjtPnzMfkItiRPKDjFtQEogmBdbq = 0; SLpWDjtPnzMfkItiRPKDjFtQEogmBdbq < 4; SLpWDjtPnzMfkItiRPKDjFtQEogmBdbq++)
                        {
                            for (int RmhBRvwmhyZTWafXUOkXUKCTcWtLITyi = SLpWDjtPnzMfkItiRPKDjFtQEogmBdbq + 1; RmhBRvwmhyZTWafXUOkXUKCTcWtLITyi < 4; RmhBRvwmhyZTWafXUOkXUKCTcWtLITyi++)
                            {
                                var BCTynpzfBrGSNvNLgDDvAYOZnjLSAhqq = (aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra[tvYXbrJPIuFeudrWwAROlVsOVadVmGrY + SLpWDjtPnzMfkItiRPKDjFtQEogmBdbq], aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra[tvYXbrJPIuFeudrWwAROlVsOVadVmGrY + RmhBRvwmhyZTWafXUOkXUKCTcWtLITyi]);
                                if (BCTynpzfBrGSNvNLgDDvAYOZnjLSAhqq.Item1 > BCTynpzfBrGSNvNLgDDvAYOZnjLSAhqq.Item2)
                                {
                                    BCTynpzfBrGSNvNLgDDvAYOZnjLSAhqq = (BCTynpzfBrGSNvNLgDDvAYOZnjLSAhqq.Item2, BCTynpzfBrGSNvNLgDDvAYOZnjLSAhqq.Item1);
                                }

                                if (!jQPPQGGYtfFUXMWrdGxsAWxXVTjQheIO.ContainsKey(BCTynpzfBrGSNvNLgDDvAYOZnjLSAhqq))
                                {
                                    
                                    int aLBpihocWYIKUHeholeuduNvnJTkglTh = -1;
                                    for (int pTNNavhfKbNQZcljcLSuxSgzLstiOJjB = 0; pTNNavhfKbNQZcljcLSuxSgzLstiOJjB < aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.SubMeshCount; pTNNavhfKbNQZcljcLSuxSgzLstiOJjB++)
                                    {
                                        TetrahedralSubMeshDescriptor mjmHuVupPNaMuHHkgyvMxVJbuIINFjBt = aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.GetSubMesh(pTNNavhfKbNQZcljcLSuxSgzLstiOJjB);

                                        if (mjmHuVupPNaMuHHkgyvMxVJbuIINFjBt.IndexStart <= tvYXbrJPIuFeudrWwAROlVsOVadVmGrY && tvYXbrJPIuFeudrWwAROlVsOVadVmGrY < mjmHuVupPNaMuHHkgyvMxVJbuIINFjBt.IndexStart + mjmHuVupPNaMuHHkgyvMxVJbuIINFjBt.IndexCount)
                                        {
                                            aLBpihocWYIKUHeholeuduNvnJTkglTh = pTNNavhfKbNQZcljcLSuxSgzLstiOJjB;
                                            break;
                                        }
                                    }

                                    jQPPQGGYtfFUXMWrdGxsAWxXVTjQheIO[BCTynpzfBrGSNvNLgDDvAYOZnjLSAhqq] = aLBpihocWYIKUHeholeuduNvnJTkglTh;
                                }
                            }
                        }
                    }

                    
                    foreach (var ue in jQPPQGGYtfFUXMWrdGxsAWxXVTjQheIO.Keys)
                    {
                        var QkUBpVCuuGTzqzHmOZilPbZHKwjOlJtr = jQPPQGGYtfFUXMWrdGxsAWxXVTjQheIO[ue];

                        DistanceConstraint beKOLEeGpkFoKzfkFoOpmxlxcnyZsdpd = new DistanceConstraint(ue.Item1, ue.Item2);

                        beKOLEeGpkFoKzfkFoOpmxlxcnyZsdpd.PhysicsMaterialIndex = QkUBpVCuuGTzqzHmOZilPbZHKwjOlJtr;

                        OMBZROpazGyIJbmcLaMOeIiHOBpuApEF.Add(beKOLEeGpkFoKzfkFoOpmxlxcnyZsdpd);
                    }

                    break;
                default:
                    break;
            }

            
            switch (aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Q<EnumField>("vc-generation-options").value)
            {
                case VolumeConstraintsGenerationOptions.OnePerTetrahedron:

                    for (int bSaWvSGsCDDZESJiICHxqmnyrVNMaPpZ = 0; bSaWvSGsCDDZESJiICHxqmnyrVNMaPpZ < aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra.Length; bSaWvSGsCDDZESJiICHxqmnyrVNMaPpZ += 4)
                    {
                        
                        int tZqNKYLCtVvopFyLSWyrnFJSRlwjUuRf = -1;
                        for (int ketZhherkzkMOwmUwATlMuizTTNokuVG = 0; ketZhherkzkMOwmUwATlMuizTTNokuVG < aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.SubMeshCount; ketZhherkzkMOwmUwATlMuizTTNokuVG++)
                        {
                            TetrahedralSubMeshDescriptor FBxIDRPniPQgXTQSfueZzKwpUPomSMIM = aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.GetSubMesh(ketZhherkzkMOwmUwATlMuizTTNokuVG);

                            if (FBxIDRPniPQgXTQSfueZzKwpUPomSMIM.IndexStart <= bSaWvSGsCDDZESJiICHxqmnyrVNMaPpZ && bSaWvSGsCDDZESJiICHxqmnyrVNMaPpZ < FBxIDRPniPQgXTQSfueZzKwpUPomSMIM.IndexStart + FBxIDRPniPQgXTQSfueZzKwpUPomSMIM.IndexCount)
                            {
                                tZqNKYLCtVvopFyLSWyrnFJSRlwjUuRf = ketZhherkzkMOwmUwATlMuizTTNokuVG;
                                break;
                            }
                        }

                        VolumeConstraint YJlSrIDasRVVuFkCaEHgyPFNLLTcFnah = new VolumeConstraint(aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra[bSaWvSGsCDDZESJiICHxqmnyrVNMaPpZ], aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra[bSaWvSGsCDDZESJiICHxqmnyrVNMaPpZ + 1], aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra[bSaWvSGsCDDZESJiICHxqmnyrVNMaPpZ + 2], aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Tetrahedra[bSaWvSGsCDDZESJiICHxqmnyrVNMaPpZ + 3]);
                        YJlSrIDasRVVuFkCaEHgyPFNLLTcFnah.PhysicsMaterialIndex = tZqNKYLCtVvopFyLSWyrnFJSRlwjUuRf;
                        OKRtoBlIFEmmOqzQwiqbZyUDZlBWJiCc.Add(YJlSrIDasRVVuFkCaEHgyPFNLLTcFnah);
                    }

                    break;
                default:
                    break;
            }

            
            switch (aWIzsEDsISYGYrvUQeuvylWRUTBGrlAk.Q<EnumField>("smc-generation-options").value)
            {
                case ShapeMatchingConstraintsGenerationOptions.OnePerParticle:

                    for (int PmsJAydsAFKxaPoaRaAMooOMtXeBWWVX = 0; PmsJAydsAFKxaPoaRaAMooOMtXeBWWVX < aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.Vertices.Length; PmsJAydsAFKxaPoaRaAMooOMtXeBWWVX++)
                    {
                        ShapeMatchingConstraint EIwoIRJObwZrsRnYqqshFFaWqhgEHqec = new ShapeMatchingConstraint
                        {
                            Enabled = true,
                            Breakable = false,

                            Particle = PmsJAydsAFKxaPoaRaAMooOMtXeBWWVX,
                            PhysicsMaterialIndex = GkclfpDgFBPwnBRNPfGVLlnJkJqaAAZe[PmsJAydsAFKxaPoaRaAMooOMtXeBWWVX].PhysicsMaterialIndex,
                        };

                        JoIbKooAuUvKMVJSrSNsMEDMNIbFOXsG.Add(EIwoIRJObwZrsRnYqqshFFaWqhgEHqec);
                    }

                    break;

                default:
                    break;
            }

            UzZXXFuzWNxNlsNSQQJyPvPrGklaxyAH.Particles = GkclfpDgFBPwnBRNPfGVLlnJkJqaAAZe.ToArray();
            UzZXXFuzWNxNlsNSQQJyPvPrGklaxyAH.DistanceConstraints = OMBZROpazGyIJbmcLaMOeIiHOBpuApEF.ToArray();
            UzZXXFuzWNxNlsNSQQJyPvPrGklaxyAH.VolumeConstraints = OKRtoBlIFEmmOqzQwiqbZyUDZlBWJiCc.ToArray();
            UzZXXFuzWNxNlsNSQQJyPvPrGklaxyAH.ShapeMatchingConstraints = JoIbKooAuUvKMVJSrSNsMEDMNIbFOXsG.ToArray();
            UzZXXFuzWNxNlsNSQQJyPvPrGklaxyAH.PhysicsMaterials = ngZpoHzfuqAxdNGlZsoRMBMaHavkonsY;

            string oWlAQQFceOwGiFkQgmhLbqiGiKxCsvZg = AssetDatabase.GetAssetPath(aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ);
            if (!string.IsNullOrEmpty(oWlAQQFceOwGiFkQgmhLbqiGiKxCsvZg))
            {
                oWlAQQFceOwGiFkQgmhLbqiGiKxCsvZg = Path.GetDirectoryName(oWlAQQFceOwGiFkQgmhLbqiGiKxCsvZg);
            }

            string XGVVENJfgwdbbpkOufdMoNFJXMCxMQYs = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(oWlAQQFceOwGiFkQgmhLbqiGiKxCsvZg, $"{aKltvsDAXMUgZASyLTUlOYQeHUkGMbzJ.name}_SimulationMesh.asset"));
            string DnpkpcTRIjbCUBvwHukbBHInsOxiufmE = EditorUtility.SaveFilePanelInProject("Save Simulation Mesh", Path.GetFileNameWithoutExtension(XGVVENJfgwdbbpkOufdMoNFJXMCxMQYs), "asset", "Save Simulation Mesh", oWlAQQFceOwGiFkQgmhLbqiGiKxCsvZg);
            if (!string.IsNullOrEmpty(DnpkpcTRIjbCUBvwHukbBHInsOxiufmE))
            {
                AssetDatabase.CreateAsset(UzZXXFuzWNxNlsNSQQJyPvPrGklaxyAH, DnpkpcTRIjbCUBvwHukbBHInsOxiufmE);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private struct rQUjczMGmAntdJzNogxmTCillUFaMlSN
        {
            public bool Tearable;

            
            public float ParticleDamping;

            
            public float DistanceConstraintsInverseStiffness;

            
            public float VolumeConstraintsInverseStiffness;

            
            public float ShapeMatchingConstraintsStiffness;

            
            public float StaticFriction;
            public float KineticFriction;
        }
    }
}
