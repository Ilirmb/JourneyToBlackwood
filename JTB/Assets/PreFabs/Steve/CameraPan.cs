using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CameraPan : MonoBehaviour
{
    public Transform Player;
    public CinemachineVirtualCamera Pan;
    public Text Speaker;
    public GameObject testObject;

    // Start is called before the first frame update
    void Start()
    {
        Pan = GameObject.Find("CM 2DCam").GetComponent<CinemachineVirtualCamera>() as CinemachineVirtualCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if (Speaker.text == "Logolio")
        {
            //Debug.Log("Shift Pan Target Here");
            Pan.Follow = testObject.transform;
        }
        else
        {
            Pan.Follow = Player;
        }
    }
}
