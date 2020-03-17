using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    private WaveManager waveManager;
    public bool exitTrigger;
    public WaveTrigger enterTriger; //If it's an exit, put the entrance trigger here so that it can deactivate it
    //A more robust code would include some sort of randomization variable. too bad
    public float timeBetweenWaves = 5f;

    [SerializeField]
    private bool spawningWaves = false;
    private bool delayingAlready = false;

    private void Start()
    {

        waveManager = transform.parent.GetComponent<WaveManager>();

      
    }

    private void Update()
    {
        //Rapid waves basically says if the coroutine is running
        if(spawningWaves && !waveManager.rapidWaves && !delayingAlready)
        {
            StartCoroutine(createWaveWithDelay());
        }
    }

    IEnumerator createWaveWithDelay()
    {
        delayingAlready = true;
        yield return new WaitForSeconds(timeBetweenWaves);
        waveManager.startRoughWaves();
        delayingAlready = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!exitTrigger)
            {
                spawningWaves = true;
            }
            else
            {
                enterTriger.spawningWaves = false;
            }
        }
    }
}
