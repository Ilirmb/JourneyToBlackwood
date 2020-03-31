using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Parallax : MonoBehaviour{

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

        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }



    /*float length, startPosX, startPosY;
    Transform cam;
    Rigidbody2D rb;
    [SerializeField] float parallaxEffectX, parallaxEffectY, limitsY;

    void Start()
    {
        cam = Camera.main.transform;
        rb = this.GetComponent<Rigidbody2D>();
        startPosX = transform.position.x;
        startPosY = transform.localPosition.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    
    void FixedUpdate()
    {
        float temp = cam.position.x * (1 - parallaxEffectX);
        float distX = cam.position.x * parallaxEffectX;
        float distY = cam.position.y * parallaxEffectY;

        rb.position = new Vector2(startPosX + distX, startPosY + distY);

        if (temp > startPosX + length) startPosX += length;
        else if (temp < startPosX - length) startPosX -= length;

        if (limitsY != 0)
        {
            if (transform.localPosition.y > limitsY)
                transform.localPosition = new Vector3(transform.localPosition.x, limitsY, transform.localPosition.z);
            else if (transform.localPosition.y < -limitsY)
                transform.localPosition = new Vector3(transform.localPosition.x, -limitsY, transform.localPosition.z);
        }
    }*/
}