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
            waveManager.startRoughWaves();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            waveManager.stopWaves();

            this.gameObject.SetActive(false);

        }
    }
}
