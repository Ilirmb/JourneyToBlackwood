using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Quest givers need a collider in order to be clicked on
[RequireComponent(typeof(Collider2D))]
public class Quest : MonoBehaviour {

    public enum QuestState { inactive, start, active, rejected, completed, failed, finished };

    private QuestState currentState = QuestState.inactive;
    
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

    private bool firstEncounter = true;
    private bool rejected = false;
    private bool clicked = false;

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

    // This will be saved and loaded
    private int friendship = 0;

    private Collider2D col;


    // Use this for initialization
    void Start () {

        col = GetComponent<Collider2D>();

        foreach(QuestItem qi in questItems)
        {
            qi.SetOwner(this);
        }

	}


    public void StartQuest()
    {
        currentState = currentState.Equals(QuestState.inactive) ? QuestState.active : currentState;
        ToggleInteractivity();

        DialogueProcessor.instance.StartDialogue(questIntro);
    }


    // If the target is interacted with in any capacity
    private void OnMouseUp()
    {
        if (!clicked)
        {
            firstEncounter = true;
            clicked = true;
        }

        GameManager.instance.SetCurrentQuest(this);

        if (firstEncounter)
        {
            StartQuest();
            return;
        }

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

                if(rejected)
                    DialogueProcessor.instance.StartDialogue(rejectedDialogue);
                else
                    DialogueProcessor.instance.StartDialogue(failedDialogue);
                break;

            case QuestState.completed:
                ToggleInteractivity();

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

    
    public QuestState GetCurrentState()
    {
        return currentState;
    }


    public void ToggleInteractivity()
    {
        col.enabled = !col.enabled;
    }


    public void ToggleInteractivity(bool status)
    {
        col.enabled = status;
    }


    public void EndFirstEncounter()
    {
        firstEncounter = false;
        activeItem = null;
    }


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
    }


    public void FailQuest()
    {
        currentState = QuestState.failed;
    }


    public void AffectFriendship(int amount)
    {
        friendship += amount;
    }


    public void CollectQuestItem()
    {
        activeItem.ChangeItemState(QuestItem.ItemState.collected);

        numberOfItems++;

        if (numberOfItems >= questItems.Count)
            CompleteQuest();
    }


    public void DestroyQuestItem()
    {
        activeItem.ChangeItemState(QuestItem.ItemState.destroyed);

        FailQuest();
    }


    public void SetActiveQuestItem(QuestItem qi)
    {
        activeItem = qi;
    }
}
