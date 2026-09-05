namespace MAGES.MeshDeformations
{
    using System.Collections.Generic;
    using MAGES.MeshDeformations.Utilities;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.Rendering;

    /// <summary>
    /// Renders a <see cref="MAGES.MeshDeformations.RopeActor"/> with a cylindrical mesh.
    /// </summary>
    [RequireComponent(typeof(RopeActor))]
    [ExecuteAlways]
    public class RopeRenderer : MonoBehaviour
    {
        private PhysicsWorld zUXOpxVhPSZjVhLHApshuyDZzhDySfZW;
        private RopeActor nVLDbHYXNRESFSdXjHwZwfyZSnksqPBj;

        private int hSMCxYLjbRMwGvbcfJFJpIzUbWKCzhHP = 0;
        private List<Mesh> nMHOhwEmdJBaPNbCoMhnarVapXIfTjDW = new List<Mesh>();

        [field: SerializeField]
        private int smoothingPointsPerSegment = 3;

        [field: SerializeField]
        private int slices = 6;

        /// <summary>
        /// Gets or sets the rope thickness.
        /// </summary>
        [field: SerializeField]
        public float Thickness { get; set; } = 0.1f;

        /// <summary>
        /// Gets or sets the number of smoothing points to be generated between two consecutive particles.
        /// </summary>
        public int SmoothingPointsPerSegment
        {
            get
            {
                return smoothingPointsPerSegment;
            }

            set
            {
                smoothingPointsPerSegment = math.max(2, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of cylindrical slices to be generated.
        /// </summary>
        public int Slices
        {
            get => slices;
            set
            {
                slices = math.max(3, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether we should generate end caps for the rope.
        /// </summary>
        [field: SerializeField]
        public bool GenerateEndCaps { get; set; } = true;

        /// <summary>
        /// Gets or sets the rendering layer mask.
        /// </summary>
        [field: SerializeField]
        public uint RenderingLayerMask { get; set; } = 1;

        /// <summary>
        /// Gets or sets the Shadow Casting Mode.
        /// </summary>
        [field: SerializeField]
        public ShadowCastingMode CastShadows { get; set; } = ShadowCastingMode.On;

        /// <summary>
        /// Gets or sets a value indicating whether the rope should receive shadows.
        /// </summary>
        [field: SerializeField]
        public bool ReceiveShadows { get; set; } = true;

        /// <summary>
        /// Gets or sets the rope's material.
        /// </summary>
        [field: SerializeField]
        public Material Material { get; set; } = null;

        private RopeActor RopeActor
        {
            get
            {
                if (nVLDbHYXNRESFSdXjHwZwfyZSnksqPBj == null)
                {
                    nVLDbHYXNRESFSdXjHwZwfyZSnksqPBj = GetComponent<RopeActor>();
                }

                return nVLDbHYXNRESFSdXjHwZwfyZSnksqPBj;
            }
        }

        private void LHhZapTHLNVYPHwgtGsneRxAoyPZvOSm()
        {
            RopeActor.SimulationMesh.AddedToPhysicsWorld.AddListener(oApRplFOnFSNpIiUyMLKYWYHelMyJEFd);
            RopeActor.SimulationMesh.RemovedFromPhysicsWorld.AddListener(DHmDCswIeUKwGhiYYlwawlgqfBnpnAHo);
        }

        private void uEAADafAffNaulqdlcHCEgfSDVezeLzr()
        {
            RopeActor.SimulationMesh.AddedToPhysicsWorld.RemoveListener(oApRplFOnFSNpIiUyMLKYWYHelMyJEFd);
            RopeActor.SimulationMesh.RemovedFromPhysicsWorld.RemoveListener(DHmDCswIeUKwGhiYYlwawlgqfBnpnAHo);
        }

        private void YQYZkhAXioDltemmkBbegEbclzhotRSQ()
        {
            if (!(RopeActor.SimulationMesh != null && RopeActor.SimulationMesh.IsPartOfPhysicsWorld))
            {
                HNntetKcRcAZKJCUNutHlNgGYFSpLGTl();
            }

            for (int AJPoYcNdKmKHvpOdGZHiBjVSMPTzghDQ = 0; AJPoYcNdKmKHvpOdGZHiBjVSMPTzghDQ < hSMCxYLjbRMwGvbcfJFJpIzUbWKCzhHP; AJPoYcNdKmKHvpOdGZHiBjVSMPTzghDQ++)
            {
                bvsRPpvIrOmZLCMdXSYduFqwGkWgjVdP(nMHOhwEmdJBaPNbCoMhnarVapXIfTjDW[AJPoYcNdKmKHvpOdGZHiBjVSMPTzghDQ]);
            }
        }

        private void qgwvoZXRFaovOOvGtYrFICzSlifSjsst()
        {
            JobHandle ksTDMPuuHRdbbDosATrFHUmwjKZuPjLp;

            NativeList<fpnPzvOaxRDnvGBQsMjhBEZZWUgRXRSA> OsHgcbJpKTPSmCLMnWlXDzoGDrGBRAbO = new NativeList<fpnPzvOaxRDnvGBQsMjhBEZZWUgRXRSA>(4, Allocator.TempJob);

            var mMraIKGvGCHIkWwOumltCcsHOdhBXbAJ = new kkzbHHnaNnUfejSlpBswgSpKblSchfyV
            {
                EnabledSegments = RopeActor.SimulationMesh.RuntimeDistanceConstraintData.Enabled,
                RopeSpans = OsHgcbJpKTPSmCLMnWlXDzoGDrGBRAbO,
            };

            ksTDMPuuHRdbbDosATrFHUmwjKZuPjLp = mMraIKGvGCHIkWwOumltCcsHOdhBXbAJ.Schedule();
            ksTDMPuuHRdbbDosATrFHUmwjKZuPjLp.Complete();

            
            while (nMHOhwEmdJBaPNbCoMhnarVapXIfTjDW.Count < OsHgcbJpKTPSmCLMnWlXDzoGDrGBRAbO.Length)
            {
                Mesh RRRGZdnyShSsTBjlTqLeLSvkGKLJQeyS = new Mesh();
                RRRGZdnyShSsTBjlTqLeLSvkGKLJQeyS.MarkDynamic();
                nMHOhwEmdJBaPNbCoMhnarVapXIfTjDW.Add(RRRGZdnyShSsTBjlTqLeLSvkGKLJQeyS);
            }

            hSMCxYLjbRMwGvbcfJFJpIzUbWKCzhHP = OsHgcbJpKTPSmCLMnWlXDzoGDrGBRAbO.Length;

            var vEoiyUUSrLVktUYFFtHAALYMXnrRMxvy = 1.0f / RopeActor.SimulationMesh.RuntimeParticleData.Count;
            int sfvJCJtHypoXBpsBuKADAFOriYXpkNpo, hwVuqpHxutyLLXKteOnucpeOjipANVFf;
            for (int XGDhETWfJWmcTADSgUrSrXKZbHzEHIkf = 0; XGDhETWfJWmcTADSgUrSrXKZbHzEHIkf < OsHgcbJpKTPSmCLMnWlXDzoGDrGBRAbO.Length; XGDhETWfJWmcTADSgUrSrXKZbHzEHIkf++)
            {
                fpnPzvOaxRDnvGBQsMjhBEZZWUgRXRSA fseAYgFjtlNMeHeqpiXZQgLJdDXlksTa = OsHgcbJpKTPSmCLMnWlXDzoGDrGBRAbO[XGDhETWfJWmcTADSgUrSrXKZbHzEHIkf];
                sfvJCJtHypoXBpsBuKADAFOriYXpkNpo = fseAYgFjtlNMeHeqpiXZQgLJdDXlksTa.StartControlPointIndex;
                hwVuqpHxutyLLXKteOnucpeOjipANVFf = fseAYgFjtlNMeHeqpiXZQgLJdDXlksTa.StartControlPointIndex + fseAYgFjtlNMeHeqpiXZQgLJdDXlksTa.ControlPointCount - 1;

                NativeSlice<float3> SNwoVoGaOdCvuMMRQLoGcwRblHjrEgGl = RopeActor.SimulationMesh.RuntimeParticleData.Position.Slice(fseAYgFjtlNMeHeqpiXZQgLJdDXlksTa.StartControlPointIndex, fseAYgFjtlNMeHeqpiXZQgLJdDXlksTa.ControlPointCount);

                fpNmUkrqdFDPAjVUCfGbJbhRIWgwDdSU(SNwoVoGaOdCvuMMRQLoGcwRblHjrEgGl, new float2(sfvJCJtHypoXBpsBuKADAFOriYXpkNpo * vEoiyUUSrLVktUYFFtHAALYMXnrRMxvy, hwVuqpHxutyLLXKteOnucpeOjipANVFf * vEoiyUUSrLVktUYFFtHAALYMXnrRMxvy), nMHOhwEmdJBaPNbCoMhnarVapXIfTjDW[XGDhETWfJWmcTADSgUrSrXKZbHzEHIkf]);
            }

            OsHgcbJpKTPSmCLMnWlXDzoGDrGBRAbO.Dispose();
        }

        private void HNntetKcRcAZKJCUNutHlNgGYFSpLGTl()
        {
            JobHandle iWEioLOZEuSzUvWTBePikHreSZckmnbz;

            
            NativeArray<bool> QsKABjGkguJVdGbdsvQcZOVrcOVdrlTI = new NativeArray<bool>(RopeActor.SharedSimulationMesh.DistanceConstraints.Length, Allocator.TempJob, NativeArrayOptions.ClearMemory);
            for (int fbfBqrbnPAKkZsdAoujYzeaqLQoYuCVU = 0; fbfBqrbnPAKkZsdAoujYzeaqLQoYuCVU < RopeActor.SharedSimulationMesh.DistanceConstraints.Length; fbfBqrbnPAKkZsdAoujYzeaqLQoYuCVU++)
            {
                QsKABjGkguJVdGbdsvQcZOVrcOVdrlTI[fbfBqrbnPAKkZsdAoujYzeaqLQoYuCVU] = RopeActor.SharedSimulationMesh.DistanceConstraints[fbfBqrbnPAKkZsdAoujYzeaqLQoYuCVU].Enabled;
            }

            NativeArray<float3> NFFwDPHDjHXYIUzLZAzhxGYqibSZvzlP = new NativeArray<float3>(RopeActor.SharedSimulationMesh.Particles.Length, Allocator.TempJob, NativeArrayOptions.ClearMemory);
            float4x4 SqSqxvNXSKXDSpMujpwrNnBQgjQCxMOI = RopeActor.transform.localToWorldMatrix;
            for (int ZyBxliwAMMajjjgiIHgQWmNVISDJeqJM = 0; ZyBxliwAMMajjjgiIHgQWmNVISDJeqJM < RopeActor.SharedSimulationMesh.Particles.Length; ZyBxliwAMMajjjgiIHgQWmNVISDJeqJM++)
            {
                NFFwDPHDjHXYIUzLZAzhxGYqibSZvzlP[ZyBxliwAMMajjjgiIHgQWmNVISDJeqJM] = math.mul(SqSqxvNXSKXDSpMujpwrNnBQgjQCxMOI, new float4(RopeActor.SharedSimulationMesh.Particles[ZyBxliwAMMajjjgiIHgQWmNVISDJeqJM].Position, 1.0f)).xyz;
            }

            NativeList<fpnPzvOaxRDnvGBQsMjhBEZZWUgRXRSA> mlDaWumPBQZHWPWnpSRJKtDAdWeYqZZo = new NativeList<fpnPzvOaxRDnvGBQsMjhBEZZWUgRXRSA>(4, Allocator.TempJob);

            var vjeNsZKkhwVhXujIjtGbaYwcIUIBWEYs = new kkzbHHnaNnUfejSlpBswgSpKblSchfyV
            {
                EnabledSegments = QsKABjGkguJVdGbdsvQcZOVrcOVdrlTI,
                RopeSpans = mlDaWumPBQZHWPWnpSRJKtDAdWeYqZZo,
            };

            iWEioLOZEuSzUvWTBePikHreSZckmnbz = vjeNsZKkhwVhXujIjtGbaYwcIUIBWEYs.Schedule();
            iWEioLOZEuSzUvWTBePikHreSZckmnbz.Complete();

            
            while (nMHOhwEmdJBaPNbCoMhnarVapXIfTjDW.Count < mlDaWumPBQZHWPWnpSRJKtDAdWeYqZZo.Length)
            {
                Mesh XLBqRTYQWzwtwOONlyMuPaJVISexWGVi = new Mesh();
                XLBqRTYQWzwtwOONlyMuPaJVISexWGVi.MarkDynamic();
                nMHOhwEmdJBaPNbCoMhnarVapXIfTjDW.Add(XLBqRTYQWzwtwOONlyMuPaJVISexWGVi);
            }

            hSMCxYLjbRMwGvbcfJFJpIzUbWKCzhHP = mlDaWumPBQZHWPWnpSRJKtDAdWeYqZZo.Length;

            var ULQiJumiNAnpbqIfAwcMVAlMkztwpugk = 1.0f / NFFwDPHDjHXYIUzLZAzhxGYqibSZvzlP.Length;
            int hLUowqvqQVhJUjSKUvfHIhvemjtktkYj, qBiQQItxSxHSYaemzmrSOiywdapFtvvD;
            for (int kDPXtrSFWKGZrAVRgEFKHjxduNQUkIoM = 0; kDPXtrSFWKGZrAVRgEFKHjxduNQUkIoM < mlDaWumPBQZHWPWnpSRJKtDAdWeYqZZo.Length; kDPXtrSFWKGZrAVRgEFKHjxduNQUkIoM++)
            {
                fpnPzvOaxRDnvGBQsMjhBEZZWUgRXRSA hMVhpWofJKUkdQrKjfDhnKcxxqKKBxfG = mlDaWumPBQZHWPWnpSRJKtDAdWeYqZZo[kDPXtrSFWKGZrAVRgEFKHjxduNQUkIoM];
                hLUowqvqQVhJUjSKUvfHIhvemjtktkYj = hMVhpWofJKUkdQrKjfDhnKcxxqKKBxfG.StartControlPointIndex;
                qBiQQItxSxHSYaemzmrSOiywdapFtvvD = hMVhpWofJKUkdQrKjfDhnKcxxqKKBxfG.StartControlPointIndex + hMVhpWofJKUkdQrKjfDhnKcxxqKKBxfG.ControlPointCount - 1;

                NativeArray<float3> IWrudigppcmkiThLJiiuLoOaWRiinNGp = NFFwDPHDjHXYIUzLZAzhxGYqibSZvzlP.GetSubArray(hMVhpWofJKUkdQrKjfDhnKcxxqKKBxfG.StartControlPointIndex, hMVhpWofJKUkdQrKjfDhnKcxxqKKBxfG.ControlPointCount);

                fpNmUkrqdFDPAjVUCfGbJbhRIWgwDdSU(IWrudigppcmkiThLJiiuLoOaWRiinNGp, new float2(hLUowqvqQVhJUjSKUvfHIhvemjtktkYj * ULQiJumiNAnpbqIfAwcMVAlMkztwpugk, qBiQQItxSxHSYaemzmrSOiywdapFtvvD * ULQiJumiNAnpbqIfAwcMVAlMkztwpugk), nMHOhwEmdJBaPNbCoMhnarVapXIfTjDW[kDPXtrSFWKGZrAVRgEFKHjxduNQUkIoM]);

                IWrudigppcmkiThLJiiuLoOaWRiinNGp.Dispose();
            }

            NFFwDPHDjHXYIUzLZAzhxGYqibSZvzlP.Dispose();
            QsKABjGkguJVdGbdsvQcZOVrcOVdrlTI.Dispose();
            mlDaWumPBQZHWPWnpSRJKtDAdWeYqZZo.Dispose();
        }

        private void bvsRPpvIrOmZLCMdXSYduFqwGkWgjVdP(Mesh m)
        {
            var BvfcSilWNdiDEICnaNYzvHWoQYSOZnGh = new RenderParams
            {
                layer = gameObject.layer,
                renderingLayerMask = RenderingLayerMask,
                motionVectorMode = MotionVectorGenerationMode.Camera,
                receiveShadows = ReceiveShadows,
                shadowCastingMode = CastShadows,
                material = Material,
            };

            Graphics.RenderMesh(BvfcSilWNdiDEICnaNYzvHWoQYSOZnGh, m, 0, float4x4.identity);
        }

        private void oApRplFOnFSNpIiUyMLKYWYHelMyJEFd()
        {
            zUXOpxVhPSZjVhLHApshuyDZzhDySfZW = RopeActor.SimulationMesh.PhysicsWorld;
            zUXOpxVhPSZjVhLHApshuyDZzhDySfZW.LatePhysicsUpdate.AddListener(fmZtKaLPCAARFvXNxLrBcwrWjZmzCMvQ);
        }

        private void fmZtKaLPCAARFvXNxLrBcwrWjZmzCMvQ(float dt)
        {
            qgwvoZXRFaovOOvGtYrFICzSlifSjsst();
        }

        private void DHmDCswIeUKwGhiYYlwawlgqfBnpnAHo()
        {
            zUXOpxVhPSZjVhLHApshuyDZzhDySfZW.LatePhysicsUpdate.RemoveListener(fmZtKaLPCAARFvXNxLrBcwrWjZmzCMvQ);
            zUXOpxVhPSZjVhLHApshuyDZzhDySfZW = null;
        }

        private void fpNmUkrqdFDPAjVUCfGbJbhRIWgwDdSU(
            NativeSlice<float3> controlPoints,
#pragma warning disable SA1305 // Field names should not use Hungarian notation
            float2 uvRange,
#pragma warning restore SA1305 // Field names should not use Hungarian notation
            Mesh mesh)
        {
            var puDujjXTckgxGMTHOvPEIJoMCVackOiN = SmoothingPointsPerSegment * (controlPoints.Length - 1);

            NativeArray<PathNode> FBwMJEwDDyGezMHZLhOYssMdcQIlLnPn = new NativeArray<PathNode>(puDujjXTckgxGMTHOvPEIJoMCVackOiN, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

            var XQmmoWVqaINiGofaUuPBbeuDkUkEnVyW = puDujjXTckgxGMTHOvPEIJoMCVackOiN * Slices;
            if (GenerateEndCaps)
            {
                XQmmoWVqaINiGofaUuPBbeuDkUkEnVyW += 2 * Slices;
            }

            var qzWfMGkbngkZyHeGttNJyVKAbyIduySz = (puDujjXTckgxGMTHOvPEIJoMCVackOiN - 1) * Slices * 6;
            if (GenerateEndCaps)
            {
                qzWfMGkbngkZyHeGttNJyVKAbyIduySz += 2 * (Slices - 2) * 3;
            }

            NativeArray<float3> NGfgrtPgMlZwPSBlyfIVXwFNQTCxARiY = new NativeArray<float3>(XQmmoWVqaINiGofaUuPBbeuDkUkEnVyW, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            NativeArray<float3> xpgcygbqMdZzyDQwOHbIYFCvLIhmsqKO = new NativeArray<float3>(XQmmoWVqaINiGofaUuPBbeuDkUkEnVyW, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            NativeArray<float2> dyMrfmDNENEWvWlnHGpLskBbfXqfaFuJ = new NativeArray<float2>(XQmmoWVqaINiGofaUuPBbeuDkUkEnVyW, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

            NativeArray<int> HGRiEWvtQwkQaOziUncCqXvYTchZHIXt = new NativeArray<int>(qzWfMGkbngkZyHeGttNJyVKAbyIduySz, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

            float3 BkVdCNeAAPnnBtNKMxMOMmpOPDuAUlhn = controlPoints[1] - controlPoints[0];
            float3 uAiNVgvutZZCqPecbAVRUptSVuAXbqCm = Math.ArbitraryPerpendicularVector(BkVdCNeAAPnnBtNKMxMOMmpOPDuAUlhn);

            var XUXVdletwCdKffQzgYhWAnXmNbsVwrGf = new vFOBXBiGDVHEIQiCUGIpijIjTjGMBggK
            {
                NumSmoothingPoints = puDujjXTckgxGMTHOvPEIJoMCVackOiN,
                ArbitraryUp = uAiNVgvutZZCqPecbAVRUptSVuAXbqCm,
                ControlPoints = controlPoints,
                SmoothedPathNodes = FBwMJEwDDyGezMHZLhOYssMdcQIlLnPn,
            };

            var WIOULYzchAuZrFKeSNEuvVbgqscodyAB = new qpEdAeBRDBCfFRTXFdJmXNYmXlcCPoxb
            {
                SmoothPath = FBwMJEwDDyGezMHZLhOYssMdcQIlLnPn,
                Slices = Slices,
                GenerateEndCaps = GenerateEndCaps,
                Radius = Thickness * 0.5f,
                Vertices = NGfgrtPgMlZwPSBlyfIVXwFNQTCxARiY,
                Normals = xpgcygbqMdZzyDQwOHbIYFCvLIhmsqKO,
                UVs = dyMrfmDNENEWvWlnHGpLskBbfXqfaFuJ,
                Indices = HGRiEWvtQwkQaOziUncCqXvYTchZHIXt,
                UVRange = uvRange,
            };

            JobHandle WFzzPOejshwkNRGeONhsmLOUwrSYYtDP = XUXVdletwCdKffQzgYhWAnXmNbsVwrGf.Schedule(puDujjXTckgxGMTHOvPEIJoMCVackOiN, 64);
            WFzzPOejshwkNRGeONhsmLOUwrSYYtDP = WIOULYzchAuZrFKeSNEuvVbgqscodyAB.Schedule(puDujjXTckgxGMTHOvPEIJoMCVackOiN, 64, WFzzPOejshwkNRGeONhsmLOUwrSYYtDP);
            WFzzPOejshwkNRGeONhsmLOUwrSYYtDP.Complete();

            mesh.Clear();
            mesh.SetVertices(NGfgrtPgMlZwPSBlyfIVXwFNQTCxARiY);
            mesh.SetNormals(xpgcygbqMdZzyDQwOHbIYFCvLIhmsqKO);
            mesh.SetUVs(0, dyMrfmDNENEWvWlnHGpLskBbfXqfaFuJ);
            mesh.SetIndices(HGRiEWvtQwkQaOziUncCqXvYTchZHIXt, MeshTopology.Triangles, 0);
            mesh.RecalculateBounds();

            FBwMJEwDDyGezMHZLhOYssMdcQIlLnPn.Dispose();
            NGfgrtPgMlZwPSBlyfIVXwFNQTCxARiY.Dispose();
            xpgcygbqMdZzyDQwOHbIYFCvLIhmsqKO.Dispose();
            dyMrfmDNENEWvWlnHGpLskBbfXqfaFuJ.Dispose();
            HGRiEWvtQwkQaOziUncCqXvYTchZHIXt.Dispose();
        }

        private struct kkzbHHnaNnUfejSlpBswgSpKblSchfyV : IJob
        {
            [ReadOnly]
            public NativeSlice<bool> EnabledSegments;

            [WriteOnly]
            public NativeList<fpnPzvOaxRDnvGBQsMjhBEZZWUgRXRSA> RopeSpans; 

            public void Execute()
            {
                int vHKiYkDcLkwfvAwJLnnpmOciHWHaemzl = -1; 

                for (int RomdBkohRnDKCgRmUypgICCzajcNYOtp = 0; RomdBkohRnDKCgRmUypgICCzajcNYOtp < EnabledSegments.Length; RomdBkohRnDKCgRmUypgICCzajcNYOtp++)
                {
                    if (EnabledSegments[RomdBkohRnDKCgRmUypgICCzajcNYOtp])
                    {
                        
                        if (vHKiYkDcLkwfvAwJLnnpmOciHWHaemzl == -1)
                        {
                            vHKiYkDcLkwfvAwJLnnpmOciHWHaemzl = RomdBkohRnDKCgRmUypgICCzajcNYOtp;
                        }
                    }
                    else
                    {
                        
                        if (vHKiYkDcLkwfvAwJLnnpmOciHWHaemzl != -1)
                        {
                            
                            RopeSpans.Add(new fpnPzvOaxRDnvGBQsMjhBEZZWUgRXRSA
                            {
                                StartControlPointIndex = vHKiYkDcLkwfvAwJLnnpmOciHWHaemzl,
                                ControlPointCount = RomdBkohRnDKCgRmUypgICCzajcNYOtp - vHKiYkDcLkwfvAwJLnnpmOciHWHaemzl + 1,
                            });

                            
                            vHKiYkDcLkwfvAwJLnnpmOciHWHaemzl = -1;
                        }
                    }
                }

                
                if (vHKiYkDcLkwfvAwJLnnpmOciHWHaemzl != -1)
                {
                    RopeSpans.Add(new fpnPzvOaxRDnvGBQsMjhBEZZWUgRXRSA
                    {
                        StartControlPointIndex = vHKiYkDcLkwfvAwJLnnpmOciHWHaemzl,
                        ControlPointCount = EnabledSegments.Length - vHKiYkDcLkwfvAwJLnnpmOciHWHaemzl + 1,
                    });
                }
            }
        }

        private struct vFOBXBiGDVHEIQiCUGIpijIjTjGMBggK : IJobParallelFor
        {
            public int NumSmoothingPoints;

            public float3 ArbitraryUp;

            [ReadOnly]
            public NativeSlice<float3> ControlPoints; 

            [WriteOnly]
            public NativeArray<PathNode> SmoothedPathNodes;

            public void Execute(int index)
            {
                int tZzTnxRArqSjHEaLoAHNCqBhNlSaIPBG = ControlPoints.Length;
                int FhmQiDhsMpmPbVjbXzSxpfJkrsNefgJm = tZzTnxRArqSjHEaLoAHNCqBhNlSaIPBG - 1;

                float FJECCYaRLqOKcEKLuoJOprDVtIaBirwS = FhmQiDhsMpmPbVjbXzSxpfJkrsNefgJm * 0.9999f;
                float RrKKeNhTkGMADNPhuVAJHOXmEemuPVmA = ((float)index / (float)(NumSmoothingPoints - 1)) * FJECCYaRLqOKcEKLuoJOprDVtIaBirwS;
                int JMBskSQHsraQmcxrKTBxFawkMtXzaveb = (int)math.floor(RrKKeNhTkGMADNPhuVAJHOXmEemuPVmA);

                float OWisuZHNDXYnbdEZViSfCNJsTZOZnork = RrKKeNhTkGMADNPhuVAJHOXmEemuPVmA - JMBskSQHsraQmcxrKTBxFawkMtXzaveb;

                
                float3 GCIYHkFbiVPbivMgghKclFSIoKXsxNpE = (JMBskSQHsraQmcxrKTBxFawkMtXzaveb == 0) ? ControlPoints[0] : ControlPoints[JMBskSQHsraQmcxrKTBxFawkMtXzaveb - 1];
                float3 JIGfwtWRKxcuGZprWQpCTmsRqmPVEATD = ControlPoints[JMBskSQHsraQmcxrKTBxFawkMtXzaveb];
                float3 CWHfFqHbWZhxJxoxFgNaciCybEalHeXn = ControlPoints[JMBskSQHsraQmcxrKTBxFawkMtXzaveb + 1];
                float3 VLmVnniZTDuJueTcrtzNoHJOiXtHSvcB = (JMBskSQHsraQmcxrKTBxFawkMtXzaveb + 2 < tZzTnxRArqSjHEaLoAHNCqBhNlSaIPBG) ? ControlPoints[JMBskSQHsraQmcxrKTBxFawkMtXzaveb + 2] : ControlPoints[tZzTnxRArqSjHEaLoAHNCqBhNlSaIPBG - 1];

                
                float3 iCuCVsJfITUHsxVTLhaxzgYlwkOKBwlL = Math.CatmullRomSpline(GCIYHkFbiVPbivMgghKclFSIoKXsxNpE, JIGfwtWRKxcuGZprWQpCTmsRqmPVEATD, CWHfFqHbWZhxJxoxFgNaciCybEalHeXn, VLmVnniZTDuJueTcrtzNoHJOiXtHSvcB, OWisuZHNDXYnbdEZViSfCNJsTZOZnork);

                
                float kqHWiqaVkpxavpPZjAlbbqfdPvTdchFo = 0.001f; 
                float3 EXYkQoIpHuNiBnmOmgWZQNSqMYVrwcsT = Math.CatmullRomSpline(GCIYHkFbiVPbivMgghKclFSIoKXsxNpE, JIGfwtWRKxcuGZprWQpCTmsRqmPVEATD, CWHfFqHbWZhxJxoxFgNaciCybEalHeXn, VLmVnniZTDuJueTcrtzNoHJOiXtHSvcB, OWisuZHNDXYnbdEZViSfCNJsTZOZnork + kqHWiqaVkpxavpPZjAlbbqfdPvTdchFo);
                float3 pwkZSvbeAJkFRniWdaSeCyrZegviREzs = math.normalize(EXYkQoIpHuNiBnmOmgWZQNSqMYVrwcsT - iCuCVsJfITUHsxVTLhaxzgYlwkOKBwlL); 

                
                float3 kbYEiPUqCXVjediqbUvGakcDtHtLMKsP = math.normalize(math.cross(ArbitraryUp, pwkZSvbeAJkFRniWdaSeCyrZegviREzs));

                
                float3 uxmZJwXwtZqUaqXchdoXiutBZYssActO = math.normalize(math.cross(pwkZSvbeAJkFRniWdaSeCyrZegviREzs, kbYEiPUqCXVjediqbUvGakcDtHtLMKsP));

                
                SmoothedPathNodes[index] = new PathNode(iCuCVsJfITUHsxVTLhaxzgYlwkOKBwlL, pwkZSvbeAJkFRniWdaSeCyrZegviREzs, kbYEiPUqCXVjediqbUvGakcDtHtLMKsP, uxmZJwXwtZqUaqXchdoXiutBZYssActO);
            }
        }

        private struct qpEdAeBRDBCfFRTXFdJmXNYmXlcCPoxb : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<PathNode> SmoothPath;
            [ReadOnly]
            public int Slices;
            [ReadOnly]
            public float Radius;
            [ReadOnly]
            public float2 UVRange;
            [ReadOnly]
            public bool GenerateEndCaps;

            [NativeDisableParallelForRestriction]
            public NativeArray<float3> Vertices;
            [NativeDisableParallelForRestriction]
            public NativeArray<float3> Normals;
            [NativeDisableParallelForRestriction]
            public NativeArray<float2> UVs;

            [NativeDisableParallelForRestriction]
            public NativeArray<int> Indices;

            public void Execute(int index)
            {
                PathNode VMUHrPqbpLgInrkjWKsTlIWrTryOYzPA = SmoothPath[index];

                var dPWYbrgmQZlgSKxBinYtodPXtCUMhqlG = VMUHrPqbpLgInrkjWKsTlIWrTryOYzPA.Tangent;
                var BbCmdbEGRNBntDUxtidKNvCeWEMSMqiE = VMUHrPqbpLgInrkjWKsTlIWrTryOYzPA.Binormal;
                var wmRpNqSEFenEdVxriEJAZODbKGgHonXi = VMUHrPqbpLgInrkjWKsTlIWrTryOYzPA.Normal;

                var NdOryfCWoWtPbfajrLevWwdbgSEBAGiD = 0;
                var HIDeSAJBasuHWqRgontsxdYSltpjIRjg = 0;

                
                if (GenerateEndCaps)
                {
                    if (index == 0)
                    {
                        
                        
                        for (int TwXRLaszQAWfmLvqAXgBZAVXvQLBAHza = 0; TwXRLaszQAWfmLvqAXgBZAVXvQLBAHza < Slices; TwXRLaszQAWfmLvqAXgBZAVXvQLBAHza++)
                        {
                            
                            float DvEIzuDopmQgCHafBXlJFcVNmJLqlpFm = (2 * math.PI) * ((float)TwXRLaszQAWfmLvqAXgBZAVXvQLBAHza / Slices);

                            
                            float3 GetbVrnrYLfZXWnSwOVirEJxDwqsmmCY = ((math.cos(DvEIzuDopmQgCHafBXlJFcVNmJLqlpFm) * BbCmdbEGRNBntDUxtidKNvCeWEMSMqiE) + (math.sin(DvEIzuDopmQgCHafBXlJFcVNmJLqlpFm) * wmRpNqSEFenEdVxriEJAZODbKGgHonXi)) * Radius;
                            float3 LSyrdixOuUdKCEXtZKYrmYrzJHJiZjYB = VMUHrPqbpLgInrkjWKsTlIWrTryOYzPA.Position + GetbVrnrYLfZXWnSwOVirEJxDwqsmmCY; 

                            int BTkgkPOwhzABrVfmszggNCFpHgbiFTGr = TwXRLaszQAWfmLvqAXgBZAVXvQLBAHza;
                            Vertices[BTkgkPOwhzABrVfmszggNCFpHgbiFTGr] = LSyrdixOuUdKCEXtZKYrmYrzJHJiZjYB;

                            
                            Normals[BTkgkPOwhzABrVfmszggNCFpHgbiFTGr] = -math.normalize(SmoothPath[0].Tangent);

                            
                            var XXhhYMUsHpAOFBRqhuhZarVnClInsRAZ = math.remap(0, 1f, UVRange.x, UVRange.y, 0f);
                            UVs[BTkgkPOwhzABrVfmszggNCFpHgbiFTGr] = new float2(XXhhYMUsHpAOFBRqhuhZarVnClInsRAZ, (float)TwXRLaszQAWfmLvqAXgBZAVXvQLBAHza / Slices);
                        }

                        
                        for (int OXWAlweLurWGmBZMBATGXCRkrHrSBecO = 0; OXWAlweLurWGmBZMBATGXCRkrHrSBecO < Slices - 2; OXWAlweLurWGmBZMBATGXCRkrHrSBecO++)
                        {
                            Indices[OXWAlweLurWGmBZMBATGXCRkrHrSBecO * 3] = 0;
                            Indices[(OXWAlweLurWGmBZMBATGXCRkrHrSBecO * 3) + 1] = OXWAlweLurWGmBZMBATGXCRkrHrSBecO + 1;
                            Indices[(OXWAlweLurWGmBZMBATGXCRkrHrSBecO * 3) + 2] = OXWAlweLurWGmBZMBATGXCRkrHrSBecO + 2;
                        }
                    }

                    HIDeSAJBasuHWqRgontsxdYSltpjIRjg += Slices;
                    NdOryfCWoWtPbfajrLevWwdbgSEBAGiD += (Slices - 2) * 3;
                }

                
                for (int TDWcGvdYDsrpsUNcUCVwBgXPcBAPkMoA = 0; TDWcGvdYDsrpsUNcUCVwBgXPcBAPkMoA < Slices; TDWcGvdYDsrpsUNcUCVwBgXPcBAPkMoA++)
                {
                    
                    float SvhjREWGuUqhvlDNpSeXEPvNVVeBnuny = (2 * math.PI) * ((float)TDWcGvdYDsrpsUNcUCVwBgXPcBAPkMoA / Slices);

                    
                    float3 VidsGpjlXWylwlrQBVZHkDkQQYmPmSrE = ((math.cos(SvhjREWGuUqhvlDNpSeXEPvNVVeBnuny) * BbCmdbEGRNBntDUxtidKNvCeWEMSMqiE) + (math.sin(SvhjREWGuUqhvlDNpSeXEPvNVVeBnuny) * wmRpNqSEFenEdVxriEJAZODbKGgHonXi)) * Radius;
                    float3 MYGtPHKFsrEzRkrMSgZAAsqanFdsvCwq = VMUHrPqbpLgInrkjWKsTlIWrTryOYzPA.Position + VidsGpjlXWylwlrQBVZHkDkQQYmPmSrE; 

                    int GsrlpJetBLRHDWCpxSroEKoBmjAWhIME = (index * Slices) + TDWcGvdYDsrpsUNcUCVwBgXPcBAPkMoA;
                    Vertices[GsrlpJetBLRHDWCpxSroEKoBmjAWhIME + HIDeSAJBasuHWqRgontsxdYSltpjIRjg] = MYGtPHKFsrEzRkrMSgZAAsqanFdsvCwq;

                    
                    Normals[GsrlpJetBLRHDWCpxSroEKoBmjAWhIME + HIDeSAJBasuHWqRgontsxdYSltpjIRjg] = math.normalize(VidsGpjlXWylwlrQBVZHkDkQQYmPmSrE);

                    
                    var hJWWfyTCADZXiSHGHyRpKmeXChHWlToe = math.remap(0, 1f, UVRange.x, UVRange.y, (float)index / SmoothPath.Length);
                    UVs[GsrlpJetBLRHDWCpxSroEKoBmjAWhIME + HIDeSAJBasuHWqRgontsxdYSltpjIRjg] = new float2(hJWWfyTCADZXiSHGHyRpKmeXChHWlToe, (float)TDWcGvdYDsrpsUNcUCVwBgXPcBAPkMoA / Slices);
                }

                
                if (index < SmoothPath.Length - 1)
                {
                    int UJbTNDgGDnpgtGHuOPzhiigjCIQUnBVo = index + 1;

                    for (int IgbOLcZevWqEVCutzweAOwGdtWHagefw = 0; IgbOLcZevWqEVCutzweAOwGdtWHagefw < Slices; IgbOLcZevWqEVCutzweAOwGdtWHagefw++)
                    {
                        
                        int uCIDFqolyjstXfggWGaFrkOPPZRoSiTp = (IgbOLcZevWqEVCutzweAOwGdtWHagefw + 1) % Slices;

                        
                        int QWYrhQQLykCWNebybhgvYgPEwEeICvPI = (index * Slices) + IgbOLcZevWqEVCutzweAOwGdtWHagefw + HIDeSAJBasuHWqRgontsxdYSltpjIRjg;
                        int YMTnRJkJiSCnBxirOYxMvuRszdWQfhsR = (index * Slices) + uCIDFqolyjstXfggWGaFrkOPPZRoSiTp + HIDeSAJBasuHWqRgontsxdYSltpjIRjg;
                        int kPrHnShSTDhHnnoslZiRrYKPbjQqQQSw = (UJbTNDgGDnpgtGHuOPzhiigjCIQUnBVo * Slices) + IgbOLcZevWqEVCutzweAOwGdtWHagefw + HIDeSAJBasuHWqRgontsxdYSltpjIRjg;
                        int bychnBsuwYHaZHfEMUraKjpXoiYuyyuN = (UJbTNDgGDnpgtGHuOPzhiigjCIQUnBVo * Slices) + uCIDFqolyjstXfggWGaFrkOPPZRoSiTp + HIDeSAJBasuHWqRgontsxdYSltpjIRjg;

                        
                        int qVFowIffLrUZzQmKfwockRQDyZPboKLq = ((index * Slices) + IgbOLcZevWqEVCutzweAOwGdtWHagefw) * 6;
                        Indices[qVFowIffLrUZzQmKfwockRQDyZPboKLq + NdOryfCWoWtPbfajrLevWwdbgSEBAGiD] = QWYrhQQLykCWNebybhgvYgPEwEeICvPI;
                        Indices[qVFowIffLrUZzQmKfwockRQDyZPboKLq + 1 + NdOryfCWoWtPbfajrLevWwdbgSEBAGiD] = kPrHnShSTDhHnnoslZiRrYKPbjQqQQSw;
                        Indices[qVFowIffLrUZzQmKfwockRQDyZPboKLq + 2 + NdOryfCWoWtPbfajrLevWwdbgSEBAGiD] = YMTnRJkJiSCnBxirOYxMvuRszdWQfhsR;

                        
                        Indices[qVFowIffLrUZzQmKfwockRQDyZPboKLq + 3 + NdOryfCWoWtPbfajrLevWwdbgSEBAGiD] = YMTnRJkJiSCnBxirOYxMvuRszdWQfhsR;
                        Indices[qVFowIffLrUZzQmKfwockRQDyZPboKLq + 4 + NdOryfCWoWtPbfajrLevWwdbgSEBAGiD] = kPrHnShSTDhHnnoslZiRrYKPbjQqQQSw;
                        Indices[qVFowIffLrUZzQmKfwockRQDyZPboKLq + 5 + NdOryfCWoWtPbfajrLevWwdbgSEBAGiD] = bychnBsuwYHaZHfEMUraKjpXoiYuyyuN;
                    }
                }

                
                if (GenerateEndCaps)
                {
                    
                    if (index == SmoothPath.Length - 1)
                    {
                        int AcBrTMmubprGIOuZYibtwUPGAJRIlqVa = (SmoothPath.Length * Slices) + HIDeSAJBasuHWqRgontsxdYSltpjIRjg;
                        int FLBNbkkCteYeyKpbgcaBAuLNIludKxvb = ((SmoothPath.Length - 1) * Slices * 6) + NdOryfCWoWtPbfajrLevWwdbgSEBAGiD;

                        
                        for (int hMAgHPuPGNvnqRRHjdChWlFguhTOhEYe = 0; hMAgHPuPGNvnqRRHjdChWlFguhTOhEYe < Slices; hMAgHPuPGNvnqRRHjdChWlFguhTOhEYe++)
                        {
                            
                            float fekNPHXCWVIWyfNbLToGSDWaXKEXyYUs = (2 * math.PI) * ((float)hMAgHPuPGNvnqRRHjdChWlFguhTOhEYe / Slices);

                            
                            float3 jnzBzyYqdqMfHqlLTjbPFaurVvrNiozF = ((math.cos(fekNPHXCWVIWyfNbLToGSDWaXKEXyYUs) * BbCmdbEGRNBntDUxtidKNvCeWEMSMqiE) + (math.sin(fekNPHXCWVIWyfNbLToGSDWaXKEXyYUs) * wmRpNqSEFenEdVxriEJAZODbKGgHonXi)) * Radius;
                            float3 fsAqwIkSbyxhTJNsaoWvHIZHSXESfVRF = VMUHrPqbpLgInrkjWKsTlIWrTryOYzPA.Position + jnzBzyYqdqMfHqlLTjbPFaurVvrNiozF; 

                            int tpWznFNUdeMdRcnhwAuqlyxoFikKWUJj = AcBrTMmubprGIOuZYibtwUPGAJRIlqVa + hMAgHPuPGNvnqRRHjdChWlFguhTOhEYe;
                            Vertices[tpWznFNUdeMdRcnhwAuqlyxoFikKWUJj] = fsAqwIkSbyxhTJNsaoWvHIZHSXESfVRF;

                            
                            Normals[tpWznFNUdeMdRcnhwAuqlyxoFikKWUJj] = math.normalize(SmoothPath[SmoothPath.Length - 1].Tangent);

                            
                            var sZIlFyJGVPCZXSMLlNRoPOlDRRxZZsCD = math.remap(0, 1f, UVRange.x, UVRange.y, 1.0f);
                            UVs[tpWznFNUdeMdRcnhwAuqlyxoFikKWUJj] = new float2(sZIlFyJGVPCZXSMLlNRoPOlDRRxZZsCD, (float)hMAgHPuPGNvnqRRHjdChWlFguhTOhEYe / Slices);
                        }

                        
                        for (int uniMEkTOpSiEqKNLGorSGuihCuZHdHog = 0; uniMEkTOpSiEqKNLGorSGuihCuZHdHog < Slices - 2; uniMEkTOpSiEqKNLGorSGuihCuZHdHog++)
                        {
                            Indices[(uniMEkTOpSiEqKNLGorSGuihCuZHdHog * 3) + FLBNbkkCteYeyKpbgcaBAuLNIludKxvb] = 0 + AcBrTMmubprGIOuZYibtwUPGAJRIlqVa;
                            Indices[(uniMEkTOpSiEqKNLGorSGuihCuZHdHog * 3) + 1 + FLBNbkkCteYeyKpbgcaBAuLNIludKxvb] = uniMEkTOpSiEqKNLGorSGuihCuZHdHog + 2 + AcBrTMmubprGIOuZYibtwUPGAJRIlqVa;
                            Indices[(uniMEkTOpSiEqKNLGorSGuihCuZHdHog * 3) + 2 + FLBNbkkCteYeyKpbgcaBAuLNIludKxvb] = uniMEkTOpSiEqKNLGorSGuihCuZHdHog + 1 + AcBrTMmubprGIOuZYibtwUPGAJRIlqVa;
                        }
                    }
                }
            }
        }

        
        private struct fpnPzvOaxRDnvGBQsMjhBEZZWUgRXRSA
        {
            public int StartControlPointIndex;
            public int ControlPointCount;
            public float ControlPointUVStart;
            public float ControlPointUVEnd;

            public override string ToString()
            {
                return $"StartControlPointIndex: {StartControlPointIndex}, ControlPointCount: {ControlPointCount}, ControlPointUVStart: {ControlPointUVStart}, ControlPointUVEnd: {ControlPointUVEnd}";
            }
        }
    }
}
