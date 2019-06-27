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
            case DialogueNode.NodeType.single:

                if(((DialogueTree)target.graph).GetFirstNode().GetID().Equals(((DialogueNode)target).GetID()))
                    EditorGUILayout.LabelField(
                        new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Textures/node_first.png", typeof(Texture2D))));
                else
                    EditorGUILayout.LabelField(
                        new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Textures/node_single.png", typeof(Texture2D))));
                break;

            case DialogueNode.NodeType.branch:

                if (((DialogueTree)target.graph).GetFirstNode().GetID().Equals(((DialogueNode)target).GetID()))
                    EditorGUILayout.LabelField(
                        new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Textures/node_first.png", typeof(Texture2D))));
                else
                    EditorGUILayout.LabelField(
                        new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Textures/node_branch.png", typeof(Texture2D))));
                break;

            case DialogueNode.NodeType.error:
                EditorGUILayout.LabelField(
                    new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Textures/node_error.png", typeof(Texture2D))));
                break;
        }


        base.OnBodyGUI();


        // First Node button.
        if (!((DialogueTree)target.graph).GetFirstNode().GetID().Equals(((DialogueNode)target).GetID()))
        {
            if (GUILayout.Button("Set First Node"))
            {
                ((DialogueNode)target).SetFirstNode();
            }
        }
    }

}
