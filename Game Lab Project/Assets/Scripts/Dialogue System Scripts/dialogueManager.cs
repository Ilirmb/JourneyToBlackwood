using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class dialogueTree
{
    private List<nodeTextLine> treeMember = new List<nodeTextLine>();
    private nodeEvent managerEvent;
    int currentParentIndex = 1; int currentChildIndex = 1;

    public dialogueTree(string fullDialogueText, nodeEvent nodeEvent, Sprite defaultSprite)
    {
        managerEvent = nodeEvent;
        string[] dialogueLines = fullDialogueText.Split('\n');
        string[] encoding = dialogueLines[0].Split('-');
        nodeTextLine currentParentNode;
        //Because of the extra encoding data line we offset the list by adding a blank constructor so the index of the object in the list is the same as the index of the line in the raw text file
        treeMember.Add(new nodeTextLine());
        //We also have to add the first real node because the code works by making the correct amount of children for each node and it needs a parent node to start on
        //We don't have to worry about creating it's children yet because it starts on this object and makes children from there
        treeMember.Add(new nodeTextLine(dialogueLines[currentChildIndex], currentChildIndex++, defaultSprite));
        //Encoding[0] is the encoded tree structure. First we take the layers
        foreach (string treeLayer in encoding[0].Split('|'))
        {
            //Then for each layer we split it again to get the individual nodes
            foreach (string sNumChildren in treeLayer.Split(':'))
            {
                currentParentNode = treeMember[currentParentIndex++];
                //The number tells us how many children to make, so we parse the string into an int and make children
                for (int numChildren = int.Parse(sNumChildren); numChildren > 0; numChildren--)
                {
                    treeMember.Add(new nodeTextLine(dialogueLines[currentChildIndex], currentChildIndex, defaultSprite));
                    currentParentNode.addChild(treeMember[currentChildIndex++]);
                }
                currentParentNode.setNumButtons(currentParentNode.getChildCount());
                //Prints out every member in order with children indexes, very useful for debugging
                Debug.Log(currentParentNode);
            }
        }
        //Encoding[1] has the nodes to add events to
        if (encoding.Length > 1)
        {
            foreach (string nodeWithEvent in encoding[1].Split(':'))
            {
                getNodeAtIndex(int.Parse(nodeWithEvent)).setEvent(managerEvent);
            }
        }
    }

    public nodeTextLine getNodeAtIndex(int index)
    {
        //Debug.Log(index);
        return treeMember[index];
    }
}

public class nodeTextLine
{
    //Node content
    private string line;
    private Sprite spriteToDisplay;
    //Node metacontent
    private int treeIndex;
    private List<nodeTextLine> children = new List<nodeTextLine>();
    //Whether this node should use button selection or rely on an outside source to choose a child to follow
    //By default this is -1 which will use button selection, any other value will be the value of the child
    [Range(-1, 3)] private int overrideButtonSelection = -1;
    [Range(0, 4)] private int numButtons;

    private nodeEvent triggeredEvent = new nodeEvent();

    public nodeTextLine()
    {
        line = "";
        treeIndex = 0;
        spriteToDisplay = null;
        numButtons = 0;
    }
    public nodeTextLine(string text, int index, Sprite defaultSprite)
    {
        line = text;
        treeIndex = index;
        triggeredEvent = null;
        spriteToDisplay = defaultSprite;
    }

    public void addChild(nodeTextLine child)
    {
        children.Add(child);
    }

    public void setNumButtons(int num)
    {
        numButtons = num;
    }
    public void setEvent(nodeEvent managerEvent)
    {
        triggeredEvent = managerEvent;
    }
    public void setLine(string newLine)
    {
        line = newLine;
    }
    public void setNextChild(int index)
    {
        overrideButtonSelection = index;
    }
    public void setSprite(Sprite sprite)
    {
        spriteToDisplay = sprite;
    }
    public int getNextChild()
    {
        return overrideButtonSelection;
    }
    public nodeEvent getEvent()
    {
        return triggeredEvent;
    }
    public string getLine()
    {
        return line;
    }
    public Sprite getSprite()
    {
        return spriteToDisplay;
    }
    public int getNumButtons()
    {
        return numButtons;
    }
    public int getIndex()
    {
        return treeIndex;
    }
    public int getChildCount()
    {
        return children.Count;
    }
    public nodeTextLine getChild(int index)
    {
        return children[index];
    }

    //This is called to send tell any listeners that the dialogue tree reached this point, which they'll know by the index of the node
    public void triggerEvent()
    {
        //Debug.Log("The dialogue event triggered! Index of " + treeIndex);
        triggeredEvent.Invoke(treeIndex);
    }

    public override string ToString()
    {
        string childrenString;
        switch (children.Count)
        {
            case 0:
                childrenString = "No Children";
                break;
            case 1:
                childrenString = "1 child at index: " + this.children[0].getIndex();
                break;
            case 2:
                childrenString = "child 1 at index: " + this.children[0].getIndex() + "| child 2 at index: " + this.children[1].getIndex();
                break;
            case 3:
                childrenString = "child 1 at index: " + this.children[0].getIndex() + "| child 2 at index: " + this.children[1].getIndex() + "| child 3 at index: " + this.children[2].getIndex();
                break;
            case 4:
                childrenString = "child 1 at index: " + this.children[0].getIndex() + "| child 2 at index: " + this.children[1].getIndex() + "| child 3 at index: " + this.children[2].getIndex() + "| child 4 at index: " + this.children[3].getIndex();
                break;
            default:
                childrenString = "";
                break;
        }
        return "Index: " + treeIndex + "\nNumber of children/buttons: " + numButtons +  "\n" + childrenString + "\n raw line: " + line;
    }
}

[System.Serializable]
public class nodeEvent : UnityEvent<int> { }
public class dialogueManager : MonoBehaviour
{

    public dialogueTree dialogueTree;
    public TextAsset dialogueTextAsset;
    public nodeTextLine currentNode;
    public UnityEngine.UI.Text textBox;
    public disableButtonChildren currentButtonLayout;
    //public Sprite playerSprite; // Currently obselete, may be useful later
    public Image NPCFaceRenderer;

    public nodeEvent dialogueEvent;
    public GameObject player;
    //Sometimes we need to disable and enable the trigger that activated this script within this script itself, so we have a variable that lets fuction calls pass their colliders in so we can disable them here
    public Collider2D triggeredThis;

    //For starting a new dialogue with the already existing dialogueTree and NPCsprite
    public void startDialogue(Collider2D triggerToDisable)
    {
        triggeredThis = triggerToDisable;
        triggeredThis.enabled = false;
        gameObject.GetComponent<Canvas>().enabled = true;
        gameObject.GetComponent<GraphicRaycaster>().enabled = true;

        //Prevent the player from moving while they're in dialogue
        player.GetComponent<CustomPlatformer2DUserControl>().canControl = false;
        player.GetComponent<Animator>().SetFloat("Speed", 0f);
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        currentNode = dialogueTree.getNodeAtIndex(1);
        currentButtonLayout = null;
        displayText(currentNode.getLine());
    }
    //Start a dialogue with the existing tree and without disabling anything.
    //This is mostly used for colliders, where we still want them to have collision but we don't want it to stop interacting entirely
    public void startDialogue()
    {
        gameObject.GetComponent<Canvas>().enabled = true;
        gameObject.GetComponent<GraphicRaycaster>().enabled = true;
        //Prevent the player from moving while they're in dialogue
        player.GetComponent<CustomPlatformer2DUserControl>().canControl = false;
        player.GetComponent<Animator>().SetFloat("Speed", 0f);
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        currentNode = dialogueTree.getNodeAtIndex(1);
        currentButtonLayout = null;
        displayText(currentNode.getLine());
    }
    //Start a new dialogue with a new textasset
    public void startDialogue(TextAsset dialogue, Sprite NPCSprite)
    {
        gameObject.GetComponent<Canvas>().enabled = true;
        gameObject.GetComponent<GraphicRaycaster>().enabled = true;

        //Prevent the player from moving while they're in dialogue
        player.GetComponent<CustomPlatformer2DUserControl>().canControl = false;
        player.GetComponent<Animator>().SetFloat("Speed", 0f);
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        dialogueTextAsset = dialogue;
        dialogueTree = new dialogueTree(dialogue.text, dialogueEvent, NPCSprite);
        currentNode = dialogueTree.getNodeAtIndex(1);
        currentButtonLayout = null;
        displayText(currentNode.getLine());
    }
    public void startDialogue(TextAsset dialogue, Collider2D triggerToDisable, Sprite NPCSprite)
    {
        triggeredThis = triggerToDisable;
        triggeredThis.enabled = false;
        gameObject.GetComponent<Canvas>().enabled = true;
        gameObject.GetComponent<GraphicRaycaster>().enabled = true;

        //Prevent the player from moving while they're in dialogue
        player.GetComponent<CustomPlatformer2DUserControl>().canControl = false;
        player.GetComponent<Animator>().SetFloat("Speed", 0f);
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        dialogueTextAsset = dialogue;
        dialogueTree = new dialogueTree(dialogue.text, dialogueEvent, NPCSprite);
        currentNode = dialogueTree.getNodeAtIndex(1);
        currentButtonLayout = null;
        displayText(currentNode.getLine());
    }

    public void endDialogue()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        gameObject.GetComponent<GraphicRaycaster>().enabled = false;
        player.GetComponent<CustomPlatformer2DUserControl>().canControl = true;
        //The bool will be set to false and it'll move on immediately if the first half is false,
        //so we can both check for triggerThis's existance and it's properties in the same line
        if (triggeredThis != null && !triggeredThis.enabled)
        {
            triggeredThis.enabled = true;
        }
    }

    public void addEventToNode(int nodeIndex)
    {
        //Node event is an instance of a class that extends UnityEvent<int> that is also serializable, meaning it's visible on the inspector view
        //Any node that triggers the event usually some time later sends with it their own index in the treeMember List<nodeTextLine> so the unique script's event handler can know what to do at each given event
        dialogueTree.getNodeAtIndex(nodeIndex).setEvent(dialogueEvent);
    }

    public void makeDialogueTree(TextAsset dialogue, Sprite defaultSprite)
    {
        dialogueTextAsset = dialogue;
        dialogueTree = new dialogueTree(dialogue.text, dialogueEvent, defaultSprite);
    }


    public void OnButtonPressed(int buttonID)
    {
        if (currentNode.getNextChild() == -1)
        {
            if (currentNode.getEvent() != null)
            {
                currentNode.triggerEvent();
            }
            if (currentNode.getChildCount() != 0)
            {
                currentNode = currentNode.getChild(buttonID);
                displayText(currentNode.getLine());
            }
            else
            {
                endDialogue();
            }
        }
        else
        {
            if (currentNode.getEvent() != null)
            {
                currentNode.triggerEvent();
            }
            currentNode = currentNode.getChild(currentNode.getNextChild());
            displayText(currentNode.getLine());
        }
    }


    public void displayText(string unsplitLine)
    {
        NPCFaceRenderer.sprite = currentNode.getSprite();

        string[] dialogue = unsplitLine.Split('|');
        textBox.text = dialogue[0];
        
        if (currentButtonLayout != null)
        {
            currentButtonLayout.disableChildren();
        }

        if (currentNode.getNumButtons() != 0 && currentNode.getNextChild() == -1)
        {
            //Well so much for elegance. To get the buttons to spawn on top of the text box and other things they need to be below them in the heirarcy
            //Before chilcount - 1 was pretty perfect, but now we use childcount + 2 because there are 3 more objects on top of the button layouts
            currentButtonLayout = transform.GetChild(currentNode.getNumButtons() + 2).gameObject.GetComponent<disableButtonChildren>();
        }
        else
        {
            currentButtonLayout = transform.GetChild(3).gameObject.GetComponent<disableButtonChildren>();
        }

        //didn't use foreach here because (reason)
        if (currentNode.getNextChild() == -1)
        {
            if (dialogue.Length > 1)
            {
                //didn't use foreach here because (reason)
                for (int i = 0; i < currentNode.getChildCount() && i + 1 < dialogue.Length; i++)
                {
                    currentButtonLayout.transform.GetChild(i).GetComponentInChildren<Text>().text = dialogue[i + 1];
                }
            }
            else
            {
                currentButtonLayout.transform.GetChild(0).GetComponentInChildren<Text>().text = "";
            }
        }
        else
        {
            if (dialogue.Length > 1)
            {
                currentButtonLayout.transform.GetChild(0).GetComponentInChildren<Text>().text = dialogue[1];
            }
            else
            {
                currentButtonLayout.transform.GetChild(0).GetComponentInChildren<Text>().text = "";
            }
        }
        currentButtonLayout.enableChidlren();
    }

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find(GameConst.PLAYER_OBJECT_NAME);
        gameObject.GetComponent<Canvas>().enabled = false;
        gameObject.GetComponent<GraphicRaycaster>().enabled = false;
        NPCFaceRenderer = GameObject.Find("NPC Face").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
