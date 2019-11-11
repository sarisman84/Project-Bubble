using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChoice : MonoBehaviour
{
    public Choice mychoice;

    public void UseChoice()
    {
        if (mychoice.choiceType == Choice.TypeOfChoice.EndDialogue)
        {
            DialogueSystem.instance.EndDialogue();
        }
        Debug.Log("Choice was used: " + mychoice.choiceText);

        Choice endChoice = new Choice();
        endChoice.choiceText = "Goodbye";
        endChoice.choiceType = Choice.TypeOfChoice.EndDialogue;

        List<Choice> newChoices = new List<Choice>();
        newChoices.Add(endChoice);
        DialogueSystem.instance.ContinueDialogue(mychoice.answerSubtitle, newChoices);
    }
}

[System.Serializable]
public class Choice
{
    public enum TypeOfChoice { Dialogue, GiveItem, RequestItem, EndDialogue}
    public TypeOfChoice choiceType;
    public string choiceText;
    public GameObject rewardObject;
    public string answerSubtitle;
}
