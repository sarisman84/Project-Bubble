using System.Collections.Generic;
using UnityEngine;

//Simon Voss
public class PlayerControls : MonoBehaviour
{
    [SerializeField] MessageUI messageUI = null;
    [SerializeField] Transform fpsCamera = null;

    [SerializeField] float interactableOutlineWidth = 0.02f;

    private GameObject highLightObject;

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
            
            //Highlight
            if (highLightObject.GetComponent<Renderer>() != null)
            {
                highLightObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_FirstOutlineWidth", interactableOutlineWidth);
            }
            else
            {
                highLightObject.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_FirstOutlineWidth", interactableOutlineWidth);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                //Interact with the last added interactable in the list
                interactable.InteractWith();
            }
        }
        else
        {   //Tar bort highlight
            if (highLightObject != null)
            {
                if (highLightObject.GetComponent<Renderer>() != null)
                {
                    highLightObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_FirstOutlineWidth", 0f);
                }
                else
                {
                    highLightObject.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_FirstOutlineWidth", 0f);
                }
            }
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
                highLightObject = hit.transform.gameObject;
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
