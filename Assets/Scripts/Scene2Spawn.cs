using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Erik Neuhohofer
public class Scene2Spawn : MonoBehaviour
{
    public Transform player = null;
    public Transform door = null;
    public Transform bed = null;

    int first = 2;

    string firstTime = "First Time";

    private void Start()
    {
        if (PlayerPrefs.GetInt(firstTime) == 0)
        {
            SpawnPlayerAt(bed);
            PlayerPrefs.SetInt(firstTime, 1);
        }

        else
        {
            SpawnPlayerAt(door);
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.F11)) // To Delete PlayerPrefs Save Spawning Happens Correctly
        {
            PlayerPrefs.DeleteKey(firstTime);
            Debug.Log("Deleted PlayerPrefs");
        }
    }

    public void SpawnPlayerAt(Transform spawnPosition)
    {
        player.position = spawnPosition.position;
    }
}