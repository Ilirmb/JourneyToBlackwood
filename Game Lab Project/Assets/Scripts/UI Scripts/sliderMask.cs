using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliderMask : MonoBehaviour {

    public Slider slider;
    private playerStatistics playerStatistics;
    private frustrationFace frustrationFace;

    private float startingY = 225f;
    private float endingY = -45f;
    private float animTotalFrames = 30;
    public float animCurrentFrame = 1;
    public float animTargetFrame = 1;
    private float distancePerFrame = 0;
    

    public void onClick()
    {
        animTargetFrame = animTotalFrames;
        frustrationFace.toggleDragging(true);
    }

    //OnDragEnd and OnPointerUp activate a little differently so we have a backup function to make sure that the mask gets where it needs to be
    //This function will always be called when the mouse is released off of the object, while the above function is only acitvated if there's a 'drag' in progress when the mouse button is released
    public void onPointerUp()
    {
        frustrationFace.toggleDragging(false);
        playerStatistics.onSliderValueChange(slider.value);
        animTargetFrame = 1;
        //We now move the large slider handle back to the top so it can be clicked on again
        //If we want to this can also trigger it leaving a temporary copy of itself in place so it doesn't appear to teleport back to the top as it should now
        slider.value = 0;
    }

    // Use this for initialization
    void Start () {
        slider = GetComponentInChildren<Slider>();
        playerStatistics = GameObject.Find(GameConst.PLAYER_OBJECT_NAME).GetComponent<playerStatistics>();
        frustrationFace = transform.parent.GetComponentInChildren<frustrationFace>();

        distancePerFrame = (endingY - startingY) / animTotalFrames;
    }
	
	// Update is called once per frame
	void Update () {
        if (animCurrentFrame < animTargetFrame)
        {
            // utter jank here. to keep the position of the slider constant we detatch the slider before moving the mask, then reattach it before the frame has ended
            transform.DetachChildren();
            transform.localPosition = new Vector3(transform.localPosition.x, startingY + (animCurrentFrame * distancePerFrame), transform.localPosition.z);
            slider.transform.SetParent(gameObject.transform);

            animCurrentFrame++;
        }
        else if (animCurrentFrame > animTargetFrame)
        {
            transform.DetachChildren();
            transform.localPosition = new Vector3(transform.localPosition.x, startingY + (animCurrentFrame * distancePerFrame), transform.localPosition.z);
            slider.transform.SetParent(gameObject.transform);

            animCurrentFrame--;
        }
        else if (animCurrentFrame == 1)
        {
            transform.DetachChildren();
            transform.localPosition = new Vector3(transform.localPosition.x, startingY, transform.localPosition.z);
            slider.transform.SetParent(gameObject.transform);
        }
        else
        {
            transform.DetachChildren();
            transform.localPosition = new Vector3(transform.localPosition.x, endingY, transform.localPosition.z);
            slider.transform.SetParent(gameObject.transform);
        }
    }
}
