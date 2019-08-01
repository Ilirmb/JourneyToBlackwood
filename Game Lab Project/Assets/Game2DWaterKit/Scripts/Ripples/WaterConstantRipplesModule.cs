namespace Game2DWaterKit.Ripples
{
    using Game2DWaterKit.Mesh;
    using Game2DWaterKit.Ripples.Effects;
    using Game2DWaterKit.Simulation;
    using Game2DWaterKit.Main;
    using System.Collections.Generic;
    using UnityEngine;

    public class WaterConstantRipplesModule
    {
        #region Variables
        private readonly Transform _ripplesEffectsRoot;
        private readonly WaterRipplesSoundEffect _soundEffect;
        private readonly WaterRipplesParticleEffect _particleEffect;

        private bool _isActive;
        private bool _updateWhenOffscreen;
        private bool _randomizeDisturbance;
        private bool _smoothRipples;
        private float _smoothingFactor;
        private float _disturbance;
        private float _minimumDisturbance;
        private float _maximumDisturbance;
        private bool _randomizeTimeInterval;
        private float _timeInterval;
        private float _minimumTimeInterval;
        private float _maximumTimeInterval;
        private bool _randomizeRipplesSourcePositions;
        private int _randomRipplesSourceCount;
        private bool _allowDuplicateRipplesSourcePositions;
        private List<float> _sourcePositions;

        private WaterMainModule _mainModule;
        private WaterMeshModule _meshModule;
        private WaterSimulationModule _simulationModule;

        private float _elapsedTime;
        private float _currentInterval;
        private List<int> _ripplesSourcesIndices;
        #endregion

        public WaterConstantRipplesModule(WaterConstantRipplesModuleParameters parameters, Transform ripplesEffectsRoot)
        {
            _isActive = parameters.IsActive;
            _updateWhenOffscreen = parameters.UpdateWhenOffscreen;
            _randomizeDisturbance = parameters.RandomizeDisturbance;
            _smoothRipples = parameters.SmoothDisturbance;
            _smoothingFactor = parameters.SmoothFactor;
            _disturbance = parameters.Disturbance;
            _minimumDisturbance = parameters.MinimumDisturbance;
            _maximumDisturbance = parameters.MaximumDisturbance;
            _randomizeTimeInterval = parameters.RandomizeInterval;
            _timeInterval = parameters.Interval;
            _minimumTimeInterval = parameters.MinimumInterval;
            _maximumTimeInterval = parameters.MaximumInterval;
            _randomizeRipplesSourcePositions = parameters.RandomizeRipplesSourcesPositions;
            _randomRipplesSourceCount = parameters.RandomizeRipplesSourcesCount;
            _allowDuplicateRipplesSourcePositions = parameters.AllowDuplicateRipplesSourcesPositions;
            _sourcePositions = parameters.SourcePositions;

            _ripplesEffectsRoot = CreateRipplesEffectsRoot(ripplesEffectsRoot);

            _particleEffect = new WaterRipplesParticleEffect(parameters.ParticleEffectParameters, _ripplesEffectsRoot);
            _soundEffect = new WaterRipplesSoundEffect(parameters.SoundEffectParameters, _ripplesEffectsRoot);

        }

        #region Properties
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }
        public bool UpdateWhenOffscreen { get { return _updateWhenOffscreen; } set { _updateWhenOffscreen = value; } }
        public float Disturbance { get { return _disturbance; } set { _disturbance = Mathf.Clamp(value, 0f, float.MaxValue); } }
        public float MaximumDisturbance { get { return _maximumDisturbance; } set { _maximumDisturbance = Mathf.Clamp(value, 0f, float.MaxValue); } }
        public float MinimumDisturbance { get { return _minimumDisturbance; } set { _minimumDisturbance = Mathf.Clamp(value, 0f, float.MaxValue); } }
        public bool RandomizeDisturbance { get { return _randomizeDisturbance; } set { _randomizeDisturbance = value; } }
        public bool SmoothRipples { get { return _smoothRipples; } set { _smoothRipples = value; } }
        public float SmoothingFactor { get { return _smoothingFactor; } set { _smoothingFactor = Mathf.Clamp01(value); } }
        public float TimeInterval { get { return _timeInterval; } set { _timeInterval = Mathf.Clamp(value, 0f, float.MaxValue); } }
        public float MaximumTimeInterval { get { return _maximumTimeInterval; } set { _maximumTimeInterval = Mathf.Clamp(value, 0f, float.MaxValue); } }
        public float MinimumTimeInterval { get { return _minimumTimeInterval; } set { _minimumTimeInterval = Mathf.Clamp(value, 0f, float.MaxValue); } }
        public bool RandomizeTimeInterval { get { return _randomizeTimeInterval; } set { _randomizeTimeInterval = value; } }
        public bool RandomizeRipplesSourcePositions
        {
            get { return _randomizeRipplesSourcePositions; }
            set
            {
                if (_randomizeRipplesSourcePositions != value)
                {
                    _randomizeRipplesSourcePositions = value;
                    if (!_randomizeRipplesSourcePositions)
                        RecomputeIndices();
                }
            }
        }
        public int RandomRipplesSourceCount { get { return _randomRipplesSourceCount; } set { _randomRipplesSourceCount = value; } }
        public List<float> SourcePositions
        {
            get { return _sourcePositions; }
            set
            {
                _sourcePositions = value;
                if (!_randomizeRipplesSourcePositions)
                    RecomputeIndices();
            }
        }
        public bool AllowDuplicateRipplesSourcePositions { get { return _allowDuplicateRipplesSourcePositions; } set { _allowDuplicateRipplesSourcePositions = value; } }
        public WaterRipplesSoundEffect SoundEffect { get { return _soundEffect; } }
        public WaterRipplesParticleEffect ParticleEffect { get { return _particleEffect; } }
        #endregion

        #region Methods

        internal void SetDependencies(WaterMainModule mainModule, WaterMeshModule meshModule, WaterSimulationModule simulationModule)
        {
            _mainModule = mainModule;
            _meshModule = meshModule;
            _simulationModule = simulationModule;
        }

        internal void Initialze()
        {
            _ripplesSourcesIndices = new List<int>();
            if (!_randomizeRipplesSourcePositions)
                RecomputeIndices();
        }

        internal void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            _soundEffect.Update();
            _particleEffect.Update();

            if (!_isActive || (!_updateWhenOffscreen && !_mainModule.IsWaterVisible))
                return;

            _elapsedTime += Time.deltaTime;
            
            if (_elapsedTime >= _currentInterval)
            {
                if (_randomizeRipplesSourcePositions)
                    RandomizeIndices();

                int surfaceVerticesCount = _meshModule.SurfaceVerticesCount;
                int startIndex = _simulationModule.IsUsingCustomBoundaries ? 1 : 0;
                int endIndex = _simulationModule.IsUsingCustomBoundaries ? surfaceVerticesCount - 2 : surfaceVerticesCount - 1;

                Vector3[] vertices = _meshModule.Vertices;
                float[] velocities = _simulationModule.Velocities;

                bool playSoundEffect = _soundEffect.IsActive;
                bool playParticleEffect = _particleEffect.IsActive;

                for (int i = 0, imax = _ripplesSourcesIndices.Count; i < imax; i++)
                {
                    int index = _ripplesSourcesIndices[i];

                    float disturbance = _randomizeDisturbance ? Random.Range(_minimumDisturbance, _maximumDisturbance) : _disturbance;

                    disturbance *= _simulationModule.StiffnessSquareRoot;

                    velocities[index] -= disturbance;

                    if (_smoothRipples)
                    {
                        float smoothedDisturbance = disturbance * _smoothingFactor;
                        int previousIndex, nextIndex;
                        previousIndex = index - 1;
                        nextIndex = index + 1;

                        if (previousIndex >= startIndex)
                            velocities[previousIndex] -= smoothedDisturbance;

                        if (nextIndex <= endIndex)
                            velocities[nextIndex] -= smoothedDisturbance;
                    }

                    if (_soundEffect.IsActive || _particleEffect.IsActive)
                    {
                        Vector3 spawnPosition = _mainModule.TransformLocalToWorld(vertices[index]);

                        if(playParticleEffect)
                            _particleEffect.PlayParticleEffect(spawnPosition);

                        if (playSoundEffect)
                        {
                            float disturbanceFactor = Mathf.InverseLerp(_minimumDisturbance, _maximumDisturbance, disturbance);
                            _soundEffect.PlaySoundEffect(spawnPosition, disturbanceFactor);
                        }
                    }
                }

                _currentInterval = _randomizeTimeInterval ? Random.Range(_minimumTimeInterval, _maximumTimeInterval) : _timeInterval;
                _elapsedTime = 0f;

                _simulationModule.MarkVelocitiesArrayAsChanged();
            }
        }

        private void RandomizeIndices()
        {
            _ripplesSourcesIndices.Clear();
            int surfaceVerticesCount = _meshModule.SurfaceVerticesCount;
            int startIndex = _simulationModule.IsUsingCustomBoundaries ? 1 : 0;
            int endIndex = _simulationModule.IsUsingCustomBoundaries ? surfaceVerticesCount - 1 : surfaceVerticesCount;

            for (int i = 0; i < _randomRipplesSourceCount; i++)
            {
                _ripplesSourcesIndices.Add(Random.Range(startIndex, endIndex));
            }
        }

        private void RecomputeIndices()
        {
            _ripplesSourcesIndices.Clear();
         
            if(_sourcePositions == null)
                return;

            float leftBoundary = _simulationModule.LeftBoundary;
            float rightBoundary = _simulationModule.RightBoundary;
            float activeSurface = rightBoundary - leftBoundary;
            int activeSurfaceVerticesCount = _meshModule.SurfaceVerticesCount - (_simulationModule.IsUsingCustomBoundaries ? 2 : 0);
            float xStep = activeSurface / (activeSurfaceVerticesCount - 1);
            int indexOffset = _simulationModule.IsUsingCustomBoundaries ? 1 : 0;

            for (int i = 0, maxi = _sourcePositions.Count; i < maxi; i++)
            {
                float xPosition = _sourcePositions[i];
                if (xPosition < leftBoundary || xPosition > rightBoundary)
                    continue;

                int nearestIndex = indexOffset + Mathf.RoundToInt((xPosition - leftBoundary) / xStep);

                if (!_allowDuplicateRipplesSourcePositions && _ripplesSourcesIndices.Contains(nearestIndex))
                    continue;

                _ripplesSourcesIndices.Add(nearestIndex);
            }
        }

        private static Transform CreateRipplesEffectsRoot(Transform parent)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return null;
#endif
            var root = new GameObject("ConstantRipplesEffects").transform;
            root.parent = parent;
            return root;
        }

        #endregion

        #region Editor Only Methods
        #if UNITY_EDITOR
        internal void Validate(WaterConstantRipplesModuleParameters parameters)
        {
            IsActive = parameters.IsActive;
            UpdateWhenOffscreen = parameters.UpdateWhenOffscreen;
            RandomizeDisturbance = parameters.RandomizeDisturbance;
            SmoothRipples = parameters.SmoothDisturbance;
            SmoothingFactor = parameters.SmoothFactor;
            Disturbance = parameters.Disturbance;
            MinimumDisturbance = parameters.MinimumDisturbance;
            MaximumDisturbance = parameters.MaximumDisturbance;
            RandomizeTimeInterval = parameters.RandomizeInterval;
            TimeInterval = parameters.Interval;
            MinimumTimeInterval = parameters.MinimumInterval;
            MaximumTimeInterval = parameters.MaximumInterval;
            RandomizeRipplesSourcePositions = parameters.RandomizeRipplesSourcesPositions;
            RandomRipplesSourceCount = parameters.RandomizeRipplesSourcesCount;
            AllowDuplicateRipplesSourcePositions = parameters.AllowDuplicateRipplesSourcesPositions;
            SourcePositions = parameters.SourcePositions;

            _particleEffect.Validate(parameters.ParticleEffectParameters);
            _soundEffect.Validate(parameters.SoundEffectParameters);

            if (Application.isPlaying)
                RecomputeIndices();
        }
        #endif
        #endregion
    }

    public struct WaterConstantRipplesModuleParameters
    {
        public bool IsActive;
        public bool UpdateWhenOffscreen;
        public bool RandomizeDisturbance;
        public bool SmoothDisturbance;
        public float SmoothFactor;
        public float Disturbance;
        public float MinimumDisturbance;
        public float MaximumDisturbance;
        public bool RandomizeInterval;
        public float Interval;
        public float MinimumInterval;
        public float MaximumInterval;
        public bool RandomizeRipplesSourcesPositions;
        public int RandomizeRipplesSourcesCount;
        public bool AllowDuplicateRipplesSourcesPositions;
        public List<float> SourcePositions;
        public WaterRipplesParticleEffectParameters ParticleEffectParameters;
        public WaterRipplesSoundEffectParameters SoundEffectParameters;
    }
}
