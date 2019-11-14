using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
        currentEvent = currentEvent.choices[index].nextEvent;
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
