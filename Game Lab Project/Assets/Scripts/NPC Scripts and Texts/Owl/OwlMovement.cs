using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlMovement : MonoBehaviour {

    public Rigidbody2D player;

    public Rigidbody2D rb;

    public SpriteRenderer sprite;

    //Distance owl is above player
    public float owlHeight = 2;

    // Amount owl is offset to the right
    [SerializeField]
    private float owlOffset = -3;

    [SerializeField]
    private float maxOwlDistance = 3.0f;

    [SerializeField]
    private float owlSpeed = 5.0f;


	// Use this for initialization
	void Start () {

        transform.position = new Vector3(player.position.x + owlOffset, player.position.y + owlHeight);

	}
	

	// Update is called once per frame
	void LateUpdate () {

        Vector3 target = new Vector3(player.transform.position.x + owlOffset, player.transform.position.y + owlHeight);
        Vector3 current = transform.position;

        if(Vector3.Distance(current, target) > maxOwlDistance)
        {
            transform.position = Vector3.MoveTowards(current, target, owlSpeed * (Vector3.Distance(current, target) - maxOwlDistance) * Time.deltaTime);

            if(transform.position.x > player.transform.position.x && sprite.flipX == true)
            {
                sprite.flipX = false;
                owlOffset *= -1;
            }
            else if(transform.position.x < player.transform.position.x && sprite.flipX == false)
            {
                sprite.flipX = true;
                owlOffset *= -1;
            }

        }

        /*if (player.velocity.x > 0 && player.position.x > rb.position.x + 3)
        {
            rb.velocity = new Vector2(player.velocity.x, 0);
            sprite.flipX = true;
        }
        else if (player.velocity.x < 0 && player.position.x < rb.position.x - 3)
        {
            rb.velocity = new Vector2(player.velocity.x, 0);
            sprite.flipX = false;
        }
        else if (player.velocity.x == 0 && rb.velocity.x != 0)
        {
            new WaitForSeconds(2);
            rb.velocity = new Vector2(0, 0);
        }

        rb.position = new Vector2(player.position.x, player.position.y + owlHeight);

        if(Mathf.Abs(player.position.x - rb.position.x) > 20)
        {
            rb.position = new Vector2(player.position.x, player.position.y + owlHeight);
        }

        /*Vector3 newPos = player.transform.position;
        newPos.y += owlHeight;
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, 0.05f);*/
    }


    public Vector3 FindTargetDirection(Vector3 current, Vector3 player)
    {
        Vector3 pos = new Vector3(current.x - owlOffset, current.y - owlHeight);
        Debug.Log("Position: " + pos);
        float yDif = player.y - pos.y;
        float xDif = player.x - pos.x;
        Debug.Log("Player Y: " + yDif);
        Debug.Log("Player X: " + xDif);

        return new Vector3(xDif, yDif);
    }
}
