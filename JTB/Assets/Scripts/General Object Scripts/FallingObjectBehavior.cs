using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectBehavior : MonoBehaviour
{

    public float waitBeforeDrop = 1f;
    public float waitBeforeDespawn = 1f;

    public float randomAngularSpeed = 45f;
    public float popOffForce = 100f;

    private bool startFlashing = false;
    private float lifetime = 0;
    public float bufferTime = 1.0f;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Collider2D cldr;

    // Use this for initialization
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        cldr = this.GetComponent<CapsuleCollider2D>();

        StartCoroutine(WaitAndDrop(waitBeforeDrop)); //The coroutine waits for a bit after the apple is spawned before dropping it
        startFlashing = false;
    }

    private IEnumerator WaitAndDrop(float waitTime)
    {
        startFlashing = true;
        yield return new WaitForSecondsRealtime(waitTime);
        if (rb.constraints != RigidbodyConstraints2D.None)
        {
            rb.constraints = RigidbodyConstraints2D.None; //Frees the apple to fall onto the player using physics
            rb.AddForce(popOffForce * Vector2.up); //Makes the apple pop in the air after it detatches
            rb.angularVelocity = (randomAngularSpeed * (Random.Range(0, 2) * 2 - 1)); //adds a bit of random rotation to the apple
        }
    }

    private IEnumerator WaitAndDespawn(float waitTime)
    {
        cldr.enabled = false; //We only want to do damage once
        startFlashing = true;
        rb.constraints = RigidbodyConstraints2D.None;
        yield return new WaitForSecondsRealtime(waitTime);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (lifetime > bufferTime)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Do damage here!");
                other.GetComponent<PlayerStatistics>().damageStamina(10, 1f);
                StartCoroutine(WaitAndDespawn(waitBeforeDespawn));
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                //don't damage
                StartCoroutine(WaitAndDespawn(waitBeforeDespawn));
            }
        }
    }

    private void Update()
    {
        if (startFlashing)
        {
            if (Time.frameCount % 5 == 0)
            {
                sr.enabled = !sr.enabled;
            }
        }

        lifetime += Time.deltaTime;
    }
}
