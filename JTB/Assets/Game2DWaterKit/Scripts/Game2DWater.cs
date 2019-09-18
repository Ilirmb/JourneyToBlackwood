namespace Game2DWaterKit
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Serialization;
    using Game2DWaterKit.Animation;
    using Game2DWaterKit.AttachedComponents;
    using Game2DWaterKit.Material;
    using Game2DWaterKit.Mesh;
    using Game2DWaterKit.Rendering;
    using Game2DWaterKit.Ripples;
    using Game2DWaterKit.Ripples.Effects;
    using Game2DWaterKit.Simulation;
    using Game2DWaterKit.Main;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    [RequireComponent(typeof(BuoyancyEffector2D),typeof(BoxCollider2D))]
    [ExecuteInEditMode]
    public partial class Game2DWater : MonoBehaviour
    {
        #region Serialized Variables

        #region Water Mesh Serialized Variables
        [SerializeField] private Vector2 waterSize = Vector2.one;
        [SerializeField] private int subdivisionsCountPerUnit = 3;
        #endregion

        #region Water Simulation Serialized Variables
        [SerializeField] private float damping = 0.05f;
        [SerializeField] private float stiffness = 60f;
        [SerializeField] private float spread = 60f;
        [SerializeField] private bool useCustomBoundaries = false;
        [SerializeField] private float firstCustomBoundary = 0.5f;
        [SerializeField] private float secondCustomBoundary = -0.5f;
        #endregion

        #region Water Collision Ripples Serialized Variables
        [SerializeField] private bool activateOnCollisionOnWaterEnterRipples = true;
        [SerializeField] private bool activateOnCollisionOnWaterExitRipples = true;
        //Disturbance Properties
        [FormerlySerializedAs("minimumDisturbance"), SerializeField] private float onCollisionRipplesMinimumDisturbance = 0.1f;
        [FormerlySerializedAs("maximumDisturbance"), SerializeField] private float onCollisionRipplesMaximumDisturbance = 0.75f;
        [FormerlySerializedAs("velocityMultiplier"), SerializeField] private float onCollisionRipplesVelocityMultiplier = 0.12f;
        //Collision Properties
        [FormerlySerializedAs("collisionMask"), SerializeField] private LayerMask onCollisionRipplesCollisionMask = ~(1 << 4);
        [FormerlySerializedAs("collisionMinimumDepth"), SerializeField] private float onCollisionRipplesCollisionMinimumDepth = -10f;
        [FormerlySerializedAs("collisionMaximumDepth"), SerializeField] private float onCollisionRipplesCollisionMaximumDepth = 10f;
        [FormerlySerializedAs("collisionRaycastMaxDistance"), SerializeField] private float onCollisionRipplesCollisionRaycastMaxDistance = 0.5f;
        //Particle Effect Properties (On Water Enter)
        [FormerlySerializedAs("activateOnCollisionSplashParticleEffect"), SerializeField] private bool onCollisionRipplesActivateOnWaterEnterParticleEffect = false;
        [FormerlySerializedAs("onCollisionSplashParticleEffect"), SerializeField] private ParticleSystem onCollisionRipplesOnWaterEnterParticleEffect = null;
        [FormerlySerializedAs("onCollisionSplashParticleEffectSpawnOffset"), SerializeField] private Vector3 onCollisionRipplesOnWaterEnterParticleEffectSpawnOffset = Vector3.zero;
        [SerializeField] private UnityEvent onCollisionRipplesOnWaterEnterParticleEffectStopAction = new UnityEvent();
        [FormerlySerializedAs("onCollisionSplashParticleEffectPoolSize"), SerializeField] private int onCollisionRipplesOnWaterEnterParticleEffectPoolSize = 10;
        [SerializeField] private bool onCollisionRipplesOnWaterEnterParticleEffectPoolExpandIfNecessary = true;
        //Sound Effect Properties (On Water Enter)
        [SerializeField] private bool onCollisionRipplesActivateOnWaterEnterSoundEffect = true;
        [FormerlySerializedAs("splashAudioClip"), SerializeField] private AudioClip onCollisionRipplesOnWaterEnterAudioClip = null;
        [FormerlySerializedAs("useConstantAudioPitch"), SerializeField] private bool onCollisionRipplesUseConstantOnWaterEnterAudioPitch = false;
        [FormerlySerializedAs("audioPitch"), SerializeField] private float onCollisionRipplesOnWaterEnterAudioPitch = 1f;
        [FormerlySerializedAs("minimumAudioPitch"), SerializeField] private float onCollisionRipplesOnWaterEnterMinimumAudioPitch = 0.75f;
        [FormerlySerializedAs("maximumAudioPitch"), SerializeField] private float onCollisionRipplesOnWaterEnterMaximumAudioPitch = 1.25f;
        [SerializeField] private float onCollisionRipplesOnWaterEnterAudioVolume = 1.0f;
        [SerializeField] private int onCollisionRipplesOnWaterEnterSoundEffectPoolSize = 10;
        [SerializeField] private bool onCollisionRipplesOnWaterEnterSoundEffectPoolExpandIfNecessary = true;
        //Particle Effect Properties (On Water Exit)
        [SerializeField] private bool onCollisionRipplesActivateOnWaterExitParticleEffect = false;
        [SerializeField] private ParticleSystem onCollisionRipplesOnWaterExitParticleEffect = null;
        [SerializeField] private Vector3 onCollisionRipplesOnWaterExitParticleEffectSpawnOffset = Vector3.zero;
        [SerializeField] private UnityEvent onCollisionRipplesOnWaterExitParticleEffectStopAction = new UnityEvent();
        [SerializeField] private int onCollisionRipplesOnWaterExitParticleEffectPoolSize = 10;
        [SerializeField] private bool onCollisionRipplesOnWaterExitParticleEffectPoolExpandIfNecessary = true;
        //Sound Effect Properties (On Water Exit)
        [SerializeField] private bool onCollisionRipplesActivateOnWaterExitSoundEffect = false;
        [SerializeField] private AudioClip onCollisionRipplesOnWaterExitAudioClip = null;
        [SerializeField] private bool onCollisionRipplesUseConstantOnWaterExitAudioPitch = false;
        [SerializeField] private float onCollisionRipplesOnWaterExitAudioPitch = 1f;
        [SerializeField] private float onCollisionRipplesOnWaterExitMinimumAudioPitch = 0.75f;
        [SerializeField] private float onCollisionRipplesOnWaterExitMaximumAudioPitch = 1.25f;
        [SerializeField] private float onCollisionRipplesOnWaterExitAudioVolume = 1.0f;
        [SerializeField] private int onCollisionRipplesOnWaterExitSoundEffectPoolSize = 10;
        [SerializeField] private bool onCollisionRipplesOnWaterExitSoundEffectPoolExpandIfNecessary = true;
        //Events
        [SerializeField] private UnityEvent onWaterEnter = new UnityEvent();
        [SerializeField] private UnityEvent onWaterExit = new UnityEvent();
        #endregion

        #region Water Constant Ripples Serialized Variables
        [SerializeField] private bool activateConstantRipples = false;
        [SerializeField] private bool constantRipplesUpdateWhenOffscreen = false;
        //Disturbance Properties
        [SerializeField] private bool constantRipplesRandomizeDisturbance = false;
        [SerializeField] private bool constantRipplesSmoothDisturbance = false;
        [SerializeField] private float constantRipplesSmoothFactor = 0.5f;
        [SerializeField] private float constantRipplesDisturbance = 0.10f;
        [SerializeField] private float constantRipplesMinimumDisturbance = 0.08f;
        [SerializeField] private float constantRipplesMaximumDisturbance = 0.12f;
        //Interval Properties
        [SerializeField] private bool constantRipplesRandomizeInterval = false;
        [SerializeField] private float constantRipplesInterval = 1f;
        [SerializeField] private float constantRipplesMinimumInterval = 0.75f;
        [SerializeField] private float constantRipplesMaximumInterval = 1.25f;
        //Ripple Source Positions Properties
        [SerializeField] private bool constantRipplesRandomizeRipplesSourcesPositions = false;
        [SerializeField] private int constantRipplesRandomizeRipplesSourcesCount = 3;
        [SerializeField] private bool constantRipplesAllowDuplicateRipplesSourcesPositions = false;
        [SerializeField] private List<float> constantRipplesSourcePositions = new List<float>();
        //Sound Effect Properties
        [SerializeField] private bool constantRipplesActivateSoundEffect = false;
        [SerializeField] private bool constantRipplesUseConstantAudioPitch = false;
        [SerializeField] private AudioClip constantRipplesAudioClip = null;
        [SerializeField] private float constantRipplesAudioPitch = 1f;
        [SerializeField] private float constantRipplesMinimumAudioPitch = 0.75f;
        [SerializeField] private float constantRipplesMaximumAudioPitch = 1.25f;
        [SerializeField] private int constantRipplesSoundEffectPoolSize = 10;
        [SerializeField] private bool constantRipplesSoundEffectPoolExpandIfNecessary = true;
        [SerializeField] private float constantRipplesAudioVolume = 1.0f;
        //Particle Effect Properties
        [FormerlySerializedAs("activateConstantSplashParticleEffect"), SerializeField] private bool constantRipplesActivateParticleEffect = false;
        [FormerlySerializedAs("constantSplashParticleEffect"), SerializeField] private ParticleSystem constantRipplesParticleEffect = null;
        [FormerlySerializedAs("constantSplashParticleEffectSpawnOffset"), SerializeField] private Vector3 constantRipplesParticleEffectSpawnOffset = Vector3.zero;
        [SerializeField] private UnityEvent constantRipplesParticleEffectStopAction = new UnityEvent();
        [FormerlySerializedAs("constantSplashParticleEffectPoolSize"), SerializeField] private int constantRipplesParticleEffectPoolSize = 10;
        [SerializeField] private bool constantRipplesParticleEffectPoolExpandIfNecessary = true;
        #endregion

        #region Water Script Generated Ripples Serialized Variables
        //Disturbance Properties
        [SerializeField] private float scriptGeneratedRipplesMinimumDisturbance = 0.1f;
        [SerializeField] private float scriptGeneratedRipplesMaximumDisturbance = 0.75f;
        //Sound Effect Properties
        [SerializeField] private bool scriptGeneratedRipplesActivateSoundEffect = false;
        [SerializeField] private AudioClip scriptGeneratedRipplesAudioClip = null;
        [SerializeField] private bool scriptGeneratedRipplesUseConstantAudioPitch = false;
        [SerializeField] private float scriptGeneratedRipplesAudioPitch = 1f;
        [SerializeField] private float scriptGeneratedRipplesMinimumAudioPitch = 0.75f;
        [SerializeField] private float scriptGeneratedRipplesMaximumAudioPitch = 1.25f;
        [SerializeField] private float scriptGeneratedRipplesAudioVolume = 1.0f;
        [SerializeField] private int scriptGeneratedRipplesSoundEffectPoolSize = 10;
        [SerializeField] private bool scriptGeneratedRipplesSoundEffectPoolExpandIfNecessary = true;
        //Particle Effect Properties
        [SerializeField] private bool scriptGeneratedRipplesActivateParticleEffect = false;
        [SerializeField] private ParticleSystem scriptGeneratedRipplesParticleEffect = null;
        [SerializeField] private Vector3 scriptGeneratedRipplesParticleEffectSpawnOffset = Vector3.zero;
        [SerializeField] private UnityEvent scriptGeneratedRipplesParticleEffectStopAction = new UnityEvent();
        [SerializeField] private int scriptGeneratedRipplesParticleEffectPoolSize = 10;
        [SerializeField] private bool scriptGeneratedRipplesParticleEffectPoolExpandIfNecessary = true;
        #endregion

        #region Water Rendering Serialized Variables
        //Refraction Properties
        [SerializeField] private float refractionRenderTextureResizeFactor = 1f;
        [SerializeField] private LayerMask refractionCullingMask = ~(1 << 4);
        [SerializeField] private LayerMask refractionPartiallySubmergedObjectsCullingMask;
        [SerializeField] private FilterMode refractionRenderTextureFilterMode = FilterMode.Bilinear;
        //Reflection Properties
        [SerializeField] private float reflectionRenderTextureResizeFactor = 1f;
        [SerializeField] private float reflectionViewingFrustumHeightScalingFactor = 1f;
        [SerializeField] private float reflectionPartiallySubmergedObjectsViewingFrustumHeightScalingFactor = 1f;
        [SerializeField] private LayerMask reflectionCullingMask = ~(1 << 4);
        [SerializeField] private LayerMask reflectionPartiallySubmergedObjectsCullingMask;
        [SerializeField] private FilterMode reflectionRenderTextureFilterMode = FilterMode.Bilinear;
        [SerializeField] private float reflectionZOffset = 0f;
        //Shared Properties
        [SerializeField] private int sortingLayerID = 0;
        [SerializeField] private int sortingOrder = 0;
        [SerializeField] private float farClipPlane = 100f;
        [SerializeField] private bool renderPixelLights = true;
        [SerializeField] private bool allowMSAA = false;
        [SerializeField] private bool allowHDR = false;
        #endregion

        #region Water Buoyancy Serialized Variables
        [SerializeField] private float buoyancyEffectorSurfaceLevel = 0.02f;
        #endregion

        #endregion

        #region Variables

        private WaterMainModule _mainModule = null;
        private WaterMeshModule _meshModule = null;
        private WaterMaterialModule _materialModule = null;
        private WaterSimulationModule _simulationModule = null;
        private WaterRenderingModule _renderingModule = null;
        private WaterAnimationModule _animationModule = null;
        private WaterCollisionRipplesModule _onCollisonRipplesModule = null;
        private WaterConstantRipplesModule _constantRipplesModule = null;
        private WaterScriptGeneratedRipplesModule _scriptGeneratedRipplesModule = null;
        private WaterAttachedComponentsModule _attachedComponentsModule = null;

        private Transform _ripplesEffectsRoot;
        private Transform _renderingCamerasRoot;

        [NonSerialized] private bool _areModulesInitialized = false;

        #endregion

        #region Properties

        public WaterMainModule MainModule { get { return _mainModule; } }
        public WaterMeshModule MeshModule { get { return _meshModule; } }
        public WaterMaterialModule MaterialModule { get { return _materialModule; } }
        public WaterSimulationModule SimulationModule { get { return _simulationModule; } }
        public WaterRenderingModule RenderingModule { get { return _renderingModule; } }
        public WaterAnimationModule AnimationModule { get { return _animationModule; } }
        public WaterCollisionRipplesModule OnCollisonRipplesModule { get { return _onCollisonRipplesModule; } }
        public WaterConstantRipplesModule ConstantRipplesModule { get { return _constantRipplesModule; } }
        public WaterScriptGeneratedRipplesModule ScriptGeneratedRipplesModule { get { return _scriptGeneratedRipplesModule; } }
        public WaterAttachedComponentsModule AttachedComponentsModule { get { return _attachedComponentsModule; } }
        public bool IsInitialized { get { return _areModulesInitialized; } }

        private Transform RipplesEffectsPoolsRoot
        {
            get
            {
#if UNITY_EDITOR
                //We don't need to spawn ripples effects objects in edit mode
                if (!Application.isPlaying)
                    return null;
#endif

                if (_ripplesEffectsRoot == null)
                {
                    var ripplesEffectsRootGO = new GameObject("Ripples Effects For Water " + GetInstanceID());
                    ripplesEffectsRootGO.hideFlags = HideFlags.HideInHierarchy;
                    _ripplesEffectsRoot = ripplesEffectsRootGO.transform;
                }
                return _ripplesEffectsRoot;
            }
        }
        private Transform RenderingCamerasRoot
        {
            get
            {
                if (_renderingCamerasRoot == null)
                {
                    var renderingCamerasRootGO = new GameObject("Rendering Cameras For Water " + GetInstanceID());
                    renderingCamerasRootGO.hideFlags = HideFlags.HideAndDontSave;
                    _renderingCamerasRoot = renderingCamerasRootGO.transform;
                }

                return _renderingCamerasRoot;
            }
        }

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            #if UNITY_EDITOR
            DestroyRenderingCameras();
            #endif

            InitializeModules();
        }
        
        private void LateUpdate()
        {
            if (!_areModulesInitialized)
                return;

            if(_attachedComponentsModule.HasAnimatorAttached)
                _animationModule.SyncAnimatableVariables(waterSize,firstCustomBoundary,secondCustomBoundary);

            _animationModule.Update();
            _mainModule.Update();

            _constantRipplesModule.Update();
            _scriptGeneratedRipplesModule.Update();
            _onCollisonRipplesModule.Update();

            _meshModule.Update();
            _attachedComponentsModule.Update();
        }

        private void FixedUpdate()
        {
            if(!_simulationModule.IsControlledByLargeWaterAreaManager)
                _simulationModule.FixedUpdate();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            _onCollisonRipplesModule.ResolveCollision(collider, isObjectEnteringWater: true);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            _onCollisonRipplesModule.ResolveCollision(collider, isObjectEnteringWater: false);
        }

        private void OnWillRenderObject()
        {
            _renderingModule.Render(Camera.current);
        }

        private void OnBecameVisible()
        {
            _mainModule.IsWaterVisible = true;
        }

        private void OnBecameInvisible()
        {
            _mainModule.IsWaterVisible = false;
        }

        private void OnDestroy()
        {
            DestroyAllSpawnedObjects();
        }

        #endregion

        #region Methods

        public void InitializeModules()
        {
            if (_areModulesInitialized)
                return;
            
            _mainModule = new WaterMainModule(this, waterSize);
            _simulationModule = new WaterSimulationModule(damping, stiffness, spread, firstCustomBoundary, secondCustomBoundary, useCustomBoundaries);
            _meshModule = new Mesh.WaterMeshModule(subdivisionsCountPerUnit);
            _materialModule = new WaterMaterialModule();
            _renderingModule = new WaterRenderingModule(GetRenderingModuleParameters(), RenderingCamerasRoot);
            _animationModule = new WaterAnimationModule();
            _onCollisonRipplesModule = new WaterCollisionRipplesModule(GetCollisionRipplesModuleParameters(), RipplesEffectsPoolsRoot);
            _constantRipplesModule = new WaterConstantRipplesModule(GetConstantRipplesModuleParameters(), RipplesEffectsPoolsRoot);
            _scriptGeneratedRipplesModule = new WaterScriptGeneratedRipplesModule(GetScriptGeneratedRipplesModuleParameters(), RipplesEffectsPoolsRoot);
            _attachedComponentsModule = new WaterAttachedComponentsModule(buoyancyEffectorSurfaceLevel);

            _simulationModule.SetDependencies(_mainModule, _meshModule);
            _meshModule.SetDependencies(_mainModule, _simulationModule);
            _materialModule.SetDependencies(_meshModule);
            _renderingModule.SetDependencies(_mainModule, _meshModule, _materialModule);
            _animationModule.SetDependencies(_mainModule,_meshModule,_simulationModule);
            _onCollisonRipplesModule.SetDependencies(_mainModule, _meshModule, _simulationModule);
            _constantRipplesModule.SetDependencies(_mainModule, _meshModule, _simulationModule);
            _scriptGeneratedRipplesModule.SetDependencies(_mainModule, _meshModule, _simulationModule);
            _attachedComponentsModule.SetDependencies(_mainModule);

            _mainModule.Initialize();
            _simulationModule.Initialize();
            _meshModule.Initialize();
            _materialModule.Initialize();
            _renderingModule.Initialize();
            _attachedComponentsModule.Initialize();
            _constantRipplesModule.Initialze();
            _animationModule.Initialze();

            _areModulesInitialized = true;
        }

        private void DestroyAllSpawnedObjects()
        {
            DestroyRenderingCamerasRoot();

            if (_ripplesEffectsRoot != null)
                Destroy(_ripplesEffectsRoot.gameObject);
        }

        private void DestroyRenderingCamerasRoot()
        {
            if (_renderingCamerasRoot != null)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    Destroy(_renderingCamerasRoot.gameObject);
                }
                else
                {
                    DestroyImmediate(_renderingCamerasRoot.gameObject);
                }
#else
                Destroy(_renderingCamerasRoot.gameObject);
#endif
            }
        }

        private WaterRenderingModuleParameters GetRenderingModuleParameters()
        {
            //ignore water layer (1 << 4)
            refractionCullingMask &= ~(1 << 4);
            refractionPartiallySubmergedObjectsCullingMask &= ~(1 << 4);
            reflectionCullingMask &= ~(1 << 4);
            reflectionPartiallySubmergedObjectsCullingMask &= ~(1 << 4);

            return new WaterRenderingModuleParameters
            {
                RefractionParameters = new WaterRenderingModeParameters
                {
                    TextuerResizingFactor = refractionRenderTextureResizeFactor,
                    ViewingFrustumHeightScalingFactor = 1f,
                    CullingMask = refractionCullingMask,
                    FilterMode = refractionRenderTextureFilterMode,
                    ZOffset = 0f
                },
                RefractionPartiallySubmergedObjectsParameters = new WaterRenderingModeParameters
                {
                    TextuerResizingFactor = refractionRenderTextureResizeFactor,
                    ViewingFrustumHeightScalingFactor = 1f,
                    CullingMask = refractionPartiallySubmergedObjectsCullingMask,
                    FilterMode = refractionRenderTextureFilterMode,
                    ZOffset = 0f
                },
                ReflectionParameters = new WaterRenderingModeParameters
                {
                    TextuerResizingFactor = reflectionRenderTextureResizeFactor,
                    ViewingFrustumHeightScalingFactor = reflectionViewingFrustumHeightScalingFactor,
                    CullingMask = reflectionCullingMask,
                    FilterMode = reflectionRenderTextureFilterMode,
                    ZOffset = reflectionZOffset
                },
                ReflectionPartiallySubmergedObjectsParameters = new WaterRenderingModeParameters
                {
                    TextuerResizingFactor = reflectionRenderTextureResizeFactor,
                    ViewingFrustumHeightScalingFactor = reflectionPartiallySubmergedObjectsViewingFrustumHeightScalingFactor,
                    CullingMask = reflectionPartiallySubmergedObjectsCullingMask,
                    FilterMode = reflectionRenderTextureFilterMode,
                    ZOffset = reflectionZOffset
                },
                FarClipPlane = farClipPlane,
                RenderPixelLights = renderPixelLights,
                AllowMSAA = allowMSAA,
                AllowHDR = allowHDR,
                SortingOrder = sortingOrder,
                SortingLayerID = sortingLayerID
            };
        }

        private WaterScriptGeneratedRipplesModuleParameters GetScriptGeneratedRipplesModuleParameters()
        {
            return new WaterScriptGeneratedRipplesModuleParameters
            {
                MinimumDisturbance = scriptGeneratedRipplesMinimumDisturbance,
                MaximumDisturbance = scriptGeneratedRipplesMaximumDisturbance,
                SoundEffectParameters = new WaterRipplesSoundEffectParameters
                {
                    IsActive = scriptGeneratedRipplesActivateSoundEffect,
                    AudioClip = scriptGeneratedRipplesAudioClip,
                    UseConstantAudioPitch = scriptGeneratedRipplesUseConstantAudioPitch,
                    AudioPitch = scriptGeneratedRipplesAudioPitch,
                    MinimumAudioPitch = scriptGeneratedRipplesMinimumAudioPitch,
                    MaximumAudioPitch = scriptGeneratedRipplesMaximumAudioPitch,
                    AudioVolume = scriptGeneratedRipplesAudioVolume,
                    PoolSize = scriptGeneratedRipplesSoundEffectPoolSize,
                    CanExpandPool = scriptGeneratedRipplesSoundEffectPoolExpandIfNecessary
                },
                ParticleEffectParameters = new WaterRipplesParticleEffectParameters
                {
                    IsActive = scriptGeneratedRipplesActivateParticleEffect,
                    ParticleSystem = scriptGeneratedRipplesParticleEffect,
                    SpawnOffset = scriptGeneratedRipplesParticleEffectSpawnOffset,
                    StopAction = scriptGeneratedRipplesParticleEffectStopAction,
                    PoolSize = scriptGeneratedRipplesParticleEffectPoolSize,
                    CanExpandPool = scriptGeneratedRipplesParticleEffectPoolExpandIfNecessary
                }
            };
        }

        private WaterConstantRipplesModuleParameters GetConstantRipplesModuleParameters()
        {
            return new WaterConstantRipplesModuleParameters
            {
                IsActive = activateConstantRipples,
                UpdateWhenOffscreen = constantRipplesUpdateWhenOffscreen,
                RandomizeDisturbance = constantRipplesRandomizeDisturbance,
                SmoothDisturbance = constantRipplesSmoothDisturbance,
                SmoothFactor = constantRipplesSmoothFactor,
                Disturbance = constantRipplesDisturbance,
                MinimumDisturbance = constantRipplesMinimumDisturbance,
                MaximumDisturbance = constantRipplesMaximumDisturbance,
                RandomizeInterval = constantRipplesRandomizeInterval,
                Interval = constantRipplesInterval,
                MinimumInterval = constantRipplesMinimumInterval,
                MaximumInterval = constantRipplesMaximumInterval,
                RandomizeRipplesSourcesPositions = constantRipplesRandomizeRipplesSourcesPositions,
                RandomizeRipplesSourcesCount = constantRipplesRandomizeRipplesSourcesCount,
                AllowDuplicateRipplesSourcesPositions = constantRipplesAllowDuplicateRipplesSourcesPositions,
                SourcePositions = constantRipplesSourcePositions,
                SoundEffectParameters = new WaterRipplesSoundEffectParameters
                {
                    IsActive = constantRipplesActivateSoundEffect,
                    AudioClip = constantRipplesAudioClip,
                    UseConstantAudioPitch = constantRipplesUseConstantAudioPitch,
                    AudioPitch = constantRipplesAudioPitch,
                    MinimumAudioPitch = constantRipplesMinimumAudioPitch,
                    MaximumAudioPitch = constantRipplesMaximumAudioPitch,
                    AudioVolume = constantRipplesAudioVolume,
                    PoolSize = constantRipplesSoundEffectPoolSize,
                    CanExpandPool = constantRipplesSoundEffectPoolExpandIfNecessary
                },
                ParticleEffectParameters = new WaterRipplesParticleEffectParameters
                {
                    IsActive = constantRipplesActivateParticleEffect,
                    ParticleSystem = constantRipplesParticleEffect,
                    SpawnOffset = constantRipplesParticleEffectSpawnOffset,
                    StopAction = constantRipplesParticleEffectStopAction,
                    PoolSize = constantRipplesParticleEffectPoolSize,
                    CanExpandPool = constantRipplesParticleEffectPoolExpandIfNecessary
                }
            };
        }

        private WaterCollisionRipplesModuleParameters GetCollisionRipplesModuleParameters()
        {
            return new WaterCollisionRipplesModuleParameters
            {
                ActivateOnWaterEnterRipples = activateOnCollisionOnWaterEnterRipples,
                ActivateOnWaterExitRipples = activateOnCollisionOnWaterExitRipples,
                MinimumDisturbance = onCollisionRipplesMinimumDisturbance,
                MaximumDisturbance = onCollisionRipplesMaximumDisturbance,
                VelocityMultiplier = onCollisionRipplesVelocityMultiplier,
                CollisionMask = onCollisionRipplesCollisionMask,
                CollisionMinimumDepth = onCollisionRipplesCollisionMinimumDepth,
                CollisionMaximumDepth = onCollisionRipplesCollisionMaximumDepth,
                CollisionRaycastMaxDistance = onCollisionRipplesCollisionRaycastMaxDistance,
                OnWaterEnter = onWaterEnter,
                OnWaterExit = onWaterExit,
                WaterEnterSoundEffectParameters = new WaterRipplesSoundEffectParameters
                {
                    IsActive = onCollisionRipplesActivateOnWaterEnterSoundEffect,
                    AudioClip = onCollisionRipplesOnWaterEnterAudioClip,
                    UseConstantAudioPitch = onCollisionRipplesUseConstantOnWaterEnterAudioPitch,
                    AudioPitch = onCollisionRipplesOnWaterEnterAudioPitch,
                    MinimumAudioPitch = onCollisionRipplesOnWaterEnterMinimumAudioPitch,
                    MaximumAudioPitch = onCollisionRipplesOnWaterEnterMaximumAudioPitch,
                    AudioVolume = onCollisionRipplesOnWaterEnterAudioVolume,
                    PoolSize = onCollisionRipplesOnWaterEnterSoundEffectPoolSize,
                    CanExpandPool = onCollisionRipplesOnWaterEnterSoundEffectPoolExpandIfNecessary
                },
                WaterEnterParticleEffectParameters = new WaterRipplesParticleEffectParameters
                {
                    IsActive = onCollisionRipplesActivateOnWaterEnterParticleEffect,
                    ParticleSystem = onCollisionRipplesOnWaterEnterParticleEffect,
                    SpawnOffset = onCollisionRipplesOnWaterEnterParticleEffectSpawnOffset,
                    StopAction = onCollisionRipplesOnWaterEnterParticleEffectStopAction,
                    PoolSize = onCollisionRipplesOnWaterEnterParticleEffectPoolSize,
                    CanExpandPool = onCollisionRipplesOnWaterEnterParticleEffectPoolExpandIfNecessary
                },
                WaterExitSoundEffectParameters = new WaterRipplesSoundEffectParameters
                {
                    IsActive = onCollisionRipplesActivateOnWaterExitSoundEffect,
                    AudioClip = onCollisionRipplesOnWaterExitAudioClip,
                    UseConstantAudioPitch = onCollisionRipplesUseConstantOnWaterExitAudioPitch,
                    AudioPitch = onCollisionRipplesOnWaterExitAudioPitch,
                    MinimumAudioPitch = onCollisionRipplesOnWaterExitMinimumAudioPitch,
                    MaximumAudioPitch = onCollisionRipplesOnWaterExitMaximumAudioPitch,
                    AudioVolume = onCollisionRipplesOnWaterExitAudioVolume,
                    PoolSize = onCollisionRipplesOnWaterExitSoundEffectPoolSize,
                    CanExpandPool = onCollisionRipplesOnWaterExitSoundEffectPoolExpandIfNecessary
                },
                WaterExitParticleEffectParameters = new WaterRipplesParticleEffectParameters
                {
                    IsActive = onCollisionRipplesActivateOnWaterExitParticleEffect,
                    ParticleSystem = onCollisionRipplesOnWaterExitParticleEffect,
                    SpawnOffset = onCollisionRipplesOnWaterExitParticleEffectSpawnOffset,
                    StopAction = onCollisionRipplesOnWaterExitParticleEffectStopAction,
                    PoolSize = onCollisionRipplesOnWaterExitParticleEffectPoolSize,
                    CanExpandPool = onCollisionRipplesOnWaterExitParticleEffectPoolExpandIfNecessary
                }
            };
        }

        #endregion

        #region Editor Only Methods

        #if UNITY_EDITOR

        // Add menu item to create Game2D Water GameObject.
        [MenuItem("GameObject/2D Object/Game2D Water Kit/Water Object", false, 10)]
        private static void CreateWaterObject(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Game2D Water", typeof(Game2DWater));
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        public void OnValidate()
        {
            if (_areModulesInitialized)
            {
                _mainModule.Validate(waterSize);
                _meshModule.Validate(subdivisionsCountPerUnit);
                _simulationModule.Validate(damping, stiffness, spread, firstCustomBoundary, secondCustomBoundary, useCustomBoundaries);
                _renderingModule.Validate(GetRenderingModuleParameters());
                _onCollisonRipplesModule.Validate(GetCollisionRipplesModuleParameters());
                _constantRipplesModule.Validate(GetConstantRipplesModuleParameters());
                _scriptGeneratedRipplesModule.Validate(GetScriptGeneratedRipplesModuleParameters());
                _attachedComponentsModule.Validate(buoyancyEffectorSurfaceLevel);
            }
        }

        private void Reset()
        {
            //Reset serialized variables to default values

            #region Water Mesh Serialized Variables
            waterSize = Vector2.one;
            subdivisionsCountPerUnit = 3;
            #endregion

            #region Water Simulation Serialized Variables
            damping = 0.05f;
            stiffness = 60f;
            spread = 60f;
            useCustomBoundaries = false;
            firstCustomBoundary = 0.5f;
            secondCustomBoundary = -0.5f;
            #endregion

            #region Water Collision Ripples Serialized Variables
            activateOnCollisionOnWaterEnterRipples = true;
            activateOnCollisionOnWaterExitRipples = true;
            //Disturbance Properties
            onCollisionRipplesMinimumDisturbance = 0.1f;
            onCollisionRipplesMaximumDisturbance = 0.75f;
            onCollisionRipplesVelocityMultiplier = 0.12f;
            //Collision Properties
            onCollisionRipplesCollisionMask = ~(1 << 4);
            onCollisionRipplesCollisionMinimumDepth = -10f;
            onCollisionRipplesCollisionMaximumDepth = 10f;
            onCollisionRipplesCollisionRaycastMaxDistance = 0.5f;
            //Particle Effect Properties (On Water Enter)
            onCollisionRipplesActivateOnWaterEnterParticleEffect = false;
            onCollisionRipplesOnWaterEnterParticleEffect = null;
            onCollisionRipplesOnWaterEnterParticleEffectSpawnOffset = Vector3.zero;
            onCollisionRipplesOnWaterEnterParticleEffectStopAction = new UnityEvent();
            onCollisionRipplesOnWaterEnterParticleEffectPoolSize = 10;
            onCollisionRipplesOnWaterEnterParticleEffectPoolExpandIfNecessary = true;
            //Sound Effect Properties (On Water Enter)
            onCollisionRipplesActivateOnWaterEnterSoundEffect = true;
            onCollisionRipplesOnWaterEnterAudioClip = null;
            onCollisionRipplesUseConstantOnWaterEnterAudioPitch = false;
            onCollisionRipplesOnWaterEnterAudioPitch = 1f;
            onCollisionRipplesOnWaterEnterMinimumAudioPitch = 0.75f;
            onCollisionRipplesOnWaterEnterMaximumAudioPitch = 1.25f;
            onCollisionRipplesOnWaterEnterAudioVolume = 1.0f;
            onCollisionRipplesOnWaterEnterSoundEffectPoolSize = 10;
            onCollisionRipplesOnWaterEnterSoundEffectPoolExpandIfNecessary = true;
            //Particle Effect Properties (On Water Exit)
            onCollisionRipplesActivateOnWaterExitParticleEffect = false;
            onCollisionRipplesOnWaterExitParticleEffect = null;
            onCollisionRipplesOnWaterExitParticleEffectSpawnOffset = Vector3.zero;
            onCollisionRipplesOnWaterExitParticleEffectStopAction = new UnityEvent();
            onCollisionRipplesOnWaterExitParticleEffectPoolSize = 10;
            onCollisionRipplesOnWaterExitParticleEffectPoolExpandIfNecessary = true;
            //Sound Effect Properties (On Water Exit)
            onCollisionRipplesActivateOnWaterExitSoundEffect = false;
            onCollisionRipplesOnWaterExitAudioClip = null;
            onCollisionRipplesUseConstantOnWaterExitAudioPitch = false;
            onCollisionRipplesOnWaterExitAudioPitch = 1f;
            onCollisionRipplesOnWaterExitMinimumAudioPitch = 0.75f;
            onCollisionRipplesOnWaterExitMaximumAudioPitch = 1.25f;
            onCollisionRipplesOnWaterExitAudioVolume = 1.0f;
            onCollisionRipplesOnWaterExitSoundEffectPoolSize = 10;
            onCollisionRipplesOnWaterExitSoundEffectPoolExpandIfNecessary = true;
            //Events
            onWaterEnter = new UnityEvent();
            onWaterExit = new UnityEvent();
            #endregion

            #region Water Constant Ripples Serialized Variables
            activateConstantRipples = false;
            constantRipplesUpdateWhenOffscreen = false;
            //Disturbance Properties
            constantRipplesRandomizeDisturbance = false;
            constantRipplesSmoothDisturbance = false;
            constantRipplesSmoothFactor = 0.5f;
            constantRipplesDisturbance = 0.10f;
            constantRipplesMinimumDisturbance = 0.08f;
            constantRipplesMaximumDisturbance = 0.12f;
            //Interval Properties
            constantRipplesRandomizeInterval = false;
            constantRipplesInterval = 1f;
            constantRipplesMinimumInterval = 0.75f;
            constantRipplesMaximumInterval = 1.25f;
            //Ripple Source Positions Properties
            constantRipplesRandomizeRipplesSourcesPositions = false;
            constantRipplesRandomizeRipplesSourcesCount = 3;
            constantRipplesAllowDuplicateRipplesSourcesPositions = false;
            constantRipplesSourcePositions = new List<float>();
            //Sound Effect Properties
            constantRipplesActivateSoundEffect = false;
            constantRipplesUseConstantAudioPitch = false;
            constantRipplesAudioClip = null;
            constantRipplesAudioPitch = 1f;
            constantRipplesMinimumAudioPitch = 0.75f;
            constantRipplesMaximumAudioPitch = 1.25f;
            constantRipplesSoundEffectPoolSize = 10;
            constantRipplesSoundEffectPoolExpandIfNecessary = true;
            constantRipplesAudioVolume = 1.0f;
            //Particle Effect Properties
            constantRipplesActivateParticleEffect = false;
            constantRipplesParticleEffect = null;
            constantRipplesParticleEffectSpawnOffset = Vector3.zero;
            constantRipplesParticleEffectStopAction = new UnityEvent();
            constantRipplesParticleEffectPoolSize = 10;
            constantRipplesParticleEffectPoolExpandIfNecessary = true;
            #endregion

            #region Water Script Generated Ripples Serialized Variables
            //Disturbance Properties
            scriptGeneratedRipplesMinimumDisturbance = 0.1f;
            scriptGeneratedRipplesMaximumDisturbance = 0.75f;
            //Sound Effect Properties
            scriptGeneratedRipplesActivateSoundEffect = false;
            scriptGeneratedRipplesAudioClip = null;
            scriptGeneratedRipplesUseConstantAudioPitch = false;
            scriptGeneratedRipplesAudioPitch = 1f;
            scriptGeneratedRipplesMinimumAudioPitch = 0.75f;
            scriptGeneratedRipplesMaximumAudioPitch = 1.25f;
            scriptGeneratedRipplesAudioVolume = 1.0f;
            scriptGeneratedRipplesSoundEffectPoolSize = 10;
            scriptGeneratedRipplesSoundEffectPoolExpandIfNecessary = true;
            //Particle Effect Properties
            scriptGeneratedRipplesActivateParticleEffect = false;
            scriptGeneratedRipplesParticleEffect = null;
            scriptGeneratedRipplesParticleEffectSpawnOffset = Vector3.zero;
            scriptGeneratedRipplesParticleEffectStopAction = new UnityEvent();
            scriptGeneratedRipplesParticleEffectPoolSize = 10;
            scriptGeneratedRipplesParticleEffectPoolExpandIfNecessary = true;
            #endregion

            #region Water Rendering Serialized Variables
            //Refraction Properties
            refractionRenderTextureResizeFactor = 1f;
            refractionCullingMask = ~(1 << 4);
            refractionPartiallySubmergedObjectsCullingMask = 0;
            refractionRenderTextureFilterMode = FilterMode.Bilinear;
            //Reflection Properties
            reflectionRenderTextureResizeFactor = 1f;
            reflectionCullingMask = ~(1 << 4);
            reflectionPartiallySubmergedObjectsCullingMask = 0;
            reflectionRenderTextureFilterMode = FilterMode.Bilinear;
            reflectionZOffset = 0f;
            //Shared Properties
            sortingLayerID = 0;
            sortingOrder = 0;
            farClipPlane = 100f;
            renderPixelLights = true;
            allowMSAA = false;
            allowHDR = false;
            #endregion

            #region Attached components Serialized Variables
            buoyancyEffectorSurfaceLevel = 0.02f;
            #endregion

            //Clean up instantiated objects (Cameras used for rendering "refraction" and "reflection", ripples particle effect and sound effect pooled objects)
            DestroyAllSpawnedObjects();

            //Reset modules!
            _areModulesInitialized = false;
            InitializeModules();
        }

        private void DestroyRenderingCameras()
        {
            if(_renderingCamerasRoot != null)
            {
                foreach (Transform cam in _renderingCamerasRoot.transform)
                {
                    DestroyImmediate(cam.gameObject);
                }
            }
        }

        #endif

        #endregion
    }
}
