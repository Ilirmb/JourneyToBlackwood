 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StamLossTextManager : MonoBehaviour {

    public GameObject textPrefab;
    public GameObject currentObject;
    public Image StaminaBar;
    public float flashInterval = .1f;

    public void spawnText(string text, Color color)
    {
        currentObject = Instantiate(textPrefab, transform.position + new Vector3(Random.Range(-.5f, .5f), Random.Range(-3f, 3f)), Quaternion.identity, this.transform);
        currentObject.GetComponent<StamLossText>().init(this, text, color);
        StartCoroutine(flashRed());
    }
    public void spawnText(string text, Color color, Vector3 offset)
    {
        currentObject = Instantiate(textPrefab, transform.position + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.2f, .2f)) + offset, Quaternion.identity);
        currentObject.GetComponent<StamLossText>().init(this, text, color);
    }
    public void spawnText(string text, Color color, float time)
    {
        currentObject = Instantiate(textPrefab, transform.position + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.2f, .2f)), Quaternion.identity, this.transform);
        currentObject.GetComponent<StamLossText>().init(this, text, color);
        currentObject.GetComponent<StamLossText>().setKillTime(time);
        StartCoroutine(flashRed());
    }

    IEnumerator flashRed()
    {
        StaminaBar.color = Color.red;
        yield return new WaitForSeconds(flashInterval);
        StaminaBar.color = Color.white;
    }

    // Use this for initialization
    void Start () {
        //textPrefab = Resources.Load<GameObject>("Prefabs/Stamina Loss Text");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
