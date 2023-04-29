using System.Collections.Generic;
using UnityEngine;


public class RotateLauncher : MonoBehaviour
{
    enum InteractionType { Pinch, Poke }
    OVRHand leftHand;
    OVRHand rightHand;

    [SerializeField]
    InteractionType interaction;

    GameObject launcher;

    Vector3 prevRightHandPos;
    Vector3 prevLeftHandPos;

    void Start()
    {
        launcher = GetComponent<SpawnLauncher>().Launcher;
        leftHand = GetComponent<SpawnLauncher>().leftHand;
        rightHand = GetComponent<SpawnLauncher>().rightHand;
    }

    void PinchRotate(Vector3 prevHandPos, Vector3 handPos)
    {
        var delta = handPos - prevHandPos;

        if (delta.sqrMagnitude > 10e-7f)
        {
            float rotationAngle = Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg;
            rotationAngle = Mathf.Clamp(rotationAngle, -90f, 90f) * 0.02f;
            launcher.transform.Rotate(0f, rotationAngle, 0f, Space.World);
        }
    }

    void Pinch()
    {
        bool isLeftIndexFingerPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        bool isRightIndexFingerPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        if (!isLeftIndexFingerPinching && isRightIndexFingerPinching)
        {
            PinchRotate(prevRightHandPos, rightHand.transform.position);
        }

        if (isRightIndexFingerPinching)
            prevRightHandPos = rightHand.transform.position;

        if (isLeftIndexFingerPinching && !isRightIndexFingerPinching)
        {
            PinchRotate(prevLeftHandPos, leftHand.transform.position);
        }

        if (isLeftIndexFingerPinching)
            prevLeftHandPos = leftHand.transform.position;
    }

    void Poke() { }

    void Update()
    {
        if (leftHand.IsTracked && rightHand.IsTracked)
        {
            switch (interaction)
            {
                case InteractionType.Pinch:
                    Pinch();
                    break;
                case InteractionType.Poke:
                    Poke();
                    break;
            }
        }
    }
}
