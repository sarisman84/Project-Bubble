using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lock : MonoBehaviour, IInteractable
{
    public Invetory inventory;
    public GameObject passwordPanel;
    public TMP_InputField passwordInputField;
    public LockType lockType;
    public int keyItem;
    public int password;
    public bool unlocked;

    private bool passwordPanelOpen;

    void Update()
    {
        if (passwordPanelOpen && passwordInputField.text == password.ToString() && Input.GetKeyDown(KeyCode.Return))
        {
            passwordPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            unlocked = true;
            passwordPanelOpen = false;
            InteractWith();
        }
        else if (passwordPanelOpen && Input.GetKeyDown(KeyCode.Return))
        {
            passwordPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            passwordPanelOpen = false;
        }
    }

    public void OpenPasswordPannel()
    {
        passwordPanel.SetActive(true);
        passwordPanelOpen = true;
    }
    public void ClosePasswordPannel()
    {
        passwordPanel.SetActive(false);
        passwordPanelOpen = false;
    }

    public bool InteractWith()
    {
        switch (lockType)
        {
            case LockType.key:
                int children = inventory.itemSpaceTransform.childCount;
                for (int i = 0; i < children; i++)
                {
                    if (inventory.itemSpaceTransform.GetChild(i).GetComponent<InventoryItem>().itemID == keyItem)
                    {
                        unlocked = true;
                        return true;
                    }
                }
                return false;
            case LockType.password:
                if (unlocked)
                {
                    return true;
                }
                else
                {
                    passwordPanel.SetActive(true);
                    passwordPanelOpen = true;
                    Cursor.lockState = CursorLockMode.None;
                    Time.timeScale = 0;
                    return false;
                }
            default:
                return false;
        }
    }

    public string MessageOnDetection()
    {
        return "Unlock!";
    }

    public bool CanBeInteractedWith()
    {
        if (unlocked)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

public enum LockType { key, password }
