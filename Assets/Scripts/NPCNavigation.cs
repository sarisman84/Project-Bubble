﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCNavigation : MonoBehaviour //Dejan, this script is used together with any nav mesh agent to create standard paths
{
    [SerializeField] bool isAGuard = false; //set true if the NPC is a guard, a guard script must be present on the same object
    [SerializeField] List<Destination> destinations = new List<Destination>(); //list of all destination positions the npc is to go through

    private NavMeshAgent agent; 
    private int destinationIndex = 0; //used to go through the destinations in correct order
    private bool hasWaited; //weather or not the npc has executed the given delay time
    private bool readyToGo; //weather or not the npc is allowed to proceed to the next position
    private bool destinationReset; //weather or not the npc destination was reset to the correct next one after stopping to detect the player
    private Guard guard; //a reference to the guard script

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //finds the nav mesh agent script
        if (isAGuard)
        {
            guard = GetComponent<Guard>(); //if marked a guard, finds the guard script
        }
        else
        {
            guard = null;
        }
    }

    private void Update()
    {
        if ((guard != null && !guard.playerIsVisible && !agent.pathPending) || (guard == null && !agent.pathPending)) //sets the next destination if the previous one is done, includes delay sequance for every destination
        {
            destinationReset = false;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    if (!hasWaited)
                    {
                        StartCoroutine("Delay", destinations[destinationIndex].delay);
                        hasWaited = true;
                    }
                    else
                    {
                        if (readyToGo)
                        {
                            agent.destination = destinations[destinationIndex].destination.position;
                            destinationIndex++;
                            hasWaited = false;
                            readyToGo = false;
                        }
                    }

                    if (destinationIndex > destinations.Count - 1)
                    {
                        destinationIndex = 0;
                    }
                }
            }
        }
        else //if a players is detected, stop the npc
        {
            agent.destination = gameObject.transform.position;

            if (!destinationReset && destinationIndex > 0)
            {
                destinationIndex = destinationIndex - 1;
                destinationReset = true;
            }
            else if (!destinationReset)
            {
                destinationIndex = destinations.Count - 1;
                destinationReset = true;
            }
        }
    }

    IEnumerator Delay(float delay) //delays next destination
    {
        yield return new WaitForSeconds(delay);
        readyToGo = true;
    }
}

[System.Serializable]
struct Destination
{
    public float delay; //delay before going to next destination
    public Transform destination; //next destination
}

