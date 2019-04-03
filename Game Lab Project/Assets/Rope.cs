using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour {

    private RogueQuest1StuckFoot quest;

    private void Start()
    {
        quest = GameObject.Find("RogueStandin").GetComponent<RogueQuest1StuckFoot>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            quest.RopeDialogueTrigger(this.GetComponent<Collider2D>());
            Destroy(this.gameObject);
        }
    }
}
