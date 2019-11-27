using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] bool playOnStart = true;
    [SerializeField] FadeTransition fadeTransitionObject = null;
    [SerializeField] int sceneIndex = 0;
    [SerializeField] SceneSwitch sceneSwitch = null;
    [SerializeField] List<GameObject> cameras = new List<GameObject>();
    private Queue<GameObject> cameraQueue = new Queue<GameObject>();
    private Queue<GameObject> CameraQueue()
    {
        if (cameras.Count > 0)
        {
            foreach (GameObject camera in cameras)
            {
                cameraQueue.Enqueue(camera);
            }
        }
        return cameraQueue;
    }

    private void Start()
    {
        if (playOnStart && CameraQueue().Count > 0)
        {
            StartCoroutine("PlayCutscene");
        }
    }

    public void CallPlayCutscene()
    {
        StartCoroutine("PlayCutscene");
    }
    IEnumerator PlayCutscene()
    {
        if (cameraQueue.Count > 0)
        {
            fadeTransitionObject.Fade(true);
            cameraQueue.Peek().SetActive(true);
            yield return new WaitForSeconds(cameraQueue.Peek().GetComponent<Animation>().clip.length - fadeTransitionObject.effectDuration);
            if (cameraQueue.Count == 1)
            {
                sceneSwitch.SwitchScene(sceneIndex);
            }
            else
            {
                fadeTransitionObject.Fade(false);
            }
            yield return new WaitForSeconds(fadeTransitionObject.effectDuration);
            if (cameraQueue.Count == 1)
            {
                cameraQueue.Dequeue();
            }
            else
            {
                cameraQueue.Dequeue().SetActive(false);
            }
            StartCoroutine("PlayCutscene");
        }
    }
}
