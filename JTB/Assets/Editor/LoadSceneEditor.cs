using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(loadScene))]
public class LoadSceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        loadScene thisTarget = (loadScene)target;
        thisTarget.inspectorsceneindex = EditorGUILayout.IntField("Scene Index to Load", thisTarget.inspectorsceneindex);
        if(GUILayout.Button("Load Scene at Runtime"))
        {
            thisTarget.Load(thisTarget.inspectorsceneindex);
        }
    }
}
