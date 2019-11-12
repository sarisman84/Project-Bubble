using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Simon Voss
public class DialogueChoiceHolder : MonoBehaviour
{
    public DialogueChoice mychoice;

    public void UseChoice()
    {
        //Debug.Log("Choice was used: " + mychoice.choiceText);
        DialogueSystem.instance.UseChoice(mychoice);
    }
}

public enum Characteristics { Neutral, Charming, Intimidation, Logical }
[System.Serializable]
public class DialogueChoice
{
    public enum TypeOfChoice { Dialogue, GiveItem, TakeItem, EndDialogue }

    public Characteristics choiceStyle;
    public TypeOfChoice choiceType;
    public string choiceText;
    public int rewardItemID;
    public int requestItemID;
    public string answerSubtitle;

    public bool isExpandable = false;
    public bool isExpended { get; set; }
    public int completeDialogueQuestID = 0;

    public List<DialogueChoice> newChoices;
}

[System.Serializable]
public class DialogueQuest
{
    public int id;
}
