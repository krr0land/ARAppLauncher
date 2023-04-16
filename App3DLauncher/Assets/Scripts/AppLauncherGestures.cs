using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tamasssss
{
    public class AppLauncherGestures : MonoBehaviour
    {
        public Camera sceneCamera;
        public OVRHand leftHand;
        public OVRHand rightHand;
        public OVRSkeleton leftSkeleton;
        public OVRSkeleton rightSkeleton;
        private bool isLeftIndexFingerPinching;
        private bool isRightIndexFingerPinching;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (leftHand.IsTracked && rightHand.IsTracked)
            {
                isLeftIndexFingerPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
                isRightIndexFingerPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

                if (isLeftIndexFingerPinching && isRightIndexFingerPinching)
                {
                    GetComponent<AppLauncherInstance>().Delete();
                    Vector3 left = leftHand.transform.position;
                    Vector3 right = rightHand.transform.position;


                    //not sure if we need this or not
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

                    GetComponent<AppLauncherInstance>().position1 = left;
                    GetComponent<AppLauncherInstance>().position2 = right;
                    GetComponent<AppLauncherInstance>().rotation = Quaternion.LookRotation(right - left);
                    GetComponent<AppLauncherInstance>().CreateOrMove();

                }
            }
        }
    }
}
