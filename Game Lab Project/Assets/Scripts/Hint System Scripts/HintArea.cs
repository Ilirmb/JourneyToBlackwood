using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes the list of available hints to the provided list if the player enters this area.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class HintArea : MonoBehaviour {

    [SerializeField]
    private List<DialogueTree> hints = new List<DialogueTree>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            GameManager.instance.UpdateHintList(hints);
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            GameManager.instance.UpdateHintList(new List<DialogueTree>());
    }

}
