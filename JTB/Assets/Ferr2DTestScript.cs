using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ferr2DTestScript : MonoBehaviour
{
    Ferr2DT_TerrainMaterial terMat;
    Ferr2DT_SegmentDescription muddySegment;
    Ferr2DPath path;
    // Start is called before the first frame update
    void Start()
    {
        muddySegment = terMat.GetDescriptor((Ferr2DT_TerrainDirection)4); //4 is the index of extra descriptor added to the terrain which describes the muddy slopes.
    }

    void checkGround(Ferr2DT_SegmentDescription s)
    {
        if(s == muddySegment)
        {

        }
    }
}
