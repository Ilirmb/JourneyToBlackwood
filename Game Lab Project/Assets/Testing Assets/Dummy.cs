using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class Dummy : MonoBehaviour {

    public SpriteMesh testMesh;
    public SpriteMeshInstance switcheroo;

	// Use this for initialization
	void Start () {

        switcheroo.spriteMesh = testMesh;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
