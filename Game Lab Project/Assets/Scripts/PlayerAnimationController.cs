using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

    public float speed;

    private Rigidbody2D rb;
    private Animator anim;

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponentInChildren<Animator>();
	}

    private void Update()
    {
        speed = rb.velocity.x;
        anim.SetFloat("Speed", speed);
    }
}
