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

    GameObject launcher;

    Vector3 prevRightHandPos;
    Vector3 prevLeftHandPos;
    int pinchTimer;

    // Poke rotation
    OVRSkeleton rightHandSkeleton;
    OVRBone rightIndexFingerBone;
    OVRSkeleton leftHandSkeleton;
    OVRBone leftIndexFingerBone;
    Vector3 lastPosition;
    Vector3 startPosition;
    [HideInInspector]
    public bool isFingerOutside;
    [HideInInspector]
    public bool isRotating;

    void Start()
    {
        launcher = GetComponent<SpawnLauncher>().Launcher;
        leftHand = GetComponent<SpawnLauncher>().leftHand;
        rightHand = GetComponent<SpawnLauncher>().rightHand;

        OVRSkeleton.BoneId boneId = OVRSkeleton.BoneId.Hand_IndexTip;

        rightHandSkeleton = GameObject.Find("OVRHandRight").GetComponent<OVRSkeleton>();
        rightIndexFingerBone = rightHandSkeleton.Bones.ToList().Where(b => b.Id == boneId).ToList().First();

        leftHandSkeleton = GameObject.Find("OVRHandLeft").GetComponent<OVRSkeleton>();
        leftIndexFingerBone = leftHandSkeleton.Bones.ToList().Where(b => b.Id == boneId).ToList().First();

        pinchTimer = 0;
    }

    public void ChangeInteraction(InteractionType interactionType)
    {
        interaction = interactionType;

    }
    void Rotate(Vector3 prevHandPos, Vector3 handPos)
    {
        var position = launcher.transform.position;
        var angle = Vector3.SignedAngle(prevHandPos - position, handPos - position, Vector3.up);
        launcher.transform.Rotate(Vector3.up, angle);
    }

    void Pinch()
    {
        bool isLeftIndexFingerPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        bool isRightIndexFingerPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        if (!isLeftIndexFingerPinching && isRightIndexFingerPinching)
        {
            if (pinchTimer < 20) { prevRightHandPos = rightIndexFingerBone.Transform.position; return; }
            if ((prevRightHandPos - rightIndexFingerBone.Transform.position).sqrMagnitude > 10e-7f)
                Rotate(prevRightHandPos, rightIndexFingerBone.Transform.position);
            prevRightHandPos = rightIndexFingerBone.Transform.position;
        }
        else if (isLeftIndexFingerPinching && !isRightIndexFingerPinching)
        {
            if (pinchTimer < 20) { prevRightHandPos = rightIndexFingerBone.Transform.position; return; }
            if ((prevLeftHandPos - leftIndexFingerBone.Transform.position).sqrMagnitude > 10e-7f)
                Rotate(prevLeftHandPos, leftIndexFingerBone.Transform.position);
            prevLeftHandPos = leftIndexFingerBone.Transform.position;
        }
        else
        {
            pinchTimer = 0;
            prevRightHandPos = rightIndexFingerBone.Transform.position;
        }
    }

    void PokeActive(Vector3 indexFingerPos)
    {
        if (isFingerOutside)
        {
            var fingerDistance = (indexFingerPos - startPosition).magnitude;

            if (fingerDistance > 0.04f)
            {
                isRotating = true;
            }
        }
        else
        {
            isFingerOutside = true;
            startPosition = indexFingerPos;
        }
        if (isRotating)
            Rotate(lastPosition, indexFingerPos);
        lastPosition = indexFingerPos;
        isFingerOutside = true;
    }

    void Poke()
    {
        if (IsThresholdReached(rightIndexFingerBone.Transform.position))
            PokeActive(rightIndexFingerBone.Transform.position);
        else if (IsThresholdReached(leftIndexFingerBone.Transform.position))
            PokeActive(leftIndexFingerBone.Transform.position);
        else
        {
            isFingerOutside = false;
            isRotating = false;
        }
    }

    private bool IsThresholdReached(Vector3 indexFingerPos)
    {
        var distance = (indexFingerPos - launcher.transform.position).magnitude;
        var state = GetComponent<SpawnLauncher>().state;
        if (state == LauncherState.Central)
        {
            return distance > 0.449f;
        }
        return distance < 0.2f;
    }

    private void FixedUpdate()
    {
        pinchTimer++;
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
