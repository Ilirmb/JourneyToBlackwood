﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Anima2D;
using Cinemachine;
public class PlayerStatistics : MonoBehaviour
{
    public CustomPlatformerCharacter2D playerCharacter;
    public StamLossTextManager textSpawn;
    public Checkpoint checkpoint;

    public float invulnTimer = 0;
    private float damageOverTime = 0;
    public float stamina;
    public float maxStamina = 100;
    public float respawnTimer = 20000f;

    Scene scene;
    string sceneName;

    // Frustration variables
    public float frustration = 0;
    private bool highFrustration;
    private int frustrationCount;
    private int highFrustrationCount;
    private float timeLeft = 15f;
    private bool increaseStamReduction = false;

    [Tooltip("The distance a player has to walk before they take one 'GameConst.STAMINA_DRAIN_PER_DISTANCE_WALKED' worth of stamina damage")]
    public float walkDistanceToDamageStam = 4.0f;
    private bool isMoving = false;
    private Vector2 positionLastFrame;
    public bool hittingWater = false;

    // The player's body. Some old behavior relating to invul flashing no longer works sense the player is no longer a single sprite
    private GameObject playerBody;

    //resets require knowing the loadscene object
    public loadScene ls;

    public int numPlayerDeaths = 0;

    private SpriteMeshInstance eyeScript;
    private SpriteMeshInstance hairScript;

    private bool warping = false;
    public GameObject TopOfHead;

    // Use this for initialization
    void Awake()
    {
        scene = SceneManager.GetActiveScene();
        sceneName = scene.name;
        eyeScript = GameObject.Find("MC Sprite").transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<SpriteMeshInstance>();
        hairScript = GameObject.Find("MC Sprite").transform.GetChild(1).GetChild(4).GetChild(2).GetComponent<SpriteMeshInstance>();
        UpdateColors();
        if (eyeScript == null || hairScript == null)
            Debug.LogError("Player hair and eye customization did not load");

        //The idea here is to create a Checkpoint at the location of the player, but it's not working and doesn't need to because 
        //Checkpoint = new Checkpoint(gameObject.transform.position);
        positionLastFrame = transform.position;

        for (int i = 0; i < transform.childCount; i++)
        {
            // Gets the player's body by name. This is far from optimal since the name may be different.
            if (transform.GetChild(i).gameObject.name.Equals("MC Sprite"))
                playerBody = transform.GetChild(i).transform.GetChild(1).gameObject;
        }

        stamina = maxStamina;
    }

    public void UpdateColors()
    {
        eyeScript.color = GlobalColor.Instance.eyeColor;
        hairScript.color = GlobalColor.Instance.hairColor;
    }


    // Update is called once per frame
    void Update()
    {
        GameObject owl = GameObject.FindGameObjectWithTag("Owl");
        CheckIfDead();

        if (warping == true)
        {
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            owl.GetComponent<OwlMovement>().enabled = false;
            owl.transform.position = Vector2.MoveTowards(owl.transform.position, TopOfHead.transform.position, 2.0f);
            transform.position = Vector2.MoveTowards(transform.position, checkpoint.transform.position, 1.0f);
            if (transform.position == checkpoint.transform.position)
            {
                Debug.Log("Releasing from warp");
                warping = false;
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                gameObject.GetComponent<CustomPlatformer2DUserControl>().enabled = true;
                stamina = 100f;
                hittingWater = false;
                invulnTimer = 1.5f;
                respawnTimer = 200;
                PlayerMovement.m_MaxSpeed = 10f;
                PlayerMovement.m_JumpForce = 400f;
                respawnTimer = 2000f;
                owl.GetComponent<OwlMovement>().enabled = true;
                gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }

        if (invulnTimer > 0)
        {
            invulnTimer -= Time.deltaTime;

            //Flickers sprite by turning the game renderer on and off every couple of frames
            if (Time.frameCount % 5 == 0)
            {
                playerBody.SetActive(!playerBody.activeInHierarchy);// = !gameObject.GetComponent<SpriteRenderer>().enabled;
            }
            //If this is the last frame where the character has invulnerability, then the sprite should be set to rendered
            if (invulnTimer <= 0)
            {
                playerBody.SetActive(true);
                //For future math reasons we reset the timer to zero because time.delta time can make it less than that.
                invulnTimer = 0;
            }
        }

        if (isMoving)
        {
            float distanceSinceLastFrame = Mathf.Abs((transform.position.x - positionLastFrame.x)); //In unity distance units
            damageFromMoving(distanceSinceLastFrame * (GameConst.STAMINA_DRAIN_PER_DISTANCE_WALKED / walkDistanceToDamageStam));
        }

        positionLastFrame = new Vector2(transform.position.x, transform.position.y);

        //If eaten apple or mushrooms, increase stamina reduction
        if (maxStamina > 100 && !increaseStamReduction) {
            increaseStamReduction = true;
        }

        //Turn off the increased stamina reduction in the event that max stamina is set back to normal
        else if (maxStamina <= 100 && increaseStamReduction) {
            increaseStamReduction = false;
        }
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public float getStamina()
    {
        return stamina;
    }


    public float getFrustration()
    {
        return frustration;
    }


    public void moving(bool m)
    {
        isMoving = m;
    }


    public void setStamina(float newStamina)
    {
        stamina = newStamina;
        if (stamina > maxStamina) stamina = maxStamina;
    }


    public void increaseMaxStamina(float maxStamIncrease)
    {
        maxStamina += maxStamIncrease;
        stamina += maxStamIncrease;
        textSpawn.spawnText(string.Format("+{0:0.##}", maxStamIncrease), new Color(0, 150, 255));
    }


    public void setFrustration(float newFrustration)
    {
        frustration = newFrustration;
    }


    //This function is called to translate the value of the slider into frustration
    public void onSliderValueChange(float sliderValue)
    {
        //Debug.Log("Slider Value Change works");
        //Slider value is between 0 and 10 so we can multiply it by itself to get an exponential curve between 0 and 100
        frustration = (Mathf.Pow(sliderValue, 2));

        // Player has felt frustrated.
        if ((Mathf.Pow(sliderValue, 2)) > frustration)
            frustrationCount++;

        // High frustration
        if (frustration >= 70.0f)
        {
            // Number of times player has been at high frustration.
            highFrustrationCount++;

            // Show a health tip if the player has been at high frustration too many times
            if (highFrustrationCount > 3)
            {
                GameManager.instance.ShowHealthTip();
                highFrustrationCount = 0;
            }
            else
                GameManager.instance.OfferHint();
        }
    }



    private float reduceDamageByFrustration(float damage)
    {
        //Conveniently because every damage value is passed through here it's a convenient place to do things relating to every time the player takes damage
        GameManager.instance.AffectStatValue("Total Damage Taken", damage);

        // Frustration is a value between 1 and 100. This is convenient because it essentially gives a percentage. 
        //We can invert it by taking 100 and subtracting frustration, then dividing by a hundred to get the percentage to apply to damage
        // hopefully
        float reduceddamage = (damage * ((100 - frustration) / 100));

        if (increaseStamReduction) {
            reduceddamage = ((damage * 1.5f) * ((100 - frustration) / 100));
        }

        GameManager.instance.AffectStatValue("Total Damage After Frustration Reduction", reduceddamage);
        GameManager.instance.AffectStatValue("Health saved by Frustration", damage - reduceddamage);
        return reduceddamage;
    }



    public void recoverStamina(float recoveredStamina)
    {
        stamina += recoveredStamina;
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
        textSpawn.spawnText(string.Format("+{0:0.##}", recoveredStamina), new Color(255, 150, 0));
    }


    /// <summary>
    /// The player takes damage, but no invulnerability timer is activated. However, if the player is invulnerable, they wont take damage from this method.
    /// To deal damage while invulnerability is active, use damageInvulnImmune()
    /// </summary>
    /// <param name="staminaDamage">Damage to be dealt</param>
    public void damageStamina(float damage)
    {
        reduceDamageByFrustration(damage);
        if (invulnTimer <= 0)
        {
            stamina -= damage;
            textSpawn.spawnText(string.Format("{0:0.##}", damage), new Color(255, 0, 0));
            CheckIfDead();
        }

    }


    /// <summary>
    /// The player takes stamina damage and an invulnerability timer is activated, where the player can't take damage
    /// </summary>
    /// <param name="damage">Damage to be dealt</param>
    /// <param name="invuln">How long in seconds invulnerability should last</param>
    public void damageStamina(float damage, float invuln)
    {
        damage = reduceDamageByFrustration(damage);
        if (invulnTimer <= 0)
        {
            stamina -= damage;
            textSpawn.spawnText(string.Format("{0:0.##}", damage), new Color(255, 0, 0));
            invulnTimer = invuln;
            //CheckIfDead();
        }

    }


    /// <summary>
    /// This damage is immune from the effects of the invulnerability timer
    /// mostly useful for bottomless drops
    /// </summary>
    /// <param name="damage">Damage dealt</param>
    /// <param name="invuln">How long should invulnerability last. 0 = no invulnerability</param>
    public void damageInvulnImmune(float damage, float invuln)
    {
        damage = reduceDamageByFrustration(damage);
        stamina -= damage;
        //This text lingers for longer than normal
        textSpawn.spawnText(string.Format("{0:0.##}", damage), new Color(255, 0, 255), 4f);
        if (invuln > 0)
        {
            invulnTimer = invuln;
            CheckIfDead();
        }

    }


    //No one outside of programmers should need to use these and they're pretty self explanitory, so I didn't summarize them
    public void damageFromMoving(float damage)
    {
        damage = reduceDamageByFrustration(damage);
        stamina -= damage;
        damageOverTime += damage;
        //if (++textFrameTimer == 20)
        if (damageOverTime >= 1.0f)
        {
            textSpawn.spawnText(string.Format("{0:0.##}", 1.0f), new Color(0, 255, 0));
            //textFrameTimer = 0;
            damageOverTime = 0;
            CheckIfDead();
        }

    }


    public void damageFromJump(float damage)
    {
        GameManager.instance.AffectStatValue("Jumps", 1);
        damage = reduceDamageByFrustration(damage);
        stamina -= damage;
        textSpawn.spawnText(string.Format("{0:0.##}", damage), new Color(255, 255, 0));
        CheckIfDead();
    }


    public void lastCheckpoint(Checkpoint newCheckpoint)
    {
        if (checkpoint != newCheckpoint)
        {
            stamina = maxStamina;
            if (checkpoint != null)
            {
                checkpoint.becomeInactive();
            }
        }
        checkpoint = newCheckpoint;
    }
    //creates respawn timer for when the player is dead
    IEnumerator deathTimer()
    {
        gameObject.GetComponent<CustomPlatformer2DUserControl>().enabled = false;
        Debug.Log(scene.name);

        ////If colliding with water, wait longer so player disappears below surface
        //if (hittingWater)
        //{

        //    yield return new WaitForSeconds(.75f);
        //}
        ////Shorter wait via spikes and whatnot
        //else
        //{
        //    yield return new WaitForSeconds(.3f);
        //}

        //yield return new WaitForSeconds(.75f);


        //if Checkpoint is null, just reload the scene
        if (checkpoint == null)
        {

            //If colliding with water, wait longer so player disappears below surface
            if (hittingWater)
            {

                yield return new WaitForSeconds(.75f);
            }
            //Shorter wait via spikes and whatnot
            else
            {
                yield return new WaitForSeconds(.7f);
            }
            // Restart if stamina is equal to or less than 0
            // Pretty blunt way of reloading, reloads the current scene
            ReloadCurrentScene();
        }
        //Otherwise go to Checkpoint
        else
        {

            //If colliding with water, wait longer so player disappears below surface
            if (hittingWater)
            {
                PlayerMovement.m_MaxSpeed = 0f;
                PlayerMovement.m_JumpForce = 0f;
                yield return new WaitForSeconds(.75f);
            }
            //Shorter wait via spikes and whatnot
            else
            {
                PlayerMovement.m_MaxSpeed = 0f;
                PlayerMovement.m_JumpForce = 0f;
                yield return new WaitForSeconds(.7f);
            }

            if (sceneName == "Scene 2")
            {
                ReloadAtCheckpoint();
            }
            else
            {
                CarryBackToCheckpoint();
            }
            

            // Invokes the player death event
            GameManager.instance.OnPlayerDeath.Invoke();

            numPlayerDeaths++;
            GameManager.instance.AffectStatValue("Num Deaths", 1);
            playerCharacter.StopSliding();
        }
    }

    public void CheckIfDead()
    {
        if (stamina <= 0)
        {
            Debug.Log("you are dead");
            StartCoroutine(deathTimer());
        }
    }

    public void ReloadAtCheckpoint()
    {
        Debug.Log("Success");
        gameObject.GetComponent<Rigidbody2D>().MovePosition(checkpoint.transform.position);
        Debug.Log("Moving player character to the position of the last checkpoint hit");
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        gameObject.GetComponent<CustomPlatformer2DUserControl>().enabled = true;
        stamina = 100f;
        hittingWater = false;
        invulnTimer = 1.5f;

        transform.position = checkpoint.transform.position;
        Debug.Log("Resetting camera");

        respawnTimer = 200;
        PlayerMovement.m_MaxSpeed = 10f;
        PlayerMovement.m_JumpForce = 400f;
        respawnTimer = 2000f;
    }

    public void CarryBackToCheckpoint()
    {
        warping = true;
    }
}
