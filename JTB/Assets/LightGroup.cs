using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The LightGroup script essentially monitors and manages a group of lights. The colliders themselves reference their own instance of this script to activate and deactivate large groups of lights
/// </summary>

public class LightGroup : MonoBehaviour
{
    [SerializeField]
    private bool isActive;
    public Light[] lights;

    // Start is called before the first frame update
    void Start()
    {
        //This allows us to activate the first group of lights automatically, meaning the player character doesn't have to interact with a preliminary collider to activate them
        if (isActive)
        {
            ActivateLights();
        }
        else
        {
            DeactivateLights();
        }
    }

    public void ActivateLights()
    {
        if (isActive == false)
        {
            isActive = true;
            foreach (Light l in lights)
            {
                l.enabled = true;
            }
        }
    }
    public void DeactivateLights()
    {
        if (isActive == true) {
            isActive = false;
            foreach (Light l in lights)
            {
                l.enabled = false;
            }
        }
    }
}
