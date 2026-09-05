namespace MAGES.MeshDeformations.Utilities
{
    using System.Collections.Generic;

    /// <summary>
    /// Utility class for mesh operations.
    /// </summary>
    public static class MeshUtilities
    {
        /// <summary>
        /// Find the neighbors of each tetrahedron in the mesh.
        /// </summary>
        /// <param name="tetrahedra">The tetrahedron indices array.</param>
        /// <returns>The neighbors of the tetrahedra.</returns>
        public static int[] FindTetrahedronNeighbors(in int[] tetrahedra)
        {
            Dictionary<cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU, List<int>> sIymVDaJqpjWLBLRlLphRuGitIClROJV = new Dictionary<cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU, List<int>>();

#if UNITY_EDITOR
            var xeVNuPDdoNfrpdUlTieZAnStCridXFwJ = 0;
            var jQgkhmiEkBVYolXFjGrLwfVKrhDDOnSB = 2 * (tetrahedra.Length / 4);
#endif

            for (int FXEBdcbwmhaiuxJZDXskJCcHrFWuJgrF = 0; FXEBdcbwmhaiuxJZDXskJCcHrFWuJgrF < tetrahedra.Length; FXEBdcbwmhaiuxJZDXskJCcHrFWuJgrF += 4)
            {
                int NKKWqgyyBanRTpPelFeynHwcevbBlJmJ = tetrahedra[FXEBdcbwmhaiuxJZDXskJCcHrFWuJgrF];
                int BMoxLGjvyBmAVINdimEmddcfKFSjiILs = tetrahedra[FXEBdcbwmhaiuxJZDXskJCcHrFWuJgrF + 1];
                int pIrWMQPnYIfJzIcIutfrqbEXDhTTLHwK = tetrahedra[FXEBdcbwmhaiuxJZDXskJCcHrFWuJgrF + 2];
                int pnqsVFavPgZoJbeTSrhbfGppNNmjIKLE = tetrahedra[FXEBdcbwmhaiuxJZDXskJCcHrFWuJgrF + 3];

                cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU EGlgEWgNAiQMznWKLeCzNhrhhtfSgMFj = new cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU(BMoxLGjvyBmAVINdimEmddcfKFSjiILs, pnqsVFavPgZoJbeTSrhbfGppNNmjIKLE, pIrWMQPnYIfJzIcIutfrqbEXDhTTLHwK);
                cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU ZixdRzIeNiJBPnwFrEcNFmHHXqDwyViV = new cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU(NKKWqgyyBanRTpPelFeynHwcevbBlJmJ, pIrWMQPnYIfJzIcIutfrqbEXDhTTLHwK, pnqsVFavPgZoJbeTSrhbfGppNNmjIKLE);
                cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU AjQCJaQzyRkejTEYAUrmwTYPtJKdiNnt = new cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU(NKKWqgyyBanRTpPelFeynHwcevbBlJmJ, pnqsVFavPgZoJbeTSrhbfGppNNmjIKLE, BMoxLGjvyBmAVINdimEmddcfKFSjiILs);
                cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU KRsTifGsIdvRwpoUunguPuEqkYpPGPNc = new cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU(NKKWqgyyBanRTpPelFeynHwcevbBlJmJ, BMoxLGjvyBmAVINdimEmddcfKFSjiILs, pIrWMQPnYIfJzIcIutfrqbEXDhTTLHwK);

                if (!sIymVDaJqpjWLBLRlLphRuGitIClROJV.ContainsKey(EGlgEWgNAiQMznWKLeCzNhrhhtfSgMFj))
                {
                    sIymVDaJqpjWLBLRlLphRuGitIClROJV[EGlgEWgNAiQMznWKLeCzNhrhhtfSgMFj] = new List<int>();
                }

                if (!sIymVDaJqpjWLBLRlLphRuGitIClROJV.ContainsKey(ZixdRzIeNiJBPnwFrEcNFmHHXqDwyViV))
                {
                    sIymVDaJqpjWLBLRlLphRuGitIClROJV[ZixdRzIeNiJBPnwFrEcNFmHHXqDwyViV] = new List<int>();
                }

                if (!sIymVDaJqpjWLBLRlLphRuGitIClROJV.ContainsKey(AjQCJaQzyRkejTEYAUrmwTYPtJKdiNnt))
                {
                    sIymVDaJqpjWLBLRlLphRuGitIClROJV[AjQCJaQzyRkejTEYAUrmwTYPtJKdiNnt] = new List<int>();
                }

                if (!sIymVDaJqpjWLBLRlLphRuGitIClROJV.ContainsKey(KRsTifGsIdvRwpoUunguPuEqkYpPGPNc))
                {
                    sIymVDaJqpjWLBLRlLphRuGitIClROJV[KRsTifGsIdvRwpoUunguPuEqkYpPGPNc] = new List<int>();
                }

                sIymVDaJqpjWLBLRlLphRuGitIClROJV[EGlgEWgNAiQMznWKLeCzNhrhhtfSgMFj].Add(FXEBdcbwmhaiuxJZDXskJCcHrFWuJgrF / 4);
                sIymVDaJqpjWLBLRlLphRuGitIClROJV[ZixdRzIeNiJBPnwFrEcNFmHHXqDwyViV].Add(FXEBdcbwmhaiuxJZDXskJCcHrFWuJgrF / 4);
                sIymVDaJqpjWLBLRlLphRuGitIClROJV[AjQCJaQzyRkejTEYAUrmwTYPtJKdiNnt].Add(FXEBdcbwmhaiuxJZDXskJCcHrFWuJgrF / 4);
                sIymVDaJqpjWLBLRlLphRuGitIClROJV[KRsTifGsIdvRwpoUunguPuEqkYpPGPNc].Add(FXEBdcbwmhaiuxJZDXskJCcHrFWuJgrF / 4);

#if UNITY_EDITOR
                xeVNuPDdoNfrpdUlTieZAnStCridXFwJ++;
                UnityEditor.EditorUtility.DisplayProgressBar("Loading", "Finding Tetrahedron Neighbors", (float)xeVNuPDdoNfrpdUlTieZAnStCridXFwJ / jQgkhmiEkBVYolXFjGrLwfVKrhDDOnSB);
#endif
            }

            int[] lApdGveqjGTolAfMjQUGfiHZryvAkUfm = new int[tetrahedra.Length];

            
            for (int RUsZdiYVIkUODgglRYbWVxXscMrUmslt = 0; RUsZdiYVIkUODgglRYbWVxXscMrUmslt < tetrahedra.Length; RUsZdiYVIkUODgglRYbWVxXscMrUmslt += 4)
            {
                int wmtXJLlEJklcMwOtAZqRdVGYeXDQglZX = RUsZdiYVIkUODgglRYbWVxXscMrUmslt / 4;

                int LbVAkXkfKBsQyPRnPkApwWrilvYMsKNf = tetrahedra[RUsZdiYVIkUODgglRYbWVxXscMrUmslt];
                int YAggsdqHxXPiqbmJugipcIQtluFGVHLt = tetrahedra[RUsZdiYVIkUODgglRYbWVxXscMrUmslt + 1];
                int LYmcYNqISrgePYlEnMuIuhCPvcjueZUs = tetrahedra[RUsZdiYVIkUODgglRYbWVxXscMrUmslt + 2];
                int MwVpDbLTQbcxBTCekoGHEORfasQEpnch = tetrahedra[RUsZdiYVIkUODgglRYbWVxXscMrUmslt + 3];

                cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU zaWCOaviuNiBUJSwbZvOvtJPJALrLXaA = new cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU(YAggsdqHxXPiqbmJugipcIQtluFGVHLt, MwVpDbLTQbcxBTCekoGHEORfasQEpnch, LYmcYNqISrgePYlEnMuIuhCPvcjueZUs);
                cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU HeISHrTiEKbieuTMIMMieqHkgQWsSKeX = new cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU(LbVAkXkfKBsQyPRnPkApwWrilvYMsKNf, LYmcYNqISrgePYlEnMuIuhCPvcjueZUs, MwVpDbLTQbcxBTCekoGHEORfasQEpnch);
                cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU MBMobXYclggjXjjpoXDGdoFdpAUZyNJT = new cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU(LbVAkXkfKBsQyPRnPkApwWrilvYMsKNf, MwVpDbLTQbcxBTCekoGHEORfasQEpnch, YAggsdqHxXPiqbmJugipcIQtluFGVHLt);
                cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU vAcXWWshznsfihcXsBpdFubaFstKaIjD = new cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU(LbVAkXkfKBsQyPRnPkApwWrilvYMsKNf, YAggsdqHxXPiqbmJugipcIQtluFGVHLt, LYmcYNqISrgePYlEnMuIuhCPvcjueZUs);

                
                var sXLgGBiqdhcqjespdVcsCJhTaQygJiOC = sIymVDaJqpjWLBLRlLphRuGitIClROJV[zaWCOaviuNiBUJSwbZvOvtJPJALrLXaA];
                var EHPFruphRKNgVaLdByezRqfVMElzRatd = sIymVDaJqpjWLBLRlLphRuGitIClROJV[HeISHrTiEKbieuTMIMMieqHkgQWsSKeX];
                var iyNkiHQlKnbYUwFjofrTacJiRYRahjhl = sIymVDaJqpjWLBLRlLphRuGitIClROJV[MBMobXYclggjXjjpoXDGdoFdpAUZyNJT];
                var OGNEGaLqxBpoJhhJgZqKvHOvclhSJuQC = sIymVDaJqpjWLBLRlLphRuGitIClROJV[vAcXWWshznsfihcXsBpdFubaFstKaIjD];

                
                var FMELPZVklYmHCJNncPqGwPDYnlyUnGuq = sXLgGBiqdhcqjespdVcsCJhTaQygJiOC.FindIndex(tetIndex => tetIndex != wmtXJLlEJklcMwOtAZqRdVGYeXDQglZX);
                var NWuHWfzMFQjfkStinjtyoNYFXXvWVqPJ = EHPFruphRKNgVaLdByezRqfVMElzRatd.FindIndex(tetIndex => tetIndex != wmtXJLlEJklcMwOtAZqRdVGYeXDQglZX);
                var hNtIYqPgcTRYZZOnbCrudkOoBIeFTXFK = iyNkiHQlKnbYUwFjofrTacJiRYRahjhl.FindIndex(tetIndex => tetIndex != wmtXJLlEJklcMwOtAZqRdVGYeXDQglZX);
                var JcrzknTZITxqJbZFfeaQSxWLCiQUpLTk = OGNEGaLqxBpoJhhJgZqKvHOvclhSJuQC.FindIndex(tetIndex => tetIndex != wmtXJLlEJklcMwOtAZqRdVGYeXDQglZX);

                lApdGveqjGTolAfMjQUGfiHZryvAkUfm[RUsZdiYVIkUODgglRYbWVxXscMrUmslt] = FMELPZVklYmHCJNncPqGwPDYnlyUnGuq == -1 ? -1 : sXLgGBiqdhcqjespdVcsCJhTaQygJiOC[FMELPZVklYmHCJNncPqGwPDYnlyUnGuq];
                lApdGveqjGTolAfMjQUGfiHZryvAkUfm[RUsZdiYVIkUODgglRYbWVxXscMrUmslt + 1] = NWuHWfzMFQjfkStinjtyoNYFXXvWVqPJ == -1 ? -1 : EHPFruphRKNgVaLdByezRqfVMElzRatd[NWuHWfzMFQjfkStinjtyoNYFXXvWVqPJ];
                lApdGveqjGTolAfMjQUGfiHZryvAkUfm[RUsZdiYVIkUODgglRYbWVxXscMrUmslt + 2] = hNtIYqPgcTRYZZOnbCrudkOoBIeFTXFK == -1 ? -1 : iyNkiHQlKnbYUwFjofrTacJiRYRahjhl[hNtIYqPgcTRYZZOnbCrudkOoBIeFTXFK];
                lApdGveqjGTolAfMjQUGfiHZryvAkUfm[RUsZdiYVIkUODgglRYbWVxXscMrUmslt + 3] = JcrzknTZITxqJbZFfeaQSxWLCiQUpLTk == -1 ? -1 : OGNEGaLqxBpoJhhJgZqKvHOvclhSJuQC[JcrzknTZITxqJbZFfeaQSxWLCiQUpLTk];

#if UNITY_EDITOR
                xeVNuPDdoNfrpdUlTieZAnStCridXFwJ++;
                UnityEditor.EditorUtility.DisplayProgressBar("Loading", "Finding Tetrahedron Neighbors", (float)xeVNuPDdoNfrpdUlTieZAnStCridXFwJ / jQgkhmiEkBVYolXFjGrLwfVKrhDDOnSB);
#endif
            }

#if UNITY_EDITOR
            xeVNuPDdoNfrpdUlTieZAnStCridXFwJ++;
            UnityEditor.EditorUtility.ClearProgressBar();
#endif

            return lApdGveqjGTolAfMjQUGfiHZryvAkUfm;
        }

        private struct cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU
        {
            public int A;
            public int B;
            public int C;

            public cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU(int a, int b, int c)
            {
                A = a;
                B = b;
                C = c;
            }

            public override bool Equals(object obj)
            {
                if (obj is not cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU)
                {
                    return false;
                }

                int[] iuIoiUZpBNdIABDEIyMZJLVMqXMbQgwS = { A, B, C };
                System.Array.Sort(iuIoiUZpBNdIABDEIyMZJLVMqXMbQgwS);

                var WWRJiSeBslGuyIQPRhZciannYOSUWYKd = (cVtaYPTMQLQbeRgDWDqWJgXWdHNYgJlU)obj;

                int[] YmMBXVXHzCNeinVOVgXNQJDowaqChYpd = { WWRJiSeBslGuyIQPRhZciannYOSUWYKd.A, WWRJiSeBslGuyIQPRhZciannYOSUWYKd.B, WWRJiSeBslGuyIQPRhZciannYOSUWYKd.C };
                System.Array.Sort(YmMBXVXHzCNeinVOVgXNQJDowaqChYpd);

                return (iuIoiUZpBNdIABDEIyMZJLVMqXMbQgwS[0], iuIoiUZpBNdIABDEIyMZJLVMqXMbQgwS[1], iuIoiUZpBNdIABDEIyMZJLVMqXMbQgwS[2]).Equals((YmMBXVXHzCNeinVOVgXNQJDowaqChYpd[0], YmMBXVXHzCNeinVOVgXNQJDowaqChYpd[1], YmMBXVXHzCNeinVOVgXNQJDowaqChYpd[2]));
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    
                    int[] KebkXCMYFVaTevbqzSVrXyEiYsfHedpg = { A, B, C };
                    System.Array.Sort(KebkXCMYFVaTevbqzSVrXyEiYsfHedpg);

                    return (KebkXCMYFVaTevbqzSVrXyEiYsfHedpg[0], KebkXCMYFVaTevbqzSVrXyEiYsfHedpg[1], KebkXCMYFVaTevbqzSVrXyEiYsfHedpg[2]).GetHashCode();
                }
            }
        }
    }
}