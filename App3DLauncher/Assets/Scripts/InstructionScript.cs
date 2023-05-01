using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

struct Instruction
{
    public enum InteractionType { Pinch, Poke }
    public enum MovementType { Still, Walking }

    public string appName;
    public InteractionType interaction;
    public MovementType movement;
}

public class InstructionScript : MonoBehaviour
{
    [SerializeField]
    GameObject launcher;

    public SelectInLauncher selectInLauncher;
    public RotateLauncher rotateLauncher;

    private bool finished = false;
    TextMeshPro text;

    private List<List<Instruction>> instructions = new List<List<Instruction>>
    {new List<Instruction>{
        new Instruction
        {
            appName = "YouTube", interaction = Instruction.InteractionType.Poke, movement = Instruction.MovementType.Still
        },
        new Instruction
        {
            appName = "Spotify", interaction = Instruction.InteractionType.Poke, movement = Instruction.MovementType.Still
        },
        new Instruction
        {
            appName = "Hulu", interaction = Instruction.InteractionType.Poke, movement = Instruction.MovementType.Walking
        },
        new Instruction
        {
            appName = "Google chrome", interaction = Instruction.InteractionType.Poke, movement = Instruction.MovementType.Walking
        }
    },
    new List<Instruction>{
        new Instruction
        {
            appName = "Pinterest", interaction = Instruction.InteractionType.Pinch, movement = Instruction.MovementType.Still
        },
        new Instruction
        {
            appName = "Reddit", interaction = Instruction.InteractionType.Pinch, movement = Instruction.MovementType.Still
        },
        new Instruction
        {
            appName = "TikTok", interaction = Instruction.InteractionType.Pinch, movement = Instruction.MovementType.Walking
        },
        new Instruction
        {
            appName = "VLC", interaction = Instruction.InteractionType.Pinch, movement = Instruction.MovementType.Walking
        }
    }};

    int instructionNum;

    int taskId = 0;
    // Start is called before the first frame update
    void Start()
    {
        instructionNum = 1;
        text = GetComponent<TextMeshPro>();
        text.text = "Task not selected";
        taskId = -1;
        NextTask();
    }

    // Update is called once per frame
    void Update()
    {
        var controller = OVRInput.GetActiveController();
        if (OVRInput.GetDown(OVRInput.RawButton.A) &&
            controller != OVRInput.Controller.Hands &&
            controller != OVRInput.Controller.LHand &&
            controller != OVRInput.Controller.RHand)
        {
            Debug.Log("A pressed");
            instructionNum = (instructionNum + 1) % instructions.Count;
            finished = false;
            taskId = -1;
            NextTask();
        }

        if (OVRInput.GetDown(OVRInput.RawButton.B) &&
            controller != OVRInput.Controller.Hands &&
            controller != OVRInput.Controller.LHand &&
            controller != OVRInput.Controller.RHand)
        {
            Debug.Log("B pressed");
            NextTask();
        }

        if (finished)
        {
            text.text = $"You finished all interactions!";
            return;
        }

        var task = instructions[instructionNum][taskId];

        string movementText;
        if (task.movement != Instruction.MovementType.Walking)
        {
            movementText = "Stand still";
        }
        else
        {
            movementText = "Open the launcher when walking";
        }

        if (launcher.activeSelf)
        {
            if (task.interaction == Instruction.InteractionType.Pinch)
            {
                text.text = $"{movementText}\nSelect an app by pinching: {task.appName}";
            }
            else
            {
                text.text = $"{movementText}\nSelect an app by poking: {task.appName}";
            }
        }
        else
        {
            text.text = $"{movementText}\nOpen the launcher by poking on both hands";
        }
    }

    async void NextTask()
    {
        await Task.Delay(1000);
        if (taskId < instructions[instructionNum].Count - 1)
        {
            taskId++;
            if (instructions[instructionNum][taskId].interaction == Instruction.InteractionType.Pinch)
            {
                selectInLauncher.ChangeInteraction(SelectInLauncher.InteractionType.Pinch);
                rotateLauncher.ChangeInteraction(RotateLauncher.InteractionType.Pinch);
            }
            else
            {
                selectInLauncher.ChangeInteraction(SelectInLauncher.InteractionType.Poke);
                rotateLauncher.ChangeInteraction(RotateLauncher.InteractionType.Poke);
            }
            if (instructions[instructionNum][taskId].movement != Instruction.MovementType.Walking)
            {
                launcher.GetComponent<SpawnLauncher>().SetState(LauncherState.Central);
            }
            else
            {
                launcher.GetComponent<SpawnLauncher>().SetState(LauncherState.Frontal);
            }
        }
        else
        {
            finished = true;
        }
    }

    public void OnAppSelected(string objName)
    {
        if (String.Equals(objName, instructions[instructionNum][taskId].appName, StringComparison.OrdinalIgnoreCase))
        {
            NextTask();
        }
    }
}
