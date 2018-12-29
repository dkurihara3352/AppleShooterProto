Shader "Custom/HardEdgeNormal" {
SubShader {
    Pass {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"

        // vertex input: position, normal
        struct appdata {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
        };

        struct v2f {
            float4 pos : SV_POSITION;
            fixed4 color : COLOR;
        };
        
        v2f vert (appdata v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex );
			half3 worldNormal = UnityObjectToWorldNormal(v.normal);
            o.color.xyz = v.normal * 0.5 + 0.5;
            // o.color.xyz = worldNormal * 0.5 + 0.5;
            o.color.w = 1.0;
            return o;
        }
        
        fixed4 frag (v2f i) : SV_Target {
			 return i.color; 
			//  float3 x = ddx(i.pos);
			//  float3 y = ddy(i.pos);
			//  float3 normal = normalize(cross(x, y));

			//  fixed4 col = 0;
			//  col.rgb = normal * .5 + .5;
			//  col.a = 1;
			//  return col;
		}
        ENDCG
    }
}
}