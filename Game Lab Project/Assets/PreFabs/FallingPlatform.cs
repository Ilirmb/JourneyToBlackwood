using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{

    private Rigidbody2D myRigidBody;
    private BoxCollider2D boxcollider;

    public float fallDelay;

    private Vector3 initialPos;


    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        boxcollider = GetComponent<BoxCollider2D>();

        initialPos = transform.position;

        // Call Reset every time the player dies
        GameManager.instance.OnPlayerDeath.AddListener(Reset);
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            StartCoroutine(Fall());
            Destroy(gameObject, 3f);
        }
    }


    IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        myRigidBody.isKinematic = false;
        boxcollider.isTrigger = true;


        yield return 0;
    }


    private void Reset()
    {
        myRigidBody.isKinematic = true;
        boxcollider.isTrigger = false;

        transform.position = initialPos;
    }

}