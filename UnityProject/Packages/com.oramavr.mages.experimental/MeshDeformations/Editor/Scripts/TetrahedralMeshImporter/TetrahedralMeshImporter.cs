namespace MAGES.MeshDeformations
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Unity.Mathematics;
    using UnityEditor.AssetImporters;
    using UnityEngine;

    /// <summary>
    /// A custom importer for tetrahedral meshes.
    /// </summary>
    [ScriptedImporter(0, "msh")]
    public class TetrahedralMeshImporter : ScriptedImporter
    {
        /// <summary>
        /// Gets or sets the scale to be applied to the mesh.
        /// </summary>
        [field: Header("Mesh Options")]
        [field: Tooltip("Scale to be applied to the mesh.")]
        [field: SerializeField]
        public float ScaleFactor { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets a value indicating whether to convert file units to Unity ones.
        /// </summary>
        [field: Tooltip("Convert file units to Unity ones. 1cm (File) to 0.01m (Unity)")]
        [field: SerializeField]
        public bool ConvertUnits { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to import the submeshes as well.
        /// </summary>
        [field: Tooltip("Imports the submeshes as well. If disabled, all tetrahedra will be in the same submesh.")]
        [field: SerializeField]
        public bool ImportSubMeshes { get; set; } = true;

        /// <inheritdoc/>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var VOYhxrivMhqyyOjNRcVyndvKMCCFWCFn = ScriptableObject.CreateInstance<TetrahedralMesh>();
            ctx.AddObjectToAsset("main obj", VOYhxrivMhqyyOjNRcVyndvKMCCFWCFn);
            ctx.SetMainObject(VOYhxrivMhqyyOjNRcVyndvKMCCFWCFn);

            float3[] bKlvJfLgSzUewjEgmdhTiSKtmKGofpaO;
            vGHjJgVMofocRoFoJmOHyEtEUXUcUdEv[] MyIRvxEyLFRHqiIQJKwCPpOBqPVLuLvW;

            string XFhGDqzaRSCurUZAhoIFAPsnhjgTUaXL = System.IO.File.ReadAllText(ctx.assetPath);

            if (!JJiInInCrgQijMVOAvkqyNCwGEEYikkE(XFhGDqzaRSCurUZAhoIFAPsnhjgTUaXL, out string _))
            {
                Debug.LogError("Unsupported mesh format version.");
                return;
            }

            if (!gsiYWGKVGbQqTeqhxVGRnnSoixeRQhhx(XFhGDqzaRSCurUZAhoIFAPsnhjgTUaXL, out bKlvJfLgSzUewjEgmdhTiSKtmKGofpaO))
            {
                Debug.LogError("Failed to parse vertices.");
                return;
            }

            if (!almtcYTeaHlKaHFjwaEgkGMIfBUUiYJZ(XFhGDqzaRSCurUZAhoIFAPsnhjgTUaXL, out MyIRvxEyLFRHqiIQJKwCPpOBqPVLuLvW))
            {
                Debug.LogError("Failed to parse tetrahedra.");
                return;
            }

            int[] QegFtqaCFBnYTHxqzdsZnBaotcNsOwAN = new int[MyIRvxEyLFRHqiIQJKwCPpOBqPVLuLvW.Length * 4];
            if (ImportSubMeshes)
            {
                Dictionary<int, List<int>> YjYYqqHAEmrqygOIfuHXuPTTpzfpYsHk = new Dictionary<int, List<int>>();

                foreach (var tet in MyIRvxEyLFRHqiIQJKwCPpOBqPVLuLvW)
                {
                    if (!YjYYqqHAEmrqygOIfuHXuPTTpzfpYsHk.ContainsKey(tet.ElementaryEntity))
                    {
                        YjYYqqHAEmrqygOIfuHXuPTTpzfpYsHk[tet.ElementaryEntity] = new List<int>();
                    }

                    YjYYqqHAEmrqygOIfuHXuPTTpzfpYsHk[tet.ElementaryEntity].Add(tet.Verts[0]);
                    YjYYqqHAEmrqygOIfuHXuPTTpzfpYsHk[tet.ElementaryEntity].Add(tet.Verts[2]);
                    YjYYqqHAEmrqygOIfuHXuPTTpzfpYsHk[tet.ElementaryEntity].Add(tet.Verts[1]);
                    YjYYqqHAEmrqygOIfuHXuPTTpzfpYsHk[tet.ElementaryEntity].Add(tet.Verts[3]);
                }

                var vmWfeMrftzJEBUSTQyNTctltfVUgQWbE = YjYYqqHAEmrqygOIfuHXuPTTpzfpYsHk.Keys.ToList();
                vmWfeMrftzJEBUSTQyNTctltfVUgQWbE.Sort();

                VOYhxrivMhqyyOjNRcVyndvKMCCFWCFn.SubMeshCount = YjYYqqHAEmrqygOIfuHXuPTTpzfpYsHk.Count;

                var EieXhHJFYRScekgzpyJmchfCiMHZpcrF = 0;
                for (int xLcVgneuBraAeOSzfjEcklUKLnWtHYCy = 0; xLcVgneuBraAeOSzfjEcklUKLnWtHYCy < vmWfeMrftzJEBUSTQyNTctltfVUgQWbE.Count; xLcVgneuBraAeOSzfjEcklUKLnWtHYCy++)
                {
                    VOYhxrivMhqyyOjNRcVyndvKMCCFWCFn.SetSubMesh(xLcVgneuBraAeOSzfjEcklUKLnWtHYCy, new TetrahedralSubMeshDescriptor
                    {
                        IndexStart = EieXhHJFYRScekgzpyJmchfCiMHZpcrF,
                        IndexCount = YjYYqqHAEmrqygOIfuHXuPTTpzfpYsHk[vmWfeMrftzJEBUSTQyNTctltfVUgQWbE[xLcVgneuBraAeOSzfjEcklUKLnWtHYCy]].Count,
                    });

                    foreach (var vert in YjYYqqHAEmrqygOIfuHXuPTTpzfpYsHk[vmWfeMrftzJEBUSTQyNTctltfVUgQWbE[xLcVgneuBraAeOSzfjEcklUKLnWtHYCy]])
                    {
                        QegFtqaCFBnYTHxqzdsZnBaotcNsOwAN[EieXhHJFYRScekgzpyJmchfCiMHZpcrF++] = vert;
                    }
                }
            }
            else
            {
                var mUhNHKDTHDrBxAdqbJOtPjGklAhlkaoc = 0;
                foreach (var tet in MyIRvxEyLFRHqiIQJKwCPpOBqPVLuLvW)
                {
                    for (int qnqUprXYijjIenGtttKdUmsLLnUfrXpH = 0; qnqUprXYijjIenGtttKdUmsLLnUfrXpH < 4; qnqUprXYijjIenGtttKdUmsLLnUfrXpH++)
                    {
                        QegFtqaCFBnYTHxqzdsZnBaotcNsOwAN[mUhNHKDTHDrBxAdqbJOtPjGklAhlkaoc++] = tet.Verts[qnqUprXYijjIenGtttKdUmsLLnUfrXpH];
                    }
                }
            }

            VOYhxrivMhqyyOjNRcVyndvKMCCFWCFn.SetVertices(bKlvJfLgSzUewjEgmdhTiSKtmKGofpaO);
            VOYhxrivMhqyyOjNRcVyndvKMCCFWCFn.SetTetrahedra(QegFtqaCFBnYTHxqzdsZnBaotcNsOwAN);
        }

        private string WMxNuUshVTSzAPtpulqGMnsxFSetZBYw(string fileContent, string section)
        {
            int EkbidSWFQIumwMOXlrHLYUiLilemHaCp = fileContent.IndexOf($"${section}");
            int BdFwEEjLDYOcRotpGYLwlIlbwllChlmz = fileContent.IndexOf($"$End{section}");

            if (EkbidSWFQIumwMOXlrHLYUiLilemHaCp == -1 || BdFwEEjLDYOcRotpGYLwlIlbwllChlmz == -1)
            {
                return null;
            }

            EkbidSWFQIumwMOXlrHLYUiLilemHaCp += section.Length + 1;

            return fileContent.Substring(EkbidSWFQIumwMOXlrHLYUiLilemHaCp, BdFwEEjLDYOcRotpGYLwlIlbwllChlmz - EkbidSWFQIumwMOXlrHLYUiLilemHaCp).Trim().Trim('\n');
        }

        private bool JJiInInCrgQijMVOAvkqyNCwGEEYikkE(string fileContent, out string fileVersion)
        {
            fileVersion = WMxNuUshVTSzAPtpulqGMnsxFSetZBYw(fileContent, "MeshFormat");

            
            if (!fileVersion.StartsWith("2"))
            {
                return false;
            }

            return true;
        }

        private bool gsiYWGKVGbQqTeqhxVGRnnSoixeRQhhx(string content, out float3[] verts)
        {
            string EcIOTlZlnlhkmAcJKdLTtvLlvtDIqPND = WMxNuUshVTSzAPtpulqGMnsxFSetZBYw(content, "Nodes");

            if (string.IsNullOrEmpty(EcIOTlZlnlhkmAcJKdLTtvLlvtDIqPND))
            {
                Debug.Log("Could not find vertices section.");
                verts = null;
                return false;
            }

            
            string[] VcoajCoMWnGdbxvFWKqmSNWPzcolQjkW = EcIOTlZlnlhkmAcJKdLTtvLlvtDIqPND.Split('\n');

            int laHzhxhHsRooajzWorwTanmHTUjBeWJA = int.Parse(VcoajCoMWnGdbxvFWKqmSNWPzcolQjkW[0]);

            verts = new float3[laHzhxhHsRooajzWorwTanmHTUjBeWJA];

            float4x4 iCenLihAaTcheXnYRSYjvvMHWKGNuEMG = float4x4.Scale(ScaleFactor * (ConvertUnits ? 0.01f : 1f));

            foreach (var line in VcoajCoMWnGdbxvFWKqmSNWPzcolQjkW)
            {
                var TDhRHyssfFZXejOaZQEBcOffJhRkgqFC = LcWTsaRNQjvCXnxibTdvHJodBalghAoB(line, out float3 val);
                if (TDhRHyssfFZXejOaZQEBcOffJhRkgqFC == -1)
                {
                    continue;
                }

                verts[TDhRHyssfFZXejOaZQEBcOffJhRkgqFC - 1] = math.mul(iCenLihAaTcheXnYRSYjvvMHWKGNuEMG, new float4(val, 1.0f)).xyz;
            }

            return true;
        }

        private bool almtcYTeaHlKaHFjwaEgkGMIfBUUiYJZ(string content, out vGHjJgVMofocRoFoJmOHyEtEUXUcUdEv[] tets)
        {
            string JmRyUrJTaqgfMatBntXnXFcYcCDjdPgj = WMxNuUshVTSzAPtpulqGMnsxFSetZBYw(content, "Elements");

            if (string.IsNullOrEmpty(JmRyUrJTaqgfMatBntXnXFcYcCDjdPgj))
            {
                tets = null;
                return false;
            }

            
            string[] XKacsZKCuMhNocgqwXkQdzEigAlMDXRx = JmRyUrJTaqgfMatBntXnXFcYcCDjdPgj.Split('\n');

            int YDQBIWeTgxcIYIoSjrJBVGxjNGFYhilF = int.Parse(XKacsZKCuMhNocgqwXkQdzEigAlMDXRx[0]);

            var qqwFPEXPvObmaTNbctxhpgYHhstGAFbJ = new List<vGHjJgVMofocRoFoJmOHyEtEUXUcUdEv>();
            foreach (var line in XKacsZKCuMhNocgqwXkQdzEigAlMDXRx)
            {
                var urWJvMcnuhxvgezhkxILQKGoOTknBkPV = CKizchRZoGLnCBfqnsrfdivqMHAluqkd(line, out int elementType, out int numberOfTags, out int physicalEntity, out int elementaryEntity, out int[] val);
                if (urWJvMcnuhxvgezhkxILQKGoOTknBkPV <= -1 || elementType != 4 || val.Length != 4)
                {
                    continue;
                }

                qqwFPEXPvObmaTNbctxhpgYHhstGAFbJ.Add(new vGHjJgVMofocRoFoJmOHyEtEUXUcUdEv
                {
                    NumberOfTags = numberOfTags,
                    PhysicalEntity = physicalEntity,
                    ElementaryEntity = elementaryEntity,
                    Verts = val,
                });
            }

            
            if (qqwFPEXPvObmaTNbctxhpgYHhstGAFbJ.Count != YDQBIWeTgxcIYIoSjrJBVGxjNGFYhilF)
            {
                Debug.LogWarning($"Some elements could not be processed");
            }

            tets = qqwFPEXPvObmaTNbctxhpgYHhstGAFbJ.ToArray();

            return true;
        }

        
        private int LcWTsaRNQjvCXnxibTdvHJodBalghAoB(string line, out float3 val)
        {
            var WukrpTwZrjyiRYFAMZmpstNMuvmeJyes = line.Split(' ');

            if (WukrpTwZrjyiRYFAMZmpstNMuvmeJyes.Length != 4)
            {
                val = float3.zero;
                return -1;
            }

            val = new float3(float.Parse(WukrpTwZrjyiRYFAMZmpstNMuvmeJyes[1], NumberStyles.Any, CultureInfo.InvariantCulture), float.Parse(WukrpTwZrjyiRYFAMZmpstNMuvmeJyes[2], NumberStyles.Any, CultureInfo.InvariantCulture), float.Parse(WukrpTwZrjyiRYFAMZmpstNMuvmeJyes[3], NumberStyles.Any, CultureInfo.InvariantCulture));
            return int.Parse(WukrpTwZrjyiRYFAMZmpstNMuvmeJyes[0]);
        }

        private int CKizchRZoGLnCBfqnsrfdivqMHAluqkd(string line, out int elementType, out int numberOfTags, out int physicalEntity, out int elementaryEntity, out int[] val)
        {
            var ivcoapYOVYKOxJfALfnmtaERWyctjPpx = line.Split(' ');

            if (ivcoapYOVYKOxJfALfnmtaERWyctjPpx.Length < 5)
            {
                elementType = 0;
                numberOfTags = 0;
                physicalEntity = 0;
                elementaryEntity = 0;
                val = null;
                return -1;
            }

            elementType = int.Parse(ivcoapYOVYKOxJfALfnmtaERWyctjPpx[1]);
            numberOfTags = int.Parse(ivcoapYOVYKOxJfALfnmtaERWyctjPpx[2]);
            physicalEntity = int.Parse(ivcoapYOVYKOxJfALfnmtaERWyctjPpx[3]);
            elementaryEntity = int.Parse(ivcoapYOVYKOxJfALfnmtaERWyctjPpx[4]);

            val = new int[ivcoapYOVYKOxJfALfnmtaERWyctjPpx.Length - 5];
            for (int ZXQGvGlLFiCOyzIEgDGSANbPfWnFUAdA = 5; ZXQGvGlLFiCOyzIEgDGSANbPfWnFUAdA < ivcoapYOVYKOxJfALfnmtaERWyctjPpx.Length; ZXQGvGlLFiCOyzIEgDGSANbPfWnFUAdA++)
            {
                val[ZXQGvGlLFiCOyzIEgDGSANbPfWnFUAdA - 5] = int.Parse(ivcoapYOVYKOxJfALfnmtaERWyctjPpx[ZXQGvGlLFiCOyzIEgDGSANbPfWnFUAdA]) - 1;
            }

            return int.Parse(ivcoapYOVYKOxJfALfnmtaERWyctjPpx[0]);
        }

        private struct vGHjJgVMofocRoFoJmOHyEtEUXUcUdEv
        {
            public int[] Verts;
            public int NumberOfTags;
            public int PhysicalEntity;
            public int ElementaryEntity;
        }
    }
}
