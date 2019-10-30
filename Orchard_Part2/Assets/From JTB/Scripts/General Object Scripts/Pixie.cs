using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixie : MonoBehaviour {

    private RogueQuest1StuckFoot quest;

	void Start () {
        quest = GameObject.Find("RogueStandin").GetComponent<RogueQuest1StuckFoot>();
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            quest.PixieDialogueTrigger(this.GetComponent<Collider2D>());
            Destroy(this.gameObject);
        }
    }
}
