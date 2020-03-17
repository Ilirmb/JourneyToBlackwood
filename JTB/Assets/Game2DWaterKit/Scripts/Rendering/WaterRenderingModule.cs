namespace Game2DWaterKit.Rendering
{
    using Game2DWaterKit.Material;
    using Game2DWaterKit.Mesh;
    using Game2DWaterKit.Utils;
    using Game2DWaterKit.Main;
    using UnityEngine;

    public class WaterRenderingModule
    {
        #region Variables

        private WaterMainModule _mainModule;
        private WaterMaterialModule _materialModule;
        private WaterMeshModule _meshModule;

        private bool _renderPixelLights;
        private float _farClipPlane;
        private bool _allowMSAA;
        private bool _allowHDR;
        private int _sortingLayerID;
        private int _sortingOrder;

        private WaterRenderingVisibleArea _fullWaterVisibleArea;
        private WaterRenderingVisibleArea _surfaceVisibleArea;
        private WaterRenderingVisibleArea _surfaceBelowSubmergeLevelVisibleArea;

        private WaterRenderingMode _refraction;
        private WaterRenderingMode _refractionPartiallySubmergedObjects;
        private WaterRenderingMode _reflection;
        private WaterRenderingMode _reflectionPartiallySubmergedObjects;

        private SimpleFixedSizedList<Vector2> _clipeePoints;
        private WaterRenderingCameraFrustum _renderingCameraFrustum = null;
        private Color _transparentBackgroundColor = new Color(0f, 0f, 0f, 0f);
        #endregion

        public WaterRenderingModule(WaterRenderingModuleParameters parameters, Transform renderingCamerasRoot)
        {
            _renderPixelLights = parameters.RenderPixelLights;
            _farClipPlane = parameters.FarClipPlane;
            _allowHDR = parameters.AllowHDR;
            _allowMSAA = parameters.AllowMSAA;
            _sortingLayerID = parameters.SortingLayerID;
            _sortingOrder = parameters.SortingOrder;

            _refraction = new WaterRenderingMode(this, parameters.RefractionParameters, renderingCamerasRoot, isReflectionMode: false);
            _refractionPartiallySubmergedObjects = new WaterRenderingMode(this, parameters.RefractionPartiallySubmergedObjectsParameters, renderingCamerasRoot, isReflectionMode: false);
            _reflection = new WaterRenderingMode(this, parameters.ReflectionParameters, renderingCamerasRoot, isReflectionMode: true);
            _reflectionPartiallySubmergedObjects = new WaterRenderingMode(this, parameters.ReflectionPartiallySubmergedObjectsParameters, renderingCamerasRoot, isReflectionMode: true);
        }

        #region Properties
        public WaterRenderingMode Refraction { get { return _refraction; } }
        public WaterRenderingMode RefractionPartiallySubmergedObjects { get { return _refractionPartiallySubmergedObjects; } }
        public WaterRenderingMode Reflection { get { return _reflection; } }
        public WaterRenderingMode ReflectionPartiallySubmergedObjects { get { return _reflectionPartiallySubmergedObjects; } }
        public bool AllowHDR { get { return _allowHDR; } set { _allowHDR = value; } }
        public bool AllowMSAA { get { return _allowMSAA; } set { _allowMSAA = value; } }
        public float FarClipPlane { get { return _farClipPlane; } set { _farClipPlane = Mathf.Clamp(value,0.001f,float.MaxValue); } }
        public bool RenderPixelLights { get { return _renderPixelLights; } set { _renderPixelLights = value; } }
        public int SortingLayerID
        {
            get { return _sortingLayerID; }
            set {
                _sortingLayerID = value;
                if (_meshModule != null)
                    _meshModule.MeshRenderer.sortingLayerID = _sortingLayerID;
            }
        }
        public int SortingOrder
        {
            get { return _sortingOrder; }
            set {
                _sortingOrder = value;
                if (_meshModule != null)
                    _meshModule.MeshRenderer.sortingOrder = _sortingOrder;
            }
        }
        #endregion

        #region Methods

        internal void SetDependencies(WaterMainModule mainModule, WaterMeshModule meshModule, WaterMaterialModule materialModule)
        {
            _mainModule = mainModule;
            _meshModule = meshModule;
            _materialModule = materialModule;
        }

        internal void Initialize()
        {
            _meshModule.MeshRenderer.sortingOrder = _sortingOrder;
            _meshModule.MeshRenderer.sortingLayerID = _sortingLayerID;

            _fullWaterVisibleArea = new WaterRenderingVisibleArea(_mainModule);
            _surfaceVisibleArea = new WaterRenderingVisibleArea(_mainModule);
            _surfaceBelowSubmergeLevelVisibleArea = new WaterRenderingVisibleArea(_mainModule);

            _renderingCameraFrustum = new WaterRenderingCameraFrustum(_mainModule);

            _clipeePoints = new SimpleFixedSizedList<Vector2>(8);
        }
        
        internal void Render(Camera currentRenderingCamera)
        {
            bool renderRefraction = _materialModule.IsRefractionEnabled;
            bool renderReflection = _materialModule.IsReflectionEnabled;

            bool isValidWaterSize = _mainModule.Width > 0f && _mainModule.Height > 0f;

            if (!currentRenderingCamera || !isValidWaterSize || !(renderReflection || renderRefraction))
                return;

            var largeWaterAreaManager = _mainModule.LargeWaterAreaManager;
            bool isConnectedToLargeWaterArea = largeWaterAreaManager != null;

            if (isConnectedToLargeWaterArea && largeWaterAreaManager.HasAlreadyRenderedCurrentFrame(currentRenderingCamera))
            {
                GetCurrentFrameRenderInformationFromLargeWaterAreaManager(largeWaterAreaManager, renderRefraction, renderReflection);
                return;
            }

            _renderingCameraFrustum.UpdateFrustum(currentRenderingCamera);

            ComputeVisibleAreas(currentRenderingCamera, !isConnectedToLargeWaterArea ? _meshModule.Bounds : largeWaterAreaManager.GetWaterObjectsBoundsRelativeToSpecifiedWaterObject(_mainModule));

            int pixelLightCount = QualitySettings.pixelLightCount;
            if (!_renderPixelLights)
                QualitySettings.pixelLightCount = 0;

            Color backgroundColor = currentRenderingCamera.backgroundColor;
            backgroundColor.a = 0f;

            float pixelsPerUnit = currentRenderingCamera.pixelHeight / _renderingCameraFrustum.WorldSpace.Height;

            if (!_materialModule.IsFakePerspectiveEnabled)
            {
                if (renderRefraction)
                {
                    _refraction.Render(_fullWaterVisibleArea, backgroundColor, pixelsPerUnit);
                    _materialModule.SetRefractionRenderTexture(_refraction.RenderTexture);
                }

                if (renderReflection)
                {
                    _reflection.Render(_fullWaterVisibleArea, backgroundColor, pixelsPerUnit);
                    _materialModule.SetReflectionRenderTexture(_reflection.RenderTexture);
                }
            }
            else
            {
                if (renderRefraction)
                {
                    _refraction.Render(_fullWaterVisibleArea, backgroundColor, pixelsPerUnit,_refractionPartiallySubmergedObjects.CullingMask);
                    _materialModule.SetRefractionRenderTexture(_refraction.RenderTexture);

                    _refractionPartiallySubmergedObjects.Render(_fullWaterVisibleArea, _transparentBackgroundColor, pixelsPerUnit);
                    _materialModule.SetRefractionPartiallySubmergedObjectsRenderTexture(_refractionPartiallySubmergedObjects.RenderTexture);
                }
                if (renderReflection)
                {
                    _reflection.Render(_surfaceVisibleArea, backgroundColor, pixelsPerUnit,_reflectionPartiallySubmergedObjects.CullingMask);
                    _materialModule.SetReflectionRenderTexture(_reflection.RenderTexture);
                    
                    _reflectionPartiallySubmergedObjects.Render(_surfaceBelowSubmergeLevelVisibleArea, _transparentBackgroundColor, pixelsPerUnit);
                    _materialModule.SetReflectionPartiallySubmergedObjectsRenderTexture(_reflectionPartiallySubmergedObjects.RenderTexture);
                }
            }

            QualitySettings.pixelLightCount = pixelLightCount;

            Matrix4x4 worldToVisibleAreaMatrix = Matrix4x4.Inverse(Matrix4x4.TRS(_fullWaterVisibleArea.Position, _fullWaterVisibleArea.Rotation, Vector3.one));
            _materialModule.SetWaterMatrix(_fullWaterVisibleArea.ProjectionMatrix * worldToVisibleAreaMatrix * _mainModule.LocalToWorldMatrix);
            _materialModule.SetReflectionLowerLimit(_mainModule.Height * 0.5f);
            _materialModule.ValidateMaterialPropertyBlock();

            if (isConnectedToLargeWaterArea)
                SetCurrentFrameRenderInformationToLargeWaterAreaManager(largeWaterAreaManager, renderRefraction, renderReflection, _fullWaterVisibleArea.ProjectionMatrix * worldToVisibleAreaMatrix,currentRenderingCamera);
        }

        private void ComputeVisibleAreas(Camera currentRenderingCamera, Bounds waterBounds)
        {
            bool isReflectionEnabled = _materialModule.IsReflectionEnabled;

            _clipeePoints.Clear();
            _clipeePoints.Add(_renderingCameraFrustum.WaterLocalSpace.TopLeft);
            _clipeePoints.Add(_renderingCameraFrustum.WaterLocalSpace.TopRight);
            _clipeePoints.Add(_renderingCameraFrustum.WaterLocalSpace.BottomRight);
            _clipeePoints.Add(_renderingCameraFrustum.WaterLocalSpace.BottomLeft);

            bool isRenderingCameraFullyContainedInWaterBox = true;

            //Fidning visible Water Area

            Vector2 waterBoundsMin = waterBounds.min;
            Vector2 waterBoundsMax = waterBounds.max;

            //Clip camera frustrum against water box edges to find the visible water area
            //top edge
            isRenderingCameraFullyContainedInWaterBox &= WaterUtility.ClipPointsAgainstEdge(_clipeePoints, isHorizontalEdge: true, keepInside: false, edgeValue: waterBoundsMax.y);
            //right edge
            isRenderingCameraFullyContainedInWaterBox &= WaterUtility.ClipPointsAgainstEdge(_clipeePoints, isHorizontalEdge: false, keepInside: false, edgeValue: waterBoundsMax.x);
            //bottom edge
            isRenderingCameraFullyContainedInWaterBox &= WaterUtility.ClipPointsAgainstEdge(_clipeePoints, isHorizontalEdge: true, keepInside: true, edgeValue: waterBoundsMin.y);
            //left edge
            isRenderingCameraFullyContainedInWaterBox &= WaterUtility.ClipPointsAgainstEdge(_clipeePoints, isHorizontalEdge: false, keepInside: true, edgeValue: waterBoundsMin.x);

            _fullWaterVisibleArea.UpdateArea(_clipeePoints, _renderingCameraFrustum, isRenderingCameraFullyContainedInWaterBox, _farClipPlane, isReflectionEnabled, reflectionAxis: -waterBoundsMin.y);

            if (_materialModule.IsFakePerspectiveEnabled)
            {
                float waterBoxHeight = waterBoundsMax.y - waterBoundsMin.y;

                //Finding visible Surface Area
                float surfaceLevel = waterBoundsMin.y + waterBoxHeight * _materialModule.SurfaceLevel;
                isRenderingCameraFullyContainedInWaterBox &= WaterUtility.ClipPointsAgainstEdge(_clipeePoints, isHorizontalEdge: true, keepInside: true, edgeValue: surfaceLevel);
                _surfaceVisibleArea.UpdateArea(_clipeePoints, _renderingCameraFrustum, isRenderingCameraFullyContainedInWaterBox, _farClipPlane, isReflectionEnabled, reflectionAxis: waterBoundsMax.y, viewingFrustrumHeightScalingFactor: _reflection.ViewingFrustumHeightScalingFactor);

                //Finding visible Surface Area below submerge level
                float submergeLevel = waterBoundsMin.y + waterBoxHeight * _materialModule.SurfaceSubmergeLevel;
                isRenderingCameraFullyContainedInWaterBox &= WaterUtility.ClipPointsAgainstEdge(_clipeePoints, isHorizontalEdge: true, keepInside: false, edgeValue: submergeLevel);
                _surfaceBelowSubmergeLevelVisibleArea.UpdateArea(_clipeePoints, _renderingCameraFrustum, isRenderingCameraFullyContainedInWaterBox, _farClipPlane, isReflectionEnabled, reflectionAxis: submergeLevel, viewingFrustrumHeightScalingFactor: _reflectionPartiallySubmergedObjects.ViewingFrustumHeightScalingFactor);
            }
        }

        private void GetCurrentFrameRenderInformationFromLargeWaterAreaManager(LargeWaterAreaManager largeWaterAreaManager, bool renderRefraction,bool renderReflection)
        {
            if (!_materialModule.IsFakePerspectiveEnabled)
            {
                if (renderRefraction)
                    _materialModule.SetRefractionRenderTexture(largeWaterAreaManager.RefractionRenderTexture);

                if (renderReflection)
                    _materialModule.SetReflectionRenderTexture(largeWaterAreaManager.ReflectionRenderTexture);
            }
            else
            {
                if (renderRefraction)
                {
                    _materialModule.SetRefractionRenderTexture(largeWaterAreaManager.RefractionRenderTexture);
                    _materialModule.SetRefractionPartiallySubmergedObjectsRenderTexture(largeWaterAreaManager.RefractionPartiallySubmergedObjectsRenderTexture);
                }
                if (renderReflection)
                {
                    _materialModule.SetReflectionRenderTexture(largeWaterAreaManager.ReflectionRenderTexture);
                    _materialModule.SetReflectionPartiallySubmergedObjectsRenderTexture(largeWaterAreaManager.ReflectionPartiallySubmergedObjectsRenderTexture);
                }
            }

            _materialModule.SetWaterMatrix(largeWaterAreaManager.ProjectionMatrix * _mainModule.LocalToWorldMatrix);
            _materialModule.SetReflectionLowerLimit(_mainModule.Height * 0.5f);
            _materialModule.ValidateMaterialPropertyBlock();
        }

        private void SetCurrentFrameRenderInformationToLargeWaterAreaManager(LargeWaterAreaManager largeWaterAreaManager, bool renderRefraction, bool renderReflection, Matrix4x4 projectionMatrix,Camera currentRenderingCamera)
        {
            if (!_materialModule.IsFakePerspectiveEnabled)
            {
                if (renderRefraction)
                    largeWaterAreaManager.RefractionRenderTexture = _refraction.RenderTexture;

                if (renderReflection)
                    largeWaterAreaManager.ReflectionRenderTexture = _reflection.RenderTexture;
            }
            else
            {
                if (renderRefraction)
                {
                    largeWaterAreaManager.RefractionRenderTexture = _refraction.RenderTexture;
                    largeWaterAreaManager.RefractionPartiallySubmergedObjectsRenderTexture = _refractionPartiallySubmergedObjects.RenderTexture;
                }

                if (renderReflection)
                {
                    largeWaterAreaManager.ReflectionRenderTexture = _reflection.RenderTexture;
                    largeWaterAreaManager.ReflectionPartiallySubmergedObjectsRenderTexture = _reflectionPartiallySubmergedObjects.RenderTexture;
                }
            }

            largeWaterAreaManager.ProjectionMatrix = projectionMatrix;
            largeWaterAreaManager.MarkCurrentFrameAsRendered(currentRenderingCamera);
        }

        #endregion

        #region Editor Only Methods

        #if UNITY_EDITOR

        internal void Validate(WaterRenderingModuleParameters parameters)
        {
            _refraction.Validate(parameters.RefractionParameters);
            _refractionPartiallySubmergedObjects.Validate(parameters.RefractionPartiallySubmergedObjectsParameters);
            _reflection.Validate(parameters.ReflectionParameters);
            _reflectionPartiallySubmergedObjects.Validate(parameters.ReflectionPartiallySubmergedObjectsParameters);

            RenderPixelLights = parameters.RenderPixelLights;
            FarClipPlane = parameters.FarClipPlane;
            AllowMSAA = parameters.AllowMSAA;
            AllowHDR = parameters.AllowHDR;
            SortingLayerID = parameters.SortingLayerID;
            SortingOrder = parameters.SortingOrder;
        }

        #endif

        #endregion
    }

    public struct WaterRenderingModuleParameters
    {
        public WaterRenderingModeParameters RefractionParameters;
        public WaterRenderingModeParameters RefractionPartiallySubmergedObjectsParameters;
        public WaterRenderingModeParameters ReflectionParameters;
        public WaterRenderingModeParameters ReflectionPartiallySubmergedObjectsParameters;
        public float FarClipPlane;
        public bool RenderPixelLights;
        public bool AllowMSAA;
        public bool AllowHDR;
        public int SortingLayerID;
        public int SortingOrder;
    }
}
