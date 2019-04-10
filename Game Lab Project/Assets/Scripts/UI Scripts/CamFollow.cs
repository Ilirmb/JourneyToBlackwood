using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {

    Transform player;
    [SerializeField] float offsetY;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	void Update ()
    {
        Vector3 newPos = new Vector3(player.position.x, player.position.y + offsetY, -10f);
        gameObject.transform.position = newPos;
	}
}
