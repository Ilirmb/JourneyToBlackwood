using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HideText : MonoBehaviour
{
    private GameObject textBoxes;


    // Start is called before the first frame update
    void Start()
    {
        textBoxes = GameObject.FindWithTag("Dialogue");
    }

    // Update is called once per frame
    void Update()
    {
        if (textBoxes)
        {
            textBoxes.SetActive(false);
        }
    }
}
