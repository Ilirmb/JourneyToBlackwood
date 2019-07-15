using UnityEngine;
using UnityEditor;
using Anima2D;

[CustomEditor(typeof(CostumeData))]
public class CostumeDataDrawer : Editor
{
    Rect buttonRect;

    private static CustomizationManager cm;
    private CostumeData value;

    public class NewCostumePopup : PopupWindowContent
    {
        CostumeDataDrawer parent;

        bool toggle1 = true;
        bool toggle2 = true;
        bool toggle3 = true;

        string target = "target";
        SpriteMesh sprite;
        bool isSkin = false;


        public NewCostumePopup(CostumeDataDrawer cdd)
        {
            parent = cdd;
        }


        public override Vector2 GetWindowSize()
        {
            return new Vector2(300, 100);
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.Label("New Costume Piece", EditorStyles.boldLabel);

            target = EditorGUILayout.TextField("Bone Target", target);
            /*EditorGUI.PrefixLabel(
                new Rect(rect.x, rect.y, rect.width * 0.6f, rect.height), 0, new GUIContent("Mesh"));*/
            sprite = EditorGUILayout.ObjectField("Sprite Mesh", sprite, typeof(SpriteMesh), false) as SpriteMesh;
            /*sprite = 
                (SpriteMesh)EditorGUI.ObjectField(new Rect(150, 0, 20, 20), "Mesh:", sprite, typeof(SpriteMesh), false);*/
            isSkin = EditorGUILayout.Toggle("Is Skin?", isSkin);

            EditorGUI.BeginDisabledGroup(sprite == null);

            if (GUILayout.Button("Confirm"))
            {
                parent.serializedObject.Update();

                SerializedProperty skinMeshes = parent.serializedObject.FindProperty("skinMeshes");

                int index = skinMeshes.arraySize;
                skinMeshes.arraySize++;

                SerializedProperty newPiece = skinMeshes.GetArrayElementAtIndex(index);

                var childEnum = newPiece.GetEnumerator();

                while (childEnum.MoveNext())
                {
                    var current = childEnum.Current as SerializedProperty;

                    if (current.name == "mesh")
                        current.objectReferenceValue = sprite;

                    if (current.name == "skinTarget")
                        current.stringValue = target;

                    if (current.name == "isSkin")
                        current.boolValue = isSkin;
                }


                parent.serializedObject.ApplyModifiedProperties();
                editorWindow.Close();
            }

            EditorGUI.EndDisabledGroup();
        }
    }



    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(cm == null)
            cm = (AssetDatabase.LoadAssetAtPath
                ("Assets/PreFabs/Managers/CustomizationManager.prefab", typeof(GameObject)) as GameObject).GetComponent<CustomizationManager>();

        if (value == null)
            value = serializedObject.targetObject as CostumeData;


        if (GUILayout.Button("Add New Piece"))
        {
            PopupWindow.Show(buttonRect, new NewCostumePopup(this));
        }


        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(value.IsSelectable(cm));

        if(GUILayout.Button("Add To Customization Manager"))
        {
            value.MakeSelectable(cm);
            EditorUtility.SetDirty(cm.gameObject);
        }

        EditorGUI.EndDisabledGroup();


        EditorGUI.BeginDisabledGroup(!value.IsSelectable(cm));

        if (GUILayout.Button("Remove From Customization Manager"))
        {
            value.MakeUnselectable(cm);
            EditorUtility.SetDirty(cm.gameObject);
        }

        EditorGUI.EndDisabledGroup();

        if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();
    }
}
