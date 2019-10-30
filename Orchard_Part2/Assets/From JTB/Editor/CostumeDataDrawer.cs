using UnityEngine;
using UnityEditor;
using Anima2D;

[CustomEditor(typeof(CostumeData))]
public class CostumeDataDrawer : Editor
{
    Rect buttonRect;

    // The CustomizationManager prefab
    private static CustomizationManager cm;

    // The CostumeData this drawer represents
    private CostumeData value;


    /// <summary>
    /// A popup window that automates creation of a costume piece.
    /// </summary>
    public class NewCostumePopup : PopupWindowContent
    {
        // The drawer this popup window was created from.
        CostumeDataDrawer parent;

        // Bone target
        string target = "target";
        // Sprite mesh
        SpriteMesh sprite;
        // Is this piece skin
        bool isSkin = false;


        /// <summary>
        /// Creates the popup window and assigns its parent.
        /// </summary>
        public NewCostumePopup(CostumeDataDrawer cdd)
        {
            parent = cdd;
        }


        /// <summary>
        /// Controls the size of the popup.
        /// </summary>
        public override Vector2 GetWindowSize()
        {
            return new Vector2(300, 100);
        }


        /// <summary>
        /// Handles the popup's appearance.
        /// </summary>
        public override void OnGUI(Rect rect)
        {
            GUILayout.Label("New Costume Piece", EditorStyles.boldLabel);

            // Fields for the target, sprite, and skin.
            target = EditorGUILayout.TextField("Bone Target", target);
            sprite = EditorGUILayout.ObjectField("Sprite Mesh", sprite, typeof(SpriteMesh), false) as SpriteMesh;
            isSkin = EditorGUILayout.Toggle("Is Skin?", isSkin);

            // If the sprite is null, do not allow the user to input a new piece.
            EditorGUI.BeginDisabledGroup(sprite == null);

            // If confirm is clicked
            if (GUILayout.Button("Confirm"))
            {
                parent.serializedObject.Update();

                // Get a reference to the parent's skin meshes list.
                SerializedProperty skinMeshes = parent.serializedObject.FindProperty("skinMeshes");

                // Add a new costume piece to the list.
                int index = skinMeshes.arraySize;
                skinMeshes.arraySize++;

                // Get a reference to the newly created costume piece.
                SerializedProperty newPiece = skinMeshes.GetArrayElementAtIndex(index);

                // This enumerator goes through all the properties of a costume piece
                var childEnum = newPiece.GetEnumerator();
                
                while (childEnum.MoveNext())
                {
                    // Set each costume piece property to the provided property.
                    var current = childEnum.Current as SerializedProperty;

                    if (current.name == "mesh")
                        current.objectReferenceValue = sprite;

                    if (current.name == "skinTarget")
                        current.stringValue = target;

                    if (current.name == "isSkin")
                        current.boolValue = isSkin;
                }

                // Update the costume data object
                parent.serializedObject.ApplyModifiedProperties();
                editorWindow.Close();
            }

            EditorGUI.EndDisabledGroup();
        }
    }



    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // If the CostumizationManager is null, grab a reference to the CustomizationManager prefab.
        if(cm == null)
            cm = (AssetDatabase.LoadAssetAtPath
                ("Assets/PreFabs/Managers/CustomizationManager.prefab", typeof(GameObject)) as GameObject).GetComponent<CustomizationManager>();

        // If the value of this object has not been assigned, assign it.
        if (value == null)
            value = serializedObject.targetObject as CostumeData;

        // If the add button is clicked, show the popup
        if (GUILayout.Button("Add New Piece"))
            PopupWindow.Show(buttonRect, new NewCostumePopup(this));


        // If this costume is already in the manager, do not allow this button to be accessed
        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(value.IsSelectable(cm));

        // When clicked, add the costume to the manager
        if(GUILayout.Button("Add To Customization Manager"))
        {
            value.MakeSelectable(cm);
            // Refresh the manager
            EditorUtility.SetDirty(cm.gameObject);
        }

        EditorGUI.EndDisabledGroup();
        
        // If this costume is not already in the manager, do not allow this button to be accessed
        EditorGUI.BeginDisabledGroup(!value.IsSelectable(cm));

        // When clicked, remove the costume from the manager
        if (GUILayout.Button("Remove From Customization Manager"))
        {
            value.MakeUnselectable(cm);
            // Refresh the manager
            EditorUtility.SetDirty(cm.gameObject);
        }

        EditorGUI.EndDisabledGroup();

        if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();
    }
}
