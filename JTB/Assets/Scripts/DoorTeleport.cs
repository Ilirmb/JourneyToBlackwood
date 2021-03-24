using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorTeleport : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    public GameObject exit;
    public Text text;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        text.enabled = false;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        text.enabled = true;
        text.text = "Click 'Mouse1' To use the door.";

        if(collision.gameObject.tag == "Player" && Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Teleport());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        text.text = " ";
        text.enabled = false;
    }

    IEnumerator Teleport()
    {
            player.transform.position = new Vector2(exit.transform.position.x, exit.transform.position.y);
            return null;
    }
}