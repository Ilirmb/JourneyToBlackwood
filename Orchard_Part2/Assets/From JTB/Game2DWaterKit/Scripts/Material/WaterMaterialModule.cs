namespace Game2DWaterKit.Material
{
    using Game2DWaterKit.Mesh;
    using UnityEngine;

    public class WaterMaterialModule
    {
        #region Variables

        private static readonly string defaultWaterMaterialShader = "Game2DWaterKit/Unlit";

        private static readonly string refractionKeyword = "Water2D_Refraction";
        private static readonly string reflectionKeyword = "Water2D_Reflection";
        private static readonly string fakePerspectiveKeyword = "Water2D_FakePerspective";
        private static readonly string gradientColorKeyword = "Water2D_ColorGradient";
        
        private readonly int refractionRenderTextureID;
        private readonly int reflectionRenderTextureID;
        private readonly int refractionPartiallySubmergedObjectsRenderTextureID;
        private readonly int reflectionPartiallySubmergedObjectsRenderTextureID;
        private readonly int waterMatrixID;
        private readonly int surfaceLevelID;
        private readonly int surfaceSubmergeLevelID;
        private readonly int waterReflectionLowerLimitID;
        private readonly int waterSolidColorID;
        private readonly int waterGradientStartColorID;
        private readonly int waterGradientEndColorID;
        private readonly int waterSurfaceColorID;

        private MaterialPropertyBlock _materialPropertyBlock;
        private Material _material;
        private bool _isRefractionEnabled;
        private bool _isReflectionEnabled;
        private bool _isFakePerspectiveEnabled;
        private bool _isUsingGradientColor;

        private WaterMeshModule _meshModule;

        #endregion

        public WaterMaterialModule()
        {
            refractionRenderTextureID = Shader.PropertyToID("_RefractionTexture");
            refractionPartiallySubmergedObjectsRenderTextureID = Shader.PropertyToID("_RefractionTexturePartiallySubmergedObjects");
            reflectionRenderTextureID = Shader.PropertyToID("_ReflectionTexture");
            reflectionPartiallySubmergedObjectsRenderTextureID = Shader.PropertyToID("_ReflectionTexturePartiallySubmergedObjects");
            waterMatrixID = Shader.PropertyToID("_WaterMVP");
            surfaceLevelID = Shader.PropertyToID("_SurfaceLevel");
            surfaceSubmergeLevelID = Shader.PropertyToID("_SubmergeLevel");
            waterReflectionLowerLimitID = Shader.PropertyToID("_ReflectionLowerLimit");
            waterSolidColorID = Shader.PropertyToID("_WaterColor");
            waterGradientStartColorID = Shader.PropertyToID("_WaterColorGradientStart");
            waterGradientEndColorID = Shader.PropertyToID("_WaterColorGradientEnd");
            waterSurfaceColorID = Shader.PropertyToID("_SurfaceColor");
        }

        #region Properties

        public bool IsUsingGradientColor
        {
            get
            {
                #if UNITY_EDITOR
                _isUsingGradientColor = Material.IsKeywordEnabled(gradientColorKeyword);
                #endif
                return _isUsingGradientColor;
            }
        }

        public Color SolidColor { get { return Material.GetColor(waterSolidColorID); } set { Material.SetColor(waterSolidColorID, value); } }

        public Color GradientStartColor { get { return Material.GetColor(waterGradientStartColorID); } set { Material.SetColor(waterGradientStartColorID, value); } }

        public Color GradientEndColor { get { return Material.GetColor(waterGradientEndColorID); } set { Material.SetColor(waterGradientEndColorID, value); } }

        public Color SurfaceColor { get { return Material.GetColor(waterSurfaceColorID); } set { Material.SetColor(waterSurfaceColorID, value); } }

        internal Material Material
        {
            get
            {
                #if UNITY_EDITOR
                CheckMaterial();
                #endif
                return _material;
            }
        }

        internal bool IsFakePerspectiveEnabled
        {
            get
            {
                #if UNITY_EDITOR
                _isFakePerspectiveEnabled = Material.IsKeywordEnabled(fakePerspectiveKeyword);
                #endif
                return _isFakePerspectiveEnabled;
            }
        }

        internal bool IsReflectionEnabled
        {
            get
            {
                #if UNITY_EDITOR
                _isReflectionEnabled = Material.IsKeywordEnabled(reflectionKeyword);
                #endif
                return _isReflectionEnabled;
            }
        }

        internal bool IsRefractionEnabled
        {
            get
            {
                #if UNITY_EDITOR
                _isRefractionEnabled = Material.IsKeywordEnabled(refractionKeyword);
                #endif
                return _isRefractionEnabled;
            }
        }

        internal float SurfaceSubmergeLevel { get { return Material.GetFloat(surfaceSubmergeLevelID); } }

        internal float SurfaceLevel { get { return Material.GetFloat(surfaceLevelID); } }

        #endregion

        #region Methods

        internal void SetDependencies(WaterMeshModule meshModule)
        {
            _meshModule = meshModule;
        }

        internal void Initialize()
        {
            CheckMaterial();

            _materialPropertyBlock = new MaterialPropertyBlock();
            _meshModule.MeshRenderer.GetPropertyBlock(_materialPropertyBlock);

            _isRefractionEnabled = Material.IsKeywordEnabled(refractionKeyword);
            _isReflectionEnabled = Material.IsKeywordEnabled(reflectionKeyword);
            _isFakePerspectiveEnabled = Material.IsKeywordEnabled(fakePerspectiveKeyword);
            _isUsingGradientColor = Material.IsKeywordEnabled(gradientColorKeyword);
        }

        internal void SetRefractionRenderTexture(RenderTexture renderTexture)
        {
            if (renderTexture != null)
                _materialPropertyBlock.SetTexture(refractionRenderTextureID, renderTexture);
        }

        internal void SetRefractionPartiallySubmergedObjectsRenderTexture(RenderTexture renderTexture)
        {
            if (renderTexture != null)
                _materialPropertyBlock.SetTexture(refractionPartiallySubmergedObjectsRenderTextureID, renderTexture);
        }

        internal void SetReflectionRenderTexture(RenderTexture renderTexture)
        {
            if (renderTexture != null)
                _materialPropertyBlock.SetTexture(reflectionRenderTextureID, renderTexture);
        }

        internal void SetReflectionPartiallySubmergedObjectsRenderTexture(RenderTexture renderTexture)
        {
            if (renderTexture != null)
                _materialPropertyBlock.SetTexture(reflectionPartiallySubmergedObjectsRenderTextureID, renderTexture);
        }

        internal void SetReflectionLowerLimit(float lowerLimit)
        {
            _materialPropertyBlock.SetFloat(waterReflectionLowerLimitID, lowerLimit);
        }

        internal void SetWaterMatrix(Matrix4x4 matrix)
        {
            _materialPropertyBlock.SetMatrix(waterMatrixID, matrix);
        }

        internal void ValidateMaterialPropertyBlock()
        {
            _meshModule.MeshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void CheckMaterial()
        {
            _material = _meshModule.MeshRenderer.sharedMaterial;
            if (_material == null)
            {
                _material = new Material(Shader.Find(defaultWaterMaterialShader));
                _meshModule.MeshRenderer.sharedMaterial = _material;
            }
        }

        #endregion
    }
}
