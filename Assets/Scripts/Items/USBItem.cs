using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USBItem : MonoBehaviour, IInteractable //Dejan, usb 3d object
{
    [SerializeField] SceneSwitch sceneSwitch = null; //a reference to the scene switch prefab (Fade In_Out)
    [SerializeField] int buildIndex = 0;

    [SerializeField] string message = "";

    public enum TypeOfRequirement { None, Either, All}
    [SerializeField] TypeOfRequirement typeOfRequirement;
    [SerializeField] List<int> questIDsToOpenDoor = new List<int>();

    public bool InteractWith() //executes when interacted with, changes scene to main menu
    {
        if (GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().Play();
        }
        sceneSwitch.SwitchScene(buildIndex);
        return false;
    }

    public string MessageOnDetection() //displays name on hover
    {
        return message;
    }

    public bool CanBeInteractedWith() //can always be interacted with
    {
        switch (typeOfRequirement)
        {
            case TypeOfRequirement.None:
                return true;
            case TypeOfRequirement.Either:
                for (int i = 0; i < questIDsToOpenDoor.Count; i++)
                {
                    if (QuestLog.Instance().QuestWithID(questIDsToOpenDoor[i]).ended)
                    {
                        return true;
                    }
                }
                return false;
            case TypeOfRequirement.All:
                for (int i = 0; i < questIDsToOpenDoor.Count; i++)
                {
                    if (!QuestLog.Instance().QuestWithID(questIDsToOpenDoor[i]).ended)
                    {
                        return false;
                    }
                }
                return true;
            default:
                return true;
        }
    }
    public void EndInteration() { } //not needed
}
