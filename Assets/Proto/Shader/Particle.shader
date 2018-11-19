Shader "Custom/Particle"
{
	Properties
	{
		_Color("Color", COLOR) = (1, 1, 1, 1)
		_MinDist("min dist", Range(0, 3)) = 1
		_MaxDist("max dist", Range(3, 5)) = 4
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100
		Blend SrcAlpha One
		ZWrite Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed alpha: TEXCOORD0;
			};
			half _MinDist;
			half _MaxDist;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				// float3 worCamPos = _WorldSpaceCameraPos;

				// o.alpha = .1;
				half fadeDist = _MaxDist - _MinDist;
				o.alpha = saturate((o.vertex.z - _MinDist)/ fadeDist);
				return o;
			}
			fixed4 _Color;
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = _Color;
				col.a *= i.alpha;
				return col;
			}
			ENDCG
		}
	}
}
