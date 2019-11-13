using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDialogueTrigger : MonoBehaviour
{
    public DialogueTree hintTree;

    public bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            DialogueProcessor.instance.StartDialogue(hintTree, true);
        }
    }
}
