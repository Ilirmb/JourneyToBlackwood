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

	//The UI calls to be resumed
	public void Resume()
	{
		PauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}

	//The UI calls to pause
	public void Pause()
	{
		PauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		GameIsPaused = true;
	}

	//Resets the Player's current position and progress on a quest
	public void Restart()
	{
		
	}

	//Brings the Player back to the main menu
	public void Quitgame()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("Main Menu");
	}
}
