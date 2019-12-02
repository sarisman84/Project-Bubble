using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//Simon Vos - Handles the 2d choice system logic
public class ChoiceSystem : MonoBehaviour
{
    [SerializeField] Scenario scenario = null;
    [SerializeField] Event currentEvent = null;
    [SerializeField] TextMeshProUGUI locationText = null;
    [SerializeField] Image background = null;
    [SerializeField] Text description = null;
    [SerializeField] GameObject[] choiceButtons = null;
    [SerializeField] Text[] choiceButtonTexts = null;
    [SerializeField] PlayerCharacteristics playerStats = null;
    [SerializeField] ScriptableInventory inventory = null;

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
        if (scenario.startEvent != null)
        {
            currentEvent = scenario.startEvent;
            DisplayGUI(currentEvent);
        }
        else
        {
            Debug.LogError("No event set in inspector of Choice System as start");
        }
    }



    public void MakeChoice(int index)
    {
        Choice usedChoice = currentEvent.choices[index];
        //if (currentEvent.choices[index].nextEvent.choices.Count > 0)
        //{
        //    Debug.Log("Changing event");
        //    currentEvent = currentEvent.choices[index].nextEvent;
        //}
        Event newEvent = scenario.FindNextEvent(usedChoice);
        if (newEvent != null && newEvent.choices.Count > 0)
        {
            //Debug.Log("Changing event");
            currentEvent = newEvent;
        }
        else if (currentEvent.choices[index].nextScenario != null)
        {
            Debug.Log("Changing scenario");
            scenario = currentEvent.choices[index].nextScenario;
            currentEvent = scenario.startEvent;
        }
        //else if (currentEvent.choices[index].nextSceneIndex != null)
        //{
        //    Debug.Log("Changing scene");
        //    SceneManager.LoadScene(currentEvent.choices[index].nextSceneIndex.name);
        //}
        else if (currentEvent.choices[index].nextScene != "")
        {
            SceneManager.LoadScene(currentEvent.choices[index].nextScene);
        }
        else
        {
            Debug.LogError("No further events or endnodes found, check setup of scenario from node: " + currentEvent.locationText);
            return;
        }
        AffectPlayer(usedChoice);
        DisplayGUI(currentEvent);
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
        if (input.locationText != "Location Text")
        {
            locationText.text = input.locationText;
        }
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }
        for (int i = 0; i < input.choices.Count; i++)
        {
            DrawChoice(input.choices[i], i);
        }
        if (currentEvent.image)
        {
            background.sprite = currentEvent.image;
        }
        description.text = currentEvent.description;
    }

    private void DrawChoice(Choice choice, int choiceIndex)
    {
        bool isAllowed = true;
        //string fault = "";
        if (choice.itemtransfer == ItemTransfer.PlayerLoseItem && !inventory.CheckIfItemIsInInventory(choice.item))
        {
            //fault = " - you don't have the item";
            isAllowed = false;
        }

        if (choice.affectedNPC != null && (int)choice.minimumRelationshiplevel > (int)choice.affectedNPC.CalculateAndCheckRelationshipLevel())
        {
            // fault = " - you don't know the person well enough";
            isAllowed = false;
        }
        if (choice.requiredSkill != Characteristics.None && choice.requiredSkillNumber > playerStats.GetCharacteristic(choice.requiredSkill))
        {
            //fault = " - you don't have the required skill";
            isAllowed = false;
        }

        if (choice.requiredStartedQuest != null)
        {
            if (!playerStats.activeQuests.Contains(choice.requiredStartedQuest))
            {
                //fault = " - you have not started the needed quest";
                isAllowed = false;
            }
        }

        if (choice.requiredCompletedQuest != null)
        {
            if (!playerStats.completedQuests.Contains(choice.requiredCompletedQuest))
            {
                //fault = " - you have not completed the needed quest";
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
}
