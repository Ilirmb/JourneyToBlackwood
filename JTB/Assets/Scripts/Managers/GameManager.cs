using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

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
    [Serializable]
    public class SceneStatistics
    {
        //Currently unused
        public string scenename = "Default";
        //Below is being used
        public Hashtable statValues = new Hashtable();
        public Hashtable socialValues = new Hashtable();
        public Hashtable friendshipValues = new Hashtable();
        public List<string> questFlags = new List<string>();
    }
    //A dictionary of scenenames as strings to Statistics
    private Dictionary<string, SceneStatistics> stats = new Dictionary<string, SceneStatistics>();
    
    public Dictionary<string, SceneStatistics> Stats
    {
        get { return new Dictionary<string, SceneStatistics>(stats); }
    }
    public Hashtable StatValues
    {
        get { return new Hashtable(stats[currentScene].statValues); }
    }
    public Hashtable SocialValues
    {
        get { return new Hashtable(stats[currentScene].socialValues); }
    }
    public Hashtable FriendshipValues
    {
        get { return new Hashtable(stats[currentScene].friendshipValues); }
    }
    public List<string> QuestFlags
    {
        get { return new List<string>(stats[currentScene].questFlags); }
    } 


    private List<QuestData> questData = new List<QuestData>();
    private string currentScene
    {
        get
        {
            return SceneManager.GetActiveScene().name;
        }
    }

    private string datapath;
    private string saveextension = "_progress.sav";
    private float loadTime = 0.0f;

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

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (CustomizationManager.instance == null)
        {
            Debug.LogWarning("Uh oh, the customization manager is not loaded yet");
        }


        // Check if a game manager instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            datapath = Application.persistentDataPath + "/Saves/";

            Debug.Log("Accessing save directory at " + datapath);
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
                else if (CustomizationManager.instance != null)
                {
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
            //Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
    }

    public void SetPlayerName(string s)
    {
        playerName = s;
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
        } else Debug.LogError("No Player Found");
        sceneLoaded = true;
        LoadQuestData();
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
        if (activeQuest != null)
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
        if (activeQuest != null)
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

    #region stat values
    /// <summary>
    /// Increases or decreases the value of the given social value.
    /// </summary>
    public void AffectStatValue(string s, float v)
    {
        if (!stats.ContainsKey(currentScene))
        {
            //Debug.Log("No previous statistics detected on this scene: Generating new statblock for scene " + currentScene);
            stats.Add(currentScene, new SceneStatistics());
        }

        s = SocialValueValidation.ValidateName(s, GetStatValueKeys());

        if (stats[currentScene].statValues.ContainsKey(s))
        {
            //Debug.Log("Stat values contains " + s + ", incrementing by " + v);
            stats[currentScene].statValues[s] = ((float)StatValues[s]) + v; // cant use operator + on <T> or else I would have used generics
        }
        else
        {
            //Debug.Log("Stat values does not yet contain " + s + ", creating new stat and setting to " + v);
            stats[currentScene].statValues.Add(s, v);
        }

    }
    public void AffectStatValue(string stat, string value)
    {
        if (!stats.ContainsKey(currentScene))
            stats.Add(currentScene, new SceneStatistics());

        stat = SocialValueValidation.ValidateName(stat, GetStatValueKeys());

        if (StatValues.ContainsKey(stat))
            stats[currentScene].statValues[stat] = value;
        else
            stats[currentScene].statValues.Add(stat, value);
    }


    /// <summary>  
    public object GetStatValueRaw(string scenename, string statname)
    {
        return stats[scenename].statValues[statname];
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the social value you want to retrieve</typeparam>
    /// <param name="name">the string name of the social value</param>
    /// <returns></returns>
    public T GetStatValue<T>(string scenename, string statname)
    {
        return (T)stats[scenename].statValues[statname];
    }


    public List<string> GetStatValueKeys()
    {
        List<string> names = new List<string>();

        foreach (object o in StatValues.Keys)
        {
            names.Add((string)o);
        }

        return names;
    }
    public List<string> GetStatValueKeys(string scenename)
    {
        List<string> names = new List<string>();

        foreach (object o in stats[scenename].statValues.Keys)
        {
            names.Add((string)o);
        }

        return names;
    }


    #endregion

    #region social values
    /// <summary>
    /// Increases or decreases the value of the given social value.
    /// </summary>
    public void AffectSocialValue(string s, float v)
    {
        s = SocialValueValidation.ValidateName(s, GetSocialValueKeys());
        Debug.Log(s);

        if (SocialValues.ContainsKey(s))
            SocialValues[s] = ((float)SocialValues[s]) + v; // cant use operator + on <T> or else I would have used generics
        else
            SocialValues.Add(s, v);
    }
    public void AffectSocialValue(string s, string v)
    {
        s = SocialValueValidation.ValidateName(s, GetSocialValueKeys());
        Debug.Log(s);

        if (SocialValues.ContainsKey(s))
            SocialValues[s] = v;
        else
            SocialValues.Add(s, v);
    }


    /// <summary>
    /// Returns the raw object stored inside the hashtable. Useful for getting the value as a string for display
    /// </summary>
    public object GetSocialValueRaw(string s)
    {
        return SocialValues[s];
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the social value you want to retrieve</typeparam>
    /// <param name="name">the string name of the social value</param>
    /// <returns></returns>
    public T GetSocialValue<T>(string name)
    {
        return (T)SocialValues[name];
    }


    public List<string> GetSocialValueKeys()
    {
        List<string> names = new List<string>();

        foreach (object o in SocialValues.Keys)
        {
            names.Add(((string)o));
        }

        return names;
    }

    #endregion

    #region flags

    /// <summary>
    /// Adds the flag to the current scene
    /// </summary>
    /// <param name="flag"></param>
    public void AddQuestFlag(string flag)
    {
        Stats[currentScene].questFlags.Add(flag);
    }
    /// <summary>
    /// Adds the flag at scene scenename
    /// </summary>
    /// <param name="flag"></param>
    /// <param name="scenename"></param>
    public void AddQuestFlag(string flag, string scenename)
    {
        Stats[scenename].questFlags.Add(flag);
    }

    /// <summary>
    /// Removes the flag from the current scene
    /// </summary>
    /// <param name="flag"></param>
    public void RemoveQuestFlag(string flag)
    {
        Stats[currentScene].questFlags.Remove(flag);
    }
    /// <summary>
    /// removes the flag from scene scenename
    /// </summary>
    /// <param name="flag"></param>
    /// <param name="scenename"></param>
    public void RemoveQuestFlag(string flag, string scenename)
    {
        Stats[scenename].questFlags.Remove(flag);
    }

    /// <summary>
    /// Checks to see if the given flag exists only on the current scene
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public bool CheckQuestFlag(string flag)
    {
        return Stats[currentScene].questFlags.Contains(flag);
    }
    /// <summary>
    /// checks to see if the given flag exists only on the given scene
    /// </summary>
    /// <param name="flag"></param>
    /// <param name="scenename"></param>
    /// <returns></returns>
    public bool CheckQuestFlag(string flag, string scenename)
    {
        return Stats[scenename].questFlags.Contains(flag);
    }
    /// <summary>
    /// If a flag exists across any scene statistic struct, this function will find it and return true
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public bool CheckQuestFlagAcrossAllScenes(string flag)
    {
        bool foundFlag = false;
        foreach(SceneStatistics statistics in stats.Values)
        {
            foundFlag = statistics.questFlags.Contains(flag);
            if (foundFlag) break;
        }
        return foundFlag;
    }

    #endregion

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

        AffectStatValue("Save Time", DateTime.Now.ToString());
        AffectStatValue("Total Playtime", Time.time - loadTime);
        loadTime = Time.time;

        saveData.sceneID = SceneManager.GetActiveScene().buildIndex;
        saveData.name = playerName;

        List<string> sceneNames = new List<string>();
        List<Hashtable> statlist = new List<Hashtable>();
        List<Hashtable> friendshiplist = new List<Hashtable>();
        List<Hashtable> sociallist = new List<Hashtable>();
        List<string[]> flaglist = new List<string[]>();

        foreach (string scenename in stats.Keys)
        {
            Debug.Log("Saving scenedata for scene " + scenename);
            sceneNames.Add(scenename);
            SceneStatistics statistics = stats[scenename];
            statlist.Add(statistics.statValues);
            friendshiplist.Add(statistics.friendshipValues);
            sociallist.Add(statistics.socialValues);
            flaglist.Add(statistics.questFlags.ToArray());
        }

        saveData.sceneNames = sceneNames.ToArray();
        saveData.statValues = statlist.ToArray();
        saveData.friendshipValues = friendshiplist.ToArray();
        saveData.socialValues = sociallist.ToArray();

        saveData.questFlags = flaglist.ToArray();

        saveData.lastScene = sceneNames.IndexOf(currentScene);

        GatherQuestData();
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

            for (int i = 0; i < saveData.sceneNames.Length; i++)
            {
                string name = saveData.sceneNames[i];
                Debug.Log("Loaded scenedata for scene " + i + ", name of " + name);
                SceneStatistics statistics = new SceneStatistics() 
                { 
                    friendshipValues = saveData.friendshipValues[i],
                    socialValues = saveData.socialValues[i],
                    statValues = saveData.statValues[i],
                    questFlags = new List<string>(saveData.questFlags[i])
                };
                stats.Add(name, statistics);
            }


            questData = saveData.questData;

            loadTime = Time.time;

            if (sceneID != 0)
            {
                sceneLoaded = false;
                SceneManager.LoadScene(sceneID);
            }
            StartCoroutine(LoadProgressWaitForSceneLoaded(saveData));


            stream.Close();

            LoadQuestData();

            Debug.Log("Load completed");
        }
        else
            Debug.Log("Load failed: file not found");
    }
    
    /// <summary>
    /// Some things need to wait for certain references to load in before they load in themselves
    /// </summary>
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

        for (int i = 0; i < files.Length; ++i)
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
    private void GatherQuestData()
    {
        // Get all quests in the scene
        Quest[] questsInScene = FindObjectsOfType<Quest>();
        Debug.Log(questsInScene.Length + " number of savable quests found");

        // For each quest in the scene
        foreach (Quest q in questsInScene)
        {
            // Get its data
            QuestData qd = q.SaveQuestData();

            // If the player has interacted with the quest giver before, increment their friendship. Otherwise, add that quest giver to the list.
            if (stats[currentScene].friendshipValues.ContainsKey(qd.owner))
                stats[currentScene].friendshipValues[qd.owner] = (qd.friendship + (int)stats[currentScene].friendshipValues[qd.owner]);
            else
                stats[currentScene].friendshipValues.Add(qd.owner, qd.friendship);

            // Check if quest exists in data. If it does, clear it.
            QuestData savedData = questData.Find(d => d.questHash.Equals(qd.questHash));

            if (savedData != null)
                questData.Remove(savedData);

            // Add the quest data
            questData.Add(qd);
        }
    }


    /// <summary>
    /// Loads the quest data for all quests in the scene
    /// </summary>
    private void LoadQuestData()
    {
        // Find all quests in the scene
        Quest[] questsInScene = FindObjectsOfType<Quest>();
        Debug.Log("Attempting to load quest data for " + questsInScene.Length + " quests");

        // For each quest in the scene
        for (int i = 0; i < questsInScene.Length; i++)
        {
            Quest q = questsInScene[i];
            // Get the quest's saved data
            QuestData qd = q.SaveQuestData();

            // If the player has interacted with the quest giver before, adjust friendship
            if (FriendshipValues.ContainsKey(qd.owner))
                qd.friendship = ((int)FriendshipValues[qd.owner]);

            // Check if quest exists in data already
            QuestData savedData = questData.Find(d => d.questHash.Equals(qd.questHash));

            // If it does, set its win and lose state
            if (savedData != null)
            {
                Debug.Log("Quest found for " + savedData.questHash);
                qd.cleared = savedData.cleared;
                qd.failed = savedData.failed;
            }
            else
            {
                Debug.Log("No quest found");
            }

            // Load the quest state using the direct reference
            questsInScene[i].LoadQuestState(qd);
            Debug.Log(questsInScene[i].GetCurrentState());
        }
    }

    public void GetAllValidSaves()
    {
    }

    public bool DoesSaveExist(string s)
    {
        return false; //unimplemented
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
            int index = UnityEngine.Random.Range(0, healthTips.Count);
            DialogueProcessor.instance.StartDialogue(healthTips[index]);
        }
    }
    public void ShowWaveTip()
    {

        DialogueProcessor.instance.StartDialogue(waveWarning);

    }

    #endregion
}
