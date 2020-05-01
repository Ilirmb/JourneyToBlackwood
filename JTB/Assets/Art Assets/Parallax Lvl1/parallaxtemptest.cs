using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class parallaxtemptest : MonoBehaviour
{

    private float length, startPos;
    public GameObject cam;
    public float pEffect; //how fast the bg moves

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float temp = (cam.transform.position.x * (1 - pEffect));
        float dist = (cam.transform.position.x * pEffect);
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        //if (temp > startPos + length && startPos < stopPos) startPos += length;
        //else if (temp < startPos - length) startPos -= length;
    }

}