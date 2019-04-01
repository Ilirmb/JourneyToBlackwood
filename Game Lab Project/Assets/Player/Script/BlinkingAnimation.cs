/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class BlinkingAnimation : MonoBehaviour
{

    public SpriteMesh[] Heads;
    public float timeBetweenBlinksMax;
    public float timeBetweenBlinksMin;
    public float blinkSpeed;
    private SpriteMeshInstance smi;

    void Start()
    {
        smi = GetComponent<SpriteMeshInstance>();
        StartCoroutine(blinking());
    }

    private IEnumerator blinking()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(timeBetweenBlinksMin, timeBetweenBlinksMax));
            for (int i = 1; i < Heads.Length; i++)
            {
                smi.spriteMesh = Heads[i];
                yield return new WaitForSecondsRealtime(blinkSpeed);
            }
            for (int i = Heads.Length - 1; i > -1; i--)
            {
                smi.spriteMesh = Heads[i];
            }
        }
    }
}
*/