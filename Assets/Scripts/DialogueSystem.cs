using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    Text[] choiceTexts;
    [SerializeField] GameObject choicePanel = null;
    [SerializeField] GameObject subtitlesPanel = null;
    Text subtitleText;
    public bool dialogueOpen = false;

    private void Start()
    {
        choiceButtons = new GameObject[choicePanel.transform.childCount];
        choiceTexts = new Text[choicePanel.transform.childCount];

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i] = choicePanel.transform.GetChild(i).gameObject;
            choiceTexts[i] = choicePanel.transform.GetChild(i).GetComponentInChildren<Text>();
        }
        subtitleText = subtitlesPanel.GetComponentInChildren<Text>();
    }

    public void UseChoice(DialogueChoice choice)
    {
        if (choice.isExpandable)
        {
            choice.isExpended = true;
        }

        for (int i = 0; i < npcTalkedTo.quests.Count; i++)
        {
            if (npcTalkedTo.quests[i].id == choice.completeDialogueQuestID)
            {
                npcTalkedTo.quests.RemoveAt(i);
                i--;
            }
        }

        switch (choice.choiceType)
        {
            case DialogueChoice.TypeOfChoice.Dialogue:
                ContinueDialogue(choice.answerSubtitle, choice.newChoices);
                PlayerCharacteristics.instance.IncreaseStat(choice.choiceStyle);
                break;
            case DialogueChoice.TypeOfChoice.GiveItem:
                Inventory.instance.AddItemToInventory(choice.rewardItemID);
                ContinueDialogue(choice.answerSubtitle, choice.newChoices);
                PlayerCharacteristics.instance.IncreaseStat(choice.choiceStyle);
                break;
            case DialogueChoice.TypeOfChoice.TakeItem:
                if (Inventory.instance.RemoveItemFromInventory(choice.requestItemID))
                {
                    ContinueDialogue(choice.answerSubtitle, choice.newChoices);
                    PlayerCharacteristics.instance.IncreaseStat(choice.choiceStyle);
                }
                else
                {
                    ContinueDialogue("(You don't have the requested item)", null);
                }
                break;
            case DialogueChoice.TypeOfChoice.EndDialogue:
                PlayerCharacteristics.instance.IncreaseStat(choice.choiceStyle);
                EndDialogue();
                break;
        }
    }


    public void StartDialogue(NPC npcTalking, string greeting, List<DialogueChoice> choices)
    {
        npcTalkedTo = npcTalking;
        dialogueOpen = true;
        Cursor.lockState = CursorLockMode.None;
        choicePanel.SetActive(true);
        subtitlesPanel.SetActive(true);
        ContinueDialogue(greeting, choices);
    }

    public void ContinueDialogue(string answer, List<DialogueChoice> furtherChoices)
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }

        choiceButtons[0].SetActive(true);
        choiceTexts[0].text = "(End conversation)";
        DialogueChoice defaultEndChoice = new DialogueChoice();
        defaultEndChoice.choiceType = DialogueChoice.TypeOfChoice.EndDialogue;
        choiceButtons[0].transform.GetComponent<DialogueChoiceHolder>().mychoice = defaultEndChoice;
        choiceButtons[0].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CalculateSquareWidth(choiceTexts[0].text));


        for (int i = 0; i < furtherChoices.Count; i++)
        {
            if (furtherChoices[i].isExpended)
            {
                furtherChoices.RemoveAt(i);
                continue;
            }
            if (furtherChoices[i].choiceType == DialogueChoice.TypeOfChoice.TakeItem && !Inventory.instance.CheckIfItemIsInInventory(furtherChoices[i].requestItemID))
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
        GameManager.instance.SetFPSControlState(true);

        if (npcTalkedTo.quests.Count == 0)
        {
            npcTalkedTo.CharacterCompleted();
        }
        npcTalkedTo = null;
    }

    float pixelsPerLetter = 15;
    private float CalculateSquareWidth(string input)
    {
        char[] chars = input.ToCharArray();

        return chars.Length * pixelsPerLetter;
    }
}
