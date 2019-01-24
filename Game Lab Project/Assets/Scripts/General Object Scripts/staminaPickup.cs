using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staminaPickup : MonoBehaviour {

    private playerStatistics playerStatistics;

    public float staminaOnPickup = 10;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            playerStatistics.recoverStamina(staminaOnPickup);
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        playerStatistics = GameObject.Find(GameConst.PLAYER_OBJECT_NAME).GetComponent<playerStatistics>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
