using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlMovement : MonoBehaviour {

    public Rigidbody2D player;

    public Rigidbody2D rb;

    public SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (player.velocity.x > 0 && player.position.x > rb.position.x + 3)
        {
            rb.velocity = new Vector2(player.velocity.x, 0);
            sprite.flipX = true;
        } else if (player.velocity.x < 0 && player.position.x < rb.position.x - 3)
        {
            rb.velocity = new Vector2(player.velocity.x, 0);
            sprite.flipX = false;
        } else if (player.velocity.x == 0 && rb.velocity.x != 0)
        {
            new WaitForSeconds(2);
            rb.velocity = new Vector2(0, 0);
        }
    }
}
