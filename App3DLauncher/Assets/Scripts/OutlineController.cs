using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("OnTriggerEnter: " + col.name);
        if (col.name.StartsWith("Hand_Index3")) // possible values are in the OVRSkeleton.BoneId enum
        {
            GetComponent<Renderer>().enabled = true;
        }
    }
    
    void OnTriggerExit(Collider col)
    {
        if (col.name.StartsWith("Hand_Index3")) // possible values are in the OVRSkeleton.BoneId enum
        {
            CloseOutline();
        }
    }
    
    public void CloseOutline()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
