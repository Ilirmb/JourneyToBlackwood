using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Kills the player if they make contact with this game object
/// </summary>
public class BottomlessDrop : MonoBehaviour {

    private Transform respawnPoint;
    private PlayerStatistics playerStatistics;

    private float tempInvuln;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //The PlayerStatistics script is attached to the player, so PlayerStatistics.gameObject.transform.position is the Vector3 position of the player
            playerStatistics.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            playerStatistics.gameObject.transform.position = respawnPoint.position;

            //The following line is probably better, however the above line is faster and allows the text to spawn at the respawn rather than where the player was at the killplane
            //PlayerStatistics.gameObject.GetComponent<Rigidbody2D>().MovePosition(respawnPoint.position);

            //OnTriggerEnter seems to be able to trigger more than once per frame, so we need to make sure that players don't take double damage from the killplane
            if (tempInvuln <= 0)
            {
                playerStatistics.damageInvulnImmune(GameConst.DAMAGE_FROM_FALL, 3.0f);
                tempInvuln = .1f;
            }
        }
    }


    // Use this for initialization
    void Start () {
        //There is only one child of BottomlessDrop, the respawn point.
        respawnPoint = transform.GetChild(0);
        playerStatistics = GameObject.Find(GameConst.PLAYER_OBJECT_NAME).GetComponent<PlayerStatistics>();
    }
	

	// Update is called once per frame
	void Update () {
        tempInvuln -= Time.deltaTime;
	}
}
