using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustrationDialogue : MonoBehaviour
{
    PlayerStatistics player;
    string line;
    DialogueManager manager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatistics>();
    }

    // Update is called once per frame
    void Update()
    {
        // checks player frustration level
        if (player.frustration >= 100f)
        {
            
            Debug.Log("waiting to show dialogue");
            new WaitForSeconds(30);
            Debug.Log("Showing Dialogue");
            manager.startDialogue();
        }
    }
}
