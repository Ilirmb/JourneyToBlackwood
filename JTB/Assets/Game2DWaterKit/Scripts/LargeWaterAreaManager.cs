namespace Game2DWaterKit
{
    using Game2DWaterKit.Main;
    using Game2DWaterKit.Simulation;
    using UnityEngine;

    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    public class LargeWaterAreaManager : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Game2DWater _waterObject;
        [SerializeField] private int _waterObjectCount = 3;

        private float _cameraLastXPosition;
        private WaterSimulationModule _leftMostWaterSimulationModule;
        private WaterSimulationModule _rightMostWaterSimulationModule;
        private float _waterSurfaceHeighestPoint;
        private Bounds _waterObjectsBounds;

        private int _frameCount;
        private int _lastRenderedFrame;
        private Camera _lastRenderedFrameCamera;

        #region Properties

        public Camera MainCamera { get { return _mainCamera; } set { _mainCamera = value; } }
        public Game2DWater WaterObject { get { return _waterObject; } set { _waterObject = value; } }
        public int WaterObjectCount { get { return _waterObjectCount; } set { _waterObjectCount = Mathf.Clamp(value, 0, int.MaxValue); } }

        private Camera Camera
        {
            get
            {
                if (_mainCamera != null)
                    return _mainCamera;
                else
                    return Camera.main;
            }
        }
        
        internal Matrix4x4 ProjectionMatrix { get; set; }
        internal RenderTexture ReflectionRenderTexture { get; set; }
        internal RenderTexture RefractionPartiallySubmergedObjectsRenderTexture { get; set; }
        internal RenderTexture ReflectionPartiallySubmergedObjectsRenderTexture { get; set; }
        internal RenderTexture RefractionRenderTexture { get; set; }

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("Water");

            if (_waterObject == null)
                return;

            var waterObjectBoxCollider2D = _waterObject.GetComponent<BoxCollider2D>();
            var waterObjectBuoyancyEffector2D = _waterObject.GetComponent<BuoyancyEffector2D>();

            waterObjectBuoyancyEffector2D.enabled = false;
            waterObjectBoxCollider2D.usedByComposite = true;

            var rigidbody2D = gameObject.AddComponent<Rigidbody2D>(); //required by the composite collider
            var compositeCollider2D = gameObject.AddComponent<CompositeCollider2D>();
            var buoyancyEffector2D = gameObject.AddComponent<BuoyancyEffector2D>();

            rigidbody2D.isKinematic = true;

            compositeCollider2D.geometryType = CompositeCollider2D.GeometryType.Polygons;
            compositeCollider2D.isTrigger = true;
            compositeCollider2D.usedByEffector = true;

            buoyancyEffector2D.surfaceLevel = waterObjectBuoyancyEffector2D.surfaceLevel;
            buoyancyEffector2D.density = waterObjectBuoyancyEffector2D.density;
            buoyancyEffector2D.flowAngle = waterObjectBuoyancyEffector2D.flowAngle;
            buoyancyEffector2D.flowMagnitude = waterObjectBuoyancyEffector2D.flowMagnitude;
            buoyancyEffector2D.flowVariation = waterObjectBuoyancyEffector2D.flowVariation;
            buoyancyEffector2D.angularDrag = waterObjectBuoyancyEffector2D.angularDrag;
            buoyancyEffector2D.linearDrag = waterObjectBuoyancyEffector2D.linearDrag;
            buoyancyEffector2D.colliderMask = waterObjectBuoyancyEffector2D.colliderMask;
            buoyancyEffector2D.useColliderMask = waterObjectBuoyancyEffector2D.useColliderMask;
        }

        private void Start()
        {
            if (Camera != null)
                _cameraLastXPosition = Camera.transform.position.x;

            if (_waterObject != null)
                InstantiateWaterObjects();

            _waterSurfaceHeighestPoint = _waterObject.MainModule.Height * 0.5f;
        }

        private void LateUpdate()
        {
            if (_waterObject != null)
                CheckWaterObjectsPositions();

            //in the editor, we'll rely on EditorApplication.update callback to increment the frame count
            //because this callback is invoked even when the editor application is paused (see OnValidate () mehod on line 284)
            #if !UNITY_EDITOR
            _frameCount++;
            #endif
        }

        private void FixedUpdate()
        {
            WaterSimulationModule currentSimulationModule = _leftMostWaterSimulationModule;
            while (currentSimulationModule != null)
            {
                currentSimulationModule.FixedUpdate();
                if (currentSimulationModule.SurfaceHeighestPoint > _waterSurfaceHeighestPoint)
                    _waterSurfaceHeighestPoint = currentSimulationModule.SurfaceHeighestPoint;

                currentSimulationModule = currentSimulationModule.NextWaterSimulationModule;
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var waterObject = GetWaterObjectLocatedAt(collider.transform.position.x);

            if (waterObject != null)
                waterObject.OnCollisonRipplesModule.ResolveCollision(collider, isObjectEnteringWater: true);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            var waterObject = GetWaterObjectLocatedAt(collider.transform.position.x);

            if (waterObject != null)
                waterObject.OnCollisonRipplesModule.ResolveCollision(collider, isObjectEnteringWater: false);
        }

        #endregion

        #region Methods

        public Game2DWater GetWaterObjectLocatedAt(float xPos)
        {
            float waterHalfWidth = _waterObject.MainModule.Width * 0.5f;

            WaterSimulationModule currentSimulationModule = _leftMostWaterSimulationModule;
            while (currentSimulationModule != null)
            {
                float waterXPos = currentSimulationModule.MainModule.Position.x;
                if (Mathf.Abs(xPos - waterXPos) < waterHalfWidth)
                    return currentSimulationModule.MainModule.WaterObject;

                currentSimulationModule = currentSimulationModule.NextWaterSimulationModule;
            }

            return null;
        }

        internal Bounds GetWaterObjectsBoundsRelativeToSpecifiedWaterObject(WaterMainModule currentWaterObject)
        {
            Vector2 waterSize = currentWaterObject.WaterSize;
            Vector2 halfWaterSize = waterSize * 0.5f;

            float leftMostWaterPos = currentWaterObject.TransformWorldToLocal(_leftMostWaterSimulationModule.MainModule.Position).x;
            float rightMostWaterPos = leftMostWaterPos + waterSize.x * (_waterObjectCount - 1);

            Vector2 min = new Vector2(-halfWaterSize.x + leftMostWaterPos, -halfWaterSize.y);
            Vector2 max = new Vector2(halfWaterSize.x + rightMostWaterPos, _waterSurfaceHeighestPoint);

            _waterObjectsBounds.SetMinMax(min, max);
            return _waterObjectsBounds;
        }

        internal bool HasAlreadyRenderedCurrentFrame(Camera currentRenderingCamera)
        {
            return _lastRenderedFrame == _frameCount && _lastRenderedFrameCamera == currentRenderingCamera;
        }

        internal void MarkCurrentFrameAsRendered(Camera currentRenderingCamera)
        {
            _lastRenderedFrame = _frameCount;
            _lastRenderedFrameCamera = currentRenderingCamera;
        }

        private void InstantiateWaterObjects()
        {
            var waterObject = _waterObject.gameObject;
            var spawnPosition = waterObject.transform.position;
            var spawnRotation = waterObject.transform.rotation;
            var parent = waterObject.transform.parent;
            float waterWidth = _waterObject.MainModule.Width;

            _waterObject.MainModule.LargeWaterAreaManager = this;
            _leftMostWaterSimulationModule = _waterObject.SimulationModule;
            _leftMostWaterSimulationModule.IsControlledByLargeWaterAreaManager = true;

            WaterSimulationModule previousSimulationModule = _leftMostWaterSimulationModule;
            for (int i = 1; i < _waterObjectCount; i++)
            {
                spawnPosition.x += waterWidth;

                Game2DWater waterObjectClone = Instantiate(waterObject, spawnPosition, spawnRotation, parent).GetComponent<Game2DWater>();
                waterObjectClone.MainModule.LargeWaterAreaManager = this;

                WaterSimulationModule currentSimulationModule = waterObjectClone.SimulationModule;

                currentSimulationModule.IsControlledByLargeWaterAreaManager = true;
                currentSimulationModule.PreviousWaterSimulationModule = previousSimulationModule;
                previousSimulationModule.NextWaterSimulationModule = currentSimulationModule;

                previousSimulationModule = currentSimulationModule;
            }

            _rightMostWaterSimulationModule = previousSimulationModule;
        }

        private void CheckWaterObjectsPositions()
        {
            float cameraCurrentXPosition = Camera.transform.position.x;
            if (Mathf.Approximately(cameraCurrentXPosition, _cameraLastXPosition))
                return;

            bool isCameraMovingLeftToRight = (cameraCurrentXPosition - _cameraLastXPosition) > 0f;
            _cameraLastXPosition = cameraCurrentXPosition;
            
            if (isCameraMovingLeftToRight)
            {
                if(!IsWaterObjectVisibleToCamera(_leftMostWaterSimulationModule.MainModule))
                {
                    //moving the leftmost water object to the rightmost position
                    float waterWidth = _leftMostWaterSimulationModule.MainModule.Width;
                    Vector3 newPosition = _rightMostWaterSimulationModule.MainModule.Position + new Vector3(waterWidth, 0f, 0f);
                    _leftMostWaterSimulationModule.MainModule.Position = newPosition;
                    _leftMostWaterSimulationModule.ResetSimulation();
                    
                    UpdateWaterObjectsOrder(newLeftMost: _leftMostWaterSimulationModule.NextWaterSimulationModule, newRightMost: _leftMostWaterSimulationModule);
                }
            }
            else
            {
                if (!IsWaterObjectVisibleToCamera(_rightMostWaterSimulationModule.MainModule))
                {
                    //moving the righmost water object to the leftmost position
                    float waterWidth = _rightMostWaterSimulationModule.MainModule.Width;
                    Vector3 newPosition = _leftMostWaterSimulationModule.MainModule.Position - new Vector3(waterWidth, 0f, 0f);
                    _rightMostWaterSimulationModule.MainModule.Position = newPosition;
                    _rightMostWaterSimulationModule.ResetSimulation();

                    UpdateWaterObjectsOrder(newLeftMost: _rightMostWaterSimulationModule, newRightMost: _rightMostWaterSimulationModule.PreviousWaterSimulationModule);
                }
                
            }
        }

        private bool IsWaterObjectVisibleToCamera(WaterMainModule waterObject)
        {
            float waterHalfWidth = waterObject.Width * 0.5f;
            float waterXPos = waterObject.Position.x;

            float cameraHalfWidth = (Screen.width / (float)Screen.height) * Camera.orthographicSize;
            float cameraXPos = Camera.transform.position.x;

            return Mathf.Abs(waterXPos - cameraXPos) < (waterHalfWidth + cameraHalfWidth);
        }

        private void UpdateWaterObjectsOrder(WaterSimulationModule newLeftMost,WaterSimulationModule newRightMost)
        {
            _leftMostWaterSimulationModule.PreviousWaterSimulationModule = _rightMostWaterSimulationModule;
            _rightMostWaterSimulationModule.NextWaterSimulationModule = _leftMostWaterSimulationModule;

            newLeftMost.PreviousWaterSimulationModule = null;
            newRightMost.NextWaterSimulationModule = null;

            _leftMostWaterSimulationModule = newLeftMost;
            _rightMostWaterSimulationModule = newRightMost;
        }
        #endregion

        #region Editor Only Methods
        #if UNITY_EDITOR

        private void OnValidate()
        {
            WaterObjectCount = _waterObjectCount;

            //continues to increment frame count even when the editor application is paused
            EditorApplication.update -= IncrementFrameCount;
            EditorApplication.update += IncrementFrameCount;
        }

        private void IncrementFrameCount()
        {
            _frameCount++;
        }

        // Add menu item to create Game2D Water GameObject.
        [MenuItem("GameObject/2D Object/Game2D Water Kit/Large Water Area", false, 10)]
        private static void CreateWaterObject(MenuCommand menuCommand)
        {
            GameObject largeWaterAreaManagerGO = new GameObject("Large Water Area Manager");
            largeWaterAreaManagerGO.layer = LayerMask.NameToLayer("Water");
            var largeWaterArea = largeWaterAreaManagerGO.AddComponent<LargeWaterAreaManager>();

            GameObject waterGO = new GameObject("Water");
            waterGO.transform.parent = largeWaterAreaManagerGO.transform;
            var water = waterGO.AddComponent<Game2DWater>();

            largeWaterArea._waterObject = water;
            largeWaterArea._mainCamera = Camera.main;
            
            GameObjectUtility.SetParentAndAlign(largeWaterAreaManagerGO, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(largeWaterAreaManagerGO, "Create " + largeWaterAreaManagerGO.name);
            Selection.activeObject = waterGO;
        }
        #endif
        #endregion
    }
}
