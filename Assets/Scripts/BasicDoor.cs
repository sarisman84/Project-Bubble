using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDoor : MonoBehaviour, IInteractable //Dejan, animates (opens and closes) the door based on if the lock is unlocked (must have a lock)
{
    public Lock doorLock; //a reference to the lock object

    private Animator animator; //a reference to the animator
    private bool doorIsOpen; //weather the door is open or not

    void Start()
    {
        animator = GetComponent<Animator>(); //get the animator
    }

    public bool InteractWith() //opens and close the door
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

    public string MessageOnDetection() //displays message
    {
        return "Open Door";
    }

    public bool CanBeInteractedWith() //the door can always be interacted with
    {
        return true;
    }

    public void EndInteration() //not needed since the door can always be interacted with
    {
    }
}
