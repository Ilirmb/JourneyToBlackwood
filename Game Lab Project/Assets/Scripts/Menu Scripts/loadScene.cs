using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour
{
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


    /// <summary>
    /// Reloads the current loaded scene
    /// </summary>
    public static void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public static int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}