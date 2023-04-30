using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPinch : MonoBehaviour
{
    SpawnLauncher spawnLauncher;
    OutlineController outlineController;

    int timer;

    bool previouslyPinched;
    Vector3 pinchPosition;

    // Start is called before the first frame update
    void Start()
    {
        spawnLauncher = transform.parent.parent.GetComponent<SpawnLauncher>();
        outlineController = transform.GetChild(0).GetComponent<OutlineController>();
        previouslyPinched = false;
        timer = 0;
    }

    void PinchSelect(Vector3 handPos)
    {
        if (timer > 20)
        {
            return;
        }
        Vector3 headPos = spawnLauncher.centerCamera.transform.position;
        Vector3 direction = handPos - headPos;
        Ray ray = new Ray(headPos, direction);
        foreach (var hit in Physics.RaycastAll(ray))
        {
            if (hit.collider.gameObject.tag == "App")
            {
                spawnLauncher.SelectApp(hit.collider.gameObject);
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        timer++;
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
            pinchPosition = pos;
            previouslyPinched = true;
        }
        else if (isLeftIndexFingerPinching && !isRightIndexFingerPinching)
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
            pinchPosition = pos;
            previouslyPinched = true;
        }
        else
        {
            if (previouslyPinched)
            {
                PinchSelect(pinchPosition);
                previouslyPinched = false;
            }
            timer = 0;
        }
    }
}
