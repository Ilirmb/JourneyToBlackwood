using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.Events;

public class RogueQuest1StuckFoot : MonoBehaviour {
    public DialogueManager dialogueManager;
    private Collider2D dialogueStartCollider;
    public GameObject ClimbingRope;

    public Sprite rogueSprite;
    public Sprite ropeSprite;
    public Sprite pixieSprite;

    public bool firstEncounter = true;
    public bool questAccepted = false;
    public bool questRejected = false;
    public bool questFinished = false;
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
        questFinished = false;
        hasRope = false;
        hasPixie = false;
        dialogueStartCollider = this.GetComponent<BoxCollider2D>();
        ClimbingRope = GameObject.Find("Climbing Rope");
        ClimbingRope.SetActive(false);

        IntroText = Resources.Load("Text/Rouge/Quest1/Intro") as TextAsset;
        OutroText = Resources.Load("Text/Rouge/Quest1/Outro") as TextAsset;
        RopeGetText = Resources.Load("Text/Rouge/Quest1/Ropeget") as TextAsset;
        PixieGetText = Resources.Load("Text/Rouge/Quest1/Pixieget") as TextAsset;
        GiveItemsText = Resources.Load("Text/Rouge/Quest1/ReturnWithRopeAndPixie") as TextAsset;
        IdleText = Resources.Load("Text/Rouge/Quest1/IdleDialogue") as TextAsset;
        LeaveWithoutRogue = Resources.Load("Text/Rouge/Quest1/LeaveWithoutRogue") as TextAsset;
        QuestRejectedText = Resources.Load("Text/Rouge/Quest1/QuestRejected") as TextAsset;
        
        //UnityEventTools.AddPersistentListener<int>(dialogueManager.dialogueEvent, EventHandler); //How this script knows what events are what. The event calls this script's eventHandler function with the ID of the dialogue node that triggered it
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!questAccepted && !questRejected && firstEncounter)
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
            dialogueManager.startDialogue(QuestRejectedText, dialogueStartCollider, rogueSprite);
        }
        else if (questAccepted)
        {
            if (hasPixie && hasRope)
            {
                if (!questFinished)
                {
                    dialogueManager.startDialogue(GiveItemsText, dialogueStartCollider, rogueSprite);
                }
                else
                {
                    dialogueManager.startDialogue(OutroText, dialogueStartCollider, rogueSprite);
                }
            }
            else
            {
                dialogueManager.startDialogue(IdleText, dialogueStartCollider, rogueSprite);
            }
        }
        else
        {
            dialogueManager.startDialogue(IntroText, dialogueStartCollider, rogueSprite);
        }
    }
    
    public void RopeDialogueTrigger(Collider2D ropeCollider)
    {
        if (!hasRope)
        {
            dialogueManager.startDialogue(RopeGetText, ropeCollider, ropeSprite);
        }
    }

    public void PixieDialogueTrigger(Collider2D pixieCollider)
    {
        if (!hasPixie)
        {
            dialogueManager.startDialogue(PixieGetText, pixieCollider, pixieSprite);
        }
    }

    public void EventHandler(int NodeIndex)
    {
        if (dialogueManager.dialogueTextAsset == IntroText)
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

        if(dialogueManager.dialogueTextAsset == GiveItemsText)
        {
            if(NodeIndex == 4)
            {
                //Do stuff related to the quest ending.
                this.GetComponent<Rigidbody2D>().MovePosition(transform.Find("FinalPosition").position);
                ClimbingRope.SetActive(true);
                questFinished = true;
            }
        }

        if (dialogueManager.dialogueTextAsset == RopeGetText)
        {
            if(NodeIndex == 2)
            {
                hasRope = true;
            }
        }

        if (dialogueManager.dialogueTextAsset == PixieGetText)
        {
            if(NodeIndex == 2)
            {
                hasPixie = true;
            }
        }
    }
}
