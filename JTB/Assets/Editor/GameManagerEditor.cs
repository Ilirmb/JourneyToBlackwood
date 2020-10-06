using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GameManager thisTarget = (GameManager)target;

        thisTarget.playerName = EditorGUILayout.TextField("Player Name", thisTarget.playerName);

        if(GUILayout.Button("Save Game"))
        {
            thisTarget.SaveProgress();
        }
        if (GUILayout.Button("Load Game"))
        {
            thisTarget.LoadProgress(thisTarget.playerName);
        }
    }
}
