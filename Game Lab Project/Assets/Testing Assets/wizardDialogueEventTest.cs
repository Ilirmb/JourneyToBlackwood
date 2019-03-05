using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wizardDialogueEventTest : MonoBehaviour {
    public int wizardFriendship = 0;

    public TextAsset textAsset;
    public DialogueManager dialogueManager;
    public Sprite defaultSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        dialogueManager.startDialogue(textAsset, defaultSprite);
        dialogueManager.dialogueTree.getNodeAtIndex(23).setEvent(dialogueManager.dialogueEvent);
        dialogueManager.dialogueTree.getNodeAtIndex(12).setEvent(dialogueManager.dialogueEvent);
    }

    public void dialogueEventHandler(int nodeIndex)
    {
        switch (nodeIndex)
        {
            case 12:
                gameObject.GetComponentInChildren<Text>().text = "Friendship: " + ++wizardFriendship;
                break;
            case 23:
                gameObject.GetComponentInChildren<Text>().text = "Friendship: " + ++wizardFriendship;
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
}
