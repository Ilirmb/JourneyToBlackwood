using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Processes a given dialogue tree, displays its contents on screen, and calls the functions associated with each line of dialogue
/// </summary>
public class DialogueProcessor : MonoBehaviour {

	// Allows DialogueProcessor to be accessed by other scripts
    public static DialogueProcessor instance;

	// Current dialogue tree
    private DialogueTree currentTree;
    private DialogueNode currentNode;

    [HideInInspector] public GameObject dialogueUI;

    [SerializeField]
    private Text textBox;
    [SerializeField]
    private Text topTextBox;
    [SerializeField]
    private Text speakerName;
    [SerializeField]
    private Text topSpeakerName;
    [SerializeField]
    private Image face;
    [SerializeField]
    private AudioSource questAudio;

    [SerializeField]
    private Button advanceButton;

    [SerializeField]
    private List<Button> dialogueOptions;
    private List<Text> dialogueOptionText;

	// If the current line of dialogue is the first line in a given tree
    private bool firstLine = true;
    
    [Header("Automatic Text Params")]
    private IEnumerator textPrint;
    private bool isPrinting = false;
    private string textToDisplay = "";

    [SerializeField]
    // Amount of time to wait before printing a character.
    private float characterDelay = 0.001f;
    private float currentDelay;

    private bool forceNext = false;
    private bool useTopTextBox = false;

    
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

	
	/// <summary>
	/// Destroys the current instance of dialogue processor
	/// </summary>
    private void OnDestroy()
    {
        instance = null;
    }


	/// <summary>
	/// Begins a dialogue.
	/// </summary>
    public void StartDialogue(DialogueTree tree)
    {
        currentTree = tree;
        currentNode = tree.GetFirstNode();

        firstLine = true;
        useTopTextBox = false;

        ProcessCurrentNode();
        dialogueUI.SetActive(true);
		
		// We don't want the player to move when dialogue is active ever.
        GameManager.instance.DisablePlayerMovement();
    }


    /// <summary>
    /// Begins a dialogue.
    /// </summary>
    public void StartDialogue(DialogueTree tree, bool allowMovement)
    {
        currentTree = tree;
        currentNode = tree.GetFirstNode();

        firstLine = true;
        // If the player should move, use the top text box to avoid obstructing their vision
        useTopTextBox = allowMovement;

        ProcessCurrentNode();
        dialogueUI.SetActive(true);

        if(!allowMovement)
            GameManager.instance.DisablePlayerMovement();
    }


    /// <summary>
    /// Processes the current selected node
    /// </summary>
    private void ProcessCurrentNode()
    {
        switch (currentNode.dialogueNodeType)
        {
			// If the node is a single node
            case DialogueNode.NodeType.single:

				// Update the textbox , name, and face
                textToDisplay = currentNode.dialogueText.Replace("[PLAYER]", "name");
                speakerName.text = currentNode.dialogueSpeaker.Replace("[PLAYER]", "name");
                topSpeakerName.text = speakerName.text;

                // If the node is auto, use the delay in its auto params
                if (currentNode.auto.isAuto)
                    currentDelay = currentNode.auto.autoSpeed;
                else
                    currentDelay = characterDelay;

                if (textPrint != null)
                    StopCoroutine(textPrint);

                Text target = useTopTextBox ? topTextBox : textBox;

                textPrint = PrintText(target);
                StartCoroutine(textPrint);

                if (useTopTextBox)
                {
                    HandleNPCFace(null);
                    textBox.transform.parent.gameObject.SetActive(false);
                    topTextBox.transform.parent.gameObject.SetActive(true);
                }
                else
                {
                    HandleNPCFace(currentNode.dialogueSprite);
                    textBox.transform.parent.gameObject.SetActive(true);
                    topTextBox.transform.parent.gameObject.SetActive(false);
                }

				// Enable the advance button so the dialogue can be advanced
                advanceButton.gameObject.SetActive(true);
				
				// Check to see if any functions should be called
                HandleDialogueFunctions();

				// Disable all buttons
                foreach (Button b in dialogueOptions)
                    b.gameObject.SetActive(false);

                break;


            case DialogueNode.NodeType.branch:
			
				// Shut off the advance button sense we have multiple choices here
                advanceButton.gameObject.SetActive(false);
				
				// Check to see if any functions should be called
                HandleDialogueFunctions();

				// Set the buttons to each child node of the branch
                for (int i=0; i<currentNode.childNodes.Count; i++)
                {
                    dialogueOptions[i].gameObject.SetActive(true);
                    dialogueOptionText[i].text = currentTree.GetNode(currentNode.childNodes[i].targetID).dialogueText.Replace("[PLAYER]", "name");
                }

                break;
        }
    } 


    /// <summary>
    /// Sets the NPC Face to either the node's NPC face, or disables it.
    /// </summary>
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
	

	/// <summary>
	/// Runs the dialogue functions of the given node
	/// </summary>
    private void HandleDialogueFunctions()
    {
		// Get a reference to the current active quest
        Quest q = GameManager.instance.GetActiveQuest();

        foreach (DialogueAction action in currentNode.actions)
        {
            switch (action.action)
            {
				// Rejects the quest
                case DialogueAction.Action.rejectQuest:
                    q.RejectQuest();
                    break;

				// Accepts the quest
                case DialogueAction.Action.acceptQuest:
                    q.AcceptQuest();
                    break;
				
				// Completes the quest. Often used by QuestItems
                case DialogueAction.Action.completeQuest:
                    q.CompleteQuest();
                    break;

				// Finishes the quest. Called at the end of a fail or win state. If it isn't, this will cause problems.
                case DialogueAction.Action.finishQuest:
                    q.FinishQuest();
                    break;

				// Increases friendship of the quest giver.
				// This value isn't saved yet, but THERE'S A REALLY COOL WAY TO DO IT SO DON'T SWEAT IT
                case DialogueAction.Action.affectFriendship:
                    q.AffectFriendship(int.Parse(action.param));
                    break;

                case DialogueAction.Action.failQuest:
                    q.FailQuest();
                    break;

				// Collects the current quest item
                case DialogueAction.Action.collectQuestItem:
                    q.CollectQuestItem();
                    break;

				// Destroys either the current quest item or a specific quest item. Be careful with the latter.
                case DialogueAction.Action.destroyQuestItem:
					
					if(action.param.Equals(""))
						q.DestroyQuestItem();
					else
						q.DestroyQuestItem(int.Parse(action.param));
					
                    break;

                // Destroys all quest items
                case DialogueAction.Action.destroyAllQuestItems:
                    q.DestroyAllQuestItems();
                    break;

				// Increases the maximum stamina of the player
                case DialogueAction.Action.increaseStamina:
                    GameManager.instance.IncreasePlayerStamina(float.Parse(action.param));
                    break;

                // Affect a social value
                case DialogueAction.Action.affectSocialValue:

                    string[] temp = action.param.Split(',');

                    // Check if there is an appropriate number of arguments
                    if (temp.Length == 2)
                    {
                        int val = int.Parse(temp[1]);
                        // The biggest problem right now is that there is no validation for SVs.
                        // This would work, but it runs the risk of people using slightly different names and throwing saving off.
                        // I'll probably add some kind of validation here which would remove social, scrub all non-letters, etc.
                        GameManager.instance.AffectSocialValue(temp[0], val);
                    }
                    else
                        Debug.LogError("Param Error: too many or few parameters!");
                    break;

                // Display a hint
                case DialogueAction.Action.showHint:
                    GameManager.instance.ShowHint();
                    break;
            }
        }
    }
	

	/// <summary>
	/// Advances to the next node. It also contains behavior for ending a dialogue
	/// </summary>
    public void Next()
    {
        questAudio.Play();
        // This button does nothing if the text is automatically being displayed.
        if (currentNode.auto.isAuto && !forceNext)
            return;

        // If the player attempts to advance while text is printing, abort the printing and return.
        if (isPrinting)
        {
            textBox.text = currentNode.dialogueText.Replace("[PLAYER]", "name");
            StopCoroutine(textPrint);
            isPrinting = false;
            return;
        }

        // Last one in the list
        if ((CountNumActiveChildren() == 0) || currentNode == null)
        {
			// Enables the player's movement, and allows the quest giver to be interacted with again
            dialogueUI.SetActive(false);
            GameManager.instance.EnablePlayerMovement();
            GameManager.instance.EndQuestFirstEncounter();
            GameManager.instance.ToggleQuestInteractivity(true);
            return;
        }

		// Get the active quest and its current state if they exist
        Quest currentQuest = GameManager.instance.GetActiveQuest();
        Quest.QuestState state = Quest.QuestState.inactive;

        if (currentQuest != null)
            state = currentQuest.GetCurrentState();

		// Which child do we advance to?
        foreach (DialogueNode.DialogueBranchCondition dbc in currentNode.childNodes)
        {
			// If the quest is cleared, use the child with that condition
            if (dbc.condition.Equals(DialogueNode.DialogueBranchCondition.Condition.cleared) && state.Equals(Quest.QuestState.completed))
            {
                currentNode = currentTree.GetNode(dbc.targetID);
                break;
            }
			
			// If the quest is failed, use the child with that condition
            else if (dbc.condition.Equals(DialogueNode.DialogueBranchCondition.Condition.failed) && state.Equals(Quest.QuestState.failed))
            {
                currentNode = currentTree.GetNode(dbc.targetID);
                break;
            }
			
			// If the quest is active, use the child with that condition
            else if (dbc.condition.Equals(DialogueNode.DialogueBranchCondition.Condition.active) && state.Equals(Quest.QuestState.active))
            {
                currentNode = currentTree.GetNode(dbc.targetID);
                break;
            }
			
			// Otherwise, just use the default condition. This one must always be last.
            else if (dbc.condition.Equals(DialogueNode.DialogueBranchCondition.Condition.none))
            {
                currentNode = currentTree.GetNode(dbc.targetID);
                break;
            }
        }

        firstLine = false;

		// Processes the node
        ProcessCurrentNode();

        forceNext = false;
    }


	/// <summary>
	/// Determines how many branches a node has given the active quest state
	/// </summary>
    private int CountNumActiveChildren()
    {
        int children = 0;

		// Get the active quest and its current state if they exist
        Quest currentQuest = GameManager.instance.GetActiveQuest();
        Quest.QuestState state = Quest.QuestState.inactive;

        if (currentQuest != null)
            state = currentQuest.GetCurrentState();

        foreach (DialogueNode.DialogueBranchCondition dbc in currentNode.childNodes)
        {
			// If the condition matches the current quest state, then it is a possible condition
            if ((dbc.condition.Equals(DialogueNode.DialogueBranchCondition.Condition.cleared) && state.Equals(Quest.QuestState.completed)) ||
                (dbc.condition.Equals(DialogueNode.DialogueBranchCondition.Condition.failed) && state.Equals(Quest.QuestState.failed))  ||
                (dbc.condition.Equals(DialogueNode.DialogueBranchCondition.Condition.active) && state.Equals(Quest.QuestState.active)) ||
                dbc.condition.Equals(DialogueNode.DialogueBranchCondition.Condition.none))
                children++;
        }

        return children;
    }

	/// <summary>
	/// Advances to the dialogue node matching a given dialogue branch option
	/// </summary>
    public void OptionSelected(int button)
    {
        currentNode = currentTree.GetNode(currentNode.childNodes[button].targetID);

        Next();
    }


    /// <summary>
    /// Prints each character in the text string after a delay
    /// </summary>
    private IEnumerator PrintText(Text target)
    {
        isPrinting = true;
        target.text = "";

        // Prints a character, then pauses for X seconds
        for(int i=0; i<textToDisplay.Length; i++)
        {
            target.text += textToDisplay[i];
            yield return new WaitForSeconds(currentDelay);
        }

        isPrinting = false;

        // If this is an auto node, jump to the next node after a delay
        if (currentNode.auto.isAuto)
            StartCoroutine(NextAfterDelay());
    }



    /// <summary>
    /// Advances to the next dialogue node after a delay.
    /// </summary>
    private IEnumerator NextAfterDelay()
    {
        yield return new WaitForSeconds(currentNode.auto.pauseBeforeNext);

        forceNext = true;
        Next();
    }
}
