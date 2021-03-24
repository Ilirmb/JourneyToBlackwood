using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script manages the light groups as a whole to allow for them to be turned off all at once
/// Only one of these should be present on the scene, and child-ed under it should be all the light groups
/// It also instances itself, which creates a singleton and allows any script to quickly deactivate all the lights
/// </summary>
public class LightManager : MonoBehaviour
{
    public static LightManager Instance;

    LightGroup[] lightGroups;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        lightGroups = this.transform.GetComponentsInChildren<LightGroup>();
    }

    public void ToggleAllOff()
    {
        foreach(LightGroup l in lightGroups)
        {
            l.DeactivateLights();
        }
    }

    public void ToggleAllOn()
    {
        foreach (LightGroup l in lightGroups)
        {
            l.ActivateLights();
        }
    }
}
