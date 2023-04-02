using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tamasssss
{
    public class SphereArranger : MonoBehaviour
    {
        public List<GameObject> arrangedItems;

        private List<GameObject> arrangedItemsCopy;


        // Start is called before the first frame update
        void Start()
        {
            int objectCount = arrangedItems.Count;
            float radius = transform.localScale.x / 2;
            arrangedItemsCopy = new List<GameObject>();

            for (int i = 0; i < objectCount; i++)
            {
                float phi = Mathf.Acos(1f - (2f * i + 1f) / objectCount);
                float theta = Mathf.PI * (1f + Mathf.Sqrt(5f)) * (i + 1f);

                Vector3 newPos = new Vector3(Mathf.Cos(theta) * Mathf.Sin(phi), Mathf.Sin(theta) * Mathf.Sin(phi), Mathf.Cos(phi)) * radius;
                Quaternion rotation = Quaternion.LookRotation(newPos - transform.position);
                GameObject newObj = Instantiate(arrangedItems[i], transform.position + newPos, rotation, transform);

                newObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                Vector3 cameraPos = Camera.main.transform.position;
                newObj.transform.LookAt(cameraPos);
                arrangedItemsCopy.Add(newObj);
            }
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < arrangedItemsCopy.Count; i++)
            {
                Vector3 cameraPos = Camera.main.transform.position;
                arrangedItemsCopy[i].transform.LookAt(cameraPos);
            }
        }
    }
}

