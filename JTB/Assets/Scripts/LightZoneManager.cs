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

    //It is important that the zones are deactivated before the new zone is activated in case of any overlapping lights
    public void ZoneAHit()
    {
        DeactivateZoneB();
        ActivateZoneA();
    }

    public void ZoneBHit()
    {
        DeactivateZoneA();
        ActivateZoneB();
    }

    //Making the code a bit simpler by having functions to reference the lights
    private void ActivateZoneA()
    {
        lightsA.ActivateLights();
    }
    private void DeactivateZoneA()
    {
        lightsA.DeactivateLights();
    }

    private void ActivateZoneB()
    {
        lightsB.ActivateLights();
    }
    private void DeactivateZoneB()
    {
        lightsB.DeactivateLights();
    }
}
