using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quicksand : MonoBehaviour
{
    public float sinkSpeed = 10;
    public float maxSinkSpeed = -1f; //Maximum negitave velocity when sinking
    private Rigidbody2D player;
    private Vector3 respawnPoint;
    private float gravity;
    private float maxSpeed;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        gravity = player.gravityScale;
        respawnPoint = this.transform.GetChild(0).position; //The index 0 child of the quicksand should be it's respawn point
        maxSpeed = player.GetComponent<CustomPlatformerCharacter2D>().m_MaxSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetType() == typeof(CircleCollider2D))
        {
            player.gravityScale = 0;
            player.GetComponent<CustomPlatformerCharacter2D>().m_MaxSpeed = maxSpeed / 2f;
            player.velocity = new Vector2(player.velocity.x, 0f);
            player.GetComponent<CustomPlatformerCharacter2D>().ableToMove = true;
        }
        Debug.Log("Enter");
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetType() == typeof(CircleCollider2D))
        {
            player.gravityScale = gravity;
            player.GetComponent<CustomPlatformerCharacter2D>().m_MaxSpeed = maxSpeed;
            player.GetComponent<CustomPlatformerCharacter2D>().ableToMove = true;
        }
        Debug.Log("Exit");
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetType() == typeof(CircleCollider2D))
        {
            if (this.GetComponent<BoxCollider2D>().bounds.Contains(player.transform.GetChild(1).position)) //GetChild(1)should be the ceiling check object aka the top of the players head
            {
                player.GetComponent<PlayerStatistics>().damageStamina(10, 1f);
                player.MovePosition(respawnPoint);
            }

            if (Input.GetKeyDown(KeyCode.Space) && !player.GetComponent<CustomPlatformerCharacter2D>().m_Grounded) // If they press space in quicksand and arent considered grounded
            {
                player.AddForce(Vector2.down * sinkSpeed * Time.deltaTime);
                player.GetComponent<PlayerStatistics>().damageFromJump(GameConst.STAMINA_TO_JUMP);
            }

            if (Input.GetAxis("Horizontal") != 0)
            {
                if (player.velocity.y >= (Input.GetKey(KeyCode.LeftShift) ? maxSinkSpeed * 10f : maxSinkSpeed))
                {
                    float tempHorizMultiplier = Mathf.Abs(Input.GetAxis("Horizontal"));
                    tempHorizMultiplier = (Input.GetKey(KeyCode.LeftShift) ? tempHorizMultiplier * 10f : tempHorizMultiplier); // If the player is holding the run button, multiply the rate you sink at
                    player.AddForce(Vector2.down * sinkSpeed * tempHorizMultiplier * Time.deltaTime * .2f);
                }
            }
            else
            {
                if (player.velocity.y < 0) player.velocity = new Vector2(player.velocity.x, 0f);
                player.AddForce(Vector2.up * sinkSpeed * Time.deltaTime * .75f);
            }
        }

    }
}
