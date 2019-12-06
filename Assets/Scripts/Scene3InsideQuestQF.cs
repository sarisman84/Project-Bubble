using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3InsideQuestQF : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance().USBPickedUp)
        {
            QuestLog.Instance().ActivateQuest(1);
        }
        else
        {
            QuestLog.Instance().ActivateQuest(0);
        }
    }
}
