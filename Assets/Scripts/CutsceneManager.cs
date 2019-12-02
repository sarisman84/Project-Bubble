using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] int sceneIndex = 0;
    [SerializeField] SceneSwitch sceneSwitch = null;
    [SerializeField] FadeTransition fadeTransition = null;
    [SerializeField] GameObject camera = null;

    private void Start()
    {
        camera.GetComponent<Animation>().PlayQueued("Second Camera");
        StartCoroutine("PlayCutscene");
    }

    IEnumerator PlayCutscene()
    {
        fadeTransition.Fade(true);
        yield return new WaitForSeconds(camera.GetComponent<Animation>().GetClip("First Camera").length - fadeTransition.effectDuration);
        fadeTransition.Fade(false);
        yield return new WaitForSeconds(fadeTransition.effectDuration);
        fadeTransition.Fade(true);
        yield return new WaitForSeconds(camera.GetComponent<Animation>().GetClip("Second Camera").length - fadeTransition.effectDuration);
        fadeTransition.Fade(false);
        yield return new WaitForSeconds(4);
        sceneSwitch.SwitchScene(sceneIndex);
    }
}
