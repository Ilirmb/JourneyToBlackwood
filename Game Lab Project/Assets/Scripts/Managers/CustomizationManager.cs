using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
