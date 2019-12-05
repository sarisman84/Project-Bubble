using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] int sceneIndex = 0;
    [SerializeField] SceneSwitch sceneSwitch = null;
    [SerializeField] FadeTransition fadeTransition = null;
    [SerializeField] AudioSource steps = null;

    public void FadeOut()
    {
        fadeTransition.Fade(false);
    }
    public void FadeIn()
    {
        fadeTransition.Fade(true);
    }
    public void SwitchScene()
    {
        sceneSwitch.SwitchScene(sceneIndex);
    }
    public void DesableStepsSound()
    {
        steps.enabled = false;
    }
}
