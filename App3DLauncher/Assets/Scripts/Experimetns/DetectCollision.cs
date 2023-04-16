using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    Renderer matRenderer;
    int counter;
    Color color;
    SpawnLauncher spawnLauncher;

    void Start()
    {
        matRenderer = GetComponent<Renderer>();
        spawnLauncher = transform.parent.parent.GetComponent<SpawnLauncher>();
        color = matRenderer.material.color;
        counter = -1;
    }

    private void FixedUpdate()
    {
        if (counter > 0)
            --counter;
    }

    void Update()
    {
        if(counter == 0)
        {
            spawnLauncher.Launcher.SetActive(false);
            spawnLauncher.appSelected = false;
            matRenderer.material.color = color;
            counter = -1;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (spawnLauncher.appSelected)
            return;

        string name = col.name.Substring(0, 4);
        if (name == "Hand" || col.name == "OVRHandRight" || col.name == "OVRHandLeft")
        {
            matRenderer.material.color = Color.red;
            counter = 50;
            spawnLauncher.appSelected = true;
        }
    }
}
