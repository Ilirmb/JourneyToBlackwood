using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Anima2D;
public class CustomizeMenuController : MonoBehaviour
{

    [SerializeField]
    private Slider skinColorSlider;
    public int startLevelID;
    //Anima2D script
    [HideInInspector]
    private SpriteMeshInstance eyeScript;
    private SpriteMeshInstance hairScript;
    // Use this for initialization
    void Start()
    {
        // Reference the mesh instance script that is on the eyes on the character sprite
        eyeScript = GameObject.Find("MC Sprite").transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<SpriteMeshInstance>();
        hairScript = GameObject.Find("MC Sprite").transform.GetChild(1).GetChild(4).GetChild(2).GetComponent<SpriteMeshInstance>();

    }


    public void StartGame()
    {

        loadScene.LoadScene(startLevelID);
    }


    public void ChangeSkinColor()
    {
        CustomizationManager.instance.AdjustSkinToneValues(skinColorSlider.value);
    }


    public void ChangeCostume(int dir)
    {
        CustomizationManager.instance.AdvanceCurrentCostume(dir);
    }
    public void BackButton()
    {
        SceneManager.LoadScene("Main Menu");
    }

    //Eye
    public void SetBlueEyeColor()
    {
        Color blueHue = new Color(0, .7f, 1, .75f);
        eyeScript.color = blueHue;
        GlobalColor.Instance.eyeColor = blueHue;

    }
    public void SetWhiteEyeColor()
    {
        Color whiteHue = new Color(1, 1, 1, .75f);
        eyeScript.color = whiteHue;
        GlobalColor.Instance.eyeColor = whiteHue;

    }
    public void SetBrownEyeColor()
    {
        Color brownHue = new Color(.55f, .27f, .08f, .75f);
        eyeScript.color = brownHue;
        GlobalColor.Instance.eyeColor = brownHue;

    }
    public void SetNavyEyeColor()
    {
        Color navyHue = new Color(0, .34f, 1, .75f);
        eyeScript.color = navyHue;
        GlobalColor.Instance.eyeColor = navyHue;

    }

    //Hair
    public void SetYellowHairColor()
    {
        Color yellowHue = new Color(1, .72f, 0, 1);
        hairScript.color = yellowHue;
        GlobalColor.Instance.hairColor = yellowHue;

    }
    public void SetBlueHairColor()
    {
        Color blueHue = new Color(0, .7f, 1, 1);
        hairScript.color = blueHue;
        GlobalColor.Instance.hairColor = blueHue;

    }
    public void SetWhiteHairColor()
    {
        Color whiteHue = new Color(1, 1, 1, 1);
        hairScript.color = whiteHue;
        GlobalColor.Instance.hairColor = whiteHue;

    }
    public void SetBlackHairColor()
    {
        Color blackHue = new Color(.3f, .3f, .3f, 1);
        hairScript.color = blackHue;
        GlobalColor.Instance.hairColor = blackHue;

    }
    public void SetRedHairColor()
    {
        Color redHue = new Color(1, .25f, 0, 1);
        hairScript.color = redHue;
        GlobalColor.Instance.hairColor = redHue;

    }
    public void SetBrownHairColor()
    {
        Color brownHue = new Color(.55f, .27f, .08f, 1);
        hairScript.color = brownHue;
        GlobalColor.Instance.hairColor = brownHue;

    }
    public void SetNavyHairColor()
    {
        Color navyHue = new Color(0, .34f, 1, 1);
        hairScript.color = navyHue;
        GlobalColor.Instance.hairColor = navyHue;

    }

    public void ChangeHairStyle(int dir)
    {
        CustomizationManager.instance.AdvanceCurrentHairStyle(dir);
    }


    public void ChangeFace(int dir)
    {
        CustomizationManager.instance.AdvanceCurrentFace(dir);
    }

}
