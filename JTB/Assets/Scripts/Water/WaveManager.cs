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
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        waterScript = gameObject.GetComponent<Game2DWaterKit.Game2DWater>();
        maxSpeed = Player.GetComponent<CustomPlatformerCharacter2D>().m_MaxSpeed;
        jumpForce = Player.GetComponent<CustomPlatformerCharacter2D>().m_JumpForce;
    }
    // Update is called once per frame
    void Update()
    {
        if (rapidWaves)
        {
            //  if (onRiverLog) { Player.GetComponent<CustomPlatformerCharacter2D>().m_MaxSpeed = 3f; }
            Debug.Log("?");
            Player.GetComponent<CustomPlatformerCharacter2D>().m_MaxSpeed = 6;
            Player.GetComponent<CustomPlatformerCharacter2D>().m_JumpForce = 300;
            waterScript.ConstantRipplesModule.Disturbance = 1.5f;

        }else if (!rapidWaves)
        {
            waterScript.ConstantRipplesModule.Disturbance = 0.1f;
            Player.GetComponent<CustomPlatformerCharacter2D>().m_MaxSpeed = maxSpeed;
            Player.GetComponent<CustomPlatformerCharacter2D>().m_JumpForce = jumpForce;
        }
    }
  
    public IEnumerator RoughWaters()
    {

        while (timer <= 0.3f)
        {
            timer += Time.deltaTime;

            float offset_Y = waterScript.gameObject.GetComponent<BoxCollider2D>().offset.y;


                waterScript.gameObject.GetComponent<BoxCollider2D>().offset =
                new Vector2(0, waterScript.gameObject.GetComponent<BoxCollider2D>().offset.y + 0.3f);
                yield return new WaitForSeconds(.25f);


                waterScript.gameObject.GetComponent<BoxCollider2D>().offset =
                new Vector2(0, waterScript.gameObject.GetComponent<BoxCollider2D>().offset.y - 0.6f);
                yield return new WaitForSeconds(.25f);


                waterScript.gameObject.GetComponent<BoxCollider2D>().offset =
                new Vector2(0, waterScript.gameObject.GetComponent<BoxCollider2D>().offset.y + 0.3f);
                yield return new WaitForSeconds(.25f);


        }
        waterScript.gameObject.GetComponent<BuoyancyEffector2D>().surfaceLevel = 3.5f;

        rapidWaves = false;
        timer = 0;
        StopCoroutine(RoughWaters());
        yield return null;
    }
}
