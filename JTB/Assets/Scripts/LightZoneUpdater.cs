using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If the player dies and returns to a checkpoint, we want the zones to update to reflect that.
/// </summary>

public class LightZoneUpdater : MonoBehaviour
{
    public LightGroup CurrentLightGroup;

    private void Awake()
    {
        if(CurrentLightGroup == null)
        {
            this.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(CurrentLightGroup != null)
            {
                LightManager.Instance.ToggleAllOff();
                CurrentLightGroup.ActivateLights();
            }
        }
    }
}
