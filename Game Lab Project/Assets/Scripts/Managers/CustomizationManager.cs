using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomizationManager : MonoBehaviour {

    // Static singleton instance that allows customization data to be accessed from any script.
    public static CustomizationManager instance = null;

    // Current selected skin tone saturation
    private float skinToneSat = 0.0f;

    // Current selected skin tone value
    private float skinToneVal = 0.0f;

    // Min and max values for skin tone saturation
    [SerializeField]
    private float skinToneMinSaturation = 0.0f;
    [SerializeField]
    private float skinToneMaxSaturation = 0.2f;

    // Min and max values for skin tone value
    [SerializeField]
    private float skinToneMinValue = -0.4f;
    [SerializeField]
    private float skinToneMaxValue = 0.4f;


    // List of all costumes in the game
    [SerializeField]
    private List<CostumeData> costumeList = new List<CostumeData>();

    // Current Selected costume
    private CostumeData currentCostume;

    // Current costume index
    private int currentCostumeIndex;


    // Event that is called whenever the skin color is changed
    [HideInInspector]
    public UnityEvent OnSkinChanged;

    // Event that is called whenever the costume is changed
    [HideInInspector]
    public UnityEvent OnCostumeChanged;



    // Use this for initialization
    void Awake () {
		
        // Check if a customization manager instance exists
        if(instance == null)
        {
            // If no, this object is our instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If yes, destroy this object
            Destroy(gameObject);
            return;
        }

        // Set current costume to the first in the list if the list is not empty
        currentCostume = costumeList.Count > 0 ? costumeList[0] : null;
        currentCostumeIndex = costumeList.Count > 0 ? 0 : -1;
    }


    /// <summary>
    /// AdjustSkinToneValues
    /// Adjusts skin tone values (saturation and value) by the amount given
    /// </summary>
    /// <param name="amt">Amount to adjust saturation and value.</param>
    public void AdjustSkinToneValues(float amt)
    {
        // Sets skin tone value using the min range if amt is less than 0 and the max range if amt is greater than 0
        skinToneVal = amt < 0.0f ? skinToneMinValue * -amt : skinToneMaxValue * amt;

        // We only need to adjust saturation for darker skin tones.
        skinToneSat = amt < 0.0f ? skinToneMaxSaturation * -amt : skinToneMinSaturation * amt;

        OnSkinChanged.Invoke();
    }


    /// <summary>
    /// GetSkinVal
    /// Returns skin tone value
    /// </summary>
    /// <returns>Current skin tone value</returns>
    public float GetSkinVal()
    {
        return skinToneVal;
    }


    /// <summary>
    /// GetSkinSat
    /// Returns skin tone saturation
    /// </summary>
    /// <returns>Current skin tone saturation</returns>
    public float GetSkinSat()
    {
        return skinToneSat;
    }


    /// <summary>
    /// GetCurrentCostume
    /// Returns the current selected costume
    /// </summary>
    /// <returns>The current selected costume</returns>
    public CostumeData GetCurrentCostume()
    {
        return currentCostume;
    }


    /// <summary>
    /// SetCurrentCostume
    /// Sets the current costume to the given index
    /// </summary>
    /// <param name="index">Costume to set</param>
    public void SetCurrentCostume(int index)
    {
        // Set the costume to the index if it is in bounds, otherwise, set it to null
        currentCostume = (index >= costumeList.Count || index < 0) ? null : costumeList[index];
        currentCostumeIndex = (index >= costumeList.Count || index < 0) ? -1: index;

        OnCostumeChanged.Invoke();
    }


    /// <summary>
    /// AdvanceCurrentCostume
    /// Advances to the next costume in the list moving in the given direction
    /// </summary>
    /// <param name="dir">Direction to scroll</param>
    public void AdvanceCurrentCostume(int dir)
    {
        currentCostumeIndex += dir;

        if (currentCostumeIndex < 0)
            currentCostumeIndex = costumeList.Count - 1;

        if (currentCostumeIndex > costumeList.Count - 1)
            currentCostumeIndex = 0;

        SetCurrentCostume(currentCostumeIndex);
    }
}
