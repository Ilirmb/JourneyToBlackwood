using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace Game2DWaterKit
{
    [CanEditMultipleObjects, CustomEditor(typeof(Game2DWater))]
    internal class Game2DWaterInspector : Editor
    {
        #region variables

        #region Serialized Properties

        #region Water Properties
        //Mesh Properties
        private SerializedProperty subdivisionsCountPerUnit;
        private SerializedProperty waterSize;
        //Wave Properties
        private SerializedProperty damping;
        private SerializedProperty stiffness;
        private SerializedProperty spread;
        private SerializedProperty useCustomBoundaries;
        private SerializedProperty firstCustomBoundary;
        private SerializedProperty secondCustomBoundary;
        //Misc Properties
        private SerializedProperty buoyancyEffectorSurfaceLevel;
        #endregion

        #region On-Collision Ripples Properties
        private SerializedProperty activateOnCollisionOnWaterEnterRipples;
        private SerializedProperty activateOnCollisionOnWaterExitRipples;
        //Disturbance Properties
        private SerializedProperty onCollisionRipplesMinimumDisturbance;
        private SerializedProperty onCollisionRipplesMaximumDisturbance;
        private SerializedProperty onCollisionRipplesVelocityMultiplier;
        //Collision Properties
        private SerializedProperty onCollisionRipplesCollisionMask;
        private SerializedProperty onCollisionRipplesCollisionMinimumDepth;
        private SerializedProperty onCollisionRipplesCollisionMaximumDepth;
        private SerializedProperty onCollisionRipplesCollisionRaycastMaxDistance;
        //Events
        private SerializedProperty onWaterEnter;
        private SerializedProperty onWaterExit;
        //Sound Effect Properties (On Water Enter)
        private SerializedProperty onCollisionRipplesActivateOnWaterEnterSoundEffect;
        private SerializedProperty onCollisionRipplesOnWaterEnterAudioClip;
        private SerializedProperty onCollisionRipplesOnWaterEnterMinimumAudioPitch;
        private SerializedProperty onCollisionRipplesOnWaterEnterMaximumAudioPitch;
        private SerializedProperty onCollisionRipplesUseConstantOnWaterEnterAudioPitch;
        private SerializedProperty onCollisionRipplesOnWaterEnterAudioPitch;
        private SerializedProperty onCollisionRipplesOnWaterEnterAudioVolume;
        private SerializedProperty onCollisionRipplesOnWaterEnterSoundEffectPoolSize;
        private SerializedProperty onCollisionRipplesOnWaterEnterSoundEffectPoolExpandIfNecessary;
        //Sound Effect Properties (On Water Exit)
        private SerializedProperty onCollisionRipplesActivateOnWaterExitSoundEffect;
        private SerializedProperty onCollisionRipplesOnWaterExitAudioClip;
        private SerializedProperty onCollisionRipplesOnWaterExitMinimumAudioPitch;
        private SerializedProperty onCollisionRipplesOnWaterExitMaximumAudioPitch;
        private SerializedProperty onCollisionRipplesUseConstantOnWaterExitAudioPitch;
        private SerializedProperty onCollisionRipplesOnWaterExitAudioPitch;
        private SerializedProperty onCollisionRipplesOnWaterExitAudioVolume;
        private SerializedProperty onCollisionRipplesOnWaterExitSoundEffectPoolSize;
        private SerializedProperty onCollisionRipplesOnWaterExitSoundEffectPoolExpandIfNecessary;
        //Particle Effect Properties (On Water Enter)
        private SerializedProperty onCollisionRipplesActivateOnWaterEnterParticleEffect;
        private SerializedProperty onCollisionRipplesOnWaterEnterParticleEffect;
        private SerializedProperty onCollisionRipplesOnWaterEnterParticleEffectPoolSize;
        private SerializedProperty onCollisionRipplesOnWaterEnterParticleEffectSpawnOffset;
        private SerializedProperty onCollisionRipplesOnWaterEnterParticleEffectStopAction;
        private SerializedProperty onCollisionRipplesOnWaterEnterParticleEffectPoolExpandIfNecessary;
        //Particle Effect Properties (On Water Exit)
        private SerializedProperty onCollisionRipplesActivateOnWaterExitParticleEffect;
        private SerializedProperty onCollisionRipplesOnWaterExitParticleEffect;
        private SerializedProperty onCollisionRipplesOnWaterExitParticleEffectPoolSize;
        private SerializedProperty onCollisionRipplesOnWaterExitParticleEffectSpawnOffset;
        private SerializedProperty onCollisionRipplesOnWaterExitParticleEffectStopAction;
        private SerializedProperty onCollisionRipplesOnWaterExitParticleEffectPoolExpandIfNecessary;
        #endregion

        #region Constant Ripples Properties
        private SerializedProperty activateConstantRipples;
        private SerializedProperty constantRipplesUpdateWhenOffscreen;
        //Disturbance Properties
        private SerializedProperty constantRipplesDisturbance;
        private SerializedProperty constantRipplesRandomizeDisturbance;
        private SerializedProperty constantRipplesMinimumDisturbance;
        private SerializedProperty constantRipplesMaximumDisturbance;
        private SerializedProperty constantRipplesSmoothDisturbance;
        private SerializedProperty constantRipplesSmoothFactor;
        //Interval Properties
        private SerializedProperty constantRipplesRandomizeInterval;
        private SerializedProperty constantRipplesInterval;
        private SerializedProperty constantRipplesMinimumInterval;
        private SerializedProperty constantRipplesMaximumInterval;
        //Ripple Source Positions Properties
        private SerializedProperty constantRipplesRandomizeRipplesSourcesPositions;
        private SerializedProperty constantRipplesRandomizeRipplesSourcesCount;
        private SerializedProperty constantRipplesAllowDuplicateRipplesSourcesPositions;
        private SerializedProperty constantRipplesSourcePositions;
        //Sound Effect Properties
        private SerializedProperty constantRipplesActivateSoundEffect;
        private SerializedProperty constantRipplesAudioClip;
        private SerializedProperty constantRipplesUseConstantAudioPitch;
        private SerializedProperty constantRipplesAudioPitch;
        private SerializedProperty constantRipplesMinimumAudioPitch;
        private SerializedProperty constantRipplesMaximumAudioPitch;
        private SerializedProperty constantRipplesAudioVolume;
        private SerializedProperty constantRipplesSoundEffectPoolSize;
        private SerializedProperty constantRipplesSoundEffectPoolExpandIfNecessary;
        //Particle Effect Properties
        private SerializedProperty constantRipplesActivateParticleEffect;
        private SerializedProperty constantRipplesParticleEffect;
        private SerializedProperty constantRipplesParticleEffectPoolSize;
        private SerializedProperty constantRipplesParticleEffectSpawnOffset;
        private SerializedProperty constantRipplesParticleEffectStopAction;
        private SerializedProperty constantRipplesParticleEffectPoolExpandIfNecessary;
        #endregion

        #region Script-Generated Ripples
        //Disturbance Properties
        private SerializedProperty scriptGeneratedRipplesMinimumDisturbance;
        private SerializedProperty scriptGeneratedRipplesMaximumDisturbance;
        //Sound Effect Properties
        private SerializedProperty scriptGeneratedRipplesActivateSoundEffect;
        private SerializedProperty scriptGeneratedRipplesAudioClip;
        private SerializedProperty scriptGeneratedRipplesUseConstantAudioPitch;
        private SerializedProperty scriptGeneratedRipplesAudioPitch;
        private SerializedProperty scriptGeneratedRipplesMinimumAudioPitch;
        private SerializedProperty scriptGeneratedRipplesAudioVolume;
        private SerializedProperty scriptGeneratedRipplesMaximumAudioPitch;
        private SerializedProperty scriptGeneratedRipplesSoundEffectPoolSize;
        private SerializedProperty scriptGeneratedRipplesSoundEffectPoolExpandIfNecessary;
        //Particle Effect Properties
        private SerializedProperty scriptGeneratedRipplesActivateParticleEffect;
        private SerializedProperty scriptGeneratedRipplesParticleEffect;
        private SerializedProperty scriptGeneratedRipplesParticleEffectPoolSize;
        private SerializedProperty scriptGeneratedRipplesParticleEffectSpawnOffset;
        private SerializedProperty scriptGeneratedRipplesParticleEffectStopAction;
        private SerializedProperty scriptGeneratedRipplesParticleEffectPoolExpandIfNecessary;
        #endregion

        #region Refraction & Reflection Rendering Properties
        //Refraction Properties
        private SerializedProperty refractionRenderTextureResizingFactor;
        private SerializedProperty refractionCullingMask;
        private SerializedProperty refractionPartiallySubmergedObjectsCullingMask;
        private SerializedProperty refractionRenderTextureFilterMode;
        //Reflection Properties
        private SerializedProperty reflectionRenderTextureResizingFactor;
        private SerializedProperty reflectionViewingFrustumHeightScalingFactor;
        private SerializedProperty reflectionPartiallySubmergedObjectsViewingFrustumHeightScalingFactor;
        private SerializedProperty reflectionCullingMask;
        private SerializedProperty reflectionPartiallySubmergedObjectsCullingMask;
        private SerializedProperty reflectionZOffset;
        private SerializedProperty reflectionRenderTextureFilterMode;
        //Other Properties
        private SerializedProperty renderPixelLights;
        private SerializedProperty sortingLayerID;
        private SerializedProperty sortingOrder;
        private SerializedProperty allowMSAA;
        private SerializedProperty allowHDR;
        private SerializedProperty farClipPlane;
        #endregion

        #endregion

        #region Serialized Properties Labels

        #region WaterProperties

        //Mesh Properties
        private static readonly string meshPropertiesLabel = "Mesh Properties";
        private static readonly GUIContent waterSizeLabel = new GUIContent("Water Size", "Sets the water size, the width and the height respectively.");
        private static readonly GUIContent subdivisionsCountPerUnitLabel = new GUIContent("Subdivisions Per Unit", "Sets the number of water’s surface vertices within one unit.");
        private static readonly GUIContent waterPropertiesFoldoutLabel = new GUIContent("Water Properties");
        //Wave Properties
        private static readonly string wavePropertiesLabel = "Wave Properties";
        private static readonly GUIContent dampingLabel = new GUIContent("Damping", "Controls how fast the waves decay. A low value will make waves oscillate for a long time, while a high value will make waves oscillate for a short time.");
        private static readonly GUIContent spreadLabel = new GUIContent("Spread", "Controls how fast the waves spread.");
        private static readonly GUIContent stiffnessLabel = new GUIContent("Stiffness", "Controls the frequency of wave vibration. A low value will make waves oscillate slowly, while a high value will make waves oscillate quickly.");
        private static readonly GUIContent useCustomBoundariesLabel = new GUIContent("Use Custom Boundaries", "Enable/Disable using custom wave boundaries. When waves reach a boundary, they bounce back.");
        private static readonly GUIContent firstCustomBoundaryLabel = new GUIContent("First Boundary", "The location of the first boundary.");
        private static readonly GUIContent secondCustomBoundaryLabel = new GUIContent("Second Boundary", "The location of the second boundary.");
        //Misc Properties
        private static readonly string miscLabel = "Misc";
        private static readonly GUIContent buoyancyEffectorSurfaceLevelLabel = new GUIContent("Surface Level", "Sets the surface location of the buoyancy fluid. When an object is above this line, no buoyancy forces are applied. When an object is intersecting or completely below this line, buoyancy forces are applied.");
        private static readonly GUIContent useEdgeCollider2DLabel = new GUIContent("Use Edge Collider 2D", "Adds/Removes an EdgeCollider2D component. The water script takes care of updating the edge collider points.");
        private static readonly GUIContent fixScalingButtonLabel = new GUIContent("Fix Scaling");
        private static readonly string nonUniformScaleWarning = "Please use uniform scaling.";

        #endregion

        #region On-Collision Ripples Properties
        //Disturbance Properties
        private static readonly GUIContent onCollisionRipplesMinimumDisturbanceLabel = new GUIContent("Minimum Disturbance", "Sets the minimum displacement of the water’s surface.");
        private static readonly GUIContent onCollisionRipplesMaximumDisturbanceLabel = new GUIContent("Maximum Disturbance", "Sets the maximum displacement of the water’s surface.");
        private static readonly GUIContent onCollisionRipplesVelocityMultiplierLabel = new GUIContent("Velocity Multiplier", "When an object falls into water or leaves the water, the amount of water’s surface displacement is determined by multiplying the object’s rigidbody velocity by this factor and clamping the result between the minimum and the maximum disturbance values.");
        //Collision Properties
        private static readonly GUIContent onCollisionRipplesCollisionMinimumDepthLabel = new GUIContent("Minimum Depth", "Only objects with Z coordinate (depth) greater than or equal to this value will disturb the water’s surface.");
        private static readonly GUIContent onCollisionRipplesCollisionMaximumDepthLabel = new GUIContent("Maximum Depth", "Only objects with Z coordinate (depth) less than or equal to this value will disturb the water’s surface.");
        private static readonly GUIContent onCollisionRipplesCollisionRaycastMaxDistanceLabel = new GUIContent("Maximum Distance", "The maximum distance from the water's surface over which to check for collisions (Default: 0.5)");
        private static readonly GUIContent onCollisionRipplesCollisionMaskLabel = new GUIContent("Collision Mask", "Only objects on these layers will disturb the water’s surface and will  trigger the OnWaterEnter and the OnWaterExit events when they get into or out of the water.");
        private static readonly string collisionPropertiesLabel = "Collision Properties";
        //Water Events Properties
        private static readonly string eventsLabel = "Events";
        private static readonly GUIContent onWaterEnterLabel = new GUIContent("OnWaterEnter", "This Unity Event is triggered when an object falls into water.");
        private static readonly GUIContent onWaterExitLabel = new GUIContent("OnWaterExit", "This Unity Event is triggered when an object gets out of the water.");
        //Sound Effect Properties (On Water Enter)
        private static readonly string onCollisionRipplesOnWaterEnterAudioPitchMessage = "The AudioSource pitch (playback speed) is linearly interpolated between the minimum pitch and the maximum pitch. When a rigidbody falls into water, the higher its velocity, the lower the pitch value is.";
        //Sound Effect Properties (On Water Exit)
        private static readonly string onCollisionRipplesOnWaterExitAudioPitchMessage = "The AudioSource pitch (playback speed) is linearly interpolated between the minimum pitch and the maximum pitch. When a rigidbody leaves the water, the higher its velocity, the lower the pitch value is.";
        //Misc
        private static readonly GUIContent onCollisionRipplesPropertiesFoldoutLabel = new GUIContent("On Collision Ripples Properties");
        private static readonly string onWaterEnterRipplesPropertiesLabel = "On Water Enter Ripples Properties";
        private static readonly string onWaterExitRipplesPropertiesLabel = "On Water Exit Ripples Properties";
        #endregion

        #region Constant Ripples Properties
        private static readonly GUIContent constantRipplesPropertiesFoldoutLabel = new GUIContent("Constant Ripples Properties");
        private static readonly GUIContent activateConstantRipplesLabel = new GUIContent("Activate Constant Ripples", "Activates/Deactivates generating ripples at regular time intervals.");
        private static readonly GUIContent constantRipplesUpdateWhenOffscreenLabel = new GUIContent("Simulate ripples when off-screen", "Generate constant ripples even when the water is not visible to any camera in the scene.");
        //Disturbance Properties
        private static readonly GUIContent constantRipplesDisturbanceLabel = new GUIContent("Disturbance", "Sets the displacement of the water’s surface when generating constant ripples.");
        private static readonly GUIContent constantRipplesRandomizeDisturbanceLabel = new GUIContent("Randomize Disturbance", "Randomize the disturbance (displacement) of the water's surface.");
        private static readonly GUIContent constantRipplesMinimumDisturbanceLabel = new GUIContent("Minimum Disturbance", "Sets the minimum displacement of the water’s surface.");
        private static readonly GUIContent constantRipplesMaximumDisturbanceLabel = new GUIContent("Maximum Disturbance", "Sets the maximum displacement of the water’s surface.");
        private static readonly GUIContent constantRipplesSmoothDisturbanceLabel = new GUIContent("Smooth Ripples", "Disturb neighbor surface vertices to create a smoother ripple.");
        private static readonly GUIContent constantRipplesSmoothFactorLabel = new GUIContent("Smoothing Factor", "The amount of disturbance to apply to neighbor surface vertices.");
        //Interval Properties
        private static readonly string intervalPropertiesLabel = "Time Interval Properties";
        private static readonly GUIContent randomizePersistnetWaveIntervalLabel = new GUIContent("Randomize Time Interval", "Randomize the time interval.");
        private static readonly GUIContent constantRipplesIntervalLabel = new GUIContent("Time Interval", "Generate constant ripples at regular time interval (expressed in seconds).");
        private static readonly GUIContent constantRipplesMinimumIntervalLabel = new GUIContent("Minimum Time Interval", "Sets the minimum time interval.");
        private static readonly GUIContent constantRipplesMaximumIntervalLabel = new GUIContent("Maximum Time Interval", "Sets the maximum time interval.");
        //Ripple Source Positions Properties
        private static readonly string constantRipplesSourcesPropertiesLabel = "Ripple Source Positions Properties";
        private static readonly GUIContent constantRipplesRandomizeRipplesSourcesCountLabel = new GUIContent("Ripples Source Count", "When Randomize Positions is checked, this sets the number of random surface vertices to disturb when generating constant ripples.");
        private static readonly GUIContent constantRipplesRandomizeRipplesSourcesPositionsLabel = new GUIContent("Randomize Positions", "Randomize constant ripples sources positions. When checked, random surface vertices are disturbed each time the constant ripples are generated.");
        private static readonly GUIContent constantRipplesSourcePositionsLabel = new GUIContent("Ripples Source Positions (X-axis)", "Sets the constant ripples source positions.");
        private static readonly GUIContent constantRipplesAllowDuplicateRipplesSourcesPositionsLabel = new GUIContent("Allow Duplicate Positions", "Allow generating multiple ripples in the same position and at the same time.");
        private static readonly GUIContent constantRipplesEditSourcesPositionsLabel = new GUIContent("Edit Positions", "Edit constant ripples sources positions.");
        //Sound Effect Properties
        private static readonly string constantRipplesAudioPitchMessage = "The AudioSource pitch (playback speed) is linearly interpolated between the minimum pitch and the maximum pitch. When a ripple is generated, the higher its disturbance, the lower the pitch value is.";
        #endregion

        #region Script-Generated Ripples Properties
        private static readonly string scriptGeneratedRipplesMessage = "You can generate ripples in script with the .ScriptGeneratedRipplesModule.GenerateRipple() method";
        //Disturbance Properties
        private static readonly GUIContent scriptGeneratedRipplesPropertiesFoldoutLabel = new GUIContent("Script-Generated Ripples Properties");
        private static readonly GUIContent scriptGeneratedRipplesMaximumDisturbanceLabel = new GUIContent("Maximum Disturbance", "Sets the maximum displacement of the water’s surface.");
        private static readonly GUIContent scriptGeneratedRipplesMinimumDisturbanceLabel = new GUIContent("Minimum Disturbance", "Sets the minimum displacement of the water’s surface.");
        //Sound Effect Properties
        private static readonly string scriptGeneratedRipplesAudioPitchMessage = "The AudioSource pitch (playback speed) is linearly interpolated between the minimum pitch and the maximum pitch. When a ripple is generated, the higher its disturbance, the lower the pitch value is.";
        #endregion

        #region Refraction & Reflection Rendering Properties
        //Refraction Properties
        private static readonly GUIContent refractionPropertiesFoldoutLabel = new GUIContent("Refraction Properties");
        private static readonly GUIContent refractionRenderTextureResizingFactorLabel = new GUIContent("Resizing Factor", "Specifies how much the refraction RenderTexture is resized.");
        private static readonly GUIContent refractionRenderTextureFilterModeLabel = new GUIContent("Filter Mode", "Sets the refraction RenderTexture filter mode.");
        private static readonly string refractionMessage = "Refraction properties are disabled. \"Refraction\" can be activated in the material editor.";
        //Reflection Properties
        private static readonly GUIContent reflectionPropertiesFoldoutLabel = new GUIContent("Reflection Properties");
        private static readonly GUIContent reflectionPartiallySubmergedObjectsViewingFrustumHeightScalingFactorLabel = new GUIContent("Partially Submerged Objects", "Sets how much to scale the partially submerged objects reflection camera viewing frustum height. The default viewing frustum height is equal to the distance between the surface level and the submerge level.");
        private static readonly GUIContent reflectionViewingFrustumHeightScalingFactorLabel = new GUIContent("Other objects", "Sets how much to scale the reflection camera viewing frustum height when rendering other objects (all objects specified in ‘Objects to render’ layers except those specified in ‘Partially Submerged Objects’ layers). The default viewing frustum height for the reflection camera is equal to the surface thickness.");
        private static readonly GUIContent reflectionRenderTextureResizingFactorLabel = new GUIContent("Resizing Factor", "Specifies how much the reflection RenderTexture is resized.");
        private static readonly GUIContent reflectionZOffsetLabel = new GUIContent("Z Offset", "Controls where to start rendering the reflection relative to the water object position.");
        private static readonly GUIContent reflectionRenderTextureFilterModeLabel = new GUIContent("Filter Mode", "Sets the reflection RenderTexture filter mode.");
        private static readonly string viewingFrustumHeightScalingFactorLabel = "Viewing Frustum Height Scaling Factors";
        private static readonly string reflectionMessage = "Reflection properties are disabled. \"Reflection\" can be activated in the material editor.";
        //Other Properties
        private static readonly GUIContent refractionReflectionCullingMaskLabel = new GUIContent("Objects to render", "Only objects on these layers will be rendered by the water refraction camera.");
        private static readonly GUIContent refractionReflectionPartiallySubmergedObjectsCullingMaskLabel = new GUIContent("Partially Submerged Objects", "Objects on these layers will be rendered as partially submerged into water when they intersect the submerge level.");
        private static readonly GUIContent renderingSettingsFoldoutLabel = new GUIContent("Rendering Settings");
        private static readonly GUIContent farClipPlaneLabel = new GUIContent("Far Clip Plane", "Sets the furthest point relative to the water that will be rendered by the refraction and/or the reflection cameras.");
        private static readonly GUIContent renderPixelLightsLabel = new GUIContent("Render Pixel Lights", "Controls whether the rendered objects will be affected by pixel lights. Disabling this parameter could increase performance at the expense of visual fidelity.");
        private static readonly GUIContent sortingLayerLabel = new GUIContent("Sorting Layer", "The name of the water mesh renderer sorting layer.");
        private static readonly GUIContent orderInLayerLabel = new GUIContent("Order In Layer", "The water mesh renderer order within a sorting layer.");
        private static readonly GUIContent allowMSAALabel = new GUIContent("Allow MSAA", "Allow multi-sample anti-aliasing rendering.");
        private static readonly GUIContent allowHDRLabel = new GUIContent("Allow HDR", "Allow high dynamic range rendering.");
        #endregion

        #region Sound Effect Properties
        private static readonly GUIContent soundEffectAudioClipLabel = new GUIContent("Audio Clip", "The AudioClip asset to play.");
        private static readonly GUIContent soundEffectMinimumAudioPitchLabel = new GUIContent("Minimum Pitch", "Sets the audio clip’s minimum playback speed. (when ‘Constant Pitch’ is toggled off)");
        private static readonly GUIContent soundEffectMaximumAudioPitchLabel = new GUIContent("Maximum Pitch", "Sets the audio clip’s maximum playback speed. (when constant pitch is toggled off)");
        private static readonly GUIContent soundEffectConstantAudioPitchLabel = new GUIContent("Constant Pitch", "Apply constant audio clip playback speed.");
        private static readonly GUIContent soundEffectAudioPitchLabel = new GUIContent("Pitch", "Sets the audio clip’s playback speed. (when ‘Constant Pitch’ is toggled on)");
        private static readonly GUIContent soundEffectAudioVolumeLabel = new GUIContent("Volume", "Sets the audio clip’s volume.");
        private static readonly GUIContent soundEffectPoolSizeLabel = new GUIContent("Pool Size", "Sets the number of audio source objects that will be created and pooled when the game starts.");
        private static readonly GUIContent soundEffectPoolExpandIfNecessaryLabel = new GUIContent("Can Expand", "Enables/Disables increasing the number of pooled audio source objects at runtime if needed.");
        private static readonly string soundEffectLabel = "Sound Effect";
        #endregion

        #region Particle Effect Properties
        private static readonly GUIContent particleEffectParticleSystemLabel = new GUIContent("Particle System", "Sets the particle effect system to play.");
        private static readonly GUIContent particleEffectPoolSizeLabel = new GUIContent("Pool Size", "Sets the number of particle system objects that will be created and pooled when the game starts.");
        private static readonly GUIContent particleEffectSpawnOffsetLabel = new GUIContent("Spawn Offset", "Shifts the particle system spawn position.");
        private static readonly GUIContent particleEffectStopActionLabel = new GUIContent("Stop Action", "This UnityEvent is triggered when the particle system finishes playing.");
        private static readonly GUIContent particleEffectPoolExpandIfNecessaryLabel = new GUIContent("Can Expand", "Enables/Disables increasing the number of pooled particle system objects at runtime if needed.");
        private static readonly string particleEffectLabel = "Particle Effect";
        #endregion

        #region Misc
        private static readonly GUIContent prefabUtilityFoldoutLabel = new GUIContent("Prefab Utility");
        private static readonly string particleSystemLoopMessage = "Please make sure the particle system is non-looping!";
        private static readonly string disturbancePropertiesLabel = "Disturbance Properties";
        private static readonly string noiseTextureShaderPropertyName = "_NoiseTexture";
#if UNITY_2018_3_OR_NEWER
        private static readonly string newPrefabWorkflowMessage = "As of Unity 2018.3, disconnecting (unlinking) and relinking a Prefab instance are no longer supported. Alternatively, you can now unpack a Prefab instance if you want to entirely remove its link to its Prefab asset and thus be able to restructure the resulting plain GameObject as you please.";
#endif

        #endregion

        #endregion

        #region Misc
        private static AnimBool waterPropertiesExpanded = new AnimBool();
        private static AnimBool onCollisionRipplesPropertiesExpanded = new AnimBool();
        private static AnimBool constantRipplesPropertiesExpanded = new AnimBool();
        private static AnimBool scriptGeneratedRipplesPropertiesExpanded = new AnimBool();
        private static AnimBool refractionPropertiesExpanded = new AnimBool();
        private static AnimBool reflectionPropertiesExpanded = new AnimBool();
        private static AnimBool renderingSettingsExpanded = new AnimBool();
        private static AnimBool prefabUtilityExpanded = new AnimBool();

        private static AnimBool meshPropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool wavePropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool miscPropertiesBoxGroupExpanded = new AnimBool();

        private static AnimBool collisionRipplesDisturbancePropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool collisionRipplesCollisionPropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool collisionRipplesOnWaterEnterRipplesPropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool collisionRipplesOnWaterEnterRipplesEventsPropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool collisionRipplesOnWaterEnterRipplesSoundEffectPropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool collisionRipplesOnWaterEnterRipplesParticleffectPropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool collisionRipplesOnWaterExitRipplesPropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool collisionRipplesOnWaterExitRipplesEventsPropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool collisionRipplesOnWaterExitRipplesSoundEffectPropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool collisionRipplesOnWaterExitRipplesParticleffectPropertiesBoxGroupExpanded = new AnimBool();

        private static AnimBool constantRipplesDisturbancePropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool constantRipplesTimeIntervalPropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool constantRipplesSourcePositionsPropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool constantRipplesSoundEffectPropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool constantRipplesParticleEffectPropertiesBoxGroupExpanded = new AnimBool();

        private static AnimBool scriptGeneratedRipplesDisturbancePropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool scriptGeneratedRipplesSoundEffectPropertiesBoxGroupExpanded = new AnimBool();
        private static AnimBool scriptGeneratedRipplesParticleffectPropertiesBoxGroupExpanded = new AnimBool();

        private static AnimBool reflectionCameraViewingFrustumHeightPropertiesBoxGroupExpanded = new AnimBool();

        private static readonly Color wireframeColor = new Color(0.89f, 0.259f, 0.204f, 0.375f);
        private static readonly Color constantRipplesSourcesColorAdd = Color.green;
        private static readonly Color constantRipplesSourcesColorRemove = Color.red;
        private static readonly Color buoyancyEffectorSurfaceLevelGuidelineColor = Color.cyan;

        private static GUIStyle helpBoxStyle;
        private static GUIStyle groupBoxStyle;

        private bool isMultiEditing = false;
        private bool constantRipplesEditSourcesPositions = false;
        private string prefabsPath;
        private UnityAction repaint;

        private WaterResizerUtility waterResizerUtility;
        #endregion

        #endregion

        #region Methods

        private void OnEnable()
        {
            foreach (Game2DWater water2D in targets)
            {
                if (!water2D.enabled && !water2D.IsInitialized)
                {
                    water2D.InitializeModules();
                }
            }

            repaint = new UnityAction(Repaint);
            isMultiEditing = targets.Length > 1;

            //Water Properties
            //Mesh Properties
            waterSize = serializedObject.FindProperty("waterSize");
            subdivisionsCountPerUnit = serializedObject.FindProperty("subdivisionsCountPerUnit");
            waterPropertiesExpanded.valueChanged.AddListener(repaint);
            //Water Wave Properties
            damping = serializedObject.FindProperty("damping");
            stiffness = serializedObject.FindProperty("stiffness");
            spread = serializedObject.FindProperty("spread");
            useCustomBoundaries = serializedObject.FindProperty("useCustomBoundaries");
            firstCustomBoundary = serializedObject.FindProperty("firstCustomBoundary");
            secondCustomBoundary = serializedObject.FindProperty("secondCustomBoundary");
            //Misc Properties
            buoyancyEffectorSurfaceLevel = serializedObject.FindProperty("buoyancyEffectorSurfaceLevel");

            //On-Collision Ripples Properties
            onCollisionRipplesPropertiesExpanded.valueChanged.AddListener(repaint);
            //Disturbance Properties
            onCollisionRipplesMinimumDisturbance = serializedObject.FindProperty("onCollisionRipplesMinimumDisturbance");
            onCollisionRipplesMaximumDisturbance = serializedObject.FindProperty("onCollisionRipplesMaximumDisturbance");
            onCollisionRipplesVelocityMultiplier = serializedObject.FindProperty("onCollisionRipplesVelocityMultiplier");
            //Collision Properties
            onCollisionRipplesCollisionMask = serializedObject.FindProperty("onCollisionRipplesCollisionMask");
            onCollisionRipplesCollisionMinimumDepth = serializedObject.FindProperty("onCollisionRipplesCollisionMinimumDepth");
            onCollisionRipplesCollisionMaximumDepth = serializedObject.FindProperty("onCollisionRipplesCollisionMaximumDepth");
            onCollisionRipplesCollisionRaycastMaxDistance = serializedObject.FindProperty("onCollisionRipplesCollisionRaycastMaxDistance");
            //On Water Enter Ripples Properties
            activateOnCollisionOnWaterEnterRipples = serializedObject.FindProperty("activateOnCollisionOnWaterEnterRipples");
            onWaterEnter = serializedObject.FindProperty("onWaterEnter");
            //Sound Effect Properies (On Water Enter)
            onCollisionRipplesActivateOnWaterEnterSoundEffect = serializedObject.FindProperty("onCollisionRipplesActivateOnWaterEnterSoundEffect");
            onCollisionRipplesOnWaterEnterAudioClip = serializedObject.FindProperty("onCollisionRipplesOnWaterEnterAudioClip");
            onCollisionRipplesOnWaterEnterMinimumAudioPitch = serializedObject.FindProperty("onCollisionRipplesOnWaterEnterMinimumAudioPitch");
            onCollisionRipplesOnWaterEnterMaximumAudioPitch = serializedObject.FindProperty("onCollisionRipplesOnWaterEnterMaximumAudioPitch");
            onCollisionRipplesUseConstantOnWaterEnterAudioPitch = serializedObject.FindProperty("onCollisionRipplesUseConstantOnWaterEnterAudioPitch");
            onCollisionRipplesOnWaterEnterAudioPitch = serializedObject.FindProperty("onCollisionRipplesOnWaterEnterAudioPitch");
            onCollisionRipplesOnWaterEnterAudioVolume = serializedObject.FindProperty("onCollisionRipplesOnWaterEnterAudioVolume");
            onCollisionRipplesOnWaterEnterSoundEffectPoolSize = serializedObject.FindProperty("onCollisionRipplesOnWaterEnterSoundEffectPoolSize");
            onCollisionRipplesOnWaterEnterSoundEffectPoolExpandIfNecessary = serializedObject.FindProperty("onCollisionRipplesOnWaterEnterSoundEffectPoolExpandIfNecessary");
            //Particle Effect Properties (OnWaterEnter)
            onCollisionRipplesActivateOnWaterEnterParticleEffect = serializedObject.FindProperty("onCollisionRipplesActivateOnWaterEnterParticleEffect");
            onCollisionRipplesOnWaterEnterParticleEffect = serializedObject.FindProperty("onCollisionRipplesOnWaterEnterParticleEffect");
            onCollisionRipplesOnWaterEnterParticleEffectPoolSize = serializedObject.FindProperty("onCollisionRipplesOnWaterEnterParticleEffectPoolSize");
            onCollisionRipplesOnWaterEnterParticleEffectSpawnOffset = serializedObject.FindProperty("onCollisionRipplesOnWaterEnterParticleEffectSpawnOffset");
            onCollisionRipplesOnWaterEnterParticleEffectStopAction = serializedObject.FindProperty("onCollisionRipplesOnWaterEnterParticleEffectStopAction");
            onCollisionRipplesOnWaterEnterParticleEffectPoolExpandIfNecessary = serializedObject.FindProperty("onCollisionRipplesOnWaterEnterParticleEffectPoolExpandIfNecessary");
            //On Water Exit Ripples Properties
            activateOnCollisionOnWaterExitRipples = serializedObject.FindProperty("activateOnCollisionOnWaterExitRipples");
            onWaterExit = serializedObject.FindProperty("onWaterExit");
            //Sound Effect Properies (On Water Exit)
            onCollisionRipplesActivateOnWaterExitSoundEffect = serializedObject.FindProperty("onCollisionRipplesActivateOnWaterExitSoundEffect");
            onCollisionRipplesOnWaterExitAudioClip = serializedObject.FindProperty("onCollisionRipplesOnWaterExitAudioClip");
            onCollisionRipplesOnWaterExitMinimumAudioPitch = serializedObject.FindProperty("onCollisionRipplesOnWaterExitMinimumAudioPitch");
            onCollisionRipplesOnWaterExitMaximumAudioPitch = serializedObject.FindProperty("onCollisionRipplesOnWaterExitMaximumAudioPitch");
            onCollisionRipplesUseConstantOnWaterExitAudioPitch = serializedObject.FindProperty("onCollisionRipplesUseConstantOnWaterExitAudioPitch");
            onCollisionRipplesOnWaterExitAudioPitch = serializedObject.FindProperty("onCollisionRipplesOnWaterExitAudioPitch");
            onCollisionRipplesOnWaterExitAudioVolume = serializedObject.FindProperty("onCollisionRipplesOnWaterExitAudioVolume");
            onCollisionRipplesOnWaterExitSoundEffectPoolSize = serializedObject.FindProperty("onCollisionRipplesOnWaterExitSoundEffectPoolSize");
            onCollisionRipplesOnWaterExitSoundEffectPoolExpandIfNecessary = serializedObject.FindProperty("onCollisionRipplesOnWaterExitSoundEffectPoolExpandIfNecessary");
            //Particle Effect Properties (On Water Exit)
            onCollisionRipplesActivateOnWaterExitParticleEffect = serializedObject.FindProperty("onCollisionRipplesActivateOnWaterExitParticleEffect");
            onCollisionRipplesOnWaterExitParticleEffect = serializedObject.FindProperty("onCollisionRipplesOnWaterExitParticleEffect");
            onCollisionRipplesOnWaterExitParticleEffectPoolSize = serializedObject.FindProperty("onCollisionRipplesOnWaterExitParticleEffectPoolSize");
            onCollisionRipplesOnWaterExitParticleEffectSpawnOffset = serializedObject.FindProperty("onCollisionRipplesOnWaterExitParticleEffectSpawnOffset");
            onCollisionRipplesOnWaterExitParticleEffectStopAction = serializedObject.FindProperty("onCollisionRipplesOnWaterExitParticleEffectStopAction");
            onCollisionRipplesOnWaterExitParticleEffectPoolExpandIfNecessary = serializedObject.FindProperty("onCollisionRipplesOnWaterExitParticleEffectPoolExpandIfNecessary");

            //Constant Ripples Properties
            activateConstantRipples = serializedObject.FindProperty("activateConstantRipples");
            constantRipplesPropertiesExpanded.valueChanged.AddListener(repaint);
            //Disturbance Properties
            constantRipplesDisturbance = serializedObject.FindProperty("constantRipplesDisturbance");
            constantRipplesUpdateWhenOffscreen = serializedObject.FindProperty("constantRipplesUpdateWhenOffscreen");
            constantRipplesRandomizeDisturbance = serializedObject.FindProperty("constantRipplesRandomizeDisturbance");
            constantRipplesMinimumDisturbance = serializedObject.FindProperty("constantRipplesMinimumDisturbance");
            constantRipplesMaximumDisturbance = serializedObject.FindProperty("constantRipplesMaximumDisturbance");
            constantRipplesSmoothDisturbance = serializedObject.FindProperty("constantRipplesSmoothDisturbance");
            constantRipplesSmoothFactor = serializedObject.FindProperty("constantRipplesSmoothFactor");
            //Time Interval Proeprties
            constantRipplesRandomizeInterval = serializedObject.FindProperty("constantRipplesRandomizeInterval");
            constantRipplesInterval = serializedObject.FindProperty("constantRipplesInterval");
            constantRipplesMinimumInterval = serializedObject.FindProperty("constantRipplesMinimumInterval");
            constantRipplesMaximumInterval = serializedObject.FindProperty("constantRipplesMaximumInterval");
            //Ripple Source Position
            constantRipplesRandomizeRipplesSourcesPositions = serializedObject.FindProperty("constantRipplesRandomizeRipplesSourcesPositions");
            constantRipplesRandomizeRipplesSourcesCount = serializedObject.FindProperty("constantRipplesRandomizeRipplesSourcesCount");
            constantRipplesSourcePositions = serializedObject.FindProperty("constantRipplesSourcePositions");
            constantRipplesAllowDuplicateRipplesSourcesPositions = serializedObject.FindProperty("constantRipplesAllowDuplicateRipplesSourcesPositions");
            //Sound Effect Properties
            constantRipplesActivateSoundEffect = serializedObject.FindProperty("constantRipplesActivateSoundEffect");
            constantRipplesUseConstantAudioPitch = serializedObject.FindProperty("constantRipplesUseConstantAudioPitch");
            constantRipplesAudioPitch = serializedObject.FindProperty("constantRipplesAudioPitch");
            constantRipplesAudioVolume = serializedObject.FindProperty("constantRipplesAudioVolume");
            constantRipplesMinimumAudioPitch = serializedObject.FindProperty("constantRipplesMinimumAudioPitch");
            constantRipplesMaximumAudioPitch = serializedObject.FindProperty("constantRipplesMaximumAudioPitch");
            constantRipplesAudioClip = serializedObject.FindProperty("constantRipplesAudioClip");
            constantRipplesSoundEffectPoolSize = serializedObject.FindProperty("constantRipplesSoundEffectPoolSize");
            constantRipplesSoundEffectPoolExpandIfNecessary = serializedObject.FindProperty("constantRipplesSoundEffectPoolExpandIfNecessary");
            //Particle Effect Proeprties
            constantRipplesActivateParticleEffect = serializedObject.FindProperty("constantRipplesActivateParticleEffect");
            constantRipplesParticleEffect = serializedObject.FindProperty("constantRipplesParticleEffect");
            constantRipplesParticleEffectPoolSize = serializedObject.FindProperty("constantRipplesParticleEffectPoolSize");
            constantRipplesParticleEffectSpawnOffset = serializedObject.FindProperty("constantRipplesParticleEffectSpawnOffset");
            constantRipplesParticleEffectStopAction = serializedObject.FindProperty("constantRipplesParticleEffectStopAction");
            constantRipplesParticleEffectPoolExpandIfNecessary = serializedObject.FindProperty("constantRipplesParticleEffectPoolExpandIfNecessary");

            //Script-Generated Ripples Properties
            scriptGeneratedRipplesPropertiesExpanded.valueChanged.AddListener(repaint);
            //Disturbance Properties
            scriptGeneratedRipplesMinimumDisturbance = serializedObject.FindProperty("scriptGeneratedRipplesMinimumDisturbance");
            scriptGeneratedRipplesMaximumDisturbance = serializedObject.FindProperty("scriptGeneratedRipplesMaximumDisturbance");
            //Sound Effect Properties
            scriptGeneratedRipplesActivateSoundEffect = serializedObject.FindProperty("scriptGeneratedRipplesActivateSoundEffect");
            scriptGeneratedRipplesUseConstantAudioPitch = serializedObject.FindProperty("scriptGeneratedRipplesUseConstantAudioPitch");
            scriptGeneratedRipplesAudioPitch = serializedObject.FindProperty("scriptGeneratedRipplesAudioPitch");
            scriptGeneratedRipplesAudioVolume = serializedObject.FindProperty("scriptGeneratedRipplesAudioVolume");
            scriptGeneratedRipplesMinimumAudioPitch = serializedObject.FindProperty("scriptGeneratedRipplesMinimumAudioPitch");
            scriptGeneratedRipplesMaximumAudioPitch = serializedObject.FindProperty("scriptGeneratedRipplesMaximumAudioPitch");
            scriptGeneratedRipplesAudioClip = serializedObject.FindProperty("scriptGeneratedRipplesAudioClip");
            scriptGeneratedRipplesSoundEffectPoolSize = serializedObject.FindProperty("scriptGeneratedRipplesSoundEffectPoolSize");
            scriptGeneratedRipplesSoundEffectPoolExpandIfNecessary = serializedObject.FindProperty("scriptGeneratedRipplesSoundEffectPoolExpandIfNecessary");
            //Particle Effect Properties
            scriptGeneratedRipplesActivateParticleEffect = serializedObject.FindProperty("scriptGeneratedRipplesActivateParticleEffect");
            scriptGeneratedRipplesParticleEffect = serializedObject.FindProperty("scriptGeneratedRipplesParticleEffect");
            scriptGeneratedRipplesParticleEffectPoolSize = serializedObject.FindProperty("scriptGeneratedRipplesParticleEffectPoolSize");
            scriptGeneratedRipplesParticleEffectSpawnOffset = serializedObject.FindProperty("scriptGeneratedRipplesParticleEffectSpawnOffset");
            scriptGeneratedRipplesParticleEffectStopAction = serializedObject.FindProperty("scriptGeneratedRipplesParticleEffectStopAction");
            scriptGeneratedRipplesParticleEffectPoolExpandIfNecessary = serializedObject.FindProperty("scriptGeneratedRipplesParticleEffectPoolExpandIfNecessary");

            //Reflection & Refraction Rendering Proeprties
            //Refraction Properties
            refractionRenderTextureResizingFactor = serializedObject.FindProperty("refractionRenderTextureResizeFactor");
            refractionCullingMask = serializedObject.FindProperty("refractionCullingMask");
            refractionPartiallySubmergedObjectsCullingMask = serializedObject.FindProperty("refractionPartiallySubmergedObjectsCullingMask");
            refractionRenderTextureFilterMode = serializedObject.FindProperty("refractionRenderTextureFilterMode");
            refractionPropertiesExpanded.valueChanged.AddListener(repaint);
            //Reflection Properties
            reflectionRenderTextureResizingFactor = serializedObject.FindProperty("reflectionRenderTextureResizeFactor");
            reflectionViewingFrustumHeightScalingFactor = serializedObject.FindProperty("reflectionViewingFrustumHeightScalingFactor");
            reflectionPartiallySubmergedObjectsViewingFrustumHeightScalingFactor = serializedObject.FindProperty("reflectionPartiallySubmergedObjectsViewingFrustumHeightScalingFactor");
            reflectionCullingMask = serializedObject.FindProperty("reflectionCullingMask");
            reflectionPartiallySubmergedObjectsCullingMask = serializedObject.FindProperty("reflectionPartiallySubmergedObjectsCullingMask");
            reflectionZOffset = serializedObject.FindProperty("reflectionZOffset");
            reflectionRenderTextureFilterMode = serializedObject.FindProperty("reflectionRenderTextureFilterMode");
            reflectionPropertiesExpanded.valueChanged.AddListener(repaint);
            //Rendering Properties
            renderPixelLights = serializedObject.FindProperty("renderPixelLights");
            sortingLayerID = serializedObject.FindProperty("sortingLayerID");
            sortingOrder = serializedObject.FindProperty("sortingOrder");
            allowMSAA = serializedObject.FindProperty("allowMSAA");
            allowHDR = serializedObject.FindProperty("allowHDR");
            farClipPlane = serializedObject.FindProperty("farClipPlane");
            renderingSettingsExpanded.valueChanged.AddListener(repaint);

            //Prefabs Utility
            prefabUtilityExpanded.valueChanged.AddListener(repaint);
            prefabsPath = EditorPrefs.GetString("Water2D_Paths_PrefabUtility_Path", "Assets/");

            //Utilities
            waterResizerUtility = new WaterResizerUtility(repaint)
            {
                IsActive = !isMultiEditing
            };

            //box groups animBools
            meshPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            wavePropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            miscPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);

            collisionRipplesDisturbancePropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            collisionRipplesCollisionPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            collisionRipplesOnWaterEnterRipplesPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            collisionRipplesOnWaterEnterRipplesEventsPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            collisionRipplesOnWaterEnterRipplesSoundEffectPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            collisionRipplesOnWaterEnterRipplesParticleffectPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            collisionRipplesOnWaterExitRipplesPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            collisionRipplesOnWaterExitRipplesEventsPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            collisionRipplesOnWaterExitRipplesSoundEffectPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            collisionRipplesOnWaterExitRipplesParticleffectPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);

            constantRipplesDisturbancePropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            constantRipplesTimeIntervalPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            constantRipplesSourcePositionsPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            constantRipplesSoundEffectPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            constantRipplesParticleEffectPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);

            scriptGeneratedRipplesDisturbancePropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            scriptGeneratedRipplesSoundEffectPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
            scriptGeneratedRipplesParticleffectPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);

            reflectionCameraViewingFrustumHeightPropertiesBoxGroupExpanded.valueChanged.AddListener(repaint);
        }

        private void OnDisable()
        {
            //Water Proeperties
            waterPropertiesExpanded.valueChanged.RemoveListener(repaint);
            //On-Collision Ripples Properties
            onCollisionRipplesPropertiesExpanded.valueChanged.RemoveListener(repaint);
            //Constant Ripples Properties
            constantRipplesPropertiesExpanded.valueChanged.RemoveListener(repaint);
            //Script-Generated Ripples Properties
            scriptGeneratedRipplesPropertiesExpanded.valueChanged.RemoveListener(repaint);
            //Refraction & Reflection Rendering Properties
            refractionPropertiesExpanded.valueChanged.RemoveListener(repaint);
            reflectionPropertiesExpanded.valueChanged.RemoveListener(repaint);
            renderingSettingsExpanded.valueChanged.RemoveListener(repaint);
            //Prefabs Utility
            prefabUtilityExpanded.valueChanged.RemoveListener(repaint);
            EditorPrefs.SetString("Water2D_Paths_PrefabUtility_Path", prefabsPath);

            //box groups animBools
            meshPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            wavePropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            miscPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);

            collisionRipplesDisturbancePropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            collisionRipplesCollisionPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            collisionRipplesOnWaterEnterRipplesPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            collisionRipplesOnWaterEnterRipplesEventsPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            collisionRipplesOnWaterEnterRipplesSoundEffectPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            collisionRipplesOnWaterEnterRipplesParticleffectPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            collisionRipplesOnWaterExitRipplesPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            collisionRipplesOnWaterExitRipplesEventsPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            collisionRipplesOnWaterExitRipplesSoundEffectPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            collisionRipplesOnWaterExitRipplesParticleffectPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);

            constantRipplesDisturbancePropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            constantRipplesTimeIntervalPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            constantRipplesSourcePositionsPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            constantRipplesSoundEffectPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            constantRipplesParticleEffectPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);

            scriptGeneratedRipplesDisturbancePropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            scriptGeneratedRipplesSoundEffectPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
            scriptGeneratedRipplesParticleffectPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);

            reflectionCameraViewingFrustumHeightPropertiesBoxGroupExpanded.valueChanged.RemoveListener(repaint);
        }

        #region Inspector

        public override void OnInspectorGUI()
        {
            Game2DWater water2D = target as Game2DWater;
            UnityEngine.Material water2DMaterial = water2D.GetComponent<MeshRenderer>().sharedMaterial;
            bool hasRefraction = false;
            bool hasReflection = false;
            bool hasFakePerspectve = false;
            if (water2DMaterial)
            {
                hasRefraction = water2DMaterial.IsKeywordEnabled("Water2D_Refraction");
                hasReflection = water2DMaterial.IsKeywordEnabled("Water2D_Reflection");
                hasFakePerspectve = water2DMaterial.IsKeywordEnabled("Water2D_FakePerspective");
            }

            if(helpBoxStyle == null)
                helpBoxStyle = new GUIStyle("HelpBox");
            if(groupBoxStyle == null)
                groupBoxStyle = new GUIStyle("GroupBox");

            serializedObject.Update();

            if (waterResizerUtility.IsActive && waterResizerUtility.HasChanged)
            {
                waterResizerUtility.HasChanged = false;
                waterSize.vector2Value = waterResizerUtility.WaterSize;
                Undo.RecordObject(water2D.transform, "changing water size/position");
                water2D.transform.position = waterResizerUtility.WaterPosition;
            }

            if (!isMultiEditing)
            {
                DrawFixScalingField(water2D);
            }

            DrawWaterProperties(water2D);
            DrawOnCollisionRipplesProperties(water2D);
            DrawConstantRipplesProperties(water2D);
            DrawScriptGeneratedRipplesProperties(water2D);
            DrawRefractionProperties(hasRefraction, hasFakePerspectve);
            DrawReflectionProperties(hasReflection, hasFakePerspectve);
            DrawRenderingSettingsProperties(hasRefraction, hasReflection);

#if UNITY_2018_3_OR_NEWER
            //Editing the Prefab GameObjects in the Project Browser is no longer supported as of Unity 2018.3 due to the technical changes in the Prefabs back-end.
            bool isPrefabSelectedInProjectBrowser = Application.isPlaying || UnityEditor.SceneManagement.EditorSceneManager.IsPreviewSceneObject(water2D.gameObject);
#else
            bool isPrefabSelectedInProjectBrowser = PrefabUtility.GetPrefabType(water2D) == PrefabType.Prefab;
#endif

            if (!isMultiEditing && !isPrefabSelectedInProjectBrowser)
                DrawPrefabUtility(water2D, water2DMaterial);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawWaterProperties(Game2DWater water2D)
        {
            waterPropertiesExpanded.target = EditorGUILayout.Foldout(waterPropertiesExpanded.target, waterPropertiesFoldoutLabel, true);
            using (EditorGUILayout.FadeGroupScope group = new EditorGUILayout.FadeGroupScope(waterPropertiesExpanded.faded))
            {
                if (group.visible)
                {
                    //Mesh Properties
                    using (BoxGroupScope boxGroup = new BoxGroupScope(meshPropertiesLabel, meshPropertiesBoxGroupExpanded, 155f, 0f, true))
                    {
                        if (boxGroup.IsFaded)
                        {
                            EditorGUI.BeginChangeCheck();
                            waterResizerUtility.IsActive = !isMultiEditing && GUILayout.Toggle(waterResizerUtility.IsActive, "Edit Water Size", "Button");
                            EditorGUILayout.PropertyField(waterSize, waterSizeLabel);
                            EditorGUILayout.PropertyField(subdivisionsCountPerUnit, subdivisionsCountPerUnitLabel);
                        }
                    }

                    //Wave Properties
                    using (BoxGroupScope boxGroup = new BoxGroupScope(wavePropertiesLabel, wavePropertiesBoxGroupExpanded, 155f, 0f, true))
                    {
                        if (boxGroup.IsFaded)
                        {
                            EditorGUILayout.PropertyField(stiffness, stiffnessLabel);
                            EditorGUILayout.PropertyField(spread, spreadLabel);
                            EditorGUILayout.Slider(damping, 0f, 1f, dampingLabel);
                            EditorGUILayout.PropertyField(useCustomBoundaries, useCustomBoundariesLabel);
                            if (useCustomBoundaries.boolValue)
                            {
                                EditorGUILayout.PropertyField(firstCustomBoundary, firstCustomBoundaryLabel);
                                EditorGUILayout.PropertyField(secondCustomBoundary, secondCustomBoundaryLabel);
                            }
                        }
                    }

                    //Misc
                    using (BoxGroupScope boxGroup = new BoxGroupScope(miscLabel, miscPropertiesBoxGroupExpanded, 155f, 0f, true))
                    {
                        if (boxGroup.IsFaded)
                        {
                            EditorGUILayout.Slider(buoyancyEffectorSurfaceLevel, 0f, 1f, buoyancyEffectorSurfaceLevelLabel);
                            if (!isMultiEditing)
                            {
                                DrawEdgeColliderPropertyField(water2D);
                            }
                        }
                    }
                }
            }
        }

        private void DrawFixScalingField(Game2DWater water2D)
        {
            Vector2 scale = water2D.transform.localScale;
            if (!Mathf.Approximately(scale.x, 1f) || !Mathf.Approximately(scale.y, 1f))
            {
                EditorGUILayout.HelpBox(nonUniformScaleWarning, MessageType.Warning, true);
                if (GUILayout.Button(fixScalingButtonLabel))
                {
                    waterSize.vector2Value = Vector2.Scale(waterSize.vector2Value, scale);
                    Undo.RecordObject(water2D.transform, "changing water size/position");
                    water2D.transform.localScale = Vector3.one;
                }
            }
        }

        private void DrawEdgeColliderPropertyField(Game2DWater water2D)
        {
            EditorGUI.BeginChangeCheck();
            bool useEdgeCollider = EditorGUILayout.Toggle(useEdgeCollider2DLabel, water2D.GetComponent<EdgeCollider2D>() != null);
            if (EditorGUI.EndChangeCheck())
            {
                if (useEdgeCollider)
                {
                    water2D.gameObject.AddComponent<EdgeCollider2D>();
                    water2D.OnValidate();
                }
                else
                {
                    DestroyImmediate(water2D.GetComponent<EdgeCollider2D>());
                }
            }
        }

        private void DrawOnCollisionRipplesProperties(Game2DWater water2D)
        {
            onCollisionRipplesPropertiesExpanded.target = EditorGUILayout.Foldout(onCollisionRipplesPropertiesExpanded.target, onCollisionRipplesPropertiesFoldoutLabel, true);
            using (EditorGUILayout.FadeGroupScope group = new EditorGUILayout.FadeGroupScope(onCollisionRipplesPropertiesExpanded.faded))
            {
                if (group.visible)
                {
                    //Disturbance Proeprties
                    using (BoxGroupScope boxGroup = new BoxGroupScope(disturbancePropertiesLabel, collisionRipplesDisturbancePropertiesBoxGroupExpanded, 155f, 0f, true))
                    {
                        if (boxGroup.IsFaded)
                        {
                            EditorGUILayout.PropertyField(onCollisionRipplesMinimumDisturbance, onCollisionRipplesMinimumDisturbanceLabel);
                            EditorGUILayout.PropertyField(onCollisionRipplesMaximumDisturbance, onCollisionRipplesMaximumDisturbanceLabel);
                            EditorGUILayout.PropertyField(onCollisionRipplesVelocityMultiplier, onCollisionRipplesVelocityMultiplierLabel);
                        }
                    }

                    //Collision Properties
                    using (BoxGroupScope boxGroup = new BoxGroupScope(collisionPropertiesLabel, collisionRipplesCollisionPropertiesBoxGroupExpanded, 155f, 0f, true))
                    {
                        if (boxGroup.IsFaded)
                        {
                            EditorGUILayout.PropertyField(onCollisionRipplesCollisionMask, onCollisionRipplesCollisionMaskLabel);
                            EditorGUILayout.PropertyField(onCollisionRipplesCollisionMinimumDepth, onCollisionRipplesCollisionMinimumDepthLabel);
                            EditorGUILayout.PropertyField(onCollisionRipplesCollisionMaximumDepth, onCollisionRipplesCollisionMaximumDepthLabel);
                            EditorGUILayout.PropertyField(onCollisionRipplesCollisionRaycastMaxDistance, onCollisionRipplesCollisionRaycastMaxDistanceLabel);
                        }
                    }

                    //On Water Enter Ripples Properties
                    using (BoxGroupScope boxGroup = new BoxGroupScope(onWaterEnterRipplesPropertiesLabel, activateOnCollisionOnWaterEnterRipples, collisionRipplesOnWaterEnterRipplesPropertiesBoxGroupExpanded, 0f, 0f, true))
                    {
                        if (boxGroup.IsFaded)
                        {
                            //OnWaterEnterEvent
                            using (BoxGroupScope subBoxGroup = new BoxGroupScope(eventsLabel, collisionRipplesOnWaterEnterRipplesEventsPropertiesBoxGroupExpanded, 0f, 0f, false))
                            {
                                if (subBoxGroup.IsFaded)
                                {
                                    EditorGUILayout.PropertyField(onWaterEnter, onWaterEnterLabel);
                                }
                            }

                            //Sound Effect
                            using (BoxGroupScope subBoxGroup = new BoxGroupScope(soundEffectLabel, onCollisionRipplesActivateOnWaterEnterSoundEffect, collisionRipplesOnWaterEnterRipplesSoundEffectPropertiesBoxGroupExpanded, 135f, 0f, false))
                            {
                                if (subBoxGroup.IsFaded)
                                {
                                    EditorGUI.BeginChangeCheck();
                                    EditorGUILayout.PropertyField(onCollisionRipplesOnWaterEnterAudioClip, soundEffectAudioClipLabel);
                                    EditorGUILayout.PropertyField(onCollisionRipplesOnWaterEnterSoundEffectPoolSize, soundEffectPoolSizeLabel);
                                    EditorGUILayout.PropertyField(onCollisionRipplesOnWaterEnterSoundEffectPoolExpandIfNecessary, soundEffectPoolExpandIfNecessaryLabel);

                                    EditorGUILayout.Slider(onCollisionRipplesOnWaterEnterAudioVolume, 0f, 1f, soundEffectAudioVolumeLabel);
                                    EditorGUILayout.PropertyField(onCollisionRipplesUseConstantOnWaterEnterAudioPitch, soundEffectConstantAudioPitchLabel);
                                    if (onCollisionRipplesUseConstantOnWaterEnterAudioPitch.boolValue)
                                    {
                                        EditorGUILayout.Slider(onCollisionRipplesOnWaterEnterAudioPitch, -3f, 3f, soundEffectAudioPitchLabel);
                                    }
                                    else
                                    {
                                        EditorGUILayout.Slider(onCollisionRipplesOnWaterEnterMinimumAudioPitch, -3f, 3f, soundEffectMinimumAudioPitchLabel);
                                        EditorGUILayout.Slider(onCollisionRipplesOnWaterEnterMaximumAudioPitch, -3f, 3f, soundEffectMaximumAudioPitchLabel);
                                        EditorGUILayout.HelpBox(onCollisionRipplesOnWaterEnterAudioPitchMessage, MessageType.None, true);
                                    }
                                }
                            }

                            //Particle Effect
                            using (BoxGroupScope subBoxGroup = new BoxGroupScope(particleEffectLabel, onCollisionRipplesActivateOnWaterEnterParticleEffect, collisionRipplesOnWaterEnterRipplesParticleffectPropertiesBoxGroupExpanded, 135f, 0f, false))
                            {
                                if (subBoxGroup.IsFaded)
                                {
                                    ParticleSystem particleSystem = onCollisionRipplesOnWaterEnterParticleEffect.objectReferenceValue as ParticleSystem;
                                    if (particleSystem != null && particleSystem.main.loop)
                                    {
                                        EditorGUILayout.HelpBox(particleSystemLoopMessage, MessageType.Warning);
                                    }

                                    EditorGUI.BeginChangeCheck();
                                    EditorGUILayout.PropertyField(onCollisionRipplesOnWaterEnterParticleEffect, particleEffectParticleSystemLabel);
                                    EditorGUILayout.DelayedIntField(onCollisionRipplesOnWaterEnterParticleEffectPoolSize, particleEffectPoolSizeLabel);
                                    EditorGUILayout.PropertyField(onCollisionRipplesOnWaterEnterParticleEffectPoolExpandIfNecessary, particleEffectPoolExpandIfNecessaryLabel);
                                    EditorGUILayout.PropertyField(onCollisionRipplesOnWaterEnterParticleEffectSpawnOffset, particleEffectSpawnOffsetLabel);
                                    EditorGUILayout.PropertyField(onCollisionRipplesOnWaterEnterParticleEffectStopAction, particleEffectStopActionLabel);
                                }
                            }
                        }
                    }

                    //On Water Exit Ripples Properties
                    using (BoxGroupScope boxGroup = new BoxGroupScope(onWaterExitRipplesPropertiesLabel, activateOnCollisionOnWaterExitRipples, collisionRipplesOnWaterExitRipplesPropertiesBoxGroupExpanded, 0f, 0f, true))
                    {
                        if (boxGroup.IsFaded)
                        {
                            //OnWaterExitEvent
                            using (BoxGroupScope subBoxGroup = new BoxGroupScope(eventsLabel, collisionRipplesOnWaterExitRipplesEventsPropertiesBoxGroupExpanded, 0f, 0f, false))
                            {
                                if (subBoxGroup.IsFaded)
                                {
                                    EditorGUILayout.PropertyField(onWaterExit, onWaterExitLabel);
                                }
                            }

                            //Sound Effect
                            using (BoxGroupScope subBoxGroup = new BoxGroupScope(soundEffectLabel, onCollisionRipplesActivateOnWaterExitSoundEffect, collisionRipplesOnWaterExitRipplesSoundEffectPropertiesBoxGroupExpanded, 135f, 0f, false))
                            {
                                if (subBoxGroup.IsFaded)
                                {
                                    EditorGUI.BeginChangeCheck();
                                    EditorGUILayout.PropertyField(onCollisionRipplesOnWaterExitAudioClip, soundEffectAudioClipLabel);
                                    EditorGUILayout.PropertyField(onCollisionRipplesOnWaterExitSoundEffectPoolSize, soundEffectPoolSizeLabel);
                                    EditorGUILayout.PropertyField(onCollisionRipplesOnWaterExitSoundEffectPoolExpandIfNecessary, soundEffectPoolExpandIfNecessaryLabel);

                                    EditorGUILayout.Slider(onCollisionRipplesOnWaterExitAudioVolume, 0f, 1f, soundEffectAudioVolumeLabel);
                                    EditorGUILayout.PropertyField(onCollisionRipplesUseConstantOnWaterExitAudioPitch, soundEffectConstantAudioPitchLabel);
                                    if (onCollisionRipplesUseConstantOnWaterExitAudioPitch.boolValue)
                                    {
                                        EditorGUILayout.Slider(onCollisionRipplesOnWaterExitAudioPitch, -3f, 3f, soundEffectAudioPitchLabel);
                                    }
                                    else
                                    {
                                        EditorGUILayout.Slider(onCollisionRipplesOnWaterExitMinimumAudioPitch, -3f, 3f, soundEffectMinimumAudioPitchLabel);
                                        EditorGUILayout.Slider(onCollisionRipplesOnWaterExitMaximumAudioPitch, -3f, 3f, soundEffectMaximumAudioPitchLabel);
                                        EditorGUILayout.HelpBox(onCollisionRipplesOnWaterExitAudioPitchMessage, MessageType.None, true);
                                    }
                                }
                            }

                            //Particle Effect
                            using (BoxGroupScope subBoxGroup = new BoxGroupScope(particleEffectLabel, onCollisionRipplesActivateOnWaterExitParticleEffect, collisionRipplesOnWaterExitRipplesParticleffectPropertiesBoxGroupExpanded, 135f, 0f, false))
                            {
                                if (subBoxGroup.IsFaded)
                                {
                                    ParticleSystem particleSystem = onCollisionRipplesOnWaterExitParticleEffect.objectReferenceValue as ParticleSystem;
                                    if (particleSystem != null && particleSystem.main.loop)
                                    {
                                        EditorGUILayout.HelpBox(particleSystemLoopMessage, MessageType.Warning);
                                    }

                                    EditorGUI.BeginChangeCheck();
                                    EditorGUILayout.PropertyField(onCollisionRipplesOnWaterExitParticleEffect, particleEffectParticleSystemLabel);
                                    EditorGUILayout.DelayedIntField(onCollisionRipplesOnWaterExitParticleEffectPoolSize, particleEffectPoolSizeLabel);
                                    EditorGUILayout.PropertyField(onCollisionRipplesOnWaterExitParticleEffectPoolExpandIfNecessary, particleEffectPoolExpandIfNecessaryLabel);
                                    EditorGUILayout.PropertyField(onCollisionRipplesOnWaterExitParticleEffectSpawnOffset, particleEffectSpawnOffsetLabel);
                                    EditorGUILayout.PropertyField(onCollisionRipplesOnWaterExitParticleEffectStopAction, particleEffectStopActionLabel);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DrawConstantRipplesProperties(Game2DWater water2D)
        {
            constantRipplesPropertiesExpanded.target = EditorGUILayout.Foldout(constantRipplesPropertiesExpanded.target, constantRipplesPropertiesFoldoutLabel, true);

            using (EditorGUILayout.FadeGroupScope group = new EditorGUILayout.FadeGroupScope(constantRipplesPropertiesExpanded.faded))
            {
                if (group.visible)
                {
                    EditorGUIUtility.labelWidth = 195f;
                    EditorGUILayout.PropertyField(activateConstantRipples, activateConstantRipplesLabel);
                    EditorGUILayout.PropertyField(constantRipplesUpdateWhenOffscreen, constantRipplesUpdateWhenOffscreenLabel);
                    EditorGUIUtility.labelWidth = 0f;

                    //Disturbance Properties
                    using (BoxGroupScope boxGroup = new BoxGroupScope(disturbancePropertiesLabel, constantRipplesDisturbancePropertiesBoxGroupExpanded, 155f, 130f, true))
                    {
                        if (boxGroup.IsFaded)
                        {
                            EditorGUILayout.PropertyField(constantRipplesRandomizeDisturbance, constantRipplesRandomizeDisturbanceLabel);
                            bool randomizeDisturbance = constantRipplesRandomizeDisturbance.boolValue;
                            if (randomizeDisturbance)
                            {
                                EditorGUILayout.PropertyField(constantRipplesMinimumDisturbance, constantRipplesMinimumDisturbanceLabel);
                                EditorGUILayout.PropertyField(constantRipplesMaximumDisturbance, constantRipplesMaximumDisturbanceLabel);
                            }
                            else
                            {
                                EditorGUILayout.PropertyField(constantRipplesDisturbance, constantRipplesDisturbanceLabel);
                            }
                            EditorGUILayout.PropertyField(constantRipplesSmoothDisturbance, constantRipplesSmoothDisturbanceLabel);
                            bool smoothWave = constantRipplesSmoothDisturbance.boolValue;
                            if (smoothWave)
                            {
                                EditorGUILayout.Slider(constantRipplesSmoothFactor, 0f, 1f, constantRipplesSmoothFactorLabel);
                            }
                        }
                    }

                    //Time Interval Properties
                    using (BoxGroupScope boxGroup = new BoxGroupScope(intervalPropertiesLabel, constantRipplesTimeIntervalPropertiesBoxGroupExpanded, 175f, 130f, true))
                    {
                        if (boxGroup.IsFaded)
                        {
                            EditorGUILayout.PropertyField(constantRipplesRandomizeInterval, randomizePersistnetWaveIntervalLabel);
                            bool randomizeInterval = constantRipplesRandomizeInterval.boolValue;
                            if (randomizeInterval)
                            {
                                EditorGUILayout.PropertyField(constantRipplesMinimumInterval, constantRipplesMinimumIntervalLabel);
                                EditorGUILayout.PropertyField(constantRipplesMaximumInterval, constantRipplesMaximumIntervalLabel);
                            }
                            else
                            {
                                EditorGUILayout.PropertyField(constantRipplesInterval, constantRipplesIntervalLabel);
                            }
                        }
                    }

                    //Ripples Source Properties
                    using (BoxGroupScope boxGroup = new BoxGroupScope(constantRipplesSourcesPropertiesLabel, constantRipplesSourcePositionsPropertiesBoxGroupExpanded, 155f, 130f, true))
                    {
                        if (boxGroup.IsFaded)
                        {
                            EditorGUILayout.PropertyField(constantRipplesRandomizeRipplesSourcesPositions, constantRipplesRandomizeRipplesSourcesPositionsLabel);
                            bool randomizeRipplesSources = constantRipplesRandomizeRipplesSourcesPositions.boolValue;
                            if (!randomizeRipplesSources)
                            {
                                EditorGUILayout.PropertyField(constantRipplesAllowDuplicateRipplesSourcesPositions, constantRipplesAllowDuplicateRipplesSourcesPositionsLabel);
                                EditorGUI.BeginDisabledGroup(isMultiEditing);
                                constantRipplesEditSourcesPositions = GUILayout.Toggle(constantRipplesEditSourcesPositions, constantRipplesEditSourcesPositionsLabel, "Button");
                                constantRipplesSourcePositions.isExpanded |= constantRipplesEditSourcesPositions;
                                EditorGUI.indentLevel++;
                                EditorGUILayout.PropertyField(constantRipplesSourcePositions, constantRipplesSourcePositionsLabel, true);
                                EditorGUI.indentLevel--;
                                EditorGUI.EndDisabledGroup();
                            }
                            else
                            {
                                EditorGUILayout.PropertyField(constantRipplesRandomizeRipplesSourcesCount, constantRipplesRandomizeRipplesSourcesCountLabel);
                                constantRipplesEditSourcesPositions = false;
                            }
                        }
                    }

                    //Sound Effect
                    using (BoxGroupScope boxGroup = new BoxGroupScope(soundEffectLabel, constantRipplesActivateSoundEffect, constantRipplesSoundEffectPropertiesBoxGroupExpanded, 135f, 0f, true))
                    {
                        if (boxGroup.IsFaded)
                        {
                            EditorGUI.BeginChangeCheck();
                            EditorGUILayout.PropertyField(constantRipplesAudioClip, soundEffectAudioClipLabel);
                            EditorGUILayout.PropertyField(constantRipplesSoundEffectPoolSize, soundEffectPoolSizeLabel);
                            EditorGUILayout.PropertyField(constantRipplesSoundEffectPoolExpandIfNecessary, soundEffectPoolExpandIfNecessaryLabel);
                            EditorGUILayout.Slider(constantRipplesAudioVolume, 0f, 1f, soundEffectAudioVolumeLabel);
                            EditorGUILayout.PropertyField(constantRipplesUseConstantAudioPitch, soundEffectConstantAudioPitchLabel);
                            if (constantRipplesUseConstantAudioPitch.boolValue)
                            {
                                EditorGUILayout.Slider(constantRipplesAudioPitch, -3f, 3f, soundEffectAudioPitchLabel);
                            }
                            else
                            {
                                EditorGUILayout.Slider(constantRipplesMinimumAudioPitch, -3f, 3f, soundEffectMinimumAudioPitchLabel);
                                EditorGUILayout.Slider(constantRipplesMaximumAudioPitch, -3f, 3f, soundEffectMaximumAudioPitchLabel);
                                EditorGUILayout.HelpBox(constantRipplesAudioPitchMessage, MessageType.None, true);
                            }
                        }
                    }

                    //Particle Effect
                    using (BoxGroupScope boxGroup = new BoxGroupScope(particleEffectLabel, constantRipplesActivateParticleEffect, constantRipplesParticleEffectPropertiesBoxGroupExpanded, 135f, 0f, true))
                    {
                        if (boxGroup.IsFaded)
                        {
                            ParticleSystem particleSystem = constantRipplesParticleEffect.objectReferenceValue as ParticleSystem;
                            if (particleSystem != null && particleSystem.main.loop)
                            {
                                EditorGUILayout.HelpBox(particleSystemLoopMessage, MessageType.Warning);
                            }

                            EditorGUI.BeginChangeCheck();
                            EditorGUILayout.PropertyField(constantRipplesParticleEffect, particleEffectParticleSystemLabel);
                            EditorGUILayout.DelayedIntField(constantRipplesParticleEffectPoolSize, particleEffectPoolSizeLabel);
                            EditorGUILayout.PropertyField(constantRipplesParticleEffectPoolExpandIfNecessary, particleEffectPoolExpandIfNecessaryLabel);
                            EditorGUILayout.PropertyField(constantRipplesParticleEffectSpawnOffset, particleEffectSpawnOffsetLabel);
                            EditorGUILayout.PropertyField(constantRipplesParticleEffectStopAction, particleEffectStopActionLabel);
                        }
                    }
                }
            }
        }

        private void DrawScriptGeneratedRipplesProperties(Game2DWater water2D)
        {
            scriptGeneratedRipplesPropertiesExpanded.target = EditorGUILayout.Foldout(scriptGeneratedRipplesPropertiesExpanded.target, scriptGeneratedRipplesPropertiesFoldoutLabel, true);
            using (EditorGUILayout.FadeGroupScope group = new EditorGUILayout.FadeGroupScope(scriptGeneratedRipplesPropertiesExpanded.faded))
            {
                if (group.visible)
                {
                    EditorGUILayout.HelpBox(scriptGeneratedRipplesMessage, MessageType.None);

                    using (BoxGroupScope boxGroup = new BoxGroupScope(disturbancePropertiesLabel, scriptGeneratedRipplesDisturbancePropertiesBoxGroupExpanded, 0f, 0f, true))
                    {
                        if (boxGroup.IsFaded)
                        {
                            EditorGUILayout.PropertyField(scriptGeneratedRipplesMinimumDisturbance, scriptGeneratedRipplesMinimumDisturbanceLabel);
                            EditorGUILayout.PropertyField(scriptGeneratedRipplesMaximumDisturbance, scriptGeneratedRipplesMaximumDisturbanceLabel);
                        }
                    }

                    //Sound Effect
                    using (BoxGroupScope boxGroup = new BoxGroupScope(soundEffectLabel, scriptGeneratedRipplesActivateSoundEffect, scriptGeneratedRipplesSoundEffectPropertiesBoxGroupExpanded, 135f, 0f, true))
                    {
                        if (boxGroup.IsFaded)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUI.BeginChangeCheck();
                            EditorGUILayout.PropertyField(scriptGeneratedRipplesAudioClip, soundEffectAudioClipLabel);
                            EditorGUILayout.PropertyField(scriptGeneratedRipplesSoundEffectPoolSize, soundEffectPoolSizeLabel);
                            EditorGUILayout.PropertyField(scriptGeneratedRipplesSoundEffectPoolExpandIfNecessary, soundEffectPoolExpandIfNecessaryLabel);
                            EditorGUILayout.Slider(scriptGeneratedRipplesAudioVolume, 0f, 1f, soundEffectAudioVolumeLabel);
                            EditorGUILayout.PropertyField(scriptGeneratedRipplesUseConstantAudioPitch, soundEffectConstantAudioPitchLabel);
                            if (scriptGeneratedRipplesUseConstantAudioPitch.boolValue)
                            {
                                EditorGUILayout.Slider(scriptGeneratedRipplesAudioPitch, -3f, 3f, soundEffectAudioPitchLabel);
                            }
                            else
                            {
                                EditorGUILayout.Slider(scriptGeneratedRipplesMinimumAudioPitch, -3f, 3f, soundEffectMinimumAudioPitchLabel);
                                EditorGUILayout.Slider(scriptGeneratedRipplesMaximumAudioPitch, -3f, 3f, soundEffectMaximumAudioPitchLabel);
                                EditorGUILayout.HelpBox(scriptGeneratedRipplesAudioPitchMessage, MessageType.None, true);
                            }
                            EditorGUI.indentLevel--;
                        }
                    }

                    //Particle Effect
                    using (BoxGroupScope boxGroup = new BoxGroupScope(particleEffectLabel, scriptGeneratedRipplesActivateParticleEffect, scriptGeneratedRipplesParticleffectPropertiesBoxGroupExpanded, 135f, 0f, true))
                    {
                        if (boxGroup.IsFaded)
                        {
                            EditorGUI.indentLevel++;
                            ParticleSystem particleSystem = scriptGeneratedRipplesParticleEffect.objectReferenceValue as ParticleSystem;
                            if (particleSystem != null && particleSystem.main.loop)
                            {
                                EditorGUILayout.HelpBox(particleSystemLoopMessage, MessageType.Warning);
                            }

                            EditorGUI.BeginChangeCheck();
                            EditorGUILayout.PropertyField(scriptGeneratedRipplesParticleEffect, particleEffectParticleSystemLabel);
                            EditorGUILayout.DelayedIntField(scriptGeneratedRipplesParticleEffectPoolSize, particleEffectPoolSizeLabel);
                            EditorGUILayout.PropertyField(scriptGeneratedRipplesParticleEffectPoolExpandIfNecessary, particleEffectPoolExpandIfNecessaryLabel);
                            EditorGUILayout.PropertyField(scriptGeneratedRipplesParticleEffectSpawnOffset, particleEffectSpawnOffsetLabel);
                            EditorGUILayout.PropertyField(scriptGeneratedRipplesParticleEffectStopAction, particleEffectStopActionLabel);
                            EditorGUI.indentLevel--;
                        }
                    }
                }
            }
        }

        private void DrawRefractionProperties(bool hasRefraction, bool hasFakePerspective)
        {
            refractionPropertiesExpanded.target = EditorGUILayout.Foldout(refractionPropertiesExpanded.target, refractionPropertiesFoldoutLabel, true);
            using (EditorGUILayout.FadeGroupScope group = new EditorGUILayout.FadeGroupScope(refractionPropertiesExpanded.faded))
            {
                if (group.visible)
                {
                    BeginBoxGroup(helpBoxStyle,170f,0f);
                    if (!hasRefraction)
                        EditorGUILayout.HelpBox(refractionMessage, MessageType.None, true);

                    EditorGUI.BeginDisabledGroup(!hasRefraction);
                    DrawRefractionReflectionRenderingCullingMaskFields(true, hasFakePerspective);
                    EditorGUILayout.Slider(refractionRenderTextureResizingFactor, 0f, 1f, refractionRenderTextureResizingFactorLabel);
                    EditorGUILayout.PropertyField(refractionRenderTextureFilterMode, refractionRenderTextureFilterModeLabel);
                    EditorGUI.EndDisabledGroup();
                    EndBoxGroup();
                }
            }
        }

        private void DrawReflectionProperties(bool hasReflection, bool hasFakePerspective)
        {
            reflectionPropertiesExpanded.target = EditorGUILayout.Foldout(reflectionPropertiesExpanded.target, reflectionPropertiesFoldoutLabel, true);
            using (EditorGUILayout.FadeGroupScope group = new EditorGUILayout.FadeGroupScope(reflectionPropertiesExpanded.faded))
            {
                if (group.visible)
                {
                    BeginBoxGroup(helpBoxStyle,170f,0f);
                    if (!hasReflection)
                    {
                        EditorGUILayout.HelpBox(reflectionMessage, MessageType.None, true);
                    }

                    EditorGUI.BeginDisabledGroup(!hasReflection);
                    DrawRefractionReflectionRenderingCullingMaskFields(false, hasFakePerspective);
                    if (hasFakePerspective)
                    {
                        using (BoxGroupScope boxGroup = new BoxGroupScope(viewingFrustumHeightScalingFactorLabel, reflectionCameraViewingFrustumHeightPropertiesBoxGroupExpanded, 200f, 0f, false))
                        {
                            if (boxGroup.IsFaded)
                            {
                                EditorGUILayout.PropertyField(reflectionPartiallySubmergedObjectsViewingFrustumHeightScalingFactor, reflectionPartiallySubmergedObjectsViewingFrustumHeightScalingFactorLabel);
                                EditorGUILayout.PropertyField(reflectionViewingFrustumHeightScalingFactor, reflectionViewingFrustumHeightScalingFactorLabel);
                            }
                        }
                    }
                    EditorGUILayout.Slider(reflectionRenderTextureResizingFactor, 0f, 1f, reflectionRenderTextureResizingFactorLabel);
                    EditorGUILayout.PropertyField(reflectionRenderTextureFilterMode, reflectionRenderTextureFilterModeLabel);
                    EditorGUILayout.PropertyField(reflectionZOffset, reflectionZOffsetLabel);
                    EditorGUI.EndDisabledGroup();
                    EndBoxGroup();
                }
            }
        }

        private void DrawRefractionReflectionRenderingCullingMaskFields(bool refraction, bool hasFakePerspective)
        {
            if (hasFakePerspective)
            {
                SerializedProperty allObjects;
                SerializedProperty partiallySubmergedObjects;
                if (refraction)
                {
                    allObjects = refractionCullingMask;
                    partiallySubmergedObjects = refractionPartiallySubmergedObjectsCullingMask;
                }
                else
                {
                    allObjects = reflectionCullingMask;
                    partiallySubmergedObjects = reflectionPartiallySubmergedObjectsCullingMask;
                }

                //All objects field
                EditorGUILayout.PropertyField(allObjects, refractionReflectionCullingMaskLabel);

                //Partially submerged objects
                string[] displayedOptions = GetAllLayersNamesInMask(allObjects.intValue);
                int mask = LayerMaskToConcatenatedLayersMask(partiallySubmergedObjects.intValue, displayedOptions);
                mask = EditorGUILayout.MaskField(refractionReflectionPartiallySubmergedObjectsCullingMaskLabel, mask, displayedOptions);
                partiallySubmergedObjects.intValue = ConcatenatedLayersMaskToLayerMask(mask, displayedOptions);
            }
            else
            {
                EditorGUILayout.PropertyField(refraction ? refractionCullingMask : reflectionCullingMask, refractionReflectionCullingMaskLabel);
            }
        }

        private void DrawRenderingSettingsProperties(bool hasRefraction, bool hasReflection)
        {
            renderingSettingsExpanded.target = EditorGUILayout.Foldout(renderingSettingsExpanded.target, renderingSettingsFoldoutLabel, true);
            using (EditorGUILayout.FadeGroupScope group = new EditorGUILayout.FadeGroupScope(renderingSettingsExpanded.faded))
            {
                if (group.visible)
                {
                    BeginBoxGroup(helpBoxStyle,155f,130f);
                    EditorGUI.BeginDisabledGroup(!(hasReflection || hasRefraction));
                    EditorGUILayout.PropertyField(farClipPlane, farClipPlaneLabel);
                    EditorGUILayout.PropertyField(allowMSAA, allowMSAALabel);
                    EditorGUILayout.PropertyField(allowHDR, allowHDRLabel);
                    EditorGUILayout.PropertyField(renderPixelLights, renderPixelLightsLabel);
                    EditorGUI.EndDisabledGroup();
                    EndBoxGroup();

                    BeginBoxGroup(helpBoxStyle,155f,130f);
                    DrawSortingLayerField(sortingLayerID, sortingOrder);
                    EndBoxGroup();
                }
            }
        }

        private static void DrawSortingLayerField(SerializedProperty layerID, SerializedProperty orderInLayer)
        {
            MethodInfo methodInfo = typeof(EditorGUILayout).GetMethod("SortingLayerField", BindingFlags.Static | BindingFlags.NonPublic, null, new[] {
                typeof( GUIContent ),
                typeof( SerializedProperty ),
                typeof( GUIStyle ),
                typeof( GUIStyle )
            }, null);

            if (methodInfo != null)
            {
                object[] parameters = { sortingLayerLabel, layerID, EditorStyles.popup, EditorStyles.label };
                methodInfo.Invoke(null, parameters);
                EditorGUILayout.PropertyField(orderInLayer, orderInLayerLabel);
            }
        }

        private void DrawPrefabUtility(Game2DWater water2D, UnityEngine.Material water2DMaterial)
        {
            prefabUtilityExpanded.target = EditorGUILayout.Foldout(prefabUtilityExpanded.target, prefabUtilityFoldoutLabel, true);
            using (EditorGUILayout.FadeGroupScope group = new EditorGUILayout.FadeGroupScope(prefabUtilityExpanded.faded))
            {
                if (group.visible)
                {
                    BeginBoxGroup(helpBoxStyle,155f,130f);
                    GameObject water2DGameObject = water2D.gameObject;
                    Texture waterNoiseTexture = water2DMaterial != null && water2DMaterial.HasProperty(noiseTextureShaderPropertyName) ? water2DMaterial.GetTexture(noiseTextureShaderPropertyName) : null;

#if UNITY_2018_3_OR_NEWER
                    bool isPrefabInstance = PrefabUtility.GetPrefabInstanceStatus(water2DGameObject) == PrefabInstanceStatus.Connected;
#else
                    PrefabType prefabType = PrefabUtility.GetPrefabType(water2DGameObject);
                    bool isPrefabInstance = prefabType == PrefabType.PrefabInstance;
                    bool isPrefabInstanceDisconnected = prefabType == PrefabType.DisconnectedPrefabInstance;
#endif

                    bool materialAssetAlreadyExist = water2DMaterial != null && AssetDatabase.Contains(water2DMaterial);
                    bool textureAssetAlreadyExist = waterNoiseTexture != null && AssetDatabase.Contains(waterNoiseTexture);

                    EditorGUI.BeginDisabledGroup(true);
#if UNITY_2018_2_OR_NEWER
                    Object prefabObjct = isPrefabInstance ? PrefabUtility.GetCorrespondingObjectFromSource(water2DGameObject) : null;
#else
                    Object prefabObjct = isPrefabInstance ? PrefabUtility.GetPrefabParent(water2DGameObject) : null;
#endif
                    EditorGUILayout.ObjectField(prefabObjct, typeof(Object), false);
                    EditorGUI.EndDisabledGroup();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(new GUIContent(prefabsPath, string.Format("Prefab Path: {0}", prefabsPath)), EditorStyles.textField);
                    if (GUILayout.Button(".", EditorStyles.miniButton, GUILayout.MaxWidth(14f)))
                    {
                        string newPrefabsPath = EditorUtility.OpenFolderPanel("Select prefabs path", "Assets", "");
                        if (!string.IsNullOrEmpty(newPrefabsPath))
                        {
                            newPrefabsPath = newPrefabsPath.Substring(Application.dataPath.Length);
                            prefabsPath = "Assets" + newPrefabsPath + "/";
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    if (!isPrefabInstance)
                    {
                        if (GUILayout.Button("Create Prefab"))
                        {
                            string fileName = GetValidAssetFileName(water2DGameObject.name, ".prefab", typeof(GameObject));

                            if (!textureAssetAlreadyExist && waterNoiseTexture != null)
                            {
                                string noiseTexturePath = prefabsPath + fileName + "_noiseTexture.asset";
                                AssetDatabase.CreateAsset(waterNoiseTexture, noiseTexturePath);
                            }

                            if (!materialAssetAlreadyExist && water2DMaterial != null)
                            {
                                string materialPath = prefabsPath + fileName + ".mat";
                                AssetDatabase.CreateAsset(water2DMaterial, materialPath);
                            }

                            string prefabPath = prefabsPath + fileName + ".prefab";
#if UNITY_2018_3_OR_NEWER
                            PrefabUtility.SaveAsPrefabAssetAndConnect(water2DGameObject, prefabPath, InteractionMode.AutomatedAction);
#else
                            PrefabUtility.CreatePrefab(prefabPath, water2DGameObject, ReplacePrefabOptions.ConnectToPrefab);
#endif
                        }
                    }
#if UNITY_2018_3_OR_NEWER
                    /*
                    As of Unity 2018.3, disconnecting (unlinking) and relinking a Prefab instance are no longer supported.
                    Alternatively, we can now unpack a Prefab instance if we want to entirely remove its link to its Prefab asset 
                    and thus be able to restructure the resulting plain GameObject as we please.
                    */
                    if (isPrefabInstance)
                    {
                        EditorGUILayout.HelpBox(newPrefabWorkflowMessage, MessageType.Info);
                    }
#else
                    if (isPrefabInstance)
                    {
                        if (GUILayout.Button("Unlink Prefab"))
                        {
#if UNITY_2018_2_OR_NEWER
                            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(water2DGameObject) as GameObject;
#else
                            GameObject prefab = PrefabUtility.GetPrefabParent(water2DGameObject) as GameObject;
#endif
                            PrefabUtility.DisconnectPrefabInstance(water2DGameObject);
                            UnityEngine.Material prefabMaterial = prefab.GetComponent<MeshRenderer>().sharedMaterial;
                            if (water2DMaterial != null && water2DMaterial == prefabMaterial)
                            {
                                bool usePrefabMaterial = EditorUtility.DisplayDialog("Use same prefab's material?",
                            "Do you still want to use the prefab's material?",
                            "Yes",
                            "No, create water's own material");

                                if (!usePrefabMaterial)
                                {
                                    UnityEngine.Material duplicateMaterial = new UnityEngine.Material(water2DMaterial);
                                    if (waterNoiseTexture != null)
                                    {
                                        Texture duplicateWaterNoiseTexture = Instantiate<Texture>(waterNoiseTexture);
                                        duplicateMaterial.SetTexture("_NoiseTexture", duplicateWaterNoiseTexture);
                                    }
                                    water2DGameObject.GetComponent<MeshRenderer>().sharedMaterial = duplicateMaterial;
                                }
                            }
                        }
                    }

                    if (isPrefabInstanceDisconnected)
                    {
                        if (GUILayout.Button("Relink Prefab"))
                        {
                            PrefabUtility.ReconnectToLastPrefab(water2DGameObject);
#if UNITY_2018_2_OR_NEWER
                            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(water2DGameObject) as GameObject;
#else
                            GameObject prefab = PrefabUtility.GetPrefabParent(water2DGameObject) as GameObject;
#endif
                            UnityEngine.Material prefabMaterial = prefab.GetComponent<MeshRenderer>().sharedMaterial;

                            if (prefabMaterial != null && water2DMaterial != prefabMaterial)
                            {
                                bool usePrefabMaterial = EditorUtility.DisplayDialog("Use prefab's material?",
                                "Do you want to use the prefab's material?",
                                "Yes",
                                "No, continue to use the current water material");

                                if (usePrefabMaterial)
                                {
                                    water2DGameObject.GetComponent<MeshRenderer>().sharedMaterial = prefabMaterial;
                                }
                                else
                                {
                                    if (!materialAssetAlreadyExist)
                                    {
                                        string fileName = GetValidAssetFileName(water2DGameObject.name, ".mat", typeof(UnityEngine.Material));

                                        if (!textureAssetAlreadyExist)
                                        {
                                            string noiseTexturePath = prefabsPath + fileName + "_noiseTexture.asset";
                                            AssetDatabase.CreateAsset(waterNoiseTexture, noiseTexturePath);
                                        }

                                        string materialPath = prefabsPath + fileName + ".mat";
                                        AssetDatabase.CreateAsset(water2DMaterial, materialPath);
                                    }
                                }
                            }
                        }
                    }
#endif
                    EndBoxGroup();
                }
            }
        }

        private string GetValidAssetFileName(string assetName, string assetExtension, System.Type assetType)
        {
            string fileName = assetName;

            string path = prefabsPath + fileName + assetExtension;
            bool prefabWithSameNameExist = AssetDatabase.LoadAssetAtPath(path, assetType) != null;
            if (prefabWithSameNameExist)
            {
                int i = 1;
                while (prefabWithSameNameExist)
                {
                    fileName = assetName + " " + i;
                    path = prefabsPath + fileName + assetExtension;
                    prefabWithSameNameExist = AssetDatabase.LoadAssetAtPath(path, assetType) != null;
                    i++;
                }
            }

            return fileName;
        }

        private void SetEditorGUISettings(float labelWidth, float fieldWidth)
        {
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUIUtility.fieldWidth = fieldWidth;
        }

        private static void BeginBoxGroup(GUIStyle boxStyle,float labelWidth,float fieldWidth)
        {
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUIUtility.fieldWidth = fieldWidth;
            EditorGUILayout.BeginVertical(boxStyle);
        }

        private static void EndBoxGroup()
        {
            EditorGUIUtility.labelWidth = 0f;
            EditorGUIUtility.fieldWidth = 0f;
            EditorGUILayout.EndVertical();
        }

        private string[] GetAllLayersNamesInMask(int mask)
        {
            List<string> layers = new List<string>();
            for (int i = 0; i < 32; i++)
            {
                if (mask == (mask | (1 << i)) && !string.IsNullOrEmpty(LayerMask.LayerToName(i)))
                {
                    layers.Add(LayerMask.LayerToName(i));
                }
            }
            return layers.ToArray();
        }

        private bool MaskContainsLayer(int mask, int layerIndex)
        {
            return mask == (mask | (1 << layerIndex));
        }

        private int LayerMaskToConcatenatedLayersMask(int mask, string[] displayedOptions)
        {
            int concatenatedMask = 0;
            for (int i = 0; i < displayedOptions.Length; i++)
            {
                int layer = LayerMask.NameToLayer(displayedOptions[i]);
                if (MaskContainsLayer(mask, layer))
                {
                    concatenatedMask |= (1 << i);
                }
            }
            return concatenatedMask;
        }

        private int ConcatenatedLayersMaskToLayerMask(int concatMask, string[] displayedOptions)
        {
            int mask = 0;
            for (int i = 0; i < displayedOptions.Length; i++)
            {
                if (MaskContainsLayer(concatMask, i))
                {
                    mask |= (1 << LayerMask.NameToLayer(displayedOptions[i]));
                }
            }
            return mask;
        }

        #endregion

        #region SceneView

        private void OnSceneGUI()
        {
            Game2DWater water2D = target as Game2DWater;

            if (!isMultiEditing)
            {
                if (waterResizerUtility.IsActive)
                {
                    waterResizerUtility.DrawWaterResizer(water2D);
                }
                if (constantRipplesEditSourcesPositions)
                {
                    DrawConstantRipplesSourcesPositions(water2D);
                }
            }
            DrawWaterWireframe(water2D);
            DrawBuoyancyEffectorSurfaceLevelGuideline(water2D);

            if (reflectionPropertiesExpanded.value && reflectionCameraViewingFrustumHeightPropertiesBoxGroupExpanded.value)
            {
                UnityEngine.Material waterMaterial = water2D.GetComponent<MeshRenderer>().sharedMaterial;
                if (waterMaterial.IsKeywordEnabled("Water2D_Reflection") && waterMaterial.IsKeywordEnabled("Water2D_FakePerspective"))
                {
                    DrawPartiallySubmergedObjectsReflectionFrustumBox(water2D, waterMaterial);
                }
            }

            if (GUI.changed)
            {
                SceneView.RepaintAll();
            }
        }

        private void DrawWaterWireframe(Game2DWater water2D)
        {
            Vector3[] vertices = water2D.MeshModule.Vertices;
            int surfaceVerticesCount = water2D.MeshModule.SurfaceVerticesCount;

            int start, end;
            if (water2D.SimulationModule.IsUsingCustomBoundaries)
            {
                start = 1;
                end = surfaceVerticesCount - 2;
            }
            else
            {
                start = 0;
                end = surfaceVerticesCount - 1;
            }

            using (new Handles.DrawingScope(wireframeColor, water2D.MainModule.LocalToWorldMatrix))
            {
                for (int i = start; i <= end; i++)
                {
                    Handles.DrawLine(vertices[i], vertices[i + surfaceVerticesCount]);
                }
            }
        }

        private void DrawBuoyancyEffectorSurfaceLevelGuideline(Game2DWater water2D)
        {
            Vector2 halfWaterSize = water2D.MainModule.WaterSize * 0.5f;
            float y = halfWaterSize.y * (1f - 2f * water2D.AttachedComponentsModule.BuoyancyEffectorSurfaceLevel);
            Vector3 lineStart = water2D.MainModule.TransformLocalToWorld(new Vector2(-halfWaterSize.x, y));
            Vector3 lineEnd = water2D.MainModule.TransformLocalToWorld(new Vector2(halfWaterSize.x, y));
            Handles.color = buoyancyEffectorSurfaceLevelGuidelineColor;
            Handles.DrawLine(lineStart, lineEnd);
            Handles.color = Color.white;
        }

        private void DrawConstantRipplesSourcesPositions(Game2DWater water2D)
        {
            List<float> ripplesSourcesPositions = water2D.ConstantRipplesModule.SourcePositions;
            List<int> ripplesSourcesIndices = new List<int>(ripplesSourcesPositions.Count);
            Vector3[] meshVerticesPositions = water2D.MeshModule.Vertices;
            int surfaceVerticesCount = water2D.MeshModule.SurfaceVerticesCount;

            Vector2 halfWaterSize = water2D.MainModule.WaterSize * 0.5f;

            float leftmostBoundary = water2D.SimulationModule.LeftBoundary;
            float rightmostBoundary = water2D.SimulationModule.RightBoundary;
            float activeSurfaceArea = rightmostBoundary - leftmostBoundary;
            int activeSurfaceAreaVerticesCount = water2D.SimulationModule.IsUsingCustomBoundaries ? surfaceVerticesCount - 3 : surfaceVerticesCount - 1;
            float columnWdth = activeSurfaceArea / activeSurfaceAreaVerticesCount;

            int indexOffset, start, end;

            if (water2D.SimulationModule.IsUsingCustomBoundaries)
            {
                indexOffset = 1;
                start = 1;
                end = surfaceVerticesCount - 2;
            }
            else
            {
                indexOffset = 0;
                start = 0;
                end = surfaceVerticesCount - 1;
            }

            bool changeMade = false;
            bool addNewSource = false;
            int index = -1;

            Quaternion handlesRotation = Quaternion.identity;
            float handlesSize = HandleUtility.GetHandleSize(water2D.MainModule.Position) * 0.05f;
            Handles.CapFunction handlesCap = Handles.DotHandleCap;
            Color handlesColor = Handles.color;

            using (new Handles.DrawingScope(water2D.MainModule.LocalToWorldMatrix))
            {
                for (int i = 0, maxi = ripplesSourcesPositions.Count; i < maxi; i++)
                {
                    float xPosition = ripplesSourcesPositions[i];
                    if (xPosition < leftmostBoundary || xPosition > rightmostBoundary)
                    {
                        Handles.color = constantRipplesSourcesColorRemove;
                        if (Handles.Button(new Vector3(xPosition, halfWaterSize.y), handlesRotation, handlesSize, handlesSize, handlesCap))
                        {
                            changeMade = true;
                            index = i;
                            addNewSource = false;
                        }
                        ripplesSourcesIndices.Add(-1);
                    }
                    else
                    {
                        int nearestIndex = Mathf.RoundToInt((xPosition - leftmostBoundary) / columnWdth) + indexOffset;
                        ripplesSourcesIndices.Add(nearestIndex);
                    }
                }

                for (int i = start; i <= end; i++)
                {
                    Vector3 pos = meshVerticesPositions[i];

                    bool foundMatch = false;
                    int foundMatchIndex = -1;
                    for (int j = 0, maxj = ripplesSourcesIndices.Count; j < maxj; j++)
                    {
                        if (ripplesSourcesIndices[j] == i)
                        {
                            foundMatch = true;
                            foundMatchIndex = j;
                            break;
                        }
                    }

                    if (foundMatch)
                    {
                        Handles.color = constantRipplesSourcesColorRemove;
                        if (Handles.Button(pos, handlesRotation, handlesSize, handlesSize, handlesCap))
                        {
                            changeMade = true;
                            index = foundMatchIndex;
                            addNewSource = false;
                        }
                    }
                    else
                    {
                        Handles.color = constantRipplesSourcesColorAdd;
                        if (Handles.Button(pos, handlesRotation, handlesSize, handlesSize, handlesCap))
                        {
                            changeMade = true;
                            index = i;
                            addNewSource = true;
                        }
                    }
                }
            }

            Handles.color = handlesColor;

            if (changeMade)
            {
                Undo.RecordObject(water2D, "editing water ripple source position");
                if (addNewSource)
                {
                    ripplesSourcesPositions.Add(meshVerticesPositions[index].x);
                }
                else
                {
                    ripplesSourcesPositions.RemoveAt(index);
                }

                EditorUtility.SetDirty(water2D);

                if (Application.isPlaying)
                {
                    water2D.ConstantRipplesModule.SourcePositions = ripplesSourcesPositions;
                }
            }
        }

        private void DrawPartiallySubmergedObjectsReflectionFrustumBox(Game2DWater water2D, UnityEngine.Material waterMaterial)
        {
            Vector2 waterSize = water2D.MainModule.WaterSize;
            float surfaceLevel = waterSize.y * (waterMaterial.GetFloat("_SurfaceLevel") - 0.5f);
            float surfaceSubmergeLevel = waterSize.y * (waterMaterial.GetFloat("_SubmergeLevel") - 0.5f);

            Color defaultHandlesColor = Handles.color;
            using (new Handles.DrawingScope(water2D.transform.localToWorldMatrix))
            {
                //partially submerged objects reflection camera viewing frustum
                Vector2 bottomLeft = new Vector2(-waterSize.x * 0.5f, surfaceSubmergeLevel);
                float scalingFactor = water2D.RenderingModule.ReflectionPartiallySubmergedObjects.ViewingFrustumHeightScalingFactor;
                Vector2 size = new Vector2(waterSize.x, (surfaceSubmergeLevel - surfaceLevel) * scalingFactor);
                DrawBox(bottomLeft, size, Color.red);

                //other objects reflection camera viewing frustum
                bottomLeft.y = waterSize.y * 0.5f;
                scalingFactor = water2D.RenderingModule.Reflection.ViewingFrustumHeightScalingFactor;
                size.y = (waterSize.y * 0.5f - surfaceLevel) * scalingFactor;
                DrawBox(bottomLeft, size, Color.green);
            }
            Handles.color = defaultHandlesColor;
        }

        private void DrawBox(Vector2 bottomLeft, Vector2 size, Color color)
        {
            Vector2 topLeft = bottomLeft + new Vector2(0f, size.y);
            Vector2 topRight = bottomLeft + size;
            Vector3 bottomRight = bottomLeft + new Vector2(size.x, 0f);

            Handles.color = color;
            Handles.DrawPolyLine(bottomLeft, topLeft, topRight, bottomRight, bottomLeft);
        }

        #endregion

        #endregion

        #region Custom Classes

        private class BoxGroupScope : GUI.Scope
        {
            private static readonly string clickToExpandTooltip = "Click to expand";
            private static readonly string clickToCollapseTooltip = "Click to collapse";
            
            public bool IsFaded { get; private set; }
            
            public BoxGroupScope(string name, AnimBool state, float labelWidth, float fieldWidth, bool useHelpBoxStyle = true)
            {
                BeginBoxGroup(useHelpBoxStyle ? helpBoxStyle : groupBoxStyle,labelWidth, fieldWidth);

                if (GUILayout.Button(new GUIContent(name, state.target ? clickToCollapseTooltip : clickToExpandTooltip), EditorStyles.boldLabel))
                    state.target = !state.target;

                IsFaded = EditorGUILayout.BeginFadeGroup(state.faded);
            }

            public BoxGroupScope(string name, SerializedProperty toggle, AnimBool state, float labelWidth, float fieldWidth, bool useHelpBoxStyle = true)
            {
                BeginBoxGroup(useHelpBoxStyle ? helpBoxStyle : groupBoxStyle, labelWidth, fieldWidth);

                Rect rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                EditorGUI.showMixedValue = toggle.hasMultipleDifferentValues;
                EditorGUI.BeginChangeCheck();
                bool toggleState = EditorGUI.Toggle(new Rect(rect.x, rect.y, 14f, rect.height), toggle.boolValue);
                if (EditorGUI.EndChangeCheck())
                    toggle.boolValue = toggleState;

                EditorGUI.showMixedValue = false;
                rect.x += 16f;
                if (GUI.Button(rect, new GUIContent(name, state.target ? clickToCollapseTooltip : clickToExpandTooltip), EditorStyles.boldLabel))
                    state.target = !state.target;

                IsFaded = EditorGUILayout.BeginFadeGroup(state.faded);
            }

            protected override void CloseScope()
            {
                EditorGUILayout.EndFadeGroup();
                EndBoxGroup();
            }
        }

        private class WaterResizerUtility
        {
            private readonly UnityAction _repaint;

            public bool IsActive { get; set; }
            public bool HasChanged { get; set; }

            public Vector2 WaterSize { get; private set; }
            public Vector3 WaterPosition { get; private set; }

            public WaterResizerUtility(UnityAction repaint)
            {
                _repaint = repaint;
            }

            public void DrawWaterResizer(Game2DWater water2D)
            {
                using (new Handles.DrawingScope(water2D.MainModule.LocalToWorldMatrix))
                {
                    Vector2 halfWaterSize = water2D.MainModule.WaterSize * 0.5f;
                    float handlesSize = HandleUtility.GetHandleSize(Vector3.zero) * 0.5f;
                    Handles.CapFunction handlesCap = Handles.ArrowHandleCap;
                    const float handlesSnap = 1f;

                    EditorGUI.BeginChangeCheck();
                    Vector3 upPos = Handles.Slider(new Vector3(0f, halfWaterSize.y, 0f), Vector3.up, handlesSize, handlesCap, handlesSnap);
                    Vector3 downPos = Handles.Slider(new Vector3(0f, -halfWaterSize.y, 0f), Vector3.down, handlesSize, handlesCap, handlesSnap);
                    Vector3 rightPos = Handles.Slider(new Vector3(halfWaterSize.x, 0f, 0f), Vector3.right, handlesSize, handlesCap, handlesSnap);
                    Vector3 leftPos = Handles.Slider(new Vector3(-halfWaterSize.x, 0f, 0f), Vector3.left, handlesSize, handlesCap, handlesSnap);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Vector2 newWaterSize;
                        newWaterSize.x = Mathf.Clamp(rightPos.x - leftPos.x, 0f, float.MaxValue);
                        newWaterSize.y = Mathf.Clamp(upPos.y - downPos.y, 0f, float.MaxValue);

                        if (newWaterSize.x > 0f && newWaterSize.y > 0f)
                        {
                            HasChanged = true;
                            WaterSize = newWaterSize;
                            WaterPosition = water2D.MainModule.TransformLocalToWorld(new Vector2((rightPos.x + leftPos.x) / 2f, (upPos.y + downPos.y) / 2f));
                            
                            if(_repaint != null)
                                _repaint.Invoke();
                        }
                    }
                }
            }
        }

        #endregion
    }
}