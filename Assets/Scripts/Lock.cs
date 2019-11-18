﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lock : MonoBehaviour, IInteractable
{
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
            EndInteration();
            unlocked = true;
            passwordPanelOpen = false;
            InteractWith();
        }
        else if (passwordPanelOpen && Input.GetKeyDown(KeyCode.Return))
        {
            passwordPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            EndInteration();
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
                int children = Inventory.instance.itemSpaceTransform.childCount;
                for (int i = 0; i < children; i++)
                {
                    if (Inventory.instance.itemSpaceTransform.GetChild(i).GetComponent<InventoryItem>().itemID == keyItem)
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
                    GameManager.Instance().SetFPSInput(false);
                    GameManager.Instance().SetMouseLook(false);
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

    public void EndInteration()
    {
        GameManager.Instance().SetFPSInput(true);
        GameManager.Instance().SetMouseLook(true);
    }
}

public enum LockType { key, password }
