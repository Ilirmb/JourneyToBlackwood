// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "FS/Wave" {
 
Properties {
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _BumpTex ("Bumped texture", 2D) = "white" {}

    _Frequency ("Frequency", Range (0, 200.0)) = 30.0
    _Amplitude ("Ampitude", Range (0, 0.05)) = 0.012
    _Displacement("Displacement", Range (0, 1)) = 0.15
    _DisplacementSmoothness("DisplacementSmoothness", Range (1, 30)) = 6.0
    _Speed("Speed", Range(1, 100)) = 80
    _Color("Color", Color) = (1, 1, 1, 1)

    _EffectPosition("Effect Position", Vector) = (-1, -1, -1, -1) 
    _EffectAmplitude("Amplitude", float) = 0
    _EffectRadius("Radius", float) = 3
}
 
SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" 
        "RenderType"="Transparent"}
    LOD 200
    Lighting Off
    ZTest Off
    Cull Off
    Blend SrcAlpha OneMinusSrcAlpha 

    GrabPass { "_BackgroundTexture" }

    Pass{
        CGPROGRAM

        #pragma target 3.0
        #pragma vertex vert
        #pragma fragment frag

        #include "UnityCG.cginc"
        #include "FSCG.cginc"
        
        sampler2D _MainTex;
        float4 _MainTex_ST;
        sampler2D _BumpTex;
        float4 _BumpTex_ST;
        sampler2D _BackgroundTexture;

        float _Frequency;
        float _Amplitude;
        float _Displacement;
        float _DisplacementSmoothness;
        float _Speed;
        float4 _Color;

        // effect vars
        float3 _EffectPosition;
        float _EffectAmplitude;
        float _EffectRadius;

        const float EFFECT_TIME = 500;
        
        struct Input {
            float4 grabUV;
            float2 uv_MainTex;
            float3 worldPos;
        };

        struct appdata
        {
            float4 pos : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f
        {
            float2 uv_MainTex : TEXCOORD0;
            float4 pos : SV_POSITION;
            float4 grabUV: TEXCOORD1;
            float3 worldPos: TEXCOORD2;
        };


        v2f vert(appdata v) {
            v2f o;
            o.worldPos = mul(unity_ObjectToWorld, v.pos).xyz;
            o.pos = UnityObjectToClipPos(v.pos);

            o.uv_MainTex = TRANSFORM_TEX(v.uv, _MainTex);
            o.grabUV = ComputeGrabScreenPos(o.pos);

            return o;
        } 

        float3 getLight(float2 uv) {
            float2 p = uv*6.28318530718-175.0;
            float2 i = p;
            float c = 1.0;
            float inten = .005;

            for (int n = 0; n < 3; n++) 
            {
                float t = _Time.w / 15.0 * (1.0 - (3.5 / float(n+1)));
                i = p + float2(cos(t - i.x) + sin(t + i.y), sin(t - i.y) + cos(t + i.x));
                c += 1.0/length(float2(p.x / (sin(i.x+t)/inten),p.y / (cos(i.y+t)/inten)));
            }
            c /= float(3);
            c = 1.17-pow(c, 1.4);
            float power = pow(abs(c), 8.0);
            float3 colour = float3(power, power, power);
            return clamp(colour, 0.0, 1.0);
        }

        float4 frag(v2f IN) : COLOR{

            fixed2 disp = tex2D(_BumpTex, IN.uv_MainTex / _DisplacementSmoothness).xy;
            float waveX = IN.uv_MainTex.x * _Frequency+_Time.x * _Speed;
            // shift of phase by hight
            waveX += 6 * sin(6 * IN.uv_MainTex.y);
            float s = sin(waveX) + 0.5 * sin(2*waveX  + 0.3) + 0.3 
                + 0.2 * sin(3*waveX + 0.9) + 0.2 +  0.1 * sin(5 * waveX + 1.7) + 0.1;

            if (_EffectAmplitude > 0) {
                float dist = (IN.worldPos.x - _EffectPosition.x) * (IN.worldPos.x - _EffectPosition.x) 
                    + (IN.worldPos.y - _EffectPosition.y) * (IN.worldPos.y - _EffectPosition.y);
                float radSqr = _EffectRadius * _EffectRadius;
                if (dist <=  radSqr) {
                    s += - 3 * _EffectAmplitude   * cos(_Frequency * _EffectRadius * 0.07 + dist * 1)
                        * (radSqr - dist) * (radSqr - dist) / (radSqr * radSqr ) ;
                }
            }
            float t= IN.uv_MainTex.y + (disp.y - 0.5) * _Displacement + (s * _Amplitude);
            fixed4 c = tex2D(_MainTex, float2(IN.uv_MainTex.x, t));

            float4 grab_uv = UNITY_PROJ_COORD(IN.grabUV);
            
            if (c.a > 0.01) {
                grab_uv.x += 0.1 * ((disp.y - 0.5) * _Displacement + (s * _Amplitude));
            }

            fixed4 bc = tex2Dproj( _BackgroundTexture, grab_uv);

            float4 out_col = bc * (c.a > 0.1 ? 1 : 0) + c * c.a * c.a;
            
            
            return out_col;
        }
        ENDCG
    }
}

Fallback "Transparent/Cutout/VertexLit"
}
