Shader "Unlit/UIShader"
{
	Properties
	{
		_BWBlend("BlackWhite blend", Range(0, 1)) = 0
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Opaque" }
		LOD 100

		GrabPass { "_GrabTexture"}
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
				float4 vertex : SV_POSITION;
				float4 grabUV: TEXCOORD1;
			};

			fixed _BWBlend;
			sampler2D _GrabTexture;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.grabUV = ComputeGrabScreenPos(o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 grabCol = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.grabUV));
				fixed lum = Luminance(grabCol.rgb);
				fixed3 bw = (lum, lum, lum);
				grabCol.rgb = lerp(grabCol, lum, _BWBlend);
				return grabCol;
			}
			ENDCG
		}
	}
}
