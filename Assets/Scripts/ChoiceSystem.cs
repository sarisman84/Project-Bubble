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
    [SerializeField] Text[] choiceButtonTexts = null;

    private void Start()
    {
        currentEvent = scenario.startEvent;
        background.sprite = currentEvent.image;
        description.text = currentEvent.description;
        for (int i = 0; i < currentEvent.choices.Count; i++)
        {
            choiceButtonTexts[i].text = currentEvent.choices[i].choiceText;
        }
    }

    public void MakeChoice(int index)
    {
        currentEvent = currentEvent.choices[index].nextEvent;
        background.sprite = currentEvent.image;
        description.text = currentEvent.description;
        for (int i = 0; i < currentEvent.choices.Count; i++)
        {
            choiceButtonTexts[i].text = currentEvent.choices[i].choiceText;
        }
    }
}
