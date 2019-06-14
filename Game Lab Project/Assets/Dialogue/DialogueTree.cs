using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewTree", menuName = "Dialogue/Dialogue Tree", order = 2)]
public class DialogueTree : ScriptableObject {

    public DialogueNode parent;

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

    public List<int> childNodeIDs;

    public void OnActivate() { }
    public NodeType GetNodeType() { return dialogueNodeType; }
}
