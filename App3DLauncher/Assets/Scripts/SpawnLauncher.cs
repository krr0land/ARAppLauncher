using UnityEngine;

namespace ARLauncher.Interaction
{
    public class SpawnLauncher : MonoBehaviour
    {
        public Camera centerCamera;
        public OVRHand leftHand;
        public OVRHand rightHand;
        public OVRSkeleton leftSkeleton;
        public OVRSkeleton rightSkeleton;

        public GameObject leftController; // ?
        public GameObject rightController;

        void Start() { }


        void Update()
        {
            if (HandTrackSpawning())
                return;
            else 
                ControllerSpawning();
        }

        public bool HandTrackSpawning()
        {
            if (leftHand.IsTracked && rightHand.IsTracked)
            {
                bool isLeftIndexFingerPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
                bool isRightIndexFingerPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

                if (isLeftIndexFingerPinching && isRightIndexFingerPinching)
                {
                    Vector3 left = leftHand.transform.position;
                    Vector3 right = rightHand.transform.position;

                    foreach (var b in leftSkeleton.Bones)
                    {
                        if (b.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                        {
                            left = b.Transform.position;
                            break;
                        }
                    }
                    foreach (var b in rightSkeleton.Bones)
                    {
                        if (b.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                        {
                            right = b.Transform.position;
                            break;
                        }
                    }

                    Vector3 head = centerCamera.transform.position; // This should work

                    Spawn(left, right, head);
                    return true;
                }
            }

            return false;
        }

        public bool ControllerSpawning()
        {
            // TODO: Check if this actually works
            // Opening: Press X and get the controller position

            if (OVRInput.GetDown(OVRInput.RawButton.X))
            {
                Vector3 left = leftController.transform.position;
                Vector3 right = rightController.transform.position;
                Vector3 head = centerCamera.transform.position;

                Spawn(left, right, head);
                return true;
            }

            return false;
        }

        // Left and right hand, head coords for central view
        // head might not be necessary, because it might know by itself
        public void Spawn(Vector3 left, Vector3 right, Vector3 head) { /*Dummy function*/ }

    }
}


