using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dumb dumb thing, but this script has to be attached to the same gameobject the GameManager is on to ensure it loads properly
/// </summary>

public class GlobalColor : MonoBehaviour
{
    public static GlobalColor Instance;

    public Color hairColor = new Color(0, 86f / 255f, 1, 1); //default bluish color
    public Color eyeColor = new Color(1, 1, 1, 1);

    public void setEyeColor(Color newColor)
    {
        eyeColor = newColor;
    }

    public void setHairColor(Color newColor)
    {
        hairColor = newColor;
    }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
