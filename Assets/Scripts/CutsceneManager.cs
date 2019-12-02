using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] bool playOnStart = true;
    [SerializeField] FadeTransition fadeTransition = null;
    [SerializeField] int sceneIndex = 0;
    [SerializeField] SceneSwitch sceneSwitch = null;
    [SerializeField] List<GameObject> cameras = new List<GameObject>();

    private void Start()
    {
        if (playOnStart)
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
        while (cameras.Count > 0)
        {
            fadeTransition.Fade(true);
            cameras[0].SetActive(true);
            yield return new WaitForSeconds(cameras[0].GetComponent<Animation>().clip.length - fadeTransition.effectDuration);
            if (cameras.Count == 1)
            {
                sceneSwitch.delay = 5;
                sceneSwitch.SwitchScene(sceneIndex);
            }
            else
            {
                fadeTransition.Fade(false);
            }
            yield return new WaitForSeconds(fadeTransition.effectDuration);
            if (cameras.Count == 1)
            {
                cameras.Remove(cameras[0]);
            }
            else
            {
                cameras[0].SetActive(false);
                cameras.Remove(cameras[0]);;
            }
        }
    }
}
