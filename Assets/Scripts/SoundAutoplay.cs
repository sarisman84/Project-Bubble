using System.Collections.Generic;
using UnityEngine;

public class SoundAutoplay : MonoBehaviour
{
    [SerializeField] List<AudioClip> sounds = new List<AudioClip>();
    [SerializeField] float interval = 0;
    [SerializeField] AudioSource audioSource = null;

    float nextTime = 0.7f;

    private void Update()
    {
        if (Time.time > nextTime)
        {
            audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Count - 1)]);
            nextTime = Time.time + interval;
        }
    }
}
