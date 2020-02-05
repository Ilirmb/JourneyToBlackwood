using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashlight : MonoBehaviour
{
    public GameObject torch;
    private GameObject handler;
    public bool canUse = true;
    private Vector3 mousePos;

    void Update()
    { 
        if (Input.GetMouseButton(0) && canUse == true)
        {
            canUse = false;
            StartCoroutine(Flashlight());
        }
    }

    IEnumerator Flashlight()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        handler = Instantiate(torch, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity) as GameObject;
        handler.transform.position = mousePos;
        yield return new WaitForSeconds(1);
        StartCoroutine(Fadeout());
    }

    IEnumerator Fadeout()
    {
        yield return new WaitForSeconds(4);;
        Reuse();
        Destroy(handler.gameObject);
    }

    public void Reuse()
    {
        canUse = true;
    }
}

