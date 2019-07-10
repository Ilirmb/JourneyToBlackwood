using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof(CustomPlatformerCharacter2D))]
public class CustomPlatformer2DUserControl : MonoBehaviour
{
    private CustomPlatformerCharacter2D m_Character;
    private bool m_Jump;
    public bool canControl = true;
    public float move;

    private float previousFrame = 0.0f;
    private bool startedMoving = false;


    private void Awake()
    {
        m_Character = GetComponent<CustomPlatformerCharacter2D>();
    }


    private void Update()
    {
        if (!m_Jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        // placeholder
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameManager.instance.AffectSocialValue("strang", -278);
            GameManager.instance.AffectSocialValue("straeng", -278);
            GameManager.instance.AffectSocialValue("strang_rg&^&^&^nerkjgbeb", -278);
            GameManager.instance.AffectSocialValue("socialstrang", -278);
            GameManager.instance.AffectSocialValue("social_strang", -278);
            GameManager.instance.AffectSocialValue("social_strung", -278);
            GameManager.instance.AffectSocialValue("socialStrang", -278);
            GameManager.instance.AffectSocialValue("socialResponsibility", -278);
            GameManager.instance.AffectSocialValue("socialRecoGnition", -278);
        }
    }


    private void FixedUpdate()
    {
        if (canControl)
        {
            startedMoving = false;

            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            bool run = Input.GetKey(KeyCode.LeftShift);
            move = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(move, crouch, m_Jump, run);
            m_Jump = false;

            if (previousFrame == 0 && CrossPlatformInputManager.GetAxis("Horizontal") != 0)
                startedMoving = true;

            previousFrame = CrossPlatformInputManager.GetAxis("Horizontal");
        }
    }


    /// <summary>
    /// GetStartedMoving
    /// Determines if the player started moving on this frame
    /// </summary>
    /// <returns>Whether or not the player started moving on this frame</returns>
    public bool GetStartedMoving()
    {
        return startedMoving;
    }
}

