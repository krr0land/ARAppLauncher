using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    SpawnLauncher spawnLauncher;
    
    private bool isColliding = false;
    private Vector3 enterPos;

    void Start()
    {
        spawnLauncher = transform.parent.parent.GetComponent<SpawnLauncher>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (spawnLauncher.AppSelected)
            return;

        if (col.name.StartsWith("Hand_Index3")) // possible values are in the OVRSkeleton.BoneId enum
        {
            enterPos = col.ClosestPoint(transform.position);
            isColliding = true;
        }
    }
    
    void OnTriggerExit(Collider col) 
    {
        if (!isColliding)
            return;
        if (spawnLauncher.AppSelected)
            return;

        if (col.name.StartsWith("Hand_Index3")) // possible values are in the OVRSkeleton.BoneId enum
        {
            Vector3 exitPos = col.ClosestPoint(transform.position);
            if (Vector3.Distance(enterPos, exitPos) < 0.02f)
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
