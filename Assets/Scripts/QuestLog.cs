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
    [SerializeField] TextMeshProUGUI questAdded = null; //a reference to the canvas text showing added or finished quests
    public List<QuestInstance> quests = new List<QuestInstance>(); //a list of all the quests

    private void Start()
    {
        questAdded.CrossFadeAlpha(0, 0, true); //sets the quests added/finished text to transparent
    }

    public void ActivateQuest(int questID) //activates quest with given questID and updates GUI
    {
        foreach (QuestInstance quest in quests)
        {
            if (quest.questID == questID)
            {
                if (!quest.started)
                {
                    questAdded.text = $"Quest Added: {quest.questName}";
                    StartCoroutine("FadeInText", questAdded);
                }
                quest.started = true;
            }
        }
    }

    public void EndQuest(int questID) //ends quest with given quest ID and updates GUI
    {
        foreach (QuestInstance quest in quests)
        {
            if (quest.questID == questID)
            {
                if (!quest.ended)
                {
                    questAdded.text = $"Quest Finished: {quest.questName}";
                    StartCoroutine("FadeInText", questAdded);
                }
                quest.ended = true;
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

    IEnumerator FadeInText(TextMeshProUGUI textMeshProUGUI) //fades in/fades out quest added/finished text
    {
        textMeshProUGUI.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSeconds(3);
        textMeshProUGUI.CrossFadeAlpha(0, 1, true);
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
