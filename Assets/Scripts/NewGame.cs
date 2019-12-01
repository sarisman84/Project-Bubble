﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGame : MonoBehaviour
{
    [SerializeField] PlayerCharacteristics playerStats = null;
    [SerializeField] ScriptableNpc[] npcs;

    private void Start()
    {
        playerStats.Reset();
        for (int i = 0; i < npcs.Length; i++)
        {
            npcs[i].Reset();
        }
        PlayerPrefs.DeleteAll();
        Debug.Log("Started new game and resetted stats");
    }
}
