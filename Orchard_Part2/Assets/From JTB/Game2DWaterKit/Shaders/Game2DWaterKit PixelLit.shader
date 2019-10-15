﻿Shader "Game2DWaterKit/PixelLit (Supports Lightmaps)" 
{
        Properties {
    	//Water Properties
		[HideInInspector] _WaterColor ("Water Color",color) = (0.11,0.64,0.92,0.25)
		[HideInInspector] _WaterColorGradientStart ("Water Color Gradient Start",color) = (1.0,1.0,1.0,0.25)
		[HideInInspector] _WaterColorGradientEnd ("Water Color Gradient End",color) = (1.0,1.0,1.0,0.25)
		[HideInInspector] _WaterTexture ("Water Texture (RGBA)" , 2D) = "white" {}
		[HideInInspector] _WaterTextureOpacity ("Water Texture Opacity",range(0,1)) = 0.5
		[HideInInspector] _WaterNoiseSpeed ("Water Noise Speed",float) = 0.025
		[HideInInspector] _WaterNoiseScaleOffset ("Water Noise Scale Offset",vector) = (1,1,0,0)
		[HideInInspector] _WaterNoiseStrength ("Water Noise Strength",Range(0.001,1.0)) = 0.025
		[HideInInspector] _WaterTextureSheetFramesPerSecond("Water Texture Sheet Frames Per Second",float) = 0.0
		[HideInInspector] _WaterTextureSheetColumns("Water Texture Sheet Columns",float) = 1.0
		[HideInInspector] _WaterTextureSheetRows ("Water Texture Sheet Rows",float) = 1.0
		[HideInInspector] _WaterTextureSheetFramesCount("Water Texture Sheet Frames Count",float) = 1.0
		[HideInInspector] _WaterTextureSheetInverseColumns("Water Texture Sheet Inverse of Columns",float) = 1.0
		[HideInInspector] _WaterTextureSheetInverseRows("Water Texture Sheet Inverse of Rows",float) = 1.0

		//Water Surface Properties
		[HideInInspector] _SurfaceLevel ("Surface Level",range(0.0,1.0)) = 0.9
		[HideInInspector] _SubmergeLevel("Submerge Level",range(0.0,1.0)) = 0.95
		[HideInInspector] _SurfaceColor ("Surface Color",color) = (0.14,0.54,0.85,0.25)
      	[HideInInspector] _SurfaceTexture ("Surface Texture (RGBA)",2D) = "white" {}
		[HideInInspector] _SurfaceTextureOpacity ("Surface Texture Opacity",range(0,1)) = 0.5
		[HideInInspector] _SurfaceNoiseSpeed ("Surface Noise Speed",float) = 0.025
		[HideInInspector] _SurfaceNoiseScaleOffset ("Surface Noise Scale Offset",vector) = (1,1,0,0)
		[HideInInspector] _SurfaceNoiseStrength ("Surface Noise Strength",Range(0.001,1.0)) = 0.025
		[HideInInspector] _SurfaceTextureSheetFramesPerSecond("Surface Texture Sheet Frames Per Second",float) = 0.0
		[HideInInspector] _SurfaceTextureSheetColumns("Surface Texture Sheet Columns",float) = 1.0
		[HideInInspector] _SurfaceTextureSheetRows ("Surface Texture Sheet Rows",float) = 1.0
		[HideInInspector] _SurfaceTextureSheetFramesCount("Surface Texture Sheet Frames Count",float) = 1.0
		[HideInInspector] _SurfaceTextureSheetInverseColumns("Surface Texture Sheet Inverse of Columns",float) = 1.0
		[HideInInspector] _SurfaceTextureSheetInverseRows("Surface Texture Sheet Inverse of Rows",float) = 1.0

		//Lighting Properties
		[HideInInspector] _WaterEmissionColor("Water Emission Color",color) = (1.0,1.0,1.0,0.0)
		[HideInInspector] _WaterEmissionColorIntensity("Water Emission Color Intensity",range(0,1)) = 0.0

		//Refraction Properties
		[HideInInspector] _RefractionAmountOfBending ("Refraction Amount Of Bending",Range(0.0,0.025)) = 0.0
		[HideInInspector] _RefractionNoiseSpeed ("Refraction Speed",float) = 0.075
		[HideInInspector] _RefractionNoiseScaleOffset ("Refraction Noise Scale Offset",vector) = (8,5,0,0)
		[HideInInspector] _RefractionNoiseStrength ("Refraction Noise Strength",Range(0.001,0.1)) = 0.015

		//Reflection Properties
		[HideInInspector] _ReflectionVisibility ("Reflection Visibility",range(0,1)) = 0.3
		[HideInInspector] _ReflectionNoiseSpeed ("Reflection Speed",float) = 0.075
		[HideInInspector] _ReflectionNoiseScaleOffset ("Reflection Noise Scale Offset",vector) = (5,14,0,0)
		[HideInInspector] _ReflectionNoiseStrength ("Reflection Noise Strength",Range(0.001,0.1)) = 0.02

		//Noise Texture (RGBA): water(A) , surface(B) , reflection(G) and refraction(R)
		[HideInInspector] _NoiseTexture ("Noise Texture (RGBA)",2D) = "black"{}

		//Camera Render Rendertextures
		[HideInInspector] _RefractionTexture ("Refraction Texture", 2D) = "white" {}
		[HideInInspector] _RefractionTexturePartiallySubmergedObjects("Refraction Texture For Partially Submerged Objects", 2D) = "white" {}
		[HideInInspector] _ReflectionTexture ("Reflection Texture",2D) = "white" {}
		[HideInInspector] _ReflectionTexturePartiallySubmergedObjects("Reflection Texture For Partially Submerged Objects",2D) = "white" {}

		// Other properties
		[HideInInspector] _Mode ("__mode", float) = 2000.0
		[HideInInspector] _SrcBlend ("__src", float) = 1.0
		[HideInInspector] _DstBlend ("__dst", float) = 0.0
		[HideInInspector] _ZWrite ("__zw", float) = 1.0
			
		[HideInInspector] _Water2D_IsFakePerspectiveEnabled ("__fakePerspectiveState",float) = 0.0
		[HideInInspector] _Water2D_IsRefractionEnabled ("__refractionState",float) = 0.0
		[HideInInspector] _Water2D_IsReflectionEnabled ("__reflectionState",float) = 0.0
		[HideInInspector] _Water2D_IsWaterNoiseEnabled ("__waterNoiseState",float) = 0.0
		[HideInInspector] _Water2D_IsWaterTextureSheetEnabled ("__waterTextureSheetState",float) = 0.0
		[HideInInspector] _Water2D_IsWaterTextureSheetWithLerpEnabled("__waterTextureSheetWithLerpState",float) = 0.0
		[HideInInspector] _Water2D_IsSurfaceEnabled ("__surfaceState",float) = 0.0
		[HideInInspector] _Water2D_IsWaterSurfaceTextureSheetEnabled ("__surfaceTextureSheetState",float) = 0.0
		[HideInInspector] _Water2D_IsWaterSurfaceTextureSheetWithLerpEnabled("__waterSurfaceTextureSheetWithLerpState",float) = 0.0
		[HideInInspector] _Water2D_IsSurfaceNoiseEnabled ("__surfaceNoiseState",float) = 0.0
		[HideInInspector] _Water2D_IsColorGradientEnabled ("__colorGradientState",float) = 0.0
		[HideInInspector] _Water2D_IsEmissionColorEnabled("__emissionColorState",float) = 0.0
	}

SubShader {
      Tags {
		"RenderType"="Opaque"
		"IgnoreProjector"="True"
		"PreviewType"="Plane"
		}

		Blend [_SrcBlend] [_DstBlend]
		ZWrite [_ZWrite]
		Cull off

	Pass {
		Name "FORWARD"
		Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase noshadowmask nodynlightmap nodirlightmap noshadow

			#pragma shader_feature Water2D_FakePerspective
			#pragma shader_feature Water2D_Refraction
			#pragma shader_feature Water2D_Reflection
			#pragma shader_feature _ Water2D_WaterTexture Water2D_WaterTextureSheet Water2D_WaterTextureSheetWithLerp
			#pragma shader_feature Water2D_WaterNoise
			#pragma shader_feature Water2D_Surface
			#pragma shader_feature _ Water2D_SurfaceTexture Water2D_SurfaceTextureSheet Water2D_SurfaceTextureSheetWithLerp
			#pragma shader_feature Water2D_SurfaceNoise
			#pragma shader_feature Water2D_ColorGradient
			#pragma shader_feature Water2D_ApplyEmissionColor

			#ifndef UNITY_PASS_FORWARDBASE
			#define UNITY_PASS_FORWARDBASE
			#endif
			#ifdef UNITY_LIGHT_PROBE_PROXY_VOLUME
			#undef UNITY_LIGHT_PROBE_PROXY_VOLUME
			#endif

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "Game2DWaterKit.cginc"

			// vertex shader
			Water2D_VertexOutput vert (water2D_VertexInput v) {

			  Water2D_VertexOutput o = Water2D_Vert(v);

			  //lighting
			  float3 worldPos = mul(unity_ObjectToWorld, v.pos).xyz;
			  o.worldPos = worldPos;

			  #ifdef LIGHTMAP_ON
				  o.lightmapCoord.xy = v.lightmapCoord.xy * unity_LightmapST.xy + unity_LightmapST.zw;
			  #endif

			  // SH/ambient and vertex lights
			  #ifndef LIGHTMAP_ON
				#if UNITY_SHOULD_SAMPLE_SH && !UNITY_SAMPLE_FULL_SH_PER_PIXEL
				  o.sh = 0;
				  // Approximated illumination from non-important point lights
				  #ifdef VERTEXLIGHT_ON
					// to light vectors
					float4 toLightX = unity_4LightPosX0 - worldPos.x;
					float4 toLightY = unity_4LightPosY0 - worldPos.y;
					float4 toLightZ = unity_4LightPosZ0 - worldPos.z;

					// squared lengths
					float4 lengthSq = float4(0.0,0.0,0.0,0.0);
					lengthSq += toLightX * toLightX;
					lengthSq += toLightY * toLightY;
					lengthSq += toLightZ * toLightZ;
					lengthSq = max(lengthSq, 0.000001); // don't produce NaNs if some vertex position overlaps with the light

					float4 ndotl = max(float4(0.0,0.0,0.0,0.0),-toLightZ *  rsqrt(lengthSq));

					// attenuation
					float4 atten = 1.0 / (1.0 + lengthSq * unity_4LightAtten0);
					float4 diff = ndotl * atten;

					// final color
					float3 col = 0;
					col += unity_LightColor[0].rgb * diff.x;
					col += unity_LightColor[1].rgb * diff.y;
					col += unity_LightColor[2].rgb * diff.z;
					col += unity_LightColor[3].rgb * diff.w;
					o.sh += col;

				  #endif
				  o.sh = ShadeSHPerVertex (fixed3(0.0,0.0,-1.0), o.sh);
				#endif
			  #endif // !LIGHTMAP_ON

			  UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
			  return o;
			}

			// fragment shader
			fixed4 frag (Water2D_VertexOutput i) : SV_Target {

				#if defined(Water2D_Refraction) && defined(Water2D_FakePerspective)
				fixed4 partiallySubmergedObjectsColor;
				fixed4 albedo = Water2D_Frag(i,partiallySubmergedObjectsColor);
				#else
				fixed4 albedo = Water2D_Frag(i);
				#endif

			  //lighting
			  float3 worldPos = i.worldPos;
			  #ifndef USING_DIRECTIONAL_LIGHT
				fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
			  #else
				fixed3 lightDir = _WorldSpaceLightPos0.xyz;
			  #endif
			  UNITY_LIGHT_ATTENUATION(atten, i, worldPos)
			  // Setup lighting environment
			  UnityGIInput giInput;
			  UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
			  giInput.light.color = _LightColor0.rgb;
			  giInput.light.dir = lightDir;
			  giInput.worldPos = worldPos;
			  giInput.atten = atten;

			  #if defined(LIGHTMAP_ON)
				giInput.lightmapUV = float4(i.lightmapCoord,0.0,0.0);
			  #else
				giInput.lightmapUV = 0.0;
			  #endif
			  #if UNITY_SHOULD_SAMPLE_SH && !UNITY_SAMPLE_FULL_SH_PER_PIXEL
				giInput.ambient = i.sh;
			  #else
				giInput.ambient.rgb = 0.0;
			  #endif
			  giInput.probeHDR[0] = unity_SpecCube0_HDR;
			  giInput.probeHDR[1] = unity_SpecCube1_HDR;
			  #if defined(UNITY_SPECCUBE_BLENDING) || defined(UNITY_SPECCUBE_BOX_PROJECTION)
				giInput.boxMin[0] = unity_SpecCube0_BoxMin; // .w holds lerp value for blending
			  #endif
			  #ifdef UNITY_SPECCUBE_BOX_PROJECTION
				giInput.boxMax[0] = unity_SpecCube0_BoxMax;
				giInput.probePosition[0] = unity_SpecCube0_ProbePosition;
				giInput.boxMax[1] = unity_SpecCube1_BoxMax;
				giInput.boxMin[1] = unity_SpecCube1_BoxMin;
				giInput.probePosition[1] = unity_SpecCube1_ProbePosition;
			  #endif

			  UnityGI gi = UnityGI_Base (giInput,1.0, half3(0.0,0.0,-1.0));
			  fixed diff = max (0.0, -lightDir.z);
			  fixed4 c = 0.0;

			  fixed3 lightingColor = gi.light.color.rgb * diff;

			  #ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
				 lightingColor += gi.indirect.diffuse;
			  #endif

			  #if defined(Water2D_ApplyEmissionColor)
				  lightingColor += _WaterEmissionColor * _WaterEmissionColorIntensity;
			  #endif
				
			  c.rgb = albedo.rgb * lightingColor;
			  
			#if defined(Water2D_Refraction) && defined(Water2D_FakePerspective)
				c.rgb += partiallySubmergedObjectsColor.a * (partiallySubmergedObjectsColor.rgb - c.rgb);
			#endif

			  c.a = albedo.a;

			  UNITY_APPLY_FOG(i.fogCoord, c); // apply fog
			  return c;
			}

			ENDCG

			}


		// ---- forward rendering additive lights pass:
		Pass {
			Name "FORWARD"
			Tags { "LightMode" = "ForwardAdd" }
			ZWrite Off 
			Blend SrcAlpha ONE

			CGPROGRAM
			// compile directives
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma skip_variants INSTANCING_ON
			#pragma multi_compile_fwdadd noshadowmask nodynlightmap nodirlightmap noshadow

			#pragma shader_feature Water2D_FakePerspective
			#pragma shader_feature Water2D_Refraction
			#pragma shader_feature Water2D_Reflection
			#pragma shader_feature _ Water2D_WaterTexture Water2D_WaterTextureSheet Water2D_WaterTextureSheetWithLerp
			#pragma shader_feature Water2D_WaterNoise
			#pragma shader_feature Water2D_Surface
			#pragma shader_feature _ Water2D_SurfaceTexture Water2D_SurfaceTextureSheet Water2D_SurfaceTextureSheetWithLerp
			#pragma shader_feature Water2D_SurfaceNoise
			#pragma shader_feature Water2D_ColorGradient
			#pragma shader_feature Water2D_ApplyEmissionColor

			#ifndef UNITY_PASS_FORWARDADD
			#define UNITY_PASS_FORWARDADD
			#endif
			#ifdef UNITY_LIGHT_PROBE_PROXY_VOLUME
			#undef UNITY_LIGHT_PROBE_PROXY_VOLUME
			#endif

			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "Game2DWaterKit.cginc"

			// vertex shader
			Water2D_VertexOutput vert (water2D_VertexInput v) {
			  Water2D_VertexOutput o = Water2D_Vert(v);

			  o.worldPos = mul(unity_ObjectToWorld, v.pos).xyz;

			  UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
			  return o;
			}

			// fragment shader
			fixed4 frag (Water2D_VertexOutput i):SV_TARGET  {
			 
				#if defined(Water2D_Refraction) && defined(Water2D_FakePerspective)
					fixed partiallaySubmergedObjectsColorOpacity;
					fixed4 albedo = Water2D_Frag(i, partiallaySubmergedObjectsColorOpacity);
				#else
					fixed4 albedo = Water2D_Frag(i);
				#endif

			  float3 worldPos = i.worldPos;

			  #ifndef USING_DIRECTIONAL_LIGHT
				fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
			  #else
				fixed3 lightDir = _WorldSpaceLightPos0.xyz;
			  #endif

			  UNITY_LIGHT_ATTENUATION(atten, i, worldPos)
				albedo.rgb *= _LightColor0.rgb * max (0, -lightDir.z) * atten;

				#if defined(Water2D_Refraction) && defined(Water2D_FakePerspective)
			    albedo.a = 1.0 - partiallaySubmergedObjectsColorOpacity;
				#else
			    albedo.a = 1.0;
				#endif

				UNITY_APPLY_FOG(i.fogCoord, albedo); // apply fog
  				return albedo;
			}
			ENDCG
			}
    } 
	CustomEditor "Game2DWaterKit.Game2DWaterShaderGUI"
  }