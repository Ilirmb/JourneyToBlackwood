﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingAppleSpawner : MonoBehaviour {

    private GameObject applePrefab;
    public bool isOnScreen;
    public bool willSpawnOffscreen = false;

    public float timeBetweenSpawns = 2f;

    private bool isRunning;

    // Use this for initialization
    void Start () {
        applePrefab = Resources.Load("Prefabs/Falling Apple") as GameObject;
        isOnScreen = false;
        isRunning = false;
	}
	
	private IEnumerator AppleSpawner(float waitTime)
    {
        isRunning = true;
        while (isOnScreen || willSpawnOffscreen)
        {
            if (Time.timeScale > 0)
            {
                Instantiate(applePrefab, this.transform);
            }
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
