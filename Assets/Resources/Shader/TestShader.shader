Shader "Custom/TestShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_SomeOtherFloat("Some Other Float", Range(0, 1)) = .2
		_BumpMap("Bump", 2D) = "bump"{}
		_RimColor ("Rim Color", Color) = (1, 1, 1, 1)
		_RimPower ("Rim Power", Range(0.5, 8.0)) = 3.0

		_Checker("Checker", 2D) = "white"{}
		_Cube("Cube", Cube) = ""{}
		_Amount("Extrusion Amount", Range(-1, 1)) = 0.5
	}
	// SubShader {
    //   Tags { "RenderType" = "Opaque" }
    //   CGPROGRAM
    //   #pragma surface surf Lambert vertex:vert
    //   struct Input {
    //       float2 uv_MainTex;
    //   };
    //   float _Amount;
    //   void vert (inout appdata_full v) {
    //       v.vertex.xyz += v.normal * _Amount;
    //   }
    //   sampler2D _MainTex;
    //   void surf (Input IN, inout SurfaceOutput o) {
    //       o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
    //   }
    //   ENDCG
    // } 
	SubShader{
		Tags{
			"RenderType" = "Opaque"
		}
		CGPROGRAM
			#pragma surface surf Lambert vertex:vert

			struct Input{
				float2 uv_MainTex;
				float2 uv_BumpMap;
				float3 viewDir;

				float2 uv_Checker;
				float4 screenPos;

				float3 worldRefl;
				float3 worldPos;
			};

			sampler2D _MainTex;
			sampler2D _BumpMap;
			fixed4 _Color;
			float4 _RimColor;
			float _RimPower;

			sampler2D _Checker;

			samplerCUBE _Cube;

			float _Amount;

			void vert(inout appdata_full v){
				v.vertex.xyz += v.normal * _Amount;
			}

			void surf(Input IN, inout SurfaceOutput o){

				// clip (frac((IN.worldPos.y+IN.worldPos.z*0.1) * 5) - 0.5);

				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				// fixed4 c = (1, 1, 1, 1);
				o.Albedo = c.rgb;

				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				

				half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          		o.Emission = _RimColor.rgb * pow (rim, _RimPower);

				// float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
				// screenUV *= float2(8,6);
				// o.Albedo *= tex2D (_Checker, screenUV).rgb * 2;

				// o.Emission = texCUBE(_Cube, IN.worldRefl).rgb;
			}
		ENDCG
	}
	SubShader{
		Tags {
			"Queue" = "Transparent"
		}
		Pass{
			Material {
				Diffuse (1, 1, 1, .5)
			}
			Lighting On
			// Cull Front
			Cull Off
			// ZTest Always
			ZWrite Off
			// SetTexture[_MainTex]{}
			Blend SrcAlpha OneMinusSrcAlpha
			// BlendOp Sub
			// Blend DstColor Zero

		}
	}
	SubShader {
		Tags { 
			"RenderType"="Opaque"
		}
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
