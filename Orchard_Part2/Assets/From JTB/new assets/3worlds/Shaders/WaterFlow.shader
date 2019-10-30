Shader "FS/WaterFlow" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_BumpTex("Bump (RGB)", 2D) = "white" {}
		_Bump2Tex("Bump (RGB)", 2D) = "white" {}
		_Depth("Depth", Range(0,10)) = 1.62
		_Speed("Speed", Range(0, 15)) = 0.025
		_LeftFlow("Left Flow", Float) = 0
		_Color("Color maks", Color) = (1,1,1)

         _Ambient("Ambient", Range(0,1)) = 0.1
         _NoCircleLight("No circle light", int) = 0
         _CircleMultiplier("Circle radius mult", Range(0, 3)) = 1 
         _BrightnessMultiplier("Circle brightness dampening", Range(0, 1)) = 1 
	}
	SubShader{
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"}
		
		Cull Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		Lighting Off
		Fog { Mode Off }

        GrabPass {"_BackgroundTexture" }

		Pass{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "FSCG.cginc"

			float FADEOUT_MARGIN = 0.1;

			uniform sampler2D _MainTex;
			uniform sampler2D _BumpTex;
			uniform sampler2D _Bump2Tex;
			uniform sampler2D _BackgroundTexture;
			float4 _MainTex_ST;

			uniform float _Depth;
			uniform float _Speed;
			uniform float _LeftFlow;
			uniform float4 _Color;

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
				
				if (v.pos.x > (1 - FADEOUT_MARGIN)) {
					v.pos.z *= (1 - v.pos.x) / FADEOUT_MARGIN;
				}

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				float phase = _Time[1] / 7.0;// + noise.r * 0.5f;
				float f = frac(phase);
				float4 flowDir = -1 * float4(1, 0, 0, 0);
				float2 bump = tex2Dlod(_Bump2Tex, float4(v.uv.xy,0,0) + flowDir * frac(phase + 0.5f));
				v.pos.y += (bump.x - 0.5);

				o.pos = UnityObjectToClipPos(v.pos);

                o.grabUV = ComputeGrabScreenPos(o.pos);
				return o;
			}

			float4 frag(v2f i) : COLOR{
				float4 res;
                float2 shift = float2(0.6 * sin(_Time.w * _Speed), 0.04 * sin(_Time.w * 0.04));
                float2 bump = tex2D(_BumpTex, (i.uv + shift) / 1.5).xy;
                float4 grab_uv = UNITY_PROJ_COORD(i.grabUV);
                float2 grab_shift = 0.5 * (bump.y - 0.5);
				float uvX = i.uv.x;
				if (uvX < (1 - FADEOUT_MARGIN)) {
					grab_uv.x += grab_shift;
				}
                fixed4 bc = tex2Dproj( _BackgroundTexture, grab_uv);

				float2 uv = i.uv;
				uv.x += ((_LeftFlow != 0) ? 1 : -1) * _Time.w * _Speed;
				uv.x += 0.15 *grab_shift;
				uv.x = uv.x - floor(uv.x);
				float4 c = tex2D(_MainTex, uv);

                float delta = GetLightDelta(i.pos);
				return c * _Color * delta;
			}
			ENDCG
		}

	}
}