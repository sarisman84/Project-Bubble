using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTriggers : MonoBehaviour
{
    QuestLog questLog;

    [SerializeField] int questID = 0;
    [SerializeField] bool start = false;

    private void Start()
    {
        questLog = QuestLog.Instance();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (start)
        {
            questLog.ActivateQuest(questID);
            questLog.UpdateObjectives();
        }
        else
        {
            questLog.EndQuest(questID);
            questLog.UpdateObjectives();
        }
    }
}
