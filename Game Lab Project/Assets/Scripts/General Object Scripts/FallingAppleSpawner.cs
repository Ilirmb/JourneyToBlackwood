using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingAppleSpawner : MonoBehaviour {

    public GameObject applePrefab;
    public bool isOnScreen;

    public float timeBetweenSpawns = 2f;

    // Use this for initialization
    void Start () {
        applePrefab = Resources.Load("Prefabs/Falling Apple") as GameObject;
        isOnScreen = false;

	}
	
	private IEnumerator AppleSpawner(float waitTime)
    {
        while (isOnScreen)
        {
            Instantiate(applePrefab, this.transform);
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;
        StartCoroutine(AppleSpawner(timeBetweenSpawns));
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;
    }
}
