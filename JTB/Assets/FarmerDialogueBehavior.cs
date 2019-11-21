using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerDialogueBehavior : MonoBehaviour
{
    public DialogueTree dialogueTree;
    public DialogueProcessor dialogueProcesor;

    private bool hasTriggered = false;

    private void Start()
    {
        hasTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasTriggered)
            {
                hasTriggered = true;
                dialogueProcesor.StartDialogue(dialogueTree);
            }
        }
    }
}
