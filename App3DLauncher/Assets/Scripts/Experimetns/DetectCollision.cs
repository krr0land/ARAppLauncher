using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    SpawnLauncher spawnLauncher;

    void Start()
    {
        spawnLauncher = transform.parent.parent.GetComponent<SpawnLauncher>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (spawnLauncher.AppSelected)
            return;

        string name = col.name.Substring(0, 5);
        if (name == "Hand_") // possible values are in the OVRSkeleton.BoneId enum
        {
            spawnLauncher.SelectApp(transform.gameObject);
            transform.GetChild(0).GetComponent<OutlineController>().CloseOutline();
        }
    }
}
