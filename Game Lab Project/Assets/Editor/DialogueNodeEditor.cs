using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(DialogueNode))]
public class DialogueNodeEditor : NodeEditor {

    private GUIStyle editorLabelStyle;

    public override void OnBodyGUI()
    {
        if (GUILayout.Button("Set First Node"))
        {
            // E
            ((DialogueNode)target).SetFirstNode();
        }

        base.OnBodyGUI();
    }

}
