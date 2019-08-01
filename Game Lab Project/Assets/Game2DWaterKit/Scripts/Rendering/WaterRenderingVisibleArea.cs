namespace Game2DWaterKit.Rendering
{
    using Game2DWaterKit.Utils;
    using Game2DWaterKit.Main;
    using UnityEngine;

    internal class WaterRenderingVisibleArea
    {
        private readonly WaterMainModule _mainModule;

        private bool _isValid = false;
        private float _width;
        private float _height;
        private Vector3 _position;
        private Vector3 _positionReflection;
        private Quaternion _rotation;
        private Quaternion _rotationReflection;
        private Matrix4x4 _projectionMatrix;

        public WaterRenderingVisibleArea(WaterMainModule transformManager)
        {
            _mainModule = transformManager;
        }

        #region Properties
        internal bool IsValid { get { return _isValid; } }
        internal float Width { get { return _width; } }
        internal float Height { get { return _height; } }
        internal Vector3 Position { get { return _position; } }
        internal Vector3 PositionReflection { get { return _positionReflection; } }
        internal Quaternion Rotation { get { return _rotation; } }
        internal Quaternion RotationReflection { get { return _rotationReflection; } }
        internal Matrix4x4 ProjectionMatrix { get { return _projectionMatrix; } }
        #endregion

        #region Methods

        internal void UpdateArea(SimpleFixedSizedList<Vector2> points, WaterRenderingCameraFrustum cameraFrustrum, bool isFullyContainedInWaterBox, float zFar, bool isReflectionEnabled, float reflectionAxis,float viewingFrustrumHeightScalingFactor = 1f)
        {
            _isValid = true;

            if (isFullyContainedInWaterBox)
            {
                MatchVisibleAreaToCameraFrustum(cameraFrustrum, zFar,isReflectionEnabled, reflectionAxis,viewingFrustrumHeightScalingFactor);
                return;
            }

            _isValid = points.Count > 0;
            if (!_isValid)
                return;

            // Compute the AABB of provided points
            Vector2 boundingBoxMin = points[0];
            Vector2 boundingBoxMax = points[0];
            for (int i = 1, imax = points.Count; i < imax; i++)
            {
                Vector2 point = points[i];

                if (point.x < boundingBoxMin.x)
                    boundingBoxMin.x = point.x;

                if (point.x > boundingBoxMax.x)
                    boundingBoxMax.x = point.x;

                if (point.y < boundingBoxMin.y)
                    boundingBoxMin.y = point.y;

                if (point.y > boundingBoxMax.y)
                    boundingBoxMax.y = point.y;
            }

            float boundingBoxArea = (boundingBoxMax.x - boundingBoxMin.x) * (boundingBoxMax.y - boundingBoxMin.y);

            _isValid = boundingBoxArea > 0f;
            if (!_isValid)
                return;

            if(boundingBoxArea > cameraFrustrum.WaterLocalSpace.Area)
            {
                MatchVisibleAreaToCameraFrustum(cameraFrustrum, zFar,isReflectionEnabled, reflectionAxis,viewingFrustrumHeightScalingFactor);
                return;
            }

            Vector2 boundsCenter = (boundingBoxMin + boundingBoxMax) * 0.5f;

            _position = _mainModule.TransformLocalToWorld(boundsCenter);
            _rotation = Quaternion.Euler(0f, 0f, _mainModule.ZRotation);

            if (isReflectionEnabled)
            {
                _positionReflection = _mainModule.TransformLocalToWorld(new Vector3(boundsCenter.x, 2f * reflectionAxis - boundsCenter.y));
                _rotationReflection = _rotation;
            }

            if(_mainModule.ZRotation != 0f)
            {
                Vector2 topLeft = _mainModule.TransformLocalToWorld(new Vector2(boundingBoxMin.x, boundingBoxMax.y));
                Vector2 bottomLeft = _mainModule.TransformLocalToWorld(boundingBoxMin);
                Vector2 topRight = _mainModule.TransformLocalToWorld(boundingBoxMax);
                _width = Vector2.Distance(topLeft, topRight);
                _height = Vector2.Distance(bottomLeft, topLeft);
            }
            else
            {
                boundingBoxMin = _mainModule.TransformLocalToWorld(boundingBoxMin);
                boundingBoxMax = _mainModule.TransformLocalToWorld(boundingBoxMax);
                _width = boundingBoxMax.x - boundingBoxMin.x;
                _height = boundingBoxMax.y - boundingBoxMin.y;
            }

            float halfWidth = _width * 0.5f;
            float halfHeight = _height * 0.5f;
            _projectionMatrix = Matrix4x4.Ortho(-halfWidth, halfWidth, -halfHeight, halfHeight * viewingFrustrumHeightScalingFactor, 0f, zFar);
        }
        
        private void MatchVisibleAreaToCameraFrustum(WaterRenderingCameraFrustum cameraFrustum, float zFar, bool isReflectionEnabled, float reflectionAxis,float heightScaleFactor)
        {
            _position = cameraFrustum.WorldSpace.Position;
            _position.z = _mainModule.Position.z;
            _rotation = Quaternion.Euler(0f,0f,cameraFrustum.WorldSpace.ZRotation);

            if (isReflectionEnabled)
            {
                Vector2 cameraPositionInLocalSpace = cameraFrustum.WaterLocalSpace.Position;
                _positionReflection = _mainModule.TransformLocalToWorld(new Vector3(cameraPositionInLocalSpace.x, 2f * reflectionAxis - cameraPositionInLocalSpace.y));
                _rotationReflection = Quaternion.Euler(0f,0f, -cameraFrustum.WaterLocalSpace.ZRotation + _mainModule.ZRotation);
            }
            
            _width = cameraFrustum.WorldSpace.Width;
            _height = cameraFrustum.WorldSpace.Height;

            float halfWidth = _width * 0.5f;
            float halfHeight = _height * 0.5f;
            _projectionMatrix = Matrix4x4.Ortho(-halfWidth, halfWidth, -halfHeight, halfHeight * heightScaleFactor, 0f, zFar);
        }
        
        #endregion
    }
}
