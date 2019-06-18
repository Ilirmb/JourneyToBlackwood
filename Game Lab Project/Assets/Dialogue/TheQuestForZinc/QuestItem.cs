using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem {

    public enum ItemState { notCollected, collected, destroyed }

    // The current state of the item
    private ItemState currentState = ItemState.notCollected;


    public void ChangeItemState(ItemState newState)
    {
        currentState = newState;
    }
}
