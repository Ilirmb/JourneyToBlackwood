using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    Game2DWaterKit.Game2DWater waterScript;
    GameObject Player;
    public bool rapidWaves;
   public float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        waterScript = gameObject.GetComponent<Game2DWaterKit.Game2DWater>();

    }
    // Update is called once per frame
    void Update()
    {
        if (rapidWaves)
        {
          //  if (onRiverLog) { Player.GetComponent<CustomPlatformerCharacter2D>().m_MaxSpeed = 3f; }
              

            waterScript.ConstantRipplesModule.Disturbance = 1.5f;

        }else if (!rapidWaves)
        {
            waterScript.ConstantRipplesModule.Disturbance = 0.1f;

        }
    }
  
    public IEnumerator RoughWaters()
    {
       
        while (timer < 0.4f)
        {
            timer += Time.deltaTime;
            float offset_Y = waterScript.gameObject.GetComponent<BoxCollider2D>().offset.y;

           // waterScript.gameObject.GetComponent<BuoyancyEffector2D>().surfaceLevel = 3f;

                waterScript.gameObject.GetComponent<BoxCollider2D>().offset =
                new Vector2(0, waterScript.gameObject.GetComponent<BoxCollider2D>().offset.y + 0.3f);
                yield return new WaitForSeconds(.25f);
                Debug.Log("increase");

         //   waterScript.gameObject.GetComponent<BuoyancyEffector2D>().surfaceLevel = 2.5f;

                waterScript.gameObject.GetComponent<BoxCollider2D>().offset =
                new Vector2(0, waterScript.gameObject.GetComponent<BoxCollider2D>().offset.y - 0.6f);
                yield return new WaitForSeconds(.25f);
                Debug.Log("decrease");

           // waterScript.gameObject.GetComponent<BuoyancyEffector2D>().surfaceLevel = 2f;

                waterScript.gameObject.GetComponent<BoxCollider2D>().offset =
                new Vector2(0, waterScript.gameObject.GetComponent<BoxCollider2D>().offset.y + 0.3f);
                yield return new WaitForSeconds(.25f);
                Debug.Log("decrease");


        }
        waterScript.gameObject.GetComponent<BuoyancyEffector2D>().surfaceLevel = 3f;

        rapidWaves = false;
        StopCoroutine(RoughWaters());
        yield return null;
    }
}
