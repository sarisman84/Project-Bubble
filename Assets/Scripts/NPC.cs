using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Simon Voss
public class NPC : MonoBehaviour , IInteractable
{
    [SerializeField] bool willingToTalk = true;
    [SerializeField] string greetingText = "";
    [SerializeField] List<DialogueChoice> choices = new List<DialogueChoice>();
    public List<DialogueQuest> quests = new List<DialogueQuest>();

    public bool TryStartTalking()
    {
        if (willingToTalk)
        {
            GameManager.instance.SetFPSControlState(false);
            Debug.Log("NPC talked with");
            DialogueSystem.instance.StartDialogue(this, greetingText, choices);
            return true;
        }
        else
        {
            Debug.Log("This NPC does not want to talk");
            return false;
        }
    }

    public void CharacterCompleted()
    {
        choices = new List<DialogueChoice>();
        DialogueChoice newChoice = new DialogueChoice();
        greetingText = "Good day to you!";
    }

    public bool InteractWith()
    {
        return TryStartTalking();
    }

    public string MessageOnDetection()
    {
        return "Click E To Talk";
    }

    public bool CanBeInteractedWith()
    {
        if (DialogueSystem.instance.dialogueOpen)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void EndInteration()
    {
        //Is done through the dialogue system
    }
}
