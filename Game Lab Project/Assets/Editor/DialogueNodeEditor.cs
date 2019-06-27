using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;

[CustomNodeEditor(typeof(DialogueNode))]
public class DialogueNodeEditor : NodeEditor {

    private GUIStyle editorLabelStyle;

    public override void OnBodyGUI()
    {
        // Change color of the editor node
        switch (((DialogueNode)target).GetNodeType())
        {
            // Adds "Single" banner, unless node is the first node
            case DialogueNode.NodeType.single:

                if(((DialogueTree)target.graph).GetFirstNode().GetID().Equals(((DialogueNode)target).GetID()))
                    EditorGUILayout.LabelField(
                        new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Textures/node_first.png", typeof(Texture2D))));
                else
                    EditorGUILayout.LabelField(
                        new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Textures/node_single.png", typeof(Texture2D))));
                break;

            // Adds "Branch" banner, unless node is the first node
            case DialogueNode.NodeType.branch:

                if (((DialogueTree)target.graph).GetFirstNode().GetID().Equals(((DialogueNode)target).GetID()))
                    EditorGUILayout.LabelField(
                        new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Textures/node_first.png", typeof(Texture2D))));
                else
                    EditorGUILayout.LabelField(
                        new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Textures/node_branch.png", typeof(Texture2D))));
                break;

            // Adds "Error" banner. You generally won't see this, but if you do it's not good.
            case DialogueNode.NodeType.error:
                EditorGUILayout.LabelField(
                    new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Textures/node_error.png", typeof(Texture2D))));
                break;
        }


        base.OnBodyGUI();


        // First Node button. Only display if the given node is not the first node
        if (!((DialogueTree)target.graph).GetFirstNode().GetID().Equals(((DialogueNode)target).GetID()))
        {
            if (GUILayout.Button("Set First Node"))
            {
                ((DialogueNode)target).SetFirstNode();
            }
        }
    }

}
