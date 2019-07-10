using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

    // Static singleton instance that allows GameManager to be accessed from any script.
    public static GameManager instance = null;

    // Event that is called whenever the player runs out of stamina
    [HideInInspector]
    public UnityEvent OnPlayerDeath;

    private Quest activeQuest;

    private PlayerStatistics player;
    private CustomPlatformer2DUserControl playerMov;
    private Animator playerAnim;
    private Rigidbody2D playerRb2d;
    
    private Hashtable socialValues = new Hashtable();
    private string path;


    // Use this for initialization
    void Awake ()
    {
        // Check if a game manager instance exists
        if (instance == null)
        {
            // If no, this object is our instance
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Loads saved progress
            path = Application.persistentDataPath + "/progress.sav";
            LoadProgress();
        }
        else
        {
            // If yes, destroy this object
            Destroy(gameObject);
            return;
        }

    }


    #region Scene Management

    /// <summary>
    /// Called automatically whenever a scene is loaded
    /// </summary>
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        OnPlayerDeath.RemoveAllListeners();
        player = FindObjectOfType<PlayerStatistics>();
        
        playerMov = player.transform.GetComponent<CustomPlatformer2DUserControl>();
        playerAnim = player.transform.GetComponentInChildren<Animator>();
        playerRb2d = player.transform.GetComponent<Rigidbody2D>();
    }

    
    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion


    #region Quest Management

    /// <summary>
    /// SetCurrentQuest
    /// Sets the current quest
    /// </summary>
    /// <param name="newQuest">The quest to set</param>
    public void SetCurrentQuest(Quest newQuest)
    {
        activeQuest = newQuest;
    }


    /// <summary>
    /// ResetQuest
    /// Resets the current quest to null
    /// </summary>
    public void ResetQuest()
    {
        SetCurrentQuest(null);
    }


	/// <summary>
	/// Gets the current active quest
	/// </summary>
    public Quest GetActiveQuest()
    {
        return activeQuest;
    }

	
	/// <summary>
	/// Toggles the interactable state of the active quest
	/// </summary>
    public void ToggleQuestInteractivity()
    {
        if(activeQuest != null)
            activeQuest.ToggleInteractivity();
    }

	
	/// <summary>
	/// Sets the interactable state of the active quest to the given status
	/// </summary>
    public void ToggleQuestInteractivity(bool status)
    {
        if (activeQuest != null)
            activeQuest.ToggleInteractivity(status);
    }

	
	/// <summary>
	/// Sets the first encounter flags of the active quest
	/// </summary>
    public void EndQuestFirstEncounter()
    {
        if(activeQuest != null)
            activeQuest.EndFirstEncounter();
    }

    #endregion

	
	/// <summary>
	/// Increases the player's max stamina
	/// </summary>
    public void IncreasePlayerStamina(float amt)
    {
        player.increaseMaxStamina(amt);
    }


	/// <summary>
	/// Prevents the player from moving
	/// </summary>
    public void DisablePlayerMovement()
    {
        playerMov.canControl = false;
        playerAnim.SetFloat("Speed", 0f);
        playerRb2d.velocity = Vector3.zero;
    }


	/// <summary>
	/// Reinstates player movement
	/// </summary>
    public void EnablePlayerMovement()
    {
        playerMov.canControl = true;
    }


    /// <summary>
    /// Returns the player statistics.
    /// </summary>
    public PlayerStatistics GetPlayerStatistics()
    {
        return player;
    }


    /// <summary>
    /// Increases or decreases the value of the given social value.
    /// </summary>
    public void AffectSocialValue(string s, int i)
    {
        s = SocialValueValidation.ValidateName(s, GetSocialValueKeys());
        Debug.Log(s);

        if (socialValues.ContainsKey(s))
            socialValues[s] = ((int)socialValues[s]) + i;
        else
            socialValues.Add(s, i);
    }


    /// <summary>
    /// Gets the value relating to the given social value name
    /// </summary>
    public int GetSocialValue(string s)
    {
        return (int)socialValues[s];
    }


    public List<string> GetSocialValueKeys()
    {
        List<string> names = new List<string>();

        foreach(object o in socialValues.Keys)
        {
            names.Add(((string)o));
        }

        return names;
    }



    #region Save Function

    /// <summary>
    /// Saves saved progress.
    /// </summary>
    public void SaveProgress()
    {
        Debug.Log("Saving...");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        
        SaveData saveData = new SaveData();
        saveData.socialValues = socialValues;

        bf.Serialize(stream, saveData);
        stream.Close();
    }


    /// <summary>
    /// Loads saved progress.
    /// </summary>
    public void LoadProgress()
    {
        Debug.Log("Attempting load...");
        if (File.Exists(Application.persistentDataPath + "/progress.sav"))
        {
            // DOOP
            Debug.Log("Load success");

            // Opens the file and deserializes the data
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/progress.sav", FileMode.Open);

            SaveData saveData = bf.Deserialize(stream) as SaveData;
            socialValues = saveData.socialValues;

            if(socialValues.ContainsKey("test"))
                Debug.Log(socialValues["test"]);

            stream.Close();
        }
    }

    #endregion
}
