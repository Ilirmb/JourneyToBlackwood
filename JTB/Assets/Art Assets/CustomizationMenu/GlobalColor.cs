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
    }
}
