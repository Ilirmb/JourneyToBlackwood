using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleManager : MonoBehaviour
{
    DistanceJoint2D distJoint;
    Vector3 aimTarget;
    public float swingSpeed = 1f;
    public float maxHitDist = 10f;
    public float jumpOffForce = 500f;
    public LayerMask hittableLayer;

    public LineRenderer GrappleLineRender;
    
    [SerializeField]
    private bool isAttached;
    private CustomPlatformer2DUserControl cc;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        distJoint = this.GetComponent<DistanceJoint2D>();
        distJoint.enabled = false;
        cc = this.GetComponent<CustomPlatformer2DUserControl>();
        rb = this.GetComponent<Rigidbody2D>();
        if(GrappleLineRender != null)
        {
            GrappleLineRender.positionCount = 2;
        }
        GrappleLineRender.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //This code must go before the following block to prevent the attachment from deactivating it the same frame it's activated
        if (isAttached && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space)))
        {
            deattach();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector2(0f, jumpOffForce), ForceMode2D.Force);
            }
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            aimTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, aimTarget - this.transform.position, maxHitDist, hittableLayer);

            if (hit.collider != null)

            if (hit.collider != null && hit.collider.gameObject.CompareTag("GrappleTarget")) //Because of tag object is guarenteed to be of type grappletarget
            {
                attach(hit.collider.GetComponent<Rigidbody2D>());
            }
        }
        //dictates the movement while grappled, and other things to happen while grappled
        if (isAttached)
        {
            float f = Input.GetAxis("Horizontal");
            rb.velocity += new Vector2(f*swingSpeed, 0);
            GrappleLineRender.SetPosition(1, rb.transform.position + Vector3.forward*10); //Add forwards to put it behind the player and the target
        }
    }


    void attach(Rigidbody2D attachmentPoint)
    {
        Debug.Log("Entered attach");
        distJoint.enabled = true;
        isAttached = true;
        distJoint.connectedBody = attachmentPoint;
        distJoint.distance = attachmentPoint.GetComponent<GrappleTarget>().maxGrappleDistance; //must use reference as target may change every attach() call
        cc.enabled = false;

        GrappleLineRender.enabled = true;
        GrappleLineRender.SetPosition(0, attachmentPoint.transform.position + Vector3.forward); //add forwards to put behind according to camera
    }
    void deattach()
    {
        Debug.Log("Entered deattach");
        isAttached = false;
        distJoint.connectedBody = null;
        cc.enabled = true;
        distJoint.enabled = false;
        GrappleLineRender.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(this.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}

