using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Events;

public class RogueQuest1StuckFoot : MonoBehaviour {
    public DialogueManager dialogueManager;
    private bool firstEncounter = true;
    private Collider2D dialogueStartCollider;
    private Sprite rogueSprite;

    public bool questAccepted = false;
    public bool questRejected = false;
    public bool hasRope = false;
    public bool hasPixie = false;

    private TextAsset IntroText;
    public TextAsset GiveItemsText;
    public TextAsset OutroText;
    public TextAsset RopeGetText;
    public TextAsset PixieGetText;
    public TextAsset IdleText;
    public TextAsset LeaveWithoutRogue;
    public TextAsset QuestRejectedText;

	// Use this for initialization
	void Start () {
        dialogueManager = GameObject.Find("Dialogue Canvas").GetComponent<DialogueManager>();
        firstEncounter = true;
        questAccepted = false;
        questRejected = false;
        hasRope = false;
        hasPixie = false;
        dialogueStartCollider = this.GetComponent<BoxCollider2D>();

        IntroText = Resources.Load("Text/Rouge/Quest1/Intro") as TextAsset;
        OutroText = Resources.Load("Text/Rouge/Quest1/Outro") as TextAsset;
        RopeGetText = Resources.Load("Text/Rouge/Quest1/Ropeget") as TextAsset;
        PixieGetText = Resources.Load("Text/Rouge/Quest1/Pixieget") as TextAsset;
        GiveItemsText = Resources.Load("Text/Rouge/Quest1/ReturnWithRopeAndPixie") as TextAsset;
        IdleText = Resources.Load("Text/Rouge/Quest1/IdleDialogue") as TextAsset;
        LeaveWithoutRogue = Resources.Load("Text/Rouge/Quest1/LeaveWithoutRogue") as TextAsset;
        QuestRejectedText = Resources.Load("Text/Rouge/Quest1/QuestRejected") as TextAsset;

        nodeEvent dialogueEvent = dialogueManager.dialogueEvent;
        UnityEventTools.AddPersistentListener<int>(dialogueEvent, EventHandler);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (firstEncounter)
            {
                dialogueManager.makeDialogueTree(IntroText, rogueSprite);
                dialogueManager.startDialogue(dialogueStartCollider);
                firstEncounter = false;
            }
        }
    }

    private void OnMouseDown()
    {
        if (questRejected)
        {
            dialogueManager.startDialogue(QuestRejectedText, rogueSprite);
        }
        else if (!firstEncounter)
        {
            if (hasPixie && hasRope)
            {
                dialogueManager.startDialogue(GiveItemsText, rogueSprite);
            }
        }
        else
        {
            OnTriggerEnter2D(GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>());
        }
    }

    public void EventHandler(int NodeIndex)
    {
        if(dialogueManager.dialogueTextAsset = IntroText)
        {
            switch (NodeIndex)
            {
                case 3: //Rogue Left Alone
                    break;
                case 8: //Quest Accepted
                    questAccepted = true;
                    break;
                case 9: //Quest rejected
                    questRejected = true;
                    break;
            }
        }
    }
    
}
