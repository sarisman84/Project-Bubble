using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionLevelGUI : MonoBehaviour
{
    [SerializeField] Image redBard;

    List<Guard> guards = new List<Guard>();

    private float highestDetectionLevel = 0;
    private Guard mostAlertGuard;

    private void Update()
    {
        foreach (Guard guard in guards)
        {
            if (guard.currentDetectionLevel > highestDetectionLevel)
            {
                mostAlertGuard = guard;
            }
        }
        if (mostAlertGuard != null)
        {
            highestDetectionLevel = mostAlertGuard.currentDetectionLevel;
        }

        redBard.fillAmount = highestDetectionLevel / 3;
    }

    public void AddGuard(Guard guard)
    {
        if (!guards.Contains(guard))
        {
            guards.Add(guard);
        }
    }
}
