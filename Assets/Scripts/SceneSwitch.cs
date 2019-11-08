using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public float delay;
    public void SwitchScene(string sceneName) //loads given scene
    {
        StartCoroutine("DelayedSceneSwitch", sceneName);
    }

    IEnumerator DelayedSceneSwitch(string sceneName) //applies delay
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
