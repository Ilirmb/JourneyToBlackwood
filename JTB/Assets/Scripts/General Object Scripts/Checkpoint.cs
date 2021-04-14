using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public Sprite inactiveSprite;
    public Sprite activeSprite;

    private PlayerStatistics playerStatistics;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            this.GetComponent<SpriteRenderer>().sprite = activeSprite;
            playerStatistics.lastCheckpoint(this);
        }
    }

    public void becomeInactive()
    {
        this.GetComponent<SpriteRenderer>().sprite = inactiveSprite;
    }
    // Use this for initialization
    void Start()
    {
        playerStatistics = GameObject.Find(GameConst.PLAYER_OBJECT_NAME).GetComponent<PlayerStatistics>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
