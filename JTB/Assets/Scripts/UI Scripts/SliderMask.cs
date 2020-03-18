using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMask : MonoBehaviour {

    public Slider slider;
    private PlayerStatistics playerStatistics;
    private FrustrationFace frustrationFace;

    //private float startingY = 300f;
    //private float endingY = 80f;



    private RectTransform backgroundTransform;
    private float scaleY = 0;
    private Image handle;

    // Use this for initialization
    void Start()
    {
        backgroundTransform = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        playerStatistics = GameObject.Find(GameConst.PLAYER_OBJECT_NAME).GetComponent<PlayerStatistics>();
        frustrationFace = transform.parent.GetComponentInChildren<FrustrationFace>();
        handle = gameObject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>();

        //make invis
        handle.color = new Color(handle.color.r, handle.color.g, handle.color.b, 0f);

        //set background bar length to zero so we can animate it extending when the players clicks
        backgroundTransform.localScale = new Vector3(1,scaleY,1);
        //  distancePerFrame = (endingY - startingY) / animTotalFrames;
    }
    IEnumerator animateSliderDOWN()
    {
        //stop coroutines from overlapping
        StopCoroutine(animateSliderUP());
        float animationDistPerFrame = .05f;
        //if the background bar isnt at full length
        for (float i = 0; i < 1; i += animationDistPerFrame)
        {
            if (backgroundTransform.localScale.y < 1.0f)
            {
                scaleY += animationDistPerFrame;
                backgroundTransform.localScale = new Vector3(backgroundTransform.localScale.x, scaleY, backgroundTransform.localScale.z);

            }
            //if bar is fully extended
            else if (backgroundTransform.localScale.y >= 1.0f)
            {
                StopCoroutine(animateSliderDOWN());
            }
            else { Debug.LogError("Error in animation"); }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    IEnumerator animateSliderUP()
    {
        //stop the coroutines from overlapping
        StopCoroutine(animateSliderDOWN());
        float animationDistPerFrame = .05f;
        //if the background bar isnt at full length, we loop until it is
        for (float i = 0; i < 1; i += animationDistPerFrame)
        {
            if (backgroundTransform.localScale.y > 0.0f)
            {
                scaleY -= animationDistPerFrame;
                backgroundTransform.localScale = new Vector3(backgroundTransform.localScale.x, scaleY, backgroundTransform.localScale.z);

            }
            //if bar is fully extended
            else if (backgroundTransform.localScale.y <= 0.0f)
            {
                StopCoroutine(animateSliderUP());
            }
            else { Debug.LogError("Error in animation"); }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    public void onClick()
    {
        Debug.Log("cli ked");
        //animate the slider by calling the coroutine
        StartCoroutine(animateSliderDOWN());
        //make visable using alpha channel
        handle.color = new Color(handle.color.r, handle.color.g, handle.color.b, 1f);
        //animTargetFrame = animTotalFrames;
        frustrationFace.toggleDragging(true);
    }

    public void onPointerUp()
    {
        StartCoroutine(animateSliderUP());
        frustrationFace.toggleDragging(false);
        playerStatistics.onSliderValueChange(slider.value);
       // animTargetFrame = 1;
        //We now move the large slider handle back to the top so it can be clicked on again
        //If we want to this can also trigger it leaving a temporary copy of itself in place so it doesn't appear to teleport back to the top as it should now
        slider.value = 0.0f;

        handle.color = new Color(handle.color.r, handle.color.g, handle.color.b, 0f);
    }
    
}

