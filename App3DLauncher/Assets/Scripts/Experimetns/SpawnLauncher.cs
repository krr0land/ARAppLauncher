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

    public int appCount = 50;

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

    bool moving = true;
    int moveCounter = 0;
    int spawnCounter = 0;

    public GameObject Launcher { get { return launcher; } }

    public bool appSelected = false;


    void Start()
    {
        prevCamPos = centerCamera.transform.position;

        launcher = Instantiate(launcherPrefab, Vector3.zero, Quaternion.identity, transform);
        Toggle();
        SetState(LauncherState.Frontal);

        apps = new List<GameObject>();
        for (int i = 0; i < appCount; i++)
            apps.Add(Instantiate(appPrefab, transform.position, Quaternion.identity, launcher.transform));
        launcher.GetComponent<SphereArranger>().Arange(apps, appScale);
    }

    private void FixedUpdate()
    {
        if (spawnCounter > 0)
            --spawnCounter;
        if (moveCounter > 0)
            --moveCounter;
    }


    void Update()
    {
        if (moveCounter == 0)
        {
            moving = (Vector3.Distance(prevCamPos, centerCamera.transform.position) > 0.1f); // 10 cm
            prevCamPos = centerCamera.transform.position;
            moveCounter = 50; // 1 sec
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
                }
            }
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

    private void OnDestroy()
    {
        Destroy(launcher);
        foreach (GameObject app in apps)
        {
            Destroy(app);
        }
    }
}
