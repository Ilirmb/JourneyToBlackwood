using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewTree", menuName = "Dialogue/Dialogue Tree", order = 2)]
public class DialogueTree : ScriptableObject {

    public List <DialogueNode> dialogue;

    private void OnEnable()
    {

    }

}


/// <summary>
/// Represents a node in a dialogue tree. This could be either a series of options, or standard text 
/// </summary>
[System.Serializable]
public class DialogueNode
{
    public enum NodeType { single, branch, error };

    public NodeType dialogueNodeType = NodeType.error;
    public string dialogueSpeaker;
    public string dialogueText;
    public Sprite dialogueSprite;

    public List<DialogueBranchCondition> childNodeIDs;

    public List<DialogueAction> actions;

    public void OnActivate() { }
    public NodeType GetNodeType() { return dialogueNodeType; }
}


[System.Serializable]
public class DialogueBranchCondition
{
    public enum Condition { none, cleared, failed, active };
    public Condition condition;
    public int targetID;
}


[System.Serializable]
public class DialogueAction
{
    public enum Action { rejectQuest, acceptQuest, completeQuest, collectQuestItem, destroyQuestItem, affectFriendship, increaseStamina, finishQuest };
    public Action action;
    public string param;
}
