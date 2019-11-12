using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour //Dejan
{
    public float delay; //should be the same as FadeTransition effectDuration
    public void SwitchScene(string sceneName) //loads given scene
    {
        Time.timeScale = 1;
        StartCoroutine("DelayedSceneSwitch", sceneName);
    }

    IEnumerator DelayedSceneSwitch(string sceneName) //applies delay
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        // Insert the call for a save game method here, if we have one
        Application.Quit();
    }
}
