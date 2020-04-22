using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
    [SerializeField]
    private GameObject exit;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(Teleport());
        }
    }

    IEnumerator Teleport()
    {
        player.transform.position = new Vector2(exit.transform.position.x, exit.transform.position.y);
        return null;
    }
}