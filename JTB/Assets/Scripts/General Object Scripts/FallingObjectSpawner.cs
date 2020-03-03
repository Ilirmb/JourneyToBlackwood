using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A more generalized version of the FallingAppleScript. I kept the old one because it uses a less robust form of prefab selection
/// </summary>

public class FallingObjectSpawner : MonoBehaviour {

    public GameObject objectPrefab;
    public bool isOnScreen;
    public bool willSpawnOffscreen = false;

    public float timeBetweenSpawns = 2f;

    private bool isRunning;

    // Use this for initialization
    void Start () {
        isOnScreen = false;
        isRunning = false;
	}
	
	private IEnumerator AppleSpawner(float waitTime)
    {
        isRunning = true;
        while (isOnScreen || willSpawnOffscreen)
        {
            Instantiate(objectPrefab, this.transform);
            yield return new WaitForSecondsRealtime(waitTime);
        }
        isRunning = false;
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;
        if(!isRunning)
            StartCoroutine(AppleSpawner(timeBetweenSpawns));
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;
    }
}
