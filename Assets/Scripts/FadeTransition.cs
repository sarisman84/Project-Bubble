using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    public float effectDuration;
    public bool ignoreTimeScale;
    public bool fadeInOnSceneLoad;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        if (fadeInOnSceneLoad)
        {
            image.CrossFadeAlpha(0, effectDuration, ignoreTimeScale);
        }
    }

    public void Fade(bool fadeIn)
    {
        if (fadeIn)
        {
            image.CrossFadeAlpha(0, effectDuration, ignoreTimeScale);
        }
        else
        {
            image.CrossFadeAlpha(1, effectDuration, ignoreTimeScale);
        }
    }
}
