using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueProcessor : MonoBehaviour {

    public static DialogueProcessor instance;

    private DialogueTree currentTree;
    private DialogueNode currentNode;

    private GameObject dialogueUI;

    [SerializeField]
    private Text textBox;
    [SerializeField]
    private Text speakerName;
    [SerializeField]
    private Image face;

    [SerializeField]
    private Button advanceButton;

    [SerializeField]
    private List<Button> dialogueOptions;
    private List<Text> dialogueOptionText;

    private bool firstLine = true;

    
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
        dialogueUI.SetActive(false);
    }


    private void OnDestroy()
    {
        instance = null;
    }


    public void StartDialogue(DialogueTree tree)
    {
        currentTree = tree;
        currentNode = tree.dialogue[0];

        firstLine = true;

        ProcessCurrentNode();
        dialogueUI.SetActive(true);
        GameManager.instance.DisablePlayerMovement();
    }


    private void ProcessCurrentNode()
    {
        switch (currentNode.dialogueNodeType)
        {
            case DialogueNode.NodeType.single:

                textBox.text = currentNode.dialogueText;
                speakerName.text = currentNode.dialogueSpeaker;

                HandleNPCFace(currentNode.dialogueSprite);

                advanceButton.gameObject.SetActive(true);
                HandleDialogueFunctions();

                foreach (Button b in dialogueOptions)
                    b.gameObject.SetActive(false);

                break;


            case DialogueNode.NodeType.branch:

                advanceButton.gameObject.SetActive(false);
                HandleDialogueFunctions();

                for (int i=0; i<currentNode.childNodeIDs.Count; i++)
                {
                    dialogueOptions[i].gameObject.SetActive(true);
                    dialogueOptionText[i].text = currentTree.dialogue[currentNode.childNodeIDs[i].targetID].dialogueText;
                }

                break;
        }
    } 


    private void HandleNPCFace(Sprite s)
    {
        if(s == null)
            face.transform.parent.gameObject.SetActive(false);
        else
        {
            face.transform.parent.gameObject.SetActive(true);
            face.sprite = s;
        }
    }


    private void HandleDialogueFunctions()
    {
        Quest q = GameManager.instance.GetActiveQuest();

        foreach (DialogueAction action in currentNode.actions)
        {
            switch (action.action)
            {
                case DialogueAction.Action.rejectQuest:
                    q.RejectQuest();
                    break;

                case DialogueAction.Action.acceptQuest:
                    q.AcceptQuest();
                    break;

                case DialogueAction.Action.completeQuest:
                    q.CompleteQuest();
                    break;

                case DialogueAction.Action.finishQuest:
                    q.FinishQuest();
                    break;

                case DialogueAction.Action.affectFriendship:
                    q.AffectFriendship(int.Parse(action.param));
                    break;

                case DialogueAction.Action.collectQuestItem:
                    q.CollectQuestItem();
                    break;

                case DialogueAction.Action.destroyQuestItem:
                    q.DestroyQuestItem();
                    break;

                case DialogueAction.Action.increaseStamina:
                    GameManager.instance.IncreasePlayerStamina(float.Parse(action.param));
                    break;
            }
        }
    }


    public void Next()
    {
        // Last one in the list
        if ((CountNumActiveChildren() == 0 && !firstLine) || currentNode == null)
        {
            dialogueUI.SetActive(false);
            GameManager.instance.EnablePlayerMovement();
            GameManager.instance.EndQuestFirstEncounter();
            GameManager.instance.ToggleQuestInteractivity(true);
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
                break;
            }
            else if (dbc.condition.Equals(DialogueBranchCondition.Condition.failed) && state.Equals(Quest.QuestState.failed))
            {
                currentNode = currentTree.dialogue[dbc.targetID];
                break;
            }
            else if (dbc.condition.Equals(DialogueBranchCondition.Condition.active) && state.Equals(Quest.QuestState.active))
            {
                currentNode = currentTree.dialogue[dbc.targetID];
                break;
            }
            else if (dbc.condition.Equals(DialogueBranchCondition.Condition.none))
            {
                currentNode = currentTree.dialogue[dbc.targetID];
                break;
            }
        }

        firstLine = false;

        ProcessCurrentNode();
    }


    private int CountNumActiveChildren()
    {
        int children = 0;

        Quest currentQuest = GameManager.instance.GetActiveQuest();
        Quest.QuestState state = Quest.QuestState.inactive;

        if (currentQuest != null)
            state = currentQuest.GetCurrentState();

        foreach (DialogueBranchCondition dbc in currentNode.childNodeIDs)
        {
            if ((dbc.condition.Equals(DialogueBranchCondition.Condition.cleared) && state.Equals(Quest.QuestState.completed)) ||
                (dbc.condition.Equals(DialogueBranchCondition.Condition.failed) && state.Equals(Quest.QuestState.failed)) ||
                dbc.condition.Equals(DialogueBranchCondition.Condition.none))
                children++;
        }

        return children;
    }


    public void OptionSelected(int button)
    {
        currentNode = currentTree.dialogue[currentNode.childNodeIDs[button].targetID];

        Next();
    }
}
