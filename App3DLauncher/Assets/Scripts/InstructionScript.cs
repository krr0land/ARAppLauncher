using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstructionScript : MonoBehaviour
{
    [SerializeField]
    GameObject launcher;

    [SerializeField]
    int task;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshPro>().text = "Task not selected";
    }

    // Update is called once per frame
    void Update()
    {
        switch (task)
        {
            case 0:
                Case0();
                break;
            case -1:
                GetComponent<TextMeshPro>().text = "Nicely done!";
                break;
            default:
                GetComponent<TextMeshPro>().text = "Task does not exist or is not selected";
                break;
        }
    }

    void Case0()
    {
        if (launcher.activeSelf)
        {
            GetComponent<TextMeshPro>().text = "Select an app";
            if (launcher.GetComponent<SpawnLauncher>().AppSelected)
            {
                task = -1;
            }
        }
        else
            GetComponent<TextMeshPro>().text = "Open the launcher by pinching on both hands";
    }
}
