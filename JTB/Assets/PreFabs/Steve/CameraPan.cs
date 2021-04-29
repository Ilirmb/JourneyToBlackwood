using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CameraPan : MonoBehaviour
{
    GameObject Player;
    CinemachineVirtualCamera Pan;
    Text Speaker;
    bool cutscene = false;

    // Start is called before the first frame update
    void Start()
    {
        Speaker = GameObject.FindGameObjectWithTag("SpeakerName").GetComponent<Text>() as Text;
        Player = GameObject.FindGameObjectWithTag("Player");
        Pan = GameObject.Find("CM 2DCam").GetComponent<CinemachineVirtualCamera>() as CinemachineVirtualCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if (Speaker?.text == "Logolio")
        {
            if (cutscene == true)
            {
                Pan.Follow = transform.GetChild(0).gameObject.transform;
            }
            else
            {
                Pan.Follow = Player.transform;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cutscene = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cutscene = false;
            Pan.Follow = Player.transform;
            gameObject.active = false;
        }
    }
}
