namespace MAGES.MeshDeformations.Utilities
{
    using System.Runtime.CompilerServices;
    using Unity.Mathematics;

    
    
    
    

    /// <summary>
    /// Library for Singular Value Decomposition.
    /// </summary>
    /// <remarks>Taken from Unity SVD implementation.</remarks>
    internal static class SVD
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void YRgsMsHFGOvvrOEnQWKWOBHpJzOeHPKb(bool c, ref float x, ref float y)
        {
            var cTIlQmcwMaZtaxgCEIBrqNyfNZeCTFSo = x;
            x = math.select(x, y, c);
            y = math.select(y, cTIlQmcwMaZtaxgCEIBrqNyfNZeCTFSo, c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void yMOEKTIWUXDAHTMIWXlPbEuAQGbdAllP(bool c, ref float3 x, ref float3 y)
        {
            var OxDfvkWBkxCclpwZtUcopWVRSQdAWMTl = -x;
            x = math.select(x, y, c);
            y = math.select(y, OxDfvkWBkxCclpwZtUcopWVRSQdAWMTl, c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static quaternion SJaqiBleQZZJxHVyeHPsDBmjNgMBDPBH(bool c, quaternion q, float4 mask)
        {
            const float SHFOSEKmsdzfxjOzHtxpBelxijVrPsOJ = 0.707106781186548f;
            return math.mul(q, math.select(quaternion.identity.value, mask * SHFOSEKmsdzfxjOzHtxpBelxijVrPsOJ, c));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void UATnoBghuavStcNrnJprWpAUyLOUlIPB(ref float3x3 b, ref quaternion v)
        {
            var diLgttqVISsxCzEoFCqhvqzlQTScFEFo = math.lengthsq(b.c0);
            var bMReQnjnqRmmkwaLoxmoWxpKgsaPnFQt = math.lengthsq(b.c1);
            var VBIBQyVVGqKIWeuKQGgKjrPnAchifZMm = math.lengthsq(b.c2);

            var XKLvufRSeQAIqhjhgDHlNIKAnBkGGPug = diLgttqVISsxCzEoFCqhvqzlQTScFEFo < bMReQnjnqRmmkwaLoxmoWxpKgsaPnFQt;
            yMOEKTIWUXDAHTMIWXlPbEuAQGbdAllP(XKLvufRSeQAIqhjhgDHlNIKAnBkGGPug, ref b.c0, ref b.c1);
            v = SJaqiBleQZZJxHVyeHPsDBmjNgMBDPBH(XKLvufRSeQAIqhjhgDHlNIKAnBkGGPug, v, math.float4(0f, 0f, 1f, 1f));
            YRgsMsHFGOvvrOEnQWKWOBHpJzOeHPKb(XKLvufRSeQAIqhjhgDHlNIKAnBkGGPug, ref diLgttqVISsxCzEoFCqhvqzlQTScFEFo, ref bMReQnjnqRmmkwaLoxmoWxpKgsaPnFQt);

            XKLvufRSeQAIqhjhgDHlNIKAnBkGGPug = diLgttqVISsxCzEoFCqhvqzlQTScFEFo < VBIBQyVVGqKIWeuKQGgKjrPnAchifZMm;
            yMOEKTIWUXDAHTMIWXlPbEuAQGbdAllP(XKLvufRSeQAIqhjhgDHlNIKAnBkGGPug, ref b.c0, ref b.c2);
            v = SJaqiBleQZZJxHVyeHPsDBmjNgMBDPBH(XKLvufRSeQAIqhjhgDHlNIKAnBkGGPug, v, math.float4(0f, -1f, 0f, 1f));
            YRgsMsHFGOvvrOEnQWKWOBHpJzOeHPKb(XKLvufRSeQAIqhjhgDHlNIKAnBkGGPug, ref diLgttqVISsxCzEoFCqhvqzlQTScFEFo, ref VBIBQyVVGqKIWeuKQGgKjrPnAchifZMm);

            XKLvufRSeQAIqhjhgDHlNIKAnBkGGPug = bMReQnjnqRmmkwaLoxmoWxpKgsaPnFQt < VBIBQyVVGqKIWeuKQGgKjrPnAchifZMm;
            yMOEKTIWUXDAHTMIWXlPbEuAQGbdAllP(XKLvufRSeQAIqhjhgDHlNIKAnBkGGPug, ref b.c1, ref b.c2);
            v = SJaqiBleQZZJxHVyeHPsDBmjNgMBDPBH(XKLvufRSeQAIqhjhgDHlNIKAnBkGGPug, v, math.float4(1f, 0f, 0f, 1f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static quaternion unaELhaIRVYbCsmiylBtBKbfThfklsgi(float3 pq, float4 mask)
        {
            const float gLrKDaeFbHCzUmvlMHEXJFMlOuTuWUsm = 0.923879532511287f; 
            const float kinzBYhYIcvWBVYtfPrhGRbqPhzYgFfN = 0.38268343236509f; 
            const float gBjdVZcEJhuUeLSrJSjRbIrrCbasfCjd = 5.82842712474619f; 

            var ZtNQcjslBbIiYMFbkFHnnVQAFqvNtbQb = 2f * (pq.x - pq.y); 
            var ORahBqpGCeusILpcYRGrZUCvIavHuUUu = pq.z; 
            var FEPjpHrfNxCXsgWfXkeWrYekXYmJxNwy = math.select(math.float4(kinzBYhYIcvWBVYtfPrhGRbqPhzYgFfN, kinzBYhYIcvWBVYtfPrhGRbqPhzYgFfN, kinzBYhYIcvWBVYtfPrhGRbqPhzYgFfN, gLrKDaeFbHCzUmvlMHEXJFMlOuTuWUsm), math.float4(ORahBqpGCeusILpcYRGrZUCvIavHuUUu, ORahBqpGCeusILpcYRGrZUCvIavHuUUu, ORahBqpGCeusILpcYRGrZUCvIavHuUUu, ZtNQcjslBbIiYMFbkFHnnVQAFqvNtbQb), gBjdVZcEJhuUeLSrJSjRbIrrCbasfCjd * ORahBqpGCeusILpcYRGrZUCvIavHuUUu * ORahBqpGCeusILpcYRGrZUCvIavHuUUu < ZtNQcjslBbIiYMFbkFHnnVQAFqvNtbQb * ZtNQcjslBbIiYMFbkFHnnVQAFqvNtbQb) * mask;
            return math.normalize(FEPjpHrfNxCXsgWfXkeWrYekXYmJxNwy);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static quaternion xdMhzCZgXamZzbiuSuyUTsqXdyXAgcyi(float2 pq, float4 mask)
        {
            var uVWbfPLCdHvovjDCfNNdjpbuAWHCzdZk = math.sqrt((pq.x * pq.x) + (pq.y * pq.y));
            var YcTmXhXLxXqkbaQrGUDnjqRcJOLnuSMK = math.select(0f, pq.y, uVWbfPLCdHvovjDCfNNdjpbuAWHCzdZk > k_EpsilonNormalSqrt);
            var tYHvjtDhRhOdkWmbEIPIyOzeIpWvgFAj = math.abs(pq.x) + math.max(uVWbfPLCdHvovjDCfNNdjpbuAWHCzdZk, k_EpsilonNormalSqrt);
            YRgsMsHFGOvvrOEnQWKWOBHpJzOeHPKb(pq.x < 0f, ref YcTmXhXLxXqkbaQrGUDnjqRcJOLnuSMK, ref tYHvjtDhRhOdkWmbEIPIyOzeIpWvgFAj);

            return math.normalize(math.float4(YcTmXhXLxXqkbaQrGUDnjqRcJOLnuSMK, YcTmXhXLxXqkbaQrGUDnjqRcJOLnuSMK, YcTmXhXLxXqkbaQrGUDnjqRcJOLnuSMK, tYHvjtDhRhOdkWmbEIPIyOzeIpWvgFAj) * mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static quaternion whvljRfrnvLFVrslSRQWZoKmZHVlrWin(float3x3 b, out float3x3 r)
        {
            var zzdgIShdsNFmeRDTbhCoSCCzUVcJRkEP = xdMhzCZgXamZzbiuSuyUTsqXdyXAgcyi(math.float2(b.c0.x, b.c0.y), math.float4(0f, 0f, 1f, 1f));
            var sICHDfQSquqIFqmtSNNsKTDxfUXyDjtX = math.float3x3(math.conjugate(zzdgIShdsNFmeRDTbhCoSCCzUVcJRkEP));
            r = math.mul(sICHDfQSquqIFqmtSNNsKTDxfUXyDjtX, b);

            var aQnynYshZxkboHOwWMCoFEMumRarfpmW = xdMhzCZgXamZzbiuSuyUTsqXdyXAgcyi(math.float2(r.c0.x, r.c0.z), math.float4(0f, -1f, 0f, 1f));
            zzdgIShdsNFmeRDTbhCoSCCzUVcJRkEP = math.mul(zzdgIShdsNFmeRDTbhCoSCCzUVcJRkEP, aQnynYshZxkboHOwWMCoFEMumRarfpmW);
            sICHDfQSquqIFqmtSNNsKTDxfUXyDjtX = math.float3x3(math.conjugate(aQnynYshZxkboHOwWMCoFEMumRarfpmW));
            r = math.mul(sICHDfQSquqIFqmtSNNsKTDxfUXyDjtX, r);

            aQnynYshZxkboHOwWMCoFEMumRarfpmW = xdMhzCZgXamZzbiuSuyUTsqXdyXAgcyi(math.float2(r.c1.y, r.c1.z), math.float4(1f, 0f, 0f, 1f));
            zzdgIShdsNFmeRDTbhCoSCCzUVcJRkEP = math.mul(zzdgIShdsNFmeRDTbhCoSCCzUVcJRkEP, aQnynYshZxkboHOwWMCoFEMumRarfpmW);
            sICHDfQSquqIFqmtSNNsKTDxfUXyDjtX = math.float3x3(math.conjugate(aQnynYshZxkboHOwWMCoFEMumRarfpmW));
            r = math.mul(sICHDfQSquqIFqmtSNNsKTDxfUXyDjtX, r);

            return zzdgIShdsNFmeRDTbhCoSCCzUVcJRkEP;
        }

        static quaternion VaZqdqeTSMriMefYxjhGgQNuNZeJJtMF(ref float3x3 s, int iterations = 5)
        {
            float3x3 PYmUTTHuekPbGiZezDVBYQJKVPpDnCmk;
            quaternion fGvDgzGcjXOaYBpiHdzdaLtXaiqDvnwW;
            quaternion JVqymETAdSAPcPTgCWPtBqfsDPVYDtvl = quaternion.identity;

            for (int VMITvBtvfGcbSmuPjuxkuDtfkQyliogs = 0; VMITvBtvfGcbSmuPjuxkuDtfkQyliogs < iterations; ++VMITvBtvfGcbSmuPjuxkuDtfkQyliogs)
            {
                fGvDgzGcjXOaYBpiHdzdaLtXaiqDvnwW = unaELhaIRVYbCsmiylBtBKbfThfklsgi(math.float3(s.c0.x, s.c1.y, s.c0.y), math.float4(0f, 0f, 1f, 1f));
                JVqymETAdSAPcPTgCWPtBqfsDPVYDtvl = math.mul(JVqymETAdSAPcPTgCWPtBqfsDPVYDtvl, fGvDgzGcjXOaYBpiHdzdaLtXaiqDvnwW);
                PYmUTTHuekPbGiZezDVBYQJKVPpDnCmk = math.float3x3(fGvDgzGcjXOaYBpiHdzdaLtXaiqDvnwW);
                s = math.mul(math.mul(math.transpose(PYmUTTHuekPbGiZezDVBYQJKVPpDnCmk), s), PYmUTTHuekPbGiZezDVBYQJKVPpDnCmk);

                fGvDgzGcjXOaYBpiHdzdaLtXaiqDvnwW = unaELhaIRVYbCsmiylBtBKbfThfklsgi(math.float3(s.c1.y, s.c2.z, s.c1.z), math.float4(1f, 0f, 0f, 1f));
                JVqymETAdSAPcPTgCWPtBqfsDPVYDtvl = math.mul(JVqymETAdSAPcPTgCWPtBqfsDPVYDtvl, fGvDgzGcjXOaYBpiHdzdaLtXaiqDvnwW);
                PYmUTTHuekPbGiZezDVBYQJKVPpDnCmk = math.float3x3(fGvDgzGcjXOaYBpiHdzdaLtXaiqDvnwW);
                s = math.mul(math.mul(math.transpose(PYmUTTHuekPbGiZezDVBYQJKVPpDnCmk), s), PYmUTTHuekPbGiZezDVBYQJKVPpDnCmk);

                fGvDgzGcjXOaYBpiHdzdaLtXaiqDvnwW = unaELhaIRVYbCsmiylBtBKbfThfklsgi(math.float3(s.c2.z, s.c0.x, s.c2.x), math.float4(0f, 1f, 0f, 1f));
                JVqymETAdSAPcPTgCWPtBqfsDPVYDtvl = math.mul(JVqymETAdSAPcPTgCWPtBqfsDPVYDtvl, fGvDgzGcjXOaYBpiHdzdaLtXaiqDvnwW);
                PYmUTTHuekPbGiZezDVBYQJKVPpDnCmk = math.float3x3(fGvDgzGcjXOaYBpiHdzdaLtXaiqDvnwW);
                s = math.mul(math.mul(math.transpose(PYmUTTHuekPbGiZezDVBYQJKVPpDnCmk), s), PYmUTTHuekPbGiZezDVBYQJKVPpDnCmk);
            }

            return JVqymETAdSAPcPTgCWPtBqfsDPVYDtvl;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float3 dGbvgXXmRitfyLTOlOYNcnMavuZiThVk(float3x3 a, out quaternion u, out quaternion v)
        {
            u = quaternion.identity;
            v = quaternion.identity;

            var GLcHKASuCuLGXwxYjyIooFZNBdlOuLhB = math.mul(math.transpose(a), a);
            v = VaZqdqeTSMriMefYxjhGgQNuNZeJJtMF(ref GLcHKASuCuLGXwxYjyIooFZNBdlOuLhB);
            var ShvRHdweOksivfPlgBXlYuofLZiQkjSW = math.float3x3(v);
            ShvRHdweOksivfPlgBXlYuofLZiQkjSW = math.mul(a, ShvRHdweOksivfPlgBXlYuofLZiQkjSW);
            UATnoBghuavStcNrnJprWpAUyLOUlIPB(ref ShvRHdweOksivfPlgBXlYuofLZiQkjSW, ref v);
            u = whvljRfrnvLFVrslSRQWZoKmZHVlrWin(ShvRHdweOksivfPlgBXlYuofLZiQkjSW, out var e);

            return math.float3(e.c0.x, e.c1.y, e.c2.z);
        }

        public const float k_EpsilonDeterminant = 1e-6f;
        public const float k_EpsilonRCP = 1e-9f;
        public const float k_EpsilonNormalSqrt = 1e-15f;
        public const float k_EpsilonNormal = 1e-30f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float3 uuICPIXGofSEZkzmJxrSSZJEhiJofoJU(float3 x, float epsilon = k_EpsilonRCP) =>
            math.select(math.rcp(x), float3.zero, math.abs(x) < epsilon);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float3x3 SVDInverse(float3x3 a)
        {
            var ceIkgsXwsaQPocKANmRtyWTZoUomBGoJ = dGbvgXXmRitfyLTOlOYNcnMavuZiThVk(a, out var u, out var v);
            var cBdwvSDmGMLarntiOrTXndfbntDeeTFC = math.float3x3(u);
            var KbaVrlTNKjloiFRoYWYwWeouaDJCTcVK = math.float3x3(v);


            return math.mul(KbaVrlTNKjloiFRoYWYwWeouaDJCTcVK, ScaleMul(uuICPIXGofSEZkzmJxrSSZJEhiJofoJU(ceIkgsXwsaQPocKANmRtyWTZoUomBGoJ, k_EpsilonDeterminant), math.transpose(cBdwvSDmGMLarntiOrTXndfbntDeeTFC)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static quaternion SVDRotation(float3x3 a)
        {
            dGbvgXXmRitfyLTOlOYNcnMavuZiThVk(a, out var u, out var v);
            return math.mul(u, math.conjugate(v));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float3x3 ScaleMul(float3 s, float3x3 m) => new float3x3(m.c0 * s, m.c1 * s, m.c2 * s);
    }
}