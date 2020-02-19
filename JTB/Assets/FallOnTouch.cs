using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallOnTouch : MonoBehaviour
{
    Rigidbody2D rb;
    public float delaySeconds = 1;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        if (!rb.isKinematic)
            rb.isKinematic = true;
    }
    private void OnColliderEnter2D(Collision2D other)
    {
        Debug.Log("Collider hit");
        if (rb.isKinematic)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(kinematicEnabled());
            }
        }
    }
    private IEnumerator kinematicEnabled()
    {
        Debug.Log("Test");
        yield return new WaitForSeconds(delaySeconds);
        rb.isKinematic = true;
    }
}
