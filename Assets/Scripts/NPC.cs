using UnityEngine;

//Simon Voss
public class NPC : MonoBehaviour , IInteractable
{
    [SerializeField] bool willingToTalk = true;

    public NPC TryStartTalking()
    {
        if (willingToTalk)
        {
            Debug.Log("NPC talked with");
            return this;
        }
        else
        {
            Debug.Log("This NPC does not want to talk");
            return null;
        }
    }

    public void InteractWith()
    {
        TryStartTalking();
    }

    public string MessageOnDetection()
    {
        return "Click E To Talk";
    }
}
