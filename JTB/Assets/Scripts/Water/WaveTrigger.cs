using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    private WaveManager waveManager;
    private void Start()
    {

        waveManager = transform.parent.GetComponent<WaveManager>();

      
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("TriggeringWaves");
            waveManager.rapidWaves = true;
            waveManager.StartCoroutine("RoughWaters");
            waveManager.StartCoroutine("SpawnWaves");
            Debug.Log("crouching true");
            other.GetComponent<CustomPlatformerCharacter2D>().isCrouching = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("turnoff");
            waveManager.StartCoroutine("DeleteWaves");
            other.GetComponent<CustomPlatformerCharacter2D>().isCrouching = false;

            this.gameObject.SetActive(false);

        }
    }
}
