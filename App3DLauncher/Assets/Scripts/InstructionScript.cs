using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstructionScript : MonoBehaviour
{
    [SerializeField]
    GameObject launcher;
    int task = 0;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshPro>().text = "Task not selected";
    }

    // Update is called once per frame
    void Update()
    {
        if (task == 1)
        {
            if (launcher.activeSelf)
            {
                GetComponent<TextMeshPro>().text = "Select an app";
                if (launcher.GetComponent<SpawnLauncher>().AppSelected)
                {
                    task = 0;
                    GetComponent<TextMeshPro>().text = "Nicely done!";
                }
            }
            else
                GetComponent<TextMeshPro>().text = "Open the launcher by pinching on both hands";
        }
    }
}
