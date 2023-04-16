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

        private GameObject launcher = null;

        public void Delete()
        {
            if (launcher != null)
            {
                Destroy(launcher);
                launcher = null;
            }
        }

        public void CreateOrMove()
        {
            var center = (position1 + position2) / 2;
            var dist = Vector3.Distance(position1, position2);
            var scale = new Vector3(
                dist,
                dist,
                dist
            );
            Debug.Log("Creating Launcher");
            launcher = Instantiate(prefab, center, rotation, transform);
            launcher.transform.localScale = scale;
            launcher.GetComponent<SphereArranger>().arrangedItems = apps;
            /* if (GameObject.Find("Launcher"))
            {
                Debug.Log("Launcher already exists");
                launcher.transform.position = center;
                launcher.transform.localScale = scale;
            }
            else
            {
                Debug.Log("Creating Launcher");
                launcher = Instantiate(prefab, center, rotation, transform);
                launcher.transform.localScale = scale;
                launcher.GetComponent<SphereArranger>().arrangedItems = apps;
            } */
        }
    }
}



