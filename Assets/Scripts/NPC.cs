using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RelationshipAttribute { Trust, Fear }
public enum RelationshipLevel { None, Familiar, Friend, Ally }

//Simon Voss
public class NPC : MonoBehaviour, IInteractable
{
    public string currentGreeting = "";
    //[SerializeField] string firstGreeting = "";
    //[SerializeField] string defaultGreeting = "";
    [SerializeField] List<string> greetings = new List<string>();

    [SerializeField] bool willingToTalk = true;
    //[SerializeField] List<DialogueChoice> dialogueChoices = new List<DialogueChoice>();
    //public List<Quest> quests = new List<Quest>();
    [SerializeField] Scenario dialogue = null;
    public List<ScriptableQuest> quests = new List<ScriptableQuest>();


    [SerializeField] SceneSwitch switchScene = null;
    [SerializeField] int ifCompletedSwitchSceneToIndex = 0;

    bool firstTimeTalkingWith = true;


    public bool TryStartTalking()
    {
        if (willingToTalk)
        {
            //Set greeting
            if (firstTimeTalkingWith)
            {
                currentGreeting = dialogue.startEvent.description;
                firstTimeTalkingWith = false;
            }
            else
            {
                if (quests.Count > 0)
                {
                    currentGreeting = dialogue.startEvent.description;
                }
                else
                {
                    currentGreeting = greetings[Random.Range(0, greetings.Count)];
                }
            }


            Debug.Log("NPC talked with");
            //DialogueSystem.instance.StartDialogue(this, currentGreeting, dialogueChoices);
            DialogueSystem.instance.StartDialogue(this, dialogue);
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
        dialogue = null;

        if (switchScene)
        {
            switchScene.SwitchScene(ifCompletedSwitchSceneToIndex);
        }
    }

    public bool InteractWith()
    {
        return TryStartTalking();
    }

    public string MessageOnDetection()
    {
        if (willingToTalk)
        {
            return "Tryck på E för att prata";
        }
        else
        {
            return "Du kan inte interagera med denna person";
        }
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
        if (attributes.Count != changes.Count)
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
