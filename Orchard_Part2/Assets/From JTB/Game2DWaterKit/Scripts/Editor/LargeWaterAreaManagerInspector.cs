namespace Game2DWaterKit
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(LargeWaterAreaManager)),CanEditMultipleObjects]
    public class LargeWaterAreaManagerInspector : Editor
    {
        private SerializedProperty _mainCamera;
        private SerializedProperty _waterObject;
        private SerializedProperty _waterObjectCount;

        private static readonly GUIContent _mainCameraLabel = new GUIContent("Main Camera" , "Sets the main camera that will be used to determine the visibility of the water object. So when a water object goes invisible to this camera, it gets respawned.");
        private static readonly GUIContent _waterObjectLabel = new GUIContent("Water Object","Set the water object. Please make sure that the water object width is at least half of the Main Camera viewing frustum width.");
        private static readonly GUIContent _waterObjectCountLabel = new GUIContent("Water Object Count","Sets the number of water objects to spawn when the game starts.");

        private void OnEnable()
        {
            _mainCamera = serializedObject.FindProperty("_mainCamera");
            _waterObject = serializedObject.FindProperty("_waterObject");
            _waterObjectCount = serializedObject.FindProperty("_waterObjectCount");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_mainCamera,_mainCameraLabel);
            EditorGUILayout.PropertyField(_waterObject,_waterObjectLabel);
            EditorGUILayout.PropertyField(_waterObjectCount,_waterObjectCountLabel);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
