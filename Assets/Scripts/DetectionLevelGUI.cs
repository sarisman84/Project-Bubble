using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionLevelGUI : MonoBehaviour //Dejan, controls the GUI bar image that shows detection level
{
    [SerializeField] Image redBard = null; //a reference to the red image canvas element

    List<Guard> guards = new List<Guard>(); //a list of all guards that have detected the player

    private float highestDetectionLevel = 0; //the highest detection level among all guards
    private Guard mostAlertGuard; //the guard that has detected the player most

    private void Update()
    {
        foreach (Guard guard in guards) //updates the mostAlertGuard
        {
            if (guard.currentDetectionLevel > highestDetectionLevel)
            {
                mostAlertGuard = guard;
            }
        }
        if (mostAlertGuard != null) //updates highestDetectionLevel
        {
            highestDetectionLevel = mostAlertGuard.currentDetectionLevel;
        }

        redBard.fillAmount = highestDetectionLevel / 3;
    }

    public void AddGuard(Guard guard) //adds any guards detecting the player to the guards list
    {
        if (!guards.Contains(guard))
        {
            guards.Add(guard);
        }
    }
}
