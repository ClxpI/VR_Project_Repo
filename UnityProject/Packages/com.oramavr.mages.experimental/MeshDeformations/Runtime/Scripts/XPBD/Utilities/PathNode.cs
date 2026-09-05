namespace MAGES.MeshDeformations
{
    using Unity.Burst;
    using Unity.Mathematics;

    [BurstCompile]
    internal struct PathNode
    {
        public float3 Position;
        public float3 Tangent;
        public float3 Normal;
        public float3 Binormal;

        public PathNode(float3 position)
        {
            Position = position;
            Tangent = float3.zero;
            Normal = float3.zero;
            Binormal = float3.zero;
        }

        public PathNode(float3 position, float3 tangent)
        {
            Position = position;
            Tangent = tangent;
            Normal = float3.zero;
            Binormal = float3.zero;
        }

        public PathNode(float3 position, float3 tangent, float3 normal, float3 binormal)
        {
            Position = position;
            Tangent = tangent;
            Normal = normal;
            Binormal = binormal;
        }

        public override string ToString()
        {
            return $"Position: {Position}, Tangent: {Tangent}, Normal: {Normal}, Binormal: {Binormal}";
        }
    }
}
