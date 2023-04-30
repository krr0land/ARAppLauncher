using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectInLauncher : MonoBehaviour
{
    public enum InteractionType { Pinch, Poke }

    [SerializeField]
    InteractionType interaction;
    // Start is called before the first frame update
    void Start()
    {
        ChangeInteraction(interaction);
    }

    public void ChangeInteraction(InteractionType interaction)
    {
        this.interaction = interaction;
        List<GameObject> apps = GetComponent<SpawnLauncher>().Apps;
        switch (interaction)
        {
            case InteractionType.Pinch:
                foreach (GameObject app in apps)
                {
                    app.GetComponent<DetectCollision>().enabled = false;
                    app.GetComponent<DetectPinch>().enabled = true;
                }
                break;
            case InteractionType.Poke:
                foreach (GameObject app in apps)
                {
                    app.GetComponent<DetectCollision>().enabled = true;
                    app.GetComponent<DetectPinch>().enabled = false;
                }
                break;
            default:
                foreach (GameObject app in apps)
                {
                    app.GetComponent<DetectCollision>().enabled = false;
                    app.GetComponent<DetectPinch>().enabled = false;
                }
                break;
        }
    }

    public void Disable()
    {
        List<GameObject> apps = GetComponent<SpawnLauncher>().Apps;
        foreach (GameObject app in apps)
        {
            app.GetComponent<DetectCollision>().enabled = false;
            app.GetComponent<DetectPinch>().enabled = false;
        }
    }

    public void Enable()
    {
        ChangeInteraction(interaction);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
