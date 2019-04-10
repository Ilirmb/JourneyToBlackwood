using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    public float Strength;
    public Vector3 WindDirection;

    void OnTriggerStay(Collider col)
    {
        Rigidbody2D colRigidbody2D = col.GetComponent<Rigidbody2D>();
        if (colRigidbody2D != null)
        {
            colRigidbody2D.AddForce(WindDirection * Strength);
        }
    }
}

