#ifndef LIGHTING_ENGINE_INCLUDED
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 gles
#define LIGHTING_ENGINE_INCLUDED

CBUFFER_START(FSLight)
    float _LightPositionsX[20];
    float _LightPositionsY[20];
    float _LightSizes[20];
    float _FlickerSizes[20];
    float _FlickerShifts[20];
    float _LightIntencities[20];
    float _LightStatuses[20];
    int _LightCount;

    float _Smoothness;
    float _Ambient;
    float _CircleMultiplier;
    float _BrightnessMultiplier;
    int _NoCircleLight;
    float _OrtographicSize;
CBUFFER_END

float4 DownRes(sampler2D tex, float2 uv, float4 size) {
    float x = (floor(uv.x / size.x) + 0.5) * size.x;
    float y = (floor(uv.y / size.y) + 0.5) * size.y;
    return tex2D(tex, float2(x, y));
}

float4 Blur(sampler2D tex, float2 uv, float4 size) {
    float4 res = float4(0, 0, 0, 0);
    int amount = 0;
    
    for (int i = -1; i < 2; i++) {
        for (int j = -1; j < 2; j++) {
            if (i == 0 || j == 0) continue;

            res += tex2D(tex, uv + float2(i * size.x, j * size.y));
            amount ++;
        }
    }
    
    return res / amount;
}

float GetCircleDelta(float4 pos, int i) {
    float2 lightPos = float2(_LightPositionsX[i], _LightPositionsY[i]);
    float circleSize = _LightSizes[i];
    float flickerSize = _FlickerSizes[i];
    float flickerShift = _FlickerShifts[i];
    float intencity = _LightIntencities[i];

    float _oscX = 2.93, _oscY = 2.24;
    float delta = 0;
    float len = length(pos.xy - lightPos.xy);
    float dotProd = dot((pos.xy - lightPos.xy)/len, 
    float2(cos(_oscX * _Time.w + flickerShift), cos(_oscY * _Time.w + flickerShift)));
    float halo  = sin(dotProd * 0.3 + 1 * _Time.w + flickerShift) * dotProd * 0.015 * flickerSize;

    float ortoSize = sqrt(5.485925 / _OrtographicSize);

    float fixedCircle = circleSize * (1 + halo) * _CircleMultiplier * ortoSize;
    float blurredCircle = circleSize * (1 + halo) * (1 + _Smoothness) * _CircleMultiplier * ortoSize;

    len /= max(_ScreenParams.x, _ScreenParams.y) / 2.0f;
    len = sqrt(len * len + pos.z * pos.z / 10.0);
    if (len < fixedCircle) {
        delta = _BrightnessMultiplier * intencity;
    } else if (len < blurredCircle) {
        float blurring = smoothstep(fixedCircle, blurredCircle, len);
        delta = _BrightnessMultiplier * (1 - blurring) * intencity;
    }

    if (len < blurredCircle) {
         delta /= abs(1 + halo);
    }

    return delta;
}

float GetLightDelta(float4 pos) {
    float delta = _Ambient * _BrightnessMultiplier;

    if (!_NoCircleLight) {
        for (int i = 0; i < _LightCount; i++) {
            if (!_LightStatuses[i]) continue;

            delta += GetCircleDelta(pos, i);
        }
    }

    delta =  delta > 1 ? 1 : delta;

    return delta;
}

#endif  