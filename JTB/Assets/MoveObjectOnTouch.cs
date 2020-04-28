using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectOnTouch : MonoBehaviour
{
    public GameObject objectToMove;
    public Transform target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            objectToMove.transform.position = target.position;
        }
    }
}
