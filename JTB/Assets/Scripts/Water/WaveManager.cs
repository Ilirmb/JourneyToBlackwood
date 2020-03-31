using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    Game2DWaterKit.Game2DWater waterScript;
    GameObject Player;
    public bool rapidWaves = false;
    public bool isEffected = true;
    public float timer = 0;
    private float maxSpeed;
    private float jumpForce;
    
    public Animator playerAnimator;
    public CustomPlatformerCharacter2D playerController;

    public GameObject InvisWave;
    public GameObject InvisWave2;

    public GameObject waveSpawnLoc;
    public GameObject waveSpawnLoc2;

    private List<GameObject> waves;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        waterScript = gameObject.GetComponent<Game2DWaterKit.Game2DWater>();
        maxSpeed = playerController.m_GroundedSpeed;
        jumpForce = playerController.m_JumpForce;
        waves = new List<GameObject>();
        waterScript.GetComponent<BuoyancyEffector2D>().surfaceLevel = 3.15f;
    }
    // Update is called once per frame
    void Update()
    {
        if (rapidWaves)
        {
           // Player.GetComponent<CustomPlatformerCharacter2D>().isCrouching = true;
            //Player.GetComponent<CustomPlatformerCharacter2D>().m_MaxSpeed = 6;
            //Player.GetComponent<CustomPlatformerCharacter2D>().m_JumpForce = 600;
            waterScript.ConstantRipplesModule.Disturbance = 1.2f;
            //waterScript.ConstantRipplesModule.SmoothingFactor = .8f;

        }
        else if (!rapidWaves)
        {
           // Player.GetComponent<CustomPlatformerCharacter2D>().isCrouching = false;
            waterScript.ConstantRipplesModule.Disturbance = 0.01f;
            //waterScript.ConstantRipplesModule.SmoothingFactor = 1f;
           // Player.GetComponent<CustomPlatformerCharacter2D>().m_MaxSpeed = 10;
           // Player.GetComponent<CustomPlatformerCharacter2D>().m_JumpForce = 1000;
        }
        //Kind of sloppy but because of the multiple wave managers and not wanting to add too much to the player controller it's just easiest to check every frame despite the increased load
        //a definite place for improvement if efficiency becomes a problem
        if(rapidWaves && !isEffected)
        {
            playerController.isCrouching = false;
            playerAnimator.SetBool("Stable", true);
        }
        else if(rapidWaves && isEffected)
        { 
            playerController.isCrouching = true;
            playerAnimator.SetBool("Stable", false);
        }
    }
  
    public IEnumerator RoughWaters()
    { 
        yield return new WaitUntil(() => playerController.m_Grounded = true);

        playerController.isCrouching = true;
        //Yes we do have to have both a trigger and a bool, to prevent the Any State from looping the animation over and over again [depreciated]
        playerAnimator.SetTrigger("Unstable");
        playerAnimator.SetBool("Stable", false);
        while (timer <= 0.3f)
        {
            timer += Time.deltaTime;

            //float offset_Y = waterScript.gameObject.GetComponent<BoxCollider2D>().offset.y;


            //    waterScript.gameObject.GetComponent<BoxCollider2D>().offset =
            //    new Vector2(0, waterScript.gameObject.GetComponent<BoxCollider2D>().offset.y + 0.3f);
                yield return new WaitForSeconds(.25f);


            //    waterScript.gameObject.GetComponent<BoxCollider2D>().offset =
            //    new Vector2(0, waterScript.gameObject.GetComponent<BoxCollider2D>().offset.y - 0.6f);
                yield return new WaitForSeconds(.25f);


            //    waterScript.gameObject.GetComponent<BoxCollider2D>().offset =
            //    new Vector2(0, waterScript.gameObject.GetComponent<BoxCollider2D>().offset.y + 0.3f);
                yield return new WaitForSeconds(.25f);


        }
       // waterScript.gameObject.GetComponent<BuoyancyEffector2D>().surfaceLevel = 3.5f;
        StartCoroutine(DeleteWaves());
        Player.GetComponent<CustomPlatformerCharacter2D>().isCrouching = false;
        rapidWaves = false;
        timer = 0;
        StopCoroutine(RoughWaters());
        playerAnimator.SetBool("Stable", true);
    }

    public IEnumerator SpawnWaves()
    {
        yield return new WaitUntil(() => playerController.m_Grounded = true);
        GameObject newWave = (GameObject)Instantiate(InvisWave, waveSpawnLoc.transform.position, waveSpawnLoc.transform.rotation);
        waves.Add(newWave);
        GameObject newWave2 = (GameObject)Instantiate(InvisWave2, waveSpawnLoc2.transform.position, waveSpawnLoc2.transform.rotation);
        waves.Add(newWave2);
        yield return null;
    }

    public IEnumerator DeleteWaves()
    {
        foreach (GameObject wave in waves)
        {
            Destroy(wave);
        }
        waves.Clear();
        yield return null;
    }

    //A more generalized public version that essentially automatically starts the waves
    public void startRoughWaves()
    {
        Debug.Log("TriggeringWaves");
        rapidWaves = true;
        StartCoroutine("RoughWaters");
        StartCoroutine("SpawnWaves");
    }

    /// <summary>
    /// DeleteWaves is automatically called by start Rough Waves with a delay. The use of stopWaves is mainly to get the player to stop wobbling and stop being slowed.
    /// </summary>
    public void stopWaves()
    {
        StartCoroutine("DeleteWaves");
        playerController.isCrouching = false;
        playerAnimator.SetBool("Stable", true);
    }
}
