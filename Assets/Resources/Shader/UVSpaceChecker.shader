Shader "Unlit/UVSpaceChecker"
{
	Properties
	{
		_Density("Density", Range(2, 50)) = 30
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
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			float _Density;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv * _Density;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed col = 0;
				float2 intPartByTwo = floor(i.uv) /2;
				float2 fracPart = frac(intPartByTwo.x + intPartByTwo.y);
				float2 colSpace = fracPart *2;
				col = colSpace;
				return col;
			}
			ENDCG
		}
	}
}
