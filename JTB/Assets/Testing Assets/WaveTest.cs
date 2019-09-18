using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game2DWaterKit;

public class WaveTest : MonoBehaviour {

    private Vector3 startPos;
    public Vector3 speed;

    public Game2DWater water;

	// Use this for initialization
	void Start () {

        startPos = transform.position;

    }
	

	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Ripple");
            water.ScriptGeneratedRipplesModule.GenerateRipple(new Vector2(0.0f, 0.0f), 20.0f, true, false, false, true, 15f);
        }

	}
}
