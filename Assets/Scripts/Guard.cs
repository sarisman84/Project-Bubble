using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour //Dejan, this is ment to be used on guards in order to handle detection and act on it. atlist one GuardDetection needed
{
    public GameObject player; //a reference to the player within GuardDetection's trigger area
    public bool playerIsDetected; //true if there is a player withi GuardDetection's trigger area

    RaycastHit rayHit;

    float nextTime = 0; //next time detection can increase/decrease
    public float currentDetectionLevel = 0; //current detection level
    [SerializeField] float interval = 1; //interval at which detection can rise/fall
    [SerializeField] float detectionPerInterval = 1;
    [SerializeField] int detectionLevel = 2; //detection level at which player will be uncovered
    public bool playerdUncovered; //indicates the player has been uncovered
    public bool playerIsVisible; //Used by navmesh
    [SerializeField] NPC connectedNPC = null; //Eventual connected NPC

    int layerMask; //mask for raycasting
    string[] layers = new string[] { "Default", "Player" }; //layers that the raycast can hit

    private void Start()
    {
        layerMask = LayerMask.GetMask(layers); //sets up layer mask
    }

    private void Update()
    {
        if (player != null && playerIsDetected)
        {
            player.GetComponent<DetectionLevelGUI>().AddGuard(this); //Adds guard to players list of guards who have been alert

            if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out rayHit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide) && rayHit.transform.tag.Equals("Player")) //casts a ray and checks if player has been hit
            {

                if (nextTime < Time.time && currentDetectionLevel < detectionLevel) //increases detection level
                {
                    currentDetectionLevel += detectionPerInterval;
                    nextTime = Time.time + interval;
                    playerIsVisible = true;
                }
            }
            else if (nextTime < Time.time && currentDetectionLevel > 0) //decreases detection level
            {
                currentDetectionLevel -= detectionPerInterval;
                nextTime = Time.time + interval;
                playerIsVisible = false;
            }
        }
        else
        {
            if (nextTime < Time.time && currentDetectionLevel > 0) //decreases detection level
            {
                currentDetectionLevel -= detectionPerInterval;
                nextTime = Time.time + interval;
                playerIsVisible = false;
            }
        }

        if (currentDetectionLevel >= detectionLevel) //if detection level has reached its maximum, set playerUncovered to true, otherwise false
        {
            if (!detectionActionTaken)
            {
                DoDetectionAction();
            }
            playerdUncovered = true;
        }
        else
        {
            playerdUncovered = false;
        }

    }

    bool detectionActionTaken = false;
    private void DoDetectionAction()
    {
        if (connectedNPC)
        {
            detectionActionTaken = true;
            connectedNPC.InteractWith();
        }
    }
}
