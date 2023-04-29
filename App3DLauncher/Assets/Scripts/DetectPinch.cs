using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPinch : MonoBehaviour
{
    SpawnLauncher spawnLauncher;
    OutlineController outlineController;


    LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        spawnLauncher = transform.parent.parent.GetComponent<SpawnLauncher>();
        outlineController = transform.GetChild(0).GetComponent<OutlineController>();

        lr = spawnLauncher.Launcher.AddComponent<LineRenderer>();
        lr.startWidth = 0.02f;
        lr.endWidth = 0.02f;
    }

    void PinchSelect(Vector3 handPos)
    {
        Vector3 headPos = spawnLauncher.centerCamera.transform.position;
        Vector3 direction = handPos - headPos;
        RaycastHit hit;
        Ray ray = new Ray(headPos, direction);
        //lr.SetPosition(0, handPos);
        //lr.SetPosition(1, headPos);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "App")
            {
                Debug.Log("Hit");
                spawnLauncher.SelectApp(transform.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnLauncher.AppSelected)
            return;

        bool isLeftIndexFingerPinching = spawnLauncher.leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        bool isRightIndexFingerPinching = spawnLauncher.rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        if (!isLeftIndexFingerPinching && isRightIndexFingerPinching)
        {
            Vector3 pos = spawnLauncher.rightHand.transform.position;

            foreach (var b in spawnLauncher.rightSkeleton.Bones)
            {
                if (b.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                {
                    pos = b.Transform.position;
                    break;
                }
            }
            PinchSelect(pos);
        }

        if (isLeftIndexFingerPinching && !isRightIndexFingerPinching)
        {
            Vector3 pos = spawnLauncher.leftHand.transform.position;

            foreach (var b in spawnLauncher.leftSkeleton.Bones)
            {
                if (b.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                {
                    pos = b.Transform.position;
                    break;
                }
            }
            PinchSelect(pos);
        }
    }
}
