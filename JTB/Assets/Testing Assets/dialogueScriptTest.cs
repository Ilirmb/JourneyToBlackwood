using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialogueScriptTest : MonoBehaviour {
    public int wizardFriendship = 0;

    public TextAsset textAsset;
    public DialogueManager dialogueManager;
    public Sprite defaultSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        dialogueManager.startDialogue(textAsset, defaultSprite);
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
}
