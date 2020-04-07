using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This acts as a control middle man between the colliders that define the edges of the zones. It takes the collider information, and from it decides which zone you should be in, then uses the LightGroups of those zones to activate or deactivate the lights accordingly.
/// </summary>

public class LightZoneManager : MonoBehaviour
{
    [SerializeField]
    private LightGroup lightsA;
    [SerializeField]
    private LightGroup lightsB;

    public void ActivateZoneA()
    {
        lightsA.ActivateLights();
    }
    public void DeactivateZoneA()
    {
        lightsA.DeactivateLights();
    }

    public void ActivateZoneB()
    {
        lightsB.ActivateLights();
    }
    public void DeactivateZoneB()
    {
        lightsB.DeactivateLights();
    }
}
