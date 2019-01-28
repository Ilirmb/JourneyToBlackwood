using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staminaPickup : MonoBehaviour {

    private playerStatistics playerStatistics;

    public float staminaOnPickup = 10;

    //OnTriggerEnter weirdness means we need to make sure the recovery doesn't trigger twice before the game object has a chance to be destroyed
    private bool hasTriggered = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !hasTriggered)
        {
            hasTriggered = true;
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
