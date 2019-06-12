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

        GameManager.instance.OnPlayerDeath.AddListener(Reset);
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            if (!hasFallen)
            {
                anim.SetBool("TreeFall", true);
                hasFallen = true;
            }
        }
    }


    private void Reset()
    {
        anim.ResetTrigger("TreeFall");
        anim.Play("Idle tree 1");

        hasFallen = false;
    }
}
