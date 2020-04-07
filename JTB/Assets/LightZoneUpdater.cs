using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If the player dies and returns to a checkpoint, we want the zones to update to reflect that. This script essentially does that, by using the defined border to tell the thing to turn off. 
/// 
/// I'll probably refine this into a better system where each 'zone' knows it's start and end colliders and can command them to toggle nearby zones off or better yet, a general zone manager that can make sure the currently active zone is the only active zone. This is probably the best solution
/// </summary>

public class LightZoneUpdater : MonoBehaviour
{
    [Header("Fill with the next Zone Colliders to make sure current light zone is active on respawn")]
    public LightZoneManager PossiblyCrossedZoneBorder;
    public bool isZoneB;

    private void Awake()
    {
        if(PossiblyCrossedZoneBorder == null)
        {
            //If there is no assigned LightZoneManager, there's no reason to bother with checking for collisions or doing comparisons
            this.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(PossiblyCrossedZoneBorder != null)
            {
                if (!isZoneB)
                {
                    PossiblyCrossedZoneBorder.ZoneAHit();
                }
                else
                {
                    PossiblyCrossedZoneBorder.ZoneBHit();
                }
            }
        }
    }
}
