using UnityEngine;

public class GuardDetection : MonoBehaviour //Dejan, this script should be used on the object containing guard's trigger colliders
{
    [SerializeField] Guard parentGuard = null; //a reference to the guard

    private void OnTriggerStay(Collider other) //passes a reference to the player and informs the guard of the players detection
    {
        parentGuard.player = other.gameObject;
        parentGuard.playerIsDetected = true;
    }

    private void OnTriggerExit(Collider other) //informs the guard the player is no longer detectable
    {
        parentGuard.player = null;
        parentGuard.playerIsDetected = false;
    }
}
