namespace Game2DWaterKit.Main
{
    using UnityEngine;

    public class WaterMainModule
    {
        #region Variables

        private Transform _transform;
        private Game2DWater _waterObject;

        private Vector2 _waterSize;

        private float _zRotation;
        private Vector3 _position;
        private Vector3 _upDirection;
        private Matrix4x4 _worldToLocalMatrix;
        private Matrix4x4 _localToWorldMatrix;

        #endregion

        public WaterMainModule(Game2DWater waterObject, Vector2 waterSize)
        {
            _waterObject = waterObject;
            _transform = waterObject.transform;
            _waterSize = waterSize;
        }

        #region Properties

        public Vector2 WaterSize { get { return _waterSize; } }
        public float Width { get { return _waterSize.x; } }
        public float Height { get { return _waterSize.y; } }
        public Vector3 Position { get { return _position; } set { _transform.position = value; } }
        public Matrix4x4 LocalToWorldMatrix { get { return _localToWorldMatrix; } }
        public Matrix4x4 WorldToLocalMatrix { get { return _worldToLocalMatrix; } }

        internal Transform Transform { get { return _transform; } }
        internal float ZRotation { get { return _zRotation; } }
        internal Vector3 UpDirection { get { return _upDirection; } }
        internal bool IsWaterVisible { get; set; }
        internal Game2DWater WaterObject { get { return _waterObject; } }
        internal LargeWaterAreaManager LargeWaterAreaManager { get; set; }

        #endregion

        #region Methods

        public void SetWaterSize(Vector2 newWaterSize, bool recomputeMesh = false)
        {
            if (newWaterSize.x > 0f && newWaterSize.y > 0f)
            {
                _waterSize = newWaterSize;
                if (recomputeMesh)
                    _waterObject.MeshModule.RecomputeMeshData();
            }
        }

        public Vector3 TransformLocalToWorld(Vector2 point)
        {
            float x = _localToWorldMatrix.m00 * point.x + _localToWorldMatrix.m01 * point.y + _localToWorldMatrix.m03;
            float y = _localToWorldMatrix.m10 * point.x + _localToWorldMatrix.m11 * point.y + _localToWorldMatrix.m13;
            return new Vector3(x, y, _position.z);
        }

        public Vector2 TransformWorldToLocal(Vector3 point)
        {
            float x = _worldToLocalMatrix.m00 * point.x + _worldToLocalMatrix.m01 * point.y + _worldToLocalMatrix.m02 * point.z + _worldToLocalMatrix.m03;
            float y = _worldToLocalMatrix.m10 * point.x + _worldToLocalMatrix.m11 * point.y + _worldToLocalMatrix.m12 * point.z + _worldToLocalMatrix.m13;
            return new Vector2(x, y);
        }

        internal void Initialize()
        {
            _transform.gameObject.layer = LayerMask.NameToLayer("Water");
#if UNITY_EDITOR
            //IsWaterVisible property is set in OnBecameVisible and OnBecameInvisible unity callbacks
            //which are not called in edit mode. So we'll assume that the water is always visible in edit mode
            if (!Application.isPlaying)
            {
                IsWaterVisible = true;
            }
#endif
            UpdateCachedTransformInformation();
        }

        internal void Update()
        {
            if (_transform.hasChanged)
            {
                _transform.hasChanged = false;
                UpdateCachedTransformInformation();
            }
        }

        private void UpdateCachedTransformInformation()
        {
            _localToWorldMatrix = _transform.localToWorldMatrix;
            _worldToLocalMatrix = _transform.worldToLocalMatrix;
            _position = _transform.position;
            _zRotation = _transform.rotation.eulerAngles.z;
            _upDirection = _transform.up;
        }

        #endregion

        #region Editor Only Methods

        #if UNITY_EDITOR
        internal void Validate(Vector2 waterSize)
        {
            if (waterSize != _waterSize)
                SetWaterSize(waterSize, true);
            UpdateCachedTransformInformation();
        }
        #endif

        #endregion
    }
}
