using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

//Gabriel

public class DoorChoice : MonoBehaviour
{
    [SerializeField]
    public GameObject buttons;
    private bool hasGottenChoice = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && hasGottenChoice == false)
        {
            hasGottenChoice = true;
            GameManager.Instance().SetFPSInput(false);
            Cursor.lockState = CursorLockMode.None;
            EnableButtons();
        }
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