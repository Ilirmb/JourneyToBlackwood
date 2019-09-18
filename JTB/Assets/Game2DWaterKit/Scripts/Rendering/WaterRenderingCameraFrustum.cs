namespace Game2DWaterKit.Rendering
{
    using UnityEngine;
    using Game2DWaterKit.Main;

    internal class WaterRenderingCameraFrustum
    {
        private readonly FrustumProperties _waterLocalSpace;
        private readonly FrustumProperties _worldSpace;

        private WaterMainModule _mainModule;

        internal WaterRenderingCameraFrustum(WaterMainModule mainModule)
        {
            _mainModule = mainModule;
            _waterLocalSpace = new FrustumProperties();
            _worldSpace = new FrustumProperties();
        }

        #region Properties
        internal FrustumProperties WaterLocalSpace { get { return _waterLocalSpace; } }
        internal FrustumProperties WorldSpace { get { return _worldSpace; } }
        #endregion

        internal void UpdateFrustum(Camera camera)
        {
            //in world space
            
            Vector2 frustrumTopLeft = camera.ViewportToWorldPoint(new Vector3(0f, 1f, 0f));
            Vector2 frustrumTopRight = camera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
            Vector2 frustrumBottomLeft = camera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
            Vector2 frustrumBottomRight = camera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f));
            Vector3 position = camera.transform.position;
            float zRotation = camera.transform.rotation.eulerAngles.z;

            _worldSpace.Set(frustrumTopLeft, frustrumTopRight, frustrumBottomLeft, frustrumBottomRight, position, zRotation);

            //in water local space

            frustrumTopLeft = _mainModule.TransformWorldToLocal(frustrumTopLeft);
            frustrumTopRight = _mainModule.TransformWorldToLocal(frustrumTopRight);
            frustrumBottomLeft = _mainModule.TransformWorldToLocal(frustrumBottomLeft);
            frustrumBottomRight = _mainModule.TransformWorldToLocal(frustrumBottomRight);
            position = _mainModule.TransformWorldToLocal(position);
            zRotation = zRotation - _mainModule.ZRotation;

            _waterLocalSpace.Set(frustrumTopLeft, frustrumTopRight, frustrumBottomLeft, frustrumBottomRight, position, zRotation);
        }

        internal class FrustumProperties
        {
            #region Variables
            private Vector2 _topLeft;
            private Vector2 _topRight;
            private Vector2 _bottomLeft;
            private Vector2 _bottomRight;
            private Vector3 _position;
            private float _zRotation;
            private float _width;
            private float _height;
            #endregion

            #region Properties
            internal Vector2 TopLeft { get { return _topLeft; } }
            internal Vector2 TopRight { get { return _topRight; } }
            internal Vector2 BottomLeft { get { return _bottomLeft; } }
            internal Vector2 BottomRight { get { return _bottomRight; } }
            internal Vector3 Position { get { return _position; } }
            internal float ZRotation { get { return _zRotation; } }
            internal float Height { get { return _height; } }
            internal float Width { get { return _width; } }
            internal float Area { get { return _width * _height; } }
            #endregion

            internal void Set(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight, Vector3 position, float zRotation)
            {
                _topLeft = topLeft;
                _topRight = topRight;
                _bottomLeft = bottomLeft;
                _bottomRight = bottomRight;
                _position = position;
                _zRotation = zRotation;

                _width = _zRotation != 0f ? Vector2.Distance(_topLeft, _topRight) : _topRight.x - _topLeft.x;
                _height = _zRotation != 0f ? Vector2.Distance(_topRight, _bottomRight) : _topRight.y - _bottomRight.y;
            }
        }
    }
}
