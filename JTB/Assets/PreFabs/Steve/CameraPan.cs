using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraPan : MonoBehaviour
{
    public GameObject Pan;
    public Text Speaker;

    // Start is called before the first frame update
    void Start()
    {
        Pan = GameObject.Find("CM 2DCam");
        Speaker = GameObject.Find("Name").GetComponent<Text>() as Text;

        if (Pan)
        {
            Debug.Log("Camera Found!");
        }

        if (Speaker)
        {
            Debug.Log("Name Found!");
        }
        else
        {
            Debug.Log("Name not Found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Speaker.text == "Logolio")
        {
            Debug.Log("Shift Pan Target Here");
        }
    }
}
