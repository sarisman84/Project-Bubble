using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Simon Voss
public class DialogueChoiceHolder : MonoBehaviour
{
    public Choice mychoice;

    public void UseChoice()
    {
        //Debug.Log("Choice was used: " + mychoice.choiceText);
        DialogueSystem.instance.UseChoice(mychoice);
    }
}