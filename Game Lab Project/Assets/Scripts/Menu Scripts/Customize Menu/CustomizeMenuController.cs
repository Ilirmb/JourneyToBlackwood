using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeMenuController : MonoBehaviour {

    [SerializeField]
    private Slider skinColorSlider;


	// Use this for initialization
	void Start () {
		
	}


    public void ChangeSkinColor()
    {
        CustomizationManager.instance.AdjustSkinToneValues(skinColorSlider.value);
    }


    public void ChangeCostume(int dir)
    {
        CustomizationManager.instance.AdvanceCurrentCostume(dir);
    }

}
