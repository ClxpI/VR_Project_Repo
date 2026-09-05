Shader "Instanced/Particle3D" {
	Properties {
		
	}
	SubShader {

		Tags {"Queue"="Geometry" }

		Pass {

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.5

            #include "UnityCG.cginc"
			
            StructuredBuffer<float3> ParticlesPosition;
            StructuredBuffer<int> ParticlesEnabled;
            StructuredBuffer<uint> ParticlesPhase;
            StructuredBuffer<float> ParticlesInverseMass;


            float scale;

            float4x4 localToWorld;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 colour : TEXCOORD1;
                float3 normal : NORMAL;
            };

            float4 randomColorHSL(unsigned int phase);
            float4 colorFromHSL(float h, float s, float l);
            float hueToRGB(float p, float q, float t);

            v2f vert(appdata_full v, uint instanceID : SV_InstanceID)
            {
				
                float3 centreWorld = ParticlesPosition[instanceID];
                float3 worldVertPos = centreWorld + mul(unity_ObjectToWorld, v.vertex * scale);
                float3 objectVertPos = mul(unity_WorldToObject, float4(worldVertPos.xyz, 1));
                v2f o;
                o.uv = v.texcoord;
                o.normal = v.normal;

                o.pos = UnityObjectToClipPos(objectVertPos);

                float4 pColor = randomColorHSL(ParticlesPhase[instanceID]);
    
                // Check if the particle is enabled
                if (ParticlesEnabled[instanceID] == 0)
                {
                    pColor.a = 0.0f;
                }
                // Check if kinematic
                else if (ParticlesInverseMass[instanceID] < 0.0000001f)
                {
                    pColor.a = 0.4f;
                }
                
                o.colour = pColor;
		
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                //float shading = saturate(dot(_WorldSpaceLightPos0.xyz, i.normal));
                //shading = (shading + 0.6) / 1.4;
				//  //          return float4(i.normal * 0.5 + 0.5, 1);
                //return float4(i.colour * shading, 1);
    
                // Simple Lambertian shading
                //float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                //float NdotL = max(0.1f, dot(i.normal, lightDir));
                //float4 col = float4(i.colour.rgb * NdotL, i.colour.a);
                float multiplier = max(0.1f, i.normal.y);
    
                return float4(multiplier * i.colour.r, multiplier * i.colour.g, multiplier * i.colour.b, i.colour.a);
            }



            float4 randomColorHSL(unsigned int phase)
            {
                float hue = (phase * 0.618033988749895f) % 1.0f;
                float saturation = 0.6f;
                float lightness = 0.7f;
                
                return colorFromHSL(hue, saturation, lightness);
            }

            float4 colorFromHSL(float h, float s, float l)
            {
                float r, g, b;
    
                if(s == 0)
                {
                    r = g = b = l; // achromatic
                }
                else
                {
                    float q = l < 0.5f ? l * (1 + s) : l + s - l * s;
                    float p = 2 * l - q;
                    r = hueToRGB(p, q, h + 1.0f / 3.0f);
                    g = hueToRGB(p, q, h);
                    b = hueToRGB(p, q, h - 1.0f / 3.0f);
                }
                
                return float4(r, g, b, 1.0f);
            }

            float hueToRGB(float p, float q, float t)
            {
                if (t < 0)
                {
                    t += 1;
                }
                if (t > 1)
                {
                    t -= 1;
                }
                if (t < 1.0f / 6.0f)
                {
                    return p + (q - p) * 6.0f * t;
                }
                if (t < 1.0f / 2.0f)
                {
                    return q;
                }
                if (t < 2.0f / 3.0f)
                {
                    return p + (q - p) * (2.0f / 3.0f - t) * 6.0f;
                }
                return p;
            }

            
			ENDCG
		}
	}
}