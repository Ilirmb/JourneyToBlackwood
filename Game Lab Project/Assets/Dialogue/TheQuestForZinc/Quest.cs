using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour {

    public enum QuestState { inactive, start, active, completed, failed, finished };

    private QuestState currentState = QuestState.inactive;
    
    [SerializeField]
    // The name of the character who gives the quest
    private string characterName;

    [SerializeField]
    // The sprite of the character who gives the quest
    private Sprite characterSprite;

    [SerializeField]
    // The number of items needed to complete the quest
    private int numberOfItems = 1;

    // List of quest items
    private List<QuestItem> questItems = new List<QuestItem>();

    private bool firstEncounter = true;

    //hghghghhg
    public TextAsset questIntro;


    // Use this for initialization
    void Start () {
		
	}


    public void StartQuest()
    {

    }


    //
    private void OnMouseDown()
    {
        //DialogueManager.instance.makeDialogueTree(questIntro, characterSprite);
        // This needs to disable the collider, otherwise, the dialogue can infinitely loop. Makes sense /s.
        //DialogueManager.instance.startDialogue(GetComponent<Collider2D>());
    }

    
    public QuestState GetCurrentState()
    {
        return currentState;
    }
}
