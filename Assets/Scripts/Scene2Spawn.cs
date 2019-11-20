using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2Spawn : MonoBehaviour
{
    public Transform player = null;
    public Transform door = null;
    public Transform bed = null;

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
        if (Input.GetKey(KeyCode.F11))
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