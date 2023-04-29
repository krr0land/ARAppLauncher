using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public void OpenOutline()
    {
        GetComponent<Renderer>().enabled = true;
    }
    public void CloseOutline()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
