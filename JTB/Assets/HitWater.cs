using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitWater : MonoBehaviour
{
    PlayerStatistics playerStatsScript;
    WaveManager waveManager;
    // Start is called before the first frame update
    void Start()
    {
        playerStatsScript = GameObject.FindWithTag("Player").GetComponent<PlayerStatistics>();
        waveManager = GameObject.Find("Segment2").transform.GetChild(0).GetChild(0).GetComponent<WaveManager>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (playerStatsScript.checkpoint == null)
            {
                playerStatsScript.ReloadCurrentScene();
            }
            else
            {

                playerStatsScript.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                playerStatsScript.gameObject.transform.position = playerStatsScript.checkpoint.transform.position;

                playerStatsScript.stamina = 100f;

                //We set the invulnerability timer to allow the player to reorient themselves at the Checkpoint
                playerStatsScript.invulnTimer = 1.5f;
                waveManager.StartCoroutine("DeleteWaves");
                waveManager.rapidWaves = false;
                
            }

            //if(playerStatsScript.gameObject != null)
            //GameManager.instance.OnPlayerDeath.Invoke();

            playerStatsScript.numPlayerDeaths++;
        }

    }
}
