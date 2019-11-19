using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Simon Voss
//Displays messages sent to it
public class MessageUI : MonoBehaviour
{
    bool isOn = false;
    [SerializeField] TextMeshProUGUI messageText = null;
    [SerializeField] Animator anim = null;

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
        anim.ResetTrigger("Show");
        anim.ResetTrigger("Hide");
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
        anim.ResetTrigger("Show");
        anim.ResetTrigger("Hide");
        anim.SetTrigger("Hide");
    }
}
