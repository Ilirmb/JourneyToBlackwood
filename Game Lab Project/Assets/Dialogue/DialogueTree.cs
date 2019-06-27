using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

/// <IMPORTANT>
/// If additional values are added to any enum in any of these classes, DO NOT, I repeat:
/// DO. NOT. put the new value ANYWHERE except at the END of the enum.
/// If the order of the values is EVER changed outside of appending new values to the end
/// ALL EXISTING DIALOGUE TREES WILL BREAK.
/// PLEASE DO NOT DO THIS.
/// https://www.youtube.com/watch?v=nEffPICj7kM
/// </IMPORTANT>

/// <summary>
/// A tree of dialogue nodes, or in simpler terms, a conversation.
/// Each node in the tree has an index, which also doubles as its ID.
/// </summary>
[CreateAssetMenu(fileName = "NewTree", menuName = "Dialogue/Dialogue Tree", order = 2)]
[System.Serializable]
public class DialogueTree : NodeGraph {

    [HideInInspector]
    public List<DialogueNode> dialogue = new List<DialogueNode>();

    [SerializeField]
    private DialogueNode firstNode;

    public override void Clear()
    {
        dialogue.Clear();

        base.Clear();
    }


    public override Node AddNode(Type type)
    {
        CleanList();

        DialogueNode newNode = (DialogueNode)base.AddNode(type);
        newNode.SetID(dialogue.Count);

        dialogue.Add(newNode);

        CheckFirstNode();

        return newNode;
    }


    public override Node CopyNode(Node original)
    {
        ((DialogueNode)original).SetID(dialogue.Count);
        dialogue.Add((DialogueNode)original);

        return base.CopyNode(original);
    }


    public override void RemoveNode(Node node)
    {
        int index = dialogue.IndexOf((DialogueNode)node);

        if (firstNode == ((DialogueNode)node))
            firstNode = null;

        dialogue.Remove((DialogueNode)node);

        foreach(DialogueNode dn in dialogue)
        {
            if (dn.GetID() >= index)
                dn.SetID(dn.GetID() - 1);

            foreach(DialogueNode.DialogueBranchCondition dbc in dn.childNodes)
            {
                if (dbc.targetID == index)
                    dbc.condition = DialogueNode.DialogueBranchCondition.Condition.error;
                else if (dbc.targetID > index)
                    dbc.targetID = dbc.targetID - 1;
            }
        }

        CheckFirstNode();

        base.RemoveNode(node);
    }


    private void CleanList()
    {
        for(int i=0; i<dialogue.Count; i++)
        {
            if (dialogue[i] == null)
            {
                dialogue.RemoveAt(i);
                i--;
            }
        }
    }


    private void OnEnable()
    {
        UpdateList();
    }


    private void UpdateList()
    {
        dialogue = new List<DialogueNode>();

        List<Node> reverse = nodes;
        reverse.Reverse();

        foreach (Node n in reverse)
        {
            dialogue.Add((DialogueNode)n);
        }

        /*for (int i = 0; i < dialogue.Count; i++)
        {
            dialogue[i].SetID(i);
        }*/

        CheckFirstNode();
    }


    private void CheckFirstNode()
    {
        if (firstNode == null && dialogue.Count > 0)
            firstNode = dialogue[0];
    }


    public void SetFirstNode(DialogueNode dn)
    {
        firstNode.SetFirstNode(false);
        dn.SetFirstNode(true);

        firstNode = dn;
    }


    public DialogueNode GetFirstNode()
    {
        return firstNode;
    }


    public DialogueNode GetNode(int ID)
    {
        DialogueNode result = dialogue.Find(d => d.GetID() == ID);

        return result;
    }
}
