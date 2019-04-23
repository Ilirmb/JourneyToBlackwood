using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFallAnimController : MonoBehaviour {

    private Animator anim;
    private bool hasFallen;

	// Use this for initialization
	void Start () {
        anim = this.GetComponentInChildren<Animator>();
        hasFallen = false;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasFallen)
        {
            anim.SetBool("TreeFall", true);
            hasFallen = true;
        }
    }
}
