using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // Static singleton instance that allows GameManager to be accessed from any script.
    public static GameManager instance = null;

    // Event that is called whenever the player runs out of stamina
    [HideInInspector]
    public UnityEvent OnPlayerDeath;

    private Quest activeQuest;
    [HideInInspector]
    public string playerName = "Default_Profile";
    private PlayerStatistics player;
    private CustomPlatformer2DUserControl playerMov;
    private Animator playerAnim;
    private Rigidbody2D playerRb2d;

    private int sceneID;
    private bool sceneLoaded;
    private Checkpoint checkpoint = null;
    private Hashtable socialValues = new Hashtable();
    private Hashtable friendshipValues = new Hashtable();
    private List<QuestData> questData = new List<QuestData>();
    private string datapath;
    private string saveextension = "_progress.sav";

    // Dialogue Tree for hint offer
    [SerializeField]
    private DialogueTree defaultHint;
    private int currentHint = 0;

    // Dialogue Tree for wave wardning
    [SerializeField]
    private DialogueTree waveWarning;

    // List of current available hints
    private List<DialogueTree> hints = new List<DialogueTree>();

    // List of health tips
    [SerializeField]
    private List<DialogueTree> healthTips = new List<DialogueTree>();

    void Awake ()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if(CustomizationManager.instance == null)
        {
            Debug.LogWarning("Uh oh, the customization manager is not loaded yet");
        }


        // Check if a game manager instance exists
        if (instance == null)
        {
            // If no, this object is our instance
            instance = this;
            DontDestroyOnLoad(gameObject);

            datapath = Application.persistentDataPath + "/Saves/";
            
            Debug.Log("Accessing save directory at " + datapath );
            //This function will only create a directory if none is found. If there is one already there it does (almost) nothing
            Directory.CreateDirectory(datapath);

            Debug.Log("Attempting to load or create a default profile");

            if (File.Exists(datapath + playerName + saveextension))
            {
                Debug.Log("Loading profile " + playerName);
                //LoadProgress(playerName); Only for testing. Use Inspector GUI button to load default profile now
            }
            else
            {
                playerName = "Default_Profile";
                if (File.Exists(datapath + playerName + saveextension))
                {
                    //LoadProgress(playerName);
                }
                else {
                    CustomizationManager.instance.SetCurrentCostume(0);
                    CustomizationManager.instance.SetCurrentFace(0);
                    CustomizationManager.instance.SetCurrentHairStyle(0);
                    sceneID = 0;
                    //SaveProgress(); 
                }
            }

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
        if (player != null)
        {
            Debug.Log("Player found on scene load: setting");
            playerMov = player.transform.GetComponent<CustomPlatformer2DUserControl>();
            playerAnim = player.transform.GetComponentInChildren<Animator>();
            playerRb2d = player.transform.GetComponent<Rigidbody2D>();
        }
        sceneLoaded = true;
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


    #region Player Management
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

    public void GetLastCheckpoint()
    {
        if (player.checkpoint != null)
            checkpoint = player.checkpoint;
        else
            checkpoint = null;
    }

    #endregion


    #region Save Function

    /// <summary>
    /// Saves saved progress.
    /// </summary>
    public void SaveProgress()
    {
        Debug.Log("Saving...");

        //GatherQuestData();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(datapath + playerName + saveextension, FileMode.Create);
        
        SaveData saveData = new SaveData();

        saveData.sceneID = SceneManager.GetActiveScene().buildIndex;
        saveData.name = playerName;
        saveData.socialValues = socialValues;
        saveData.friendshipValues = friendshipValues;
        saveData.questData = questData;

        GetLastCheckpoint();
        if (checkpoint != null)
        {
            saveData.checkpoint = new float[3];
            saveData.checkpoint[(int)vectorVal.x] = checkpoint.transform.position.x;
            saveData.checkpoint[(int)vectorVal.y] = checkpoint.transform.position.y;
            saveData.checkpoint[(int)vectorVal.z] = checkpoint.transform.position.z;
        }
        else //If no checkpoint is found just save the player's position
        {
            saveData.checkpoint = new float[3];
            saveData.checkpoint[(int)vectorVal.x] = player.transform.position.x;
            saveData.checkpoint[(int)vectorVal.y] = player.transform.position.y;
            saveData.checkpoint[(int)vectorVal.z] = player.transform.position.z;
        }

        saveData.PlayerCustomization = CustomizationManager.instance.State();

        bf.Serialize(stream, saveData);
        stream.Close();

        Debug.Log("Save Complete");
    }


    /// <summary>
    /// Loads saved progress.
    /// </summary>
    public void LoadProgress(string playername)
    {
        Debug.Log("Attempting load...");
        if (File.Exists(datapath + playername + saveextension))
        {
            // DOOP
            Debug.Log("File found, beginning load");

            // Opens the file and deserializes the data
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(datapath + playername + saveextension, FileMode.Open);

            SaveData saveData = bf.Deserialize(stream) as SaveData;

            playerName = saveData.name;
            sceneID = saveData.sceneID;
            socialValues = saveData.socialValues;
            friendshipValues = saveData.friendshipValues;
            questData = saveData.questData; 
            
            if (sceneID != 0)
            {
                sceneLoaded = false;
                SceneManager.LoadScene(sceneID);
            }
            StartCoroutine(LoadProgressWaitForSceneLoaded(saveData));
            

            stream.Close();

            //LoadQuestData();

            Debug.Log("Load completed");
        }
        else
            Debug.Log("Load failed: file not found");
    }

    public IEnumerator LoadProgressWaitForSceneLoaded(SaveData saveData)
    {
        yield return new WaitWhile(() => sceneLoaded == false);
        Debug.Log("Post-Scene-loading loading beginning");
        if (player == null)
        {
            Debug.Log("Player == null");
        }
        else
        {
            Debug.Log("Player != null");
        }

        if (checkpoint == null)
        {
            GameObject chgo = new GameObject("Load in Point");
            checkpoint = chgo.gameObject.AddComponent<Checkpoint>();
        }

        checkpoint.transform.position = new Vector3(
                saveData.checkpoint[(int)vectorVal.x],
                saveData.checkpoint[(int)vectorVal.y],
                saveData.checkpoint[(int)vectorVal.z]);
        player.checkpoint = checkpoint;
        player.ReloadAtCheckpoint();
        Destroy(checkpoint); //It's ok to leave the game object for testing purposes, but the script will try to reference components that do not exist so we do not need it

        if (CustomizationManager.instance != null)
            CustomizationManager.instance.SetState(saveData.PlayerCustomization);
        else
            Debug.Log("Cannot set customize state: Customization Manager not loaded");

        if (player != null)
            player.UpdateColors();
    }

    public FileStream[] LoadSaveFiles()
    {
        Debug.Log("Retrieving all save files within direcctory " + datapath);

        string[] files = Directory.GetFiles(datapath);
        FileStream[] streams = new FileStream[files.Length];

        for(int i = 0; i < files.Length; ++i)
        {
            Debug.Log("File " + files[i] + " detected in save file directory, adding to filestream list");
            streams[i] = new FileStream(files[i], FileMode.Open);
        }
        return streams;
    }


    /// <summary>
    /// Gathers all quests in the scene, serializes them, and saves everything.
    /// Disabled for now
    /// </summary>
 /*   private void GatherQuestData()
    {
        // Get all quests in the scene
        Quest[] questsInScene = FindObjectsOfType<Quest>();

        // For each quest in the scene
        foreach (Quest q in questsInScene)
        {
            // Get its data
            QuestData qd = q.SaveQuestData();

            // If the player has interacted with the quest giver before, increment their friendship. Otherwise, add that quest giver to the list.
            if (friendshipValues.ContainsKey(qd.owner))
                friendshipValues[qd.owner] = (qd.friendship + (int)friendshipValues[qd.owner]);
            else
                friendshipValues.Add(qd.owner, qd.friendship);

            // Check if quest exists in data. If it does, clear it.
            QuestData savedData = questData.Find(d => d.questHash.Equals(qd.questHash));

            if (savedData != null)
                questData.Remove(savedData);

            // Add the quest data
            questData.Add(qd);
        }
    }*/


    /// <summary>
    /// Loads the quest data for all quests in the scene
    /// </summary>
    private void LoadQuestData()
    {
        // Find all quests in the scene
        Quest[] questsInScene = FindObjectsOfType<Quest>();

        // For each quest in the scene
        foreach (Quest q in questsInScene)
        {
            // Get the quest's saved data
            QuestData qd = q.SaveQuestData();

            // If the player has interacted with the quest giver before, adjust friendship
            if (friendshipValues.ContainsKey(qd.owner))
                qd.friendship = ((int)friendshipValues[qd.owner]);

            // Check if quest exists in data already
            QuestData savedData = questData.Find(d => d.questHash.Equals(qd.questHash));

            // If it does, set its win and lose state
            if (savedData != null)
            {
                qd.cleared = savedData.cleared;
                qd.failed = savedData.failed;
            }

            // Load the quest state
            q.LoadQuestState(qd);
        }
    }

    public void GetAllValidSaves()
    {
    }

    public bool DoesSaveExist(string s)
    {
        return false;
    }


    /// <summary>
    /// Attempts to load the data of a quest with a given name. Returns null if it doesn't exist.
    /// </summary>
    public QuestData LoadSpecificQuest(string questName)
    {
        return questData.Find(d => d.questHash.Equals(questName));
    }

    #endregion


    #region Hint Functions

    /// <summary>
    /// Update the list of available hints
    /// </summary>
    public void UpdateHintList(List<DialogueTree> newHints)
    {
        currentHint = 0;
        hints = newHints;
    }


    /// <summary>
    /// Show hint from the list of hints in order
    /// </summary>
    public void ShowHint()
    {
        if (hints.Count > 0)
        {
            DialogueProcessor.instance.StartDialogue(hints[currentHint], true);
            ++currentHint;
            currentHint %= hints.Count;
        }
        else
        {
            Debug.Log("No hints found: Is your Hint Area correctly set?");
            DialogueProcessor.instance.StartDialogue(defaultHint, true);
        }
    }


    /// <summary>
    /// Offers a hint to the player
    /// </summary>
    public void OfferHint()
    {
        DialogueProcessor.instance.StartDialogue(defaultHint);
    }



    /// <summary>
    /// Shows a health tip to the player.
    /// </summary>
    public void ShowHealthTip()
    {
        if (healthTips.Count > 0)
        {
            int index = Random.Range(0, healthTips.Count);
            DialogueProcessor.instance.StartDialogue(healthTips[index]);
        }
    }
    public void ShowWaveTip()
    {
 
           DialogueProcessor.instance.StartDialogue(waveWarning);
        
    }

    #endregion
}
