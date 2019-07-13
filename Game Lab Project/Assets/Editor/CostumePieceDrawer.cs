using UnityEngine;
using UnityEditor;
using Anima2D;


[CustomPropertyDrawer(typeof(CostumePiece))]
public class CostumePieceDrawer : PropertyDrawer {

    int scale = 7;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        if (property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true))
        {
        // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var meshRect = new Rect(position.x, position.y + position.height / scale, position.width * 0.45f, position.height / scale);
            var targetRect = new Rect(position.x + position.width * 0.5f, position.y + position.height / scale, position.width * 0.45f, position.height / scale);
            var skinRect = new Rect(position.x + position.width * 0.5f, position.y + position.height * 0.5f, position.width * 0.45f, position.height / scale);

            EditorGUI.PrefixLabel(
                new Rect(position.x, position.y, position.width * 0.45f, position.height / scale), 0, new GUIContent("Mesh"));
            EditorGUI.PrefixLabel(
                new Rect(position.x + position.width * 0.5f, position.y, position.width * 0.5f - 5, position.height / scale), 1, new GUIContent("Target"));
            EditorGUI.PrefixLabel(new Rect(position.x + position.width * 0.5f, position.y + position.height * 0.4f, position.width * 0.45f, position.height / scale), 
                2, new GUIContent("Is Skin?"));

            EditorGUI.PropertyField(meshRect, property.FindPropertyRelative("mesh"), GUIContent.none);
            EditorGUI.PropertyField(targetRect, property.FindPropertyRelative("skinTarget"), GUIContent.none);
            EditorGUI.PropertyField(skinRect, property.FindPropertyRelative("isSkin"), GUIContent.none);

            // Preview sprite
            var sprite = (property.FindPropertyRelative("mesh").objectReferenceValue as SpriteMesh).sprite;
            if (sprite != null)
            {
                Rect pos = position;

                Vector2 fullSize = new Vector2(sprite.texture.width, sprite.texture.height);
                Vector2 size = new Vector2(sprite.textureRect.width, sprite.textureRect.height);

                Rect coords = sprite.textureRect;
                coords.x /= fullSize.x;
                coords.width /= fullSize.x;
                coords.y /= fullSize.y;
                coords.height /= fullSize.y;

                Vector2 ratio;
                ratio.x = (position.width / 3.5f) / size.x;
                ratio.y = (position.height / 3.5f) / size.y;
                float minRatio = Mathf.Min(ratio.x, ratio.y);

                Vector2 center = pos.center;
                pos.width = size.x * minRatio;
                pos.height = size.y * minRatio;
                pos.center = center;
                pos.x = position.x;
                pos.y = position.y + (position.height * 0.4f);

                GUI.DrawTextureWithTexCoords(pos, sprite.texture, coords);
            }


            if (GUI.Button(new Rect(position.x, position.y + position.height * 0.8f, position.width, 15), "Remove Costume Piece"))
            {
                SerializedProperty list = property.serializedObject.FindProperty("skinMeshes");

                // Very hacky solution. Not a big fan, but my research has not turned up a good way to get an array index
                int index = int.Parse(property.displayName.Replace("Element ", ""));

                if(EditorUtility.DisplayDialog("Delete Costume Piece",
                    "Are you sure you want to delete this costume piece?", "Yes", "No"))
                {
                    list.DeleteArrayElementAtIndex(index);
                    property.serializedObject.ApplyModifiedProperties();
                }
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
        }

        EditorGUI.EndProperty();

        //base.OnGUI(position, property, label);
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return property.isExpanded ? base.GetPropertyHeight(property, label) * scale : base.GetPropertyHeight(property, label);
        //return base.GetPropertyHeight(property, label) * 5;
    }

}
