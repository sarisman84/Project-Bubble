using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USBItem : MonoBehaviour, IInteractable //Dejan, usb 3d object
{
    [SerializeField] SceneSwitch sceneSwitch = null; //a reference to the scene switch prefab (Fade In_Out)

    public bool InteractWith() //executes when interacted with, changes scene to main menu
    {
        sceneSwitch.SwitchScene("Main Menu");
        return false;
    }

    public string MessageOnDetection() //displays name on hover
    {
        return "USB";
    }

    public bool CanBeInteractedWith() //can always be interacted with
    {
        return true;
    }
    public void EndInteration() { } //not needed
}
