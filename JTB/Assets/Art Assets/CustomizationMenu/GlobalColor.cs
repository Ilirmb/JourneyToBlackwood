using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalColor : MonoBehaviour
{
    public static GlobalColor Instance;

    public Color hairColor;
    public Color eyeColor;

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

        eyeColor = new Color(1, 1, 1, 1);
        hairColor = new Color(0, 86f/255f, 1, 1); //default bluish color
    }
}
