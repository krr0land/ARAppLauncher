using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RotateLauncher : MonoBehaviour
{
    public enum InteractionType { Pinch, Poke }
    OVRHand leftHand;
    OVRHand rightHand;

    [SerializeField]
    InteractionType interaction;

    GameObject Launcher { get { return GetComponent<SpawnLauncher>().Launcher; } }

    Vector3 prevRightHandPos;
    Vector3 prevLeftHandPos;
    
    // Poke rotation
    public OVRSkeleton rightHandSkeleton;
    private OVRBone indexFingerBone;
    private Vector3 lastPosition;
    private Vector3 startPosition;
	public bool isFingerOutside;
    public bool isRotating;

    void Start()
    {
        leftHand = GetComponent<SpawnLauncher>().leftHand;
        rightHand = GetComponent<SpawnLauncher>().rightHand;

        rightHandSkeleton = GameObject.Find("OVRHandRight").GetComponent<OVRSkeleton>();
        OVRSkeleton.BoneId boneId = OVRSkeleton.BoneId.Hand_IndexTip;
        indexFingerBone = rightHandSkeleton.Bones.ToList().Where(b => b.Id == boneId).ToList().First();
    }

    public void ChangeInteraction(InteractionType interactionType)
    {
        interaction = interactionType;
        
    }
    void Rotate(Vector3 prevHandPos, Vector3 handPos)
    {
        var delta = handPos - prevHandPos;

        if (delta.sqrMagnitude > 10e-7f)
        {
            float rotationAngle = Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg;
            rotationAngle = Mathf.Clamp(rotationAngle, -90f, 90f) * 0.02f;
            Launcher.transform.Rotate(0f, rotationAngle, 0f, Space.World);
        }
    }

    void Pinch()
    {
        bool isLeftIndexFingerPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        bool isRightIndexFingerPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        if (!isLeftIndexFingerPinching && isRightIndexFingerPinching)
        {
            Rotate(prevRightHandPos, rightHand.transform.position);
        }

        if (isRightIndexFingerPinching)
            prevRightHandPos = rightHand.transform.position;

        if (isLeftIndexFingerPinching && !isRightIndexFingerPinching)
        {
            Rotate(prevLeftHandPos, leftHand.transform.position);
        }

        if (isLeftIndexFingerPinching)
            prevLeftHandPos = leftHand.transform.position;
    }

    void Poke()
    {
        var distance = (indexFingerBone.Transform.position - transform.position).magnitude;
        Debug.Log("RotateLauncher distance: " + distance + " isFingerOutside: " + isFingerOutside + " isRotating: " + isRotating + " startPosition: " + startPosition + " lastPosition: " + lastPosition);
        if (IsThresholdReached())
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
                var position = Launcher.transform.position;
                var angle = Vector3.SignedAngle(lastPosition-position, indexFingerBone.Transform.position-position, Vector3.up);
                Launcher.transform.Rotate(Vector3.up, angle);
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
    
    private bool IsThresholdReached()
    {
        var distance = (indexFingerBone.Transform.position - Launcher.transform.position).magnitude;
        var state = GetComponent<SpawnLauncher>().state;
        if (state == LauncherState.Central)
        {
            return distance > 0.449f;
        }
        return distance < 0.2f;
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
