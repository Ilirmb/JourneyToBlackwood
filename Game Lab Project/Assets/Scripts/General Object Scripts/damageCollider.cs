using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Take this script and put it on to any object with a 2D Collider that is a trigger and it'll deal damage to the player
/// </summary>

public class DamageCollider : MonoBehaviour {

    [SerializeField] private float damageToDeal = GameConst.DAMAGE_FROM_HIT;
    [SerializeField] private float invulnerabilityTime = 1.5f;

    private bool triggered;
    private PlayerStatistics playerStatistics;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            triggered = true;
            //Debug.Log("triggered");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            triggered = false;
            //Debug.Log("untriggered");
        }
    }

    // Use this for initialization
    void Start () {
        playerStatistics = GameObject.Find(GameConst.PLAYER_OBJECT_NAME).GetComponent<PlayerStatistics>();
    }
	
	// Update is called once per frame
	void Update () {
		if (triggered == true)
        {
            playerStatistics.damageStamina(damageToDeal, invulnerabilityTime);
        }
	}
}
