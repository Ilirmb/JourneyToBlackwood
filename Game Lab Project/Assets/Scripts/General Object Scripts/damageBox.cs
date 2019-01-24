using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageBox : MonoBehaviour {

    private bool triggered;
    private playerStatistics playerStatistics;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            triggered = true;
            Debug.Log("triggered");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            triggered = false;
            Debug.Log("untriggered");
        }
    }

    // Use this for initialization
    void Start () {
        playerStatistics = GameObject.Find(GameConst.PLAYER_OBJECT_NAME).GetComponent<playerStatistics>();
    }
	
	// Update is called once per frame
	void Update () {
		if (triggered == true)
        {
            playerStatistics.damageStamina(GameConst.DAMAGE_FROM_HIT, 1.5f);
        }
	}
}
