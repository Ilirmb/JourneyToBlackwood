using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just collider management. Essentially activates it's 'zone' whenever the player walks into it
/// </summary>

public class LightZoneCollider : MonoBehaviour
{
    public LightZoneManager manager;
    public bool ZoneB;

    private void Start()
    {
        if(manager == null)
            manager = transform.parent.GetComponent<LightZoneManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!ZoneB)
            {
                manager.ActivateZoneA();
                manager.DeactivateZoneB();
            }
            else
            {
                manager.ActivateZoneB();
                manager.DeactivateZoneA();
            }
        }
    }
}
