using System.Collections.Generic;
using UnityEngine;

//Simon Voss
public class PlayerControls : MonoBehaviour
{
    [SerializeField] MessageUI messageUI = null;
    [SerializeField] Transform fpsCamera = null;

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
        IInteractable interactable = DetectInteractable();
        if (interactable != null && interactable.CanBeInteractedWith())
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
                Debug.DrawRay(fpsCamera.transform.position, hit.transform.position - transform.position, Color.green);
                //Debug.Log("Did Hit:" + interactable.ToString());
            }
        }
        else
        {
            Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * raycastDetectionRange, Color.red);
        }

        return interactable;
    }
}
