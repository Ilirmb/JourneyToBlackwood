using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


/// <summary>
/// Represents a node in a dialogue tree. This could be either a series of options, or standard text 
/// </summary>
public class DialogueNode : Node
{
    /// <summary>
    /// A class which represents a condition that when met, will advance to the ID of the given node.
    /// Kind of awkward to explain, but simple in application.
    /// </summary>
    [System.Serializable]
    public class DialogueBranchCondition
    {
        // The various conditions. Note that these act as branches in one dialogue interaction.
        // Conditions should only be used when appropriate.
        // Correct usage: Wizard says different dialogue if the player takes or eats the apple before talking with him for the first time.
        // Incorrect usage: Using the cleared condition for all nodes on a quest cleared tree.

        // None is your default.
        // Cleared should be used for dialogue that should only play if a quest is cleared.
        // Failed should be used for dialogue that should only play if a quest is failed.
        // Active should be used for dialogue that should only play if a quest is active.
        public enum Condition { none, cleared, failed, active, error };

        public Condition condition;
        public int targetID = -1;
    }

    [SerializeField]
    private int ID = -1;
    private bool isFirstNode = false;

    [Input]
    public DialogueNode previous;

    // The type of node. Branch nodes should have no info except for children. The error node should never be used.
    public enum NodeType { single, branch, error };

    public NodeType dialogueNodeType = NodeType.single;

    // Name of the speaker. This name can be anything.
    public string dialogueSpeaker;
    // What the speaker is saying
    public string dialogueText;
    // Picture of the speaker
    public Sprite dialogueSprite;

    // List of child nodes and what conditions must be met in order to access them.
    [Output(dynamicPortList = true)]
    public List<DialogueBranchCondition> childNodes = new List<DialogueBranchCondition>();

    // List of functions to call when the this dialogue node displays.
    public List<DialogueAction> actions = new List<DialogueAction>();
    public NodeType GetNodeType() { return dialogueNodeType; }


    public int GetID() { return ID; }
    public void SetID(int val) { ID = val; }


    public override object GetValue(NodePort port)
    {
        return ID;
    }


    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        if (from.ConnectionCount > 1)
        {
            from.ClearConnections();
            from.Connect(to);
        }

        // This gets the port index
        string indexName = from.fieldName.Replace("childNodes ", "");
        int index = int.Parse(indexName);

        // Sets the target ID in the child node
        ((DialogueNode)from.node).childNodes[index].targetID = ((DialogueNode)to.node).GetID();

        base.OnCreateConnection(from, to);
    }


    public void SetFirstNode()
    {
        ((DialogueTree)graph).SetFirstNode(this);
    }


    public void SetFirstNode(bool first) { isFirstNode = first; }


    public bool GetFirstNode() { return isFirstNode; }
}



/// <summary>
/// A class which represents an action (or function) to be called
/// </summary>
[System.Serializable]
public class DialogueAction
{
    // All possible actions:
    // rejectQuest rejects the current quest (ex: telling Wizard you are busy and can't help. This still allows the quest to be completed)
    // acceptQuest accepts the current quest
    // completeQuest completes the current quest
    // collectQuestItem collects the current quest item. The param can be used to target a specific quest item instead.
    // affectFriendship changes the friendship value of the quest giver by an amount given as a param
    // increaseStamina increases the player's max stamina by the amount given as a param
    // finishQuest finishes the quest and prevents it from being interacted with again. Call this for the last node in quest success or failed trees.
    // destroyAllQuestItems removes all quest items related to a current quest from the field.
    public enum Action { rejectQuest, acceptQuest, completeQuest, collectQuestItem, destroyQuestItem, affectFriendship, increaseStamina, finishQuest, destroyAllQuestItems };
    public Action action;

    // Parameter of the function. This is optional in most cases, but is required for a few functions.
    public string param;
}
