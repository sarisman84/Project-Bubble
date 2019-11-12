using UnityEngine;

// Erik Neuhofer
public class OnOffSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject[] objectsToTurnOf = null; // Set objects to be affected in Inspector
    private string whatToSay = "Interact";       

    public bool CanBeInteractedWith()
    {
        if (objectsToTurnOf == null)
        {
            Debug.LogWarning("No references made in Inspector!");
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool InteractWith()
    {
        if (CanBeInteractedWith() == true)
        {
            foreach (GameObject obj in objectsToTurnOf)
            {
                obj.SetActive(!obj.activeSelf);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public string MessageOnDetection()
    {
        return whatToSay;
    }

    public void EndInteration() { }
}