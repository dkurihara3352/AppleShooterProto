Shader "Custom/StealthCamo" 
{
	Properties 
	{
		_Colour ("Colour", Color) = (1,1,1,1)

		// _BumpMap ("Noise text", 2D) = "bump" {}
		_Magnitude ("Magnitude", Range(0,1)) = 0.05
	}
	
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Opaque"}

		GrabPass { "_GrabTexture" }
		
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _GrabTexture;

			fixed4 _Colour;

			float  _Magnitude;

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;

				half3 normal: NORMAL;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;

				float4 uvgrab : TEXCOORD1;

				// half3 normal: NORMAL;
				half3 vDotN: TEXCOORD2;
			};

			// Vertex function 
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;

				o.texcoord = v.texcoord;

				o.uvgrab = ComputeGrabScreenPos(o.vertex);

				half3 viewDir = WorldSpaceViewDir(v.vertex);
				o.vDotN = dot(normalize(viewDir), v.normal);
				// o.normal = v.normal;
				
				return o;
			}

			// Fragment function
			half4 frag (v2f i) : COLOR
			{
				// half4 mainColour = tex2D(_MainTex, i.texcoord);
				
				// half4 bump = tex2D(_BumpMap, i.texcoord);
				// half2 distortion = UnpackNormal(bump).rg;

				// half2 correctedNormal = i.normal * .5 + .5;
				half2 distortion = _Magnitude * i.vDotN;

				i.uvgrab.xy += distortion * _Magnitude;

				fixed4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));

				return col * (1 - _Colour.a) + _Colour * _Colour.a;
				// return col * mainColour * _Colour;
			}
		
			ENDCG
		} 
	}
}