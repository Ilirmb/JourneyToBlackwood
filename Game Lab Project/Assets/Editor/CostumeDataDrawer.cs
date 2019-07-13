using UnityEngine;
using UnityEditor;
using Anima2D;

[CustomEditor(typeof(CostumeData))]
public class CostumeDataDrawer : Editor
{
    Rect buttonRect;

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
            return new Vector2(400, 150);
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.Label("New Costume Piece", EditorStyles.boldLabel);

            target = EditorGUILayout.TextField("Bone Target", target);
            //sprite = (SpriteMesh)EditorGUI.ObjectField(new Rect(150, 0, 200, 200), "Mesh:", sprite, typeof(SpriteMesh));
            isSkin = EditorGUILayout.Toggle("Is Skin?", isSkin);

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
                    {
                    }

                    if (current.name == "skinTarget")
                        current.stringValue = target;

                    if (current.name == "isSkin")
                        current.boolValue = isSkin;
                }


                parent.serializedObject.ApplyModifiedProperties();
            }
        }
    }



    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Add New Piece"))
        {
            PopupWindow.Show(buttonRect, new NewCostumePopup(this));
        }

        if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();
    }
}
