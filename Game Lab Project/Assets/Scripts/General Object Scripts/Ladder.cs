using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public bool onLadder;
    public float climbSpeed = 400;
    public float drag = 1;
    private static CustomPlatformerCharacter2D player;
    private static Rigidbody2D playerRigidbody;
    private float gravity;
    private float attachDowntime;
    private bool canHoldToAttach;


    // Use this for initialization
    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<CustomPlatformerCharacter2D>();
            playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        }

        gravity = player.GetGravityScale(); //Save this so we can go between no gravity and full gravity
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
            player.SetOnLadder(onLadder);

            this.gameObject.layer = 8; //8 is ground
            player.SetGravityScale(0);
            playerRigidbody.drag = drag;
            playerRigidbody.transform.position = new Vector2(this.gameObject.transform.position.x, playerRigidbody.transform.position.y);
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }


    void getOffLadder()
    {
        onLadder = false;

        this.gameObject.layer = 0; //0 is default
        player.SetGravityScale(gravity);
        playerRigidbody.drag = 0;
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }


    void getOffLadder(float downtime)
    {
        getOffLadder();

        attachDowntime = downtime;

        if ((player.vspeed < -0.5f && playerRigidbody.velocity.y == 0.0f && onLadder))
            player.SetOnLadder(false);
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

            if ((!player.GetOnLadder() && onLadder) || (player.vspeed < -0.5f && playerRigidbody.velocity.y == 0.0f && onLadder))
            {
                Debug.Log("Glitch");
                getOffLadder(.5f);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (onLadder)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, Input.GetAxis("Vertical") * climbSpeed * Time.deltaTime);
        }
    }

    private void Update()
    {
        if (attachDowntime > 0)
            attachDowntime -= Time.deltaTime;
    }
}
