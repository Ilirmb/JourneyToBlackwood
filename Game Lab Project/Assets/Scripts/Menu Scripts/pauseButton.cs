using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseButton : MonoBehaviour {

    public GameObject pauseMenu;
    private bool paused = false;


    public void Pause()
    {
        if (paused)
        {
            Time.timeScale = 1f;
            paused = false;
            pauseMenu.SetActive(false);
        } else
        {
            Time.timeScale = 0f;
            paused = true;
            pauseMenu.SetActive(true);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
