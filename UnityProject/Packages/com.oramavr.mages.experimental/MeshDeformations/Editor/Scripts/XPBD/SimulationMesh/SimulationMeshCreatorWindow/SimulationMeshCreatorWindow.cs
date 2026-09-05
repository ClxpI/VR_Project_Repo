namespace MAGES.MeshDeformations.Editor
{
    using MAGES.Editor;
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    /// <summary>
    /// A simulation mesh creator window, for creating primitive shapes as simulation meshes.
    /// </summary>
    public class SimulationMeshCreatorWindow : EditorWindow
    {
        private VisualElement LOuqNsecZVHfzUmJWqqYWNhOKiPqgOyz;
        private VisualElement sTPRardBtPRPmxaiRaoLmiXWVfxpDrbJ;

        /// <summary>
        /// Shows the window.
        /// </summary>
        [MenuItem("MAGES/Mesh Deformations/Simulation Mesh Creator Tool", priority = MAGESMenuItemPriorities.MeshDeformations)]
        public static void ShowWindow()
        {
            SimulationMeshCreatorWindow dxhRaPPhKhzMydOaKHBBEUimlTAZymop = GetWindow<SimulationMeshCreatorWindow>();
            dxhRaPPhKhzMydOaKHBBEUimlTAZymop.titleContent = new GUIContent("Simulation Mesh Creator Tool");
            dxhRaPPhKhzMydOaKHBBEUimlTAZymop.minSize = new Vector2(400, 400);
        }

        private void DDteQoMNhUxoFkauPspNNzlUtDvJsnqi()
        {
            LOuqNsecZVHfzUmJWqqYWNhOKiPqgOyz = rootVisualElement;
            LOuqNsecZVHfzUmJWqqYWNhOKiPqgOyz.style.paddingLeft = 10;
            LOuqNsecZVHfzUmJWqqYWNhOKiPqgOyz.style.paddingRight = 10;
            LOuqNsecZVHfzUmJWqqYWNhOKiPqgOyz.style.paddingTop = 10;
            LOuqNsecZVHfzUmJWqqYWNhOKiPqgOyz.style.paddingBottom = 10;

            Label rJqUpBgqWUuteWXOhrkSCdSOvalkyckL = new Label("Simulation Mesh Creator Tool");
            rJqUpBgqWUuteWXOhrkSCdSOvalkyckL.style.fontSize = 20;
            LOuqNsecZVHfzUmJWqqYWNhOKiPqgOyz.Add(rJqUpBgqWUuteWXOhrkSCdSOvalkyckL);

            DropdownField HPnSrFqtGdCxnRDPniAOcjNsOhSfCSHn = new DropdownField();
            HPnSrFqtGdCxnRDPniAOcjNsOhSfCSHn.label = "Primitive Type";
            sTPRardBtPRPmxaiRaoLmiXWVfxpDrbJ = new VisualElement();
            sTPRardBtPRPmxaiRaoLmiXWVfxpDrbJ.style.marginTop = 5;
            sTPRardBtPRPmxaiRaoLmiXWVfxpDrbJ.style.marginLeft = 10;
            sTPRardBtPRPmxaiRaoLmiXWVfxpDrbJ.style.marginRight = 10;
            sTPRardBtPRPmxaiRaoLmiXWVfxpDrbJ.Add(pEAJdXNBQjAamEJKNUFCGeUmpKQsrBKS());

            HPnSrFqtGdCxnRDPniAOcjNsOhSfCSHn.choices = new List<string> { "Rod", "Rope", "Cube", "Sphere" };
            HPnSrFqtGdCxnRDPniAOcjNsOhSfCSHn.index = 0;
            HPnSrFqtGdCxnRDPniAOcjNsOhSfCSHn.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                sTPRardBtPRPmxaiRaoLmiXWVfxpDrbJ.Clear();
                switch (evt.newValue)
                {
                    case "Rod":
                        sTPRardBtPRPmxaiRaoLmiXWVfxpDrbJ.Add(pEAJdXNBQjAamEJKNUFCGeUmpKQsrBKS());
                        break;
                    case "Cube":
                        sTPRardBtPRPmxaiRaoLmiXWVfxpDrbJ.Add(ITLUmGYvtRhatUkAqQLSGmVtQkmWQLIY());
                        break;
                    case "Sphere":
                        sTPRardBtPRPmxaiRaoLmiXWVfxpDrbJ.Add(iglMWCXxoXLSsnKjHXHAfNbUEfTqdfKD());
                        break;
                    case "Rope":
                        sTPRardBtPRPmxaiRaoLmiXWVfxpDrbJ.Add(ThYoqgnYSiHkLBDtpFtUYsKGagCFCqQx());
                        break;
                }
            });
            LOuqNsecZVHfzUmJWqqYWNhOKiPqgOyz.Add(HPnSrFqtGdCxnRDPniAOcjNsOhSfCSHn);
            LOuqNsecZVHfzUmJWqqYWNhOKiPqgOyz.Add(sTPRardBtPRPmxaiRaoLmiXWVfxpDrbJ);
        }

        private VisualElement iglMWCXxoXLSsnKjHXHAfNbUEfTqdfKD()
        {
            VisualElement JLpNhRNBikaRDVBNXLVLmPDQICUmqHgB = new VisualElement();

            Label sAfIOsdmaoRFyYtjXVxnfUmNOJYqUZVS = new Label("Sphere Settings");
            sAfIOsdmaoRFyYtjXVxnfUmNOJYqUZVS.style.fontSize = 16;
            JLpNhRNBikaRDVBNXLVLmPDQICUmqHgB.Add(sAfIOsdmaoRFyYtjXVxnfUmNOJYqUZVS);

            return JLpNhRNBikaRDVBNXLVLmPDQICUmqHgB;
        }

        private VisualElement ITLUmGYvtRhatUkAqQLSGmVtQkmWQLIY()
        {
            VisualElement katnYJpfnEmgWrvbRddiVpHCJfRpajfx = new VisualElement();

            Label DGMmcIELpKaKbmgBgPYPPZBTDHivhJsf = new Label("Cube Settings");
            DGMmcIELpKaKbmgBgPYPPZBTDHivhJsf.style.fontSize = 16;
            katnYJpfnEmgWrvbRddiVpHCJfRpajfx.Add(DGMmcIELpKaKbmgBgPYPPZBTDHivhJsf);

            return katnYJpfnEmgWrvbRddiVpHCJfRpajfx;
        }

        private VisualElement pEAJdXNBQjAamEJKNUFCGeUmpKQsrBKS()
        {
            VisualElement OeenelGvPfxCnnCJKhcRZDPGmtSDfXch = new VisualElement();

            Label tXOlWrTChEMABbjnGSFpijDwVvdLRTzV = new Label("Rod Settings");
            tXOlWrTChEMABbjnGSFpijDwVvdLRTzV.style.fontSize = 16;
            OeenelGvPfxCnnCJKhcRZDPGmtSDfXch.Add(tXOlWrTChEMABbjnGSFpijDwVvdLRTzV);

            Label asFsqxOinkafPJliOsDAxFDZgkIjDGSq = new Label("Geometry Settings");
            asFsqxOinkafPJliOsDAxFDZgkIjDGSq.style.fontSize = 14;
            asFsqxOinkafPJliOsDAxFDZgkIjDGSq.style.marginTop = 5;
            OeenelGvPfxCnnCJKhcRZDPGmtSDfXch.Add(asFsqxOinkafPJliOsDAxFDZgkIjDGSq);

            FloatField NAAvOQjsxIriGLtjCDhzERlMLmnmTkbk = new FloatField();
            NAAvOQjsxIriGLtjCDhzERlMLmnmTkbk.label = "Particle Radius";
            NAAvOQjsxIriGLtjCDhzERlMLmnmTkbk.value = ZkFhWUUJcsgYIPjZWaBNrOGPTrqtrwtN();
            NAAvOQjsxIriGLtjCDhzERlMLmnmTkbk.tooltip = "The radius of the particles that will make up the rod.";
            OeenelGvPfxCnnCJKhcRZDPGmtSDfXch.Add(NAAvOQjsxIriGLtjCDhzERlMLmnmTkbk);

            FloatField yqybgmaTAZvmYlRpWsthJGcxIowgWOWS = new FloatField();
            yqybgmaTAZvmYlRpWsthJGcxIowgWOWS.label = "Height";
            yqybgmaTAZvmYlRpWsthJGcxIowgWOWS.value = 1.0f;
            yqybgmaTAZvmYlRpWsthJGcxIowgWOWS.tooltip = "The preferred height of the rod.";
            OeenelGvPfxCnnCJKhcRZDPGmtSDfXch.Add(yqybgmaTAZvmYlRpWsthJGcxIowgWOWS);

            Toggle OpNBuOlyGQEyPiSkMVIwpETCdBGHsxNV = new Toggle();
            OpNBuOlyGQEyPiSkMVIwpETCdBGHsxNV.label = "Overlap Particles";
            OpNBuOlyGQEyPiSkMVIwpETCdBGHsxNV.value = true;
            OpNBuOlyGQEyPiSkMVIwpETCdBGHsxNV.tooltip = "Overlap particles to create a smoother collision surface?";
            OeenelGvPfxCnnCJKhcRZDPGmtSDfXch.Add(OpNBuOlyGQEyPiSkMVIwpETCdBGHsxNV);

            Label lALnbsUgnAZSANhaqGrOgHVBEgGydsSd = new Label("Physics Settings");
            lALnbsUgnAZSANhaqGrOgHVBEgGydsSd.style.fontSize = 14;
            lALnbsUgnAZSANhaqGrOgHVBEgGydsSd.style.marginTop = 5;
            OeenelGvPfxCnnCJKhcRZDPGmtSDfXch.Add(lALnbsUgnAZSANhaqGrOgHVBEgGydsSd);

            FloatField NXwMoUSMpXlMtrazIfzntigSqhoDerbN = new FloatField();
            NXwMoUSMpXlMtrazIfzntigSqhoDerbN.label = "Total Mass";
            NXwMoUSMpXlMtrazIfzntigSqhoDerbN.value = 1.0f;
            NXwMoUSMpXlMtrazIfzntigSqhoDerbN.tooltip = "The total mass of the rod.";
            OeenelGvPfxCnnCJKhcRZDPGmtSDfXch.Add(NXwMoUSMpXlMtrazIfzntigSqhoDerbN);

            FloatField vcdDguEpJkRNNMguseWHtdrkukrWSnfk = new FloatField();
            vcdDguEpJkRNNMguseWHtdrkukrWSnfk.label = "Particle Damping";
            vcdDguEpJkRNNMguseWHtdrkukrWSnfk.value = 0.1f;
            vcdDguEpJkRNNMguseWHtdrkukrWSnfk.tooltip = "The damping of the particles in the rod.";
            OeenelGvPfxCnnCJKhcRZDPGmtSDfXch.Add(vcdDguEpJkRNNMguseWHtdrkukrWSnfk);

            FloatField zPlQbyApXBeuyVPtEktpbfqJfWBdEELj = new FloatField();
            zPlQbyApXBeuyVPtEktpbfqJfWBdEELj.label = "Static Friction";
            zPlQbyApXBeuyVPtEktpbfqJfWBdEELj.value = 0.7f;
            zPlQbyApXBeuyVPtEktpbfqJfWBdEELj.tooltip = "The static friction of the particles in the rod.";
            OeenelGvPfxCnnCJKhcRZDPGmtSDfXch.Add(zPlQbyApXBeuyVPtEktpbfqJfWBdEELj);

            FloatField WjBrUjRwxrxVfUTsWqQzLCfpQgohtysP = new FloatField();
            WjBrUjRwxrxVfUTsWqQzLCfpQgohtysP.label = "Kinetic Friction";
            WjBrUjRwxrxVfUTsWqQzLCfpQgohtysP.value = 0.6f;
            WjBrUjRwxrxVfUTsWqQzLCfpQgohtysP.tooltip = "The kinetic friction of the particles in the rod.";
            OeenelGvPfxCnnCJKhcRZDPGmtSDfXch.Add(WjBrUjRwxrxVfUTsWqQzLCfpQgohtysP);

            Button zCdndSFhqnvwskSoSUkWueOejHDdCpQb = new Button(() =>
            {
                EXQhcYYShzyYigSBsGXfiFOTCFQKijjf(NAAvOQjsxIriGLtjCDhzERlMLmnmTkbk.value, yqybgmaTAZvmYlRpWsthJGcxIowgWOWS.value, OpNBuOlyGQEyPiSkMVIwpETCdBGHsxNV.value, NXwMoUSMpXlMtrazIfzntigSqhoDerbN.value, vcdDguEpJkRNNMguseWHtdrkukrWSnfk.value, zPlQbyApXBeuyVPtEktpbfqJfWBdEELj.value, WjBrUjRwxrxVfUTsWqQzLCfpQgohtysP.value);
            });
            zCdndSFhqnvwskSoSUkWueOejHDdCpQb.text = "Create Simulation Mesh";
            zCdndSFhqnvwskSoSUkWueOejHDdCpQb.style.marginTop = 5;
            OeenelGvPfxCnnCJKhcRZDPGmtSDfXch.Add(zCdndSFhqnvwskSoSUkWueOejHDdCpQb);

            return OeenelGvPfxCnnCJKhcRZDPGmtSDfXch;
        }

        private VisualElement ThYoqgnYSiHkLBDtpFtUYsKGagCFCqQx()
        {
            VisualElement FFEFxKkkAPbwVjNGIgJiRBvXBnAfgdnC = new VisualElement();

            Label USdnmWlBiKswHHrMYcHZXYCgXftrYwOZ = new Label("Rope Settings");
            USdnmWlBiKswHHrMYcHZXYCgXftrYwOZ.style.fontSize = 16;
            FFEFxKkkAPbwVjNGIgJiRBvXBnAfgdnC.Add(USdnmWlBiKswHHrMYcHZXYCgXftrYwOZ);

            Label eOLErzAkmELthgoDqwmcKJTnnIgJPuQF = new Label("Geometry Settings");
            eOLErzAkmELthgoDqwmcKJTnnIgJPuQF.style.fontSize = 14;
            eOLErzAkmELthgoDqwmcKJTnnIgJPuQF.style.marginTop = 5;
            FFEFxKkkAPbwVjNGIgJiRBvXBnAfgdnC.Add(eOLErzAkmELthgoDqwmcKJTnnIgJPuQF);

            FloatField JpvaguuKXeCjCQELnYqThkVkwAvKAALb = new FloatField();
            JpvaguuKXeCjCQELnYqThkVkwAvKAALb.label = "Particle Radius";
            JpvaguuKXeCjCQELnYqThkVkwAvKAALb.value = ZkFhWUUJcsgYIPjZWaBNrOGPTrqtrwtN();
            JpvaguuKXeCjCQELnYqThkVkwAvKAALb.tooltip = "The radius of the particles that will make up the rod.";
            FFEFxKkkAPbwVjNGIgJiRBvXBnAfgdnC.Add(JpvaguuKXeCjCQELnYqThkVkwAvKAALb);

            FloatField RgaBSMLTRVMCStTOcpKKJxFaDMFNRIHs = new FloatField();
            RgaBSMLTRVMCStTOcpKKJxFaDMFNRIHs.label = "Height";
            RgaBSMLTRVMCStTOcpKKJxFaDMFNRIHs.value = 1.0f;
            RgaBSMLTRVMCStTOcpKKJxFaDMFNRIHs.tooltip = "The preferred height of the rod.";
            FFEFxKkkAPbwVjNGIgJiRBvXBnAfgdnC.Add(RgaBSMLTRVMCStTOcpKKJxFaDMFNRIHs);

            Toggle JLkXiMSftMvQGcAsMOKtASunATKrMOCJ = new Toggle();
            JLkXiMSftMvQGcAsMOKtASunATKrMOCJ.label = "Overlap Particles";
            JLkXiMSftMvQGcAsMOKtASunATKrMOCJ.value = true;
            JLkXiMSftMvQGcAsMOKtASunATKrMOCJ.tooltip = "Overlap particles to create a smoother collision surface?";
            FFEFxKkkAPbwVjNGIgJiRBvXBnAfgdnC.Add(JLkXiMSftMvQGcAsMOKtASunATKrMOCJ);

            Label XHDRiyBvXHzghyuWEBNVjAFRbvupfXEb = new Label("Physics Settings");
            XHDRiyBvXHzghyuWEBNVjAFRbvupfXEb.style.fontSize = 14;
            XHDRiyBvXHzghyuWEBNVjAFRbvupfXEb.style.marginTop = 5;
            FFEFxKkkAPbwVjNGIgJiRBvXBnAfgdnC.Add(XHDRiyBvXHzghyuWEBNVjAFRbvupfXEb);

            FloatField ITqIkFQQYDqauuZAvohrRPgkBIhctWAW = new FloatField();
            ITqIkFQQYDqauuZAvohrRPgkBIhctWAW.label = "Total Mass";
            ITqIkFQQYDqauuZAvohrRPgkBIhctWAW.value = 1.0f;
            ITqIkFQQYDqauuZAvohrRPgkBIhctWAW.tooltip = "The total mass of the rod.";
            FFEFxKkkAPbwVjNGIgJiRBvXBnAfgdnC.Add(ITqIkFQQYDqauuZAvohrRPgkBIhctWAW);

            FloatField OHgNZTfoZnJqNsopZfdxRLvnQLXaEfjr = new FloatField();
            OHgNZTfoZnJqNsopZfdxRLvnQLXaEfjr.label = "Particle Damping";
            OHgNZTfoZnJqNsopZfdxRLvnQLXaEfjr.value = 0.1f;
            OHgNZTfoZnJqNsopZfdxRLvnQLXaEfjr.tooltip = "The damping of the particles in the rod.";
            FFEFxKkkAPbwVjNGIgJiRBvXBnAfgdnC.Add(OHgNZTfoZnJqNsopZfdxRLvnQLXaEfjr);

            FloatField xqeMNGcvlrRSpjTgHtvDFYJnjKZGZDEP = new FloatField();
            xqeMNGcvlrRSpjTgHtvDFYJnjKZGZDEP.label = "Static Friction";
            xqeMNGcvlrRSpjTgHtvDFYJnjKZGZDEP.value = 0.7f;
            xqeMNGcvlrRSpjTgHtvDFYJnjKZGZDEP.tooltip = "The static friction of the particles in the rod.";
            FFEFxKkkAPbwVjNGIgJiRBvXBnAfgdnC.Add(xqeMNGcvlrRSpjTgHtvDFYJnjKZGZDEP);

            FloatField cDjxlYytjGkIYIGUmFRpWfGnsqMZQncz = new FloatField();
            cDjxlYytjGkIYIGUmFRpWfGnsqMZQncz.label = "Kinetic Friction";
            cDjxlYytjGkIYIGUmFRpWfGnsqMZQncz.value = 0.6f;
            cDjxlYytjGkIYIGUmFRpWfGnsqMZQncz.tooltip = "The kinetic friction of the particles in the rod.";
            FFEFxKkkAPbwVjNGIgJiRBvXBnAfgdnC.Add(cDjxlYytjGkIYIGUmFRpWfGnsqMZQncz);

            Button ZnSAWdpLkxaLTrqXTgmOcTNDWwXhPllC = new Button(() =>
            {
                UuZiNUKwfLEZnfldoqhhbPaNgAESasSv(JpvaguuKXeCjCQELnYqThkVkwAvKAALb.value, RgaBSMLTRVMCStTOcpKKJxFaDMFNRIHs.value, JLkXiMSftMvQGcAsMOKtASunATKrMOCJ.value, ITqIkFQQYDqauuZAvohrRPgkBIhctWAW.value, OHgNZTfoZnJqNsopZfdxRLvnQLXaEfjr.value, xqeMNGcvlrRSpjTgHtvDFYJnjKZGZDEP.value, cDjxlYytjGkIYIGUmFRpWfGnsqMZQncz.value);
            });
            ZnSAWdpLkxaLTrqXTgmOcTNDWwXhPllC.text = "Create Simulation Mesh";
            ZnSAWdpLkxaLTrqXTgmOcTNDWwXhPllC.style.marginTop = 5;
            FFEFxKkkAPbwVjNGIgJiRBvXBnAfgdnC.Add(ZnSAWdpLkxaLTrqXTgmOcTNDWwXhPllC);

            return FFEFxKkkAPbwVjNGIgJiRBvXBnAfgdnC;
        }

        private void EXQhcYYShzyYigSBsGXfiFOTCFQKijjf(float particleRadius, float height, bool overlap, float mass, float damping, float staticFriction, float kineticFriction)
        {
            var JcwLgWVRDmFfohjnmcFNEQgKDEjRnWYi = wszlFgwkBZbWMYkPdgitrOzgedSUrNSZ("Rod");
            if (string.IsNullOrEmpty(JcwLgWVRDmFfohjnmcFNEQgKDEjRnWYi))
            {
                return;
            }

            if (overlap)
            {
                particleRadius *= 0.6f;
            }

            
            int cYjwKkElYazIRprsyeWrivYqksFywzSV = Mathf.CeilToInt(height / (2.0f * particleRadius));
            float DVfdDfBVHmOHQKKvMoktmBAjLEVqvoDj = mass / cYjwKkElYazIRprsyeWrivYqksFywzSV;
            float kOVXJorbTLuOdBQoYMTDmZikhDlWSdlT = -cYjwKkElYazIRprsyeWrivYqksFywzSV * particleRadius * 0.5f;

            PhysicsMaterial[] KNTbnLMwvzRFeKodkGKZAQKZqauKylFP = new PhysicsMaterial[]
            {
                new PhysicsMaterial
                {
                    DistanceConstraintInverseStiffness = 0.0f,
                    VolumeConstraintInverseStiffness = 0.0f,
                    CutForceThreshold = 0.0f,
                    ParticleDamping = damping,
                    StaticFriction = staticFriction,
                    KineticFriction = kineticFriction,
                },
            };

            Particle[] qbOKiOCRYGNZotruNsvehepvIOlwnoqt = new Particle[cYjwKkElYazIRprsyeWrivYqksFywzSV];
            ShapeMatchingConstraint[] QIcjvUSulLWrGthwchqanObVXShCYESB = new ShapeMatchingConstraint[cYjwKkElYazIRprsyeWrivYqksFywzSV];
            for (int iphMcNCUjaaLhVLPzhbiNfmMrSGxBxmJ = 0; iphMcNCUjaaLhVLPzhbiNfmMrSGxBxmJ < cYjwKkElYazIRprsyeWrivYqksFywzSV; iphMcNCUjaaLhVLPzhbiNfmMrSGxBxmJ++)
            {
                Particle ReCHJrudAAZltkiklnScZFdYcQjsYxJI = new Particle
                {
                    Enabled = true,
                    Breakable = false,
                    Position = new Vector3(Random.Range(-0.000001f, +0.000001f), kOVXJorbTLuOdBQoYMTDmZikhDlWSdlT, Random.Range(-0.000001f, +0.000001f)),
                    Velocity = Vector3.zero,
                    PhysicsMaterialIndex = 0,
                    InverseMass = 1f / DVfdDfBVHmOHQKKvMoktmBAjLEVqvoDj,
                    Kinematic = false,
                };

                ShapeMatchingConstraint OMFMyhAKmRTLvdnAwpjiQXWfeqUcGefl = new ShapeMatchingConstraint
                {
                    Enabled = true,
                    Breakable = false,
                    Particle = iphMcNCUjaaLhVLPzhbiNfmMrSGxBxmJ,
                    PhysicsMaterialIndex = 0,
                };

                qbOKiOCRYGNZotruNsvehepvIOlwnoqt[iphMcNCUjaaLhVLPzhbiNfmMrSGxBxmJ] = ReCHJrudAAZltkiklnScZFdYcQjsYxJI;
                QIcjvUSulLWrGthwchqanObVXShCYESB[iphMcNCUjaaLhVLPzhbiNfmMrSGxBxmJ] = OMFMyhAKmRTLvdnAwpjiQXWfeqUcGefl;

                kOVXJorbTLuOdBQoYMTDmZikhDlWSdlT += 2.0f * particleRadius;
            }

            SimulationMesh stEPsHGRTQkYkqMTjIfCSbnWBvbBADgy = ScriptableObject.CreateInstance<SimulationMesh>();
            stEPsHGRTQkYkqMTjIfCSbnWBvbBADgy.name = "Rod_SimulationMesh";
            stEPsHGRTQkYkqMTjIfCSbnWBvbBADgy.PhysicsMaterials = KNTbnLMwvzRFeKodkGKZAQKZqauKylFP;
            stEPsHGRTQkYkqMTjIfCSbnWBvbBADgy.Particles = qbOKiOCRYGNZotruNsvehepvIOlwnoqt;
            stEPsHGRTQkYkqMTjIfCSbnWBvbBADgy.ShapeMatchingConstraints = QIcjvUSulLWrGthwchqanObVXShCYESB;

            AssetDatabase.CreateAsset(stEPsHGRTQkYkqMTjIfCSbnWBvbBADgy, JcwLgWVRDmFfohjnmcFNEQgKDEjRnWYi);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void UuZiNUKwfLEZnfldoqhhbPaNgAESasSv(float particleRadius, float height, bool overlap, float mass, float damping, float staticFriction, float kineticFriction)
        {
            var uTXoMmqcOJvlqqVkJJPckmXxFiSTFmBl = wszlFgwkBZbWMYkPdgitrOzgedSUrNSZ("Rod");
            if (string.IsNullOrEmpty(uTXoMmqcOJvlqqVkJJPckmXxFiSTFmBl))
            {
                return;
            }

            if (overlap)
            {
                particleRadius *= 0.6f;
            }

            
            int lbiAapQOVKdbgQdZSSzTDeMCmdNiqugp = Mathf.CeilToInt(height / (2.0f * particleRadius));
            float VeJUOgXNAmHDWswlCRMTGaijgXpiVfFO = mass / lbiAapQOVKdbgQdZSSzTDeMCmdNiqugp;
            float KgtLzvQhWMIQJZEpENXFXJaNistIZjin = -lbiAapQOVKdbgQdZSSzTDeMCmdNiqugp * particleRadius * 0.5f;

            PhysicsMaterial[] TtEvGnJvrMobcWMuWKXHUVNvqGlikCMt = new PhysicsMaterial[]
            {
                new PhysicsMaterial
                {
                    DistanceConstraintInverseStiffness = 0.0f,
                    VolumeConstraintInverseStiffness = 0.0f,
                    CutForceThreshold = 0.0f,
                    ParticleDamping = damping,
                    StaticFriction = staticFriction,
                    KineticFriction = kineticFriction,
                },
            };

            Particle[] mAUbEZKTLXnesYAOtODTcCgpqCQZUsIQ = new Particle[lbiAapQOVKdbgQdZSSzTDeMCmdNiqugp];
            for (int kpKfWqgRXPrSfuCVofqBFWPLFbhsOQbF = 0; kpKfWqgRXPrSfuCVofqBFWPLFbhsOQbF < lbiAapQOVKdbgQdZSSzTDeMCmdNiqugp; kpKfWqgRXPrSfuCVofqBFWPLFbhsOQbF++)
            {
                Particle CcPqhktvFCvEFqgeTWHTRVkDGCMDeZyT = new Particle
                {
                    Enabled = true,
                    Breakable = false,
                    Position = new Vector3(Random.Range(-0.000001f, +0.000001f), KgtLzvQhWMIQJZEpENXFXJaNistIZjin, Random.Range(-0.000001f, +0.000001f)),
                    Velocity = Vector3.zero,
                    PhysicsMaterialIndex = 0,
                    InverseMass = 1f / VeJUOgXNAmHDWswlCRMTGaijgXpiVfFO,
                    Kinematic = false,
                };

                mAUbEZKTLXnesYAOtODTcCgpqCQZUsIQ[kpKfWqgRXPrSfuCVofqBFWPLFbhsOQbF] = CcPqhktvFCvEFqgeTWHTRVkDGCMDeZyT;

                KgtLzvQhWMIQJZEpENXFXJaNistIZjin += 2.0f * particleRadius;
            }

            List<DistanceConstraint> TTxDoiDCEcbUvJsuzVmbnxiZGKSvepBT = new List<DistanceConstraint>();
            for (int EiijceLeVUdTjpndrAfwksWRFYTmjbXE = 0; EiijceLeVUdTjpndrAfwksWRFYTmjbXE < lbiAapQOVKdbgQdZSSzTDeMCmdNiqugp - 1; EiijceLeVUdTjpndrAfwksWRFYTmjbXE++)
            {
                DistanceConstraint jpObVGXPOIqacPdqZkaZratjsMwLOsED = new DistanceConstraint
                {
                    Enabled = true,
                    Breakable = false,
                    ParticleA = EiijceLeVUdTjpndrAfwksWRFYTmjbXE,
                    ParticleB = EiijceLeVUdTjpndrAfwksWRFYTmjbXE + 1,
                    PhysicsMaterialIndex = 0,
                };

                TTxDoiDCEcbUvJsuzVmbnxiZGKSvepBT.Add(jpObVGXPOIqacPdqZkaZratjsMwLOsED);
            }

            
            for (int fGiRRuvwLNUmrLRpesvSJPXoRGjByZBK = 0; fGiRRuvwLNUmrLRpesvSJPXoRGjByZBK < lbiAapQOVKdbgQdZSSzTDeMCmdNiqugp - 2; fGiRRuvwLNUmrLRpesvSJPXoRGjByZBK++)
            {
                DistanceConstraint ZkiLgPXPhjNFcnSsulZVKsXkjbzpkhJT = new DistanceConstraint
                {
                    Enabled = true,
                    Breakable = false,
                    ParticleA = fGiRRuvwLNUmrLRpesvSJPXoRGjByZBK,
                    ParticleB = fGiRRuvwLNUmrLRpesvSJPXoRGjByZBK + 2,
                    PhysicsMaterialIndex = 0,
                };

                TTxDoiDCEcbUvJsuzVmbnxiZGKSvepBT.Add(ZkiLgPXPhjNFcnSsulZVKsXkjbzpkhJT);
            }

            CollinearConstraint[] MQqAVXxeqEockbduWcTgGJXzYORUeRlh = new CollinearConstraint[lbiAapQOVKdbgQdZSSzTDeMCmdNiqugp - 2];
            for (int NFtPIDLCHwLrIZjbNlAuPRpuIgvNxvzI = 0; NFtPIDLCHwLrIZjbNlAuPRpuIgvNxvzI < lbiAapQOVKdbgQdZSSzTDeMCmdNiqugp - 2; NFtPIDLCHwLrIZjbNlAuPRpuIgvNxvzI++)
            {
                CollinearConstraint qHkwhjemsEPBWUQkvzNKITmTmTzgzXSw = new CollinearConstraint
                {
                    Enabled = true,
                    Breakable = false,
                    ParticleA = NFtPIDLCHwLrIZjbNlAuPRpuIgvNxvzI,
                    ParticleB = NFtPIDLCHwLrIZjbNlAuPRpuIgvNxvzI + 1,
                    ParticleC = NFtPIDLCHwLrIZjbNlAuPRpuIgvNxvzI + 2,
                    PhysicsMaterialIndex = 0,
                };

                MQqAVXxeqEockbduWcTgGJXzYORUeRlh[NFtPIDLCHwLrIZjbNlAuPRpuIgvNxvzI] = qHkwhjemsEPBWUQkvzNKITmTmTzgzXSw;
            }

            SimulationMesh ipeVHMpkzEXbeIXVImIDlpGCSelFlwvi = ScriptableObject.CreateInstance<SimulationMesh>();
            ipeVHMpkzEXbeIXVImIDlpGCSelFlwvi.name = "Rod_SimulationMesh";
            ipeVHMpkzEXbeIXVImIDlpGCSelFlwvi.PhysicsMaterials = TtEvGnJvrMobcWMuWKXHUVNvqGlikCMt;
            ipeVHMpkzEXbeIXVImIDlpGCSelFlwvi.Particles = mAUbEZKTLXnesYAOtODTcCgpqCQZUsIQ;
            ipeVHMpkzEXbeIXVImIDlpGCSelFlwvi.DistanceConstraints = TTxDoiDCEcbUvJsuzVmbnxiZGKSvepBT.ToArray();
            ipeVHMpkzEXbeIXVImIDlpGCSelFlwvi.CollinearConstraints = MQqAVXxeqEockbduWcTgGJXzYORUeRlh;

            AssetDatabase.CreateAsset(ipeVHMpkzEXbeIXVImIDlpGCSelFlwvi, uTXoMmqcOJvlqqVkJJPckmXxFiSTFmBl);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private string wszlFgwkBZbWMYkPdgitrOzgedSUrNSZ(string defaultFileName)
        {
            string WfOTWXRlAtgZBudoaHdoNhVbPhwySEqX = "Assets";
            string yJmdFluCuWEglhEWGuxAmBLYGXjgoSrs = Path.GetFileNameWithoutExtension(AssetDatabase.GenerateUniqueAssetPath(Path.Combine(WfOTWXRlAtgZBudoaHdoNhVbPhwySEqX, $"{defaultFileName}_SimulationMesh.asset")));

            string DFxOCHrOEfOLREqjtFPwtysBgwLrshPJ = EditorUtility.SaveFilePanelInProject(
                title: "Save Simulation Mesh",
                defaultName: yJmdFluCuWEglhEWGuxAmBLYGXjgoSrs,
                "asset",
                "Select location to save Simulation Mesh");

            return DFxOCHrOEfOLREqjtFPwtysBgwLrshPJ;
        }

        private PhysicsWorld tQcWNiqUPBDhjjLzcfiXKZybsSmROgeF()
        {
            var KyAHHMDGdIQInLVqUehFSMIuVDLTYLKF = GameObject.FindFirstObjectByType<PhysicsWorld>();
            return KyAHHMDGdIQInLVqUehFSMIuVDLTYLKF;
        }

        private float ZkFhWUUJcsgYIPjZWaBNrOGPTrqtrwtN()
        {
            var PIOuaFhMoqTivLDwvmXZDGKvtFGcoUQj = tQcWNiqUPBDhjjLzcfiXKZybsSmROgeF();
            if (PIOuaFhMoqTivLDwvmXZDGKvtFGcoUQj != null)
            {
                return PIOuaFhMoqTivLDwvmXZDGKvtFGcoUQj.ParticleRadius;
            }

            return 0.1f;
        }
    }
}
