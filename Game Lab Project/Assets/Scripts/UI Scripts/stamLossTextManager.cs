 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stamLossTextManager : MonoBehaviour {

    public GameObject textPrefab;
    public GameObject currentObject;

    public void spawnText(string text, Color color)
    {
        currentObject = Instantiate(textPrefab, transform.position + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.2f, .2f)), Quaternion.identity);
        currentObject.GetComponent<stamLossText>().init(this, text, color);
    }
    public void spawnText(string text, Color color, Vector3 offset)
    {
        currentObject = Instantiate(textPrefab, transform.position + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.2f, .2f)) + offset, Quaternion.identity);
        currentObject.GetComponent<stamLossText>().init(this, text, color);
    }
    public void spawnText(string text, Color color, float time)
    {
        currentObject = Instantiate(textPrefab, transform.position + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.2f, .2f)), Quaternion.identity);
        currentObject.GetComponent<stamLossText>().init(this, text, color);
        currentObject.GetComponent<stamLossText>().setKillTime(time);
    }


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
