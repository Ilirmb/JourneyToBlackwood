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
    private Hashtable friendshipValues = new Hashtable();
    private List<QuestData> questData = new List<QuestData>();
    private string path;

    // Dialogue Tree for hint offer
    [SerializeField]
    private DialogueTree hintOffer;

    // List of current available hints
    private List<DialogueTree> hints = new List<DialogueTree>();

    // List of health tips
    [SerializeField]
    private List<DialogueTree> healthTips = new List<DialogueTree>();


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

        GatherQuestData();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        
        SaveData saveData = new SaveData();

        saveData.socialValues = socialValues;
        saveData.friendshipValues = friendshipValues;
        saveData.questData = questData;

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
            friendshipValues = saveData.friendshipValues;
            questData = saveData.questData;

            stream.Close();

            LoadQuestData();
        }
    }


    /// <summary>
    /// Gathers all quests in the scene, serializes them, and saves everything.
    /// </summary>
    private void GatherQuestData()
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
    }


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
        hints = newHints;
    }


    /// <summary>
    /// Show a randomized hint from the list of hints
    /// </summary>
    public void ShowHint()
    {
        if(hints.Count > 0)
        {
            int index = Random.Range(0, hints.Count);
            DialogueProcessor.instance.StartDialogue(hints[index]);
        }
    }


    /// <summary>
    /// Offers a hint to the player
    /// </summary>
    public void OfferHint()
    {
        DialogueProcessor.instance.StartDialogue(hintOffer);
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

    #endregion
}
