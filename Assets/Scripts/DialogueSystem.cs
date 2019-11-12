﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

        EndDialogue();
    }

    public void UseChoice(Choice choice)
    {
        switch (choice.choiceType)
        {
            case Choice.TypeOfChoice.Dialogue:
                ContinueDialogue(choice.answerSubtitle, choice.newChoices);
                PlayerCharacteristics.instance.IncreaseStat(choice.choiceStyle);
                break;
            case Choice.TypeOfChoice.GiveItem:
                Inventory.instance.AddItemToInventory(choice.rewardItemID);
                ContinueDialogue(choice.answerSubtitle, choice.newChoices);
                PlayerCharacteristics.instance.IncreaseStat(choice.choiceStyle);
                break;
            case Choice.TypeOfChoice.TakeItem:
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
            case Choice.TypeOfChoice.EndDialogue:
                PlayerCharacteristics.instance.IncreaseStat(choice.choiceStyle);
                EndDialogue();
                break;
        }
    }


    public void StartDialogue(string greeting, List<Choice> choices)
    {
        dialogueOpen = true;
        Cursor.lockState = CursorLockMode.None;
        choicePanel.SetActive(true);
        subtitlesPanel.SetActive(true);
        ContinueDialogue(greeting, choices);
    }

    public void ContinueDialogue(string answer, List<Choice> furtherChoices)
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }

        choiceButtons[0].SetActive(true);
        choiceTexts[0].text = "Goodbye";
        Choice defaultEndChoice = new Choice();
        defaultEndChoice.choiceType = Choice.TypeOfChoice.EndDialogue;
        choiceButtons[0].transform.GetComponent<DialogueChoice>().mychoice = defaultEndChoice;

        if (furtherChoices != null && furtherChoices.Count > 0)
        {
            for (int i = 0; i < furtherChoices.Count; i++)
            {
                choiceButtons[i+1].SetActive(true);
                choiceTexts[i+1].text = furtherChoices[i].choiceText;
                choiceButtons[i+1].transform.GetComponent<DialogueChoice>().mychoice = furtherChoices[i];
            }
        }
        subtitleText.text = answer;
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
    }

}
