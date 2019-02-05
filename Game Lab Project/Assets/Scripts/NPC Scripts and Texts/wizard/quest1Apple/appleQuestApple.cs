using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//For debugging purposes only, remove later
using UnityEditor.Events;

public class appleQuestApple : MonoBehaviour
{
    private playerStatistics playerStatistics;
    private nodeEvent dialogueEvent;
    public Sprite appleSprite;
    public Sprite eatenSprite;
    public dialogueManager dialogue;
    public wizardQuestApple quest;
    public TextAsset appleDialogue;
    private bool triggered = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!triggered && other.gameObject.CompareTag("Player"))
        {
            triggered = true;
            dialogue.makeDialogueTree(appleDialogue, appleSprite);
            //The node is found using a diagram of the dialogue tree and listing each node in order by layer
            dialogue.dialogueTree.getNodeAtIndex(2).setSprite(eatenSprite);
            if (quest.questInProgress)
            {
                dialogue.dialogueTree.getNodeAtIndex(1).setLine("Apple gotten!|Yum!|The wizard wanted this");
            }
            dialogue.startDialogue();
        }
    }

    public void dialogueEventHandler(int nodeIndex)
    {
        if (dialogue.dialogueTextAsset == appleDialogue)
        {
            switch (nodeIndex)
            {
                case 2:
                    playerStatistics.increaseMaxStamina(5);
                    quest.changeAppleStatus(wizardQuestApple.appleStatus.eaten);
                    UnityEventTools.RemovePersistentListener<int>(dialogueEvent, dialogueEventHandler);
                    //dialogueEvent.RemoveListener(this.dialogueEventHandler);
                    break;
                case 3:
                    quest.changeAppleStatus(wizardQuestApple.appleStatus.gotten);
                    UnityEventTools.RemovePersistentListener<int>(dialogueEvent, dialogueEventHandler);
                    //dialogueEvent.RemoveListener(this.dialogueEventHandler);
                    break;
            }
            //We make a manual call to endDialogue to make sure it ends before we destroy the game object
            //Uh, I've forgotten why I exactly do this. The dialogue canvas isn't linked to the apple game object so it shouldn't matter I think?
            //Especially if all the event handling is already finished. I guess it could disappear while the player is still in dialogue which would be a bit weird but shouldn't cause any issues
            dialogue.endDialogue();
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        playerStatistics = GameObject.Find(GameConst.PLAYER_OBJECT_NAME).GetComponent<playerStatistics>();
        dialogueEvent = GameObject.Find("Dialogue Canvas").GetComponent<dialogueManager>().dialogueEvent;
        UnityEventTools.AddPersistentListener<int>(dialogueEvent, dialogueEventHandler);
        //dialogueEvent.AddListener(dialogueEventHandler);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
