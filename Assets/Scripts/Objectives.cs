using TMPro;
using UnityEngine;

// Erik Neuhofer, Dejan Savic
public class Objectives : MonoBehaviour
{
    QuestLog questLog;

    [SerializeField] TextMeshProUGUI objectiveTextField;

    int currentIndex = 0;

    private void Start()
    {
        questLog = QuestLog.Instance();
        questLog.objectivesWindow = this;
        PrintCurrentObjective();
    }

    public void PrintCurrentObjective()
    {
        objectiveTextField.text = "";
        foreach (QuestInstance quest in questLog.quests)
        {
            if (quest.started && !quest.ended)
            {
                Debug.Log("Quest Found");
                objectiveTextField.text = $"{objectiveTextField.text}{quest.questName.ToUpper()}:\n{quest.questDescription}\n";
            }
        }
    }
}