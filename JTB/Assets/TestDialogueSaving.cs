using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogueSaving : MonoBehaviour
{
    public DialogueTree testtree1, testtree2;

    private void OnMouseUp()
    {
        GameManager manager = GameManager.instance;
        if (!manager.CheckQuestFlag("Spoke"))
        {
            DialogueProcessor.instance.StartDialogue(testtree1);
            manager.AddQuestFlag("Spoke");
        }
        else
        {
            DialogueProcessor.instance.StartDialogue(testtree2);
        }
    }
}
