using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour //Dejan
{
    public float effectDuration; //should be the same as SceneSwitch delay
    public bool ignoreTimeScale; 
    public bool fadeInOnSceneLoad; //set true to fade on scene load without needing activation
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        if (fadeInOnSceneLoad)
        {
            image.CrossFadeAlpha(0, effectDuration, ignoreTimeScale); //fades in image
        }
    }

    public void Fade(bool fadeIn)
    {
        if (fadeIn)
        {
            image.CrossFadeAlpha(0, effectDuration, ignoreTimeScale); //fades in image
        }
        else
        {
            image.CrossFadeAlpha(1, effectDuration, ignoreTimeScale); //fades out image
        }
    }
}
