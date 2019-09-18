﻿Shader "Game2DWaterKit/VertexLit (Supports Lightmaps)"
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

	SubShader
	{
		Tags {
		"RenderType"="Opaque" 
		"IgnoreProjector"="True"
		"PreviewType"="Plane"
		}

		Blend [_SrcBlend] [_DstBlend]
		ZWrite [_ZWrite]
		Cull off

		Pass
		{
			Tags {"LIGHTMODE"="Vertex"}

			CGPROGRAM

			#define UNITY_PASS_VERTEX
			#define Water2D_LIGHT_ON
			#include "UnityCG.cginc"
			#include "Game2DWaterKit.cginc"

			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			// Compile specialized variants for when point and spot lights are present
			#pragma multi_compile __ POINT SPOT

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

			Water2D_VertexOutput vert (water2D_VertexInput v )
			{
				Water2D_VertexOutput o = Water2D_Vert(v);

				//lighting
				half3 lcolor = UNITY_LIGHTMODEL_AMBIENT.rgb;
				#if defined(POINT) || defined(SPOT)
				float3 mvPos = UnityObjectToViewPos(v.pos);
				#endif
				for (int il = 0; il < LIGHT_LOOP_LIMIT; ++il) {
				  #if defined(POINT) || defined(SPOT)
				  lcolor += computeLight(il, mvPos);
				  #else
				  lcolor += computeLight(il);
				  #endif
				}
				o.lightColor =lcolor;

				//Applying Fog
				UNITY_TRANSFER_FOG(o,o.pos);

				return o;
			}
			
			fixed4 frag (Water2D_VertexOutput i) : SV_Target
			{
				#if defined(Water2D_Refraction) && defined(Water2D_FakePerspective)
				fixed4 partiallySubmergedObjectsColor;
				fixed4 albedo = Water2D_Frag(i,partiallySubmergedObjectsColor);
				#else
				fixed4 albedo = Water2D_Frag(i);
				#endif

				//Applying light
				#if defined(Water2D_ApplyEmissionColor)
					albedo.rgb *= i.lightColor + _WaterEmissionColor * _WaterEmissionColorIntensity;
				#else
					albedo.rgb *= i.lightColor;
				#endif

				#if defined(Water2D_Refraction) && defined(Water2D_FakePerspective)
				albedo.rgb += partiallySubmergedObjectsColor.a * (partiallySubmergedObjectsColor.rgb - albedo.rgb);
				#endif

				// Applying fog
				UNITY_APPLY_FOG(i.fogCoord, albedo);

				return albedo;
			}

			ENDCG
	}

	Pass
		{
			Tags {"LIGHTMODE"="VertexLM"}

			CGPROGRAM

			#define UNITY_PASS_VERTEXLM
			#define Water2D_LIGHT_ON
			#define LIGHTMAP_ON

			#include "UnityCG.cginc"
			#include "Game2DWaterKit.cginc"

			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			#pragma multi_compile __ POINT SPOT

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

			Water2D_VertexOutput vert (water2D_VertexInput v)
			{
				Water2D_VertexOutput o = Water2D_Vert(v);

				//lighting
				half3 lcolor = UNITY_LIGHTMODEL_AMBIENT.rgb;
				#if defined(POINT) || defined(SPOT)
				float3 mvPos = UnityObjectToViewPos(v.pos);
				#endif
				for (int il = 0; il < LIGHT_LOOP_LIMIT; ++il) {
				  #if defined(POINT) || defined(SPOT)
				  lcolor += computeLight(il, mvPos);
				  #else
				  lcolor += computeLight(il);
				  #endif
				}
				o.lightColor =lcolor;

				//calculating lightmap coord
				o.lightmapCoord.xy = v.lightmapCoord * unity_LightmapST.xy + unity_LightmapST.zw;

				//Applying Fog
				UNITY_TRANSFER_FOG(o,o.pos);

				return o;
			}
			
			fixed4 frag (Water2D_VertexOutput i) : SV_Target
			{
				#if defined(Water2D_Refraction) && defined(Water2D_FakePerspective)
				fixed4 partiallySubmergedObjectsColor;
				fixed4 albedo = Water2D_Frag(i,partiallySubmergedObjectsColor);
				#else
				fixed4 albedo = Water2D_Frag(i);
				#endif

				//Applying light
				#if defined(Water2D_ApplyEmissionColor)
					albedo.rgb *= (i.lightColor + DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lightmapCoord))) + _WaterEmissionColor * _WaterEmissionColorIntensity;
				#else
					albedo.rgb *= i.lightColor + DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lightmapCoord));
				#endif

				#if defined(Water2D_Refraction) && defined(Water2D_FakePerspective)
				albedo.rgb += partiallySubmergedObjectsColor.a * (partiallySubmergedObjectsColor.rgb - albedo.rgb);
				#endif

				// Applying fog
				UNITY_APPLY_FOG(i.fogCoord, albedo);

				return albedo;
			}

			ENDCG
		}
	}

	CustomEditor "Game2DWaterKit.Game2DWaterShaderGUI"
}
