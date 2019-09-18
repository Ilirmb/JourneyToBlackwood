namespace Game2DWaterKit.Animation
{
    using Game2DWaterKit.Main;
    using Game2DWaterKit.Simulation;
    using Game2DWaterKit.Mesh;
    using UnityEngine;

    public class WaterAnimationModule
    {
        private WaterMainModule _mainModule;
        private WaterSimulationModule _simulationModule;
        private WaterMeshModule _meshModule;

        private bool _isAnimatingWaterSize;
        private Vector2 _animationTargetSize;
        private Vector2 _animationInitialSize;
        private WaterAnimationConstraint _animationConstraints;
        private WaterAnimationWrapMode _animationWrapMode;
        private float _animationDuration;
        private Vector2 _animationVelocity;

        private Vector2 _lastWaterSize;
        private float _lastLeftCustomBoundary;
        private float _lastRightCustomBoundary;

        #region Methods

        public void AnimateWaterSize(Vector2 targetSize, float duration, WaterAnimationConstraint constraint, WaterAnimationWrapMode wrapMode = WaterAnimationWrapMode.Once)
        {
            if (targetSize.x <= 0f)
                targetSize.x = 0.001f;
            if (targetSize.y <= 0f)
                targetSize.y = 0.001f;

            _animationTargetSize = targetSize;
            _animationInitialSize = _mainModule.WaterSize;
            _animationDuration = Mathf.Clamp(duration,0f,float.MaxValue);
            _animationConstraints = constraint;
            _animationWrapMode = wrapMode;
            _isAnimatingWaterSize = true;
        }

        internal void SetDependencies(WaterMainModule mainModule,WaterMeshModule meshModule,WaterSimulationModule simulationModule)
        {
            _mainModule = mainModule;
            _meshModule = meshModule;
            _simulationModule = simulationModule;
        }

        internal void Initialze()
        {
            _meshModule.OnRecomputeMesh += ResetCachedVariables;
            ResetCachedVariables();
        }

        internal void Update()
        {
            Vector2 waterSize = _mainModule.WaterSize;

            if (_isAnimatingWaterSize)
            {
                waterSize = Vector2.SmoothDamp(_lastWaterSize, _animationTargetSize, ref _animationVelocity, _animationDuration, Mathf.Infinity, Time.deltaTime);
                const float threshold = 0.005f;
                _isAnimatingWaterSize = (Mathf.Abs(waterSize.y - _animationTargetSize.y) > threshold) || (Mathf.Abs(waterSize.x - _animationTargetSize.x) > threshold);
                ApplyAnimationConstraints(waterSize);

                if (!_isAnimatingWaterSize)
                {
                    switch (_animationWrapMode)
                    {
                        case WaterAnimationWrapMode.Once:
                            break;
                        case WaterAnimationWrapMode.Loop:
                            _isAnimatingWaterSize = true;
                            waterSize = _animationInitialSize;
                            break;
                        case WaterAnimationWrapMode.PingPong:
                            _isAnimatingWaterSize = true;
                            _animationTargetSize = _animationInitialSize;
                            _animationInitialSize = waterSize;
                            break;
                    }

                    _animationVelocity = Vector2.zero;
                }
            }

            if (waterSize != _lastWaterSize)
                UpdateWaterSize(waterSize);

            if (_simulationModule.IsUsingCustomBoundaries && (_simulationModule.LeftCustomBoundary != _lastLeftCustomBoundary || _simulationModule.RightCustomBoundary != _lastRightCustomBoundary))
                UpdateMeshCustomBoundaries();
        }

        internal void SyncAnimatableVariables(Vector2 waterSize, float firstCustomBoundary, float secondCustomBoundary)
        {
            //These variables could be animated in Unity animation system,
            //so we make sure to reflect any changes to their values into their respective modules
            _mainModule.SetWaterSize(waterSize);
            _simulationModule.FirstCustomBoundary = firstCustomBoundary;
            _simulationModule.SecondCustomBoundary = secondCustomBoundary;
        }

        private void ApplyAnimationConstraints(Vector2 newSize)
        {
            float xFactor = 0f;
            float yFactor = 0f;

            bool hasBottomConstraint = HasConstraintDefined(WaterAnimationConstraint.Bottom);
            bool hasVerticalConstraint = hasBottomConstraint || HasConstraintDefined(WaterAnimationConstraint.Top);
            if (hasVerticalConstraint)
            {
                yFactor = hasBottomConstraint ? 1f : -1f;
            }
            
            bool hasLeftConstraint = HasConstraintDefined(WaterAnimationConstraint.Left);
            bool hasHorizontalConstraint = hasLeftConstraint || HasConstraintDefined(WaterAnimationConstraint.Right);
            if (hasHorizontalConstraint)
            {
                xFactor = hasLeftConstraint ? 1f : -1f;
            }

            // Calculating new water position = currentPosition ± deltaChange * 0.5f
            // we're working here in local space so the current water position is always equal to (0,0)
            // the newPosition = ± deltaChange * 0.5f
            // ± : the sign depends on the defined constraints
            float x = ((newSize.x - _lastWaterSize.x) * 0.5f) * xFactor;
            float y = ((newSize.y - _lastWaterSize.y) * 0.5f) * yFactor;

            _mainModule.Position = _mainModule.TransformLocalToWorld(new Vector2(x, y));
        }

        private bool HasConstraintDefined(WaterAnimationConstraint constraint)
        {
            return (_animationConstraints & constraint) == constraint;
        }

        private void UpdateWaterSize(Vector2 newWaterSize)
        {
            int surfaceVerticesCount = _meshModule.SurfaceVerticesCount;
            var vertices = _meshModule.Vertices;

            if (_simulationModule.IsUsingCustomBoundaries)
            {
                if(newWaterSize.x != _lastWaterSize.x)
                {
                    int firstSurfaceVertexIndex = 0; //topLeft
                    int lastSurfaceVertexIndex = surfaceVerticesCount - 1; //topRight

                    int firstBottomVertexIndex = surfaceVerticesCount; //bottomLeft
                    int lastBottomVertexIndex = surfaceVerticesCount * 2 - 1; //bottomRight

                    float halfWidth = newWaterSize.x * 0.5f;
                    vertices[firstSurfaceVertexIndex].x = vertices[firstBottomVertexIndex].x = -halfWidth;
                    vertices[lastSurfaceVertexIndex].x = vertices[lastBottomVertexIndex].x = halfWidth;
                }

                if(newWaterSize.y != _lastWaterSize.y)
                {
                    float halfDeltaHeight = (newWaterSize.y - _lastWaterSize.y) * 0.5f;
                    for (int surfaceVertexIndex = 0; surfaceVertexIndex < surfaceVerticesCount; surfaceVertexIndex++)
                    {
                        int bottomVertexIndex = surfaceVertexIndex + surfaceVerticesCount;

                        vertices[surfaceVertexIndex].y += halfDeltaHeight;
                        vertices[bottomVertexIndex].y -= halfDeltaHeight;
                    }
                }
            }
            else
            {
                float halfDeltaHeight = (newWaterSize.y - _lastWaterSize.y) * 0.5f;
                float columnWidth = newWaterSize.x / (surfaceVerticesCount - 1);

                float xPos = -newWaterSize.x * 0.5f;
                for (int surfaceVertexIndex = 0; surfaceVertexIndex < surfaceVerticesCount; surfaceVertexIndex++)
                {
                    int bottomVertexIndex = surfaceVertexIndex + surfaceVerticesCount;

                    vertices[surfaceVertexIndex].x = vertices[bottomVertexIndex].x = xPos;
                    vertices[surfaceVertexIndex].y += halfDeltaHeight;
                    vertices[bottomVertexIndex].y -= halfDeltaHeight;

                    xPos += columnWidth;
                }
            }

            _mainModule.SetWaterSize(newWaterSize);
            _meshModule.UpdateMeshData();

            _lastWaterSize = newWaterSize;
        }

        private void UpdateMeshCustomBoundaries()
        {
            float newLeftCustomBoundary = _simulationModule.LeftCustomBoundary;
            float newRightCustomBoundary = _simulationModule.RightCustomBoundary;

            int surfaceVerticesCount = _meshModule.SurfaceVerticesCount;
            var vertices = _meshModule.Vertices;
            float columnWidth = (newRightCustomBoundary - newLeftCustomBoundary) / (surfaceVerticesCount - 3);

            float xPos = newLeftCustomBoundary;
            for (int surfaceVertexIndex = 1, max = surfaceVerticesCount - 1; surfaceVertexIndex < max; surfaceVertexIndex++)
            {
                int bottomVertexIndex = surfaceVertexIndex + surfaceVerticesCount;
                vertices[surfaceVertexIndex].x = vertices[bottomVertexIndex].x = xPos;

                xPos += columnWidth;
            }

            _lastLeftCustomBoundary = newLeftCustomBoundary;
            _lastRightCustomBoundary = newRightCustomBoundary;
            _meshModule.UpdateMeshData();
        }

        private void ResetCachedVariables()
        {
            _lastWaterSize = _mainModule.WaterSize;
            _lastLeftCustomBoundary = _simulationModule.LeftCustomBoundary;
            _lastRightCustomBoundary = _simulationModule.RightCustomBoundary;
        }

        #endregion
    }

    [System.Flags]
    public enum WaterAnimationConstraint
    {
        None = 0,
        Top = 1 << 0,
        Bottom = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
        TopLeft = Top | Left,
        TopRight = Top | Right,
        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right
    }

    public enum WaterAnimationWrapMode
    {
        Once,
        Loop,
        PingPong
    }
}