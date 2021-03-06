﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableButtonChildren : MonoBehaviour {

    int numChildren;
    private GameObject currentChild;
    private bool childrenDisabled = false;


    /// <summary>
    /// Toggles whether or not the children of this game object should be active or not
    /// </summary>
    public void toggleChildren()
    {
        if (!childrenDisabled)
        {
            disableChildren();
        }
        else
        {
            enableChidlren();
        }
    }


    /// <summary>
    /// Disables all children of this game object
    /// </summary>
    public void disableChildren()
    {
        for(int i = 0; i < numChildren; ++i)
        {
            currentChild = gameObject.transform.GetChild(i).gameObject;
            currentChild.GetComponentInChildren<Text>().enabled = false;
            currentChild.GetComponent<Button>().enabled = false;
            currentChild.GetComponent<Image>().enabled = false;
            childrenDisabled = true;
        }
    }


    /// <summary>
    /// Enables all children of this game object
    /// </summary>
    public void enableChidlren()
    {
        for (int i = 0; i < numChildren; ++i)
        {
            currentChild = gameObject.transform.GetChild(i).gameObject;
            currentChild.GetComponentInChildren<Text>().enabled = true;
            currentChild.GetComponent<Button>().enabled = true;
            currentChild.GetComponent<Image>().enabled = true;
            childrenDisabled = false;
        }
    }


	// Use this for initialization
	void Start () {
		numChildren = gameObject.transform.childCount;
    }
}
