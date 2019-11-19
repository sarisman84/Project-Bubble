using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RelationshipAttribute { Trust, Fear }
public enum RelationshipLevel { None, Familiar, Friend, Ally }

//Simon Voss
public class NPC : MonoBehaviour, IInteractable
{
    private string currentGreeting = "";
    [SerializeField] string firstGreeting = "";
    [SerializeField] string defaultGreeting = "";
    [SerializeField] List<string> greetings = new List<string>();

    [SerializeField] bool willingToTalk = true;
    [SerializeField] List<DialogueChoice> dialogueChoices = new List<DialogueChoice>();
    public List<Quest> quests = new List<Quest>();

    bool firstTimeTalkingWith = true;
    

    public bool TryStartTalking()
    {
        if (willingToTalk)
        {
            //Set greeting
            if (firstTimeTalkingWith)
            {
                currentGreeting = firstGreeting;
                firstTimeTalkingWith = false;
            }
            else
            {
                if (quests.Count > 0)
                {
                    currentGreeting = defaultGreeting;
                }
                else
                {
                    currentGreeting = greetings[Random.Range(0, greetings.Count)];
                }
            }


            GameManager.Instance().SetFPSInput(false);
            Debug.Log("NPC talked with");
            DialogueSystem.instance.StartDialogue(this, currentGreeting, dialogueChoices);
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
    public void AffectAttribute(RelationshipAttribute attribute, int change)
    {
        switch (attribute)
        {
            case RelationshipAttribute.Trust:
                trust += change;
                break;
            case RelationshipAttribute.Fear:
                fear += change;
                break;
        }
        CalculateAndCheckRelationshipLevel();
    }

    public void AffectAttribute(List<RelationshipAttribute> attributes, List<int> changes)
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
                case RelationshipAttribute.Trust:
                    trust += changes[i];
                    break;
                case RelationshipAttribute.Fear:
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
