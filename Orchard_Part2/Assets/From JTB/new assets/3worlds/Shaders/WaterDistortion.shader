Shader "FS/WaterDistortion" {
	Properties{
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _BumpTex ("Bump texture for distortion", 2D) = "white" {}

         _Ambient("Ambient", Range(0,1)) = 0.1
         _NoCircleLight("No circle light", int) = 0
         _CircleMultiplier("Circle radius mult", Range(0, 3)) = 1 
         _BrightnessMultiplier("Circle brightness dampening", Range(0, 1)) = 1 
	}
	SubShader{
        Tags { "Queue"="Transparent" "RenderType"="Transparent"  "IgnoreProjector"="True" }
		Blend SrcAlpha OneMinusSrcAlpha
		    
        GrabPass { "_BackgroundTexture" }

		Pass{
            Name "DISTORTION"

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "FSCG.cginc"

			sampler2D _MainTex;
			sampler2D _BumpTex;
            sampler2D _BackgroundTexture;
			float4 _MainTex_ST;
			float4 _BumpTex_ST;
            

			struct appdata
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
                float4 grabUV: TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.pos);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.grabUV = ComputeGrabScreenPos(o.pos);
				return o;
			}

			float4 frag(v2f i) : COLOR{
                float2 shift = float2(0.3 * sin(_Time.w / 25.), 0.2 * sin(_Time.w / 25.));
                float2 bump = tex2D(_BumpTex, (i.uv + shift)).xy;
                float4 grab_uv = UNITY_PROJ_COORD(i.grabUV);
                float2 uv_shift = float2(0.02 * (bump.x - 0.5), 0);
				if (i.uv.y > 0.8) {
					uv_shift *= (0.9 - i.uv.y) / 0.1;
				}
                grab_uv.xy += uv_shift;
                //i.uv.xy += uv_shift;

                fixed4 bc = tex2Dproj( _BackgroundTexture, grab_uv);
                float4 text_c = tex2D(_MainTex, i.uv - float2(0, 0.01));

				bc *= float4(3 * text_c.rgb,1);

				return bc;
			}
			ENDCG
		}
	}
}