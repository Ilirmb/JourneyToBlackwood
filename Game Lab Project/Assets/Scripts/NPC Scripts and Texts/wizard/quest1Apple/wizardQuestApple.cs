using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Events;

public class wizardQuestApple : MonoBehaviour
{
    public int wizardFriendship = 0;

    public TextAsset questIntro;
    public TextAsset questOutro;
    public TextAsset idleDialogue;
    public TextAsset rejectedDialogue;
    public TextAsset questCompleted;

    private nodeEvent dialogueEvent;

    public Sprite wizardSprite;

    public bool questInProgress = false;
    private bool questWasRejected = false;

    public enum appleStatus { notGotten, gotten, eaten };
    public appleStatus currentAppleStatus = appleStatus.notGotten;

    public DialogueManager dialogueManager;
    bool firstEncoutner = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (firstEncoutner)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;

                dialogueManager.makeDialogueTree(questIntro, wizardSprite);
                nodeTextLine branchingNode = dialogueManager.dialogueTree.getNodeAtIndex(3);
                //We hide the third option because we only want to access it through external variables
                branchingNode.setNumButtons(2);

                //These are those external variables that affect the 
                if (currentAppleStatus == appleStatus.eaten)
                {
                    branchingNode.setNextNode(2);
                    branchingNode.setLine("So I helped you, will you help me?|Oh, Um...");
                }
                else if (currentAppleStatus == appleStatus.gotten)
                {
                    branchingNode.setNextNode(3);
                    branchingNode.setLine("So I helped you, will you help me?|Oh, actually...!");
                }

                dialogueManager.startDialogue(GetComponent<Collider2D>());

                firstEncoutner = false;
                //Shrink the box back down after the player has ran into him the first time.
                GetComponent<BoxCollider2D>().size = new Vector2(5.5f, 6.5f);
                GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0f);
            }
        }
    }

    private void OnMouseDown()
    {
        //Debug.Log("wizard clicked");
        if (!firstEncoutner)
        {
            if (!questWasRejected)
            {
                if (questInProgress)
                {
                    if (currentAppleStatus != appleStatus.notGotten)
                    {
                        //Whenever we have a dialogue choice based on a variable we need to make the tree without starting the dialogue so we can modify it first
                        dialogueManager.makeDialogueTree(questOutro, wizardSprite);
                        if (currentAppleStatus == appleStatus.gotten)
                        {
                            dialogueManager.dialogueTree.getNodeAtIndex(1).setNextNode(1);
                        }
                        else
                        {
                            dialogueManager.dialogueTree.getNodeAtIndex(1).setNextNode(0);
                        }
                        //The quest is over now
                        questInProgress = false;
                        dialogueManager.startDialogue(GetComponent<BoxCollider2D>());
                    }
                    else
                    {
                        dialogueManager.startDialogue(idleDialogue, GetComponent<BoxCollider2D>(), wizardSprite);
                    }
                }
                else
                {
                    dialogueManager.makeDialogueTree(questCompleted, wizardSprite);
                    if (currentAppleStatus == appleStatus.gotten)
                    {
                        dialogueManager.dialogueTree.getNodeAtIndex(1).setNextNode(0);
                    }
                    else
                    {
                        dialogueManager.dialogueTree.getNodeAtIndex(1).setNextNode(1);
                    }
                    dialogueManager.startDialogue(GetComponent<BoxCollider2D>());
                }
            }
            else
            {
                //This works because we only need to rebuild the tree and reset the hidden option when the dialogue has switched to the apple and then back to the wizard
                if (dialogueManager.dialogueTextAsset != rejectedDialogue)
                {
                    dialogueManager.makeDialogueTree(rejectedDialogue, wizardSprite);
                    if (currentAppleStatus != appleStatus.gotten)
                    {
                        dialogueManager.dialogueTree.getNodeAtIndex(2).setNumButtons(0);
                    }
                }
                dialogueManager.startDialogue(GetComponent<Collider2D>());
            }

        }
        else
        {
            // Just call the collision script if they click on him first
            OnTriggerEnter2D(new Collider2D());
        }
    }

    public void changeAppleStatus(appleStatus newStatus)
    {
        currentAppleStatus = newStatus;
    }

    //This describes what each of the events do
    //A node that triggers an event will call it's event trigger, and this script will listen and calls this function
    //The event is dynamic so it will send it's nodeIndex with the call, and this function knows what to do for each given index
    public void dialogueEventHandler(int nodeIndex)
    {
        if (dialogueManager.dialogueTextAsset == questIntro)
        {
            switch (nodeIndex)
            {
                case 5:
                    gameObject.GetComponentInChildren<Text>().text = "Friendship: " + --wizardFriendship;
                    questWasRejected = true;
                    //dialogueEvent.RemoveListener(this.dialogueEventHandler);
                    break;
                case 8:
                    questInProgress = true;
                    break;
                case 9:
                    gameObject.GetComponentInChildren<Text>().text = "Friendship: " + --wizardFriendship;
                    questWasRejected = true;
                    //dialogueEvent.RemoveListener(this.dialogueEventHandler);
                    break;
                case 10:
                    gameObject.GetComponentInChildren<Text>().text = "Friendship: " + --wizardFriendship;
                    UnityEventTools.RemovePersistentListener<int>(dialogueEvent, dialogueEventHandler);
                    //dialogueEvent.RemoveListener(this.dialogueEventHandler);
                    break;
                case 11:
                    gameObject.GetComponentInChildren<Text>().text = "Friendship: " + ++wizardFriendship;
                    break;
                case 12:
                case 14:
                    GameObject.Find(GameConst.PLAYER_OBJECT_NAME).GetComponent<PlayerStatistics>().increaseMaxStamina(20);
                    UnityEventTools.RemovePersistentListener<int>(dialogueEvent, dialogueEventHandler);
                    //dialogueEvent.RemoveListener(this.dialogueEventHandler);
                    break;
                default:
                    break;
            }
        }
        else if (dialogueManager.dialogueTextAsset == questOutro)
        {
            switch (nodeIndex)
            {
                case 6:
                    wizardFriendship -= 2;
                    gameObject.GetComponentInChildren<Text>().text = "Friendship: " + wizardFriendship;
                    UnityEventTools.RemovePersistentListener<int>(dialogueEvent, dialogueEventHandler);
                    //dialogueEvent.RemoveListener(this.dialogueEventHandler);
                    break;
                case 7:
                    gameObject.GetComponentInChildren<Text>().text = "Friendship: " + ++wizardFriendship;
                    break;
                case 8: //Both case 8 and 10 go to the same place.
                case 10:
                    GameObject.Find(GameConst.PLAYER_OBJECT_NAME).GetComponent<PlayerStatistics>().increaseMaxStamina(20);
                    questInProgress = false;
                    UnityEventTools.RemovePersistentListener<int>(dialogueEvent, dialogueEventHandler);
                    //dialogueEvent.RemoveListener(this.dialogueEventHandler);
                    break;
            }
        }
        else if (dialogueManager.dialogueTextAsset == rejectedDialogue)
        {
            switch (nodeIndex)
            {
                case 7:
                    wizardFriendship -= 2;
                    gameObject.GetComponentInChildren<Text>().text = "Friendship: " + wizardFriendship;
                    questWasRejected = false;
                    questInProgress = false;
                    currentAppleStatus = appleStatus.notGotten;
                    UnityEventTools.RemovePersistentListener<int>(dialogueEvent, dialogueEventHandler);
                    break;
                case 9:
                    gameObject.GetComponentInChildren<Text>().text = "Friendship: " + ++wizardFriendship;
                    UnityEventTools.RemovePersistentListener<int>(dialogueEvent, dialogueEventHandler);
                    break;
                case 10:
                case 12:
                    GameObject.Find(GameConst.PLAYER_OBJECT_NAME).GetComponent<PlayerStatistics>().increaseMaxStamina(20);
                    break;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        dialogueEvent = GameObject.Find("Dialogue Canvas").GetComponent<DialogueManager>().dialogueEvent;
        UnityEventTools.AddPersistentListener<int>(dialogueEvent, dialogueEventHandler);
        //dialogueEvent.AddListener(dialogueEventHandler);
        //dialogueEvent.AddListener((int i) => { Debug.Log("Event triggered index of: " + i); });
    }

    // Update is called once per frame
    void Update()
    {
    }
}
