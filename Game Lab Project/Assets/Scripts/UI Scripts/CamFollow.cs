using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {

    Camera cam;
    Transform player;
    [SerializeField] float offsetY = 2.5f;
    [SerializeField] float movementSpeed = 0.2f;

    [SerializeField]
    float horizontalDeadZoneLength = 0.04f;
    [SerializeField]
    float verticalDeadZoneLength = 0.01f;
    [SerializeField]
    float verticalDeadZoneCenter = 0.25f;

    // Used to determine when the player starts moving
    private CustomPlatformerCharacter2D playerMov;
    private Rigidbody2D playerRigidbody;

    //private Vector3 initialPos;
    private Vector3 velocity = Vector3.zero;

    private bool trackPlayerFall = false;


    void Start ()
    {
        cam = GetComponent<Camera>();
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerMov = player.gameObject.GetComponent<CustomPlatformerCharacter2D>();
        playerRigidbody = player.gameObject.GetComponent<Rigidbody2D>();
        
        Vector3 newPos = new Vector3(player.position.x, player.position.y + offsetY, -10f);
        gameObject.transform.position = newPos;
    }
	
    // LateUpdate fires after all other update functions
	void LateUpdate ()
    {
        //gameObject.transform.position = newPos;

        Vector3 playerPos = cam.WorldToViewportPoint(player.position);

        if(playerPos.x > (0.5f + horizontalDeadZoneLength / 2.0f) || playerPos.x < (0.5f - horizontalDeadZoneLength / 2.0f) ||
            playerPos.y > (verticalDeadZoneCenter + verticalDeadZoneLength / 2.0f) || playerPos.y < (verticalDeadZoneCenter - verticalDeadZoneLength / 2.0f))
        {
            Vector3 newPos = new Vector3(player.position.x, transform.position.y, -10f);
            float speed = movementSpeed;

            // Track player y position if the player is not jumping.
            if (playerMov.GetIsGrounded())
            {
                newPos.y = player.position.y + offsetY;
            }
            else if(!playerMov.GetIsGrounded() && playerPos.y < (verticalDeadZoneCenter - verticalDeadZoneLength / 2.0f))
            {
                newPos.y = player.position.y;
            }

            transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, speed);

            //Debug.Log("Adjust");
        }

    }
}
