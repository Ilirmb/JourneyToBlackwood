using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    GameObject waterObject;
    // Start is called before the first frame update
    void Start()
    {
        waterObject = GameObject.FindWithTag("Water");


    }
    private void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "RiverLog")
        {
            //transform.parent.GetComponent<CustomPlatformerCharacter2D>().m_Anim.SetBool("Grounded", true);
            transform.parent.GetComponent<CustomPlatformerCharacter2D>().m_Grounded = true;
           
        }

        else if (other.gameObject.tag == "RoughWaters")
        {
            Debug.Log("startRoughWaters");
            waterObject.GetComponent<WaveManager>().rapidWaves = true;
            waterObject.GetComponent<WaveManager>().StartCoroutine("RoughWaters");
        }
        else if (other.gameObject.tag == "Log")
        {
          //  transform.parent.GetComponent<CustomPlatformerCharacter2D>().m_Anim.SetBool("Grounded", true);
            transform.parent.GetComponent<CustomPlatformerCharacter2D>().m_Grounded = true;

        }
        else if (other.gameObject.tag == "Ground")
        {
          //  transform.parent.GetComponent<CustomPlatformerCharacter2D>().m_Anim.SetBool("Grounded", true);
           // transform.parent.GetComponent<CustomPlatformerCharacter2D>().m_Grounded = true;
        }

    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "RiverLog")
        {
            // transform.parent.GetComponent<CustomPlatformerCharacter2D>().m_Anim.SetBool("Grounded", false);

            transform.parent.GetComponent<CustomPlatformerCharacter2D>().m_Grounded = false;

        }
        else if (other.gameObject.tag == "Log")
        {
        //    transform.parent.GetComponent<CustomPlatformerCharacter2D>().m_Anim.SetBool("Grounded", false);
            
            transform.parent.GetComponent<CustomPlatformerCharacter2D>().m_Grounded = false;
          
        }
        else if(other.gameObject.tag == "Ground")
        {
          //  transform.parent.GetComponent<CustomPlatformerCharacter2D>().m_Anim.SetBool("Grounded", false);

            //transform.parent.GetComponent<CustomPlatformerCharacter2D>().m_Grounded = false;
        }
    }
    // Update is called once per fram

}
