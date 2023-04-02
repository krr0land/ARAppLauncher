using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tamasssss
{
    public class AppLauncherInstance : MonoBehaviour
    {
        public GameObject prefab;
        public Vector3 position1;
        public Vector3 position2;
        public Quaternion rotation;
        public List<GameObject> apps;
        void Start()
        {
            var center = (position1 + position2) / 2;
            var dist = Vector3.Distance(position1, position2);
            var scale = new Vector3(
                dist,
                dist,
                dist
            );
            GameObject obj = Instantiate(prefab, center, rotation, transform);
            obj.transform.localScale = scale;
            obj.GetComponent<SphereArranger>().arrangedItems = apps;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

