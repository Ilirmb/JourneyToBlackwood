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

    // Range of acceptable saturation values
    private float saturationRange;

    // Range of acceptable value...values
    private float valueRange;



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

        // Define ranges for skin saturation and value
        saturationRange = (Mathf.Abs(skinToneMaxSaturation) + Mathf.Abs(skinToneMinSaturation));
        valueRange = (Mathf.Abs(skinToneMaxValue) + Mathf.Abs(skinToneMinValue));

    }


    /// <summary>
    /// AdjustSkinToneValues
    /// Adjusts skin tone values (saturation and value) by the amount given
    /// </summary>
    /// <param name="amt">Amount to adjust saturation and value.</param>
    public void AdjustSkinToneValues(float amt)
    {
        skinToneVal = 0.0f + ((valueRange / 2.0f) * amt);

        // We only need to adjust saturation for darker skin tones.
        if (amt < 0.0f)
            skinToneSat = 0.0f + ((saturationRange / 2.0f) * -amt);
        else
            skinToneSat = 0.0f;
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
