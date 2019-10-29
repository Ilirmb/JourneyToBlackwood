using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A trigger box that activates a quest then disables itself. This must always be a child of the quest giver.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class QuestTrigger : MonoBehaviour {

    private Quest quest;


    void Start()
    {
        quest = transform.parent.GetComponent<Quest>();

        if (!GetComponent<Collider2D>().isTrigger)
            GetComponent<Collider2D>().isTrigger = true;

        if (quest.GetCurrentState().Equals(Quest.QuestState.finished))
            gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !quest.GetCurrentState().Equals(Quest.QuestState.finished))
        {
            // Runs the quest and disables the trigger.
            quest.ProcessClick();
            gameObject.SetActive(false);
        }
    }

}
