using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CustomizationManager : MonoBehaviour {

    // Static singleton instance that allows customization data to be accessed from any script.
    public static CustomizationManager instance = null;

    // Min and max ranges of color the material should target.
    [SerializeField]
    private float HSVRangeMin = 0.04f;
    [SerializeField]
    private float HSVRangeMax = 0.08f;

    // Current selected skin tone saturation
    private float skinToneSat = 0.0f;
    public float SkinToneSat { get; }

    // Current selected skin tone value
    private float skinToneVal = 0.0f;
    public float SkinToneVal { get; }

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

    // List of all hairstyles in the game
    [SerializeField]
    private List<CostumeData> hairList = new List<CostumeData>();

    // List of all faces in the game
    [SerializeField]
    private List<CostumeData> faceList = new List<CostumeData>();


    // Current Selected costume
    private CostumeData currentCostume;

    // Current Selected costume
    private CostumeData currentHair;

    // Current Selected costume
    private CostumeData currentFace;



    // Current costume index
    private int currentCostumeIndex, currentHairIndex, currentFaceIndex;
    public int CurrentCostumeIndex{ get; }
    public int CurrentHairIndex { get; }
    public int CurrentFaceIndex { get; }


    // Event that is called whenever the skin color is changed
    [HideInInspector]
    public UnityEvent OnSkinChanged;

    // Event that is called whenever the costume is changed
    [HideInInspector]
    public UnityEvent OnCostumeChanged;

    // Event that is called whenever the hairstyle is changed
    [HideInInspector]
    public UnityEvent OnHairStyleChanged;


    // Event that is called whenever the face is changed
    [HideInInspector]
    public UnityEvent OnFaceChanged;



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

        // Set current hairstyle to the first in the list if the list is not empty
        currentHair = hairList.Count > 0 ? hairList[0] : null;
        currentHairIndex = hairList.Count > 0 ? 0 : -1;

        // Set current face to the first in the list if the list is not empty
        currentFace = faceList.Count > 0 ? faceList[0] : null;
        currentFaceIndex = faceList.Count > 0 ? 0 : -1;

       
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
    /// GetCurrentHairStyle
    /// Returns the current selected hairstyle
    /// </summary>
    /// <returns>The current selected hairstyle</returns>
    public CostumeData GetCurrentHairStyle()
    {
        return currentHair;
    }


    /// <summary>
    /// GetCurrentFace
    /// Returns the current selected face
    /// </summary>
    /// <returns>The current selected face</returns>
    public CostumeData GetCurrentFace()
    {
        return currentFace;
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
    /// SetCurrentHairStyle
    /// Sets the current hairstyle to the given index
    /// </summary>
    /// <param name="index">Hairstyle to set</param>
    public void SetCurrentHairStyle(int index)
    {
        // Set the hairstyle to the index if it is in bounds, otherwise, set it to null
        currentHair = (index >= hairList.Count || index < 0) ? null : hairList[index];
        currentHairIndex = (index >= hairList.Count || index < 0) ? -1 : index;

        OnHairStyleChanged.Invoke();
    }


    /// <summary>
    /// SetCurrentFace
    /// Sets the current face to the given index
    /// </summary>
    /// <param name="index">Face to set</param>
    public void SetCurrentFace(int index)
    {
        // Set the hairstyle to the index if it is in bounds, otherwise, set it to null
        currentFace = (index >= faceList.Count || index < 0) ? null : faceList[index];
        currentFaceIndex = (index >= faceList.Count || index < 0) ? -1 : index;

        OnFaceChanged.Invoke();
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
  

    /// <summary>
    /// AdvanceCurrentHairStyle
    /// Advances to the next hairstyle in the list moving in the given direction
    /// </summary>
    /// <param name="dir">Direction to scroll</param>
    public void AdvanceCurrentHairStyle(int dir)
    {
        currentHairIndex += dir;

        if (currentHairIndex < 0)
            currentHairIndex = hairList.Count - 1;

        if (currentHairIndex > hairList.Count - 1)
            currentHairIndex = 0;

        SetCurrentHairStyle(currentHairIndex);
    }


    /// <summary>
    /// AdvanceCurrentFace
    /// Advances to the next face in the list moving in the given direction
    /// </summary>
    /// <param name="dir">Direction to scroll</param>
    public void AdvanceCurrentFace(int dir)
    {
        currentFaceIndex += dir;

        if (currentFaceIndex < 0)
            currentFaceIndex = faceList.Count - 1;

        if (currentFaceIndex > faceList.Count - 1)
            currentFaceIndex = 0;

        SetCurrentFace(currentFaceIndex);
    }


    /// <summary>
    /// GetHSVRangeMin
    /// </summary>
    /// <returns></returns>
    public float GetHSVRangeMin()
    {
        return HSVRangeMin;
    }


    /// <summary>
    /// GetHSVRangeMax
    /// </summary>
    /// <returns></returns>
    public float GetHSVRangeMax()
    {
        return HSVRangeMax;
    }


    /// <summary>
    /// GetPlayerEyes
    /// </summary>
    /// <returns>Returns the player's eyes as a sprite, used by the UI</returns>
    public Sprite GetPlayerEyes()
    {
        foreach (CostumePiece cp in currentHair.skinMeshes)
        {
            if (cp.GetSkinTarget().Equals("Eyeris"))
                return cp.GetSpriteMesh().sprite;
        }

        return null;
    } 



    /// <summary>
    /// GetPlayerHair
    /// </summary>
    /// <returns>Returns the player's hair as a sprite, used by the UI</returns>
    public Sprite GetPlayerHair()
    {
        foreach (CostumePiece cp in currentHair.skinMeshes)
        {
            if (cp.GetSkinTarget().Equals("Hair"))
                return cp.GetSpriteMesh().sprite;
        }

        return null;
    }


    /// <summary>
    /// GetPlayerHairExtra
    /// </summary>
    /// <returns>Returns the player's hair2 (ponytail) as a sprite, used by the UI</returns>
    public Sprite GetPlayerHairExtra()
    {
        foreach (CostumePiece cp in currentHair.skinMeshes)
        {
            if (cp.GetSkinTarget().Equals("PonyTail"))
                return cp.GetSpriteMesh().sprite;
        }

        return null;
    }


    /// <summary>
    /// GetPlayerFace
    /// </summary>
    /// <returns>Returns the player's face as a sprite, used by the UI</returns>
    public Sprite GetPlayerFace()
    {
        foreach (CostumePiece cp in currentFace.skinMeshes)
        {
            if (cp.GetSkinTarget().Equals("Head"))
                return cp.GetSpriteMesh().sprite;
        }

        return null;
    }


    #region Costume Inclusion Check

    public bool IsCostumeIncluded(CostumeData cd)
    {
        return costumeList.Contains(cd);
    }



    public bool IsHairStyleIncluded(CostumeData cd)
    {
        return hairList.Contains(cd);
    }



    public bool IsFaceIncluded(CostumeData cd)
    {
        return faceList.Contains(cd);
    }



    public void AddCostume(CostumeData cd)
    {
        costumeList.Add(cd);
    }



    public void AddHairStyle(CostumeData cd)
    {
        hairList.Add(cd);
    }



    public void AddFace(CostumeData cd)
    {
        faceList.Add(cd);
    }



    public void RemoveCostume(CostumeData cd)
    {
        costumeList.Remove(cd);
    }



    public void RemoveHairStyle(CostumeData cd)
    {
        hairList.Remove(cd);
    }



    public void RemoveFace(CostumeData cd)
    {
        faceList.Remove(cd);
    }

    #endregion
}
