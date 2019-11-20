using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepCheck : MonoBehaviour
{
    [SerializeField]
    public GameObject buttons;
    bool canISleep = false;

    private void Update()
    {
        Debug.Log("canISleep = " + canISleep);
    }

    private void OnTriggerEnter(Collider other)
    {
        CanISleep();
        if (canISleep == true && other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance().SetFPSInput(false);
            Cursor.lockState = CursorLockMode.None;
            EnableButtons();
        }
    }

    public bool CanISleep()
    {
        //for (int i = 0; i < QuestLog.Instance().quests.Count; i++)
        //{
        //    if (QuestLog.Instance().quests[i].questID == 0 && QuestLog.Instance().quests[i].ended)
        //    {
        //        canISleep = true;
        //    }
        //}

        foreach (QuestInstance quest in QuestLog.Instance().quests)
        {
            if (quest.questID == 0 && quest.ended)
            {
                canISleep = true;
                return canISleep;
            }
        }
        return canISleep;
    }

    public void EnableButtons()
    {
        buttons.SetActive(true);
    }

    public void DisableButtons()
    {
        buttons.SetActive(false);
        GameManager.Instance().SetFPSInput(true);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
