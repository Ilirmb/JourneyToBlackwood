using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotationLimiter : MonoBehaviour
{

    public float limitDegrees = 80;
    
    // Update is called once per frame
    void Update()
    {
        //The display of z in the inspector goes from 0 to 180, then switches to -180 then back to zero instead of going from 0 to 360 as rotation.eulerAngles returns
        //This is just a little bit of math to get it to line back up with what is expected
        float z = transform.rotation.eulerAngles.z;
        if (z > 180) z = -(z - 2*(z % 180));
        if (z > limitDegrees)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, limitDegrees);
        } else if(z <= -limitDegrees)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -limitDegrees);
        }
    }

}
