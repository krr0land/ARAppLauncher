using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

struct Task
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

    Task task => instructions[taskId];
    private bool finished = false;
    TextMeshPro text;

    private List<Task> instructions1 = new List<Task>
    {
        new Task
        {
            appName = "Youtube", interaction = Task.InteractionType.Poke, movement = Task.MovementType.Still
        },
        new Task
        {
            appName = "Spotify", interaction = Task.InteractionType.Poke, movement = Task.MovementType.Still
        },
        new Task
        {
            appName = "Hulu", interaction = Task.InteractionType.Pinch, movement = Task.MovementType.Walking
        },
        new Task
        {
            appName = "Chrome", interaction = Task.InteractionType.Pinch, movement = Task.MovementType.Walking
        }
    };

    private List<Task> instructions2 = new List<Task>
    {
        new Task
        {
            appName = "Youtube2", interaction = Task.InteractionType.Pinch, movement = Task.MovementType.Still
        },
        new Task
        {
            appName = "Spotify2", interaction = Task.InteractionType.Pinch, movement = Task.MovementType.Still
        },
        new Task
        {
            appName = "Hulu2", interaction = Task.InteractionType.Pinch, movement = Task.MovementType.Walking
        },
        new Task
        {
            appName = "Chrome2", interaction = Task.InteractionType.Pinch, movement = Task.MovementType.Walking
        }
    };
    
    List<Task> instructions;

    [SerializeField]
    int taskId = 0;
    // Start is called before the first frame update
    void Start()
    {
        instructions = instructions1;
        text = GetComponent<TextMeshPro>();
        text.text = "Task not selected";
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            if (instructions == instructions1)
            {
                instructions = instructions2;
            }
            if (instructions == instructions2)
            {
                instructions = instructions1;
            }

            taskId = 0;
        }
        
        if(Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.RawButton.B))
        {
            NextTask();
        }
        
        if (finished)
        {
            text.text = $"You finished all interactions!";
            return;
        }
        
        string movementText;
        if (task.movement == Task.MovementType.Walking)
        {
            movementText = "Stand still";
        }
        else
        {
            movementText = "Open the launcher when walking";
        }
        
        if (launcher.activeSelf)
        {
            if (task.interaction == Task.InteractionType.Pinch)
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

    void NextTask()
    {
        if(taskId < instructions.Count - 1)
        {
            taskId++;
            if (task.interaction == Task.InteractionType.Pinch)
            {
                selectInLauncher.ChangeInteraction(SelectInLauncher.InteractionType.Pinch);
                rotateLauncher.ChangeInteraction(RotateLauncher.InteractionType.Pinch);
            }
            else
            {
                selectInLauncher.ChangeInteraction(SelectInLauncher.InteractionType.Poke);
                rotateLauncher.ChangeInteraction(RotateLauncher.InteractionType.Poke);
            }
        }
        else
        {
            finished = true;
        }
    }

    public void OnAppSelected(string objName)
    {
        if (String.Equals(objName, task.appName, StringComparison.CurrentCultureIgnoreCase))
        {
            NextTask();
        }
    }
}
