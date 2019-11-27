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
    Scenario dialogue = null;
    Event currentEvent = null;
    //[SerializeField] TextMeshProUGUI locationText = null;
    //[SerializeField] Image background = null;
    [SerializeField] TextMeshProUGUI subtitleText = null;
    [SerializeField] GameObject subtitlesPanel = null;
    [SerializeField] GameObject[] choiceButtons = null;
    [SerializeField] TextMeshProUGUI[] choiceButtonTexts = null;
    [SerializeField] PlayerCharacteristics playerStats = null;
    [SerializeField] ScriptableInventory inventory = null;
    public bool dialogueOpen = false;


    private void Start()
    {
        if (inventory == null)
        {
            Debug.LogError("Inventory is missing");
        }
        if (playerStats == null)
        {
            Debug.LogError("Playercharacteristics is missing");
        }
    }

    public void StartDialogue(NPC npcTalking, Scenario dialogue)
    {
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance().SetFPSInput(false);
        dialogueOpen = true;
        npcTalkedTo = npcTalking;
        if (dialogue == null)
        {
            ShowDefault();
        }
        else
        {
            this.dialogue = dialogue;
            currentEvent = dialogue.startEvent;
            DisplayGUI(currentEvent);
        }
    }

    public void ShowDefault()
    {
        subtitlesPanel.SetActive(true);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }
        Choice defaultChoice = new Choice("Hej hej");
        DrawChoice(defaultChoice, 0);
        //background.sprite = currentEvent.image;
        subtitleText.text = npcTalkedTo.currentGreeting;
    }

    public void MakeChoice(int index)
    {
        if (currentEvent == null)
        {
            EndDialogue();
            return;
        }
        Choice usedChoice = currentEvent.choices[index];

        if (currentEvent.choices[index].nextEvent != null && currentEvent.choices[index].nextEvent.choices.Count > 0)
        {
            Debug.Log("Changing event");
            currentEvent = currentEvent.choices[index].nextEvent;
            AffectPlayer(usedChoice);
            DisplayGUI(currentEvent);
        }
        else if (currentEvent.choices[index].nextScenario != null)
        {
            Debug.LogWarning("Not implemented scenario change");
        }
        else if (currentEvent.choices[index].nextScene != null)
        {
            Debug.LogWarning("Not implemented scenechange");
        }
        else
        {
            Debug.Log("No further events or endnodes found, check setup of scenario from node. Dialogue will close from node: " + currentEvent.locationText);
            AffectPlayer(usedChoice);
            EndDialogue();
            //return;
        }
    }

    private void AffectPlayer(Choice usedChoice)
    {
        playerStats.IncreaseStat(usedChoice.skillType, usedChoice.skillNumberIncrease);

        //AFFECT Relationship
        if (usedChoice.affectedNPC)
        {
            usedChoice.affectedNPC.AffectAttribute(usedChoice.relationshipAttributeToChange, usedChoice.relationshipAttributeChangeNumber);
        }

        //Give take item
        switch (usedChoice.itemtransfer)
        {
            case ItemTransfer.Off:
                break;
            case ItemTransfer.PlayerGetItem:
                inventory.AddItemToInventory(usedChoice.item);
                break;
            case ItemTransfer.PlayerLoseItem:
                inventory.RemoveItemFromInventory(usedChoice.item);
                break;
        }

        if (usedChoice.connectedQuest != null)
        {
            switch (usedChoice.processQuest)
            {
                case QuestProcessing.GiveQuest:
                    playerStats.activeQuests.Add(usedChoice.connectedQuest);
                    Debug.Log("Quest added: " + usedChoice.connectedQuest.questName);
                    break;
                case QuestProcessing.CompleteQuest:
                    if (playerStats.activeQuests.Contains(usedChoice.connectedQuest))
                    {
                        if (npcTalkedTo.quests.Contains(usedChoice.connectedQuest))
                        {
                            npcTalkedTo.quests.Remove(usedChoice.connectedQuest);

                        }
                        playerStats.activeQuests.Remove(usedChoice.connectedQuest);
                        playerStats.completedQuests.Add(usedChoice.connectedQuest);
                        QuestLog.Instance().EndQuest(usedChoice.connectedQuest.id);
                        Debug.Log("Quest completed: " + usedChoice.connectedQuest.questName);
                    }
                    else
                    {
                        Debug.LogError("Tried to complete a quest not given to the player");
                    }
                    break;
                case QuestProcessing.Failquest:
                    if (playerStats.activeQuests.Contains(usedChoice.connectedQuest))
                    {
                        if (npcTalkedTo.quests.Contains(usedChoice.connectedQuest))
                        {
                            npcTalkedTo.quests.Remove(usedChoice.connectedQuest);
                        }
                        playerStats.activeQuests.Remove(usedChoice.connectedQuest);
                        playerStats.failedQuests.Add(usedChoice.connectedQuest);
                        Debug.Log("Quest failed: " + usedChoice.connectedQuest.questName);
                    }
                    else
                    {
                        Debug.LogError("Tried to fail a quest not given to the player");
                    }
                    break;
            }
        }
    }

    private void DisplayGUI(Event input)
    {
        subtitlesPanel.SetActive(true);
        //locationText.text = input.locationText;
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }
        for (int i = 0; i < input.choices.Count; i++)
        {
            DrawChoice(input.choices[i], i);
        }
        //background.sprite = currentEvent.image;
        subtitleText.text = currentEvent.description;
    }

    private void DrawChoice(Choice choice, int choiceIndex)
    {
        bool isAllowed = true;
        string fault = "";
        if (choice.itemtransfer == ItemTransfer.PlayerLoseItem && !inventory.CheckIfItemIsInInventory(choice.item))
        {
            fault = " - you don't have the item";
            isAllowed = false;
        }

        if (choice.affectedNPC != null && (int)choice.minimumRelationshiplevel > (int)choice.affectedNPC.CalculateAndCheckRelationshipLevel())
        {
            fault = " - you don't know the person well enough";
            isAllowed = false;
        }
        if (choice.requiredSkill != Characteristics.None && choice.requiredSkillNumber > playerStats.GetCharacteristic(choice.requiredSkill))
        {
            fault = " - you don't have the required skill";
            isAllowed = false;
        }

        if (choice.requiredStartedQuest != null)
        {
            if (!playerStats.activeQuests.Contains(choice.requiredStartedQuest))
            {
                fault = " - you have not started the needed quest";
                isAllowed = false;
            }
        }

        if (choice.requiredCompletedQuest != null)
        {
            if (!playerStats.completedQuests.Contains(choice.requiredCompletedQuest))
            {
                fault = " - you have not completed the needed quest";
                isAllowed = false;
            }
        }

        if (isAllowed)
        {
            choiceButtons[choiceIndex].GetComponent<Button>().interactable = true;
            choiceButtons[choiceIndex].SetActive(true);
            choiceButtonTexts[choiceIndex].text = choice.choiceText;
        }
        else
        {
            //choiceButtons[choiceIndex].GetComponent<Button>().interactable = false;
            //choiceButtons[choiceIndex].SetActive(true);
            //choiceButtonTexts[choiceIndex].text = choice.choiceText + fault;
        }
    }

    private void EndDialogue()
    {
        currentEvent = null;
        dialogue = null;
        subtitlesPanel.SetActive(false);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }
        dialogueOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance().SetFPSInput(true);
        GameManager.Instance().SetMouseLook(true);
        if (npcTalkedTo.quests.Count == 0)
        {
            npcTalkedTo.CharacterCompleted();
        }
    }




    //NPC npcTalkedTo;
    //GameObject[] choiceButtons;
    //TextMeshProUGUI[] choiceTexts;
    //[SerializeField] GameObject choicePanel = null;
    //[SerializeField] GameObject subtitlesPanel = null;
    //TextMeshProUGUI subtitleText;
    //[SerializeField] PlayerCharacteristics playerStats = null;
    //public bool dialogueOpen = false;

    ////Sets up the references needed
    //private void Start()
    //{
    //    if (!playerStats)
    //    {
    //        Debug.LogError("Playercharacteristics is missing");
    //    }

    //    choiceButtons = new GameObject[choicePanel.transform.childCount];
    //    choiceTexts = new TextMeshProUGUI[choicePanel.transform.childCount];

    //    for (int i = 0; i < choiceButtons.Length; i++)
    //    {
    //        choiceButtons[i] = choicePanel.transform.GetChild(i).gameObject;
    //        choiceTexts[i] = choicePanel.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
    //    }
    //    subtitleText = subtitlesPanel.GetComponentInChildren<TextMeshProUGUI>();
    //}





    ////Handles our dialogue input
    //public void UseChoice(DialogueChoice choice)
    //{
    //    if (choice.canBeReused)
    //    {
    //        choice.isUsed = true;
    //    }

    //    if (choice.connectedQuest != null)
    //    {
    //        for (int i = 0; i < npcTalkedTo.quests.Count; i++)
    //        {
    //            if (npcTalkedTo.quests[i].id == choice.connectedQuest.id)
    //            {
    //                npcTalkedTo.quests.RemoveAt(i);
    //                i--;
    //            }
    //        }
    //    }

    //    switch (choice.optionAction)
    //    {
    //        case DialogueChoice.TypeOfChoice.Dialogue:
    //            ContinueDialogue(choice.responseText, choice.continuedChoices);
    //            playerStats.IncreaseStat(choice.characteristics);
    //            break;
    //        case DialogueChoice.TypeOfChoice.PlayerGetsItem:
    //            Inventory.instance.AddItemToInventory(choice.getItemID);
    //            ContinueDialogue(choice.responseText, choice.continuedChoices);
    //            playerStats.IncreaseStat(choice.characteristics);
    //            break;
    //        case DialogueChoice.TypeOfChoice.PlayerLosesItem:
    //            if (Inventory.instance.RemoveItemFromInventory(choice.loseItemID))
    //            {
    //                ContinueDialogue(choice.responseText, choice.continuedChoices);
    //                playerStats.IncreaseStat(choice.characteristics);
    //            }
    //            else
    //            {
    //                ContinueDialogue("(You don't have the requested item)", null);
    //            }
    //            break;
    //        case DialogueChoice.TypeOfChoice.EndDialogue:
    //            playerStats.IncreaseStat(choice.characteristics);
    //            EndDialogue();
    //            break;
    //    }

    //    if (choice.attributeChanges != null && choice.attributesToChange != null && choice.attributeChanges.Count > 0 && choice.attributesToChange.Count > 0)
    //    {
    //        npcTalkedTo.AffectAttribute(choice.attributesToChange, choice.attributeChanges);
    //    }
    //}

    ////Opens the dialogue UI
    //public void StartDialogue(NPC npcTalking, string greeting, List<DialogueChoice> choices)
    //{
    //    npcTalkedTo = npcTalking;
    //    dialogueOpen = true;
    //    Cursor.lockState = CursorLockMode.None;
    //    choicePanel.SetActive(true);
    //    subtitlesPanel.SetActive(true);
    //    ContinueDialogue(greeting, choices);
    //}

    ////Shows choices to the player and activates buttons to answer
    //public void ContinueDialogue(string answer, List<DialogueChoice> furtherChoices)
    //{
    //    for (int i = 0; i < choiceButtons.Length; i++)
    //    {
    //        choiceButtons[i].SetActive(false);
    //    }

    //    choiceButtons[0].SetActive(true);
    //    choiceTexts[0].text = "(End conversation)";
    //    DialogueChoice defaultEndChoice = new DialogueChoice();
    //    defaultEndChoice.optionAction = DialogueChoice.TypeOfChoice.EndDialogue;
    //    choiceButtons[0].transform.GetComponent<DialogueChoiceHolder>().mychoice = defaultEndChoice;
    //    choiceButtons[0].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CalculateSquareWidth(choiceTexts[0].text));


    //    for (int i = 0; i < furtherChoices.Count; i++)
    //    {
    //        if (furtherChoices[i].isUsed)
    //        {
    //            furtherChoices.RemoveAt(i);
    //            continue;
    //        }
    //        if (furtherChoices[i].optionAction == DialogueChoice.TypeOfChoice.PlayerLosesItem && !Inventory.instance.CheckIfItemIsInInventory(furtherChoices[i].loseItemID))
    //        {
    //            furtherChoices.RemoveAt(i);
    //            continue;
    //        }
    //    }

    //    if (furtherChoices != null && furtherChoices.Count > 0)
    //    {
    //        for (int i = 0; i < furtherChoices.Count; i++)
    //        {
    //            choiceButtons[i + 1].SetActive(true);
    //            choiceTexts[i + 1].text = furtherChoices[i].choiceText;
    //            choiceButtons[i + 1].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CalculateSquareWidth(furtherChoices[i].choiceText));
    //            choiceButtons[i + 1].transform.GetComponent<DialogueChoiceHolder>().mychoice = furtherChoices[i];
    //        }
    //    }
    //    subtitleText.text = "'" + answer + "'";
    //    subtitlesPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CalculateSquareWidth(answer));
    //}

    ////Ends the dialogue and closes the UI windows
    //public void EndDialogue()
    //{
    //    choicePanel.SetActive(false);
    //    subtitlesPanel.SetActive(false);
    //    for (int i = 0; i < choiceButtons.Length; i++)
    //    {
    //        choiceButtons[i].SetActive(false);
    //    }
    //    Cursor.lockState = CursorLockMode.Locked;
    //    dialogueOpen = false;
    //    GameManager.Instance().SetFPSInput(true);

    //    if (npcTalkedTo.quests.Count == 0)
    //    {
    //        npcTalkedTo.CharacterCompleted();
    //    }
    //    npcTalkedTo = null;
    //}

    ////Calculates width for UI elements
    //float pixelsPerLetter = 12;
    //float baseWidth = 50;
    //private float CalculateSquareWidth(string input)
    //{
    //    char[] chars = input.ToCharArray();

    //    return chars.Length * pixelsPerLetter + baseWidth;
    //}
}

//[System.Serializable]
//public class DialogueChoice
//{
//    public string choiceText;
//    public enum TypeOfChoice { Dialogue, PlayerGetsItem, PlayerLosesItem, EndDialogue }
//    public TypeOfChoice optionAction;

//    public Characteristics characteristics;
//    public int getItemID;
//    public int loseItemID;
//    public string responseText;

//    public List<RelationshipAttribute> attributesToChange;
//    public List<int> attributeChanges;

//    public bool canBeReused = false;
//    public bool isUsed { get; set; }
//    public Quest connectedQuest;

//    public List<DialogueChoice> continuedChoices;
//}

//[System.Serializable]
//public class Quest
//{
//    public string description = "";
//    public int id;
//}
