using System.Collections.Generic;
using UnityEngine;

//Simon Voss
public class PlayerControls : MonoBehaviour
{
    List<IInteractable> interactables = new List<IInteractable>();
    public int numberOfInteractables = 0;
    [SerializeField] MessageUI messageUI;
    [SerializeField] Transform fpsCamera;

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
        //numberOfInteractables = interactables.Count;
        ////Display text from the last encountered interactable
        //if (interactables.Count > 0 && messageUI)
        //{
        //    messageUI.DisplayText(interactables[interactables.Count - 1].MessageOnDetection());
        //}
        //else if (messageUI)
        //{
        //    //Disable the message panel
        //    messageUI.DisablePanel();
        //}

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    //Interact with the last added interactable in the list
        //    if (interactables.Count > 0)
        //    {
        //        if (interactables[interactables.Count - 1].InteractWith())
        //        {
        //            interactables.RemoveAt(interactables.Count - 1);
        //        }
        //    }
        //}

        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 10, Color.red);
        IInteractable interactable = DetectInteractable();
        if (interactable != null)
        {
            messageUI.DisplayText(interactable.MessageOnDetection());
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Interact with the last added interactable in the list
                interactable.InteractWith();
            }
        }
        else
        {
            messageUI.DisablePanel();
        }
    }

    [SerializeField] float raycastThickness = 0.25f;
    [SerializeField] float raycastDetectionRange = 3;
    private IInteractable DetectInteractable()
    {
        RaycastHit hit;
        IInteractable interactable = null;

        if (Physics.SphereCast(fpsCamera.transform.position, raycastThickness, fpsCamera.transform.forward, out hit, raycastDetectionRange))
        {
            if (hit.transform.GetComponent<IInteractable>() != null)
            {
                interactable = hit.transform.GetComponent<IInteractable>();
                Debug.DrawRay(fpsCamera.transform.position, hit.point - transform.position, Color.green);
                Debug.Log("Did Hit");

                if (!interactable.CanBeInteractedWith())
                {
                    interactable = null;
                }
            }
        }
        return interactable;
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
