Shader "Custom/Particle"
{
	Properties
	{
		_Color("Color", COLOR) = (1, 1, 1, 1)
		_ClearDist("clear dist", Range(0, 10)) = 3
		_FadeDist("fade dist", Range(0, 10)) = 3
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
			half _ClearDist;
			half _FadeDist;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.alpha = saturate((o.vertex.z - _ClearDist)/ _FadeDist);
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
