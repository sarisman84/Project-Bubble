using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour //Dejan, Erik, must be on the same object as FadeTransition.cs
{
    FadeTransition fadeTransition; //a referense to the FadeTransition.cs

    public float delay; //should be the same as FadeTransition effectDuration

    private void Start()
    {
        fadeTransition = GetComponent<FadeTransition>(); //finds FadeTrasition.cs
    }

    public void SwitchScene(string sceneName) //loads given scene
    {
        Time.timeScale = 1;
        StartCoroutine("DelayedSceneSwitch", sceneName);
    }

    IEnumerator DelayedSceneSwitch(string sceneName) //applies delay and transition
    {
        fadeTransition.Fade(false);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        // Insert the call for a save game method here, if we have one
        Application.Quit();
    }
}
