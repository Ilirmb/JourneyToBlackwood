using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveOnE : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.E))
        {
            GameManager.instance.SaveProgress();
        }

	}
}
