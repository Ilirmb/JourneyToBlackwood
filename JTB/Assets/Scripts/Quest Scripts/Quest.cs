using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Quest givers need a collider in order to be clicked on
[RequireComponent(typeof(Collider2D))]

/// <summary>
/// A class which represents a quest
/// </summary>
public class Quest : MonoBehaviour {

	// The possible states for a quest. Most are self explanatory, but...
	// completed = quest item obtained, or condition cleared
	// finished = quest is done (success or failed) and does not produce any meaningful interaction when giver is touched.
	// Use these states correctly.
    public enum QuestState { inactive, start, active, rejected, completed, failed, finished };

    private QuestState currentState = QuestState.inactive;

    [SerializeField]
    private string questName;

    [SerializeField]
    // The name of the character who gives the quest
    private string characterName;

    [SerializeField]
    // The sprite of the character who gives the quest
    private Sprite characterSprite;

    // List of quest items
    [SerializeField]
    private List<QuestItem> questItems = new List<QuestItem>();
    private QuestItem activeItem;

    // The number of items collected
    private int numberOfItems = 0;

    [SerializeField]
    private bool collisionStartsFirstEncounter = false;
    private bool firstEncounter = true;
    private bool rejected = false;
    private bool clicked = false;
	private bool cleared = false;
    private bool failed = false;

    // Dialogue assets
    [SerializeField]
    private DialogueTree questIntro;
    [SerializeField]
    private DialogueTree rejectedDialogue;
    [SerializeField]
    private DialogueTree activeDialogue;
    [SerializeField]
    private DialogueTree completedDialogue;
    [SerializeField]
    private DialogueTree failedDialogue;
    [SerializeField]
    private DialogueTree finishedDialogue;

    [Header("Conditions to Activate")]
    public int friendshipThreshold = 0;
    [SerializeField]
    private List<string> questsRequired = new List<string>();

    // This will be saved and loaded
    private int friendship = 0;

    private Collider2D col;

    [SerializeField]
    private bool removeItemsIfInactive = false;


    // Use this for initialization
    void Start () {

        col = GetComponent<Collider2D>();

		// Binds the quest items to this quest
        foreach(QuestItem qi in questItems)
        {
            qi.SetOwner(this);
        }

        // This checks if the quest should be active.
        // Ex: Rogue quests are disabled if the first rogue quest is not completed.
        // This will check the saved friendship value for the quest giver and a list of quest names.

        if (friendship < friendshipThreshold || failed)
        {
            DisableQuest();
            return;
        }

        foreach (string name in questsRequired)
        {
            QuestData qd = GameManager.instance.LoadSpecificQuest(name);

            if(qd != null && (!qd.cleared || qd.failed))
            {
                DisableQuest();
                return;
            }
        }

        if (cleared)
        {
            foreach (QuestItem item in questItems)
                item.gameObject.SetActive(false);
        }

        // Yes, this is actually needed to fix issues.
        col.enabled = false;
        col.enabled = true;

	}


    /// <summary>
    /// Disables a quest. This should only be called if the quest preconditions are not met.
    /// </summary>
    public void DisableQuest()
    {
        if (removeItemsIfInactive)
        {
            foreach (QuestItem item in questItems)
                item.gameObject.SetActive(false);
        }

        transform.gameObject.SetActive(false);
    }

	
	/// <summary>
	/// Begins a quest. Simple stuff.
	/// </summary>
    public void StartQuest()
    {
		// If a quest is cleared or failed before giver is interacted with, do not change its state
        currentState = currentState.Equals(QuestState.inactive) ? QuestState.active : currentState;
		
		// Stops the giver from being interacted with
        ToggleInteractivity();

        DialogueProcessor.instance.StartDialogue(questIntro);
    }


    // If the target is interacted with in any capacity
	// We use OnMouseUp to avoid conflicts with the "next node" button on the UI
    private void OnMouseUp()
    {
        ProcessClick();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && firstEncounter && collisionStartsFirstEncounter)
        {
            ProcessClick();
        }
    }


    /// <summary>
    /// Handles what should happen when the quest giver is clicked on.
    /// </summary>
    public void ProcessClick()
    {
        // Failsafe. First encounter flags can be set by quest item interaction, so we use an additional check.
        if (!clicked)
        {
            firstEncounter = true;
            clicked = true;
        }

        Debug.Log("Current State: " + currentState);

        GameManager.instance.SetCurrentQuest(this);

        Debug.Log("Current State: " + currentState);

        // If the player has not been interacted, we just need to start the quest
        if (firstEncounter)
        {
            StartQuest();
            return;
        }

        Debug.Log("Current State: " + currentState);
        // Do different interactions based on the current state of the quest.
        switch (currentState)
        {
            case QuestState.active:
                ToggleInteractivity();
                DialogueProcessor.instance.StartDialogue(activeDialogue);
                break;

            case QuestState.rejected:
                ToggleInteractivity();
                DialogueProcessor.instance.StartDialogue(rejectedDialogue);
                break;

            case QuestState.failed:
                ToggleInteractivity();

                // If the quest was ever rejected, we want to start off the dialogue differently. Plan around this.
                if (rejected)
                    DialogueProcessor.instance.StartDialogue(rejectedDialogue);
                else
                    DialogueProcessor.instance.StartDialogue(failedDialogue);
                break;

            case QuestState.completed:
                ToggleInteractivity();

                // If the quest was ever rejected, we want to start off the dialogue differently. Plan around this.
                if (rejected)
                    DialogueProcessor.instance.StartDialogue(rejectedDialogue);
                else
                    DialogueProcessor.instance.StartDialogue(completedDialogue);
                break;

            case QuestState.finished:
                ToggleInteractivity();
                DialogueProcessor.instance.StartDialogue(finishedDialogue);
                break;

            default:
                ToggleInteractivity();
                DialogueProcessor.instance.StartDialogue(questIntro);
                break;

        }
    }

	
    /// <summary>
	/// Gets the current state of the quest
	/// </summary>
    public QuestState GetCurrentState()
    {
        return currentState;
    }


	/// <summary>
	/// Toggles whether or not the quest giver can be interacted with
	/// </summary>
    public void ToggleInteractivity()
    {
        col.enabled = !col.enabled;
    }


	/// <summary>
	/// Forbicbly sets the interactable state of the quest giver
	/// </summary>
    public void ToggleInteractivity(bool status)
    {
       col.enabled = status;
    }


	/// <summary>
	/// Sets first encounter flags
	/// </summary>
    public void EndFirstEncounter()
    {
        firstEncounter = false;
        activeItem = null;
    }


	#region QuestState Setters
	
    public void AcceptQuest()
    {
        currentState = QuestState.active;
    }


    public void RejectQuest()
    {
        currentState = QuestState.rejected;
        rejected = true;
    }


    public void CompleteQuest()
    {
        currentState = QuestState.completed;
    }


    public void FinishQuest()
    {
        Debug.Log(friendship);
        currentState = QuestState.finished;
        cleared = true;
    }


    public void FailQuest()
    {
        currentState = QuestState.failed;
        failed = true;
        Debug.Log("Failed");
    }
	#endregion


	/// <summary>
	/// Increases or decreases the friendship of the quest giver
	/// </summary>
    public void AffectFriendship(int amount)
    {
        friendship += amount;
        Debug.Log("Current Friendship for " + characterName + ": " + friendship);
    }


	/// <summary>
	/// Sets the state of the active quest item to collected
	/// </summary>
    public void CollectQuestItem()
    {
        activeItem.ChangeItemState(QuestItem.ItemState.collected);

        numberOfItems++;

        if (numberOfItems >= questItems.Count)
            CompleteQuest();
    }


	/// <summary>
	/// Sets the state of the active quest item to destroyed
	/// </summary>
    public void DestroyQuestItem()
    {
        activeItem.ChangeItemState(QuestItem.ItemState.destroyed);

        FailQuest();
    }


	/// <summary>
	/// Sets the state of a specific quest item to destroyed
	/// </summary>
    public void DestroyQuestItem(int index)
    {
        questItems[index].ChangeItemState(QuestItem.ItemState.destroyed);

        FailQuest();
    }

	
	/// <summary>
	/// Sets the state of all quest items to destroyed
	/// </summary>
	public void DestroyAllQuestItems()
	{
		foreach (QuestItem qi in questItems)
		{
			qi.ChangeItemState(QuestItem.ItemState.destroyed);
		}
		
        FailQuest();
	}

	
	/// <summary>
	/// Sets the given quest item to the quest's current active item
	/// </summary>
    public void SetActiveQuestItem(QuestItem qi)
    {
        activeItem = qi;
    }


    public void LoadQuestState(QuestData data)
    {
        // Eventually, this will be called from the GameManager

        if (data.cleared)
        {
            Debug.Log("Quest loaded in with the 'cleared' state active!");
            clicked = true;
            cleared = true;
            firstEncounter = false;
            currentState = QuestState.finished;
        }
        else
            Debug.Log("Quest loaded in with uncleared state!");

        friendship = data.friendship;
        failed = data.failed;
    }


    /// <summary>
    /// Returns a QuestData object that represents a quest. This data can then be saved to an external file.
    /// </summary>
    public QuestData SaveQuestData()
    {
        QuestData data = new QuestData();

        // Unless someone isn't paying attention, this will be unique for all quests.
        // If the quest is unnamed, it will default to a gross combination of strings to ensure it's unique.
        data.questHash = !questName.Equals("") ? questName :
            characterName + questIntro.name + rejectedDialogue.name + activeDialogue.name + failedDialogue.name + finishedDialogue.name;

        data.owner = characterName;
        data.friendship = friendship;
        data.cleared = cleared;
        data.failed = failed;

        return data;
    }
}


/// <summary>
/// Serialized representation of a Quest.
/// </summary>
[System.Serializable]
public class QuestData
{
    public string questHash;

    public string owner;
    public int friendship;
    public bool cleared;
    public bool failed;
}
