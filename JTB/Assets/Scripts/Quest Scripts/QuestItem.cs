using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
/// <summary>
/// Represents an item that is tied to the completion state of a quest.
/// </summary>
public class QuestItem : MonoBehaviour {

	// The state of a quest item.
	// Note that destroyed is a general fail state and does not necesarily mean "destroyed."
	// Ex. in the case of the apple quest "destroyed" serves as the "eaten" state
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


	/// <summary>
	/// Changes the item state to the given state.
	/// </summary>
    public void ChangeItemState(ItemState newState)
    {
        currentState = newState;

        if (newState.Equals(ItemState.destroyed) || newState.Equals(ItemState.collected))
            transform.gameObject.SetActive(false);
        else
            SetInteractivity(true);
    }
	

	/// <summary>
	/// Returns the current item state
	/// </summary>
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

	/// <summary>
	/// Called whenever the player interacts with the quest item.
	/// For more complex items, this function can be overwritten.
	/// </summary>
    public virtual void ProcessClick()
    {
        DialogueProcessor.instance.StartDialogue(itemText);
    }

	
	/// <summary>
	/// Sets the interactivity of the item to the given status.
	/// </summary>
    private void SetInteractivity(bool status)
    {
        col.enabled = status;
    }

	/// <summary>
	/// Ties the item to the given quest.
	/// </summary>
    public void SetOwner(Quest q)
    {
        owner = q;
    }
}
