using System.Collections.Generic;
using UnityEngine;

public enum LauncherState { Frontal, Central }

public class SpawnLauncher : MonoBehaviour
{
    public Camera centerCamera;
    Vector3 prevCamPos;

    public OVRHand leftHand;
    public OVRHand rightHand;

    public GameObject launcherPrefab;
    public GameObject appPrefab;

    GameObject launcher;
    List<GameObject> apps;
    LauncherState state = LauncherState.Frontal;

    readonly Vector3 appScale = new Vector3(0.1f, 0.1f, 0.1f);

    readonly float frontalDistance = 0.4f; // distance from the camera on X and Z axis
    readonly float frontalHeight = -0.4f; // height from the camera on Y axis
    readonly Vector3 frontalScale = new Vector3(0.4f, 0.4f, 0.4f);
    Vector3 frontalPos;

    readonly Vector3 centralPos = new Vector3(0f, 0f, 0f);
    readonly Vector3 centralScale = new Vector3(1.2f, 1.2f, 1.2f);

    readonly Color emissionColor = new Color(0.5f, 0.5f, 0.5f);

    bool moving = true;
    int moveCounter = 0;
    int spawnCounter = 0;

    int selectCounter = 0;
    GameObject selectedObject = null;

    public GameObject Launcher { get { return launcher; } }
    public bool AppSelected { get { return selectedObject != null; } }

    Vector3 prevHandPos;

    void Start()
    {
        prevCamPos = centerCamera.transform.position;

        launcher = Instantiate(launcherPrefab, Vector3.zero, Quaternion.identity, transform);
        Toggle();
        SetState(LauncherState.Frontal);

        apps = new List<GameObject>();
        foreach (AppInfo info in AppCatalog.catalog)
        {
            GameObject app = Instantiate(appPrefab, transform.position, Quaternion.identity, launcher.transform);
            app.name = info.name;
            Renderer renderer = app.GetComponent<Renderer>();

            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.mainTexture = Resources.Load(info.path, typeof(Texture)) as Texture;
            renderer.material.color = Color.white;

            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor("_EmissionColor", emissionColor);
            renderer.material.DisableKeyword("_EMISSION");

            apps.Add(app);
        }
            
        launcher.GetComponent<SphereArranger>().Arange(apps, appScale);
        selectCounter = -1;
    }

    private void FixedUpdate()
    {
        if (spawnCounter > 0)
            --spawnCounter;
        if (moveCounter > 0)
            --moveCounter;
        if (selectCounter > 0)
            --selectCounter;
    }

    void Update()
    {
        if (moveCounter == 0)
        {
            prevCamPos = centerCamera.transform.position;
            moveCounter = 50; // 1 sec
        }

        if (selectCounter == 0)
        {
            selectCounter = -1;
            launcher.SetActive(false);
            selectedObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            selectedObject = null;
        }

        if (leftHand.IsTracked && rightHand.IsTracked)
        {
            bool isLeftIndexFingerPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
            bool isRightIndexFingerPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

            if (isLeftIndexFingerPinching && isRightIndexFingerPinching)
            {
                if (spawnCounter == 0)
                {
                    spawnCounter = 50; // 1 sec
                    Toggle();
                    moving = (Vector3.Distance(prevCamPos, centerCamera.transform.position) > 0.1f); // 10 cm
                }
            }

            //rotation // TODO: left hand doesnt have to be tracked
            if (!isLeftIndexFingerPinching && isRightIndexFingerPinching)
            {
                var delta = rightHand.transform.position - prevHandPos;

                if (delta.sqrMagnitude > 10e-7f)
                {
                    float rotationAngle = Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg;
                    rotationAngle = Mathf.Clamp(rotationAngle, -90f, 90f) * 0.02f;
                    launcher.transform.Rotate(0f, rotationAngle, 0f, Space.World);
                }

                prevHandPos = rightHand.transform.position;
            }

            if(isRightIndexFingerPinching)
                prevHandPos = rightHand.transform.position;
        }

        if (!launcher.activeSelf)
            return;

        if (moving && state == LauncherState.Central)
            SetState(LauncherState.Frontal);
        else if (!moving && state == LauncherState.Frontal)
            SetState(LauncherState.Central);
    }

    void SetState(LauncherState newState)
    {
        if (newState == LauncherState.Frontal)
        {
            launcher.transform.localPosition = frontalPos;
            launcher.transform.localScale = frontalScale;
            state = LauncherState.Frontal;
        }
        else if (newState == LauncherState.Central)
        {
            launcher.transform.localPosition = centralPos;
            launcher.transform.localScale = centralScale;
            state = LauncherState.Central;
        }
    }

    public void Toggle()
    {
        launcher.SetActive(!launcher.activeSelf);

        if (launcher.activeSelf)
        {
            Vector3 direction = centerCamera.transform.forward * frontalDistance;
            frontalPos.x = direction.x;
            frontalPos.y = frontalHeight;
            frontalPos.z = direction.z;
        }
    }

    public void SelectApp(GameObject obj)
    {
        if (selectedObject != null)
            return;

        selectedObject = obj;
        selectedObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        selectCounter = 50;
        Debug.Log("Selected: " + obj.name);
    }

    private void OnDestroy()
    {
        Destroy(launcher);
        foreach (GameObject app in apps)
        {
            Destroy(app);
        }
    }
}
