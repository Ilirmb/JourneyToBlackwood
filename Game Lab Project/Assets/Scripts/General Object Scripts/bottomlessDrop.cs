using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottomlessDrop : MonoBehaviour {

    private Transform respawnPoint;
    private playerStatistics playerStatistics;

    private float tempInvuln;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //The playerStatistics script is attached to the player, so playerStatistics.gameObject.transform.position is the Vector3 position of the player
            playerStatistics.gameObject.transform.position = respawnPoint.position;
            //The following line is probably better, however the above line is faster and allows the text to spawn at the respawn rather than where the player was at the killplane
            //playerStatistics.gameObject.GetComponent<Rigidbody2D>().MovePosition(respawnPoint.position);

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
        //There is only one child of bottomlessDrop, the respawn point.
        respawnPoint = transform.GetChild(0);
        playerStatistics = GameObject.Find(GameConst.PLAYER_OBJECT_NAME).GetComponent<playerStatistics>();
    }
	
	// Update is called once per frame
	void Update () {
        tempInvuln -= Time.deltaTime;
	}
}
