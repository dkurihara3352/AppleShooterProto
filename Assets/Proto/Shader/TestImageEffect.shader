Shader "Hidden/TestImageEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BWBlend("BlackWhite blend", Range(0, 1)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			
			sampler2D _MainTex;
			fixed _BWBlend;

			fixed4 frag (v2f_img i) : COLOR
			{
				fixed4 c = tex2D(_MainTex, i.uv);
				fixed lum = Luminance(c.rgb);
				fixed3 bw = (lum, lum, lum);
				fixed4 result = c;
				result.rgb = lerp(c.rgb, bw, _BWBlend);
				return result;
			}
			ENDCG
		}
	}
}