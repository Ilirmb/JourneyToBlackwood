using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornbush : MonoBehaviour {

    private Collider2D cldr;
    private Animator anim;

	// Use this for initialization
	void Start () {
        cldr = this.GetComponent<Collider2D>();
        anim = this.GetComponent<Animator>();
	}

	public void enableCollider()
    {
        cldr.enabled = true;
    }
    public void disableCollider()
    {
        cldr.enabled = false;
    }

    private void OnBecameVisible()
    {
        anim.SetBool("OnScreen", true);
    }
    private void OnBecameInvisible()
    {
        anim.SetBool("OnScreen", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cldr.enabled = false;
            collision.GetComponent<PlayerStatistics>().damageStamina(5, 1f);
            Debug.Log("Damage Player Thornbush");
        }
    }
}
