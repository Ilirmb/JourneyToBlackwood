﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWavesInstabilityChecker : MonoBehaviour
{
    public float checkLength = 3f;
    public WaveManager currentWaveManager;
    LayerMask hittables;
    //public LayerMask hittables = LayerMask.NameToLayer("Ground");

    void Start()
    {
        hittables = LayerMask.NameToLayer("Ground");
    }

    private void Update()
    {
        var hit = Physics2D.Raycast(this.transform.position, Vector2.down, checkLength, hittables);
        if(hit.collider != null && (hit.collider.CompareTag("RiverLog") || hit.collider.CompareTag("Water")))
        {
            if (currentWaveManager != null)
            {
                currentWaveManager.isEffected = true;
            }
        }
        else
        {
            if (currentWaveManager != null)
            {
                currentWaveManager.isEffected = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y - checkLength, this.transform.position.z));
    }
}
