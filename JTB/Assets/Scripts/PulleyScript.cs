using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyScript : MonoBehaviour
{
    public Transform[] points;
    public GameObject[] platforms;
    private int destination = 0;

    // Start is called before the first frame update
    void Start()
    {
        Gottonextpoint();
    }

    public void Gottonextpoint()
    {
        if (points.Length == 0)
            return;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
