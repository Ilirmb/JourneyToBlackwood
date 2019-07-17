using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMask : MonoBehaviour {

    public Slider slider;
    private PlayerStatistics playerStatistics;
    private FrustrationFace frustrationFace;

    private float startingY = 300f;
    private float endingY = 80f;
    [SerializeField]
    private float animTotalFrames = 30;
    public float animCurrentFrame = 1;
    public float animTargetFrame = 1;
    private float distancePerFrame = 0;



    private Image handle;


    public void onClick()
    {
        handle.color = new Color(handle.color.r, handle.color.g, handle.color.b, 1f);
        animTargetFrame = animTotalFrames;
        frustrationFace.toggleDragging(true);
    }

    public void onPointerUp()
    {
        frustrationFace.toggleDragging(false);
        playerStatistics.onSliderValueChange(slider.value);
        animTargetFrame = 1;
        //We now move the large slider handle back to the top so it can be clicked on again
        //If we want to this can also trigger it leaving a temporary copy of itself in place so it doesn't appear to teleport back to the top as it should now
        slider.value = 0;

        handle.color = new Color(handle.color.r, handle.color.g, handle.color.b, 0f);
    }

    // Use this for initialization
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        playerStatistics = GameObject.Find(GameConst.PLAYER_OBJECT_NAME).GetComponent<PlayerStatistics>();
        frustrationFace = transform.parent.GetComponentInChildren<FrustrationFace>();

        handle = gameObject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>();
        handle.color = new Color(handle.color.r, handle.color.g, handle.color.b, 0f);

        distancePerFrame = (endingY - startingY) / animTotalFrames;
    }

    // Update is called once per frame
    void Update()
    {
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

