using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamageOnTrigger : MonoBehaviour
{
    GameObject Player;
    PlayerStatistics playerStatsScript;
    //public Checkpoint checkPoint;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        playerStatsScript = GameObject.FindWithTag("Player").GetComponent<PlayerStatistics>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //checkPoint = playerStatsScript.checkpoint;
            playerStatsScript.stamina -=100;
            //Player.transform.position = checkPoint.transform.position;
            playerStatsScript.CheckIfDead();
        }
    }
}
