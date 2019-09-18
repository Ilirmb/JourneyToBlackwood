using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrustrationFace : MonoBehaviour
{
    public Sprite happySprite;
    public Sprite ehhSprite;
    public Sprite frustratedSprite;
    private Image image;
    private Slider slider;

    private Image eyes;
    private Image hair;
    private Image hairBack;
    private Image face;

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

        hairBack = transform.GetChild(0).GetComponent<Image>();
        eyes = transform.GetChild(1).GetComponent<Image>();
        face = transform.GetChild(2).GetComponent<Image>();
        hair = transform.GetChild(3).GetComponent<Image>();

        Material skinMaterial = new Material(Shader.Find("Custom/HSVRangeShader"));

        skinMaterial.SetFloat("_HSVRangeMin", CustomizationManager.instance.GetHSVRangeMin());
        skinMaterial.SetFloat("_HSVRangeMax", CustomizationManager.instance.GetHSVRangeMax());

        skinMaterial.SetVector("_HSVAAdjust",
            new Vector4(0.0f, CustomizationManager.instance.GetSkinSat(), CustomizationManager.instance.GetSkinVal(), 0.0f));

        face.material = skinMaterial;

        hair.sprite = CustomizationManager.instance.GetPlayerHair();
        hairBack.sprite = CustomizationManager.instance.GetPlayerHairExtra();
        eyes.sprite = CustomizationManager.instance.GetPlayerEyes();
        face.sprite = CustomizationManager.instance.GetPlayerFace();
    }

    // Update is called once per frame
    void Update () {
       if(isDragging == true)
        {
            if (slider.value <= 3.33)
            {
                face.sprite = happySprite;
            }
            else if (slider.value >= 3.33 && slider.value <= 6.66)
            {
                face.sprite = ehhSprite;
            }
            else
            {
                face.sprite = frustratedSprite;
            }
        }
       
    }
}
