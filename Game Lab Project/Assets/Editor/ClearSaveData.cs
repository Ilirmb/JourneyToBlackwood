using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ClearSaveData : EditorWindow
{
    // Creates a menu icon that when clicked, opens up a window that allows the user to delete saved data
    [MenuItem("Save Data/Clear Data")]
    public static void OpenGUI()
    {
        ClearSaveData window = ScriptableObject.CreateInstance<ClearSaveData>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 90);
        window.ShowPopup();
    }


    // Deletes the save file
    public static void ClearData()
    {
        if (File.Exists(Application.persistentDataPath + "/progress.sav"))
        {
            File.Delete(Application.persistentDataPath + "/progress.sav");
            Debug.LogAssertion("Save Data Deleted");
        }
        else
            Debug.LogError("File not found");
    }


    void OnGUI()
    {
        // Label
        EditorGUILayout.LabelField("Delete all saved data?", EditorStyles.wordWrappedLabel);
        GUILayout.Space(30);

        GUILayout.BeginHorizontal();

        // Do not delete. Close the window.
        if (GUILayout.Button("No"))
            this.Close();

        // Delete and close.
        if (GUILayout.Button("Yes"))
        {
            ClearData();
            this.Close();
        }

        GUILayout.EndHorizontal();

        // Display this as a warning if no file exists
        if(!File.Exists(Application.persistentDataPath + "/progress.sav"))
            EditorGUILayout.LabelField("No save data found.", EditorStyles.wordWrappedLabel);
    }

}