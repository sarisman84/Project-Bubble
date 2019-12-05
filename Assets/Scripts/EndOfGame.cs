using UnityEngine;

public class EndOfGame : MonoBehaviour
{
    [SerializeField] SceneSwitch sceneSwitch = null;
    [SerializeField] int sceneToLoadIndex = 0;
    [SerializeField] bool playKnockOutAnimation = true;
    [SerializeField] Animation KnockOutAnimation = null;
    [SerializeField] FadeTransition fadeTransition = null;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance().USBPickedUp)
        {
            GameManager.Instance().USBPickedUp = false;
            GameManager.Instance().SetFPSInput(false);
            GameManager.Instance().SetMouseLook(false);
            if (playKnockOutAnimation)
            {
                KnockOutAnimation.Play();
                fadeTransition.effectDuration = 3;
                sceneSwitch.delay = 3;
            }
            sceneSwitch.SwitchScene(sceneToLoadIndex);
        }
    }
}
