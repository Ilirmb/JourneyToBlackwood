using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour
{

    public void Load(string s)
    {
        SceneManager.LoadScene(s);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}