using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleSlip : MonoBehaviour
{
    public CustomPlatformerCharacter2D player;
    public bool enableSlip;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CustomPlatformerCharacter2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggrd");
        if(other.CompareTag("Player"))
        {
            if (enableSlip)
            {
                player.StartSliding();
            }
            else
            {
                player.StopSliding();
            }
        }
    }
}
