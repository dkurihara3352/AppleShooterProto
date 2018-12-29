// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/VertexNormal"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (.5, .5, .5, 1)
		_GlitterThreshold("Glitter Threshold", Range(0, 1)) = .5
		_GlitterColor("Glitter Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				half3 normal: NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				// fixed4 color: COLOR;
				fixed4 glitter: TEXCOORD0;
			};

			half _GlitterThreshold;
			fixed4 _Color;
			fixed4 _GlitterColor;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				half3 viewDir = WorldSpaceViewDir(v.vertex);
				half3 normViewDir = normalize(viewDir);
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half3 normWorldNormal = normalize(worldNormal);
				float vDotN = dot(normViewDir, normWorldNormal);
				
				fixed glitterVal = 1 - vDotN >= _GlitterThreshold? 1: 0;
				o.glitter = _GlitterColor * glitterVal;
				o.glitter.a = 1;
				
				// o.color.rgb = worldNormal * .5 + .5;
				// o.color.a = 1;
				// o.normal = v.normal;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 c = _Color;
				c += i.glitter;
				return c;
			}
			ENDCG
		}
	}
}
