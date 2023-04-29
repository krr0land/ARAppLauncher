using System.Collections.Generic;
using System.Linq;
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
    
    // Poke rotation
    public OVRSkeleton rightHandSkeleton;
    private OVRBone indexFingerBone;
    private Vector3 lastPosition;
    private Vector3 startPosition;
	private bool isFingerOutside;
    public bool isRotating;

    void Start()
    {
        launcher = GetComponent<SpawnLauncher>().Launcher;
        leftHand = GetComponent<SpawnLauncher>().leftHand;
        rightHand = GetComponent<SpawnLauncher>().rightHand;

        rightHandSkeleton = GameObject.Find("OVRHandRight").GetComponent<OVRSkeleton>();
        OVRSkeleton.BoneId boneId = OVRSkeleton.BoneId.Hand_IndexTip;
        indexFingerBone = rightHandSkeleton.Bones.ToList().Where(b => b.Id == boneId).ToList().First();
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

    void Poke()
    {
        var distance = (indexFingerBone.Transform.position - transform.position).magnitude;
        Debug.Log("RotateLauncher distance: " + distance + " isFingerOutside: " + isFingerOutside + " isRotating: " + isRotating + " startPosition: " + startPosition + " lastPosition: " + lastPosition);
        if (distance > 0.55f)
        {
            if (isFingerOutside)
            {
				var fingerDistance = (indexFingerBone.Transform.position - startPosition).magnitude;

                if (fingerDistance > 0.04f)
                {
                    isRotating = true;
                }
            }
            else
            {
                isFingerOutside = true;
                startPosition = indexFingerBone.Transform.position;
            }
			if (isRotating) {
                var angle = Vector3.SignedAngle(lastPosition, indexFingerBone.Transform.position, Vector3.up);
                launcher.transform.Rotate(Vector3.up, angle);
			}
            lastPosition = indexFingerBone.Transform.position;
            isFingerOutside = true;
        }
        else
        {
            isFingerOutside = false;
            isRotating = false;
        }
    }

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
