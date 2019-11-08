using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//Simon Voss
//Displays messages sent to it
public class MessageUI : MonoBehaviour
{
    bool isOn = false;
    [SerializeField] Text messageText;
    [SerializeField] Animator anim;

    private void Start()
    {
        if (!messageText || !anim)
        {
            Debug.LogWarning("Message UI does not have the assigned components assigned. Plz fix");
        }
    }

    public void DisplayText(string input)
    {
        if (isOn)
        {
            return;
        }
        StartCoroutine(ShowText(input));
        StopCoroutine(DisableWithAnimator());
    }

    IEnumerator ShowText(string input)
    {
        isOn = true;
        yield return new WaitForSeconds(0.05f);
        messageText.text = input;
        anim.SetTrigger("Show");
    }

    public void DisablePanel()
    {
        if (!isOn)
        {
            return;
        }
        StartCoroutine(DisableWithAnimator());
        StopCoroutine(ShowText(""));
    }

    IEnumerator DisableWithAnimator()
    {
        isOn = false;
        yield return new WaitForSeconds(0.05f);
        anim.SetTrigger("Hide");
    }
}
