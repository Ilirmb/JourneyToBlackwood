using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeMenuController : MonoBehaviour {

    // Placeholder, will be deleted
    public SkinToneAdjust sta;

    [SerializeField]
    private Slider skinColorSlider;


	// Use this for initialization
	void Start () {
		
	}


    public void ChangeSkinColor()
    {
        CustomizationManager.instance.AdjustSkinToneValues(skinColorSlider.value);
        sta.SetSkinSV();
    }

}
