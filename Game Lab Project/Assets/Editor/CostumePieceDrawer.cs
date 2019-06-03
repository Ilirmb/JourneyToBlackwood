using UnityEngine;
using UnityEditor;
using Anima2D;


[CustomPropertyDrawer(typeof(CostumePiece))]
public class CostumePieceDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);

        EditorGUI.BeginProperty(position, label, property);

        //if (property.isExpanded)
        //{
            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var meshRect = new Rect(position.x, position.y, position.width * 0.5f - 5, position.height / 4);
            var targetRect = new Rect(position.x + position.width * 0.5f, position.y, position.width * 0.45f, position.height / 4);
            var skinRect = new Rect(position.x, position.y + position.height * 0.75f, position.width * 0.45f, position.height);
            //EditorGUI.PrefixLabel(new Rect(25, 45, 100, 15), 0, new GUIContent("Preview:"));

            EditorGUI.PropertyField(meshRect, property.FindPropertyRelative("mesh"), GUIContent.none);
            EditorGUI.PropertyField(targetRect, property.FindPropertyRelative("skinTarget"), GUIContent.none);
            EditorGUI.PropertyField(skinRect, property.FindPropertyRelative("isSkin"), GUIContent.none);

            // Preview sprite
            var sprite = (property.FindPropertyRelative("mesh").objectReferenceValue as SpriteMesh).sprite;
            if (sprite != null)
            {
                Vector2 fullSize = new Vector2(sprite.texture.width, sprite.texture.height);
                Vector2 size = new Vector2(sprite.textureRect.width, sprite.textureRect.height);

                Rect coords = sprite.textureRect;
                coords.x /= fullSize.x;
                coords.width /= fullSize.x;
                coords.y /= fullSize.y;
                coords.height /= fullSize.y;

                Vector2 ratio;
                ratio.x = (position.width / 2.0f) / size.x;
                ratio.y = (position.height / 2.0f) / size.y;
                float minRatio = Mathf.Min(ratio.x, ratio.y);

                Vector2 center = position.center;
                position.width = size.x * minRatio;
                position.height = size.y * minRatio;
                position.center = center;

                GUI.DrawTextureWithTexCoords(position, sprite.texture, coords);
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
        //}

        EditorGUI.EndProperty();

        //base.OnGUI(position, property, label);
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //return property.isExpanded ? base.GetPropertyHeight(property, label) * 4 : base.GetPropertyHeight(property, label);
        return base.GetPropertyHeight(property, label) * 4;
    }

}
