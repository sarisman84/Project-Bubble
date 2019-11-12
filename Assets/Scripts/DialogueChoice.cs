using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Simon Voss
public class DialogueChoice : MonoBehaviour
{
    public Choice mychoice;

    public void UseChoice()
    {
        //Debug.Log("Choice was used: " + mychoice.choiceText);
        DialogueSystem.instance.UseChoice(mychoice);
    }
}

public enum Characteristics { Neutral, Charming, Intimidation, Logical }
[System.Serializable]
public class Choice
{
    public enum TypeOfChoice { Dialogue, GiveItem, TakeItem, EndDialogue }

    public Characteristics choiceStyle;
    public TypeOfChoice choiceType;
    public string choiceText;
    public int rewardItemID;
    public int requestItemID;
    public string answerSubtitle;

    public List<Choice> newChoices;
}
