namespace Game2DWaterKit.AttachedComponents
{
    using Game2DWaterKit.Main;
    using UnityEngine;

    public class WaterAttachedComponentsModule
    {
        private readonly Vector2[] edgeColliderPoints = new Vector2[4];

        private WaterMainModule _mainModule;
        private BuoyancyEffector2D _buoyancyEffector;
        private BoxCollider2D _boxCollider;
        private EdgeCollider2D _edgeCollider;
        private bool _hasAnimatorAttached;
        private Vector2 _cachedWaterSize;
        private float _buoyancyEffectorSurfaceLevel;

        public WaterAttachedComponentsModule(float buoyancyEffectorSurfaceLevel)
        {
            _buoyancyEffectorSurfaceLevel = buoyancyEffectorSurfaceLevel;
        }

        #region Properties
        public float BuoyancyEffectorSurfaceLevel
        {
            get { return _buoyancyEffectorSurfaceLevel; }
            set
            {
                _buoyancyEffectorSurfaceLevel = Mathf.Clamp01(value);
                _buoyancyEffector.surfaceLevel = _cachedWaterSize.y * (0.5f - _buoyancyEffectorSurfaceLevel);
            }
        }
        internal bool HasAnimatorAttached { get { return _hasAnimatorAttached; } }
        #endregion

        #region Methods

        internal void SetDependencies(WaterMainModule mainModule)
        {
            _mainModule = mainModule;
        }

        internal void Initialize()
        {
            _buoyancyEffector = _mainModule.Transform.GetComponent<BuoyancyEffector2D>();
            _hasAnimatorAttached = _mainModule.Transform.GetComponent<Animator>() != null;
            _edgeCollider = _mainModule.Transform.GetComponent<EdgeCollider2D>();
            _boxCollider = _mainModule.Transform.GetComponent<BoxCollider2D>();
            _boxCollider.isTrigger = true;
            _boxCollider.usedByEffector = true; //used by the buoyancy effector

            _cachedWaterSize = _mainModule.WaterSize;
            ApplyChanges();
        }

        internal void Update()
        {
            if (_cachedWaterSize != _mainModule.WaterSize)
            {
                _cachedWaterSize = _mainModule.WaterSize;
                ApplyChanges();
            }
        }

        private void ApplyChanges()
        {
            _buoyancyEffector.surfaceLevel = _cachedWaterSize.y * (0.5f - _buoyancyEffectorSurfaceLevel);
            _boxCollider.size = _cachedWaterSize;
            if (_edgeCollider != null)
            {
                Vector2 halfSize = _cachedWaterSize * 0.5f;
                edgeColliderPoints[0].x = edgeColliderPoints[1].x = -halfSize.x;
                edgeColliderPoints[2].x = edgeColliderPoints[3].x = halfSize.x;

                edgeColliderPoints[0].y = edgeColliderPoints[3].y = halfSize.y;
                edgeColliderPoints[1].y = edgeColliderPoints[2].y = -halfSize.y;

                _edgeCollider.points = edgeColliderPoints;
            }
        }

        #endregion

        #region Editor Only Methods

        #if UNITY_EDITOR

        internal void Validate(float buoyancyEffectorSurfaceLevel)
        {
            _cachedWaterSize = _mainModule.WaterSize;
            BuoyancyEffectorSurfaceLevel = buoyancyEffectorSurfaceLevel;
            _edgeCollider = _mainModule.Transform.GetComponent<EdgeCollider2D>();
            _hasAnimatorAttached = _mainModule.Transform.GetComponent<Animator>() != null;
            ApplyChanges();
        }

        #endif

        #endregion
    }
}
