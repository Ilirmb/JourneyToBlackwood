using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleManager : MonoBehaviour
{
    DistanceJoint2D distJoint;
    Vector3 aimTarget;
    public float maxDist = 10f;
    public LayerMask hittableLayer;

    // Start is called before the first frame update
    void Start()
    {
        distJoint = this.GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            aimTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var hit = Physics2D.Raycast(this.transform.position, aimTarget - this.transform.position, maxDist, hittableLayer);
            if (hit.collider != null)
            {

            }
        }
    }
}
