using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof(CustomPlatformerCharacter2D))]
public class CustomPlatformer2DUserControl : MonoBehaviour
{
    public bool touchControls = false;
    public Joystick joystick;

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
    }

    /// <summary>
    /// Called by the touchscreen button
    /// </summary>
    public void Jump()
    {
        if(touchControls)
            m_Jump = true;
    }

    private void FixedUpdate()
    {
        if (canControl)
        {
            startedMoving = false;

            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl); //<<currently not used in game
            //This inefficiency WILL be replaced soon enough, once I fully figure out this CrossPlatformInputManager stuff
            bool run = (touchControls ? Input.GetKey(KeyCode.LeftShift) : CrossPlatformInputManager.GetButton("Sprint"));
            move = (touchControls ? joystick.Horizontal : CrossPlatformInputManager.GetAxis("Horizontal"));
            // Pass all parameters to the character control script.
            m_Character.Move(move, crouch, m_Jump, run);
            m_Jump = false;

            if (previousFrame == 0 && move != 0)
                startedMoving = true;

            previousFrame = move;
        }
        else
            m_Character.Move(0, false, false, false);
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

