using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Attribute { Trust, Fear }
public enum RelationshipLevel { None, Familiar, Friend, Ally }

//Simon Voss
public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] string greetingText = "";
    [SerializeField] bool willingToTalk = true;
    [SerializeField] List<DialogueChoice> dialogueChoices = new List<DialogueChoice>();
    public List<Quest> quests = new List<Quest>();
    

    public bool TryStartTalking()
    {
        if (willingToTalk)
        {
            GameManager.instance.SetFPSControlState(false);
            Debug.Log("NPC talked with");
            DialogueSystem.instance.StartDialogue(this, greetingText, dialogueChoices);
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
        dialogueChoices = new List<DialogueChoice>();
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

    [Header("NPC-Player Relationship")]
    [SerializeField] RelationshipLevel relationship;
    [SerializeField] int trust = 0;
    [SerializeField] int fear = 0;
    public void AffectAttribute(Attribute attribute, int change)
    {
        switch (attribute)
        {
            case Attribute.Trust:
                trust += change;
                break;
            case Attribute.Fear:
                fear += change;
                break;
        }
        CalculateAndCheckRelationshipLevel();
    }

    public void AffectAttribute(List<Attribute> attributes, List<int> changes)
    {
        if(attributes.Count != changes.Count)
        {
            Debug.LogWarning("Attributes of NPC wont change since the attributes changes are not correctly set up");
            return;
        }
        for (int i = 0; i < attributes.Count; i++)
        {
            switch (attributes[i])
            {
                case Attribute.Trust:
                    trust += changes[i];
                    break;
                case Attribute.Fear:
                    fear += changes[i];
                    break;
            }
        }
        CalculateAndCheckRelationshipLevel();
    }

    int trustPerLevel = 10;
    public RelationshipLevel CalculateAndCheckRelationshipLevel()
    {
        relationship = (RelationshipLevel)Mathf.FloorToInt(trust / trustPerLevel);
        return relationship;
    }
}
