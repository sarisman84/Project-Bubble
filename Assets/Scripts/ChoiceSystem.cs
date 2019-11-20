using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChoiceSystem : MonoBehaviour
{
    [SerializeField] Scenario scenario = null;
    [SerializeField] Event currentEvent = null;
    [SerializeField] Image background = null;
    [SerializeField] Text description = null;
    [SerializeField] GameObject[] choiceButtons = null;
    [SerializeField] Text[] choiceButtonTexts = null;

    private void Start()
    {
        if (!Inventory.instance)
        {
            Debug.LogError("Inventory is missing");
        }
        if (!PlayerCharacteristics.instance)
        {
            Debug.LogError("Playercharacteristics is missing");
        }
        currentEvent = scenario.startEvent;
        DisplayGUI(currentEvent);
    }

    public void MakeChoice(int index)
    {
        Choice usedChoice = currentEvent.choices[index];
        if (currentEvent.choices[index].nextEvent.choices.Count > 0)
        {
            Debug.Log("Changing event");
            currentEvent = currentEvent.choices[index].nextEvent;
        }
        else if (currentEvent.choices[index].nextScenario != null)
        {
            Debug.Log("Changing scenario");
            scenario = currentEvent.choices[index].nextScenario;
            currentEvent = scenario.startEvent;
        }
        else if (currentEvent.choices[index].nextScene != null)
        {
            Debug.Log("Changing scene");
            SceneManager.LoadScene(currentEvent.choices[index].nextScene.name);
        }
        else
        {
            Debug.LogError("No further events or endnodes found, check setup of scenario from node: " + currentEvent.title);
        }
        AffectPlayer(usedChoice);
        DisplayGUI(currentEvent);
    }

    private void AffectPlayer(Choice usedChoice)
    {
        PlayerCharacteristics.instance.IncreaseStat(usedChoice.skillType);

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
                Inventory.instance.AddItemToInventory(usedChoice.itemID);
                break;
            case ItemTransfer.PlayerLoseItem:
                Inventory.instance.RemoveItemFromInventory(usedChoice.itemID);
                break;
        }
    }

    private void DisplayGUI(Event input)
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }
        for (int i = 0; i < input.choices.Count; i++)
        {
            DrawChoice(input.choices[i], i);
        }
        background.sprite = currentEvent.image;
        description.text = currentEvent.description;
    }

    private void DrawChoice(Choice choice, int choiceIndex)
    {
        bool isAllowed = true;
        string fault = "";
        if (choice.itemtransfer == ItemTransfer.PlayerLoseItem && !Inventory.instance.CheckIfItemIsInInventory(choice.itemID))
        {
            fault = " - you don't have the item";
            isAllowed = false;
        }

        if (choice.affectedNPC != null && (int)choice.minimumRelationshiplevel > (int)choice.affectedNPC.CalculateAndCheckRelationshipLevel())
        {
            fault = " - you don't know the person well enough";
            isAllowed = false;
        }
        if (choice.requiredSkill != Characteristics.None && choice.requiredSkillNumber > PlayerCharacteristics.instance.GetCharacteristic(choice.requiredSkill))
        {
            fault = " - you don't have the required skill";
            isAllowed = false;
        }

        if (isAllowed)
        {
            choiceButtons[choiceIndex].GetComponent<Button>().interactable = true;
            choiceButtons[choiceIndex].SetActive(true);
            choiceButtonTexts[choiceIndex].text = choice.choiceText;
        }
        else
        {
            choiceButtons[choiceIndex].GetComponent<Button>().interactable = false;
            choiceButtons[choiceIndex].SetActive(true);
            choiceButtonTexts[choiceIndex].text = choice.choiceText + fault;
        }
    }
}
