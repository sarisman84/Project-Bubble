using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDoor : MonoBehaviour, IInteractable
{
    public Lock doorLock;

    private Animator animator;
    private bool doorIsOpen;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool InteractWith()
    {
        if (doorLock.unlocked && !doorIsOpen)
        {
            animator.SetBool("open", true);
            doorIsOpen = true;
        }
        else if (doorIsOpen)
        {
            animator.SetBool("open", false);
            doorIsOpen = false;
        }
        return false;
    }

    public string MessageOnDetection()
    {
        return "Open Door";
    }

    public bool CanBeInteractedWith()
    {
        return true;
    }

    public void EndInteration()
    {
    }
}
