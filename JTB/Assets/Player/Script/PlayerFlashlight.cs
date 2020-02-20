using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashlight : MonoBehaviour
{
    private Light flashlight;
    [Header("Light Intensity Settings")]
    public float minIntensity = 0;
    public float maxIntensity = 1; //min and max intensity for light (brightness)
    [Header("Rate of Change")]
    [Tooltip("per frame, how much intensity is changed")]
    public float changePerTick = 0.005f; //per frame, how much intensity is changed
    [Header("Delay Settings")]
    [Tooltip("How long the light stays at maximum intensity")]
    public float lightLifetime = 3;
    [Tooltip("When the player can use the flashlight ability again, A Cooldown Mechanic")]
    public float userCooldown = 2;

    [Header("Can the player turn the lights on")]
    public bool canUse = true;

    private void Start()
    {
        flashlight = GetComponent<Light>(); //access the actual light component to change intensity
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && canUse == true)
            StartCoroutine(ChangeLight());
    }

    private IEnumerator ChangeLight()
    {
        //while (true) //infinite loop to just repeat forever yea
        //{
        canUse = false;
            while (flashlight.intensity <= maxIntensity)
            {
                flashlight.intensity += changePerTick;
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(lightLifetime); // wait X amount of seconds

            while (flashlight.intensity > minIntensity)
            {
                flashlight.intensity -= changePerTick;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(userCooldown); //delay before the lights turn back on
        //}
        canUse = true;
    }
}

