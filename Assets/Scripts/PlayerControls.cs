using System.Collections.Generic;
using UnityEngine;

//Simon Voss
public class PlayerControls : MonoBehaviour
{
    List<IInteractable> interactables = new List<IInteractable>();
    [SerializeField] MessageUI messageUI;

    private void Start()
    {
        if (!messageUI)
        {
            Debug.LogWarning("Message UI is not attached, we cannot display the interactable objects message without it, plz fix");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Display text from the last encountered interactable
        if (interactables.Count > 0 && messageUI)
        {
            messageUI.DisplayText(interactables[interactables.Count - 1].MessageOnDetection());
        }
        else if (messageUI)
        {
            //Disable the message panel
            messageUI.DisablePanel();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //Interact with the last added interactable in the list
            if (interactables.Count > 0)
            {
                if(interactables[interactables.Count - 1].InteractWith())
                {
                    interactables.RemoveAt(interactables.Count -1);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Add interactable to the list
        if (other.gameObject.GetComponent<IInteractable>() != null)
        {
            if (!interactables.Contains(other.gameObject.GetComponent<IInteractable>()))
            {
                interactables.Add(other.gameObject.GetComponent<IInteractable>());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Remove interactable from the list
        if (other.gameObject.GetComponent<IInteractable>() != null)
        {
            interactables.Remove(other.gameObject.GetComponent<IInteractable>());
        }
    }
}
