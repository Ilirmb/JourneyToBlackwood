using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour{

    float length, startPosX, startPosY;
    Transform cam;
    [SerializeField] float parallaxEffectX, parallaxEffectY, limitsY;

    void Start()
    {
        cam = Camera.main.GetComponent<Transform>();
        startPosX = transform.position.x;
        startPosY = transform.localPosition.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = cam.position.x * (1 - parallaxEffectX);
        float distX = cam.position.x * parallaxEffectX;
        float distY = cam.position.y * parallaxEffectY;

        transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);

        if (temp > startPosX + length) startPosX += length;
        else if (temp < startPosX - length) startPosX -= length;

        if (limitsY != 0)
        {
            if (transform.localPosition.y > limitsY) transform.localPosition = new Vector3(transform.localPosition.x, limitsY, transform.localPosition.z);
            else if (transform.localPosition.y < -limitsY) transform.localPosition = new Vector3(transform.localPosition.x, -limitsY, transform.localPosition.z);
        }
    }
}