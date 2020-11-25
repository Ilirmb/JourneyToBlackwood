using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right_Door : MonoBehaviour
{

    [SerializeField] private Animator myAnimationController;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            myAnimationController.SetBool("Right_Rotate", true);

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            myAnimationController.SetBool("Right_Rotate", false);

        }
    }

}
