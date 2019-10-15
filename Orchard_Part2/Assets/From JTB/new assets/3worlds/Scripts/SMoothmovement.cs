using UnityEngine;
using System.Collections;

public class SMoothmovement : MonoBehaviour
{

    private Vector2 velocity;

    public float smoothTimeX;

    public GameObject player;

    void LateUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeX);

        transform.position = new Vector3(posX, posY, transform.position.z);
    }
}
