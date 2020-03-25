using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformOccupantChecker : MonoBehaviour
{
    public PlatformMovement Platform;
    // Start is called before the first frame update
    void Start()
    {
        if (Platform == null)
            Platform = this.transform.parent.parent.GetComponent<PlatformMovement>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Platform.playerIsOnPlatform = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Platform.playerIsOnPlatform = false;
    }
}
