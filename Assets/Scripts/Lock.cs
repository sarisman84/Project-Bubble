using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lock : MonoBehaviour, IInteractable //Dejan, updates unlocked boolean according to either a password or key item
{
    public GameObject passwordPanel; //a reference to the canvas element password panel
    public TMP_InputField passwordInputField; //a reference to the canvas element input field
    public LockType lockType; //a reference to enum LockType ( key, password)
    public int keyItem; //item ID that unlocks the lock
    public int password; //password that unlockd the door
    public bool unlocked; //check this boolean for locks status

    private bool passwordPanelOpen; //keeps track of the password panel canvas element, if its active or not

    void Update()
    {
        if (passwordPanelOpen && passwordInputField.text == password.ToString() && Input.GetKeyDown(KeyCode.Return)) //closes/open password panel, locks/unlocks cursor. if code is correct unlocks the lock
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

    public void OpenPasswordPannel() //enables password panel canvas element
    {
        passwordPanel.SetActive(true);
        passwordPanelOpen = true;
    }
    public void ClosePasswordPannel() //disables password panel canvas element
    {
        passwordPanel.SetActive(false);
        passwordPanelOpen = false;
    }

    public bool InteractWith() //if LockType = key and inventory contains correct item, opens door. if LockType password, opens password panel
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

    public string MessageOnDetection() //displays message on crosshair hover
    {
        return "Unlock!";
    }

    public bool CanBeInteractedWith() //if the lock can be interacted with
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

    public void EndInteration() //reenables players controls
    {
        GameManager.Instance().SetFPSInput(true);
        GameManager.Instance().SetMouseLook(true);
    }
}

public enum LockType { key, password } //enum holding different types of locks
