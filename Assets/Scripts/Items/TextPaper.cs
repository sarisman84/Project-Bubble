using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPaper : MonoBehaviour, IInteractable //Dejan, an example script for a "document" object
{
    [SerializeField] GameObject document = null; //a reference to the canvas element dokument
    [SerializeField] MeshRenderer meshRenderer = null; //a reference to the mesh renderer
    [SerializeField] Animator animator = null; //a reference to the animator

    public int itemID; //unique item id

    public bool InteractWith() //executes when interacted, shows canvas element
    {
        StartCoroutine("WaitForShowAnimation");
        return false;
    }

    public string MessageOnDetection() //displays message on hover
    {
        return "Text Paper";
    }

    public bool CanBeInteractedWith() //can always be interacted with
    {
        return true;
    }
    public void EndInteration() { } //not used

    public void HideDocument() //hides canvas element
    {
        StartCoroutine("WaitForHideAnimation");
    }

    IEnumerator WaitForHideAnimation() //used to execute hide animation correctly
    {
        animator.SetBool("show", false);
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance().SetFPSInput(true);
        GameManager.Instance().SetMouseLook(true);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
    IEnumerator WaitForShowAnimation()
    {
        document.SetActive(true); //used to execute show animation correctly
        animator.SetBool("show", true);
        meshRenderer.enabled = false;
        GameManager.Instance().SetFPSInput(false);
        GameManager.Instance().SetMouseLook(false);
        Inventory.instance.AddItemToInventory(1);
        yield return new WaitForSeconds(1);
        Cursor.lockState = CursorLockMode.None;
    }
}
