using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalFlashlight : MonoBehaviour
{
    private Light flashlight;
    [Header("Light Intensity Settings")]
    public float minIntensity = 0;
    public float maxIntensity = 5; //min and max intensity for light (brightness)
    [Header("Rate of Change")]
    [Tooltip("per frame, how much intensity is changed")]
    public float changePerTick = 0.1f; //per frame, how much intensity is changed
    [Header("Delay Settings")]
    [Tooltip("How long the light stays at maximum intensity")]
    public float lightLifetime = 1;
    [Tooltip("Cooldown on blinking")]
    public float lightCooldown = 2;

    private void Start()
    {
        flashlight = GetComponent<Light>(); //access the actual light component to change intensity
        StartCoroutine(ChangeLight());
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Flashlight" && canUse == true)
            StartCoroutine(ChangeLight());
    }*/

    /*public void activateCrystal()
    {
        if (canUse == true)
            StartCoroutine(ChangeLight());
    }*/

    private IEnumerator ChangeLight()
    {
        while (true)
        {
            //This check now needs to be made in case the crystallight is outside of the active zone
            if (flashlight.enabled == true)
            {
                while (flashlight.intensity <= maxIntensity)
                {
                    flashlight.intensity += changePerTick;
                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForSeconds(lightLifetime); //wait X amount of seconds

                while (flashlight.intensity > minIntensity)
                {
                    flashlight.intensity -= changePerTick;
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(lightCooldown); //wait till next blink
            }
            else
            {
                //Because this is an enumerator, we can use a fancy little lambda expression to watch for a particular boolean expression to return true
                yield return new WaitUntil(() => flashlight.enabled == true);
            }
        }
    }
}

