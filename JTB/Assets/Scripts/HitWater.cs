using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitWater : MonoBehaviour
{
    PlayerStatistics playerStatsScript;
    WaveManager waveManager;
    GameObject Player;

    Vector3[] defaultPosition;
    Quaternion[] defaultRotation;

    Transform[] log;

    // Start is called before the first frame update
    void Start()
    {
        //Testing with Logs - Steve L 9/15/20
        SetInitialPositions();

        Player = GameObject.FindWithTag("Player");
        playerStatsScript = GameObject.FindWithTag("Player").GetComponent<PlayerStatistics>();
        waveManager = GameObject.Find("Segment2").transform.GetChild(0).GetChild(0).GetComponent<WaveManager>();
    }


    void SetInitialPositions()
    {
        //Search for all Logs
        GameObject[] Logs = GameObject.FindGameObjectsWithTag("RiverLog");

        //Create position and rotation with array size based on objects found
        defaultPosition = new Vector3[Logs.Length];
        defaultRotation = new Quaternion[Logs.Length];

        log = new Transform[Logs.Length];

        for (int i = 0; i < Logs.Length; i++)
        {
            log[i] = Logs[i].GetComponent<Transform>();

            defaultPosition[i] = log[i].position;
            defaultRotation[i] = log[i].rotation;
        }
    }

    void resetLogs()
    {
        //Reset all logs back to each original position and rotation
        for (int i = 0; i < log.Length; i++)
        {
            log[i].position = defaultPosition[i];
            log[i].rotation = defaultRotation[i];
        }
    }

   public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerStatsScript.stamina = 0;
            playerStatsScript.CheckIfDead();
            /*if (playerStatsScript.checkpoint == null)
            {
                playerStatsScript.ReloadCurrentScene();
            }
            else
            {
                resetLogs();

                playerStatsScript.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                playerStatsScript.gameObject.transform.position = playerStatsScript.checkpoint.transform.position;

                playerStatsScript.stamina = 100f;
                Player.GetComponent<CustomPlatformerCharacter2D>().isCrouching = false;

                //We set the invulnerability timer to allow the player to reorient themselves at the Checkpoint
                playerStatsScript.invulnTimer = 1.5f;

                waveManager.StartCoroutine("DeleteWaves");
                waveManager.rapidWaves = false;

            }

            //if(playerStatsScript.gameObject != null)
            //GameManager.instance.OnPlayerDeath.Invoke();

            playerStatsScript.numPlayerDeaths++;*/
        }

    }
}
