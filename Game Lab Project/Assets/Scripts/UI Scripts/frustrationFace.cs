using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrustrationFace : MonoBehaviour {

    public Sprite happySprite;
    public Sprite ehhSprite;
    public Sprite frustratedSprite;
    private Image image;
    private Slider slider;

    private bool isDragging = false;

    public void toggleDragging(bool state)
    {
        isDragging = state; 
    }

    // Use this for initialization
    void Start ()
    {
        image = GetComponent<Image>();
        image.sprite = happySprite;

        slider = transform.parent.GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update () {
        if(isDragging == true)
        {
            if (slider.value <= 3.33)
            {
                image.sprite = happySprite;
            }
            else if (slider.value >= 3.33 && slider.value <= 6.66)
            {
                image.sprite = ehhSprite;
            }
            else
            {
                image.sprite = frustratedSprite;
            }
        }
    }
}
