using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public bool onLadder;
    public float climbSpeed = 400;
    public float drag = 1;
    private Rigidbody2D player;
    private float gravity;
    private float attachDowntime;
    private bool canHoldToAttach;


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        gravity = player.gravityScale; //Save this so we can go between no gravity and full gravity
        canHoldToAttach = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canHoldToAttach = true;
            if (onLadder)
                getOffLadder(.75f); //time in seconds that the player can't get back on the ladder after they hop off
        }
    }


    void getOnLadder()
    {
        if (attachDowntime <= 0)
        {
            onLadder = true;
            this.gameObject.layer = 8; //8 is ground
            player.gravityScale = 0;
            player.drag = drag;
            player.transform.position = new Vector2(this.gameObject.transform.position.x, player.transform.position.y);
            player.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }


    void getOffLadder()
    {
        onLadder = false;
        this.gameObject.layer = 0; //0 is default
        player.gravityScale = gravity;
        player.drag = 0;
        player.constraints = RigidbodyConstraints2D.FreezeRotation;
    }


    void getOffLadder(float downtime)
    {
        onLadder = false;
        this.gameObject.layer = 0; //0 is default
        player.gravityScale = gravity;
        player.drag = 0;
        player.constraints = RigidbodyConstraints2D.FreezeRotation;
        attachDowntime = downtime;
    }


    // Called similar to update function
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (canHoldToAttach) //If it's the first time the player is entering the collider, they're allowed to attach to it by holding down the button.
            {
                if (Input.GetAxisRaw("Vertical") > 0 && !onLadder) getOnLadder();
                canHoldToAttach = false;
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") > 0 && !onLadder) getOnLadder();
            }

            if (Input.GetKeyDown(KeyCode.Space) && onLadder)
            {
                getOffLadder(.5f);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (onLadder)
        {
            player.velocity = new Vector2(player.velocity.x, Input.GetAxis("Vertical") * climbSpeed * Time.deltaTime);
        }
    }

    private void Update()
    {
        if (attachDowntime > 0)
            attachDowntime -= Time.deltaTime;
    }
}
