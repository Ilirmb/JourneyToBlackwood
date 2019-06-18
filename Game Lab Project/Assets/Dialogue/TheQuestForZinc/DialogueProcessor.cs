using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueProcessor : MonoBehaviour {

    // Delet this please
    public DialogueTree durr;

    public static DialogueProcessor instance;

    private DialogueTree currentTree;
    private DialogueNode currentNode;

    private GameObject dialogueUI;

    [SerializeField]
    private Text textBox;

    [SerializeField]
    private Button advanceButton;

    [SerializeField]
    private List<Button> dialogueOptions;
    private List<Text> dialogueOptionText;

    
    void Start()
    {
        if (instance == null)
            instance = this;

        dialogueOptionText = new List<Text>();

        foreach(Button b in dialogueOptions)
        {
            dialogueOptionText.Add(b.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>());
        }

        dialogueUI = transform.GetChild(0).gameObject;

        // Testing shit
        currentTree = durr;
        StartDialogue(currentTree);
    }


    private void OnDestroy()
    {
        instance = null;
    }


    public void StartDialogue(DialogueTree tree)
    {
        dialogueUI.SetActive(true);

        currentTree = tree;
        currentNode = tree.dialogue[0];

        ProcessCurrentNode();
    }
    
    
    private void ProcessCurrentNode()
    {
        switch (currentNode.dialogueNodeType)
        {
            case DialogueNode.NodeType.single:

                textBox.text = currentNode.dialogueText;
                advanceButton.gameObject.SetActive(true);

                foreach (Button b in dialogueOptions)
                    b.gameObject.SetActive(false);

                break;


            case DialogueNode.NodeType.branch:

                advanceButton.gameObject.SetActive(false);

                for (int i=0; i<currentNode.childNodeIDs.Count; i++)
                {
                    dialogueOptions[i].gameObject.SetActive(true);
                    dialogueOptionText[i].text = currentTree.dialogue[currentNode.childNodeIDs[i].targetID].dialogueText;
                }

                break;
        }
    } 


    public void Next()
    {
        // Last one in the list
        if (currentNode.childNodeIDs.Count == 0 || currentNode == null)
        {
            dialogueUI.SetActive(false);
            return;
        }

        Quest currentQuest = GameManager.instance.GetActiveQuest();
        Quest.QuestState state = Quest.QuestState.inactive;

        if (currentQuest != null)
            state = currentQuest.GetCurrentState();

        foreach (DialogueBranchCondition dbc in currentNode.childNodeIDs)
        {
            if (dbc.condition.Equals(DialogueBranchCondition.Condition.cleared) && state.Equals(Quest.QuestState.completed))
            {
                currentNode = currentTree.dialogue[dbc.targetID];
            }
            else if (dbc.condition.Equals(DialogueBranchCondition.Condition.failed) && state.Equals(Quest.QuestState.failed))
            {
                currentNode = currentTree.dialogue[dbc.targetID];
            }
            else if (dbc.condition.Equals(DialogueBranchCondition.Condition.none))
            {
                currentNode = currentTree.dialogue[dbc.targetID];
            }
        }

        ProcessCurrentNode();
    }


    public void OptionSelected(int button)
    {
        currentNode = currentTree.dialogue[currentNode.childNodeIDs[button].targetID];

        Next();
    }
}
