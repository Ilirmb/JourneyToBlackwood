using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTree : MonoBehaviour {

    private PlayerStatistics ps;
    private Collider2D cldr;

    // Use this for initialization
    void Start() {
        ps = GameObject.Find(GameConst.PLAYER_OBJECT_NAME).GetComponent<PlayerStatistics>();
        cldr = this.GetComponent<Collider2D>();
        cldr.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ps.damageStamina(10, 1f);
        cldr.enabled = false;
    }

    public void startDamageCollider()
    {
        cldr.enabled = true;
    }

    public void endDamageCollider()
    {
        cldr.enabled = false;
    }
}
