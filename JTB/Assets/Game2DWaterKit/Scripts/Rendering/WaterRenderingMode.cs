namespace Game2DWaterKit.Rendering
{
    using UnityEngine;

    public class WaterRenderingMode
    {
        private readonly Transform _cameraParent;
        private readonly bool _isReflectionMode;

        private WaterRenderingModule _renderingModule;

        private float _renderTextureResizingFactor;
        private float _viewingFrustumHeightScalingFactor;
        private float _zOffset;
        private LayerMask _cullingMask;
        private FilterMode _renderTextureFilterMode;

        private Camera _camera;
        private RenderTexture _renderTexture;

        public WaterRenderingMode(WaterRenderingModule renderingModule, WaterRenderingModeParameters parameters, Transform renderingModeCameraParent, bool isReflectionMode)
        {
            _renderingModule = renderingModule;

            _renderTextureResizingFactor = parameters.TextuerResizingFactor;
            _viewingFrustumHeightScalingFactor = parameters.ViewingFrustumHeightScalingFactor;
            _cullingMask = parameters.CullingMask;
            _renderTextureFilterMode = parameters.FilterMode;
            _zOffset = parameters.ZOffset;

            _cameraParent = renderingModeCameraParent;
            _isReflectionMode = isReflectionMode;
        }

        #region Properties
        public float ViewingFrustumHeightScalingFactor { get { return _viewingFrustumHeightScalingFactor; } set { _viewingFrustumHeightScalingFactor = Mathf.Clamp(value,0f,float.MaxValue); } }
        public float RenderTextureResizingFactor { get { return _renderTextureResizingFactor; } set { _renderTextureResizingFactor = Mathf.Clamp01(value); } }
        public float ZOffset { get { return _zOffset; } set { _zOffset = value; } }
        public LayerMask CullingMask { get { return _cullingMask; } set { _cullingMask = value & ~(1 << 4); } }
        public FilterMode RenderTextureFilterMode
        {
            get { return _renderTextureFilterMode; }
            set
            {
                _renderTextureFilterMode = value;
                if (_renderTexture != null)
                {
                    RenderTexture.ReleaseTemporary(_renderTexture);
                    _renderTexture = null;
                    //We'll get a new renderTexture in the next call to GetRenderTexture()
                }
            }
        }

        internal RenderTexture RenderTexture { get { return _renderTexture; } }
        #endregion

        #region Methods

        internal void Render(WaterRenderingVisibleArea visibleArea, Color backgroundColor, float pixelsPerUnit, int extraLayersToIgnoreMask = 0)
        {
            if (!visibleArea.IsValid)
                return;

            int textureWidth = Mathf.RoundToInt(visibleArea.Width * _renderTextureResizingFactor * pixelsPerUnit);
            int textureHeight = Mathf.RoundToInt(visibleArea.Height * _renderTextureResizingFactor * pixelsPerUnit);
            if (textureWidth < 1 || textureHeight < 1)
                return;

            Quaternion cameraRotation = _isReflectionMode ? visibleArea.RotationReflection : visibleArea.Rotation;
            Vector3 cameraPosition = _isReflectionMode ? visibleArea.PositionReflection : visibleArea.Position;
            cameraPosition.z += _zOffset;

            Camera camera = GetCamera();

            camera.targetTexture = GetRenderTexture(textureWidth, textureHeight);
            camera.transform.SetPositionAndRotation(cameraPosition, cameraRotation);
            camera.projectionMatrix = visibleArea.ProjectionMatrix;
            camera.cullingMask = _cullingMask & (~extraLayersToIgnoreMask);
            camera.backgroundColor = backgroundColor;
            camera.farClipPlane = _renderingModule.FarClipPlane;
            camera.allowHDR = _renderingModule.AllowHDR;
            camera.allowMSAA = _renderingModule.AllowMSAA;

            camera.Render();
        }

        private RenderTexture GetRenderTexture(int width, int height)
        {
            if (_renderTexture == null)
            {
                _renderTexture = GetTemporaryRenderTexture(width, height, _renderTextureFilterMode);
                return _renderTexture;
            }

            //get a new temporary render texture for any change in texture size larger than this threshold
            const int changeInTextureSizeMinimumThreshold = 5; //5 pixels (You could vary this parameter to your liking)
            bool getNewTexture = ((Mathf.Abs(_renderTexture.height - height) > changeInTextureSizeMinimumThreshold) || (Mathf.Abs(_renderTexture.width - width) > changeInTextureSizeMinimumThreshold));

            if (getNewTexture)
            {
                RenderTexture.ReleaseTemporary(_renderTexture);
                _renderTexture = GetTemporaryRenderTexture(width, height, _renderTextureFilterMode);
            }

            return _renderTexture;
        }

        private Camera GetCamera()
        {
            if (_camera == null)
                _camera = CreateCamera(_cameraParent, _isReflectionMode);

            return _camera;
        }

        private static RenderTexture GetTemporaryRenderTexture(int width, int height,FilterMode filterMode)
        {
            RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 16);
            renderTexture.filterMode = filterMode;
            return renderTexture;
        }

        private static Camera CreateCamera(Transform parent, bool usedToRenderReflection)
        {
            GameObject go = new GameObject (usedToRenderReflection ? "Reflection_Camera" : "Refraction_Camera");
            go.transform.parent = parent;
            go.hideFlags = HideFlags.DontSave;
            go.SetActive(false);

            Camera camera = go.AddComponent<Camera>();
            camera.enabled = false;
            camera.orthographic = true;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.nearClipPlane = 0f;
            return camera;
        }

        #endregion

        #region Editor Only Methods

        #if UNITY_EDITOR
        internal void Validate(WaterRenderingModeParameters parameters)
        {
            RenderTextureResizingFactor = parameters.TextuerResizingFactor;
            ViewingFrustumHeightScalingFactor = parameters.ViewingFrustumHeightScalingFactor;
            CullingMask = parameters.CullingMask;
            RenderTextureFilterMode = parameters.FilterMode;
            ZOffset = parameters.ZOffset;
        }
        #endif

        #endregion
    }

    public struct WaterRenderingModeParameters
    {
        public float TextuerResizingFactor;
        public float ViewingFrustumHeightScalingFactor;
        public LayerMask CullingMask;
        public FilterMode FilterMode;
        public float ZOffset;
    }

}
