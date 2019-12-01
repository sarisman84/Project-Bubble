using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitchOnTriggerEnter : MonoBehaviour
{
    [SerializeField] int sceneToLoadIndex = 0;
    [SerializeField] int ifQuestIDFinished = 0;
    [SerializeField] SceneSwitch sceneSwitch = null;

    private void OnTriggerStay(Collider other)
    {
        if (QuestLog.Instance().QuestWithID(ifQuestIDFinished).ended)
        {
            sceneSwitch.SwitchScene(sceneToLoadIndex);
        }
    }
}
