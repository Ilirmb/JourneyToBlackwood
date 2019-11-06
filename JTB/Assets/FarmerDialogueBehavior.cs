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
        Debug.Log("Entered collider");
        if (other.CompareTag("Player"))
            Debug.Log("and is player");
        {
            if (!hasTriggered)
            {
                Debug.Log("and hasnt triggered");
                hasTriggered = true;
                dialogueProcesor.StartDialogue(dialogueTree);
            }
        }
    }
}
