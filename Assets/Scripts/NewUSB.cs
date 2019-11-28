using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewUSB : MonoBehaviour, IInteractable
{
    [SerializeField] string messageOnHover = null;
    [SerializeField] int questIDToActivate = 0;

    public bool CanBeInteractedWith() { return true; }
    public void EndInteration() { }

    public bool InteractWith()
    {
        GameManager.Instance().USBPickedUp = true;
        QuestLog.Instance().ActivateQuest(questIDToActivate);
        Destroy(gameObject);
        return true;
    }

    public string MessageOnDetection()
    {
        return messageOnHover;
    }
}
