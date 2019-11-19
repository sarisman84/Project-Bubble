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
        currentEvent = scenario.startEvent;
        DisplayGUI(currentEvent);
    }

    public void MakeChoice(int index)
    {
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
        DisplayGUI(currentEvent);
    }

    private void DisplayGUI(Event input)
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }
        for (int i = 0; i < input.choices.Count; i++)
        {
            choiceButtons[i].SetActive(true);
            choiceButtonTexts[i].text = currentEvent.choices[i].choiceText;
        }
        background.sprite = currentEvent.image;
        description.text = currentEvent.description;
    }
}
