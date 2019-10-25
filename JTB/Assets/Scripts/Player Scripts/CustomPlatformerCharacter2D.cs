using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomPlatformerCharacter2D : MonoBehaviour
{
    public float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis. Unserialized to change with code
    public float m_JumpForce = 400f;                  // Amount of force added when the player jumps. Unaerialized to change with code
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    [SerializeField] private float m_RunMultiplier = 1.5f;
    private float m_GravityScale = 3.0f;
    [SerializeField] private bool m_GravityOnGround = true;
    [SerializeField] private bool m_SlideDownSlope = false;

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    private float m_TrueSpeed;
<<<<<<< HEAD
    private float m_SlidingSpeed = 55;
    private float m_SlidingDrag = 1.5f;
    const float k_GroundedRadius = .25f; // Radius of the overlap circle to determine if grounded
=======
    private float m_GroundedSpeed = 10;
    private float m_SlidingSpeed = 55;
    private float m_SlidingDrag = 1.5f;
    private float k_GroundedRadius = .5f; // Radius of the overlap circle to determine if grounded
>>>>>>> master
    public bool m_Grounded;            // Whether or not the player is grounded.
    private bool m_PrevGrounded;
    private bool m_CanCheckGrounded = true;
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator m_Anim;            // Reference to the player's animator component.
    public Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private bool m_Running = false;
    public bool ableToMove = false; //Specifically for cases when grounded is false but the character should still move as if grounded (quicksand, possibly water)
    public bool m_Sliding = false;
    public float vspeed;
    public float hspeed;

    private bool m_OnLadder = false;
    private bool m_RunLock = false;

    private PlayerStatistics playerStatistics;
    private Vector3 velocity = Vector3.zero;
    private Vector2 normal;

    public bool OnRiverLog = false;

    #region Gameplay Ref

    private Ladder ladderRef;

    #endregion


    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponentInChildren<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        //m_Rigidbody2D.gravityScale = 0.0f;

        playerStatistics = GetComponent<PlayerStatistics>();
        ableToMove = false;
<<<<<<< HEAD

        StopSliding();// temporary- remove once you reset the values from the sliding testing values. Function returns the values to their 'normal' state.
=======
        m_AirControl = true;
        StopSliding();// temporary- remove once you reset the values from the sliding testing values. Function returns the values to their 'normal' state.
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "Segment2")
        {
            k_GroundedRadius = 0.5f;
            m_JumpForce = 500f;
        }
>>>>>>> master
    }


    private void FixedUpdate()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        if (m_CanCheckGrounded)
        {
            RaycastHit2D hit = Physics2D.Raycast(m_GroundCheck.transform.position, -Vector2.up, k_GroundedRadius, m_WhatIsGround);

            if (hit.collider != null && ((m_GroundCheck.transform.position.y +10> hit.point.y) || m_OnLadder))
            {
                normal = hit.normal;
                m_Grounded = true;
                m_RunLock = false;

                if (!m_PrevGrounded)
                {
                    Debug.Log("Landed");
                }
            }
            else
                normal = Vector2.up;

            // original ground check code.

            /*Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                Debug.Log(colliders[i].gameObject.name);
                if (colliders[i].gameObject != gameObject)
                {
                    Debug.Log(colliders[i].enabled);
                    m_Grounded = true;
                    m_RunLock = false;

                    if (!m_PrevGrounded)
                    {
                        Debug.Log("Landed");
                    }
                }
            }*/
        }

        /* Gravity code. Interferes with sliding mud physics. Default rigidbody physics were used instead
        if ((m_GravityOnGround) || (!m_GravityOnGround && !m_Grounded))
        {
            m_Rigidbody2D.AddForce(Physics2D.gravity * m_GravityScale);
        }
        
        if(!m_SlideDownSlope && m_Grounded && normal.y != 1.0f)
            m_Rigidbody2D.AddForce(-Physics2D.gravity * m_GravityScale);
        */
        m_Anim.SetBool("Grounded", m_Grounded);

        // Set the vertical animation
        vspeed = m_Rigidbody2D.velocity.y;
        m_Anim.SetFloat("vSpeed", vspeed);

        m_PrevGrounded = m_Grounded;
    }
 

    public void Move(float move, bool crouch, bool jump, bool run)
    {
        // If crouching, check to see if the character can stand up
        /*if (!crouch && m_Anim.GetBool("Crouch"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }*/

        m_Running = (run && m_Grounded && Mathf.Abs(move) > 0.85f) || (m_RunLock && Mathf.Abs(move) > 0.85f);

        // Set whether or not the character is crouching in the animator
        //m_Anim.SetBool("Crouch", crouch);

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move * m_CrouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }

            // Move the character
            float tempSpeed = (m_Sliding && m_Grounded ? m_SlidingSpeed : m_MaxSpeed);
            m_TrueSpeed = (m_Running ? tempSpeed * 1.5f : tempSpeed);

            float _yVelocity = m_Grounded && !m_OnLadder ? 0.0f: m_Rigidbody2D.velocity.y;

            /*m_Rigidbody2D.velocity = m_Grounded && -normal.x <= 0 ? new Vector2(move * m_TrueSpeed * normal.y, m_yVelocity + (move * m_TrueSpeed * -normal.x)) 
                : new Vector2(move * m_TrueSpeed, m_yVelocity);*/

            m_Rigidbody2D.AddForce(new Vector2(move * m_TrueSpeed, 0f));

            
            if (m_Grounded && ((-normal.x * move) < 0) && !m_Sliding) // If grounded
            {
                m_Rigidbody2D.velocity = new Vector2((move * normal.y) * m_TrueSpeed, ((move * -normal.x) * m_TrueSpeed) + _yVelocity);
            }
            else if(!m_Sliding) // If airborn
            {
                m_Rigidbody2D.velocity = new Vector2(move * m_TrueSpeed, _yVelocity);
            }
            else // If sliding
            {
                m_Rigidbody2D.AddForce(new Vector2(move * (m_TrueSpeed/2), 0f));
            }
            //m_Rigidbody2D = new Vector2((move * normal.y) * m_TrueSpeed, ((move * -normal.x) * m_TrueSpeed) + m_yVelocity);


            m_Anim.SetFloat("Speed", Mathf.Abs(m_Rigidbody2D.velocity.x),.3f, Time.deltaTime);

            //Tells the player object if it's moving or not
            //I've decided to only allow the player to be considered moving under their own will if they are grounded, in case something else decides to move the player while they're in the air. Essentially, stamina will only be added while on the ground.
            //There are tentative plans to move this system to a local velocity based system by calculating valocity without taking into account other
            playerStatistics.moving(move != 0 && (m_Grounded || ableToMove));
        }

        /*if (!m_Grounded)
            m_Rigidbody2D.AddForce(Physics2D.gravity * m_GravityScale);*/

        // If the player should jump...
        if ((m_Grounded || m_OnLadder) && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Anim.SetBool("Grounded", false);
            Vector2 vel = new Vector2(m_Rigidbody2D.velocity.x, 0.0f);
            m_Rigidbody2D.AddForce(vel, ForceMode2D.Force);

            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce), ForceMode2D.Force);


            // Prevents an issue that occasionally stops the player from jumping due to oddities with the ground check.
            StartCoroutine(DisableGroundCheck(0.1f));

            // Set a flag that says the player was running if they were
            m_RunLock = m_Running;

            playerStatistics.damageFromJump(GameConst.STAMINA_TO_JUMP);

            if (m_OnLadder)
                m_OnLadder = false;
        }

        if(m_OnLadder && Mathf.Abs(move) > 0.75f && ladderRef.GetOnTime() <= 0.0f)
            m_OnLadder = false;
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // If the player changes direction in midair, cancel momentum
        m_RunLock = false;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    /// <summary>
    /// GetIsGrounded
    /// Returns true if the player is grounded
    /// </summary>
    /// <returns>If the player is grounded</returns>
    public bool GetIsGrounded()
    {
        return m_Grounded;
    }


    /// <summary>
    /// GetDirection
    /// Returns an int value based on what direction the player is facing
    /// </summary>
    /// <returns>1 if the player is facing right, -1 if the player is facing left</returns>
    public int GetDirection()
    {
        return m_FacingRight ? 1 : -1;
    }


    /// <summary>
    /// GetIsRunning
    /// Returns true if the player is running
    /// </summary>
    /// <returns>If the player is running</returns>
    public bool GetIsRunning()
    {
        return m_Running;
    }
    

    private IEnumerator DisableGroundCheck(float time)
    {
        m_CanCheckGrounded = false;

        yield return new WaitForSeconds(time);

        m_CanCheckGrounded = true;
    }

    public void StartSliding()
    {
        m_Sliding = true;
        m_Rigidbody2D.drag = m_SlidingDrag;
<<<<<<< HEAD
=======
        m_MaxSpeed = m_SlidingSpeed;
>>>>>>> master
    }
    public void StopSliding() 
    {
        m_Sliding = false;
        m_Rigidbody2D.drag = 0f;
<<<<<<< HEAD
=======
        m_MaxSpeed = m_GroundedSpeed;
>>>>>>> master
    }

    public void SetGravityScale(float amt)
    {
        m_GravityScale = amt;
    }


    public float GetGravityScale()
    {
        return m_GravityScale;
    }


    public void SetOnLadder(bool state, Ladder l)
    {
        m_OnLadder = state;
        ladderRef = l;
    }


    public bool GetOnLadder()
    {
        return m_OnLadder;
    }
}

