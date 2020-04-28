using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    private Vector3 posA;

    private Vector3 posB;

    private Vector3 nexPos;

    [SerializeField]
    private bool onlyMoveWithPlayer = false;
    [HideInInspector]
    public bool playerIsOnPlatform = false;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform childTransform;

    [SerializeField]
    private Transform transformB;

    // Start is called before the first frame update
    void Start()
    {
        posA = childTransform.localPosition;
        posB = transformB.localPosition;
        nexPos = posB;

    }

    // Update is called once per frame
    void Update()
    {
        //If you only move with the player, and the player is on board, move towards destination. Otherwise, move towards origin
        if(onlyMoveWithPlayer && playerIsOnPlatform)
        {
            nexPos = posB;
        }else if (onlyMoveWithPlayer)
        {
            nexPos = posA;
        }

        Move();

    }

    private void Move()
    {
        if(!onlyMoveWithPlayer)
            childTransform.localPosition = Vector3.MoveTowards(childTransform.localPosition, nexPos, speed * Time.deltaTime);
        if(onlyMoveWithPlayer && !(Vector3.Distance(childTransform.localPosition, nexPos) <= 0.1))
        {
            childTransform.localPosition = Vector3.MoveTowards(childTransform.localPosition, nexPos, speed * Time.deltaTime);
        }
        
        if (!onlyMoveWithPlayer && Vector3.Distance(childTransform.localPosition,nexPos) <= 0.1)
        {
            ChangeDestination();

        }
    }

    private void ChangeDestination()
    {
        nexPos = nexPos != posA ? posA : posB;

    }

 
}
