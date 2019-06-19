using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class QuestItem : MonoBehaviour {

    public enum ItemState { notCollected, collected, destroyed }

    // The current state of the item
    private ItemState currentState = ItemState.notCollected;

    [SerializeField]
    private DialogueTree itemText;

    private Collider2D col;
    private Quest owner;


    public void Start()
    {
        col = GetComponent<Collider2D>();

        if (!col.isTrigger)
            col.isTrigger = true;
    }


    public void ChangeItemState(ItemState newState)
    {
        currentState = newState;

        if (newState.Equals(ItemState.destroyed) || newState.Equals(ItemState.collected))
            transform.gameObject.SetActive(false);
        else
            SetInteractivity(true);
    }


    public ItemState GetItemState()
    {
        return currentState;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetInteractivity(false);
            owner.SetActiveQuestItem(this);
            GameManager.instance.SetCurrentQuest(owner);
            ProcessClick();
        }
    }


    public virtual void ProcessClick()
    {
        DialogueProcessor.instance.StartDialogue(itemText);
    }


    private void SetInteractivity(bool status)
    {
        col.enabled = status;
    }


    public void SetOwner(Quest q)
    {
        owner = q;
    }
}
