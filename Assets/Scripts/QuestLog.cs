using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestLog : MonoBehaviour //Dejan, this script keeps track of quests and updates the GUI Objectives.cs
{
    #region
    private static QuestLog instance;
    public static QuestLog Instance()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load("Quest Log") as GameObject).GetComponent<QuestLog>();
        }
        return instance;
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    public Objectives objectivesWindow; //a reference to the Objectives.cs
    [SerializeField] TextMeshProUGUI questDisplay = null; //a reference to the canvas text showing added or finished quests
    public List<QuestInstance> quests = new List<QuestInstance>(); //a list of all the quests

    List<string> questsToDisplay = new List<string>(); //contains all string messages to be displayed
    bool coroutineIsRunning; //is coroutine FadeInText running?

    private void Start()
    {
        questDisplay.CrossFadeAlpha(0, 0, true); //sets the quests added/finished text to transparent

        if (quests.Count > 0) //displays text for any quests that auto-started on load
        {
            foreach (QuestInstance quest in quests)
            {
                if (quest.started)
                {
                    questsToDisplay.Add($"Quest Added: {quest.questName}");
                    if (!coroutineIsRunning)
                    {
                        StartCoroutine("FadeInText");
                    }
                    UpdateObjectives();
                }
            }
        }
    }

    public void ActivateQuest(int questID) //activates quest with given questID and updates GUI
    {
        foreach (QuestInstance quest in quests)
        {
            if (quest.questID == questID && !quest.started)
            {
                quest.started = true;
                questsToDisplay.Add($"Quest Added: {quest.questName}");
                if (!coroutineIsRunning)
                {
                    StartCoroutine("FadeInText");
                }
                UpdateObjectives();
            }
        }
    }

    public void EndQuest(int questID) //ends quest with given quest ID and updates GUI
    {
        foreach (QuestInstance quest in quests)
        {
            if (quest.questID == questID && quest.started && !quest.ended)
            {
                quest.ended = true;
                questsToDisplay.Add($"Quest Finished: {quest.questName}");
                if (!coroutineIsRunning)
                {
                    StartCoroutine("FadeInText");
                }
                UpdateObjectives();
            }
        }
    }

    public QuestInstance QuestWithID(int questID) //returns QuestInstance with given quest ID
    {
        QuestInstance questToReturn = null;
        foreach (QuestInstance quest in quests)
        {
            if (quest.questID == questID)
            {
                questToReturn = quest;
            }
        }
        return questToReturn;
    }

    public void UpdateObjectives() //updates Objectives.cs
    {
        if (objectivesWindow != null)
        {
            objectivesWindow.PrintCurrentObjective();
        }
    }

    IEnumerator FadeInText() //fades in/fades out quest added/finished texts
    {
        if (questsToDisplay.Count > 0)
        {
            coroutineIsRunning = true;
            questDisplay.text = questsToDisplay[0];
            questsToDisplay.Remove(questsToDisplay[0]);
            questDisplay.CrossFadeAlpha(1, 1, true);
            yield return new WaitForSeconds(3);
            questDisplay.CrossFadeAlpha(0, 1, true);
            yield return new WaitForSeconds(1);
            StartCoroutine("FadeInText");
        }
        else
        {
            coroutineIsRunning = false;
        }
    }
}

[System.Serializable]
public class QuestInstance //holds all nececcary information for a quest
{
    public int questID;
    public string questName;
    [TextAreaAttribute(3, 3)] public string questDescription;
    public bool started;
    public bool ended;
}
