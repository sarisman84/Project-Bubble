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
    [SerializeField]
    public GameObject goToWorkButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && hasGottenChoice == false)
        {
            hasGottenChoice = true;
            GameManager.Instance().SetFPSInput(false);
            Cursor.lockState = CursorLockMode.None;
            EnableButtons();
        }
        else if (other.gameObject.CompareTag("Player") && hasGottenChoice == true)
        {
            hasGottenChoice = true;
            GameManager.Instance().SetFPSInput(false);
            Cursor.lockState = CursorLockMode.None;
            Destroy(goToWorkButton);
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