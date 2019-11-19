using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Simon Voss
public class DialogueSystem : MonoBehaviour
{
    #region Singleton
    public static DialogueSystem instance;
    private void Awake()
    {
        if (DialogueSystem.instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Another instance of: " + this + " , was tried to be instantiated, but was destroyed! This instance was tried to be instantiated on: " + this.gameObject);
            Destroy(this);
        }
    }
    #endregion


    NPC npcTalkedTo;
    GameObject[] choiceButtons;
    TextMeshProUGUI[] choiceTexts;
    [SerializeField] GameObject choicePanel = null;
    [SerializeField] GameObject subtitlesPanel = null;
    TextMeshProUGUI subtitleText;
    public bool dialogueOpen = false;

    //Sets up the references needed
    private void Start()
    {
        choiceButtons = new GameObject[choicePanel.transform.childCount];
        choiceTexts = new TextMeshProUGUI[choicePanel.transform.childCount];

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i] = choicePanel.transform.GetChild(i).gameObject;
            choiceTexts[i] = choicePanel.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
        }
        subtitleText = subtitlesPanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    //Handles our dialogue input
    public void UseChoice(DialogueChoice choice)
    {
        if (choice.canBeReused)
        {
            choice.isUsed = true;
        }

        if (choice.connectedQuest != null)
        {
            for (int i = 0; i < npcTalkedTo.quests.Count; i++)
            {
                if (npcTalkedTo.quests[i].id == choice.connectedQuest.id)
                {
                    npcTalkedTo.quests.RemoveAt(i);
                    i--;
                }
            }
        }

        switch (choice.optionAction)
        {
            case DialogueChoice.TypeOfChoice.Dialogue:
                ContinueDialogue(choice.responseText, choice.continuedChoices);
                PlayerCharacteristics.instance.IncreaseStat(choice.characteristics);
                break;
            case DialogueChoice.TypeOfChoice.PlayerGetsItem:
                Inventory.instance.AddItemToInventory(choice.getItemID);
                ContinueDialogue(choice.responseText, choice.continuedChoices);
                PlayerCharacteristics.instance.IncreaseStat(choice.characteristics);
                break;
            case DialogueChoice.TypeOfChoice.PlayerLosesItem:
                if (Inventory.instance.RemoveItemFromInventory(choice.loseItemID))
                {
                    ContinueDialogue(choice.responseText, choice.continuedChoices);
                    PlayerCharacteristics.instance.IncreaseStat(choice.characteristics);
                }
                else
                {
                    ContinueDialogue("(You don't have the requested item)", null);
                }
                break;
            case DialogueChoice.TypeOfChoice.EndDialogue:
                PlayerCharacteristics.instance.IncreaseStat(choice.characteristics);
                EndDialogue();
                break;
        }

        if (choice.attributeChanges != null && choice.attributesToChange != null && choice.attributeChanges.Count > 0 && choice.attributesToChange.Count > 0)
        {
            npcTalkedTo.AffectAttribute(choice.attributesToChange, choice.attributeChanges);
        }
    }

    //Opens te dialogue UI
    public void StartDialogue(NPC npcTalking, string greeting, List<DialogueChoice> choices)
    {
        npcTalkedTo = npcTalking;
        dialogueOpen = true;
        Cursor.lockState = CursorLockMode.None;
        choicePanel.SetActive(true);
        subtitlesPanel.SetActive(true);
        ContinueDialogue(greeting, choices);
    }

    //Shows choices to the player and activates buttons to answer
    public void ContinueDialogue(string answer, List<DialogueChoice> furtherChoices)
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }

        choiceButtons[0].SetActive(true);
        choiceTexts[0].text = "(End conversation)";
        DialogueChoice defaultEndChoice = new DialogueChoice();
        defaultEndChoice.optionAction = DialogueChoice.TypeOfChoice.EndDialogue;
        choiceButtons[0].transform.GetComponent<DialogueChoiceHolder>().mychoice = defaultEndChoice;
        choiceButtons[0].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CalculateSquareWidth(choiceTexts[0].text));


        for (int i = 0; i < furtherChoices.Count; i++)
        {
            if (furtherChoices[i].isUsed)
            {
                furtherChoices.RemoveAt(i);
                continue;
            }
            if (furtherChoices[i].optionAction == DialogueChoice.TypeOfChoice.PlayerLosesItem && !Inventory.instance.CheckIfItemIsInInventory(furtherChoices[i].loseItemID))
            {
                furtherChoices.RemoveAt(i);
                continue;
            }
        }

        if (furtherChoices != null && furtherChoices.Count > 0)
        {
            for (int i = 0; i < furtherChoices.Count; i++)
            {
                choiceButtons[i + 1].SetActive(true);
                choiceTexts[i + 1].text = furtherChoices[i].choiceText;
                choiceButtons[i + 1].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CalculateSquareWidth(furtherChoices[i].choiceText));
                choiceButtons[i + 1].transform.GetComponent<DialogueChoiceHolder>().mychoice = furtherChoices[i];
            }
        }
        subtitleText.text = "'" + answer + "'";
        subtitlesPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CalculateSquareWidth(answer));
    }

    //Ends the dialogue and closes the UI windows
    public void EndDialogue()
    {
        choicePanel.SetActive(false);
        subtitlesPanel.SetActive(false);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }
        Cursor.lockState = CursorLockMode.Locked;
        dialogueOpen = false;
        GameManager.Instance().SetFPSInput(true);

        if (npcTalkedTo.quests.Count == 0)
        {
            npcTalkedTo.CharacterCompleted();
        }
        npcTalkedTo = null;
    }

    //Calculates width for UI elements
    float pixelsPerLetter = 12;
    float baseWidth = 50;
    private float CalculateSquareWidth(string input)
    {
        char[] chars = input.ToCharArray();

        return chars.Length * pixelsPerLetter + baseWidth;
    }
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public enum TypeOfChoice { Dialogue, PlayerGetsItem, PlayerLosesItem, EndDialogue }
    public TypeOfChoice optionAction;

    public Characteristics characteristics;
    public int getItemID;
    public int loseItemID;
    public string responseText;

    public List<RelationshipAttribute> attributesToChange;
    public List<int> attributeChanges;

    public bool canBeReused = false;
    public bool isUsed { get; set; }
    public Quest connectedQuest;

    public List<DialogueChoice> continuedChoices;
}

[System.Serializable]
public class Quest
{
    public string description = "";
    public int id;
}
