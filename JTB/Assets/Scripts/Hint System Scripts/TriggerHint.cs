using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerHint : MonoBehaviour
{
    bool hintTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hintTriggered = true;
            GameManager.instance.ShowHint();
        }
    }
}
