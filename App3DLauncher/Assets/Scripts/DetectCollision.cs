using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    SpawnLauncher spawnLauncher;
    RotateLauncher rotateLauncher;

    private bool isColliding = false;
    private Vector3 enterPos;

    void Start()
    {
        spawnLauncher = transform.parent.parent.GetComponent<SpawnLauncher>();
        rotateLauncher = transform.parent.parent.GetComponent<RotateLauncher>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (!enabled) return;
        if (spawnLauncher.AppSelected)
            return;
        if (rotateLauncher.isRotating)
            return;

        if (col.name.StartsWith("Hand_Index3")) // possible values are in the OVRSkeleton.BoneId enum
        {
            enterPos = col.ClosestPoint(transform.position);
            isColliding = true;
            transform.GetChild(0).GetComponent<OutlineController>().OpenOutline();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (!enabled) return;
        if (!isColliding)
            return;
        if (spawnLauncher.AppSelected)
            return;

        if (col.name.StartsWith("Hand_Index3")) // possible values are in the OVRSkeleton.BoneId enum
        {
            Vector3 exitPos = col.ClosestPoint(transform.position);
            if (!rotateLauncher.isRotating && Vector3.Distance(enterPos, exitPos) < 0.02f)
            {
                spawnLauncher.SelectApp(transform.gameObject);
            }
            else
            {
                isColliding = false;
            }
            transform.GetChild(0).GetComponent<OutlineController>().CloseOutline();
        }
    }
}
