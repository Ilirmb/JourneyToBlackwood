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
    [Header("Please try to limit the number of lights in each light group to under 10")]

    //We could have the system search for each group's children, and then of those take the children with the 'light' component and add it to the list
    //However this would come with the downside of being unable to do 'overlap' where lights exist in two groups and remain active through both to allow for smooth transitions
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
