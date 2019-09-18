namespace Game2DWaterKit.Ripples
{
    using Game2DWaterKit.Mesh;
    using Game2DWaterKit.Ripples.Effects;
    using Game2DWaterKit.Simulation;
    using Game2DWaterKit.Main;
    using UnityEngine;
    using UnityEngine.Events;

    public class WaterCollisionRipplesModule
    {
        #region Variables
        private readonly Transform _onCollisionRipplesEffectsPoolsRoot;
        private readonly WaterRipplesParticleEffect _onWaterEnterRipplesParticleEffect;
        private readonly WaterRipplesParticleEffect _onWaterExitRipplesParticleEffect;
        private readonly WaterRipplesSoundEffect _onWaterEnterRipplesSoundEffect;
        private readonly WaterRipplesSoundEffect _onWaterExitRipplesSoundEffect;

        private bool _isOnWaterEnterRipplesActive;
        private bool _isOnWaterExitRipplesActive;
        private float _minimumDisturbance;
        private float _maximumDisturbance;
        private float _velocityMultiplier;
        private LayerMask _collisionMask;
        private float _collisionMinimumDepth;
        private float _collisionMaximumDepth;
        private float _collisionRaycastMaximumDistance;
        private UnityEvent _onWaterEnter;
        private UnityEvent _onWaterExit;

        private WaterMeshModule _meshModule;
        private WaterMainModule _mainModule;
        private WaterSimulationModule _simulationModule;
        #endregion

        public WaterCollisionRipplesModule(WaterCollisionRipplesModuleParameters parameters, Transform ripplesEffectsPoolsRootParent)
        {
            _isOnWaterEnterRipplesActive = parameters.ActivateOnWaterEnterRipples;
            _isOnWaterExitRipplesActive = parameters.ActivateOnWaterExitRipples;
            _minimumDisturbance = parameters.MinimumDisturbance;
            _maximumDisturbance = parameters.MaximumDisturbance;
            _velocityMultiplier = parameters.VelocityMultiplier;
            _collisionMask = parameters.CollisionMask;
            _collisionMinimumDepth = parameters.CollisionMinimumDepth;
            _collisionMaximumDepth = parameters.CollisionMaximumDepth;
            _collisionRaycastMaximumDistance = parameters.CollisionRaycastMaxDistance;
            _onWaterEnter = parameters.OnWaterEnter;
            _onWaterExit = parameters.OnWaterExit;

            _onCollisionRipplesEffectsPoolsRoot = CreateRipplesEffectsPoolsRoot(ripplesEffectsPoolsRootParent);

            _onWaterEnterRipplesParticleEffect = new WaterRipplesParticleEffect(parameters.WaterEnterParticleEffectParameters, _onCollisionRipplesEffectsPoolsRoot);
            _onWaterExitRipplesParticleEffect = new WaterRipplesParticleEffect(parameters.WaterExitParticleEffectParameters, _onCollisionRipplesEffectsPoolsRoot);
            _onWaterEnterRipplesSoundEffect = new WaterRipplesSoundEffect(parameters.WaterEnterSoundEffectParameters, _onCollisionRipplesEffectsPoolsRoot);
            _onWaterExitRipplesSoundEffect = new WaterRipplesSoundEffect(parameters.WaterExitSoundEffectParameters, _onCollisionRipplesEffectsPoolsRoot);
        }

        #region Properties
        public bool IsOnWaterEnterRipplesActive { get { return _isOnWaterEnterRipplesActive; } set { _isOnWaterEnterRipplesActive = value; } }
        public bool IsOnWaterExitRipplesActive { get { return _isOnWaterExitRipplesActive; } set { _isOnWaterExitRipplesActive = value; } }
        public LayerMask CollisionMask { get { return _collisionMask; } set { _collisionMask = value; } }
        public float CollisionMaximumDepth { get { return _collisionMaximumDepth; } set { _collisionMaximumDepth = value; } }
        public float CollisionMinimumDepth { get { return _collisionMinimumDepth; } set { _collisionMinimumDepth = value; } }
        public float CollisionRaycastMaximumDistance { get { return _collisionRaycastMaximumDistance; } set { _collisionRaycastMaximumDistance = Mathf.Clamp(value, 0f, float.MaxValue); } }
        public float MaximumDisturbance { get { return _maximumDisturbance; } set { _maximumDisturbance = Mathf.Clamp(value, 0f, float.MaxValue); } }
        public float MinimumDisturbance { get { return _minimumDisturbance; } set { _minimumDisturbance = Mathf.Clamp(value, 0f, float.MaxValue); } }
        public UnityEvent OnWaterEnter { get { return _onWaterEnter; } set { _onWaterEnter = value; } }
        public UnityEvent OnWaterExit { get { return _onWaterExit; } set { _onWaterExit = value; } }
        public float VelocityMultiplier { get { return _velocityMultiplier; } set { _velocityMultiplier = Mathf.Clamp(value, 0f, float.MaxValue); } }
        public WaterRipplesParticleEffect OnWaterEnterRipplesParticleEffect { get { return _onWaterEnterRipplesParticleEffect; } }
        public WaterRipplesParticleEffect OnWaterExitRipplesParticleEffect { get { return _onWaterExitRipplesParticleEffect; } }
        public WaterRipplesSoundEffect OnWaterEnterRipplesSoundEffect { get { return _onWaterEnterRipplesSoundEffect; } }
        public WaterRipplesSoundEffect OnWaterExitRipplesSoundEffect { get { return _onWaterExitRipplesSoundEffect; } }
        #endregion

        #region Methods

        internal void SetDependencies(WaterMainModule mainModule, WaterMeshModule meshModule, WaterSimulationModule simulationModule)
        {
            _mainModule = mainModule;
            _meshModule = meshModule;
            _simulationModule = simulationModule;
        }

        internal void ResolveCollision(Collider2D objectCollider, bool isObjectEnteringWater)
        {
            if ((isObjectEnteringWater && !_isOnWaterEnterRipplesActive) || (!isObjectEnteringWater && !_isOnWaterExitRipplesActive))
                return;

            if (_collisionMask != (_collisionMask | (1 << objectCollider.gameObject.layer)))
                return;

            Vector3[] vertices = _meshModule.Vertices;
            float[] velocities = _simulationModule.Velocities;
            float stiffnessSquareRoot = _simulationModule.StiffnessSquareRoot;
            Vector3 raycastDirection = _mainModule.UpDirection;

            int surfaceVerticesCount = _meshModule.SurfaceVerticesCount;
            int subdivisionsPerUnit = _meshModule.SubdivisionsPerUnit;
            
            Bounds objectColliderBounds = objectCollider.bounds;
            float minColliderBoundsX = _mainModule.TransformWorldToLocal(objectColliderBounds.min).x;
            float maxColliderBoundsX = _mainModule.TransformWorldToLocal(objectColliderBounds.max).x;

            int firstSurfaceVertexIndex = _simulationModule.IsUsingCustomBoundaries ? 1 : 0;
            int lastSurfaceVertexIndex = _simulationModule.IsUsingCustomBoundaries ? surfaceVerticesCount - 2 : surfaceVerticesCount - 1;
            float firstSurfaceVertexPosition = vertices[firstSurfaceVertexIndex].x;
            int startIndex = Mathf.Clamp(Mathf.RoundToInt((minColliderBoundsX - firstSurfaceVertexPosition) * subdivisionsPerUnit), firstSurfaceVertexIndex, lastSurfaceVertexIndex);
            int endIndex = Mathf.Clamp(Mathf.RoundToInt((maxColliderBoundsX - firstSurfaceVertexPosition) * subdivisionsPerUnit), firstSurfaceVertexIndex, lastSurfaceVertexIndex);

            int hitPointsCount = 0;
            float hitPointsVelocitiesSum = 0f;
            Vector2 hitPointsPositionsSum = new Vector2(0f, 0f);

            for (int surfaceVertexIndex = startIndex; surfaceVertexIndex <= endIndex; surfaceVertexIndex++)
            {
                Vector2 surfaceVertexPosition = _mainModule.TransformLocalToWorld(vertices[surfaceVertexIndex]);
                RaycastHit2D hit = Physics2D.Raycast(surfaceVertexPosition, raycastDirection, _collisionRaycastMaximumDistance, _collisionMask, _collisionMinimumDepth, _collisionMaximumDepth);
                if (hit.collider == objectCollider && hit.rigidbody != null)
                {
                    float velocity = hit.rigidbody.GetPointVelocity(surfaceVertexPosition).y * _velocityMultiplier;
                    velocity = Mathf.Clamp(Mathf.Abs(velocity), _minimumDisturbance, _maximumDisturbance);
                    velocities[surfaceVertexIndex] += velocity * stiffnessSquareRoot * (isObjectEnteringWater ? -1f : 1f);
                    hitPointsVelocitiesSum += velocity;
                    hitPointsPositionsSum += hit.point;
                    hitPointsCount++;
                }
            }

            if (hitPointsCount > 0)
            {
                _simulationModule.MarkVelocitiesArrayAsChanged();

                Vector2 hitPointsPositionsMean = hitPointsPositionsSum / hitPointsCount;
                Vector3 spawnPosition = new Vector3(hitPointsPositionsMean.x, hitPointsPositionsMean.y, _mainModule.Position.z);

                float hitPointsVelocitiesMean = hitPointsVelocitiesSum / hitPointsCount;
                float disturbanceFactor = Mathf.InverseLerp(_minimumDisturbance, _maximumDisturbance, hitPointsVelocitiesMean);

                if (isObjectEnteringWater)
                {
                    OnWaterEnterRipplesParticleEffect.PlayParticleEffect(spawnPosition);
                    OnWaterEnterRipplesSoundEffect.PlaySoundEffect(spawnPosition, disturbanceFactor);

                    if (_onWaterEnter != null)
                        _onWaterEnter.Invoke();
                }
                else
                {
                    OnWaterExitRipplesParticleEffect.PlayParticleEffect(spawnPosition);
                    OnWaterExitRipplesSoundEffect.PlaySoundEffect(spawnPosition, disturbanceFactor);

                    if (_onWaterExit != null)
                        _onWaterExit.Invoke();
                }
            }
        }

        internal void Update()
        {
            _onWaterEnterRipplesSoundEffect.Update();
            _onWaterEnterRipplesParticleEffect.Update();
            _onWaterExitRipplesSoundEffect.Update();
            _onWaterExitRipplesParticleEffect.Update();
        }

        private static Transform CreateRipplesEffectsPoolsRoot(Transform parent)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return null;
#endif
            var root = new GameObject("OnCollisionRipplesEffects").transform;
            root.parent = parent;
            return root;
        }

        #endregion

        #region Editor Only Methods
        #if UNITY_EDITOR
        internal void Validate(WaterCollisionRipplesModuleParameters parameters)
        {
            IsOnWaterEnterRipplesActive = parameters.ActivateOnWaterEnterRipples;
            IsOnWaterExitRipplesActive = parameters.ActivateOnWaterExitRipples;
            MinimumDisturbance = parameters.MinimumDisturbance;
            MaximumDisturbance = parameters.MaximumDisturbance;
            VelocityMultiplier = parameters.VelocityMultiplier;
            CollisionMask = parameters.CollisionMask;
            CollisionMinimumDepth = parameters.CollisionMinimumDepth;
            CollisionMaximumDepth = parameters.CollisionMaximumDepth;
            CollisionRaycastMaximumDistance = parameters.CollisionRaycastMaxDistance;
            OnWaterEnter = parameters.OnWaterEnter;
            OnWaterExit = parameters.OnWaterExit;

            OnWaterEnterRipplesParticleEffect.Validate(parameters.WaterEnterParticleEffectParameters);
            OnWaterEnterRipplesSoundEffect.Validate(parameters.WaterEnterSoundEffectParameters);
            OnWaterExitRipplesParticleEffect.Validate(parameters.WaterExitParticleEffectParameters);
            OnWaterExitRipplesSoundEffect.Validate(parameters.WaterExitSoundEffectParameters);
        }
        #endif
        #endregion
    }

    public struct WaterCollisionRipplesModuleParameters
    {
        public bool ActivateOnWaterEnterRipples;
        public bool ActivateOnWaterExitRipples;
        public float MinimumDisturbance;
        public float MaximumDisturbance;
        public float VelocityMultiplier;
        public LayerMask CollisionMask;
        public float CollisionMinimumDepth;
        public float CollisionMaximumDepth;
        public float CollisionRaycastMaxDistance;
        public UnityEvent OnWaterEnter;
        public UnityEvent OnWaterExit;
        public WaterRipplesParticleEffectParameters WaterEnterParticleEffectParameters;
        public WaterRipplesSoundEffectParameters WaterEnterSoundEffectParameters;
        public WaterRipplesParticleEffectParameters WaterExitParticleEffectParameters;
        public WaterRipplesSoundEffectParameters WaterExitSoundEffectParameters;
    }

}
