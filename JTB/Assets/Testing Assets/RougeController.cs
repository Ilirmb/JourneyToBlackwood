using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RougeController : MonoBehaviour {
    //private Rigidbody2D rb;
    private Animator anim;

	// Use this for initialization
	void Start () {
        //rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();

        //Flip the rouge from right facing to left facing

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreCollision(this.GetComponent<CircleCollider2D>(), player.GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(this.GetComponent<CircleCollider2D>(), player.GetComponent<CircleCollider2D>());
        anim.SetBool("Ground", true);
    }

	//Put functions in here to control the left and right movement of the rogue
}
