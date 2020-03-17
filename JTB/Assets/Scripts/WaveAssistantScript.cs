using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAssistantScript : MonoBehaviour
{
    public GameObject WaveStart, WaveEnd, WaveMakerObj;
    private CircleCollider2D WaveMaker;
    private float objDistance, radiusStart;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        WaveMaker = gameObject.transform.Find("WaveObject").GetComponent<CircleCollider2D>();
        radiusStart = WaveMaker.radius;
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed;

        objDistance = Vector3.Distance(WaveMakerObj.transform.position, WaveEnd.transform.position);

        WaveMakerObj.transform.position = Vector3.MoveTowards(WaveMakerObj.transform.position, WaveEnd.transform.position, step);

        WaveMaker.radius -= 0.001f;

        if (objDistance < 0.001f)
        {
            WaveMakerObj.transform.position = WaveStart.transform.position;
            WaveMaker.radius = radiusStart;
            
        }
    }
}
