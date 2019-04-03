using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornbush : MonoBehaviour {

    private Collider2D cldr;

	// Use this for initialization
	void Start () {
        cldr = this.GetComponent<Collider2D>();
	}
	
	public void enableCollider()
    {
        cldr.enabled = true;
    }
    public void disableCollider()
    {
        cldr.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cldr.enabled = false;
            Debug.Log("Damage Player Thornbush");
        }
    }
}
