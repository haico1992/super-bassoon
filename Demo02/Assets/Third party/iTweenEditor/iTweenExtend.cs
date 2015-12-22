using System;
using UnityEngine;

public partial class iTween
{
    //HuyHQ
    public static void PutOnPathExtended(GameObject target, Vector3[] points, float percent)
    {
        target.transform.position = Interp(points, percent);
    }
	
}
