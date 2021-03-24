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
    [Header("Add only overlaps to this list, direct children of the group are added automatically")]
    
    public List<Light> lights;

    // Start is called before the first frame update
    void Start()
    {
        //If someone increases the size of lights and doesn't fill the array, we don't want the whole lights system to fall apart because of it
        for(int i = 0; i < lights.Capacity; i++)
        { 
            if(lights[i] == null)
            {
                lights[i] = new Light();
            }
        }

        Light[] childLights = this.transform.GetComponentsInChildren<Light>(true);
        lights.AddRange(childLights);

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
        isActive = true;
        foreach (Light l in lights)
        {
            l.enabled = true;
        }
    }
    public void DeactivateLights()
    {
        isActive = false;
        foreach (Light l in lights)
        {
            l.enabled = false;
        }
    }
}
