using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource doorSound = null;
    [SerializeField] AudioSource busSound = null;
    [SerializeField] AudioSource footsteps = null;

    private void Start()
    {
        StartCoroutine("DoorSound");
    }

    IEnumerator DoorSound()
    {
        yield return new WaitForSeconds(2);
        doorSound.PlayOneShot(doorSound.clip);
        yield return new WaitForSeconds(25);
        footsteps.enabled = false;
        busSound.PlayOneShot(busSound.clip);
    }
}
