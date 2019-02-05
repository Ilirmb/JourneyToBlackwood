using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof(CustomPlatformerCharacter2D))]
public class CustomPlatformer2DUserControl : MonoBehaviour
{
    private CustomPlatformerCharacter2D m_Character;
    private bool m_Jump;
    public bool canControl = true;


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


    private void FixedUpdate()
    {
        if (canControl)
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }
    }
}

