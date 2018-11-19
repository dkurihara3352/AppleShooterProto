// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/VFShaderTest"
{
	Properties
	{
		_Color ("Color", Color) = (1, .2, 1, 1)
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
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				half3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half3 worldRefl: TEXCOORD0;
			};

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				half3 worldSpaceViewDir = UnityWorldSpaceViewDir(worldPos);
				half3 normalizedWorldSpaceViewDir = normalize(worldSpaceViewDir);
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldRefl = reflect(-normalizedWorldSpaceViewDir, worldNormal);
				return o;
			}
			fixed4 _Color;
			fixed4 frag (v2f i) : SV_Target
			{
				half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, i.worldRefl);
				half3 skyColor = DecodeHDR(skyData, unity_SpecCube0_HDR);

				fixed4 c = 0;
				c.rgb = skyColor;
				return c;
			}
			ENDCG
		}
	}
}
