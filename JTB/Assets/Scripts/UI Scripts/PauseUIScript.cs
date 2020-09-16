using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUIScript : MonoBehaviour
{
	//The State in where the game would be active
    public static bool GameIsPaused = false;

	public GameObject PauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameIsPaused)
			{
				Resume();
			} 
			else
			{
				Pause();
			}
		}
    }

	public void Resume()
	{
		PauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}

	public void Pause()
	{
		PauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		GameIsPaused = true;
	}

	public void Restart()
	{
		Time.timeScale = 1f;
		
	}

	public void Quitgame()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("Main Menu");
	}
}
