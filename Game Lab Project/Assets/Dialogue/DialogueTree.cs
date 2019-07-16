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

    private List<int> idsInUse = new List<int>();

    [SerializeField]
    private DialogueNode firstNode;

    /// <summary>
    /// Deletes all nodes and resets the first node.
    /// </summary>
    public override void Clear()
    {
        dialogue.Clear();
        firstNode = null;

        base.Clear();
    }


    /// <summary>
    /// Adds a node to the graph
    /// </summary>
    /// <param name="type">The type of node that is added</param>
    /// <returns>The node that is added</returns>
    public override Node AddNode(Type type)
    {
        CleanList();

        DialogueNode newNode = (DialogueNode)base.AddNode(type);
        newNode.SetID(GenerateID());

        dialogue.Add(newNode);
        idsInUse.Add(newNode.GetID());

        CheckFirstNode();

        return newNode;
    }


    /// <summary>
    /// Creates a copy of the given node
    /// </summary>
    /// <param name="original">The node that is to be copied</param>
    /// <returns>The node that is added</returns>
    public override Node CopyNode(Node original)
    {
        DialogueNode copy = (DialogueNode)(base.CopyNode(original));
        copy.SetID(GenerateID());

        if (copy.GetFirstNode())
            copy.SetFirstNode(false);

        return copy;
    }


    /// <summary>
    /// Removes the given node from the graph
    /// </summary>
    /// <param name="node">The node to be removed</param>
    public override void RemoveNode(Node node)
    {
        if (firstNode == ((DialogueNode)node))
            firstNode = null;

        idsInUse.Remove(((DialogueNode)node).GetID());
        dialogue.Remove((DialogueNode)node);

        CheckFirstNode();

        base.RemoveNode(node);
    }



    /// <summary>
    /// Removes all null nodes from the list
    /// </summary>
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


    /// <summary>
    /// We need to make sure the list of dialogue nodes are accessible
    /// </summary>
    private void OnEnable()
    {
        UpdateList();
    }


    /// <summary>
    /// Populates dialogue list with the nodes.
    /// </summary>
    private void UpdateList()
    {
        dialogue = new List<DialogueNode>();
        idsInUse = new List<int>();

        List<Node> reverse = nodes;
        reverse.Reverse();

        foreach (Node n in reverse)
        {
            dialogue.Add((DialogueNode)n);
            idsInUse.Add(((DialogueNode)n).GetID());
        }

        CheckFirstNode();
    }


    public void ValidateNode(DialogueNode dn)
    {
        if (idsInUse.Count < nodes.Count)
            return;

        int numIDs = 0;

        foreach (DialogueNode.DialogueBranchCondition dbc in dn.childNodes)
        {
            if (!idsInUse.Contains(dbc.targetID))
            {
                dn.dialogueNodeType = DialogueNode.NodeType.error;
                break;
            }

            numIDs++;
        }

        if (numIDs == dn.childNodes.Count && dn.dialogueNodeType.Equals(DialogueNode.NodeType.error))
        {
            if (dn.dialogueText.Equals(""))
                dn.dialogueNodeType = DialogueNode.NodeType.branch;
            else
                dn.dialogueNodeType = DialogueNode.NodeType.single;
        }
    }


    /// <summary>
    /// Sets the first node in the list to the first node for dialogue
    /// </summary>
    private void CheckFirstNode()
    {
        if (firstNode == null && dialogue.Count > 0)
            firstNode = dialogue[0];
    }


    /// <summary>
    /// Sets the first node of the tree
    /// </summary>
    /// <param name="dn">The new first node</param>
    public void SetFirstNode(DialogueNode dn)
    {
        firstNode.SetFirstNode(false);
        dn.SetFirstNode(true);

        firstNode = dn;
    }


    /// <summary>
    /// Returns the first node of the tree
    /// </summary>
    /// <returns></returns>
    public DialogueNode GetFirstNode()
    {
        return firstNode;
    }


    /// <summary>
    /// Returns a node by its ID
    /// </summary>
    /// <param name="ID">ID of the node to return</param>
    /// <returns></returns>
    public DialogueNode GetNode(int ID)
    {
        DialogueNode result = dialogue.Find(d => d.GetID() == ID);

        return result;
    }


    /// <summary>
    /// Returns a new unique ID for this node.
    /// </summary>
    /// <returns>A new ID</returns>
    private int GenerateID()
    {
        if (dialogue.Count == 0)
            return 0;

        List<int> idsSorted = idsInUse;

        idsSorted.Sort();

        for(int i=0; i<idsSorted.Count; i++)
        {
            if (!idsSorted.Contains(i))
                return i;
        }

        return idsSorted[idsSorted.Count - 1] + 1;
    }
}
