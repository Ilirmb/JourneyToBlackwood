using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StamLossText : MonoBehaviour {

    private float timer = 0;
    private float killTime = 2f;
    public StamLossTextManager destroyer;

    public void init(StamLossTextManager creator, string Text, Color color)
    {
        GetComponentInChildren<Text>().color = color;
        GetComponentInChildren<Text>().text = Text;
        destroyer = creator;
    }
    public void setKillTime(float time)
    {
        killTime = time;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(Time.frameCount % 10 == 0)
        {
            transform.position += new Vector3(0, .1f);
        }
        if(timer >= killTime)
        {
            Destroy(gameObject);
        }
	}
}
