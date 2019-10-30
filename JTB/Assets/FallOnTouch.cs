using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallOnTouch : MonoBehaviour
{
    Rigidbody2D rb;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        if (!rb.isKinematic)
            rb.isKinematic = true;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (rb.isKinematic)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                rb.isKinematic = false;
            }
        }
    }
}
