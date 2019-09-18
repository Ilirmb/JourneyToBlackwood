namespace Game2DWaterKit
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    public partial class Game2DWater : MonoBehaviour
    {
        #region Deprecated Properties

        #region Water Properties
        //MeshProperties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public Vector2 WaterSize { get { return _mainModule.WaterSize; } set { _mainModule.SetWaterSize(value); } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public int SubdivisionsCountPerUnit { get { return _meshModule.SubdivisionsPerUnit; } set { _meshModule.SubdivisionsPerUnit = value; } }
        //Wave Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float Damping { get { return _simulationModule.Damping; } set { _simulationModule.Damping = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float Spread { get { return _simulationModule.Spread; } set { _simulationModule.Spread = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float Stiffness { get { return _simulationModule.Stiffness; } set { _simulationModule.Stiffness = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool UseCustomBoundaries { get { return _simulationModule.IsUsingCustomBoundaries; } set { _simulationModule.IsUsingCustomBoundaries = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float FirstCustomBoundary { get { return _simulationModule.FirstCustomBoundary; } set { _simulationModule.FirstCustomBoundary = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float SecondCustomBoundary { get { return _simulationModule.SecondCustomBoundary; } set { _simulationModule.SecondCustomBoundary = value; } }
        //Misc Properties
        public float BuoyancyEffectorSurfaceLevel { get { return _attachedComponentsModule.BuoyancyEffectorSurfaceLevel; } set { _attachedComponentsModule.BuoyancyEffectorSurfaceLevel = value; } }
        //Water Events
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public UnityEvent OnWaterEnter { get { return _onCollisonRipplesModule.OnWaterEnter; } set { _onCollisonRipplesModule.OnWaterEnter = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public UnityEvent OnWaterExit { get { return _onCollisonRipplesModule.OnWaterExit; } set { _onCollisonRipplesModule.OnWaterExit = value; } }
        #endregion

        #region On-Collision Ripples Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ActivateOnCollisionOnWaterEnterRipples { get { return _onCollisonRipplesModule.IsOnWaterEnterRipplesActive; } set { _onCollisonRipplesModule.IsOnWaterEnterRipplesActive = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ActivateOnCollisionOnWaterExitRipples { get { return _onCollisonRipplesModule.IsOnWaterExitRipplesActive; } set { _onCollisonRipplesModule.IsOnWaterExitRipplesActive = value; } }
        //Disturbance Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float OnCollisionRipplesMinimumDisturbance { get { return _onCollisonRipplesModule.MinimumDisturbance; } set { _onCollisonRipplesModule.MinimumDisturbance = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float MinimumDisturbance { get { return _onCollisonRipplesModule.MinimumDisturbance; } set { _onCollisonRipplesModule.MinimumDisturbance = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float OnCollisionRipplesMaximumDisturbance { get { return _onCollisonRipplesModule.MaximumDisturbance; } set { _onCollisonRipplesModule.MaximumDisturbance = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float MaximumDisturbance { get { return _onCollisonRipplesModule.MaximumDisturbance; } set { _onCollisonRipplesModule.MaximumDisturbance = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float OnCollisionRipplesVelocityMultiplier { get { return _onCollisonRipplesModule.VelocityMultiplier; } set { _onCollisonRipplesModule.VelocityMultiplier = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float VelocityMultiplier { get { return _onCollisonRipplesModule.VelocityMultiplier; } set { _onCollisonRipplesModule.VelocityMultiplier = value; } }
        //Collision Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public LayerMask OnCollisionRipplesCollisionMask { get { return _onCollisonRipplesModule.CollisionMask; } set { _onCollisonRipplesModule.CollisionMask = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public LayerMask CollisionMask { get { return _onCollisonRipplesModule.CollisionMask; } set { _onCollisonRipplesModule.CollisionMask = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float OnCollisionRipplesCollisionRaycastMaxDistance { get { return _onCollisonRipplesModule.CollisionRaycastMaximumDistance; } set { _onCollisonRipplesModule.CollisionRaycastMaximumDistance = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float CollisionRaycastMaxDistance { get { return _onCollisonRipplesModule.CollisionRaycastMaximumDistance; } set { _onCollisonRipplesModule.CollisionRaycastMaximumDistance = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float OnCollisionRipplesCollisionMinimumDepth { get { return _onCollisonRipplesModule.CollisionMinimumDepth; } set { _onCollisonRipplesModule.CollisionMinimumDepth = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float MinimumCollisionDepth { get { return _onCollisonRipplesModule.CollisionMinimumDepth; } set { _onCollisonRipplesModule.CollisionMinimumDepth = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float OnCollisionRipplesCollisionMaximumDepth { get { return _onCollisonRipplesModule.CollisionMaximumDepth; } set { _onCollisonRipplesModule.CollisionMaximumDepth = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float MaximumCollisionDepth { get { return _onCollisonRipplesModule.CollisionMaximumDepth; } set { _onCollisonRipplesModule.CollisionMaximumDepth = value; } }
        //Sound Effect Properties (On Water Enter)
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool OnCollisionRipplesActivateOnWaterEnterSoundEffect { get { return _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.IsActive; } set { _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.IsActive = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public AudioClip OnCollisionRipplesOnWaterEnterAudioClip { get { return _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.AudioClip; } set { _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.AudioClip = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public AudioClip SplashAudioClip { get { return _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.AudioClip; } set { _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.AudioClip = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float OnCollisionRipplesOnWaterEnterMinimumAudioPitch { get { return _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.AudioPitch; } set { _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.AudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float MinimumAudioPitch { get { return _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.MinimumAudioPitch; } set { _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.MinimumAudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float OnCollisionRipplesOnWaterEnterMaximumAudioPitch { get { return _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.MaximumAudioPitch; } set { _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.MaximumAudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float MaximumAudioPitch { get { return _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.MaximumAudioPitch; } set { _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.MaximumAudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float OnCollisionRipplesOnWaterEnterAudioVolume { get { return _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.AudioVolume; } set { _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.AudioVolume = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool OnCollisionRipplesUseConstantOnWaterEnterAudioPitch { get { return _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.IsUsingConstantAudioPitch; } set { _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.IsUsingConstantAudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool UseConstantAudioPitch { get { return _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.IsUsingConstantAudioPitch; } set { _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.IsUsingConstantAudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float OnCollisionRipplesOnWaterEnterAudioPitch { get { return _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.AudioPitch; } set { _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.AudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float AudioPitch { get { return _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.AudioPitch; } set { _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.AudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public int OnCollisionRipplesOnWaterEnterSoundEffectPoolSize { get { return _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.PoolSize; } set { _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.PoolSize = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool OnCollisionRipplesOnWaterEnterSoundEffectPoolExpandIfNecessary { get { return _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.CanExpandPool; } set { _onCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.CanExpandPool = value; } }
        //Particle Effect Proeprties (On Water Enter)
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool OnCollisionRipplesActivateOnWaterEnterParticleEffect { get { return _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.IsActive; } set { _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.IsActive = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ActivateOnCollisionSplashParticleEffect { get { return _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.IsActive; } set { _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.IsActive = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public ParticleSystem OnCollisionRipplesOnWaterEnterParticleEffect { get { return _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.ParticleSystem; } set { _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.ParticleSystem = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public ParticleSystem OnCollisionSplashParticleEffect { get { return _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.ParticleSystem; } set { _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.ParticleSystem = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public int OnCollisionRipplesOnWaterEnterParticleEffectPoolSize { get { return _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.PoolSize; } set { _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.PoolSize = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public int OnCollisionSplashParticleEffectPoolSize { get { return _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.PoolSize; } set { _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.PoolSize = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public Vector3 OnCollisionRipplesOnWaterEnterParticleEffectSpawnOffset { get { return _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.SpawnOffset; } set { _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.SpawnOffset = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public Vector3 OnCollisionSplashParticleEffectSpawnOffset { get { return _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.SpawnOffset; } set { _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.SpawnOffset = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public UnityEvent OnCollisionRipplesOnWaterEnterParticleEffectStopAction { get { return _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.StopAction; } set { _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.StopAction = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool OnCollisionRipplesOnWaterEnterParticleEffectPoolExpandIfNecessary { get { return _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.CanExpandPool; } set { _onCollisonRipplesModule.OnWaterEnterRipplesParticleEffect.CanExpandPool = value; } }
        //Sound Effect Properties (On Water Exit)
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool OnCollisionRipplesActivateOnWaterExitSoundEffect { get { return _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.IsActive; } set { _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.IsActive = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public AudioClip OnCollisionRipplesOnWaterExitAudioClip { get { return _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.AudioClip; } set { _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.AudioClip = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float OnCollisionRipplesOnWaterExitMinimumAudioPitch { get { return _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.AudioPitch; } set { _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.AudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float OnCollisionRipplesOnWaterExitMaximumAudioPitch { get { return _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.MaximumAudioPitch; } set { _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.MaximumAudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float OnCollisionRipplesOnWaterExitAudioVolume { get { return _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.AudioVolume; } set { _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.AudioVolume = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool OnCollisionRipplesUseConstantOnWaterExitAudioPitch { get { return _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.IsUsingConstantAudioPitch; } set { _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.IsUsingConstantAudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float OnCollisionRipplesOnWaterExitAudioPitch { get { return _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.AudioPitch; } set { _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.AudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public int OnCollisionRipplesOnWaterExitSoundEffectPoolSize { get { return _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.PoolSize; } set { _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.PoolSize = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool OnCollisionRipplesOnWaterExitSoundEffectPoolExpandIfNecessary { get { return _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.CanExpandPool; } set { _onCollisonRipplesModule.OnWaterExitRipplesSoundEffect.CanExpandPool = value; } }
        //Particle Effect Proeprties (On Water Exit)
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool OnCollisionRipplesActivateOnWaterExitParticleEffect { get { return _onCollisonRipplesModule.OnWaterExitRipplesParticleEffect.IsActive; } set { _onCollisonRipplesModule.OnWaterExitRipplesParticleEffect.IsActive = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public ParticleSystem OnCollisionRipplesOnWaterExitParticleEffect { get { return _onCollisonRipplesModule.OnWaterExitRipplesParticleEffect.ParticleSystem; } set { _onCollisonRipplesModule.OnWaterExitRipplesParticleEffect.ParticleSystem = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public int OnCollisionRipplesOnWaterExitParticleEffectPoolSize { get { return _onCollisonRipplesModule.OnWaterExitRipplesParticleEffect.PoolSize; } set { _onCollisonRipplesModule.OnWaterExitRipplesParticleEffect.PoolSize = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public Vector3 OnCollisionRipplesOnWaterExitParticleEffectSpawnOffset { get { return _onCollisonRipplesModule.OnWaterExitRipplesParticleEffect.SpawnOffset; } set { _onCollisonRipplesModule.OnWaterExitRipplesParticleEffect.SpawnOffset = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public UnityEvent OnCollisionRipplesOnWaterExitParticleEffectStopAction { get { return _onCollisonRipplesModule.OnWaterExitRipplesParticleEffect.StopAction; } set { _onCollisonRipplesModule.OnWaterExitRipplesParticleEffect.StopAction = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool OnCollisionRipplesOnWaterExitParticleEffectPoolExpandIfNecessary { get { return _onCollisonRipplesModule.OnWaterExitRipplesParticleEffect.CanExpandPool; } set { _onCollisonRipplesModule.OnWaterExitRipplesParticleEffect.CanExpandPool = value; } }
        #endregion

        #region Constant Ripples Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ActivateConstantRipples { get { return _constantRipplesModule.IsActive; } set { _constantRipplesModule.IsActive = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ConstantRipplesUpdateWhenOffscreen { get { return _constantRipplesModule.UpdateWhenOffscreen; } set { _constantRipplesModule.UpdateWhenOffscreen = value; } }
        //Disturbance Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ConstantRipplesRandomizeDisturbance { get { return _constantRipplesModule.RandomizeDisturbance; } set { _constantRipplesModule.RandomizeDisturbance = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ConstantRipplesDisturbance { get { return _constantRipplesModule.Disturbance; } set { _constantRipplesModule.Disturbance = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ConstantRipplesMinimumDisturbance { get { return _constantRipplesModule.MinimumDisturbance; } set { _constantRipplesModule.MinimumDisturbance = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ConstantRipplesMaximumDisturbance { get { return _constantRipplesModule.MaximumDisturbance; } set { _constantRipplesModule.MaximumDisturbance = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ConstantRipplesSmoothDisturbance { get { return _constantRipplesModule.SmoothRipples; } set { _constantRipplesModule.SmoothRipples = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ConstantRipplesSmoothFactor { get { return _constantRipplesModule.SmoothingFactor; } set { _constantRipplesModule.SmoothingFactor = value; } }
        //Interval Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ConstantRipplesRandomizeInterval { get { return _constantRipplesModule.RandomizeTimeInterval; } set { _constantRipplesModule.RandomizeTimeInterval = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ConstantRipplesInterval { get { return _constantRipplesModule.TimeInterval; } set { _constantRipplesModule.TimeInterval = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ConstantRipplesMinimumInterval { get { return _constantRipplesModule.MinimumTimeInterval; } set { _constantRipplesModule.MinimumTimeInterval = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ConstantRipplesMaximumInterval { get { return _constantRipplesModule.MaximumTimeInterval; } set { _constantRipplesModule.MaximumTimeInterval = value; } }
        //Ripple Source Positions Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ConstantRipplesRandomizeRipplesSourcesPositions { get { return _constantRipplesModule.RandomizeRipplesSourcePositions; } set { _constantRipplesModule.RandomizeRipplesSourcePositions = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public List<float> ConstantRipplesSourcePositions { get { return _constantRipplesModule.SourcePositions; } set { _constantRipplesModule.SourcePositions = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public int ConstantRipplesRandomizeRipplesSourcesCount { get { return _constantRipplesModule.RandomRipplesSourceCount; } set { _constantRipplesModule.RandomRipplesSourceCount = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ConstantRipplesAllowDuplicateRipplesSourcesPositions { get { return _constantRipplesModule.AllowDuplicateRipplesSourcePositions; } set { _constantRipplesModule.AllowDuplicateRipplesSourcePositions = value; } }
        //Sound Effect Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ConstantRipplesActivateSoundEffect { get { return _constantRipplesModule.SoundEffect.IsActive; } set { _constantRipplesModule.SoundEffect.IsActive = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public AudioClip ConstantRipplesAudioClip { get { return _constantRipplesModule.SoundEffect.AudioClip; } set { _constantRipplesModule.SoundEffect.AudioClip = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ConstantRipplesMinimumAudioPitch { get { return _constantRipplesModule.SoundEffect.AudioPitch; } set { _constantRipplesModule.SoundEffect.AudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ConstantRipplesMaximumAudioPitch { get { return _constantRipplesModule.SoundEffect.MaximumAudioPitch; } set { _constantRipplesModule.SoundEffect.MaximumAudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ConstantRipplesAudioVolume { get { return _constantRipplesModule.SoundEffect.AudioVolume; } set { _constantRipplesModule.SoundEffect.AudioVolume = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ConstantRipplesUseConstantAudioPitch { get { return _constantRipplesModule.SoundEffect.IsUsingConstantAudioPitch; } set { _constantRipplesModule.SoundEffect.IsUsingConstantAudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ConstantRipplesAudioPitch { get { return _constantRipplesModule.SoundEffect.AudioPitch; } set { _constantRipplesModule.SoundEffect.AudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public int ConstantRipplesSoundEffectPoolSize { get { return _constantRipplesModule.SoundEffect.PoolSize; } set { _constantRipplesModule.SoundEffect.PoolSize = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ConstantRipplesSoundEffectPoolExpandIfNecessary { get { return _constantRipplesModule.SoundEffect.CanExpandPool; } set { _constantRipplesModule.SoundEffect.CanExpandPool = value; } }
        //Particle Effect Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ConstantRipplesActivateParticleEffect { get { return _constantRipplesModule.ParticleEffect.IsActive; } set { _constantRipplesModule.ParticleEffect.IsActive = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ActivateConstantSplashParticleEffect { get { return _constantRipplesModule.ParticleEffect.IsActive; } set { _constantRipplesModule.ParticleEffect.IsActive = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public ParticleSystem ConstantRipplesParticleEffect { get { return _constantRipplesModule.ParticleEffect.ParticleSystem; } set { _constantRipplesModule.ParticleEffect.ParticleSystem = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public ParticleSystem ConstantSplashParticleEffect { get { return _constantRipplesModule.ParticleEffect.ParticleSystem; } set { _constantRipplesModule.ParticleEffect.ParticleSystem = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public int ConstantRipplesParticleEffectPoolSize { get { return _constantRipplesModule.ParticleEffect.PoolSize; } set { _constantRipplesModule.ParticleEffect.PoolSize = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public int ConstantSplashParticleEffectPoolSize { get { return _constantRipplesModule.ParticleEffect.PoolSize; } set { _constantRipplesModule.ParticleEffect.PoolSize = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public UnityEvent ConstantRipplesParticleEffectStopAction { get { return _constantRipplesModule.ParticleEffect.StopAction; } set { _constantRipplesModule.ParticleEffect.StopAction = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public Vector3 ConstantRipplesParticleEffectSpawnOffset { get { return _constantRipplesModule.ParticleEffect.SpawnOffset; } set { _constantRipplesModule.ParticleEffect.SpawnOffset = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public Vector3 ConstantSplashParticleEffectSpawnOffset { get { return _constantRipplesModule.ParticleEffect.SpawnOffset; } set { _constantRipplesModule.ParticleEffect.SpawnOffset = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ConstantRipplesParticleEffectPoolExpandIfNecessary { get { return _constantRipplesModule.ParticleEffect.CanExpandPool; } set { _constantRipplesModule.ParticleEffect.CanExpandPool = value; } }
        #endregion

        #region Script-Generated Ripples Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ScriptGeneratedRipplesMinimumDisturbance { get { return _scriptGeneratedRipplesModule.MinimumDisturbance; } set { _scriptGeneratedRipplesModule.MinimumDisturbance = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ScriptGeneratedRipplesMaximumDisturbance { get { return _scriptGeneratedRipplesModule.MaximumDisturbance; } set { _scriptGeneratedRipplesModule.MaximumDisturbance = value; } }
        //Sound Effect Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ScriptGeneratedRipplesActivateSoundEffect { get { return _scriptGeneratedRipplesModule.SoundEffect.IsActive; } set { _scriptGeneratedRipplesModule.SoundEffect.IsActive = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public AudioClip ScriptGeneratedRipplesAudioClip { get { return _scriptGeneratedRipplesModule.SoundEffect.AudioClip; } set { _scriptGeneratedRipplesModule.SoundEffect.AudioClip = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ScriptGeneratedRipplesMinimumAudioPitch { get { return _scriptGeneratedRipplesModule.SoundEffect.MinimumAudioPitch; } set { _scriptGeneratedRipplesModule.SoundEffect.MinimumAudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ScriptGeneratedRipplesMaximumAudioPitch { get { return _scriptGeneratedRipplesModule.SoundEffect.MaximumAudioPitch; } set { _scriptGeneratedRipplesModule.SoundEffect.MaximumAudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ScriptGeneratedRipplesUseConstantAudioPitch { get { return _scriptGeneratedRipplesModule.SoundEffect.IsUsingConstantAudioPitch; } set { _scriptGeneratedRipplesModule.SoundEffect.IsUsingConstantAudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ScriptGeneratedRipplesAudioPitch { get { return _scriptGeneratedRipplesModule.SoundEffect.AudioPitch; } set { _scriptGeneratedRipplesModule.SoundEffect.AudioPitch = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ScriptGeneratedRipplesAudioVolume { get { return _scriptGeneratedRipplesModule.SoundEffect.AudioVolume; } set { _scriptGeneratedRipplesModule.SoundEffect.AudioVolume = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public int ScriptGeneratedRipplesSoundEffectPoolSize { get { return _scriptGeneratedRipplesModule.SoundEffect.PoolSize; } set { _scriptGeneratedRipplesModule.SoundEffect.PoolSize = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ScriptGeneratedRipplesSoundEffectPoolExpandIfNecessary { get { return _scriptGeneratedRipplesModule.SoundEffect.CanExpandPool; } set { _scriptGeneratedRipplesModule.SoundEffect.CanExpandPool = value; } }
        //Particle Effect Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ScriptGeneratedRipplesActivateParticleEffect { get { return _scriptGeneratedRipplesModule.ParticleEffect.IsActive; } set { _scriptGeneratedRipplesModule.ParticleEffect.IsActive = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public ParticleSystem ScriptGeneratedRipplesParticleEffect { get { return _scriptGeneratedRipplesModule.ParticleEffect.ParticleSystem; } set { _scriptGeneratedRipplesModule.ParticleEffect.ParticleSystem = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public int ScriptGeneratedRipplesParticleEffectPoolSize { get { return _scriptGeneratedRipplesModule.ParticleEffect.PoolSize; } set { _scriptGeneratedRipplesModule.ParticleEffect.PoolSize = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public Vector3 ScriptGeneratedRipplesParticleEffectSpawnOffset { get { return _scriptGeneratedRipplesModule.ParticleEffect.SpawnOffset; } set { _scriptGeneratedRipplesModule.ParticleEffect.SpawnOffset = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public UnityEvent ScriptGeneratedRipplesParticleEffectStopAction { get { return _scriptGeneratedRipplesModule.ParticleEffect.StopAction; } set { _scriptGeneratedRipplesModule.ParticleEffect.StopAction = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool ScriptGeneratedRipplesParticleEffectPoolExpandIfNecessary { get { return _scriptGeneratedRipplesModule.ParticleEffect.CanExpandPool; } set { _scriptGeneratedRipplesModule.ParticleEffect.CanExpandPool = value; } }
        #endregion

        #region Refraction & Reflection Rendering Properties
        //Refraction Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float RefractionRenderTextureResizeFactor { get {return _renderingModule.Refraction.RenderTextureResizingFactor; } set { _renderingModule.Refraction.RenderTextureResizingFactor = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public LayerMask RefractionCullingMask { get { return _renderingModule.Refraction.CullingMask; } set { _renderingModule.Refraction.CullingMask = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public FilterMode RefractionRenderTextureFilterMode { get { return _renderingModule.Refraction.RenderTextureFilterMode; } set { _renderingModule.Refraction.RenderTextureFilterMode = value; } }
        //Reflection Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ReflectionRenderTextureResizeFactor { get { return _renderingModule.Reflection.RenderTextureResizingFactor; } set { _renderingModule.Reflection.RenderTextureResizingFactor = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public LayerMask ReflectionCullingMask { get { return _renderingModule.Reflection.CullingMask; } set { _renderingModule.Reflection.CullingMask = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public FilterMode ReflectionRenderTextureFilterMode { get { return _renderingModule.Reflection.RenderTextureFilterMode; } set { _renderingModule.Reflection.RenderTextureFilterMode = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float ReflectionZOffset { get { return _renderingModule.Reflection.ZOffset; } set { _renderingModule.Reflection.ZOffset = value; } }
        //Other Properties
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public int SortingLayerID { get { return _renderingModule.SortingLayerID; } set { _renderingModule.SortingLayerID = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public int SortingOrder { get { return _renderingModule.SortingOrder; } set { _renderingModule.SortingOrder = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool RenderPixelLights { get { return _renderingModule.RenderPixelLights; } set { _renderingModule.RenderPixelLights = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public float FarClipPlane { get { return _renderingModule.FarClipPlane; } set { _renderingModule.FarClipPlane = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool AllowMSAA { get { return _renderingModule.AllowMSAA; } set { _renderingModule.AllowMSAA = value; } }
        [System.Obsolete("This property is now deprecated. You could safely remove the [Deprecated] folder in the scripts folder to get rid of all of deprecated properties")]
        public bool AllowHDR { get { return _renderingModule.AllowHDR; } set { _renderingModule.AllowHDR = value; } }
        #endregion

        #endregion

        #region Deprecated Methods
        [System.Obsolete("Please use ScriptGeneratedRipplesManager.GenerateRipple instead")]
        public void GenerateRipple(Vector2 position, float disturbanceFactor, bool playSoundEffect, bool playParticleEffect, bool smooth, float smoothingFactor = 0.5f)
        {
            _scriptGeneratedRipplesModule.GenerateRipple(position, disturbanceFactor,(disturbanceFactor < 0f), playSoundEffect, playParticleEffect, smooth, smoothingFactor);
        }
        [System.Obsolete("Please use ScriptGeneratedRipplesManager.GenerateRipple instead")]
        public void CreateSplash(float xPosition, float disturbanceFactor, bool playSoundEffect, bool playParticleEffect, bool smooth, float smoothingFactor = 0.5f)
        {
            _scriptGeneratedRipplesModule.GenerateRipple(new Vector2(xPosition, 0f), disturbanceFactor, (disturbanceFactor < 0f), playSoundEffect, playParticleEffect, smooth, smoothingFactor);
        }
        #endregion
    }
}
