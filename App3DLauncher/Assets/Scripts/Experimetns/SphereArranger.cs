using System.Collections.Generic;
using UnityEngine;

public class SphereArranger : MonoBehaviour
{
    List<GameObject> arrangedItems;
    bool aranged = false;

    public void Arange(List<GameObject> items, Vector3 appScale)
    {
        arrangedItems = items;

        int objectCount = arrangedItems.Count;
        float radius = transform.localScale.x / 2;

        Vector3 cameraPos = Camera.main.transform.position;

        for (int i = 0; i < objectCount; i++)
        {
            float phi = Mathf.Acos(1f - (2f * (i * 0.8f + objectCount * 0.1f) + 1f) / objectCount);
            float theta = Mathf.PI * (1f + Mathf.Sqrt(5f)) * (i + 1f);

            Vector3 newPos = new Vector3(Mathf.Cos(theta) * Mathf.Sin(phi), Mathf.Cos(phi), Mathf.Sin(theta) * Mathf.Sin(phi)) * radius;
            Quaternion rotation = Quaternion.LookRotation(newPos - transform.position);

            arrangedItems[i].transform.position = transform.position + newPos;
            arrangedItems[i].transform.rotation = rotation;
            arrangedItems[i].transform.localScale = appScale;
            arrangedItems[i].transform.LookAt(cameraPos);
        }

        aranged = true;
    }

    void Update()
    {
        if (!aranged)
            return;

        for (int i = 0; i < arrangedItems.Count; i++)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            arrangedItems[i].transform.LookAt(cameraPos);
        }
    }
}

