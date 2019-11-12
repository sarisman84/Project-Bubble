using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Simon Voss
public class DialogueChoice : MonoBehaviour
{
    public Choice mychoice;

    public void UseChoice()
    {
        Debug.Log("Choice was used: " + mychoice.choiceText);
        switch (mychoice.choiceType)
        {
            case Choice.TypeOfChoice.Dialogue:
                Choice endChoice = new Choice();
                endChoice.choiceText = "Goodbye";
                endChoice.choiceType = Choice.TypeOfChoice.EndDialogue;
                List<Choice> newChoices = new List<Choice>();
                newChoices.Add(endChoice);
                DialogueSystem.instance.ContinueDialogue(mychoice.answerSubtitle, newChoices);
                break;
            case Choice.TypeOfChoice.GiveItem:
                Inventory.instance.AddItemToInventory(mychoice.rewardItemID);
                endChoice = new Choice();
                endChoice.choiceText = "Goodbye";
                endChoice.choiceType = Choice.TypeOfChoice.EndDialogue;
                newChoices = new List<Choice>();
                newChoices.Add(endChoice);
                DialogueSystem.instance.ContinueDialogue(mychoice.answerSubtitle, newChoices);
                break;
            case Choice.TypeOfChoice.RequestItem:
                break;
            case Choice.TypeOfChoice.EndDialogue:
                DialogueSystem.instance.EndDialogue();
                break;
        }

        

        
    }
}

[System.Serializable]
public class Choice
{
    public enum TypeOfChoice { Dialogue, GiveItem, RequestItem, EndDialogue}
    public TypeOfChoice choiceType;
    public string choiceText;
    public int rewardItemID;
    public string answerSubtitle;
}
