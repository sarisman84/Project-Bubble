using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestLog : MonoBehaviour
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

    public Objectives objectivesWindow;
    [SerializeField] TextMeshProUGUI questAdded;
    public List<QuestInstance> quests = new List<QuestInstance>();

    private void Start()
    {
        questAdded.CrossFadeAlpha(0, 0, true);
    }

    public void ActivateQuest(int questID)
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

    public void EndQuest(int questID)
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

    public QuestInstance QuestWithID(int questID)
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

    public void UpdateObjectives()
    {
        if (objectivesWindow != null)
        {
            objectivesWindow.PrintCurrentObjective();
        }
    }

    IEnumerator FadeInText(TextMeshProUGUI textMeshProUGUI)
    {
        textMeshProUGUI.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSeconds(3);
        textMeshProUGUI.CrossFadeAlpha(0, 1, true);
    }
}



[System.Serializable]
public class QuestInstance
{
    public int questID;
    public string questName;
    [TextAreaAttribute(3, 3)] public string questDescription;
    public bool started;
    public bool ended;
}
