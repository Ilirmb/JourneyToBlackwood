using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quicksand : MonoBehaviour
{
    public float sinkSpeed = 25;
    public float maxSinkSpeed = -10f; //Maximum negitave velocity when sinking
    private Rigidbody2D player;
    private float gravity;
    private float maxSpeed;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        gravity = player.gravityScale;
        maxSpeed = player.GetComponent<CustomPlatformerCharacter2D>().m_GroundedSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetType() == typeof(CapsuleCollider2D))
        {
            player.GetComponent<CustomPlatformerCharacter2D>().quicksand = true;
            player.gravityScale = 0;
            player.GetComponent<CustomPlatformerCharacter2D>().m_GroundedSpeed = .05f;
            player.velocity = new Vector2(player.velocity.x, .1f);
            player.GetComponent<CustomPlatformerCharacter2D>().ableToMove = true;
        }
        Debug.Log("Enter");
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetType() == typeof(CapsuleCollider2D))
        {
            player.GetComponent<CustomPlatformerCharacter2D>().quicksand = false;
            player.gravityScale = gravity;
            player.GetComponent<CustomPlatformerCharacter2D>().m_GroundedSpeed = maxSpeed;
            player.GetComponent<CustomPlatformerCharacter2D>().ableToMove = true;
            player.GetComponent<CustomPlatformerCharacter2D>().m_AirControl = true;
            player.AddForce(Vector2.up * 100f * Time.deltaTime);
        }
        Debug.Log("Exit");
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetType() == typeof(CapsuleCollider2D))
        {
            if (this.GetComponent<BoxCollider2D>().bounds.Contains(player.transform.GetChild(1).position))
            {
                player.GetComponent<PlayerStatistics>().damageStamina(10, 1f);
            }

            if (Input.GetAxis("Horizontal") != 0)
            {
                if (player.velocity.y >= (Input.GetKey(KeyCode.LeftShift) ? maxSinkSpeed * 10f : maxSinkSpeed))
                {
                    float tempHorizMultiplier = Mathf.Abs(Input.GetAxis("Horizontal"));
                    tempHorizMultiplier = (Input.GetKey(KeyCode.LeftShift) ? tempHorizMultiplier * 10f : tempHorizMultiplier); // If the player is holding the run button, multiply the rate you sink at
                    player.AddForce(Vector2.up * sinkSpeed * tempHorizMultiplier * Time.deltaTime * .2f);
                }
            }

            if (player.velocity.y > 0)
            {
                player.gravityScale = gravity;
            }
            else
            {
                player.gravityScale = 0;
            }

            player.AddForce(Vector2.down * sinkSpeed * Time.deltaTime);

            if (player.GetComponent<PlayerStatistics>().stamina <= 0)
            {
                player.GetComponent<CustomPlatformerCharacter2D>().quicksand = false;
                player.gravityScale = gravity;
                player.GetComponent<CustomPlatformerCharacter2D>().m_GroundedSpeed = maxSpeed;
                player.GetComponent<CustomPlatformerCharacter2D>().ableToMove = true;
                player.GetComponent<CustomPlatformerCharacter2D>().m_AirControl = true;
            }

        }
    }
}
