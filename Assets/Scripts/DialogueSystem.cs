using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueSystem : MonoBehaviour
{
    #region Singleton
    public static DialogueSystem instance;
    private void Awake()
    {
        if (DialogueSystem.instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Another instance of: " + this + " , was tried to be instantiated, but was destroyed! This instance was tried to be instantiated on: " + this.gameObject);
            Destroy(this);
        }
    }
    #endregion



    GameObject[] choiceButtons;
    Text[] choiceTexts;
    [SerializeField] GameObject choicePanel;
    [SerializeField] GameObject subtitlesPanel;
    Text subtitleText;

    private void Start()
    {

        choiceButtons = new GameObject[choicePanel.transform.childCount];
        choiceTexts = new Text[choicePanel.transform.childCount];

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i] = choicePanel.transform.GetChild(i).gameObject;
            choiceTexts[i] = choicePanel.transform.GetChild(i).GetComponentInChildren<Text>();
        }
        subtitleText = subtitlesPanel.GetComponentInChildren<Text>();

        EndDialogue();
    }

    public void StartDialogue(string greeting, List<Choice> choices)
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        choicePanel.SetActive(true);
        subtitlesPanel.SetActive(true);
        if (choices != null)
        {
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                choiceButtons[i].SetActive(false);
            }

            for (int i = 0; i < choices.Count; i++)
            {
                choiceButtons[i].SetActive(true);
                choiceTexts[i].text = choices[i].choiceText;
                choiceButtons[i].transform.GetComponent<DialogueChoice>().mychoice = choices[i];
            }
        }
        subtitleText.text = greeting;

        
    }

    public void ContinueDialogue(string answer, List<Choice> furtherChoices)
    {
        if (furtherChoices != null)
        {
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                choiceButtons[i].SetActive(false);
            }

            for (int i = 0; i < furtherChoices.Count; i++)
            {
                choiceButtons[i].SetActive(true);
                choiceTexts[i].text = furtherChoices[i].choiceText;
                choiceButtons[i].transform.GetComponent<DialogueChoice>().mychoice = furtherChoices[i];
            }
        }
        subtitleText.text = answer;
    }

    public void EndDialogue()
    {
        choicePanel.SetActive(false);
        subtitlesPanel.SetActive(false);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
