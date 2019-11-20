using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepCheck : MonoBehaviour
{
    [SerializeField]
    public GameObject gilad;
    [SerializeField]
    public GameObject buttons;
    bool canISleep = false;

    void Update()
    {
        //if () //INSERT "IS QUEST COMPLETE" HERE TO MAKE canISleep TRUE!
        //{
        //    canISleep = true;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canISleep == true && other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance().SetFPSInput(false);
            Cursor.lockState = CursorLockMode.None;
            EnableButtons();
        }
    }

    //public bool CanISleep()
    //{
    //    for (int i = 0; i < QuestLog.Instance().quests.Count; i++)
    //    {
    //        if ()
    //    }
    //}

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
