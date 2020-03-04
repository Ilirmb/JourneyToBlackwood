using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    Game2DWaterKit.Game2DWater waterScript;
    GameObject Player;
    public bool rapidWaves = false;
    public float timer = 0;
    private float maxSpeed;
    private float jumpForce;
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
        maxSpeed = Player.GetComponent<CustomPlatformerCharacter2D>().m_MaxSpeed;
        jumpForce = Player.GetComponent<CustomPlatformerCharacter2D>().m_JumpForce;
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

        }else if (!rapidWaves)
        {
           // Player.GetComponent<CustomPlatformerCharacter2D>().isCrouching = false;
            waterScript.ConstantRipplesModule.Disturbance = 0.1f;
           // Player.GetComponent<CustomPlatformerCharacter2D>().m_MaxSpeed = 10;
           // Player.GetComponent<CustomPlatformerCharacter2D>().m_JumpForce = 1000;
        }
    }
  
    public IEnumerator RoughWaters()
    {

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
        yield return null;
    }

    public IEnumerator SpawnWaves()
    {
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
}
