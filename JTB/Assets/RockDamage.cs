using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDamage : MonoBehaviour
{
    GameObject Player;
    PlayerStatistics playerStatsScript;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        playerStatsScript = GameObject.FindWithTag("Player").GetComponent<PlayerStatistics>();
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerStatsScript.stamina -= 25;
            playerStatsScript.CheckIfDead();

            Object.Destroy(this.gameObject);
        }
    }
}