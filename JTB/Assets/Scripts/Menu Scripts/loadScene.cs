using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour
{
    private List<Scene> scenes;
    public int inspectorsceneindex = 0;

    private void Start()
    {
        scenes = new List<Scene>();
        scenes.Add(SceneManager.GetActiveScene());
        for(int i = scenes[0].buildIndex + 1; i < scenes[0].buildIndex + SceneManager.sceneCount; ++i)
        {
            scenes.Add(SceneManager.GetSceneAt(i));
        }
    }

    /// <summary>
    /// Loads a scene by its name
    /// </summary>
    /// <param name="s">Name of scene</param>
    public void Load(string s)
    {
        SceneManager.LoadScene(s);
    }


    /// <summary>
    /// Loads a scene by its index
    /// </summary>
    /// <param name="i">Index of scene</param>
    public void Load(int i)
    {
        SceneManager.LoadScene(i);
    }


    public void LoadAdd(int i)
    {
        SceneManager.LoadScene(i, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Loads a scene by its name
    /// </summary>
    /// <param name="s">Name of scene</param>
    public static void LoadScene(string s)
    {
        SceneManager.LoadScene(s);
    }
    

    /// <summary>
    /// Loads a scene by its index
    /// </summary>
    /// <param name="i">Index of scene</param>
    public static void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }


    public static void LoadSceneAdd(int i)
    {
        SceneManager.LoadScene(i, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Reloads the current loaded scene
    /// </summary>
    public void ReloadCurrentScene()
    {
        var cur = SceneManager.GetActiveScene();
        foreach(var s in scenes)
        {
             if (s == cur)
                SceneManager.LoadScene(s.buildIndex);
             else
                SceneManager.LoadScene(s.buildIndex, LoadSceneMode.Additive);
        }
    }


    /// <summary>
    /// Gets the index of the current active scene.
    /// </summary>
    public static int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}